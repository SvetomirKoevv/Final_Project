using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.User;
using API.Services;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin, Administrator")]
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
        entity.Phone = request.Phone;
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
        return await ReturnFromValidationModel<int, UserPostResponse>(
            await new FullEntityRequestValidator<User, UsersService, Membership, MembershipService>()
                .Validate(userId, membershipId),
            new TwoIdsDelegate<UserPostResponse>(async (uId, mId) =>
            {
                UsersService usersService = new UsersService();
                await usersService.ChangeMembershipAsync(uId, mId);
                
                User user = await usersService.GetById(uId);
                UserPostResponse userResponse = new UserPostResponse();
                
                PopulateResponse(user, userResponse);
                return ResultSetGenerator<UserPostResponse>.Success(userResponse);
            }));
    }

    [HttpPost]
    [Route("{userId}/bookAppointment")]
    public async Task<IActionResult> BookAppointment([FromRoute] int userId, [FromQuery] int trainingSessionId)
    {
        return await ReturnFromValidationModel<int, TrainingSession>(
            await new FullEntityRequestValidator<User, UsersService, TrainingSession, TrainingSessionService>()
                .Validate(userId, trainingSessionId),
            new TwoIdsDelegate<TrainingSession>(async (uId, tsId) =>
            {
                UsersService usersService = new UsersService();
                User user = await usersService.GetById(uId);

                TrainingSessionService trainingSessionService = new TrainingSessionService();
                TrainingSession trainingSession = await trainingSessionService.GetById(tsId);
                
                trainingSession.Users.Add(user);
                await trainingSessionService.Update(trainingSession);

                return ResultSetGenerator<TrainingSession>.Success(trainingSession);
            }));

    }

    [HttpPut]
    [Route("{userId}/addRole")]
    public async Task<IActionResult> AddRole([FromRoute] int userId, [FromQuery] int roleId)
    {
        return await ReturnFromValidationModel<int, UserPostResponse>(
            await new FullEntityRequestValidator<User, UsersService, Role, RoleService>()
                .Validate(userId, roleId),
            new TwoIdsDelegate<UserPostResponse>(async (uId, rId) =>
            {
                UsersService usersService = new UsersService();
                await usersService.AddRoleAsync(userId, roleId);

                User user = await usersService.GetById(userId);
                UserPostResponse userResponse = new UserPostResponse();
                PopulateResponse(user, userResponse);
                return ResultSetGenerator<UserPostResponse>.Success(userResponse);
            }));
    }
    
    [HttpPut]
    [Route("{userId}/removeRole")]
    public async Task<IActionResult> RemoveRole([FromRoute] int userId, [FromQuery] int roleId)
    {
        return await ReturnFromValidationModel<int, UserPostResponse>(
            await new FullEntityRequestValidator<User, UsersService, Role, RoleService>()
                .Validate(userId, roleId),
            new TwoIdsDelegate<UserPostResponse>(async (uId, rId) =>
            {
                UsersService usersService = new UsersService();
                await usersService.RemoveRoleAsync(userId, roleId);
                
                User user = await usersService.GetById(userId);
                UserPostResponse userResponse = new UserPostResponse();
                
                PopulateResponse(user, userResponse);
                return ResultSetGenerator<UserPostResponse>.Success(userResponse);
            }));
    }
}