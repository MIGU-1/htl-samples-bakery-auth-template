using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Bakery.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SchoolLocker.Web.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Customer> _userManager;

        [BindProperty]
        public CustomerWithDetailsDto Customer { get; set; }

        public EditModel(IUnitOfWork uow, UserManager<Customer> userManager)
        {
            _unitOfWork = uow;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            Customer = new CustomerWithDetailsDto()
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                IsAdmin = await _userManager.IsInRoleAsync(customer, "Admin"),
                UserName = customer.UserName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(Customer.Id.ToString());

                    if (Customer.IsAdmin && !await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else if (!Customer.IsAdmin && await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }

                    var customerInDb = await _unitOfWork.Customers.GetByIdAsync(Customer.Id);
                    customerInDb.Firstname = Customer.Firstname;
                    customerInDb.Lastname = Customer.Lastname;
                    await _unitOfWork.SaveChangesAsync();

                    return RedirectToPage("/Customers/Index");
                }
                catch(ValidationException ex)
                {
                    ValidationResult result = ex.ValidationResult;
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return Page();
                }
            }

            return Page();
        }
    }
}
