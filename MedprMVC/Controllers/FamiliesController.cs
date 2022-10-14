using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;
using MedprBusiness.ServiceImplementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MedprDB.Entities;

namespace MedprMVC.Controllers;

[Authorize]
public class FamiliesController : Controller
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IUserService _userService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
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

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var dtos = await _familyService.GetFamiliesRelevantToUser(currentUser.Id);
            var familiesModels = _mapper.Map<List<FamilyModel>>(dtos);

            if (familiesModels.Any())
            {
                foreach (var family in familiesModels)
                {
                    var membersDTO = await _familyMemberService.GetMembersRelevantToFamily(family.Id);
                    var membersModels = _mapper.Map<List<FamilyMemberModel>>(membersDTO);

                    foreach (var member in membersModels)
                    {
                        var userDTO = await _userService.GetUsersByIdAsync(member.UserId);
                        var userModel = _mapper.Map<UserModel>(userDTO);
                        member.User = userModel;

                        var FamilyDTO = await _userService.GetUsersByIdAsync(member.FamilyId);
                        var familyModel = _mapper.Map<FamilyModel>(FamilyDTO);
                        member.Family = familyModel;
                    }

                    family.Members = membersModels;
                }

                return View(familiesModels);
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
            if (!await CheckRelevancy(id))
            {
                return RedirectToAction("Denied", "Home");
            }

            var dto = await _familyService.GetFamiliesByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<FamilyModel>(dto);

                var membersDTO = await _familyMemberService.GetMembersRelevantToFamily(model.Id);
                var membersModels = _mapper.Map<List<FamilyMemberModel>>(membersDTO);

                foreach (var member in membersModels)
                {
                    var userDTO = await _userService.GetUsersByIdAsync(member.UserId);
                    var userModel = _mapper.Map<UserModel>(userDTO);
                    member.User = userModel;
                }

                model.Members = membersModels;
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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FamilyModel model)
    {
        try
        {
            if (model.Surname != null)
            {
                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<FamilyDTO>(model);

                var currentUser = await _userManager.GetUserAsync(User);

                var familyTie = new FamilyMemberModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = currentUser.Id,
                    FamilyId = model.Id,
                    IsAdmin = true
                };

                var familyTieDTO = _mapper.Map<FamilyMemberDTO>(familyTie);

                await _familyService.CreateFamilyAsync(dto);
                await _familyMemberService.CreateFamilyMemberAsync(familyTieDTO);

                return RedirectToAction("Index", "Families");
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

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                if (!await CheckRelevancy(id))
                {
                    return RedirectToAction("Denied", "Home");
                }

                var dto = await _familyService.GetFamiliesByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var editModel = _mapper.Map<FamilyModel>(dto);

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
    public async Task<IActionResult> Edit(FamilyModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                foreach (var member in model.Members)
                {
                    RedirectToAction("Edit", "FamilyMembers", member);
                }

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

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                if (!await CheckRelevancy(id))
                {
                    return RedirectToAction("Denied", "Home");
                }

                var dto = await _familyService.GetFamiliesByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var deleteModel = _mapper.Map<FamilyModel>(dto);

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
                var dto = await _familyService.GetFamiliesByIdAsync(id);

                await _familyService.DeleteFamilyAsync(dto);

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
