using System;
using System.Threading.Tasks;
using C3Apparel.Frontend.Data.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace C3Apparel.Web.Features.Account
{
    
    public class AccountController : Controller
    {
        
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICurrentUserProvider _currentUserProvider;
        
        public AccountController(SignInManager<IdentityUser> signInManager, ICurrentUserProvider currentUserProvider)
        {
            _signInManager = signInManager;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ActionResult> LoginPage()
        {
            var currenctUeer = _currentUserProvider.GetCurrentUserInfo();
            if (currenctUeer.IsPublicUser)
            {
                return View("~/Features/Account/LoginPage.cshtml", new LoginPageViewModel());
                
            }

            if (currenctUeer.IsGlobalAdministrator)
            {
                return new RedirectResult("/pricing-inquiry");
            }

            if (!string.IsNullOrEmpty(currenctUeer.CountryRole))
            {
                return new RedirectResult("/pricing");
            }

            return new RedirectResult("/page-not-found");
        }
        
        [HttpPost]
        [Route("/submit-login")]
        public async Task<ActionResult> Login(LoginPageViewModel request)
        {
            var signInResult = SignInResult.Failed;

            try
            {
                signInResult = await _signInManager.PasswordSignInAsync(
                    request.Username, 
                    request.Password, 
                    false, 
                    false);
            }
            catch (Exception ex)
            {
               
            }

            if (signInResult.Succeeded)
            {
                
                return Redirect("/login");
            }

            TempData["ModelStateError"] = "Login failed.";
            return Redirect("/login");
        }
        
        [Route("/logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}