using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PropertyLeasingSystem.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PropertyLeasingSystem.Controllers
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var tenant = _context.Tenants
                .FirstOrDefault(t => t.Email == dto.Email);

            var staff = _context.StaffMembers
                .FirstOrDefault(s => s.Email == dto.Email);

            string role = "";
            string email = "";
            string name = "";
            int userId = 0;

            if (tenant != null)
            {
                role = "Tenant";
                email = tenant.Email;
                name = tenant.FullName;
                userId = tenant.TenantId;
            }
            else if (staff != null)
            {
                role = staff.RoleType;
                email = staff.Email;
                name = staff.FullName;
                userId = staff.StaffId;
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = role,
                email = email,
                name = name
            });
        }
    }
}