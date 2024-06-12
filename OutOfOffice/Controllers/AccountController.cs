using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.Infrastructure;
using OutOfOffice.Models.Account;
using OutOfOffice.Services.Image;
using System.Text.Json;

namespace OutOfOffice.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserActionRepository userActionRepository;
        private readonly IImageService imageService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserActionRepository userActionRepository, IImageService imageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userActionRepository = userActionRepository;
            this.imageService = imageService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] SignInViewModel signInModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? applicationUser = await userManager.FindByEmailAsync(signInModel.Email);

                if (applicationUser == null)
                {
                    ModelState.AddModelError("", "Incorrect password or login.");
                    return View();
                }

                Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(applicationUser, signInModel.Password, true, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterSecondStage()
        {
            return View(JsonSerializer.Deserialize<RegisterViewModel>(TempData["IntermidiateRegisterData"] as string ?? throw new Exception("Intermidiate data lost"))!);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSecondStage(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                EmployeeBase employeeBase;
                switch (viewModel.SelectedEmployeeType)
                {
                    case EmployeeType.Employee:
                        employeeBase = await userActionRepository.CreateNewEmployeeAsync(new Employee
                        {
                            FullName = viewModel.FullName,
                            HRManagerId = viewModel.HRManagerId ??throw new Exception(),
                            IsActive = viewModel.IsActive,
                            Photo = viewModel.Photo == null ? null : await imageService.ToByteArrayAsync(viewModel.Photo),
                            Position = viewModel.SelectedPosition,
                            Subdivition = viewModel.SelectedSubdivision,
                        });
                        break;
                    case EmployeeType.ProjectManager:
                        employeeBase = await userActionRepository.CreateNewEmployeeAsync(new ProjectManager
                        {
                            FullName = viewModel.FullName,
                            HRManagerId = viewModel.HRManagerId ?? throw new Exception(),
                            IsActive = viewModel.IsActive,
                            Photo = viewModel.Photo == null ? null : await imageService.ToByteArrayAsync(viewModel.Photo),
                            Position = viewModel.SelectedPosition,
                            Subdivition = viewModel.SelectedSubdivision,
                        });
                        break;
                    case EmployeeType.HR:
                        employeeBase = await userActionRepository.CreateNewEmployeeAsync(new HRManager
                        {
                            FullName = viewModel.FullName,
                            IsActive = viewModel.IsActive,
                            Photo = viewModel.Photo == null ? null : await imageService.ToByteArrayAsync(viewModel.Photo),
                            Position = viewModel.SelectedPosition,
                            Subdivition = viewModel.SelectedSubdivision,
                        });
                        break;
                    default:
                        throw new ArgumentException(nameof(viewModel.SelectedEmployeeType));
                }

                await CreateApplicationUser(employeeBase, viewModel.SelectedEmployeeType, viewModel.Password, viewModel.Email);

                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                TempData["IntermidiateRegisterData"] = JsonSerializer.Serialize(viewModel);
                return RedirectToAction(nameof(RegisterSecondStage));
            }
            return View(viewModel);
        }

        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.SetJson("CurrentUser", null);
            return RedirectToAction("Index", "Home");
        }

        private async Task CreateApplicationUser(EmployeeBase employeeBase,EmployeeType employeeType, string password, string email)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email,
                EmployeeId = employeeBase.Id,
                UserName = employeeBase.FullName
            };

            var pswrdResult = await userManager.CreateAsync(applicationUser, password);
            if (!pswrdResult.Succeeded)
            {
                throw new ArgumentException(nameof(pswrdResult));
            }

            IdentityResult? identResult;
            switch (employeeType)
            {
                case EmployeeType.Employee:
                    identResult = await userManager.AddToRoleAsync(applicationUser, "Employee");
                    break;
                case EmployeeType.ProjectManager:
                    identResult = await userManager.AddToRoleAsync(applicationUser, "Project Manager");
                    break;
                case EmployeeType.HR:
                    identResult = await userManager.AddToRoleAsync(applicationUser, "HR Manager");
                    break;
                default:
                    throw new ArgumentException(nameof(employeeType));
            }

            if (!identResult.Succeeded)
            {
                throw new ArgumentException(nameof(identResult));
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
