using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;
using MedprDB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MedprMVC.Controllers;

[Authorize]
public class FamilyMembersController : Controller
{
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IFamilyService _familyService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public FamilyMembersController(IFamilyMemberService familyMemberService,
        UserManager<IdentityUser<Guid>> userManager,
        IFamilyService familyService,
        IUserService userService,
        IMapper mapper)
    {
        _familyMemberService = familyMemberService;
        _familyService = familyService;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var allFamilys = await _familyService.GetAllFamiliesAsync();
        var allUsers = await _userService.GetAllUsersAsync();
        FamilyMemberModel model = new();
        model.Families = new SelectList(_mapper.Map<List<FamilyModel>>(allFamilys), "Id", "Surname");
        model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "Login");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FamilyMemberModel model)
    {
        try
        {
            // At the current moment FamilyMember Model has 4 additional fields to form other
            // actions's models. They aren't suppose to fill in, so ModelState.IsValid won't
            // be true. The other way around is used in Edit[post]
            if (ModelState.ErrorCount < 5)
            {
                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<FamilyMemberDTO>(model);

                await _familyMemberService.CreateFamilyMemberAsync(dto);

                return RedirectToAction("Index", "FamilyMembers");
            }

            else
            {
                return View(model);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FamilyMemberModel model)
    {
        try
        {
            if (model != null)
            {
                var dto = _mapper.Map<FamilyMemberDTO>(model);

                var sourceDto = await _familyMemberService.GetFamilyMembersByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(FamilyMemberDTO).GetProperties())
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
                }

                await _familyMemberService.PatchFamilyMemberAsync(model.Id, patchList);

                return RedirectToAction("Index", "Families");
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
    public async Task<IActionResult> Delete(Guid MemberId)
    {
        try
        {
            if (MemberId != Guid.Empty)
            {
                var memberDTO = await _familyMemberService.GetFamilyMembersByIdAsync(MemberId);
                var familyDTO = await _familyService.GetFamiliesByIdAsync(memberDTO.FamilyId);
                var currentUser = await _userManager.GetUserAsync(User);
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

                if (!await CheckRelevancy(familyDTO.Id) || !isAdmin)
                {
                    return RedirectToAction("Denied", "Home");
                }

                await _familyMemberService.DeleteFamilyMemberAsync(memberDTO);

                return RedirectToAction("Index", "Families");
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

    private async Task<bool> CheckRelevancy(Guid familyId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);
        if (currentUserRole[0] == "Default")
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
