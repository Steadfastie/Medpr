using MedprMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text;
using MedprCore.Abstractions;
using AutoMapper;
using MedprCore.DTO;
using MedprCore;

namespace MedprMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        SignInManager<IdentityUser<Guid>> signInManager,
        IMapper mapper,
        IUserService userService)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserModel model)
    {
        if (model.Login.Any() && model.PasswordHash.Any())
        {
            model.Id = Guid.NewGuid();
            //var dto = _mapper.Map<UserDTO>(model);
            //dto.PasswordHash = PasswordHash.CreateMd5(model.PasswordHash);
            //await _userService.CreateUserAsync(dto);

            var identityUser = new IdentityUser<Guid>(model.Login);
            var result = await _userManager.CreateAsync(identityUser, model.PasswordHash);

            if (result.Succeeded)
            {
                var defaultrole = await _roleManager.FindByIdAsync("e1f26886-137e-446d-9ad8-c85d774305ce");
                if (defaultrole != null)
                {
                    var check = await _roleManager.RoleExistsAsync(defaultrole.Name);
                    var check2 = await _roleManager.RoleExistsAsync("Default");
                    var roleResult = await _userManager.AddToRoleAsync(identityUser, "Default");
                }
                await _signInManager.SignInAsync(identityUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserModel model)
    {
        if (model.Login.Any() && model.PasswordHash.Any())
        {
            var user = await _userManager.FindByEmailAsync(model.Login);
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.PasswordHash, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        return View(model);
    }
}