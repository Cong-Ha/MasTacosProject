using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MasTacos.Models;

namespace MasTacos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
        : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true // Auto-confirm for development
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign default role
                await userManager.AddToRoleAsync(user, "Customer");

                logger.LogInformation("User {Email} registered successfully", model.Email);
                
                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = await GenerateJwtToken(user)
                });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                return Unauthorized(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid email or password" 
                });
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);

            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                
                logger.LogInformation("User {Email} logged in successfully", model.Email);
                
                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = await userManager.GetRolesAsync(user)
                    }
                });
            }

            if (result.IsLockedOut)
            {
                return StatusCode(423, new AuthResponse 
                { 
                    Success = false, 
                    Message = "Your account is locked due to multiple failed login attempts" 
                });
            }

            return Unauthorized(new AuthResponse 
            { 
                Success = false, 
                Message = "Invalid email or password" 
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new AuthResponse 
            { 
                Success = true, 
                Message = "Logout successful" 
            });
        }

        [HttpGet("user")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = await userManager.GetRolesAsync(user)
            });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // View Models
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // Response Models
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
    }
}