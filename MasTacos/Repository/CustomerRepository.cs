using MasTacos.Data;
using MasTacos.Models;
using MasTacos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MasTacos.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MasTacosContext _context;

        public CustomerRepository(MasTacosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetLoyaltyPointsAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer?.LoyaltyPoints ?? 0;
        }

        public async Task AddLoyaltyPointsAsync(int id, int points)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.LoyaltyPoints += points;
                await _context.SaveChangesAsync();
            }
        }
    }
}
