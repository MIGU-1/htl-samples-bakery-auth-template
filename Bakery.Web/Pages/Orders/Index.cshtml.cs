using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<Customer> _userManager;

        [Display(Name = "Nachname")]
        [BindProperty]
        public string FilterLastName { get; set; }
        public bool IsAdmin { get; set; }

        public IEnumerable<OrderDto> Orders { get; set; }

        public IndexModel(
            IUnitOfWork uow,
            UserManager<Customer> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            Customer customer = await _userManager.GetUserAsync(HttpContext.User);
            IsAdmin = (await _userManager.GetRolesAsync(customer)).Any(r => r == "Admin");

            if (IsAdmin)
            {
                Orders = await _uow.Orders.GetAllAsync();
            }
            else
            {
                Orders = await _uow.Orders.GetOrdersByCustomer(customer.Id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSearch()
        {
            Customer customer = await _userManager.GetUserAsync(HttpContext.User);
            IsAdmin = (await _userManager.GetRolesAsync(customer)).Any(r => r == "Admin");

            Orders = await _uow.Orders.GetFilteredForCustomers(FilterLastName);
            return Page();
        }
    }
}
