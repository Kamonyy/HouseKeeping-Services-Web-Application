using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using HousekeepingAPI.Data;

namespace HousekeepingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostEnvironment _env;

        public SeedController(IServiceProvider serviceProvider, IHostEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            if (!_env.IsDevelopment())
            {
                return Forbid("Seeding is only allowed in development mode.");
            }

            await SeedData.Initialize(_serviceProvider);
            return Ok("Database seeded successfully.");
        }
    }
} 