using Microsoft.AspNetCore.Http;
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

        [HttpPost("login")] // returns a token(UserId as PrimarySid, Id as Sid, Role) -> UserId = 0 for admin
        public async Task<ActionResult> Login(LoginDto dto) 
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
            if (user != null)
            {
                if (user.RoleId == 1)
                {
                    Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == user.Id);
                    var token = CreateToken(driver.UserId+"", driver.Id+"", "driver");
                    return Ok(token);

                }
                else
                {
                    Passenger passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.UserId == user.Id);
                    var token = CreateToken(passenger.UserId+"", passenger.Id+"", "passenger");
                    return Ok(token);
                }
            }
            else
            {
                Admin admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email && a.Password == dto.Password);
                if (admin != null)
                {
                    var token = CreateToken("0", admin.Id+"", "Admin");
                    return Ok(token);
                }
                else
                {
                    return NotFound("wrong email or password");
                }
            }
        }


        public static string CreateToken(String UserId, string id, String Role)
        {
            string secretKey = "sz8eI7OdHBrjrIo8j9jkloLnTHEIdovpdvecW/KrQymjuyhnO1OvY0pAQ2wDKQZw/0=";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, UserId),
                new Claim(ClaimTypes.Sid, id),
                new Claim(ClaimTypes.Role, Role)
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
        public static object GetDataFromToken(string token)
        {
            string secretKey = "sz8eI7OdHBrjrIo8j9jkloLnTHEIdovpdvecW/KrQymjuyhnO1OvY0pAQ2wDKQZw/0=";
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // Set to true to validate the signature of the token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,           // Set to true if you want to validate the issuer of the token
                ValidateAudience = true,        // Set to true if you want to validate the audience of the token
                ValidIssuer = "RideMe",          // Set the expected issuer name
                ValidAudience = "Riders",        // Set the expected audience name
                ValidateLifetime = true         // Set to true to validate the token's expiration time
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenException ex)
            {
                // Handle invalid token exceptions (e.g., expired token, invalid signature)
                Console.WriteLine("Invalid token: " + ex.Message);
                return null;
            }

            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            var UserIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid);
            var idClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            var RoleClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            var response = new
            {
                UserId = UserIdClaim.Value,
                id = idClaim.Value,
                Role = RoleClaim.Value,
            };

            return response;
        }
    }
}

// hashing -> will be added to AddDriver, AddPassenger, AddAdmin???? & login
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