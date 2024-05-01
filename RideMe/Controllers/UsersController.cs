using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RideMe.Data;
using RideMe.Dtos;
using RideMe.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            User userCheck = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (userCheck != null)
                return BadRequest("this email already exists");

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
            User userCheck = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (userCheck != null)
                return BadRequest("this email already exists");

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
                if (user.RoleId == 1)
                {
                    var driverDto = await _context.Drivers.Include(d => d.User)
                        .Where(d => d.UserId == user.Id)
                        .Select(d => new DriverTokenDto
                        {
                            Id = d.Id,
                            UserId = d.UserId,
                            Email = d.User.Email,
                            Name = d.User.Name,
                            Role = d.User.Role.Name,
                            Status = d.User.Status.Name,
                            PhoneNumber = d.User.PhoneNumber,
                            CarType = d.CarType,
                            Smoking = d.Smoking,
                            City = d.City.Name,
                            Region = d.Region,
                            Available = d.Available,
                            Rating = d.AvgRating
                        })
                        .FirstOrDefaultAsync();
                    var token = CreateDriverToken(driverDto);
                    return Ok(token);

                }
                else
                {
                    var passengerDto = await _context.Passengers.Include(p => p.User)
                        .Where(p => p.UserId == user.Id)
                        .Select(p => new PassengerTokenDto
                        {
                            Id = p.Id,
                            UserId = p.UserId,
                            Email = p.User.Email,
                            Name = p.User.Name,
                            Role = p.User.Role.Name,
                            Status = p.User.Status.Name,
                            PhoneNumber = p.User.PhoneNumber,
                        })
                        .FirstOrDefaultAsync();
                    var token = CreatePassengerToken(passengerDto);
                    return Ok(token);
                }
            }
            else
            {
                Admin admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email && a.Password == dto.Password);
                if (admin != null)
                {
                    var token = CreateAdminToken(admin);
                    return Ok(token);
                }
                else
                {
                    return NotFound("wrong email or password");
                }
            }
        }


        public static string CreateAdminToken(Admin admin)
        {
            string secretKey = "sz8eI7OdHBrjrIo8j9jkloLnTHEIdovpdvecW/KrQymjuyhnO1OvY0pAQ2wDKQZw/0=";
            var claims = new List<Claim>
            {
                new Claim("Id", admin.Id.ToString()),
                new Claim("Role", "admin"),
                new Claim("Email", admin.Email),
                new Claim("Name", admin.Name),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: "RideMe",
                audience: "Riders",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string CreateDriverToken(DriverTokenDto driver)
        {
            string secretKey = "sz8eI7OdHBrjrIo8j9jkloLnTHEIdovpdvecW/KrQymjuyhnO1OvY0pAQ2wDKQZw/0=";
            var claims = new List<Claim>
            {
                new Claim("UserId", driver.UserId.ToString()),
                new Claim("Id", driver.Id.ToString()),
                new Claim("Role", driver.Role),
                new Claim("Name", driver.Name),
                new Claim("Email", driver.Email),
                new Claim("Status", driver.Status),
                new Claim("Smoking", driver.Smoking.ToString()),
                new Claim("Available", driver.Available.ToString()),
                new Claim("City", driver.City),
                new Claim("Region", driver.Region),
                new Claim("CarType", driver.CarType),
                new Claim("PhoneNumber", driver.PhoneNumber),
                new Claim("Rating", driver.Rating.ToString())

             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: "RideMe",
                audience: "Riders",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string CreatePassengerToken(PassengerTokenDto passenger)
        {
            string secretKey = "sz8eI7OdHBrjrIo8j9jkloLnTHEIdovpdvecW/KrQymjuyhnO1OvY0pAQ2wDKQZw/0=";
            var claims = new List<Claim>
            {
                new Claim("UserId", passenger.UserId.ToString()),
                new Claim("Id", passenger.Id.ToString()),
                new Claim("Role", passenger.Role),
                new Claim("Name", passenger.Name),
                new Claim("Email", passenger.Email),
                new Claim("Status", passenger.Status),
                new Claim("PhoneNumber", passenger.PhoneNumber),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: "RideMe",
                audience: "Riders",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

// hashing -> will be added to AddDriver, AddPassenger, AddAdmin & login
/*
string data = "This is some data to hash";
byte[] dataBytes = Encoding.UTF8.GetBytes(data);

string hashedData; string hashedData2;

using (var sha256 = SHA256.Create())
{
    byte[] hash = sha256.ComputeHash(dataBytes);
    // Convert hash to a string representation (e.g., hexadecimal)
    hashedData = Convert.ToHexString(hash);
    Console.WriteLine(hashedData);
}

string data2 = "This is some data to hash";
byte[] dataBytes2 = Encoding.UTF8.GetBytes(data);

using (var sha256 = SHA256.Create())
{
    byte[] hash2 = sha256.ComputeHash(dataBytes);
    // Convert hash to a string representation (e.g., hexadecimal)
    hashedData2 = Convert.ToHexString(hash2);
    Console.WriteLine(hashedData2);
}

bool areHashesEqual = true;
for (int i = 0; i < hashedData.Length; i++)
{
    if (hashedData[i] != hashedData2[i])
    {
        areHashesEqual = false;
        break;
    }
}

if (areHashesEqual)
{
    Console.WriteLine("Hashes are equal!");
}
else
{
    Console.WriteLine("Hashes are different!");
}
*/