using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;

namespace RideMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-rides")]
        /* The API will get all rides from table and frontend will use them to
         * Show details of the ride and use the "status" show completed or not
        */
        public async Task<IActionResult> GetAllRidesAsync()
        {
            var rides = await _context.Rides.ToListAsync();

            return Ok(rides);
        }

        [HttpGet("get-all-drivers")]
        /* Same as the previous api, the api will return all values of the table
         * and the front will utilize only the field they want => (avgRating field)
        */
        public async Task<IActionResult> GetAllDriversAsync()
        {
            var drivers = await _context.Drivers.ToListAsync();

            return Ok(drivers);
        }

        [HttpPut("block-driver/{id}")]
        // Enter Driver ID to block. Returns 404 if id doesnt exist
        public async Task<IActionResult> blockDriverByIdAsync(int id)
        {
            var driver = await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (driver == null)
                return NotFound($"No driver was found with id: {id}");

            /* The following piece of code avoids hard-coding
             * Instead of putting 4 for blocked, the code searches
             * for the status named "blocked" and gets it's id
             */
            var blockedStatus = await _context.UserStatuses.FirstOrDefaultAsync(s => s.Name == "blocked");

            if (blockedStatus == null)
                return NotFound("Blocked status not found in the database.");

            driver.User.StatusId = blockedStatus.Id;

            // This will return a driver with its status
            var response = new
            {
                driver.Id,
                driver.User.Username, // Change this to name later
                driver.User.StatusId
            };

            await _context.SaveChangesAsync();

            return Ok(response);
        }

        [HttpPut("unblock-driver/{id}")]
        // Enter Driver ID to block. Returns 404 if id doesnt exist
        public async Task<IActionResult> unblockDriverByIdAsync(int id)
        {
            var driver = await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (driver == null)
                return NotFound($"No driver was found with id: {id}");

            /* The following piece of code avoids hard-coding
             * Instead of putting 4 for blocked, the code searches
             * for the status named "blocked" and gets it's id
             */
            var blockedStatus = await _context.UserStatuses.FirstOrDefaultAsync(s => s.Name == "accepted");

            if (blockedStatus == null)
                return NotFound("Blocked status not found in the database.");

            driver.User.StatusId = blockedStatus.Id;

            // This will return a driver with its status
            var response = new
            {
                driver.Id,
                driver.User.Username, // Change this to name later
                driver.User.StatusId
            };

            await _context.SaveChangesAsync();

            return Ok(response);
        }


    }
}
