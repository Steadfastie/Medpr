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
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly int _pagesize = 15;
        public DoctorsController(IDoctorService DoctorService, IMapper mapper)
        {
            _doctorService = DoctorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            try
            {
                var dtos = await _doctorService.GetDoctorsByPageNumberAndPageSizeAsync(page, _pagesize);

                var models = _mapper.Map<List<DoctorModel>>(dtos);

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
                var dto = await _doctorService.GetDoctorsByIdAsync(id);
                if (dto != null)
                {
                    var model = _mapper.Map<DoctorModel>(dto);
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
        public async Task<IActionResult> Create(DoctorModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var alreadyCreated = await _doctorService.GetDoctorsByIdAsync(model.Id);
                    if (alreadyCreated != null)
                    {
                        RedirectToAction("Details", "Drugs", model.Id);
                    }

                    model.Id = Guid.NewGuid();

                    var dto = _mapper.Map<DoctorDTO>(model);

                    await _doctorService.CreateDoctorAsync(dto);

                    return RedirectToAction("Index", "Doctors");
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
                    var dto = await _doctorService.GetDoctorsByIdAsync(id);
                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var editModel = _mapper.Map<DoctorModel>(dto);

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
        public async Task<IActionResult> Edit(DoctorModel model)
        {
            try
            {
                if (model != null)
                {
                    var alreadyCreated = await _doctorService.GetDoctorsByIdAsync(model.Id);
                    if (alreadyCreated != null)
                    {
                        RedirectToAction("Details", "Drugs", model.Id);
                    }

                    var dto = _mapper.Map<DoctorDTO>(model);

                    var sourceDto = await _doctorService.GetDoctorsByIdAsync(model.Id);

                    var patchList = new List<PatchModel>();

                    if (dto != null)
                    {
                        foreach (PropertyInfo property in typeof(DoctorDTO).GetProperties())
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

                    await _doctorService.PatchDoctorAsync(model.Id, patchList);

                    return RedirectToAction("Index", "Doctors");
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
                    var dto = await _doctorService.GetDoctorsByIdAsync(id);

                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var deleteModel = _mapper.Map<DoctorModel>(dto);

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
                    var dto = await _doctorService.GetDoctorsByIdAsync(id);

                    await _doctorService.DeleteDoctorAsync(dto);

                    return RedirectToAction("Index", "Doctors");
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
