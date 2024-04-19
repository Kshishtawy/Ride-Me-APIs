using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;

namespace RideMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // The api gets all requested for a driver using the driver id
        [HttpGet("get-requested-ride/{id}")]
        public async Task<IActionResult> GetRequestedRideAsync(int id)
        {
            var rides = await _context.Rides
                .Where(r => r.DriverId == id &&
                            r.StatusId == 1)
                .ToListAsync();

            return Ok(rides);
        }
    }
}
