using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Membership;
using API.Infrastructure.ResponseDTOs.Membership;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator")]
[Route("api/[controller]")]
public class MembershipsController : BaseController<Membership, MembershipService, MembershipRequest, MembershipGetRequest, MembershipPostResponse>
{
    override protected void PopulateRequest(Membership entity, MembershipRequest request)
    {
        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.DurationDays = request.DurationDays;
    }

    override protected void PopulateResponse(Membership entity, MembershipPostResponse response)
    {
        response.Id = entity.Id;
        response.Name = entity.Name;
        response.Price = entity.Price;
        response.DurationDays = entity.DurationDays;
    }

    override protected Expression<Func<Membership, bool>> GetFilter(MembershipGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }


        Expression<Func<Membership, bool>> filter =
            e => (string.IsNullOrEmpty(request.Filter.Name) ||
                    (e.Name != null && e.Name.Contains(request.Filter.Name))) &&

                (request.Filter.Price == 0 || e.Price == request.Filter.Price) &&

                (request.Filter.MinurationDays == 0 || e.DurationDays >= request.Filter.MinurationDays) &&

                (request.Filter.MaxnurationDays == 0 || e.DurationDays <= request.Filter.MaxnurationDays)
            ;

        return filter;
    }
}
