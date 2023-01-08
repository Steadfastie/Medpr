using AutoMapper;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Responses;
using MedprWebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for users and errors
/// </summary>
[Route("feed")]
[ApiController]
[Authorize]
public class FeedController : ControllerBase
{
    private readonly ILogger<AppController> _logger;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly IUserService _userService;
    private readonly IFeedService _feedService;
    private readonly IAppointmentService _appointmentService;
    private readonly IVaccinationService _vaccinationService;
    private readonly IPrescriptionService _prescriptionService;
    private readonly IDoctorService _doctorService;
    private readonly IVaccineService _vaccineService;
    private readonly IDrugService _drugService;

    private readonly IMapper _mapper;

    private WardedPeople WardedPeople => new(_familyService, _familyMemberService);

    public FeedController(ILogger<AppController> logger,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IFamilyService familyService,
        IFamilyMemberService familyMemberService,
        IMapper mapper,
        IUserService userService,
        IFeedService feedService,
        IAppointmentService appointmentService,
        IVaccinationService vaccinationService,
        IPrescriptionService prescriptionService,
        IDoctorService doctorService,
        IVaccineService vaccineService,
        IDrugService drugService
        )
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _userService = userService;
        _feedService = feedService;
        _familyMemberService = familyMemberService;
        _familyService = familyService;
        _appointmentService = appointmentService;
        _vaccinationService = vaccinationService;
        _prescriptionService = prescriptionService;
        _doctorService = doctorService;
        _drugService = drugService;
        _vaccineService = vaccineService;
    }

    /// <summary>
    /// Get upcoming events
    /// </summary>
    /// <returns></returns>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(FeedModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upcoming()
    {
        try
        {
            var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
            var currentUser = await _userManager.FindByNameAsync(userName);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            List<Guid> wardedUserIds = await WardedPeople.GetWardedByUserPeople(currentUser.Id);

            var appointmentDtos = await GetRelevantUpcomingAppointments(currentUserRole[0], wardedUserIds);
            var appointmentModels = appointmentDtos
                .Select(async appointment => await FillAppointmentResponseModel(appointment))
                .Select(t => t.Result)
                .ToList();

            var vaccinationDtos = await GetRelevantUpcomingVaccinations(currentUserRole[0], wardedUserIds);
            var vaccinationModels = vaccinationDtos
                .Select(async vaccination => await FillVaccinationResponseModel(vaccination))
                .Select(t => t.Result)
                .ToList();

            var upcomingPrescriptionDtos = await GetRelevantUpcomingPrescriptions(currentUserRole[0], wardedUserIds);
            var upcomingPrescriptionModels = upcomingPrescriptionDtos
                .Select(async prescription => await FillPrescriptionResponseModel(prescription))
                .Select(t => t.Result)
                .ToList();

            var responseModel = new FeedModelResponse()
            {
                Appointments = appointmentModels.Count > 0 ? appointmentModels : null,
                Vaccinations = vaccinationModels.Count > 0 ? vaccinationModels : null,
                Prescriptions = upcomingPrescriptionModels.Count > 0 ? upcomingPrescriptionModels : null
            };

            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load upcoming events",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Get upcoming events
    /// </summary>
    /// <returns></returns>
    [HttpGet("ongoing")]
    [ProducesResponseType(typeof(FeedModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Ongoing()
    {
        try
        {
            var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
            var currentUser = await _userManager.FindByNameAsync(userName);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            List<Guid> wardedUserIds = await WardedPeople.GetWardedByUserPeople(currentUser.Id);

            var ongoingPrescriptionDtos = await GetRelevantOngoingPrescriptions(currentUserRole[0], wardedUserIds);
            var ongoingPrescriptionModels = ongoingPrescriptionDtos
                .Select(async prescription => await FillPrescriptionResponseModel(prescription))
                .Select(t => t.Result)
                .ToList();

            var responseModel = new FeedModelResponse()
            {
                Appointments = null,
                Vaccinations = null,
                Prescriptions = ongoingPrescriptionModels.Count > 0 ? ongoingPrescriptionModels : null
            };

            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load ongoing prescritpions",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    private async Task<List<AppointmentDTO>> GetRelevantUpcomingAppointments(
        string currentUserRole,
        List<Guid> wardedUserId)
    {
        List<AppointmentDTO> dtos = new();
        if (currentUserRole == "Default")
        {
            foreach (Guid userId in wardedUserId)
            {
                var userAppointments = await _feedService.GetUpcomingAppointmentsByUserIdAsync(userId);
                dtos.AddRange(userAppointments);
            }

            dtos.Sort((appointment1, appointment2) => DateTime.Compare(appointment1.Date, appointment2.Date));
            return dtos.Take(5).ToList();
        }
        else
        {
            return await _appointmentService.GetAllAppointmentsAsync();
        }
    }

    private async Task<AppointmentModelResponse> FillAppointmentResponseModel(AppointmentDTO dto)
    {
        var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);

        var responseModel = _mapper.Map<AppointmentModelResponse>(dto);

        responseModel.Doctor = _mapper.Map<DoctorModelResponse>(doctorSelected);
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected);

        return responseModel;
    }

    private async Task<List<VaccinationDTO>> GetRelevantUpcomingVaccinations(
        string currentUserRole,
        List<Guid> wardedUserId)
    {
        List<VaccinationDTO> dtos = new();
        if (currentUserRole == "Default")
        {
            foreach (Guid userId in wardedUserId)
            {
                var userVaccinations = await _feedService.GetUpcomingVaccinationsByUserIdAsync(userId);
                dtos.AddRange(userVaccinations);
            }

            dtos.Sort((vaccination1, vaccination2) => DateTime.Compare(vaccination1.Date, vaccination2.Date));
            return dtos.Take(5).ToList();
        }
        else
        {
            return await _vaccinationService.GetAllVaccinationsAsync();
        }
    }

    private async Task<VaccinationModelResponse> FillVaccinationResponseModel(VaccinationDTO dto)
    {
        var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);

        var responseModel = _mapper.Map<VaccinationModelResponse>(dto);

        responseModel.Vaccine = _mapper.Map<VaccineModelResponse>(vaccineSelected);
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected);

        return responseModel;
    }

    private async Task<List<PrescriptionDTO>> GetRelevantUpcomingPrescriptions(
        string currentUserRole,
        List<Guid> wardedUserId)
    {
        List<PrescriptionDTO> dtos = new();
        if (currentUserRole == "Default")
        {
            foreach (Guid userId in wardedUserId)
            {
                var userPrescriptions = await _feedService.GetUpcomingPrescriptionsByUserIdAsync(userId);
                dtos.AddRange(userPrescriptions);
            }

            dtos.Sort((prescription1, prescription2) => DateTime.Compare(prescription1.Date, prescription2.Date));
            return dtos.Take(5).ToList();
        }
        else
        {
            return await _prescriptionService.GetAllPrescriptionsAsync();
        }
    }

    private async Task<List<PrescriptionDTO>> GetRelevantOngoingPrescriptions(
        string currentUserRole,
        List<Guid> wardedUserId)
    {
        List<PrescriptionDTO> dtos = new();
        if (currentUserRole == "Default")
        {
            foreach (Guid userId in wardedUserId)
            {
                var userPrescriptions = await _feedService.GetOngoingPrescriptionsByUserIdAsync(userId);
                dtos.AddRange(userPrescriptions);
            }

            dtos.Sort((prescription1, prescription2) => DateTime.Compare(prescription1.Date, prescription2.Date));
            return dtos.Take(5).ToList();
        }
        else
        {
            return await _prescriptionService.GetAllPrescriptionsAsync();
        }
    }

    private async Task<PrescriptionModelResponse> FillPrescriptionResponseModel(PrescriptionDTO dto)
    {
        var doctorSelected = await _doctorService.GetDoctorByIdAsync(dto.DoctorId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);
        var drugSelected = await _drugService.GetDrugByIdAsync(dto.DrugId);

        var responseModel = _mapper.Map<PrescriptionModelResponse>(dto);

        responseModel.Doctor = _mapper.Map<DoctorModelResponse>(doctorSelected);
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected);
        responseModel.Drug = _mapper.Map<DrugModelResponse>(drugSelected);

        return responseModel;
    }
}