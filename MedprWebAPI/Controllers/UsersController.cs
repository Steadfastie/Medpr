using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using MedprModels.Responses;
using MedprModels;
using MedprModels.Requests;
using MedprModels.Links;
using Microsoft.AspNetCore.Identity;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for users
/// </summary>
[Route("users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
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

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(List<UserModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _userService.GetAllUsersAsync();

            var models = _mapper.Map<List<UserModelResponse>>(dtos);

            if (models.Any())
            {
                return Ok(models.Select(model => model.GenerateLinks("users")));
            }
            else
            {
                return Ok(null);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load users",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            if (!await CheckAccess(id))
            {
                return Forbid();
            }

            var dto = await _userService.GetUserByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<UserModelResponse>(dto);
                var requestUser = await _userManager.FindByNameAsync(model.Login);
                var requestUserRole = await _userManager.GetRolesAsync(requestUser);
                model.Role = requestUserRole[0];
                model.GenerateLinks("users");

                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load user",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Edit some data about user. Forbids user's name change. Returns SC304 if there is nothing to patch.
    /// </summary>
    /// <param name="model">User parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UserModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromForm] UserModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (!await CheckAccess(model.Id))
                {
                    return Forbid();
                }

                var dto = _mapper.Map<UserDTO>(model);

                var sourceDto = await _userService.GetUserByIdAsync(dto.Id);
                var sourceUser = await _userManager.FindByNameAsync(sourceDto.Login);
                var sourceUserRole = await _userManager.GetRolesAsync(sourceUser);

                // Update user info in main database
                var patchList = new List<PatchModel>();
                foreach (PropertyInfo property in typeof(UserDTO).GetProperties())
                {
                    if (!property.GetValue(dto).Equals(property.GetValue(sourceDto)))
                    {
                        patchList.Add(new PatchModel()
                        {
                            PropertyName = property.Name,
                            PropertyValue = property.GetValue(dto)
                        });
                    }
                }

                if (patchList.Any())
                {
                    await _userService.PatchUserAsync(model.Id, patchList);                    
                }

                // Update user info in identity database
                if (sourceUser.UserName != dto.Login)
                {
                    sourceUser.UserName = dto.Login;
                    sourceUser.NormalizedUserName = dto.Login.ToUpperInvariant();
                    sourceUser.Email = dto.Login;
                    sourceUser.NormalizedEmail = dto.Login.ToUpperInvariant();
                }

                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                if (sourceUserRole[0] != model.Role.ToString() && currentUserRole[0] == "Admin")
                {
                    await _userManager.RemoveFromRoleAsync(sourceUser, sourceUserRole[0]);
                    await _userManager.AddToRoleAsync(sourceUser, model.Role.ToString());
                }
                if (model.Password != model.NewPassword)
                {
                    await _userManager.ChangePasswordAsync(sourceUser, model.Password, model.NewPassword);
                }

                // Return updated user
                var updatedUser = await _userService.GetUserByIdAsync(model.Id);

                var responseModel = _mapper.Map<UserModelResponse>(updatedUser);

                var responseUser = await _userManager.FindByNameAsync(model.Login);
                var responseUserRole = await _userManager.GetRolesAsync(responseUser);
                responseModel.Role = responseUserRole[0];

                return Ok(responseModel.GenerateLinks("users"));
            }
            else
            {
                return Ok();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not update user info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Remove user from app
    /// </summary>
    /// <param name="id">User's id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                if (!await CheckAccess(id))
                {
                    return Forbid();
                }

                var dto = await _userService.GetUserByIdAsync(id);
                var identityUser = await _userManager.FindByNameAsync(dto.Login);

                if (dto == null)
                {
                    return BadRequest();
                }

                await _userService.DeleteUserAsync(dto);
                await _userManager.DeleteAsync(identityUser);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not delete user from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Returns true if current user has access to user's CRUD
    /// </summary>
    /// <param name="userId">Id of user to change</param>
    /// <returns></returns>
    private async Task<bool> CheckAccess(Guid userId)
    {
        var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
        var currentUser = await _userManager.FindByNameAsync(userName);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        if (currentUserRole[0] == "Admin" || currentUser.Id == userId)
        {
            return true;
        }
        return false;
    } 
}
