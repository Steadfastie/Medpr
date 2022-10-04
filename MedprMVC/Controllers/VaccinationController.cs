using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;
using MedprDB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedprMVC.Controllers
{
    public class VaccinationsController : Controller
    {
        private readonly IVaccinationService _vaccinationService;
        private readonly IVaccineService _vaccineService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly int _pagesize = 15;
        public VaccinationsController(IVaccinationService vaccinationService,
            IVaccineService vaccineService,
            IUserService userService,
            IMapper mapper)
        {
            _vaccinationService = vaccinationService;
            _vaccineService = vaccineService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            try
            {
                var dtos = await _vaccinationService.GetVaccinationsByPageNumberAndPageSizeAsync(page, _pagesize);

                List<VaccinationModel> models = new();

                foreach (var dto in dtos)
                {
                    var vaccineSelected = await _vaccineService.GetVaccinesByIdAsync(dto.VaccineId);
                    var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                    var model = _mapper.Map<VaccinationModel>(dto);

                    model.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                    model.User = _mapper.Map<UserModel>(userSelected);

                    models.Add(model);
                }

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
                var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                if (dto != null)
                {
                    var vaccineSelected = await _vaccineService.GetVaccinesByIdAsync(dto.VaccineId);
                    var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                    var model = _mapper.Map<VaccinationModel>(dto);

                    model.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                    model.User = _mapper.Map<UserModel>(userSelected);

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
        public async Task<IActionResult> CreateAsync()
        {
            var allVaccines = await _vaccineService.GetAllVaccinesAsync();
            var allUsers = await _userService.GetAllUsersAsync();
            VaccinationModel model = new();
            model.Vaccines = new SelectList(_mapper.Map<List<VaccineModel>>(allVaccines));
            model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VaccinationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = Guid.NewGuid();

                    var dto = _mapper.Map<VaccinationDTO>(model);

                    await _vaccinationService.CreateVaccinationAsync(dto);

                    return RedirectToAction("Index", "Vaccinations");
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
                    var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);
                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var vaccineSelected = await _vaccineService.GetVaccinesByIdAsync(dto.VaccineId);
                    var allVaccines = await _vaccineService.GetAllVaccinesAsync();

                    var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                    var allUsers = await _userService.GetAllUsersAsync();

                    var editModel = _mapper.Map<VaccinationModel>(dto);

                    editModel.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                    editModel.Vaccines = new SelectList(_mapper.Map<List<VaccineModel>>(allVaccines));
                    editModel.User = _mapper.Map<UserModel>(userSelected);
                    editModel.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers));

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
        public async Task<IActionResult> Edit(VaccinationModel model)
        {
            try
            {
                if (model != null)
                {
                    var dto = _mapper.Map<VaccinationDTO>(model);

                    var sourceDto = await _vaccinationService.GetVaccinationsByIdAsync(model.Id);

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

                    return RedirectToAction("Index", "Vaccinations");
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
                    var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var vaccineSelected = await _vaccineService.GetVaccinesByIdAsync(dto.VaccineId);
                    var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);

                    var deleteModel = _mapper.Map<VaccinationModel>(dto);

                    deleteModel.Vaccine = _mapper.Map<VaccineModel>(vaccineSelected);
                    deleteModel.User = _mapper.Map<UserModel>(userSelected);

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
                    var dto = await _vaccinationService.GetVaccinationsByIdAsync(id);

                    await _vaccinationService.DeleteVaccinationAsync(dto);

                    return RedirectToAction("Index", "Vaccinations");
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
