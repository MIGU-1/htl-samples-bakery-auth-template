using System.Threading.Tasks;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Customer> _signInManager;

        public LogoutModel(SignInManager<Customer> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
