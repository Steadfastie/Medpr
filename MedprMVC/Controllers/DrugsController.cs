using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;


namespace MedprMVC.Controllers
{
    public class DrugsController : Controller
    {
        private readonly IDrugService _drugService;
        public DrugsController(IDrugService drugService)
        {
            _drugService = drugService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                
                var dto = await _drugService.GetDrugsByIdAsync(id);
                if (dto != null)
                {
                    return View(dto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
