using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;

namespace RideMe.Controllers
{
    [Authorize(Roles = "passenger")]
    [Route("api/Passenger")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        [HttpGet("get-filtered-drivers")]
        public async Task<IActionResult> getFilteredDriver([FromQuery] PassengerPreferenceDto dto)
        {
            var acceptedStatus = await _context.UserStatuses.FirstOrDefaultAsync(s => s.Name == "accepted");

            var drivers = await _context.Drivers
                .Include(d => d.User)
                .Where(d => d.Available == true &&
                            d.User.Status == acceptedStatus &&
                            d.CarType.ToLower() == (dto.CarType).ToLower() &&
                            d.CityId == dto.CityId &&
                            d.Region.ToLower() == (dto.Region).ToLower())
                .Select(d => new
                {
                    d.Id,
                    d.UserId,
                    d.CarType,
                    d.Smoking,
                    d.CityId,
                    d.Region,
                    d.Available,
                    d.AvgRating
                })
                .ToListAsync();

            if (drivers == null)
                return NotFound("No drivers found with selected preferences");

            return Ok(drivers);

        }
        */

        // Aya apis
        [Authorize(Roles = "passenger")]
        [HttpGet("get-passenger-ride-history/{PassengerId}")]
        public async Task<ActionResult> GetPassengerRideHistory(int PassengerId)
        {
            var rides = await _context.Rides.Include(r => r.Driver).Include(r => r.Passenger).Include(r => r.Status)
                .Where(r => r.PassengerId == PassengerId)
                .Select(r => new
                {
                    RideId = r.Id,
                    Driver = r.Driver.User.Name,
                    DriverPhoneNumber = r.Driver.User.PhoneNumber,
                    Passenger = r.Passenger.User.Name,
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

        [HttpGet("get-current-ride-status/{PassengerId}")]
        public async Task<ActionResult> GetCurrentRideStatus(int PassengerId)
        {
            var rides = await _context.Rides.Include(r => r.Driver).Include(r => r.Passenger).Include(r => r.Status)
                .Where(r => r.PassengerId == PassengerId && r.StatusId == 3 && r.RideDate.Date == DateTime.Now.Date)
                .Select(r => new
                {
                    RideId = r.Id,
                    Driver = r.Driver.User.Name,
                    DriverPhoneNumber = r.Driver.User.PhoneNumber,
                    Passenger = r.Passenger.User.Name,
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

        [HttpGet("get-available-drivers")]
        public async Task<ActionResult> GetAvailableDrivers()
        {
            var drivers = await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.City)
                .Where(d => d.Available == true)
                .Select(d => new
                {
                    id = d.Id,
                    name = d.User.Name,
                    car = d.CarType,
                    city = d.City.Name,
                    region = d.Region,
                    smoking = d.Smoking,
                    rating = d.AvgRating

                })
                .ToListAsync();

            if (drivers == null)
                return Ok("No avaiable drivers");


            return Ok(drivers);

        }

        [HttpGet("get-available-car-types")]
        public async Task<ActionResult> GetAvailableCarTypes()
        {
            var carTypes = await _context.Drivers
                .Where(d => d.Available == true)
                .Select(d => d.CarType) 
                .Distinct()
                .ToListAsync();

            return Ok(carTypes);
        }


        [HttpGet("get-filtered-drivers")]
        public async Task<ActionResult> GetFilteredDrivers(
            [FromQuery] string carType = null, // A defualt value of null is important
            [FromQuery] bool? smoking = null,
            [FromQuery] string city = null)
        {
            // Get all drivers
            var driversQuery = _context.Drivers
                .Include(d => d.User)
                .Include(d => d.City)
                .Where(d => d.Available == true)
                .Select(d => new
                {
                    id = d.Id,
                    name = d.User.Name,
                    car = d.CarType,
                    city = d.City.Name,
                    region = d.Region,
                    smoking = d.Smoking,
                    rating = d.AvgRating
                });

            // Apply filters
            if (!string.IsNullOrEmpty(carType))
            {
                driversQuery = driversQuery.Where(d => d.car == carType);
            }
            if (smoking.HasValue)
            {
                driversQuery = driversQuery.Where(d => d.smoking == smoking);
            }
            if (!string.IsNullOrEmpty(city))
            {
                driversQuery = driversQuery.Where(d => d.city == city);
            }

            // Execute the query
            var filteredDrivers = await driversQuery.ToListAsync();

            return Ok(filteredDrivers);
        }

        [HttpPost("rate-ride")]
        public async Task<ActionResult> addRatingAsync(RateAndFeedbackDto dto)
        {
            // make the ride completed
            Ride ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == dto.Id);
            if (ride == null)
                return NotFound("wrong id");

            if (dto.Rating < 0 || dto.Rating > 5)
                return BadRequest("Rating must be between 0 and 5");

            ride.Rating = dto.Rating;

            ride.Feedback = dto.Feedback;

            var response = new
            {
                id = ride.Id,
                rating = ride.Rating,
                feedback = ride.Feedback
            };

            _context.SaveChanges();
            return Ok(response);
        }

        [HttpPut("confirm-payment/{rideId}")]
        public async Task<ActionResult> ConfirmPayment(int rideId)
        {
            // make the ride completed
            Ride ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == rideId);
            if (ride == null)
                return NotFound("wrong id");
            ride.StatusId = 4;

            // make the driver available
            Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == ride.DriverId);
            if (driver == null)
                return NotFound("wrong id");
            driver.Available = true;

            _context.SaveChanges();
            return Ok(ride);
        }
    }
}
