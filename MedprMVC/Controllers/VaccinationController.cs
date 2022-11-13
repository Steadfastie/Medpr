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
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedprMVC.Controllers;

[Authorize]
public class VaccinationsController : Controller
{
    private readonly IVaccinationService _vaccinationService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IVaccineService _vaccineService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public VaccinationsController(IVaccinationService vaccinationService,
        IVaccineService vaccineService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService, 
        IMapper mapper, 
        UserManager<IdentityUser<Guid>> userManager)
    {
        _vaccinationService = vaccinationService;
        _vaccineService = vaccineService;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
        _familyMemberService = familyMemberService;
        _familyService = familyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<VaccinationDTO> dtos = await GetRelevantVaccinations();

            List<VaccinationModel> models = new();

            foreach (var dto in dtos)
            {
                var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<VaccinationModel>(dto);

                model.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
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
            return RedirectToAction("Error", "Home");
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

            var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

            if (dto != null)
            {
                var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<VaccinationModel>(dto);

                model.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
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
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            var allVaccines = await _vaccineService.GetAllVaccinesAsync();
            VaccinationModel model = new()
            {
                Vaccines = new SelectList(_mapper.Map<List<VaccineModel>>(allVaccines), "Id", "Name")
            };

            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRole[0] == "Default")
            {
                var ids = await GetWardedByUserPeople(currentUser.Id);
                if (ids.Count < 2)
                {
                    model.UserId = currentUser.Id;
                }
                else
                {
                    List<UserDTO> userList = new();
                    foreach (var id in ids)
                    {
                        userList.Add(await _userService.GetUsersByIdAsync(id));
                    }
                    model.Users = new SelectList(_mapper.Map<List<UserModel>>(userList), "Id", "Login");
                }
            }
            else
            {
                var allUsers = await _userService.GetAllUsersAsync();
                model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "Login");
            }

            return View(model);
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccinationModel model)
    {
        try
        {
            // At the current moment Vaccination Model has 4 additional fields to form other
            // actions's models. They aren't suppose to fill in, so ModelState.IsValid won't
            // be true. The other way around is used in Edit[post]
            if (ModelState.ErrorCount < 5)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var ids = await GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(model.UserId))
                {
                    RedirectToAction("Denied", "Home");
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<VaccinationDTO>(model);

                await _vaccinationService.CreateVaccinationAsync(dto);

                return RedirectToAction("Index", "Vaccinations");
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

                var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
                var allVaccines = await _vaccineService.GetAllVaccinesAsync();

                var editModel = _mapper.Map<VaccinationModel>(dto);

                editModel.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                editModel.Vaccines = new SelectList(_mapper.Map<List<VaccineModel>>(allVaccines), "Id", "Name", vaccineSelected.Id.ToString());

                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                editModel.User = _mapper.Map<UserModel>(userSelected);

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
    public async Task<IActionResult> Edit(VaccinationModel model)
    {
        try
        {
            if (model != null)
            {
                if (!await CheckRelevancy(model.Id))
                {
                    return RedirectToAction("Denied", "Home");
                }

                var dto = _mapper.Map<VaccinationDTO>(model);

                var sourceDto = await _vaccinationService.GetVaccinationsByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(VaccinationDTO).GetProperties())
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

                await _vaccinationService.PatchVaccinationAsync(model.Id, patchList);

                return RedirectToAction("Index", "Vaccinations");
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

                var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var deleteModel = _mapper.Map<VaccinationModel>(dto);

                deleteModel.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                deleteModel.User = _mapper.Map<UserModel>(userSelected);

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
                var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                await _vaccinationService.DeleteVaccinationAsync(dto);

                return RedirectToAction("Index", "Vaccinations");
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

    private async Task<List<VaccinationDTO>> GetRelevantVaccinations()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<VaccinationDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = new();
            users.AddRange(await GetWardedByUserPeople(currentUser.Id));

            foreach (var user in users)
            {
                var userVaccinations = await _vaccinationService.GetVaccinationsRelevantToUser(user);
                dtos.AddRange(userVaccinations);
            }
            return dtos;
        }
        else
        {
            return await _vaccinationService.GetAllVaccinations();
        }
        
    }

    private async Task<bool> CheckRelevancy(Guid vaccinationId)
    {
        var dtos = await GetRelevantVaccinations();

        var ids = dtos.Select(dto => dto.Id).ToList();

        if (!ids.Contains(vaccinationId))
        {
            return false;
        }

        return true;
    }

    private async Task<List<Guid>> GetWardedByUserPeople(Guid userId)
    {
        var families = await _familyService.GetFamiliesRelevantToUser(userId);
        HashSet<Guid> usersInAllFamilies = new()
        {
            userId
        };

        foreach (var family in families)
        {
            var membersDTO = await _familyMemberService.GetMembersRelevantToFamily(family.Id);
            var isCurrentUserAdmin = membersDTO
                .Where(member => member.UserId == userId)
                .ToList()[0]
                .IsAdmin;
            if (isCurrentUserAdmin)
            {
                var wardedPeople = membersDTO.Select(member => member.UserId).Where(member => member != userId);
                foreach (var person in wardedPeople)
                {
                    usersInAllFamilies.Add(person);
                }
            }
        }

        return usersInAllFamilies.ToList();
    }
}
