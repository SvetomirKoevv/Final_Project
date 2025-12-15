using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Hall;
using API.Infrastructure.ResponseDTOs.Hall;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator, Manager, Coach")]
[Route("api/[controller]")]
public class HallsController : BaseController<Hall, HallService, HallRequest, HallGetRequest, HallPostResponse>
{
    override protected void PopulateRequest(Hall entity, HallRequest request)
    {
        entity.Name = request.Name;
        entity.Capacity = request.Capacity;
        entity.Floor = request.Floor;
    }

    override protected void PopulateResponse(Hall entity, HallPostResponse response)
    {
        response.Id = entity.Id;
        response.Name =  entity.Name;
        response.Capacity = entity.Capacity;
        response.Floor = entity.Floor;
    }

    override protected Expression<Func<Hall, bool>> GetFilter(HallGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }


        Expression<Func<Hall, bool>> filter =
            e => (string.IsNullOrEmpty(request.Filter.Name) ||
                    (e.Name != null && e.Name.Contains(request.Filter.Name))) &&

                (request.Filter.MinCapacity == 0 || e.Capacity >= request.Filter.MinCapacity) &&

                (request.Filter.MaxCapacity == 0 || e.Capacity <= request.Filter.MaxCapacity) &&

                (request.Filter.Floor == 0 || e.Floor == request.Filter.Floor)
            ;

        return filter;
    }
}