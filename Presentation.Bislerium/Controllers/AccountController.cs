using Domaim.Bislerium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Domain.Bislerium.RequestedViewModel.ViewAuthenticationModel;

namespace Presentation.Bislerium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller

    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;

        public record LoginResponse(bool Flag, string Token, string Message);
        public record UserSession(string? Id, string? Name, string? Email, string? Role);
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody] LoginModel loginUser)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {

                var getUser = await _userManager.FindByEmailAsync(loginUser.Email);
                var getUserRole = await _userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.UserName, getUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);

                return new LoginResponse(true, token!, "Login completed");

            }
            else
            {
                return new LoginResponse(false, null!, "Login not completed");
            }
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,     // Assign the Name property
                Gender = model.Gender, // Assign the Gender property
                Phone = model.Phone    // Assign the Phone property
            };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role!);



                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }



        [HttpGet("getAll")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with given ID is not found
            }

            return Ok(user);
        }


        [HttpPut("updateByEmail")]
        public async Task<IActionResult> Update(string email, UpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;     // Update the Name property
            user.Phone = model.Phone;   // Update the Phone property
/*
            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            // Get the roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);
            // Remove existing roles
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            // Assign the specified role to the user
            await _userManager.AddToRoleAsync(user, model.Role!);*/

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }

            return BadRequest(result.Errors);
        }


        [HttpDelete("deleteByEmail")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("changepassword"), Authorize]

        public async Task<IActionResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with given ID is not found
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors); // Return any validation errors if password change fails
            }

            return Ok(result); // Return 204 No Content if password change is successful
        }


        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}