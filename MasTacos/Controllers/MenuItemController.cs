using MasTacos.Models;
using MasTacos.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasTacos.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        [HttpGet("menu")]
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

        [HttpGet("menu/{id}")]
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

        [HttpPost("menu")]
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

        [HttpPut("menu/{id}")]
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
                await _menuItemRepository.UpdateAsync(existingMenuItem);
                return Ok(existingMenuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating menu item: {ex.Message}");
            }
        }

        [HttpDelete("menu/{id}")]
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
