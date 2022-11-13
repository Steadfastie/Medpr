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
public class PrescriptionsController : Controller
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;
    private readonly IDrugService _drugService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public PrescriptionsController(IPrescriptionService prescriptionService,
        IDoctorService doctorService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService,
        IDrugService drugService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager)
    {
        _prescriptionService = prescriptionService;
        _doctorService = doctorService;
        _mapper = mapper;
        _userService = userService;
        _drugService = drugService;
        _userManager = userManager;
        _familyMemberService = familyMemberService;
        _familyService = familyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<PrescriptionDTO> dtos = await GetRelevantPrescriptions();

            List<PrescriptionModel> models = new();

            foreach (var dto in dtos)
            {
                var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);

                var model = _mapper.Map<PrescriptionModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                model.User = _mapper.Map<UserModel>(userSelected);
                model.Drug = _mapper.Map<DrugModel>(drugSelected);

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

            var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

            if (dto != null)
            {
                var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);

                var model = _mapper.Map<PrescriptionModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                model.User = _mapper.Map<UserModel>(userSelected);
                model.Drug = _mapper.Map<DrugModel>(drugSelected);

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
            var allDoctors = await _doctorService.GetAllDoctorsAsync();
            var allDrugs = await _drugService.GetAllDrugsAsync();
            PrescriptionModel model = new()
            {
                Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name"),
                Drugs = new SelectList(_mapper.Map<List<DrugModel>>(allDrugs), "Id", "Name")
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
    public async Task<IActionResult> Create(PrescriptionModel model)
    {
        try
        {
            // At the current moment Prescription Model has 6 additional fields to form other
            // actions's models. They aren't suppose to fill in, so ModelState.IsValid won't
            // be true. The other way around is used in Edit[post]
            if (ModelState.ErrorCount < 7 && CheckDate(model))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var ids = await GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(model.UserId))
                {
                    RedirectToAction("Denied", "Home");
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<PrescriptionDTO>(model);

                await _prescriptionService.CreatePrescriptionAsync(dto);

                return RedirectToAction("Index", "Prescriptions");
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

                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);
                var allDrugs = await _drugService.GetAllDrugsAsync();

                var editModel = _mapper.Map<PrescriptionModel>(dto);

                editModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                editModel.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name", doctorSelected.Id.ToString());
                
                editModel.Drug = _mapper.Map<DrugModel>(drugSelected);
                editModel.Drugs = new SelectList(_mapper.Map<List<DrugModel>>(allDrugs), "Id", "Name", drugSelected.Id.ToString());

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
    public async Task<IActionResult> Edit(PrescriptionModel model)
    {
        try
        {
            if (model != null)
            {
                if (!await CheckRelevancy(model.Id))
                {
                    return RedirectToAction("Denied", "Home");
                }

                var dto = _mapper.Map<PrescriptionDTO>(model);

                var sourceDto = await _prescriptionService.GetPrescriptionsByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(PrescriptionDTO).GetProperties())
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

                await _prescriptionService.PatchPrescriptionAsync(model.Id, patchList);

                return RedirectToAction("Index", "Prescriptions");
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

                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);

                var deleteModel = _mapper.Map<PrescriptionModel>(dto);

                deleteModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                deleteModel.User = _mapper.Map<UserModel>(userSelected);
                deleteModel.Drug = _mapper.Map<DrugModel>(drugSelected);

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
                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

                await _prescriptionService.DeletePrescriptionAsync(dto);

                return RedirectToAction("Index", "Prescriptions");
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
    public IActionResult CheckDate(string startDate, string endDate)
    {
        var start = DateTime.Parse(startDate);
        var end = DateTime.Parse(endDate);
        if (start < end)
        {
            return Ok(true);
        }
        return Ok(false);
    }

    private static bool CheckDate(PrescriptionModel model)
    {
        if (model.StartDate > model.EndDate)
        {
            return false;
        }
        return true;
    }

    private async Task<List<PrescriptionDTO>> GetRelevantPrescriptions()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<PrescriptionDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = new();
            users.AddRange(await GetWardedByUserPeople(currentUser.Id));

            foreach (var user in users)
            {
                var userPrescriptions = await _prescriptionService.GetPrescriptionsRelevantToUser(user);
                dtos.AddRange(userPrescriptions);
            }
            return dtos;
        }
        else
        {
            return await _prescriptionService.GetAllPrescriptions();
        }

    }

    private async Task<bool> CheckRelevancy(Guid prescriptionId)
    {
        var dtos = await GetRelevantPrescriptions();

        var ids = dtos.Select(dto => dto.Id).ToList();

        if (!ids.Contains(prescriptionId))
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
