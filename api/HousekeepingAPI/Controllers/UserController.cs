using HousekeepingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousekeepingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    roles = roles,
                    isActive = user.LockoutEnd == null || user.LockoutEnd < System.DateTimeOffset.UtcNow,
                    createdDate = user.DateRegistered,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString()
                });
            }

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email,
                roles = roles,
                isActive = user.LockoutEnd == null || user.LockoutEnd < System.DateTimeOffset.UtcNow,
                createdDate = user.DateRegistered,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType.ToString()
            };

            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new AppUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                DateRegistered = DateTime.UtcNow,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                // Set UserType based on roles
                UserType = DetermineUserType(userDto.Roles)
            };

            var result = await _userManager.CreateAsync(appUser, userDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add roles
            foreach (var role in userDto.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(appUser, role);
                }
            }

            return CreatedAtAction(nameof(GetUser), new { id = appUser.Id }, new { 
                id = appUser.Id, 
                username = appUser.UserName, 
                email = appUser.Email,
                roles = userDto.Roles,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                DateRegistered = appUser.DateRegistered,
                UserType = appUser.UserType.ToString()
            });
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update basic user info
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            
            // Update username if provided and different from current
            if (!string.IsNullOrEmpty(userDto.Username) && user.UserName != userDto.Username)
            {
                // Check if the new username is already taken
                var existingUser = await _userManager.FindByNameAsync(userDto.Username);
                if (existingUser != null && existingUser.Id != id)
                {
                    return BadRequest(new { error = "Username is already taken" });
                }
                
                user.UserName = userDto.Username;
            }
            
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            // Update roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            foreach (var role in userDto.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            // Update UserType based on new roles
            user.UserType = DetermineUserType(userDto.Roles);
            await _userManager.UpdateAsync(user);

            // Handle account activation/deactivation
            if (userDto.IsActive)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, System.DateTimeOffset.MaxValue);
            }

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // Helper method to determine UserType based on roles
        private UserType DetermineUserType(List<string> roles)
        {
            if (roles.Contains("Admin"))
                return UserType.Admin;
            else if (roles.Contains("Provider"))
                return UserType.Provider;
            else
                return UserType.Customer;
        }
    }

    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
} 