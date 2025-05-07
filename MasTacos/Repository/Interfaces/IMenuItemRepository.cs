using MasTacos.Models;

namespace MasTacos.Repository.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(int id);
        Task<MenuItem> CreateAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task<bool> DeleteAsync(MenuItem menuItem);
    }
}