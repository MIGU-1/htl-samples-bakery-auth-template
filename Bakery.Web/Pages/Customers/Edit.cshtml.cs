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
    public class EditModel : PageModel
    {
        public CustomerWithDetailsDto Customer { get; set; }

        public IActionResult OnGetAsync(int id)
        {
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            return Page();
        }
    }
}
