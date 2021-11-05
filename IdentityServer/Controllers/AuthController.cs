using IdentityServer.Models;
using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signinManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IIdentityServerInteractionService interactionService) =>
            (_signinManager, _userManager, _interactionService) = (signInManager, userManager, interactionService);

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "User not found");
                return View(viewModel);
            }

            var signinResult = await _signinManager.PasswordSignInAsync(user, viewModel.Password, false, false);
            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Sign In failed. Check your credentials");
                return View(viewModel);
            }

            return Redirect(viewModel.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegistrationViewModel
            {
                ReturnUrl = returnUrl,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (!viewModel.Password.Equals(viewModel.PasswordConfirmation))
            {
                ModelState.AddModelError("Password", "Passwords doesn't match");
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                return View(viewModel);
            }

            user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "User with this User Name already exists");
                return View(viewModel);
            }

            user = new AppUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
            };
            var userCreationResult = await _userManager.CreateAsync(user, viewModel.Password);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(viewModel);
            }
            
            return Redirect(viewModel.ReturnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signinManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutResult.PostLogoutRedirectUri);
        }
    }
}
