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
/// Controller for vaccinations
/// </summary>
[Route("vaccinations")]
[ApiController]
[Authorize]
public class VaccinationsController : ControllerBase
{
    private readonly IVaccinationService _vaccinationService;
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IVaccineService _vaccineService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private WardedPeople WardedPeople => new(_familyService, _familyMemberService);
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

    /// <summary>
    /// Get all vaccinations
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<VaccinationModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<VaccinationDTO> dtos = await GetRelevantVaccinations();

            List<VaccinationModelResponse> models = new();

            foreach (var dto in dtos)
            {
                var responseModel = await FillResponseModel(dto);

                models.Add(responseModel.GenerateLinks("vaccinations"));
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
                Message = "Could not load vaccinations",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the vaccination</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VaccinationModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _vaccinationService.GetVaccinationByIdAsync(id);

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
                Message = "Could not load vaccination",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Create new vaccination for the app
    /// </summary>
    /// <param name="model">Model with vaccination parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(VaccinationModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VaccinationModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] VaccinationModelRequest model)
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

                var dto = _mapper.Map<VaccinationDTO>(model);

                await _vaccinationService.CreateVaccinationAsync(dto);

                var responseModel = await FillResponseModel(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("vaccinations"));
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
                Message = "Could not create vaccination",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Edit some data about vaccination
    /// </summary>
    /// <param name="model">Vaccination parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(VaccinationModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VaccinationModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromForm] VaccinationModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<VaccinationDTO>(model);

                var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                var currentUser = await _userManager.FindByNameAsync(userName);

                var ids = await WardedPeople.GetWardedByUserPeople(currentUser.Id);
                if (!ids.Contains(dto.UserId))
                {
                    return Forbid();
                }

                var sourceDto = await _vaccinationService.GetVaccinationByIdAsync(model.Id);

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

                var responseModel = await FillResponseModel(dto);

                return Ok(responseModel.GenerateLinks("vaccinations"));
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
                Message = "Could not update vaccination info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Remove vaccination from app
    /// </summary>
    /// <param name="id">Vaccination's id</param>
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
                var dto = await _vaccinationService.GetVaccinationByIdAsync(id);

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

                await _vaccinationService.DeleteVaccinationAsync(dto);

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
                Message = "Could not delete vaccination from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    private async Task<List<VaccinationDTO>> GetRelevantVaccinations()
    {
        var userName = User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
        var currentUser = await _userManager.FindByNameAsync(userName);
        var currentUserRole = await _userManager.GetRolesAsync(currentUser);

        List<VaccinationDTO> dtos = new();
        if (currentUserRole[0] == "Default")
        {
            List<Guid> users = await WardedPeople.GetWardedByUserPeople(currentUser.Id);

            foreach (Guid userId in users)
            {
                var userVaccinations = await _vaccinationService.GetVaccinationsByUserIdAsync(userId);
                dtos.AddRange(userVaccinations);
            }
            return dtos;
        }
        else
        {
            return await _vaccinationService.GetAllVaccinationsAsync();
        }

    }

    private async Task<VaccinationModelResponse> FillResponseModel(VaccinationDTO dto)
    {
        var vaccineSelected = await _vaccineService.GetVaccineByIdAsync(dto.VaccineId);
        var userSelected = await _userService.GetUserByIdAsync(dto.UserId);

        var responseModel = _mapper.Map<VaccinationModelResponse>(dto);

        responseModel.Vaccine = _mapper.Map<VaccineModelResponse>(vaccineSelected)
            .GenerateLinks("doctors");
        responseModel.User = _mapper.Map<UserModelResponse>(userSelected)
            .GenerateLinks("users");

        return responseModel;
    }
}
