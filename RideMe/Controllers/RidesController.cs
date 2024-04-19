using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;

namespace RideMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RidesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("request-ride")]
        public async Task<IActionResult> addRidesAsync([FromBody] RequestRideDto dto)
        {
            // Validating both passenger and driver id
            var passenger = await _context.Passengers.FindAsync(dto.PassengerId);
            if(passenger == null)
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
                RideDistention = dto.RideDistention,
                StatusId = requestedStatus.Id,
                Price = dto.Price,
                Rating = -1,
                Feedback = "",
                RideDate = DateOnly.FromDateTime(DateTime.Today)
            };

            await _context.AddAsync(ride);
            _context.SaveChanges();

            return Ok(new
            {
                ride.Id,
                ride.PassengerId,
                ride.DriverId,
                ride.RideSource,
                ride.RideDistention,
                ride.StatusId,
                ride.Price,
                ride.RideDate
            });
        }

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
    }
}
