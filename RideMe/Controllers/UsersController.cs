using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;

namespace RideMeDB.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-cities")]
        public async Task<ActionResult> getCities()
        {
            var cities = await _context.Cities
                .Select(d => new
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
            return Ok(cities);
        }

        [HttpPost("add-driver")]
        public async Task<ActionResult> AddDriver(AddDriverDto dto)
        {
            User user = new User
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = 1,
                StatusId = 1
            };
            await _context.AddAsync(user);
            _context.SaveChanges();

            Driver driver = new Driver
            {
                UserId = user.Id,
                CityId = dto.CityId,
                Region = dto.Region,
                CarType = dto.CarType,
                Smoking = dto.Smoking,
                Available = false
            };

            await _context.AddAsync(driver);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("add-passenger")]
        public async Task<ActionResult> AddPassenger(AddPassengerDto dto)
        {
            User user = new User
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = 2,
                StatusId = 1
            };

            await _context.AddAsync(user);
            _context.SaveChanges();

            Passenger passenger = new Passenger
            {
                UserId = user.Id
            };

            await _context.AddAsync(passenger);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
            if (user != null)
            {
                if (user.RoleId == 1) // driver
                {
                    var drivers = await _context.Drivers.Include(d => d.User)
                    .Where(d => d.UserId == user.Id)
                    .Select(d => new
                    {
                        UserId = d.UserId,
                        DriverId = d.Id,
                        Role = d.User.Role.Name,
                        Name = d.User.Name,
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
                else // passenger
                {
                    var passengers = await _context.Passengers.Include(p => p.User)
                    .Where(p => p.UserId == user.Id)
                    .Select(p => new
                    {
                        UserId = p.UserId,
                        Name = p.User.Name,
                        Email = p.User.Email,
                        Status = p.User.Status.Name,
                        Role = p.User.Role.Name
                    })
                    .ToListAsync();
                    return Ok(passengers);
                }
            }
            else
            {
                Admin admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email && a.Password == dto.Password);
                if (admin != null)
                {
                    var Response = new
                    {
                        Id = admin.Id,
                        Name = admin.Name,
                        Email = admin.Email,
                        Role = "Admin"
                    };
                    return Ok(Response);
                }
                else
                {
                    return NotFound("Invalid login credentials!");
                }
            }
        }
    }
}
