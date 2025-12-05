using API.Infrastructure.RequestDTOs;
using API.Infrastructure.ResponseDTOs.User;
using API.Services;
using Common.Entities.BEntities;
using Common.Entities.Other;
using Common.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _service;
    public UsersController()
    {
        _service = new UsersService();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var items = _service.GetAll();

        return Ok(ResultSetGenerator<List<User>>.Success(items));
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var item = _service.GetById(id);

        if (item == null)
        {
            return Ok(ResultSetGenerator<User>
                        .Failure(
                            item,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = "User",
                                    Mesages = new List<string>() { "User not found" }
                                }
                            }));
        }

        return Ok(ResultSetGenerator<User>.Success(item));
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserRequest model)
    {
        ValidationResult result = ValidationService<UserRequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<UserRequest>.Failure(model, result));
        }

        User item = new User
        {
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            MembershipId = 2
        };

        _service.Create(item);

        UserPostResponse responseItem = new UserPostResponse
        {
            Username = item.Username,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.Email,
            MembershipId = item.MembershipId
        };

        return Ok(ResultSetGenerator<UserPostResponse>.Success(responseItem));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Post([FromQuery] int id, [FromBody] UserRequest model)
    {
        ValidationResult result = ValidationService<UserRequest>.Validate(model);

        if (!result.IsValid)
        {
            return BadRequest(ResultSetGenerator<UserRequest>.Failure(model, result));
        }

        User item = new User
        {
            Id = id,
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
        };

        _service.Update(item);

        UserPostResponse responseItem = new UserPostResponse
        {
            Username = item.Username,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.Email,
            MembershipId = item.MembershipId
        };

        return Ok(ResultSetGenerator<UserPostResponse>.Success(responseItem));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 0)
            return BadRequest(ResultSetGenerator<int>
                        .Failure(
                            id,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = "Id",
                                    Mesages = new List<string>
                                    {
                                        "Invalid Id!"
                                    }
                                }
                            }));

        var selectedUser = _service.GetById(id);
        if (selectedUser == null)
            return BadRequest(ResultSetGenerator<int>
                        .Failure(
                            id,
                            new List<ResultSetError>
                            {
                                new ResultSetError
                                {
                                    Name = "Id",
                                    Mesages = new List<string>
                                    {
                                        "No User with this id!"
                                    }
                                }
                            }));

        var deletedUser = _service.Delete(id);

        return Ok(ResultSetGenerator<User>.Success(deletedUser));
    }
}
