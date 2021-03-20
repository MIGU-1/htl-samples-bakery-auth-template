using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Web.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public IEnumerable<ProductDto> Products { get; set; }

        [BindProperty]
        public OrderWithItemsDto Order { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Sie müssen ein Produkt auswählen!")]
        public int NewProductId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Sie müssen die Anzahl festlegen!")]
        [Range(1, 5000, ErrorMessage = "Die Anzahl muss zwischen {1} und {2} Stück liegen!")]
        public int NewProductAmount { get; set; }

        public EditModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private async Task InitPageAsync(int orderId)
        {
            Order = await _uow.Orders.GetOrderWithItems(orderId);
            Products = await _uow.Products.GetAllAsync();
            NewProductAmount = 1;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await InitPageAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            OrderItem newOrderItem = new()
            {
                OrderId = Order.OrderId,
                ProductId = NewProductId,
                Amount = NewProductAmount
            };

            await _uow.OrderItems.AddAsync(newOrderItem);
            await _uow.SaveChangesAsync();

            await InitPageAsync(Order.OrderId);

            return Page();
        }

        public async Task<IActionResult> OnGetDeleteOrderItemAsync(int orderId, int orderItemId)
        {
            await _uow.OrderItems.RemoveAsync(orderItemId);
            await _uow.SaveChangesAsync();

            await InitPageAsync(orderId);

            return Page();
        }
    }
}
