using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetCountAsync()
            => await _dbContext.Customers.CountAsync();

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _dbContext.Customers
                .OrderBy(c => c.Lastname)
                .ThenBy(c => c.Firstname)
                .ToListAsync();

        public async Task<IEnumerable<Customer>> GetByLastnameFilterAsync(string lastnameFilter)
            => await _dbContext.Customers
                .Where(c => string.IsNullOrEmpty(lastnameFilter) || c.Lastname.ToUpper().StartsWith(lastnameFilter.ToUpper()))
                .OrderBy(c => c.Lastname)
                .ThenBy(c => c.Firstname)
                .ToListAsync();

        public async Task<Customer> GetByIdAsync(int id)
            => await _dbContext.Customers
                .FindAsync(id);
    }
}
