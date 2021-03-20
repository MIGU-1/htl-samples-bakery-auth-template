using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Bakery.Web.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<Customer> _userManager;

        public OrdersController(
            IUnitOfWork uow,
            UserManager<Customer> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        
        /// <summary>
        /// Retrievs a list of orders.
        /// If the user has the admin role then all the orders are retrieved.
        /// If the user does not has the admin role then only the personal order items are retrieved.
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // GET: api/Orders
        public async Task<ActionResult<IEnumerable<OrderWithItemsDto>>> GetOrders()
        {
            string userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var customer = await _userManager.FindByNameAsync(userName);

            return Ok(await _uow.Orders.GetAllWithItemsAsync(customer.Id));
        }

        
        /// <summary>
        /// Retrieves a list of orders for a specific customer.
        /// Admin role is needed to perform this action!
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("ordersByCustomerId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Orders/ordersByCustomerId/5
        public async Task<ActionResult<IEnumerable<OrderWithItemsDto>>> GetOrdersByCustomer(int id)
        {
            var orders = await _uow.Orders.GetOrdersByCustomer(id);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

    }
}
