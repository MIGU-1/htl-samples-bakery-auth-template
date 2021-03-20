using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;

namespace SchoolLocker.Web.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;

        public EditModel(UserManager<Customer> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public CustomerWithDetailsDto Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer customer = await _userManager.FindByIdAsync(id.ToString());

            if (customer == null)
            {
                return NotFound();
            }

            Customer = new CustomerWithDetailsDto
            {
                Id = customer.Id,
                UserName = customer.UserName,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                IsAdmin = (await _userManager.GetRolesAsync(customer)).Any(r => r == "Admin")
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer dbCustomer = await _userManager.FindByIdAsync(Customer.Id.ToString());
            dbCustomer.Firstname = Customer.Firstname;
            dbCustomer.Lastname = Customer.Lastname;

            await _userManager.UpdateAsync(dbCustomer);

            if (Customer.IsAdmin)
            {
                await _userManager.AddToRoleAsync(dbCustomer, "Admin");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(dbCustomer, "Admin");
            }

            return RedirectToPage("./Index");
        }
    }
}
