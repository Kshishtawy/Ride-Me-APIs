using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Models;
using RideMe.Dtos;

namespace RideMe.Controllers
{
    [Route("api/Driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // The api gets all requested for a driver using the driver id
        [HttpGet("get-requested-ride/{driverId}")]
        public async Task<IActionResult> GetRequestedRideAsync(int driverId)
        {
            var rides = await _context.Rides
                .Where(r => r.DriverId == driverId && r.StatusId == 1)
                .Select(r => new
                {
                    Ride = r,
                    PassengerName = _context.Users.FirstOrDefault(u => u.Id == r.Passenger.UserId).Name,
                    PassengerPhone = _context.Users.FirstOrDefault(u => u.Id == r.Passenger.UserId).PhoneNumber
                })
                .ToListAsync();

            return Ok(rides);
        }

        // takes ride id and gets its status
        [HttpGet("get-ride-status/{rideId}")]
        public async Task<IActionResult> GetRideStatusAsync(int rideId)
        {
            var rides = await _context.Rides
                .Include(r => r.Status)
                .Where(r => r.Id == rideId)
                .Select(r => new
                {
                    rideId = r.Id,
                    rideStatus = r.Status.Name

                })
                .FirstOrDefaultAsync();

            if (rides == null)
                return NotFound("Invalid ride Id");

            return Ok(rides);
        }

        // Aya apis



        [HttpGet("get-current-ride-status/{DriverId}")]
        public async Task<ActionResult> GetCurrentRideStatus(int DriverId)
        {
            var rides = await _context.Rides.Include(r => r.Driver).Include(r => r.Passenger).Include(r => r.Status)
                .Where(r => r.DriverId == DriverId && r.StatusId == 3)
                .Select(r => new
                {
                    RideId = r.Id,
                    Driver = r.Driver.User.Name,
                    Passenger = r.Passenger.User.Name,
                    PassengerPhoneNumber = r.Passenger.User.PhoneNumber,
                    Source = r.RideSource,
                    Destination = r.RideDestination,
                    Status = r.Status.Name,
                    Price = r.Price,
                    Rating = r.Rating,
                    Feedback = r.Feedback,
                    Date = r.RideDate
                })
                .ToListAsync();
            return Ok(rides);
        }

        [HttpGet("get-driver-daily-income")]
        public async Task<ActionResult> GetDriversDailyIncome(DailyIncomeDto dto)
        {
            DateOnly date = DateOnly.Parse(dto.DateString);
            var DriverRides = await _context.Rides.Where(r => (r.DriverId == dto.DriverId) && (r.RideDate.Day == date.Day) && (r.RideDate.Month == date.Month) && (r.RideDate.Year == date.Year)).ToListAsync();
            double income = 0;
            foreach (var ride in DriverRides)
            {
                income += (double)ride.Price;
            }
            return Ok(income);
        }

        [HttpGet("get-driver-monthly-income")]
        public async Task<ActionResult> GetDriversMonthlyIncome(MonthlyIncomeDto dto)
        {
            var DriverRides = await _context.Rides.Where(r => (r.DriverId == dto.DriverId)).ToListAsync();
            double income = 0;
            foreach (var ride in DriverRides)
            {
                if (ride.RideDate.Month == dto.Month)
                    income += (double)ride.Price;
            }
            return Ok(income);
        }

        [HttpPut("available/{id}")]
        public async Task<ActionResult> Available(int id)
        {
            Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null)
                return NotFound("wrong id");
            driver.Available = true;
            _context.SaveChanges();
            return Ok(driver);
        }

        [HttpPut("not-available/{id}")]
        public async Task<ActionResult> NOtAvailable(int id)
        {
            Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null)
                return NotFound("wrong id");
            driver.Available = false;
            _context.SaveChanges();
            return Ok(driver);
        }

        [HttpPut("accept-ride/{id}")]
        public async Task<ActionResult> AcceptRide(int id)
        {
            // validation
            Ride ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);
            if (ride == null)
                return NotFound("wrong ride id");

            // accepting ride
            ride.StatusId = 3;
            _context.SaveChanges();

            // rejecting other requested rides
            Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == ride.DriverId);
            var otherRides = await _context.Rides.Where(r => r.DriverId == ride.DriverId && r.StatusId == 1).ToListAsync();
            foreach (var otherRide in otherRides)
            {
                otherRide.StatusId = 2;
            }
            _context.SaveChanges();

            // changing driver to not available
            driver.Available = false;
            _context.SaveChanges();

            // returning the ride
            var response = await _context.Rides.Include(r => r.Driver).Include(r => r.Passenger).Include(r => r.Status)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    RideId = r.Id,
                    Driver = r.Driver.User.Name,
                    Passenger = r.Passenger.User.Name,
                    Source = r.RideSource,
                    Destination = r.RideDestination,
                    Status = r.Status.Name,
                    Price = r.Price,
                })
                .FirstOrDefaultAsync();
            return Ok(response);
        }

        [HttpPut("reject-ride/{id}")]
        public async Task<ActionResult> RejectRide(int id)
        {
            Ride ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);
            if (ride == null)
                return NotFound("wrong id");
            ride.StatusId = 2;
            _context.SaveChanges();
            return Ok(ride);
        }
    }
}
