using AutoMapper;
using Hangfire;
using Hangfire.Storage;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Links;
using MedprModels.Requests;
using MedprModels.Responses;
using MedprWebAPI.Utils;
using MedprWebAPI.Utils.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Reflection;

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
    private readonly INotificationService _notificationService;
    private readonly IHubContext<EventNotificationHub, INotificationHub> _eventNotification;
    private readonly string NotificationMessage = "It's time for an appointment";
    private readonly string NotificationType = "appointments";

    private WardedPeople WardedPeople => new(_familyService, _familyMemberService);

    public AppointmentsController(IAppointmentService appointmentService,
        IDoctorService doctorService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager,
        INotificationService notificationService,
        IHubContext<EventNotificationHub, INotificationHub> eventNotificationHub)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
        _familyMemberService = familyMemberService;
        _familyService = familyService;
        _notificationService = notificationService;
        _eventNotification = eventNotificationHub;
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

                models.Add(responseModel.GenerateLinks(NotificationType));
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
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
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

                return Ok(responseModel.GenerateLinks(NotificationType));
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
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
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
    public async Task<IActionResult> Create([FromBody] AppointmentModelRequest model)
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

                if (model.Date.ToUniversalTime() > DateTime.UtcNow)
                {
                    dto.NotificationId = BackgroundJob
                        .Schedule(() => _notificationService
                            .SendNotification(NotificationMessage, NotificationType, $"{dto.Id}", model.UserId),
                        dto.Date.ToUniversalTime() - DateTime.UtcNow);
                }

                await _appointmentService.CreateAppointmentAsync(dto);

                var responseModel = await FillResponseModel(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks(NotificationType));
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
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Edit some data about appointment
    /// </summary>
    /// <param name="id">URL check</param>
    /// <param name="model">Appointment parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppointmentModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(Guid id, [FromBody] AppointmentModelRequest model)
    {
        try
        {
            if (ModelState.IsValid && id == model.Id)
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
                dto.NotificationId = sourceDto.NotificationId;

                if (dto.NotificationId != null)
                {
                    // Check if job was not enqueued by dev in hangfire UI
                    IStorageConnection connection = JobStorage.Current.GetConnection();
                    JobData? jobData = connection.GetJobData(dto.NotificationId);
                    if (jobData != null && !jobData.State.Equals("Succeeded"))
                    {
                        // Refresh notification
                        if (dto.Date != sourceDto.Date && dto.Date.ToUniversalTime() > DateTime.UtcNow)
                        {
                            BackgroundJob.Delete(sourceDto.NotificationId);
                            dto.NotificationId = BackgroundJob.Schedule(() => _notificationService
                                .SendNotification(NotificationMessage, NotificationType, $"{dto.Id}", model.UserId),
                                dto.Date.ToUniversalTime() - DateTime.UtcNow);
                        }
                        if (dto.Date != sourceDto.Date && dto.Date.ToUniversalTime() < DateTime.UtcNow)
                        {
                            BackgroundJob.Delete(sourceDto.NotificationId);
                            dto.NotificationId = null;
                        }
                    }
                    else
                    {
                        dto.NotificationId = null;
                    }
                }

                if (sourceDto.NotificationId == null && dto.Date.ToUniversalTime() > DateTime.UtcNow)
                {
                    dto.NotificationId = BackgroundJob.Schedule(() => _notificationService
                        .SendNotification(NotificationMessage, NotificationType, $"{dto.Id}", model.UserId),
                        dto.Date.ToUniversalTime() - DateTime.UtcNow);
                }

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(AppointmentDTO).GetProperties())
                    {
                        if (property.Name.Equals("NotificationId"))
                        {
                            patchList.Add(new PatchModel()
                            {
                                PropertyName = property.Name,
                                PropertyValue = property.GetValue(dto)
                            });
                            continue;
                        }
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

                return Ok(responseModel.GenerateLinks(NotificationType));
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
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
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
                if (dto.NotificationId != null)
                {
                    IStorageConnection connection = JobStorage.Current.GetConnection();
                    JobData? jobData = connection.GetJobData(dto.NotificationId);
                    if (jobData != null)
                    {
                        BackgroundJob.Delete(dto.NotificationId);
                    }
                }

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
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
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