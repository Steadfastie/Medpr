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
using MedprBusiness.ServiceImplementations;

namespace MedprMVC.Controllers;

[Authorize]
public class AppointmentsController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public AppointmentsController(IAppointmentService appointmentService,
        IDoctorService doctorService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
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
            List<AppointmentDTO> dtos = await GetRelevantAppointments();

            List<AppointmentModel> models = new();

            foreach (var dto in dtos)
            {
                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<AppointmentModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
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

            var dto = await _appointmentService.GetAppointmentsByIdAsync(id);

            if (dto != null)
            {
                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var model = _mapper.Map<AppointmentModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
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
            var allDoctors = await _doctorService.GetAllDoctorsAsync();
            AppointmentModel model = new()
            {
                Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name")
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
    public async Task<IActionResult> Create(AppointmentModel model)
    {
        try
        {
            // At the current moment Appointment Model has 4 additional fields to form other
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

                var dto = _mapper.Map<AppointmentDTO>(model);

                await _appointmentService.CreateAppointmentAsync(dto);

                return RedirectToAction("Index", "Appointments");
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

                var dto = await _appointmentService.GetAppointmentsByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                var editModel = _mapper.Map<AppointmentModel>(dto);
                editModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                editModel.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name", doctorSelected.Id.ToString());

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
    public async Task<IActionResult> Edit(AppointmentModel model)
    {
        try
        {
            if (model != null)
            {
                if (!await CheckRelevancy(model.Id))
                {
                    return RedirectToAction("Denied", "Home");
                }

                var dto = _mapper.Map<AppointmentDTO>(model);

                var sourceDto = await _appointmentService.GetAppointmentsByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(AppointmentDTO).GetProperties())
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

                await _appointmentService.PatchAppointmentAsync(model.Id, patchList);

                return RedirectToAction("Index", "Appointments");
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

                var dto = await _appointmentService.GetAppointmentsByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                var deleteModel = _mapper.Map<AppointmentModel>(dto);

                deleteModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
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
                var dto = await _appointmentService.GetAppointmentsByIdAsync(id);

                await _appointmentService.DeleteAppointmentAsync(dto);

                return RedirectToAction("Index", "Appointments");
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

    private async Task<List<AppointmentDTO>> GetRelevantAppointments()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<AppointmentDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = new();
            users.AddRange(await GetWardedByUserPeople(currentUser.Id));

            foreach (var user in users)
            {
                var userAppointments = await _appointmentService.GetAppointmentsRelevantToUser(user);
                dtos.AddRange(userAppointments);
            }
            return dtos;
        }
        else
        {
            return await _appointmentService.GetAllAppointments();
        }

    }

    private async Task<bool> CheckRelevancy(Guid appointmentId)
    {
        var dtos = await GetRelevantAppointments();

        var ids = dtos.Select(dto => dto.Id).ToList();

        if (!ids.Contains(appointmentId))
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
