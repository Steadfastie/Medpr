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
/// Controller for vaccines
/// </summary>
[Route("vaccines")]
[ApiController]
[Authorize]
public class VaccinesController : ControllerBase
{
    private readonly IVaccineService _vaccineService;
    private readonly IMapper _mapper;
    public VaccinesController(IVaccineService vaccineService, IMapper mapper)
    {
        _vaccineService = vaccineService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all vaccines
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<VaccineModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _vaccineService.GetAllVaccinesAsync();

            var models = _mapper.Map<List<VaccineModelResponse>>(dtos);

            if (models.Any())
            {
                return Ok(models.Select(model => model.GenerateLinks("vaccines")));
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
                Message = "Could not load vaccines",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the vaccine</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VaccineModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _vaccineService.GetVaccineByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<VaccineModelResponse>(dto);

                model.GenerateLinks("vaccines");

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
                Message = "Could not load vaccine",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Create new vaccine for the app. Forbids creation of vaccine with existing in app name
    /// </summary>
    /// <param name="model">Model with vaccine parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(VaccineModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VaccineModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] VaccineModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var alreadyCreated = await _vaccineService.GetVaccineByNameAsync(model.Name);
                if (alreadyCreated != null)
                {
                    return Forbid();
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<VaccineDTO>(model);

                await _vaccineService.CreateVaccineAsync(dto);

                var responseModel = _mapper.Map<VaccineModelResponse>(dto);

                return CreatedAtAction(nameof(Create), new { id = dto.Id }, responseModel.GenerateLinks("vaccines"));
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
                Message = "Could not create vaccine",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Edit some data about vaccine. Forbids vaccine's name change. Returns SC304 if there is nothing to patch.
    /// </summary>
    /// <param name="model">Vaccine parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(VaccineModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VaccineModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromForm] VaccineModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var sourceDto = await _vaccineService.GetVaccineByIdAsync(model.Id);
                if (sourceDto.Name != model.Name)
                {
                    return Forbid();
                }

                var dto = _mapper.Map<VaccineDTO>(model);

                var patchList = new List<PatchModel>();

                foreach (PropertyInfo property in typeof(VaccineDTO).GetProperties())
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
                    await _vaccineService.PatchVaccineAsync(model.Id, patchList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status304NotModified, model);
                }

                var updatedVaccine = await _vaccineService.GetVaccineByIdAsync(model.Id);

                var responseModel = _mapper.Map<VaccineModelResponse>(updatedVaccine);

                return Ok(responseModel.GenerateLinks("vaccines"));
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
                Message = "Could not update vaccine info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    /// <summary>
    /// Remove vaccine from app
    /// </summary>
    /// <param name="id">Vaccine's id</param>
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
                var dto = await _vaccineService.GetVaccineByIdAsync(id);

                if(dto == null)
                {
                    return BadRequest();
                }

                await _vaccineService.DeleteVaccineAsync(dto);

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
                Message = "Could not delete vaccine from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}
