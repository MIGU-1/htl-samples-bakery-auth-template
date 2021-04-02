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
        [Display(Name = "Nachname")]
        [BindProperty]
        public string FilterLastName { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
        [BindProperty]
        public IEnumerable<OrderDto> Orders { get; set; }
        
        private readonly UserManager<Customer> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(UserManager<Customer> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IsAdmin = await _userManager.IsInRoleAsync(user,"Admin");

            if(IsAdmin)
            {
                Orders = await _unitOfWork.Orders.GetAllAsync();
            }
            else
            {
                Orders = await _unitOfWork.Orders.GetOrdersByCustomer(user.Id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSearch()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            Orders = await _unitOfWork.Orders.GetFilteredForCustomers(FilterLastName);
            return Page();
        }
    }
}
