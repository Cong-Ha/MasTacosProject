using MasTacos.Models;
using MasTacos.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasTacos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public MenuItemsController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            try
            {
                var menu = await _menuItemRepository.GetAllAsync();
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItemsByCategory(string category)
        {
            try
            {
                var menu = await _menuItemRepository.GetAllAsync();
                var filteredItems = menu.Where(item => item.Category != null &&
                                                     item.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                return Ok(filteredItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetPopularItems()
        {
            try
            {
                var menu = await _menuItemRepository.GetAllAsync();
                var popularItems = menu.Where(item => item.PopularityScore >= 80);
                return Ok(popularItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
        {
            try
            {
                if (menuItem == null)
                {
                    return BadRequest("Menu item is null");
                }
                var createdMenuItem = await _menuItemRepository.CreateAsync(menuItem);
                return CreatedAtAction(nameof(GetMenuItem), new { id = createdMenuItem.ItemId }, createdMenuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating menu item: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem)
        {
            try
            {
                if (id != menuItem.ItemId)
                {
                    return BadRequest("Menu item ID mismatch");
                }
                var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);
                if (existingMenuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }
                // Update the menu item properties here
                existingMenuItem.Name = menuItem.Name;
                existingMenuItem.Price = menuItem.Price;
                existingMenuItem.Description = menuItem.Description;
                existingMenuItem.Category = menuItem.Category;
                existingMenuItem.PopularityScore = menuItem.PopularityScore;
                // Add any other properties that need updating

                await _menuItemRepository.UpdateAsync(existingMenuItem);
                return Ok(existingMenuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating menu item: {ex.Message}");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateMenuItemStatus(int id, [FromBody] StatusUpdateModel model)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }
                menuItem.IsActive = model.IsActive;
                await _menuItemRepository.UpdateAsync(menuItem);
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating menu item status: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);

                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }

                var result = await _menuItemRepository.DeleteAsync(menuItem);

                if (result)
                {
                    return Ok($"Menu item with ID {id} deleted successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete menu item");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting menu item: {ex.Message}");
            }
        }
    }
}