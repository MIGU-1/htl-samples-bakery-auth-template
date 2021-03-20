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
        [Display(Name = "Nachname")]
        public string FilterLastname { get; set; }

        public CustomerWithDetailsDto[] Customers { get; set; }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public IActionResult OnPostSearchAsync()
        {
            return Page();
        }
    }
}
