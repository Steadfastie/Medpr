using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MedprCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedprMVC.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class UsersController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public UsersController(IUserService userService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userService = userService;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var dtos = await _userService.GetUsersByPageNumberAndPageSizeAsync(page, _pagesize);

            var models = _mapper.Map<List<UserModel>>(dtos);

            if (models.Any())
            {
                return View(models);
            }
            else
            {
                return View(null);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _userService.GetUsersByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<UserModel>(dto);
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        var model = new UserModel
        {
            Roles = new SelectList(Enum
                    .GetValues(typeof(AppRole))
                    .Cast<AppRole>()
                    .Select(role => new
                    {
                        Value = ((int)role).ToString(),
                        Text = role.ToString()
                    })
                    .ToList(), "Value", "Text")
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserModel model)
    {
        try
        {
            if (model.Login.Any() && model.Password.Any())
            {
                var identityUser = new IdentityUser<Guid>(model.Login);
                var result = await _userManager.CreateAsync(identityUser, model.Password);
                model.Id = Guid.Parse(await _userManager.GetUserIdAsync(identityUser));

                if (result.Succeeded)
                {
                    AppRole selectedRole = model.Roles?.SelectedValue != null ? (AppRole)model.Roles.SelectedValue : AppRole.Default;

                    if (await EnsureRoleCreatedAsync(selectedRole.ToString()))
                    {
                        var role = await _roleManager.FindByNameAsync(selectedRole.ToString());
                        var roleResult = await _userManager.AddToRoleAsync(identityUser, role.Name);

                        if (roleResult.Succeeded)
                        {
                            var dto = _mapper.Map<UserDTO>(model);
                            dto.Id = Guid.Parse(await _userManager.GetUserIdAsync(identityUser));
                            await _userService.CreateUserAsync(dto);
                        }
                        // TODO: User should be removed from Identity DB
                    }
                    return RedirectToAction("Index", "Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        catch (Exception ex)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            await _userManager.DeleteAsync(user);
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _userService.GetUsersByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var editModel = _mapper.Map<UserModel>(dto);

                var user = await _userManager.FindByIdAsync(id.ToString());
                var role = await _userManager.GetRolesAsync(user);

                var listOfRoles = Enum
                    .GetValues(typeof(AppRole))
                    .Cast<AppRole>()
                    .ToList();

                editModel.Roles = new SelectList(listOfRoles, role[0]);

                return View(editModel);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserModel model)
    {
        try
        {
            if (model.Login != null)
            {
                var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());

                await UpdateIdentityDB(model, currentUser);

                await UpdateMainDB(model);

                return RedirectToAction("Index", "Users");
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _userService.GetUsersByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var deleteModel = _mapper.Map<UserModel>(dto);

                return View(deleteModel);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _userService.GetUsersByIdAsync(id);

                await _userService.DeleteUserAsync(dto);

                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.DeleteAsync(user);

                return RedirectToAction("Index", "Users");
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    private async Task<bool> EnsureRoleCreatedAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        bool check =
            role != null && await _roleManager.RoleExistsAsync(role.Name);

        if (!check)
        {
            var newRole = new IdentityRole<Guid>(roleName);
            await _roleManager.CreateAsync(newRole);
        }
        return true;
    }

    private async Task UpdateMainDB(UserModel model)
    {
        var dto = _mapper.Map<UserDTO>(model);

        var patchList = new List<PatchModel>();

        var sourceDto = await _userService.GetUsersByIdAsync(dto.Id);

        foreach (PropertyInfo property in typeof(UserDTO).GetProperties())
        {
            if (!Equals(property.GetValue(dto), property.GetValue(sourceDto)))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = property.Name,
                    PropertyValue = property.GetValue(dto)
                });
            }
        }

        await _userService.PatchUserAsync(dto.Id, patchList);
    }

    private async Task UpdateIdentityDB(UserModel model, IdentityUser<Guid> user)
    {
        if (model.Login != user.Email)
        {
            user.Email = model.Login;
            await _userManager.UpdateAsync(user);
        }

        var currentRole = await _userManager.GetRolesAsync(user);
        if (model.SelectedRole != null)
        {
            var selectedRole = ((AppRole)model.SelectedRole).ToString();
            if (selectedRole != currentRole[0])
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole[0]);
                await _userManager.AddToRoleAsync(user, selectedRole);
            }
        }
    }
}
