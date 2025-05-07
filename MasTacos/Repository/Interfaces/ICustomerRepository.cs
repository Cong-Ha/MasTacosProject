using MasTacos.Models;

namespace MasTacos.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddLoyaltyPointsAsync(int id, int points);
        Task<Customer> CreateAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetByIdAsync(int id);
        Task<int> GetLoyaltyPointsAsync(int id);
        Task UpdateAsync(Customer customer);
    }
}