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
    public async Task<IActionResult> Create([FromForm] FamilyMemberModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                var dto = _mapper.Map<FamilyMemberDTO>(model);

                await _familyMemberService.CreateFamilyMemberAsync(dto);

                var responseModel = _mapper.Map<FamilyMemberModelResponse>(dto);

                var creatorDTO = await _userService.GetUserByIdAsync(dto.UserId);
                var creatorModel = _mapper.Map<UserModelResponse>(creatorDTO);

                responseModel.User = creatorModel.GenerateLinks("users");

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
            return RedirectToAction("Error", "Home", errorModel);
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

                if (!await CheckRelevancy(familyDTO.Id, currentUser, currentUserRole[0]) || !isAdmin)
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
            return RedirectToAction("Error", "Home", errorModel);
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
