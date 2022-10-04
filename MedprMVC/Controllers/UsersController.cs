using Microsoft.AspNetCore.Mvc;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using AutoMapper;
using MedprMVC.Models;
using Serilog;
using System.Reflection;

namespace MedprMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly int _pagesize = 15;
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            try
            {
                var dtos = await _userService.GetUsersByPageNumberAndPageSizeAsync(page, _pagesize);

                var models = _mapper.Map<List<UserModel>>(dtos);

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
                var dto = await _userService.GetUsersByIdAsync(id);
                if (dto != null)
                {
                    var model = _mapper.Map<UserModel>(dto);
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var alreadyCreated = await _userService.GetUsersByIdAsync(model.Id);
                    if (alreadyCreated != null)
                    {
                        RedirectToAction("Details", "Users", model.Id);
                    }

                    model.Id = Guid.NewGuid();

                    var dto = _mapper.Map<UserDTO>(model);

                    dto.PasswordHash = PasswordHash.CreateMd5(dto.PasswordHash);

                    await _userService.CreateUserAsync(dto);

                    return RedirectToAction("Index", "Users");
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
                    var dto = await _userService.GetUsersByIdAsync(id);
                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var editModel = _mapper.Map<UserModel>(dto);

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
        public async Task<IActionResult> Edit(UserModel model)
        {
            try
            {
                if (model != null)
                {
                    var alreadyCreated = await _userService.GetUsersByIdAsync(model.Id);
                    if (alreadyCreated != null)
                    {
                        RedirectToAction("Details", "Users", model.Id);
                    }

                    var dto = _mapper.Map<UserDTO>(model);

                    var sourceDto = await _userService.GetUsersByIdAsync(model.Id);

                    var patchList = new List<PatchModel>();

                    if (dto != null)
                    {
                        foreach (PropertyInfo property in typeof(UserDTO).GetProperties())
                        {
                            if (!property.GetValue(dto).Equals(property.GetValue(sourceDto)))
                            {
                                if (property.Name.Equals("PasswordHash"))
                                {
                                    var passwordHash = PasswordHash.CreateMd5(property.GetValue(dto).ToString());
                                    property.SetValue(dto, passwordHash);
                                }
                                patchList.Add(new PatchModel()
                                {
                                    PropertyName = property.Name,
                                    PropertyValue = property.GetValue(dto)
                                });
                            }
                        }
                    }

                    await _userService.PatchUserAsync(model.Id, patchList);

                    return RedirectToAction("Index", "Users");
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
                    var dto = await _userService.GetUsersByIdAsync(id);

                    if (dto == null)
                    {
                        return BadRequest();
                    }

                    var deleteModel = _mapper.Map<UserModel>(dto);

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
                    var dto = await _userService.GetUsersByIdAsync(id);

                    await _userService.DeleteUserAsync(dto);

                    return RedirectToAction("Index", "Users");
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
