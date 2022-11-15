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
/// Controller for drugs
/// </summary>
[Route("drugs")]
[ApiController]
[Authorize]
public class DrugsController : ControllerBase
{
    private readonly IDrugService _drugService;
    private readonly IMapper _mapper;
    public DrugsController(IDrugService drugService, IMapper mapper)
    {
        _drugService = drugService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all drugs
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<DrugModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _drugService.GetAllDrugsAsync();

            var models = _mapper.Map<List<DrugModelResponse>>(dtos);

            if (models.Any())
            {
                return Ok(models.Select(model => model.GenerateLinks("drugs")));
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
                Message = "Could not load drugs",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Find info on one particular resourse
    /// </summary>
    /// <param name="id">Id of the drug</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DrugModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var dto = await _drugService.GetDrugByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<DrugModelResponse>(dto);

                model.GenerateLinks("drugs");

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
                Message = "Could not load drug",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Create new drug for the app. Forbids creation of drug with existing in app name
    /// </summary>
    /// <param name="model">Model with drug parameters</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(DrugModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DrugModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromForm] DrugModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var alreadyCreated = await _drugService.GetDrugByNameAsync(model.Name);
                if (alreadyCreated != null)
                {
                    return Forbid();
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<DrugDTO>(model);

                await _drugService.CreateDrugAsync(dto);

                var responseModel = _mapper.Map<DrugModelResponse>(dto);

                return CreatedAtAction(nameof(Details), new { id = dto.Id }, responseModel.GenerateLinks("drugs"));
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
                Message = "Could not create drug",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Edit some data about drug. Forbids drug's name change. Returns SC304 if there is nothing to patch.
    /// </summary>
    /// <param name="model">Drug parameters. Name should not change</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(DrugModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DrugModelResponse), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromForm] DrugModelRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var sourceDto = await _drugService.GetDrugByIdAsync(model.Id);
                if (sourceDto.Name != model.Name)
                {
                    return Forbid();
                }

                var dto = _mapper.Map<DrugDTO>(model);

                var patchList = new List<PatchModel>();

                foreach (PropertyInfo property in typeof(DrugDTO).GetProperties())
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
                    await _drugService.PatchDrugAsync(model.Id, patchList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status304NotModified, model);
                }

                var updatedDrug = await _drugService.GetDrugByIdAsync(model.Id);

                var responseModel = _mapper.Map<DrugModelResponse>(updatedDrug);

                return Ok(responseModel.GenerateLinks("drugs"));
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
                Message = "Could not update drug info",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }

    /// <summary>
    /// Remove drug from app
    /// </summary>
    /// <param name="id">Drug's id</param>
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
                var dto = await _drugService.GetDrugByIdAsync(id);

                if(dto == null)
                {
                    return BadRequest();
                }

                await _drugService.DeleteDrugAsync(dto);

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
                Message = "Could not delete drug from app",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return RedirectToAction("Error", "Home", errorModel);
        }
    }
}
