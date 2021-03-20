using System.Threading.Tasks;
using Bakery.Core.Entities;
using Bakery.Web.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public CredentialDto Credentials { get; set; }

        public IActionResult OnPostAsync()
        {
            return Page();
        }
    }
}
