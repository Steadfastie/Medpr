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

namespace MedprMVC.Controllers;

public class PrescriptionsController : Controller
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;
    private readonly IDrugService _drugService;
    private readonly IMapper _mapper;
    private readonly int _pagesize = 15;
    public PrescriptionsController(IPrescriptionService prescriptionService,
        IDoctorService doctorService,
        IUserService userService,
        IDrugService drugService,
        IMapper mapper)
    {
        _prescriptionService = prescriptionService;
        _doctorService = doctorService;
        _mapper = mapper;
        _userService = userService;
        _drugService = drugService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page)
    {
        try
        {
            var dtos = await _prescriptionService.GetPrescriptionsByPageNumberAndPageSizeAsync(page, _pagesize);

            List<PrescriptionModel> models = new();

            foreach (var dto in dtos)
            {
                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugsByIdAsync(dto.DrugId);

                var model = _mapper.Map<PrescriptionModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                model.User = _mapper.Map<UserModel>(userSelected);
                model.Drug = _mapper.Map<DrugModel>(drugSelected);

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
            var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

            if (dto != null)
            {
                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugsByIdAsync(dto.DrugId);

                var model = _mapper.Map<PrescriptionModel>(dto);

                model.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                model.User = _mapper.Map<UserModel>(userSelected);
                model.Drug = _mapper.Map<DrugModel>(drugSelected);

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
        var allDoctors = await _doctorService.GetAllDoctorsAsync();
        var allUsers = await _userService.GetAllUsersAsync();
        var allDrugs = await _drugService.GetAllDrugsAsync();
        PrescriptionModel model = new();
        model.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name");
        model.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "FullName");
        model.Drugs = new SelectList(_mapper.Map<List<DrugModel>>(allDrugs), "Id", "Name");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PrescriptionModel model)
    {
        try
        {
            // At the current moment Prescription Model has 6 additional fields to form other
            // actions's models. They aren't suppose to fill in, so ModelState.IsValid won't
            // be true. The other way around is used in Edit[post]
            if (ModelState.ErrorCount < 7 && CheckDate(model))
            {
                model.Id = Guid.NewGuid();

                var dto = _mapper.Map<PrescriptionDTO>(model);

                await _prescriptionService.CreatePrescriptionAsync(dto);

                return RedirectToAction("Index", "Prescriptions");
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
                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);
                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var allUsers = await _userService.GetAllUsersAsync();

                var drugSelected = await _drugService.GetDrugsByIdAsync(dto.DrugId);
                var allDrugs = await _drugService.GetAllDrugsAsync();

                var editModel = _mapper.Map<PrescriptionModel>(dto);

                editModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                editModel.Doctors = new SelectList(_mapper.Map<List<DoctorModel>>(allDoctors), "Id", "Name", doctorSelected.Id.ToString());
                editModel.User = _mapper.Map<UserModel>(userSelected);
                editModel.Users = new SelectList(_mapper.Map<List<UserModel>>(allUsers), "Id", "FullName", userSelected.Id.ToString());
                editModel.Drug = _mapper.Map<DrugModel>(drugSelected);
                editModel.Drugs = new SelectList(_mapper.Map<List<DrugModel>>(allDrugs), "Id", "Name", drugSelected.Id.ToString());

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
    public async Task<IActionResult> Edit(PrescriptionModel model)
    {
        try
        {
            if (model != null)
            {
                var dto = _mapper.Map<PrescriptionDTO>(model);

                var sourceDto = await _prescriptionService.GetPrescriptionsByIdAsync(model.Id);

                var patchList = new List<PatchModel>();

                if (dto != null)
                {
                    foreach (PropertyInfo property in typeof(PrescriptionDTO).GetProperties())
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

                await _prescriptionService.PatchPrescriptionAsync(model.Id, patchList);

                return RedirectToAction("Index", "Prescriptions");
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
                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

                if (dto == null)
                {
                    return BadRequest();
                }

                var doctorSelected = await _doctorService.GetDoctorsByIdAsync(dto.DoctorId);
                var userSelected = await _userService.GetUsersByIdAsync(dto.UserId);
                var drugSelected = await _drugService.GetDrugsByIdAsync(dto.DrugId);

                var deleteModel = _mapper.Map<PrescriptionModel>(dto);

                deleteModel.Doctor = _mapper.Map<DoctorModel>(doctorSelected);
                deleteModel.User = _mapper.Map<UserModel>(userSelected);
                deleteModel.Drug = _mapper.Map<DrugModel>(drugSelected);

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
                var dto = await _prescriptionService.GetPrescriptionsByIdAsync(id);

                await _prescriptionService.DeletePrescriptionAsync(dto);

                return RedirectToAction("Index", "Prescriptions");
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
    public IActionResult CheckDate(string startDate, string endDate)
    {
        var start = DateTime.Parse(startDate);
        var end = DateTime.Parse(endDate);
        if (start < end)
        {
            return Ok(true);
        }
        return Ok(false);
    }

    private bool CheckDate(PrescriptionModel model)
    {
        if (model.StartDate > model.EndDate)
        {
            return false;
        }
        return true;
    }
}
