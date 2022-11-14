using Microsoft.AspNetCore.Mvc;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MedprMVC.Controllers;

[Authorize]
public class FamiliesController : Controller
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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
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
            var familiesModels = _mapper.Map<List<FamilyModel>>(dtos);

            if (familiesModels.Any())
            {
                foreach (var family in familiesModels)
                {
                    var membersDTO = await _familyMemberService.GetMembersRelevantToFamily(family.Id);
                    var membersModels = _mapper.Map<List<FamilyMemberModel>>(membersDTO);

                    foreach (var member in membersModels)
                    {
                        if (currentUserRole[0] == "Default" && member.UserId == currentUser.Id)
                        {
                            var isAdmin = member.IsAdmin;
                            ViewData[$"{family.Surname}"] = isAdmin;
                        }
                        if (currentUserRole[0] == "Admin")
                        {
                            ViewData[$"{family.Surname}"] = true;
                        }

                        var userDTO = await _userService.GetUserByIdAsync(member.UserId);
                        var userModel = _mapper.Map<UserModel>(userDTO);
                        member.User = userModel;

                        var FamilyDTO = await _familyService.GetFamiliesByIdAsync(member.FamilyId);
                        var familyModel = _mapper.Map<FamilyModel>(FamilyDTO);
                        member.Family = familyModel;
                    }

                    family.Members = SortMembers(membersModels, family);
                    ViewData[$"Creator of {family.Surname}"] = family.Creator;
                }

                ViewData["CurrentUser"] = currentUser.Id;
                
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
            return RedirectToAction("Error", "Home");
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
                var currentUser = await _userManager.GetUserAsync(User);

                model.Id = Guid.NewGuid();
                model.Creator = currentUser.Id;

                var familyDTO = _mapper.Map<FamilyDTO>(model);

                var familyMemberModel = new FamilyMemberModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = currentUser.Id,
                    FamilyId = model.Id,
                    IsAdmin = true
                };

                var familyMemberDTO = _mapper.Map<FamilyMemberDTO>(familyMemberModel);

                await _familyService.CreateFamilyAsync(familyDTO);
                await _familyMemberService.CreateFamilyMemberAsync(familyMemberDTO);

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
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _familyService.GetFamiliesByIdAsync(id);
                var currentUser = await _userManager.GetUserAsync(User);
                var currentUserRole = await _userManager.GetRolesAsync(currentUser);

                if (currentUserRole[0] == "Default" && dto.Creator != currentUser.Id || dto == null)
                {
                    return RedirectToAction("Denied", "Home");
                }

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
            return RedirectToAction("Error", "Home");
        }
    }

    private static List<FamilyMemberModel> SortMembers(List<FamilyMemberModel> membersModels, FamilyModel family)
    {
        var sorted = new List<FamilyMemberModel>();

        // Push creator to first position
        var creator = membersModels.Where(member => member.UserId == family.Creator).FirstOrDefault();
        sorted.Add(creator);
        membersModels.Remove(creator);

        // Then add all admins
        var admins = membersModels.Where(member => member.IsAdmin);
        sorted.AddRange(admins);
        membersModels.RemoveAll(member => admins.Contains(member));

        sorted.AddRange(membersModels);

        return sorted;
    }
}
