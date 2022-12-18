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
using MedprMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MedprDB.Entities;
using AutoMapper.Execution;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for members of families
/// </summary>
[Route("members")]
[ApiController]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IUserService _userService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IMapper _mapper;
    public MembersController(IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager)
    {
        _familyService = familyService;
        _familyMemberService = familyMemberService;
        _mapper = mapper;
        _userManager = userManager;
        _userService = userService;
    }

    /// <summary>
    /// Attach user to family
    /// </summary>
    /// <param name="model">Model with familyMember parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] FamilyMemberModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                if (currentUserRole[0] != "Admin" && model.UserId != currentUser.Id)
                {
                    return Forbid();
                }

                model.Id = Guid.NewGuid();
                var dto = _mapper.Map<FamilyMemberDTO>(model);

                await _familyMemberService.CreateFamilyMemberAsync(dto);

                var responseModel = _mapper.Map<FamilyMemberModelResponse>(dto);

                var creatorDTO = await _userService.GetUserByIdAsync(dto.UserId);
                var creatorModel = _mapper.Map<UserModelResponse>(creatorDTO);

                responseModel.User = creatorModel.GenerateLinks("users");

                responseModel.FamilyId = model.FamilyId;

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("members"));
            }

            else
            {
                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not attach user to family",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Make member family
    /// </summary>
    /// <param name="model">Model of member</param>
    /// <returns></returns>
    [HttpPatch]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MakeAdmin([FromBody] FamilyMemberModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                var sourceMember = await _familyMemberService.GetFamilyMemberByIdAsync(model.Id);
                var members = await _familyMemberService.GetMembersRelevantToFamily(sourceMember.FamilyId);
                var isCurrentUserAdminForThisFamily = members
                    .Where(member => member.UserId == currentUser.Id)
                    .Select(member => member.IsAdmin)
                    .FirstOrDefault();

                if (currentUserRole[0] != "Admin" && !isCurrentUserAdminForThisFamily)
                {
                    return Forbid();
                }

                var dto = _mapper.Map<FamilyMemberDTO>(model);

                var patchList = new List<PatchModel>()
                {
                    new PatchModel()
                    {
                        PropertyName = "IsAdmin",
                        PropertyValue = dto.IsAdmin,
                    },
                };

                await _familyMemberService.PatchFamilyMemberAsync(dto.Id, patchList);

                var responseModel = _mapper.Map<FamilyMemberModelResponse>(dto);

                var memberDTO = await _userService.GetUserByIdAsync(dto.UserId);
                var memberModel = _mapper.Map<UserModelResponse>(memberDTO);

                responseModel.User = memberModel.GenerateLinks("users");

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("members"));
            }

            else
            {
                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not attach user to family",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Detach member from family
    /// </summary>
    /// <param name="id">Family's id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var memberDTO = await _familyMemberService.GetFamilyMemberByIdAsync(id);
                var familyDTO = await _familyService.GetFamilyByIdAsync(memberDTO.FamilyId);

                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                bool isAdmin;
                if (currentUserRole[0] == "Admin")
                {
                    isAdmin = true;
                }
                else
                {
                    isAdmin = await _familyMemberService.GetRoleByFamilyIdAndUserId(memberDTO.FamilyId, currentUser.Id); ;
                }

                if (!isAdmin && !await CheckRelevancy(familyDTO.Id, currentUser, currentUserRole[0]))
                {
                    return Forbid();
                }

                await _familyMemberService.DeleteFamilyMemberAsync(memberDTO);

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
                Message = "Could not delete familyMember from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    private async Task<bool> CheckRelevancy(Guid familyId, IdentityUser<Guid> currentUser, string currentUserRole)
    {
        if (currentUserRole == "Default")
        {
            var dtos = await _familyService.GetFamiliesRelevantToUser(currentUser.Id);

            var ids = dtos.Select(dto => dto.Id).ToList();

            if (!ids.Contains(familyId))
            {
                return false;
            }
        }
        return true;
    }
}
