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
using Hangfire;
using Hangfire.Storage;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for prescriptions
/// </summary>
[Route("prescriptions")]
[ApiController]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IDoctorService _doctorService;
    private readonly IDrugService _drugService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly string NotificationMessage = "It's time to take a pill";
    private readonly string NotificationType = "prescriptions";
    private WardedPeople WardedPeople => new(_familyService, _familyMemberService);
    public PrescriptionsController(IPrescriptionService prescriptionService,
        IDoctorService doctorService,
        IDrugService drugService,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IUserService userService,
        IMapper mapper,
        UserManager<IdentityUser<Guid>> userManager,
        INotificationService notificationService)
    {
        _prescriptionService = prescriptionService;
        _doctorService = doctorService;
        _drugService = drugService;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
        _familyMemberService = familyMemberService;
        _familyService = familyService;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Get all prescriptions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<PrescriptionModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<PrescriptionDTO> dtos = await GetRelevantPrescriptions();

            List<PrescriptionModelResponse> models = new();

            foreach (var dto in dtos)
            {
                var responseModel = await FillResponseModel(dto);

                models.Add(responseModel.GenerateLinks("prescriptions"));
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
                Message = "Could not load prescriptions",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the prescription</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrescriptionModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _prescriptionService.GetPrescriptionByIdAsync(id);

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

                return Ok(responseModel.GenerateLinks("prescriptions"));
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
                Message = "Could not load prescription",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Create new prescription for the app
    /// </summary>
    /// <param name="model">Model with prescription parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(PrescriptionModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PrescriptionModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] PrescriptionModelRequest model)
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

                var dto = _mapper.Map<PrescriptionDTO>(model);

                if (model.Date.ToUniversalTime() > DateTime.UtcNow)
                {
                    dto.NotificationId = BackgroundJob
                    .Schedule(() => _notificationService.SendNotification(NotificationMessage, NotificationType, $"{dto.Id}"),
                    dto.Date.ToUniversalTime() - DateTime.UtcNow);
                }

                await _prescriptionService.CreatePrescriptionAsync(dto);

                var responseModel = await FillResponseModel(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("prescriptions"));
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
                Message = "Could not create prescription",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Edit some data about prescription
    /// </summary>
    /// <param name="id">URL check</param>
    /// <param name="model">Prescription parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(PrescriptionModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PrescriptionModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(Guid id, [FromBody] PrescriptionModelRequest model)
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

                var dto = _mapper.Map<PrescriptionDTO>(model);

                var sourceDto = await _prescriptionService.GetPrescriptionByIdAsync(model.Id);
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
                            dto.NotificationId = BackgroundJob.Schedule(() =>
                                _notificationService.SendNotification(NotificationMessage, NotificationType, $"{dto.Id}"),
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
                    dto.NotificationId = BackgroundJob
                    .Schedule(() => _notificationService.SendNotification(NotificationMessage, NotificationType, $"{dto.Id}"),
                    dto.Date.ToUniversalTime() - DateTime.UtcNow);
                }

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(PrescriptionDTO).GetProperties())
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

                await _prescriptionService.PatchPrescriptionAsync(model.Id, patchList);

                var responseModel = await FillResponseModel(dto);

                return Ok(responseModel.GenerateLinks("prescriptions"));
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
                Message = "Could not update prescription info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Remove prescription from app
    /// </summary>
    /// <param name="id">Prescription's id</param>
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
                var dto = await _prescriptionService.GetPrescriptionByIdAsync(id);

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

                await _prescriptionService.DeletePrescriptionAsync(dto);
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
                Message = "Could not delete prescription from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    private async Task<List<PrescriptionDTO>> GetRelevantPrescriptions()
    {
        var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
        var currentUser = await _userManager.FindByNameAsync(userName);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<PrescriptionDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = await WardedPeople.GetWardedByUserPeople(currentUser.Id);

            foreach (Guid userId in users)
            {
                var userPrescriptions = await _prescriptionService.GetPrescriptionsByUserIdAsync(userId);
                dtos.AddRange(userPrescriptions);
            }
            return dtos;
        }
        else
        {
            return await _prescriptionService.GetAllPrescriptionsAsync();
        }

    }

    private async Task<PrescriptionModelResponse> FillResponseModel(PrescriptionDTO dto)
    {
        var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);
        var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);

        var responseModel = _mapper.Map<PrescriptionModelResponse>(dto);

        responseModel.Doctor = _mapper.Map<DoctorModelResponse>(doctorSelected)
            .GenerateLinks("doctors");
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected)
            .GenerateLinks("users");
        responseModel.Drug = _mapper.Map<DrugModelResponse>(drugSelected)
            .GenerateLinks("drugs");

        return responseModel;
    }
}
