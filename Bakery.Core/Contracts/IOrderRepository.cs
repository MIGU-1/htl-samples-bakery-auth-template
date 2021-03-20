using System.Collections.Generic;
using System.Threading.Tasks;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;

namespace Bakery.Core.Contracts
{
    public interface IOrderRepository
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<OrderDto>> GetAllAsync();

        Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsAsync(int? customerId);
        Task<OrderDto> FindAsync(int id);

        Task<OrderWithItemsDto> GetOrderWithItems(int id);

        Task<IEnumerable<OrderWithItemsDto>> GetOrdersByCustomer(int id);
        Task<IEnumerable<OrderDto>> GetFilteredForCustomers(string lastNameFilter);
        Task RemoveAsync(int orderId);
        Task AddAsync(Order order);
    }
}