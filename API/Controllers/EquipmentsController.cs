using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Equipment;
using API.Infrastructure.ResponseDTOs.Equipment;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator, Manager, Coach")]
[Route("api/[controller]")]
public class EquipmentsController : BaseController<Equipment, EquipmentService, EquipmentRequest, EquipmentGetRequest, EquipmentPostResponse>
{
    override protected void PopulateRequest(Equipment entity, EquipmentRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.QunatityAvailable = request.QunatityAvailable;
    }

    override protected void PopulateResponse(Equipment entity, EquipmentPostResponse response)
    {
        response.Name =  entity.Name;
        response.Description = entity.Description;
        response.QunatityAvailable = entity.QunatityAvailable;
    }

    override protected Expression<Func<Equipment, bool>> GetFilter(EquipmentGetRequest request)
    {
        if (request?.Filter == null)
        {
            return e => true;
        }

        Expression<Func<Equipment, bool>> filter =
            e => (string.IsNullOrEmpty(request.Filter.Name) ||
                    (e.Name != null && e.Name.Contains(request.Filter.Name))) &&

                (string.IsNullOrEmpty(request.Filter.Description) ||
                    (e.Description != null && e.Description.Contains(request.Filter.Description))) &&

                (request.Filter.MinQuantity == 0 || e.QunatityAvailable >= request.Filter.MinQuantity) &&

                (request.Filter.MaxQuantity == 0 || e.QunatityAvailable <= request.Filter.MaxQuantity)
            ;

        return filter;
    }
}
