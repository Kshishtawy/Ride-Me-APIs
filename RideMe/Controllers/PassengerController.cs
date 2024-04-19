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
    public class PassengerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
