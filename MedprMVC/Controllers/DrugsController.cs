using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;

namespace MedprMVC.Controllers
{
    public class DrugsController : Controller
    {
        private readonly IDrugService _drugService;
        private readonly IMapper _mapper;
        private readonly int _pagesize = 15;
        public DrugsController(IDrugService drugService, IMapper mapper)
        {
            _drugService = drugService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            try
            {
                var dtos = await _drugService.GetDrugsByPageNumberAndPageSizeAsync(page, _pagesize);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var dto = await _drugService.GetDrugsByIdAsync(id);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
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
                    model.Id = Guid.NewGuid();

                    var dto = _mapper.Map<DrugDTO>(model);

                    await _drugService.CreateArticleAsync(dto);

                    return RedirectToAction("Index", "Drugs");
                }

                else
                {
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
