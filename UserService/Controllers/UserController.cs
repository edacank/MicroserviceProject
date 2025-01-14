using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Models;
using BCrypt.Net;
using System.Linq;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        // 1. Kullanıcı Ekleme (Register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
            {
                return BadRequest("Kullanıcı adı zaten var.");
            }

            // Şifreyi hashle ve kullanıcıyı ekle
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayıt başarılı.");
        }

        // 2. Kullanıcı Giriş (Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            // JWT Oluşturma
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
        // 3. Kullanıcıları Listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // 4. Kullanıcı Güncelleme


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles) // UserRoles ile birlikte yükleme
                .ThenInclude(ur => ur.Role) // Role ile birlikte yükleme
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            // Kullanıcı bilgilerini güncelle
            user.UserName = updatedUser.UserName;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);
            }

            // Kullanıcı rollerini güncelle
            var existingRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var updatedRoleIds = updatedUser.UserRoles.Select(ur => ur.RoleId).ToList();

            // Yeni eklenen roller
            var rolesToAdd = updatedRoleIds.Except(existingRoleIds)
                .Select(roleId => new UserRole { UserId = user.UserId, RoleId = roleId });
            _context.UserRoles.AddRange(rolesToAdd);

            // Kaldırılması gereken roller
            var rolesToRemove = user.UserRoles.Where(ur => !updatedRoleIds.Contains(ur.RoleId)).ToList();
            _context.UserRoles.RemoveRange(rolesToRemove);

            await _context.SaveChangesAsync();
            return Ok("Kullanıcı güncellendi.");
        }





        // 5. Kullanıcı Silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Kullanıcı silindi.");
        }

        // JWT Token Oluşturma
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BuCokGucluBirGizliAnahtar"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName)
    };
            claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
