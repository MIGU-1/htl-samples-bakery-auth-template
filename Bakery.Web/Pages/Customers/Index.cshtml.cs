using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Bakery.Web.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Web.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Customer> _userManager;

        [Display(Name = "Nachname")]
        [BindProperty]
        public string FilterLastname { get; set; }
        [BindProperty]
        public CustomerWithDetailsDto[] Customers { get; set; }

        public IndexModel(IUnitOfWork unitOfWork, UserManager<Customer> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            Customers = customers.Select(c => new CustomerWithDetailsDto()
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                IsAdmin = _userManager.IsInRoleAsync(c, "Admin").Result,
                NrOfOrders = c.Orders != null ? c.Orders.Count() : 0,
                Username = c.UserName,
                RegisteredSince = c.RegisteredSince
            })
            .ToArray();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            var customers = await _unitOfWork.Customers.GetByLastnameFilterAsync(FilterLastname);
            Customers = customers.Select(c => new CustomerWithDetailsDto()
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                IsAdmin = _userManager.IsInRoleAsync(c, "Admin").Result,
                NrOfOrders = c.Orders != null ? c.Orders.Count() : 0,
                Username = c.UserName,
                RegisteredSince = c.RegisteredSince
            })
            .ToArray();

            return Page();
        }
    }
}
