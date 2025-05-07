using MasTacos.Models;
using MasTacos.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasTacos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // GET: api/Customers/email/john@example.com
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _customerRepository.GetByEmailAsync(email);

                if (customer == null)
                {
                    return NotFound($"Customer with email {email} not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest("Customer is null");
                }

                // Set default values
                if (customer.JoinDate == default)
                {
                    customer.JoinDate = DateTime.Now;
                }

                var createdCustomer = await _customerRepository.CreateAsync(customer);

                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating record: {ex.Message}");
            }
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            try
            {
                if (id != customer.CustomerId)
                {
                    return BadRequest("Customer ID mismatch");
                }

                var customerToUpdate = await _customerRepository.GetByIdAsync(id);

                if (customerToUpdate == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                await _customerRepository.UpdateAsync(customer);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating record: {ex.Message}");
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                await _customerRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting record: {ex.Message}");
            }
        }

        // GET: api/Customers/5/loyalty
        [HttpGet("{id}/loyalty")]
        public async Task<ActionResult<int>> GetLoyaltyPoints(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                var loyaltyPoints = await _customerRepository.GetLoyaltyPointsAsync(id);
                return Ok(loyaltyPoints);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // POST: api/Customers/5/loyalty/add/100
        [HttpPost("{id}/loyalty/add/{points}")]
        public async Task<IActionResult> AddLoyaltyPoints(int id, int points)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                await _customerRepository.AddLoyaltyPointsAsync(id, points);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating record: {ex.Message}");
            }

        }
    }
}
