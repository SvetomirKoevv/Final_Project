using API.Infrastructure.RequestDTOs.Auth;
using API.Infrastructure.ResponseDTOs.Auth;
using API.Services;
using Common.Entities.BEntities;
using Common.Entities.Other;
using Common.Persistance;
using Common.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] AuthRequest model)
    {
        ValidationResult result = new AuthRequestValidator().Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<AuthRequest>.Failure(model, result));
        }

        UsersService service = new UsersService();

        User user = (await service.GetAll("Roles"))
            .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        if (user == null)
        {
            return Unauthorized(ResultSetGenerator<AuthRequest>.Failure(model, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Authentication",
                    Mesages = new List<string> { "Invalid username or password." }
                }
            }));
        }

        TokenService tokenService = new TokenService();
        string token = tokenService.CreateToken(user);

        return Ok(ResultSetGenerator<AuthResponse>.Success(new AuthResponse
        {
            Token = token
        }));
    }
}