using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using MedprModels.Responses;
using MedprModels.Requests;
using MedprModels.Links;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;
using MedprWebAPI.Utils;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for appointments
/// </summary>
[Route("appointments")]
[ApiController]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private WardedPeople WardedPeople => new(_familyService, _familyMemberService);
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

    /// <summary>
    /// Get all appointments
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<AppointmentModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<AppointmentDTO> dtos = await GetRelevantAppointments();

            List<AppointmentModelResponse> models = new();

            foreach (var dto in dtos)
            {
                var responseModel = await FillResponseModel(dto);

                models.Add(responseModel.GenerateLinks("appointments"));
            }

            if (models.Any())
            {
                return Ok(models);
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
                Message = "Could not load appointments",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the appointment</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _appointmentService.GetAppointmentByIdAsync(id);

            var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
            var currentUser = await _userManager.FindByNameAsync(userName);

            var ids = await WardedPeople.GetWardedByUserPeople(currentUser.Id);
            if (!ids.Contains(dto.UserId))
            {
                return Forbid();
            }

            if (dto != null)
            {
                var responseModel = await FillResponseModel(dto);

                return Ok(responseModel.GenerateLinks("appontments"));
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load appointment",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Create new appointment for the app
    /// </summary>
    /// <param name="model">Model with appointment parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] AppointmentModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);

                var ids = await WardedPeople.GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(model.UserId))
                {
                    return Forbid();
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<AppointmentDTO>(model);

                await _appointmentService.CreateAppointmentAsync(dto);

                var responseModel = await FillResponseModel(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("appointments"));
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
                Message = "Could not create appointment",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Edit some data about appointment
    /// </summary>
    /// <param name="model">Appointment parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromForm] AppointmentModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<AppointmentDTO>(model);

                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);

                var ids = await WardedPeople.GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(dto.UserId))
                {
                    return Forbid();
                }

                var sourceDto = await _appointmentService.GetAppointmentByIdAsync(model.Id);

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

                var responseModel = await FillResponseModel(dto);

                return Ok(responseModel.GenerateLinks("appointments"));
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
                Message = "Could not update appointment info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Remove appointment from app
    /// </summary>
    /// <param name="id">Appointment's id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id != Guid.Empty)
            {
                var dto = await _appointmentService.GetAppointmentByIdAsync(id);

                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);

                var ids = await WardedPeople.GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(dto.UserId))
                {
                    return Forbid();
                }

                if (dto == null)
                {
                    return BadRequest();
                }

                await _appointmentService.DeleteAppointmentAsync(dto);

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
                Message = "Could not delete appointment from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    private async Task<List<AppointmentDTO>> GetRelevantAppointments()
    {
        var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
        var currentUser = await _userManager.FindByNameAsync(userName);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<AppointmentDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = await WardedPeople.GetWardedByUserPeople(currentUser.Id);

            foreach (Guid userId in users)
            {
                var userAppointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
                dtos.AddRange(userAppointments);
            }
            return dtos;
        }
        else
        {
            return await _appointmentService.GetAllAppointmentsAsync();
        }

    }

    private async Task<AppointmentModelResponse> FillResponseModel(AppointmentDTO dto)
    {
        var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);

        var responseModel = _mapper.Map<AppointmentModelResponse>(dto);

        responseModel.Doctor = _mapper.Map<DoctorModelResponse>(doctorSelected)
            .GenerateLinks("doctors");
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected)
            .GenerateLinks("users");

        return responseModel;
    }
}
