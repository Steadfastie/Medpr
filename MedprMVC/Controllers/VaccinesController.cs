using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;
using MedprBusiness.ServiceImplementations;

namespace MedprMVC.Controllers
{
    public class VaccinesController : Controller
    {
        private readonly IVaccineService _vaccineService;
        private readonly IMapper _mapper;
        private readonly int _pagesize = 15;
        public VaccinesController(IVaccineService VaccineService, IMapper mapper)
        {
            _vaccineService = VaccineService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            try
            {
                var dtos = await _vaccineService.GetVaccinesByPageNumberAndPageSizeAsync(page, _pagesize);

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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var dto = await _vaccineService.GetVaccinesByIdAsync(id);
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
                return BadRequest(ex.Message);
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
                    var alreadyCreated = await _vaccineService.GetVaccinesByIdAsync(model.Id);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var dto = await _vaccineService.GetVaccinesByIdAsync(id);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VaccineModel model)
        {
            try
            {
                if (model != null)
                {
                    var alreadyCreated = await _vaccineService.GetVaccinesByIdAsync(model.Id);
                    if (alreadyCreated != null)
                    {
                        RedirectToAction("Details", "Drugs", model.Id);
                    }

                    var dto = _mapper.Map<VaccineDTO>(model);

                    var sourceDto = await _vaccineService.GetVaccinesByIdAsync(model.Id);

                    var patchList = new List<PatchModel>();

                    if (dto != null)
                    {
                        foreach (PropertyInfo property in typeof(VaccineDTO).GetProperties())
                        {
                            if (!property.GetValue(dto).Equals(property.GetValue(sourceDto))) {
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var dto = await _vaccineService.GetVaccinesByIdAsync(id);

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
                return BadRequest(ex.Message);
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
                    var dto = await _vaccineService.GetVaccinesByIdAsync(id);

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
                return BadRequest(ex.Message);
            }
        }
    }
}
