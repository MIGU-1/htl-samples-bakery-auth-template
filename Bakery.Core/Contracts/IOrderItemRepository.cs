using System.Collections.Generic;
using System.Threading.Tasks;
using Bakery.Core.Entities;

namespace Bakery.Core.Contracts
{
    public interface IOrderItemRepository
    {
        Task<int> GetCountAsync();
        Task AddRangeAsync(IEnumerable<OrderItem> orderItems);
        Task AddAsync(OrderItem orderItem);
        Task RemoveAsync(int id);
    }
}