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

        [HttpGet("get-filtered-drivers")]
        public async Task<ActionResult> GetFilteredDrivers(FilterDriversDto dto)
        {
            // get all drivers
            var drivers = await _context.Drivers.Include(d => d.User).Include(d => d.City)
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
    }
}
