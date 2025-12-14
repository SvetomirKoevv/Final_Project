using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Role;
using API.Infrastructure.ResponseDTOs.Role;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : BaseController<Role, RoleService, RoleRequest, RoleGetRequest, RolePostResponse>
{
    override protected void PopulateRequest(Role entity, RoleRequest request)
    {
        entity.Name = request.Name;
    }

    override protected void PopulateResponse(Role entity, RolePostResponse response)
    {
        response.Id = entity.Id;
        response.Name =  entity.Name;
    }

    override protected Expression<Func<Role, bool>> GetFilter(RoleGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }

        Expression<Func<Role, bool>> filter =
            e => (string.IsNullOrEmpty(request.Filter.Name) ||
                    (e.Name != null && e.Name.Contains(request.Filter.Name)))
            ;

        return filter;
    }
}
