using System.Threading.Tasks;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public ActionResult OnPostAsync()
        {
            return Page();
        }
    }
}
