using MasTacos.Data;
using MasTacos.Models;
using MasTacos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MasTacos.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly MasTacosContext _context;

        public MenuItemRepository(MasTacosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }
            return menuItem;
        }

        public async Task<MenuItem> CreateAsync(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.Entry(menuItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(MenuItem menuItem)
        {
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
