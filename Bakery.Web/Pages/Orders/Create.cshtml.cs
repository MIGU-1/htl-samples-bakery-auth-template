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
        [BindProperty]
        [Required(ErrorMessage = "Bestellnummer darf nicht leer sein!")]
        public string OrderNr { get; set; }

        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [BindProperty]
        [Required(ErrorMessage = "Das Datum ist erforderlich!")]
        public DateTime Date { get; set; } = DateTime.Now;

        [BindProperty]
        public int CustomerId { get; set; }

        [DisplayName("Kunde")]
        [BindProperty]
        public string CustomerName { get; set; }

        private readonly UserManager<Customer> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Customer> Customers { get; set; }

        public CreateModel(UserManager<Customer> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            CustomerId = user.Id;
            CustomerName = user.FullName;
            Customers = await _unitOfWork.Customers.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var customerInDb = await _unitOfWork.Customers.GetByIdAsync(CustomerId);
                    Order newOrder = new()
                    {
                        Customer = customerInDb,
                        CustomerId = customerInDb.Id,
                        OrderNr = OrderNr,
                        Date = Date,
                        OrderItems = new List<OrderItem>()
                    };

                    await _unitOfWork.Orders.AddAsync(newOrder);
                    await _unitOfWork.SaveChangesAsync();

                    return RedirectToPage("/Orders/Index");
                }
                catch(ValidationException ex)
                {
                    ValidationResult result = ex.ValidationResult;
                    ModelState.AddModelError(String.Empty, result.ErrorMessage);
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Fehler beim Speichern");
            }

            return RedirectToPage("/Orders/Index");
        }
    }
}
