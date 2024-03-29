﻿using AutoMapper;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Reflection;

namespace MedprMVC.Controllers;

[Authorize]
public class VaccinesController : Controller
{
    private readonly IVaccineService _vaccineService;
    private readonly IMapper _mapper;

    public VaccinesController(IVaccineService VaccineService, IMapper mapper)
    {
        _vaccineService = VaccineService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _vaccineService.GetAllVaccinesAsync();

            var models = _mapper.Map<List<VaccineModel>>(dtos);

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
            var dto = await _vaccineService.GetVaccineByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<VaccineModel>(dto);
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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var alreadyCreated = await _vaccineService.GetVaccineByIdAsync(model.Id);
                if (alreadyCreated != null)
                {
                    RedirectToAction("Details", "Drugs", model.Id);
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<VaccineDTO>(model);

                await _vaccineService.CreateVaccineAsync(dto);

                return RedirectToAction("Index", "Vaccines");
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
                var dto = await _vaccineService.GetVaccineByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var editModel = _mapper.Map<VaccineModel>(dto);

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
    public async Task<IActionResult> Edit(VaccineModel model)
    {
        try
        {
            if (model != null)
            {
                var alreadyCreated = await _vaccineService.GetVaccineByIdAsync(model.Id);
                if (alreadyCreated != null)
                {
                    RedirectToAction("Details", "Drugs", model.Id);
                }

                var dto = _mapper.Map<VaccineDTO>(model);

                var sourceDto = await _vaccineService.GetVaccineByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
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
                }

                await _vaccineService.PatchVaccineAsync(model.Id, patchList);

                return RedirectToAction("Index", "Vaccines");
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
                var dto = await _vaccineService.GetVaccineByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var deleteModel = _mapper.Map<VaccineModel>(dto);

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
                var dto = await _vaccineService.GetVaccineByIdAsync(id);

                await _vaccineService.DeleteVaccineAsync(dto);

                return RedirectToAction("Index", "Vaccines");
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
}