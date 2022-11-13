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
using System;

namespace MedprMVC.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class UsersController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserService _userService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IMapper _mapper;
    public UsersController(IUserService userService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userService = userService;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _familyService = familyService;
        _familyMemberService = familyMemberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var dtos = await _userService.GetAllUsers();

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
            return RedirectToAction("Error", "Home");
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
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        var listOfRoles = Enum
                    .GetValues(typeof(AppRole))
                    .Cast<AppRole>()
                    .Select(role => new SelectListItem
                    {
                        Text = role.ToString(),
                        Value = ((int)role).ToString()
                    })
                    .ToList();

        var model = new UserModel
        {
            Roles = new SelectList(listOfRoles, "Value", "Text")
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
                    var selectedRole = model.SelectedRole != null ? ((AppRole)model.SelectedRole).ToString() : "Default";

                    if (await EnsureRoleCreatedAsync(selectedRole))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(identityUser, selectedRole);

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

            var dto = _mapper.Map<UserDTO>(model);
            await _userService.DeleteUserAsync(dto);

            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return RedirectToAction("Error", "Home");
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
                    .Select(role => new SelectListItem
                        {
                            Text = role.ToString(),
                            Value = ((int)role).ToString()
                        })
                    .ToList();

                editModel.Roles = new SelectList(listOfRoles, "Value", "Text", role[0]);

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
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserModel model)
    {
        var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());
        var currentDTO = await _userService.GetUsersByIdAsync(model.Id);
        try
        {
            if (model.Login != null)
            {
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
            var savedModel = _mapper.Map<UserModel>(currentDTO);

            await UpdateIdentityDB(savedModel, currentUser);

            await UpdateMainDB(savedModel);

            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return RedirectToAction("Error", "Home");
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
            return RedirectToAction("Error", "Home");
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

                await _familyService.DeleteAllCreatedFamilies(user.Id);
                await _familyMemberService.DeleteMemberFromDBAsync(user.Id);
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
            return RedirectToAction("Error", "Home");
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
