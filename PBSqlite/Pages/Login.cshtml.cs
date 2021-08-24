using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBSqlite.Models;
using PBSqlite.Services;

namespace PBSqlite.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public User LoginUser { get; set; }

        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var role = await AuthenticateUser(LoginUser.UserName, LoginUser.Password);

                if (!role)
                    return RedirectToPage("Error");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, LoginUser.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5)
                    });


                return RedirectToPage("Admin");
            }

            return RedirectToPage("Error");
        }

        private async Task<bool> AuthenticateUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) return false;

            await using var db = new ApplicationDbContext();
            var x = await db.Users.FindAsync(LoginUser.UserName);
            if (x != null)
            {
                if (password == x.Password)
                    return true;
            }

            return false;
        }
    }
}