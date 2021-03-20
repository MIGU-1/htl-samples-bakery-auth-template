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
using System;

namespace Bakery.Web.ApiControllers
{
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // GET: api/Orders
        public ActionResult<IEnumerable<OrderWithItemsDto>> GetOrders()
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Retrieves a list of orders for a specific customer.
        /// Admin role is needed to perform this action!
        /// </summary>
        [HttpGet("ordersByCustomerId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Orders/ordersByCustomerId/5
        public ActionResult<IEnumerable<OrderWithItemsDto>> GetOrdersByCustomer(int id)
        {
            throw new NotImplementedException();
        }

    }
}
