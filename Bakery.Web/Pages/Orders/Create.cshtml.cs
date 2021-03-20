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
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<Customer> _userManager;

        [BindProperty]
        [DisplayName("Bestellnummer")]
        [Required(ErrorMessage = "Bestellnummer darf nicht leer sein!")]
        public string OrderNr { get; set; }

        [BindProperty]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Das Datum ist erforderlich!")]
        public DateTime Date { get; set; }

        [BindProperty]
        public int CustomerId { get; set; }


        [BindProperty]
        [DisplayName("Kunde")]
        public string CustomerName { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        public CreateModel(IUnitOfWork uow,
            UserManager<Customer> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var customer = await _userManager.GetUserAsync(HttpContext.User);
            CustomerId = customer.Id;
            CustomerName = customer.FullName;

            Customers = await _uow.Customers.GetAllAsync();
            Date = DateTime.Today;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Order newOrder = new()
                {
                    CustomerId = CustomerId,
                    Date = Date,
                    OrderNr = OrderNr
                };

                await _uow.Orders.AddAsync(newOrder);

                try
                {
                    await _uow.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(nameof(OrderNr), ex.Message);
                    Customers = await _uow.Customers.GetAllAsync();
                }
            }

            var customer = await _userManager.GetUserAsync(HttpContext.User);
            CustomerId = customer.Id;
            CustomerName = customer.FullName;

            return Page();
        }

    }
}
