using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bakery.Web.DataTransferObjects;
using Bakery.Core.Entities;
using System;

namespace Bakery.Web.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public UserDto AuthUser { get; set; }

        public IActionResult OnPostAsync()
        {
            return Page();
        }
    }
}
