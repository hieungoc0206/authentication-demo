using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Data.Repositories;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "/")
        {
            return View(new LoginModel() { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var user = _userRepository.GetByUserNameAndPassword(model.UserName, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("FavoriteColor", user.FavoriteColor),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = model.RememberMe,
                IssuedUtc = DateTime.Now.AddDays(1)
            });
            return LocalRedirect(model.ReturnUrl);
        }

        [AllowAnonymous]
        public ActionResult LoginWithGoogle(string returnUrl = "/")
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleLoginCallback"),
                Items =
                {
                    { "returnUrl" , returnUrl}
                }
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Claims;
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
            return LocalRedirect(result.Properties.Items["returnUrl"]);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}