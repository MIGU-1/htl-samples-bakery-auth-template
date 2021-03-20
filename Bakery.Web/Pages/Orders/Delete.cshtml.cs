using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Bakery.Web.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public int OrderId { get; set; }
        public string OrderNr { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }

        public DeleteModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            OrderDto order = await _uow.Orders.FindAsync(id);

            OrderId = order.OrderId;
            OrderNr = order.OrderNr;
            Date = order.Date;
            CustomerName = order.CustomerName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _uow.Orders.RemoveAsync(OrderId);
            await _uow.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
