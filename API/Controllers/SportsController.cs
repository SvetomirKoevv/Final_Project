using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Sport;
using API.Infrastructure.ResponseDTOs.Sport;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator, Manager, Coach")]
[Route("api/[controller]")]
public class SportsController : BaseController<Sport, SportService, SportRequest, SportGetRequest, SportPostResponse>
{
    override protected void PopulateRequest(Sport entity, SportRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
    }

    override protected void PopulateResponse(Sport entity, SportPostResponse response)
    {
        response.Id = entity.Id;
        response.Name =  entity.Name;
        response.Description = entity.Description;
    }

    override protected Expression<Func<Sport, bool>> GetFilter(SportGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }


        Expression<Func<Sport, bool>> filter =
            e => (string.IsNullOrEmpty(request.Filter.Name) ||
                    (e.Name != null && e.Name.Contains(request.Filter.Name))) &&

                (string.IsNullOrEmpty(request.Filter.Description) ||
                    (e.Description != null && e.Description.Contains(request.Filter.Description)))
            ;

        return filter;
    }
}
