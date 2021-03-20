using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Web.Pages.Orders
{
    public class CreateModel : PageModel
    {
        [DisplayName("Bestellnummer")]
        [Required(ErrorMessage = "Bestellnummer darf nicht leer sein!")]
        public string OrderNr { get; set; }

        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Das Datum ist erforderlich!")]
        public DateTime Date { get; set; }

        public int CustomerId { get; set; }

        [DisplayName("Kunde")]
        public string CustomerName { get; set; }

        public IEnumerable<Customer> Customers { get; set; }


        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            return Page();
        }

    }
}
