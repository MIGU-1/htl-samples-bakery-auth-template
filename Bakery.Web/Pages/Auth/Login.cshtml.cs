using System.Threading.Tasks;
using Bakery.Core.Entities;
using Bakery.Web.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public CredentialDto Credentials { get; set; }
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public LoginModel(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Credentials.Username);
                if(user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(
                        Credentials.Username,
                        Credentials.Password,
                        isPersistent: true,
                        lockoutOnFailure: false);

                    if (signInResult.Succeeded)
                    {
                        string redirectTo = !string.IsNullOrEmpty(
                            Request.Query["ReturnUrl"]) ? Request.Query["ReturnUrl"] : "/Orders";
                        return RedirectToPage(redirectTo + "/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Credentials.Password", "Login fehlgeschlagen");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError("Credentials.Username", "Login fehlgeschlagen");
                }
            }
            return Page();
        }
    }
}
