using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using MedprModels.Responses;
using MedprModels;
using MedprModels.Requests;
using MedprModels.Links;

namespace MedprWebAPI.Controllers;

/// <summary>
/// Controller for doctors
/// </summary>
[Route("doctors")]
[ApiController]
[Authorize]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    public DoctorsController(IDoctorService doctorService, IMapper mapper)
    {
        _doctorService = doctorService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all doctors
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<DoctorModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _doctorService.GetAllDoctorsAsync();

            var models = _mapper.Map<List<DoctorModelResponse>>(dtos);

            if (models.Any())
            {
                return Ok(models.Select(model => model.GenerateLinks("doctors")));
            }
            else
            {
                return Ok(null);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            ErrorModel errorModel = new()
            {
                Message = "Could not load doctors",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the doctor</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DoctorModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _doctorService.GetDoctorByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<DoctorModelResponse>(dto);

                model.GenerateLinks("doctors");

                return Ok(model);
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
                Message = "Could not load doctor",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Create new doctor for the app. Forbids creation of doctor with existing in app name
    /// </summary>
    /// <param name="model">Model with doctor parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(DoctorModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DoctorModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] DoctorModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var alreadyCreated = await _doctorService.GetDoctorByNameAsync(model.Name);
                if (alreadyCreated != null)
                {
                    return Forbid();
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<DoctorDTO>(model);

                await _doctorService.CreateDoctorAsync(dto);

                var responseModel = _mapper.Map<DoctorModelResponse>(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("doctors"));
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
                Message = "Could not create doctor",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Edit some data about doctor. Forbids doctor's name change. Returns SC304 if there is nothing to patch.
    /// </summary>
    /// <param name="model">Doctor parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(DoctorModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DoctorModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromBody] DoctorModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var sourceDto = await _doctorService.GetDoctorByIdAsync(model.Id);
                if (sourceDto.Name != model.Name)
                {
                    return Forbid();
                }

                var dto = _mapper.Map<DoctorDTO>(model);

                var patchList = new List<PatchModel>();

                foreach (PropertyInfo property in typeof(DoctorDTO).GetProperties())
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

                if (patchList.Any())
                {
                    await _doctorService.PatchDoctorAsync(model.Id, patchList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status304NotModified, model);
                }

                var updatedDoctor = await _doctorService.GetDoctorByIdAsync(model.Id);

                var responseModel = _mapper.Map<DoctorModelResponse>(updatedDoctor);

                return Ok(responseModel.GenerateLinks("doctors"));
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
                Message = "Could not update doctor info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Remove doctor from app
    /// </summary>
    /// <param name="id">Doctor's id</param>
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
                var dto = await _doctorService.GetDoctorByIdAsync(id);

                if(dto == null)
                {
                    return BadRequest();
                }

                await _doctorService.DeleteDoctorAsync(dto);

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
                Message = "Could not delete doctor from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}
