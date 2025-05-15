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

                if (menuItem.ImageData != null)
                {
                    existingMenuItem.ImageData = menuItem.ImageData;
                    existingMenuItem.ImageMimeType = menuItem.ImageMimeType;
                }

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

                // First check if the menu item has an image and clear it
                if (menuItem.ImageData != null)
                {
                    menuItem.ImageData = null;
                    menuItem.ImageMimeType = null;

                    // Update the menu item to clear the image data
                    await _menuItemRepository.UpdateAsync(menuItem);

                    // Re-fetch the menu item
                    menuItem = await _menuItemRepository.GetByIdAsync(id);
                }

                // Now delete the menu item
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

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);

                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }

                if (menuItem.ImageData == null || menuItem.ImageMimeType == null)
                {
                    return NotFound("This menu item has no image");
                }

                return File(menuItem.ImageData, menuItem.ImageMimeType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving image: {ex.Message}");
            }
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest("Invalid file type. Only JPEG, PNG, and GIF are allowed.");
                }

                if (file.Length > 2 * 1024 * 1024) // 2 MB limit
                {
                    return BadRequest("File size exceeds the limit of 2 MB.");
                }
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    menuItem.ImageData = memoryStream.ToArray();
                    menuItem.ImageMimeType = file.ContentType;
                }

                await _menuItemRepository.UpdateAsync(menuItem);
                return Ok($"Image for menu item with ID {id} uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpDelete("{id}/image")]
        public async Task<IActionResult> RemoveImage(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found");
                }
                if (menuItem.ImageData == null)
                {
                    return NotFound("This menu item has no image to remove");
                }

                menuItem.ImageData = null;
                menuItem.ImageMimeType = null;
                await _menuItemRepository.UpdateAsync(menuItem);
                return Ok($"Image for menu item with ID {id} removed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error removing image: {ex.Message}");
            }
        }
    }
}