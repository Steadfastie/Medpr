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

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for families
/// </summary>
[Route("families")]
[ApiController]
[Authorize]
public class FamiliesController : ControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IUserService _userService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IMapper _mapper;
    public FamiliesController(IFamilyService familyService,
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
    /// Get list of families with creatorId and added to each family users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<FamilyModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
            var currentUser = await _userManager.FindByNameAsync(userName);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            List<FamilyDTO> dtos;
            if (currentUserRole[0] == "Default")
            {
                dtos = await _familyService.GetFamiliesRelevantToUser(currentUser.Id);
            }
            else
            {
                dtos = await _familyService.GetAllFamiliesAsync();
            }
            var familiesModels = _mapper.Map<List<FamilyModelResponse>>(dtos);

            if (familiesModels.Any())
            {
                foreach (var family in familiesModels)
                {
                    await GetMembersForFamily(family);
                    family.GenerateLinks("families");
                }

                return Ok(familiesModels);
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
                Message = "Could not load families",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "App", errorModel);
        }
    }

    /// <summary>
    /// Create new family for the app. Forbids creation of family with existing in app name
    /// </summary>
    /// <param name="model">Model with family parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FamilyModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] FamilyModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);

                model.Id = Guid.NewGuid();

                var familyDTO = _mapper.Map<FamilyDTO>(model);

                familyDTO.Creator = currentUser.Id;

                var familyMember = new FamilyMemberDTO()
                {
                    Id = Guid.NewGuid(),
                    IsAdmin = true,
                    UserId = currentUser.Id,
                    FamilyId = model.Id
                };

                await _familyService.CreateFamilyAsync(familyDTO);
                await _familyMemberService.CreateFamilyMemberAsync(familyMember);

                var responseModel = _mapper.Map<FamilyModelResponse>(familyDTO);
                await GetMembersForFamily(responseModel);

                return CreatedAtAction(nameof(Create), new { id = familyDTO.Id }, responseModel.GenerateLinks("families"));
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
                Message = "Could not create family",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "App", errorModel);
        }
    }

    /// <summary>
    /// Remove family from app
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
                var dto = await _familyService.GetFamilyByIdAsync(id);
                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                if (currentUserRole[0] == "Default" && dto.Creator != currentUser.Id || dto == null)
                {
                    return Forbid();
                }

                await _familyService.DeleteFamilyAsync(dto);

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
                Message = "Could not delete family from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "App", errorModel);
        }
    }

    private async Task GetMembersForFamily(FamilyModelResponse family)
    {
        var memberDTOs = await _familyMemberService.GetMembersRelevantToFamily(family.Id);
        List<FamilyMemberModelResponse> memberModels = new();

        foreach (var memberDTO in memberDTOs)
        {
            var userDTO = await _userService.GetUserByIdAsync(memberDTO.UserId);
            var userModel = _mapper.Map<UserModelResponse>(userDTO);
            userModel.GenerateLinks("users");

            var memberModel = _mapper.Map<FamilyMemberModelResponse>(memberDTO);
            memberModel.User = userModel;
            memberModel.GenerateLinks("members");

            memberModels.Add(memberModel);
        }

        family.Members = SortMembers(memberModels, family);
    } 
    private List<FamilyMemberModelResponse> SortMembers(
        List<FamilyMemberModelResponse> membersModels,
        FamilyModelResponse family)
    {
        var sorted = new List<FamilyMemberModelResponse>();

        // Push creator to first position
        var creator = membersModels.FirstOrDefault(member => member.User.Id == family.Creator);
        sorted.Add(creator);
        membersModels.Remove(creator);

        // Then add all admins
        var admins = membersModels
            .Where(member => member.IsAdmin)
            .OrderBy(member => member.User.FullName);
        sorted.AddRange(admins);
        membersModels.RemoveAll(member => admins.Contains(member));

        sorted.AddRange(membersModels.OrderBy(member => member.User.FullName));

        return sorted;
    }
}
