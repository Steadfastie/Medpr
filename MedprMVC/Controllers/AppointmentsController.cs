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

namespace MedprMVC.Controllers;

public class AppointmentsController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public AppointmentsController(IAppointmentService appointmentService,
        IDoctorService doctorService,
        IUserService userService,
        IMapper mapper)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var dtos = await _appointmentService.GetAppointmentsByPageNumberAndPageSizeAsync(page, _pagesize);

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
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
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
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        var allDoctors = await _doctorService.GetAllDoctorsAsync();
        var allUsers = await _userService.GetAllUsersAsync();
        AppointmentModel model = new();
        model.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name");
        model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "FullName");
        return View(model);
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
                var dto = await _appointmentService.GetAppointmentsByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var allUsers = await _userService.GetAllUsersAsync();

                var editModel = _mapper.Map<AppointmentModel>(dto);

                editModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                editModel.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name", doctorSelected.Id.ToString());
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
    public async Task<IActionResult> Edit(AppointmentModel model)
    {
        try
        {
            if (model != null)
            {
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
            return BadRequest(ex.Message);
        }
    }
}
