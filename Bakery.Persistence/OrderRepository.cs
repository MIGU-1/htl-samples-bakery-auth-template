using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Persistence
{
  public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Orders.CountAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .OrderBy(o => o.OrderNr)
                .Select(o => new OrderDto(o))
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsAsync(int? customerId)
        {
            return await _dbContext.Orders
                .Where(o => !customerId.HasValue || o.CustomerId == customerId)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .OrderBy(o => o.OrderNr)
                .Select(o => new OrderWithItemsDto(o))
                .ToListAsync();
        }

        public async Task<OrderDto> FindAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();
            return new OrderDto(order);
            
        }

        public async Task<OrderWithItemsDto> GetOrderWithItems(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();
            return new OrderWithItemsDto(order);
        }

        public async Task<IEnumerable<OrderWithItemsDto>> GetOrdersByCustomer(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .Where(o => o.Customer.Id == id)
                .OrderBy(o => o.OrderNr)
                .Select(o => new OrderWithItemsDto(o))
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredForCustomers(string lastNameFilter)
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .Where(o => string.IsNullOrEmpty(lastNameFilter) || o.Customer.Lastname.ToUpper().StartsWith(lastNameFilter.ToUpper()))
                .OrderBy(o => o.OrderNr)
                .Select(o => new OrderDto(o))
                .ToListAsync();
        }

        public async Task RemoveAsync(int orderId)
        {
            var o = await _dbContext.Orders.FindAsync(orderId);
            _dbContext.Orders.Remove(o);
        }

        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }
    }
}
