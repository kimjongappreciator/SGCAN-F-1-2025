using hub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hub.Controllers;

[ApiController]
public class UserController :ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return Conflict("El correo electrónico ya está registrado.");
        }

        user.Id = Guid.NewGuid();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        
        return Ok(new { UserId = user.Id, Message = "Usuario registrado correctamente." });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
       
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null)
        {
            return Unauthorized("Correo electrónico o contraseña incorrectos.");
        }
        
        var match = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
        
        if (!match)
        {
            return Unauthorized("Correo electrónico o contraseña incorrectos.");
        }
        
        return Ok(new { Message = "Login exitoso.", UserId = user.Id, name = user.Name });
    }
}