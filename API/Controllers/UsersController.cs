using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.User;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
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
            e =>(string.IsNullOrEmpty(filter.Username) ||
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
}
