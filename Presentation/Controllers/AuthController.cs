using Business.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public IActionResult SignUp()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var formData = model.MapTo<SignUpFormData>();
            var result = await _authService.SignUpAsync(formData);

            if (result.Succeeded)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            ModelState.AddModelError(
                nameof(SignUpViewModel.Email),
                result.Error ?? "An unknown error has occurred during sign-up"
            );
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl = "~/")
        {
            ModelState.Clear();
            ViewBag.ReturnUrl = returnUrl;

            if (TempData.TryGetValue("AuthError", out var err) && err is string msg)
            {
                ModelState.AddModelError(string.Empty, msg);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var formData = model.MapTo<SignInFormData>();
            var result = await _authService.SignInAsync(formData);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
