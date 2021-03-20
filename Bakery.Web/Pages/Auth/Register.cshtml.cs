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
        private readonly UserManager<Customer> _userManager;

        public RegisterModel(UserManager<Customer> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public UserDto AuthUser { get; set; }

        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(
                    new Customer
                    {
                        Firstname = AuthUser.Firstname,
                        Lastname = AuthUser.Lastname,
                        UserName = AuthUser.Username,
                        CustomerNr = new Random().Next(1000).ToString()
                    },
                    AuthUser.Password); 

                if (!result.Succeeded)
                {
                    foreach (string error in result.Errors.Select(e => e.Description))
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    return RedirectToPage("./Login");
                }
            }

            return Page();
        }
    }
}
