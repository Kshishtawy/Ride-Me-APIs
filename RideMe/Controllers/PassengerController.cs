using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;

namespace RideMe.Controllers
{
    [Route("api/Passenger")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-car-types")]
        public async Task<ActionResult> GetCarTypes()
        {
            var carTypes = await _context.Drivers
                .Select(d => new
                {
                    d.CarType
                })
                .Distinct()
                .ToListAsync();
            return Ok(carTypes);
        }

        [HttpGet("get-cities")]
        public async Task<ActionResult> GetCities()
        {
            var drivers = await _context.Cities
                .Select(c => new
                {
                    c.Name
                })
                .ToListAsync();
            return Ok(drivers);
        }

        [HttpGet("get-filtered-drivers")]
        public async Task<ActionResult> GetFilteredDrivers(FilterDriversDto dto)
        {
            // get all available drivers
            var drivers = await _context.Drivers.Include(d => d.User).Include(d => d.City)
            .Where(d => d.Available == true)
            .Select(d => new
            {
                Name = d.User.Name,
                PhoneNumber = d.User.PhoneNumber,
                Status = d.User.Status.Name,
                CarType = d.CarType,
                IsSmoking = d.Smoking,
                City = d.City.Name,
                Region = d.Region,
                Isavailable = d.Available,
                Rating = d.AvgRating
            })
            .ToListAsync();

            // filter drivers
            if (dto.CarType is not null)
            {
                drivers = drivers.Where(d => d.CarType == dto.CarType).ToList();
            }
            if (dto.Smoking is not null)
            {
                drivers = drivers.Where(d => d.IsSmoking == dto.Smoking).ToList();
            }
            if (dto.City is not null)
            {
                drivers = drivers.Where(d => d.City == dto.City).ToList();
            }
            return Ok(drivers);
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


        [HttpPost("request-ride")]
        public async Task<IActionResult> addRidesAsync([FromBody] RequestRideDto dto)
        {
            // Validating both passenger and driver id
            var passenger = await _context.Passengers.FindAsync(dto.PassengerId);
            if (passenger == null)
                return NotFound($"No passenger with id: {dto.PassengerId}");

            var driver = await _context.Drivers.FindAsync(dto.DriverId);
            if (driver == null)
                return NotFound($"No driver with id: {dto.DriverId}");

            var requestedStatus = await _context.RideStatuses
                .FirstOrDefaultAsync(s => s.Name == "requested");

            var ride = new Ride
            {
                PassengerId = dto.PassengerId,
                DriverId = dto.DriverId,
                RideSource = dto.RideSource,
                RideDestination = dto.RideDestination,
                StatusId = requestedStatus.Id,
                Price = dto.Price,
                Rating = -1,
                Feedback = "",
                RideDate = DateTime.Today
            };

            await _context.AddAsync(ride);
            _context.SaveChanges();

            return Ok(new
            {
                ride.Id,
                ride.PassengerId,
                ride.DriverId,
                ride.RideSource,
                ride.RideDestination,
                ride.StatusId,
                ride.Price,
                ride.RideDate
            });
        }


        [HttpPut("confirm-payment/{id}")]
        public async Task<ActionResult> ConfirmPayment(int id)
        {
            // make the ride completed
            Ride ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);
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

        /*
        [HttpDelete("cancel-ride/{id}")]
        public async Task<IActionResult> cancelRideAsync(int id)
        {
            var ride = await _context.Rides.FindAsync(id);

            if (ride == null)
                return NotFound($"No ride was found with id: {id}");

            _context.Remove(ride);
            _context.SaveChanges();

            return Ok(ride);
        }
        */
    }
}
