using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;

namespace RideMe.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-drivers")]
        public async Task<ActionResult> GetAllDrivers()
        {
            var drivers = await _context.Drivers.Include(d => d.User).Include(d => d.City)
                .Select(d => new
                {
                    Name = d.User.Name,
                    PhoneNumber = d.User.PhoneNumber,
                    Email = d.User.Email,
                    Status = d.User.Status.Name,
                    CarType = d.CarType,
                    IsSmoking = d.Smoking,
                    City = d.City.Name,
                    Region = d.Region,
                    Isavailable = d.Available,
                    Rating = d.AvgRating
                })
                .ToListAsync();
            return Ok(drivers);
        }

        [HttpGet("get-all-rides")]
        public async Task<ActionResult> GetAllRides()
        {
            var rides = await _context.Rides.Include(r => r.Status).Include(r => r.Passenger).Include(r => r.Driver)
                .Select(r => new
                {
                    RideId = r.Id,
                    DriverName = r.Driver.User.Name,
                    PassengerName = r.Passenger.User.Name,
                    RideSource = r.RideSource,
                    RideDestination = r.RideDestination,
                    Status = r.Status.Name,
                    Price = r.Price,
                    Rating = r.Rating,
                    Feedback = r.Feedback,
                    RideDate = r.RideDate
                })
                .ToListAsync();
            return Ok(rides);
        }

        [HttpGet("get-waiting-passengers")]
        public async Task<ActionResult> GetWaitingPassengers()
        {
            var passengers = await _context.Passengers.Include(p => p.User)
                .Where(p => p.User.StatusId == 1)
                .Select(p => new
                {
                    Name = p.User.Name,
                    Email = p.User.Email,
                    PhoneNumber = p.User.PhoneNumber,
                    Status = p.User.Status.Name
                })
                .ToListAsync();
            return Ok(passengers);
        }

        [HttpGet("get-waiting-drivers")]
        public async Task<ActionResult> GetWaitingDrivers()
        {
            var drivers = await _context.Drivers.Include(d => d.User)
            .Where(d => d.User.StatusId == 1)
            .Select(d => new
            {
                Name = d.User.Name,
                PhoneNumber = d.User.PhoneNumber,
                Email = d.User.Email,
                Status = d.User.Status.Name,
                CarType = d.CarType,
                IsSmoking = d.Smoking,
                City = d.City.Name,
                Region = d.Region,
                IsAvailable = d.Available
            })
            .ToListAsync();
            return Ok(drivers);
        }


        [HttpPost("add-role/{RName}")]
        public async Task<ActionResult> AddRole(String RName)
        {
            Role role = new Role
            {
                Name = RName,
            };
            await _context.AddAsync(role);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("add-city/{CName}")]
        public async Task<ActionResult> AddCity(String CName)
        {
            City city = new City
            {
                Name = CName,
            };
            await _context.AddAsync(city);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("add-admin")]
        public async Task<ActionResult> AddAdmin(AddPassengerDto dto)
        {
            Admin admin = new Admin
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };
            await _context.AddAsync(admin);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPut("accept-user/{id}")]
        public async Task<ActionResult> AcceptUser(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("wrong id");
            user.StatusId = 2;
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("reject-user/{id}")]
        public async Task<ActionResult> RejectUser(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("wrong id");
            user.StatusId = 3;
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("block-driver/{id}")]
        public async Task<ActionResult> BlockDriver(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("wrong id");
            user.StatusId = 4;
            _context.SaveChanges();
            return Ok(user);
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
                driver.User.Name,
                driver.User.Email,
                driver.User.StatusId
            };

            await _context.SaveChangesAsync();

            return Ok(response);
        }








        //        [HttpGet("get-all-rides")]
        /* The API will get all rides from table and frontend will use them to
         * Show details of the ride and use the "status" show completed or not
        */
        /*        public async Task<IActionResult> GetAllRidesAsync()
                {
                    var rides = await _context.Rides.ToListAsync();

                    return Ok(rides);
                }
        */

        //        [HttpGet("get-all-drivers")]
        /* Same as the previous api, the api will return all values of the table
         * and the front will utilize only the field they want => (avgRating field)
        */
        /*        public async Task<IActionResult> GetAllDriversAsync()
                {
                    var drivers = await _context.Drivers.ToListAsync();

                    return Ok(drivers);
                }
        */


        //        [HttpPut("block-driver/{id}")]
        // Enter Driver ID to block. Returns 404 if id doesnt exist
        /*        public async Task<IActionResult> blockDriverByIdAsync(int id)
                {
                    var driver = await _context.Drivers
                        .Include(d => d.User)
                        .FirstOrDefaultAsync(d => d.Id == id);

                    if (driver == null)
                        return NotFound($"No driver was found with id: {id}");
        */
        /* The following piece of code avoids hard-coding
         * Instead of putting 4 for blocked, the code searches
         * for the status named "blocked" and gets it's id
         */
        /*           var blockedStatus = await _context.UserStatuses.FirstOrDefaultAsync(s => s.Name == "blocked");

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
        */

    }
}
