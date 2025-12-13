using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Infrastructure.RequestDTOs;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.User;
using API.Services;
using Common.Entities.BEntities;
using Common.Entities.Other;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : BaseController<User, UsersService, UserRequest, UserGetRequest, UserPostResponse>
{
    override protected void PopulateRequest(User entity, UserRequest request)
    {

        entity.Username = request.Username;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Email = request.Email;
        entity.Password = request.Password;
        entity.MembershipId = 2;
    }
    override protected void PopulateResponse(User entity, UserPostResponse response)
    {
        response.Username = entity.Username;
        response.FirstName = entity.FirstName;
        response.LastName = entity.LastName;
        response.Email = entity.Email;
        response.MembershipId = entity.MembershipId;
    }

    override protected Expression<Func<User, bool>> GetFilter(UserGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }

        var filter = request.Filter;

        Expression<Func<User, bool>> predicate =
            e => (string.IsNullOrEmpty(filter.Username) ||
                    (e.Username != null && e.Username.Contains(filter.Username))) &&

                (string.IsNullOrEmpty(filter.Email) ||
                    (e.Email != null && e.Email.Contains(filter.Email))) &&

                (string.IsNullOrEmpty(filter.FirstName) ||
                    (e.FirstName != null && e.FirstName.Contains(filter.FirstName))) &&

                (string.IsNullOrEmpty(filter.LastName) ||
                    (e.LastName != null && e.LastName.Contains(filter.LastName)))
            ;

        return predicate;
    }

    [HttpPost]
    [Route("{userId}/changeMembership")]
    public async Task<IActionResult> ChangeMembership([FromRoute] int userId, [FromQuery] int membershipId)
    {
        if (membershipId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(membershipId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid Id!" }
                }
            }));
        }

        MembershipService membershipService = new MembershipService();
        if (membershipService.GetById(membershipId) == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(membershipId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No Membership with this Id!" }
                }
            }));
        }

        UsersService service = new UsersService();
        User user = await service.GetById(userId);

        if (user == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(membershipId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No User with this Id!" }
                }
            }));
        }

        user.MembershipId = membershipId;

        await service.Update(user);

        UserPostResponse userResponse = new UserPostResponse();
        PopulateResponse(user, userResponse);

        return Ok(ResultSetGenerator<UserPostResponse>.Success(userResponse));
    }

    [HttpPost]
    [Route("{userId}/bookAppointment")]
    public async Task<IActionResult> BookAppointment([FromRoute] int userId, [FromQuery] int trainingSessionId)
    {
        if (userId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(userId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid User Id!" }
                }
            }));
        }

        if (trainingSessionId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(trainingSessionId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid TrainingSession Id!" }
                }
            }));
        }

        TrainingSessionService trainingSessionService = new TrainingSessionService();
        if (trainingSessionService.GetById(trainingSessionId) == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(trainingSessionId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No Training seassion with this Id!" }
                }
            }));
        }

        UsersService service = new UsersService();
        User user = await service.GetById(userId);

        if (user == null)
        {
            return Unauthorized(ResultSetGenerator<int>.Failure(trainingSessionId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "User",
                    Mesages = new List<string> { "User must be logged in!" }
                }
            }));
        }

        TrainingSession trainingSession = await trainingSessionService.GetById(trainingSessionId);

        if (trainingSession.Users == null)
        {
            trainingSession.Users = new List<User>();
        }

        trainingSession.Users.Add(user);
        await trainingSessionService.Update(trainingSession);

        return Ok(ResultSetGenerator<TrainingSession>.Success(trainingSession));
    }

    [HttpPut]
    [Route("{userId}/addRole")]
    public async Task<IActionResult> AddRole([FromRoute] int userId, [FromQuery] int roleId)
    {
        if (userId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(userId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid User Id!" }
                }
            }));
        }
        if (roleId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(roleId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid Role Id!" }
                }
            }));
        }
        UsersService service = new UsersService();
        User user = await service.GetById(userId);
        if (user == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(userId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No User with this Id!" }
                }
            }));
        }
        RoleService roleService = new RoleService();
        Role role = await roleService.GetById(roleId);
        if (role == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(roleId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No Role with this Id!" }
                }
            }));
        }

        if (user.Roles == null)
        {
            user.Roles = new List<Role>();
        }

        UsersService usersService = new UsersService();
        await usersService.AddRoleAsync(userId, roleId);

        UserPostResponse userResponse = new UserPostResponse();
        PopulateResponse(user, userResponse);
        return Ok(ResultSetGenerator<UserPostResponse>.Success(userResponse));
    }
    
    [HttpPut]
    [Route("{userId}/removeRole")]
    public async Task<IActionResult> RemoveRole([FromRoute] int userId, [FromQuery] int roleId)
    {
        if (userId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(userId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid User Id!" }
                }
            }));
        }
        if (roleId <= 0)
        {
            return BadRequest(ResultSetGenerator<int>.Failure(roleId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "Invalid Role Id!" }
                }
            }));
        }
        UsersService service = new UsersService();
        User user = await service.GetById(userId);
        if (user == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(userId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No User with this Id!" }
                }
            }));
        }
        RoleService roleService = new RoleService();
        Role role = await roleService.GetById(roleId);
        if (role == null)
        {
            return NotFound(ResultSetGenerator<int>.Failure(roleId, new List<ResultSetError>
            {
                new ResultSetError
                {
                    Name = "Id",
                    Mesages = new List<string> { "No Role with this Id!" }
                }
            }));
        }

        if (user.Roles == null)
        {
            user.Roles = new List<Role>();
        }

        UsersService usersService = new UsersService();
        await usersService.RemoveRoleAsync(userId, roleId);
        
        UserPostResponse userResponse = new UserPostResponse();
        PopulateResponse(user, userResponse);
        return Ok(ResultSetGenerator<UserPostResponse>.Success(userResponse));
    }
}