using GameNest_BackEnd.Data;
using GameNest_BackEnd.DTOs.Auth;
using GameNest_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameNest_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GameNestDbContext _context;

    public AuthController(GameNestDbContext context)
    {
        _context = context;
    }
    private string CreateToken(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
            HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()["Jwt:Key"]!
        )
    );

    var credentials = new SigningCredentials(
        key,
        SecurityAlgorithms.HmacSha256
    );

    var token = new JwtSecurityToken(
        issuer: HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["Jwt:Issuer"],

        audience: HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["Jwt:Audience"],

        claims: claims,

        expires: DateTime.UtcNow.AddHours(1),

        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Перевіряємо, чи Username або Email вже зайняті
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Username == dto.Username ||
                u.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest("Username або Email вже використовується.");
        }

        // Хешуємо пароль
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // Створюємо користувача
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Користувача успішно зареєстровано."
        });
    }
    [HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (user == null)
    {
        return Unauthorized("Неправильний email або пароль.");
    }

    var passwordIsValid = BCrypt.Net.BCrypt.Verify(
        dto.Password,
        user.PasswordHash
    );

    if (!passwordIsValid)
    {
        return Unauthorized("Неправильний email або пароль.");
    }

    var token = CreateToken(user);

    return Ok(new
    {
        token = token
    });
    }
}