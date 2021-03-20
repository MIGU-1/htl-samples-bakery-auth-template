using System.Collections.Generic;
using System.Threading.Tasks;
using Bakery.Core.Entities;

namespace Bakery.Core.Contracts
{
    public interface ICustomerRepository
    {
        Task<int> GetCountAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<IEnumerable<Customer>> GetByLastnameFilterAsync(string lastnameFilter);
    }
}