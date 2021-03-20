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

        public IndexModel(
            IUnitOfWork unitOfWork,
            UserManager<Customer> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Display(Name = "Nachname")]
        [BindProperty]
        public string FilterLastname { get; set; }
        
        public CustomerWithDetailsDto[] Customers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var customers = await _unitOfWork.Customers.GetByLastnameFilterAsync(FilterLastname);

            Customers = customers
                .Select(async c => new CustomerWithDetailsDto()
                {
                    Id = c.Id,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    Username = c.UserName,
                    RegisteredSince = c.RegisteredSince,
                    IsAdmin = (await _userManager.GetRolesAsync(c)).Any(r => r == "Admin")
                })
                .Select(t => t.Result)
                .ToArray();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            var customers = await _unitOfWork.Customers.GetByLastnameFilterAsync(FilterLastname);

            Customers = customers
                .Select(async c => new CustomerWithDetailsDto()
                {
                    Id = c.Id,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    Username = c.UserName,
                    RegisteredSince = c.RegisteredSince,
                    IsAdmin = (await _userManager.GetRolesAsync(c)).Any(r => r == "Admin")
                })
                .Select(t => t.Result)
                .ToArray();
     
            return Page();
        }
    }
}
