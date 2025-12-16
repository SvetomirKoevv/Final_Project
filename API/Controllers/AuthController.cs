using API.Infrastructure.RequestDTOs.Auth;
using API.Infrastructure.ResponseDTOs.Auth;
using API.Services;
using Common.Entities.BEntities;
using Common.Entities.Other;
using Common.Persistance;
using Common.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        ValidationResult result = new RegisterRequestValidator().Validate(model);
        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<RegisterRequest>.Failure(model, result));
        }

        UsersService service = new UsersService();
        if (service.GetAll()
            .Any(u => u.Username == model.Username))
        {
            return Conflict(ResultSetGenerator<RegisterRequest>.Failure(model, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Username",
                    Mesages = new List<string> { "Username already exists." }
                }
            }));
        }

        User user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            Email = model.Email,
            Phone = model.Phone,
            Password = model.Password,
        };

        service.Create(user);
        AuthService tokenService = new AuthService();

        string token = await tokenService.Register(user);
        return Ok(ResultSetGenerator<AuthResponse>.Success(new AuthResponse
        {
            Token = token
        }));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest model)
    {
        ValidationResult result = new AuthRequestValidator().Validate(model);
        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<AuthRequest>.Failure(model, result));
        }
        
        UsersService service = new UsersService();
        User user = (service.GetAll("Roles"))
            .FirstOrDefault(u => u.Username == model.Username);

        if (user == null)
        {
            return Unauthorized(ResultSetGenerator<AuthRequest>.Failure(model, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Authentication",
                    Mesages = new List<string> { "Invalid username." }
                }
            }));
        }
        
        AuthService authService = new AuthService();
        string token = await authService.Login(user, model.Password);
        if (token == null)
        {
            return Unauthorized(ResultSetGenerator<AuthRequest>.Failure(model, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Authentication",
                    Mesages = new List<string> { "Invalid password for specified username." }
                }
            }));
        }
        return Ok(ResultSetGenerator<AuthResponse>.Success(new AuthResponse
        {
            Token = token
        }));
    }
}