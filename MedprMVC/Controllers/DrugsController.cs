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
public class DrugsController : Controller
{
    private readonly IDrugService _drugService;
    private readonly IMapper _mapper;

    public DrugsController(IDrugService drugService, IMapper mapper)
    {
        _drugService = drugService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var dtos = await _drugService.GetAllDrugsAsync();

            var models = _mapper.Map<List<DrugModel>>(dtos);

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
            var dto = await _drugService.GetDrugByIdAsync(id);
            if (dto != null)
            {
                var model = _mapper.Map<DrugModel>(dto);
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
    public async Task<IActionResult> Create(DrugModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var alreadyCreated = await _drugService.GetDrugByIdAsync(model.Id);
                if (alreadyCreated != null)
                {
                    RedirectToAction("Details", "Drugs", model.Id);
                }

                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<DrugDTO>(model);

                await _drugService.CreateDrugAsync(dto);

                return RedirectToAction("Index", "Drugs");
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
                var dto = await _drugService.GetDrugByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var editModel = _mapper.Map<DrugModel>(dto);

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
    public async Task<IActionResult> Edit(DrugModel model)
    {
        try
        {
            if (model != null)
            {
                var alreadyCreated = await _drugService.GetDrugByIdAsync(model.Id);
                if (alreadyCreated != null)
                {
                    RedirectToAction("Details", "Drugs", model.Id);
                }

                var dto = _mapper.Map<DrugDTO>(model);

                var sourceDto = await _drugService.GetDrugByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
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
                }

                await _drugService.PatchDrugAsync(model.Id, patchList);

                return RedirectToAction("Index", "Drugs");
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
                var dto = await _drugService.GetDrugByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var deleteModel = _mapper.Map<DrugModel>(dto);

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
                var dto = await _drugService.GetDrugByIdAsync(id);

                await _drugService.DeleteDrugAsync(dto);

                return RedirectToAction("Index", "Drugs");
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