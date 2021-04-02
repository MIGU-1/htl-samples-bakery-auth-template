using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bakery.Web.DataTransferObjects;
using Bakery.Core.Entities;
using System;

namespace Bakery.Web.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserDto AuthUser { get; set; }
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;

        public RegisterModel(SignInManager<Customer> signInManager, UserManager<Customer> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (_userManager.FindByNameAsync(AuthUser.Username) == null)
                {
                    var result = await _userManager.CreateAsync(new Customer()
                    {
                        Firstname = AuthUser.Firstname,
                        Lastname = AuthUser.Lastname,
                        UserName = AuthUser.Username
                    }, AuthUser.Password);

                    if (result.Succeeded)
                    {
                        var customer = await _userManager.FindByNameAsync(AuthUser.Username);
                        await _signInManager.SignInAsync(customer, false);
                        return RedirectToPage("/Orders/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                            string.Join(string.Empty, result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    ModelState.AddModelError("AuthUser.Username", "Username existiert bereits");
                }
            }
            return Page();
        }
    }
}
