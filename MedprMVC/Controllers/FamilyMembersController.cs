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

namespace MedprMVC.Controllers;

[Authorize]
public class FamilyMembersController : Controller
{
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IFamilyService _familyService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public FamilyMembersController(IFamilyMemberService familyMemberService,
        IFamilyService familyService,
        IUserService userService,
        IMapper mapper)
    {
        _familyMemberService = familyMemberService;
        _familyService = familyService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var dtos = await _familyMemberService.GetFamilyMembersByPageNumberAndPageSizeAsync(page, _pagesize);

            List<FamilyMemberModel> models = new();

            foreach (var dto in dtos)
            {
                var familySelected = await _familyService.GetFamiliesByIdAsync(dto.FamilyId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<FamilyMemberModel>(dto);

                model.Family = _mapper.Map<FamilyModel>(familySelected);
                model.User = _mapper.Map<UserModel>(userSelected);

                models.Add(model);
            }

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
            var dto = await _familyMemberService.GetFamilyMembersByIdAsync(id);

            if (dto != null)
            {
                var familySelected = await _familyService.GetFamiliesByIdAsync(dto.FamilyId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<FamilyMemberModel>(dto);

                model.Family = _mapper.Map<FamilyModel>(familySelected);
                model.User = _mapper.Map<UserModel>(userSelected);

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
    public async Task<IActionResult> CreateAsync()
    {
        var allFamilys = await _familyService.GetAllFamiliesAsync();
        var allUsers = await _userService.GetAllUsersAsync();
        FamilyMemberModel model = new();
        model.Families = new SelectList(_mapper.Map<List<FamilyModel>>(allFamilys), "Id", "Surname");
        model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "FullName");
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

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _familyMemberService.GetFamilyMembersByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var familySelected = await _familyService.GetFamiliesByIdAsync(dto.FamilyId);
                var allFamilys = await _familyService.GetAllFamiliesAsync();

                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var allUsers = await _userService.GetAllUsersAsync();

                var editModel = _mapper.Map<FamilyMemberModel>(dto);

                editModel.Family = _mapper.Map<FamilyModel>(familySelected);
                editModel.Families = new SelectList(_mapper.Map<List<FamilyModel>>(allFamilys), "Id", "Surname", familySelected.Id.ToString());
                editModel.User = _mapper.Map<UserModel>(userSelected);
                editModel.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "FullName", userSelected.Id.ToString());

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
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid MemberId, Guid UserId)
    {
        try
        {
            if (MemberId != Guid.Empty)
            {
                var dto = await _familyMemberService.GetFamilyMembersByIdAsync(MemberId);

                if (false)

                await _familyMemberService.DeleteFamilyMemberAsync(dto);

                return RedirectToAction("Index", "FamilyMembers");
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
}
