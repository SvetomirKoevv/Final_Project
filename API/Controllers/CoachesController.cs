using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Coach;
using API.Infrastructure.ResponseDTOs.Coach;
using API.Services;
using Common.Entities.BEntities;
using Common.Entities.MTMTables;
using Common.Entities.Other;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator, Manager")]
[Route("api/[controller]")]
public class CoachesController : BaseController<Coach, CoachService, CoachRequest, CoachGetRequest, CoachResponse>
{
    override protected void PopulateRequest(Coach entity, CoachRequest request)
    {
        request.FirstName = entity.FirstName;
        request.LastName = entity.LastName;
        request.Phone = entity.Phone;
    }
    override protected void PopulateResponse(Coach entity, CoachResponse response)
    {
        response.FirstName = entity.FirstName;
        response.LastName = entity.LastName;
        response.Phone = entity.Phone;
    }

    override protected Expression<Func<Coach, bool>> GetFilter(CoachGetRequest request)
    {
        Expression<Func<Coach, bool>> filter = c =>
            (string.IsNullOrEmpty(request.Filter.FirstName) ||
                c.FirstName.Contains(request.Filter.FirstName)) &&

            (string.IsNullOrEmpty(request.Filter.LastName) ||
                c.LastName.Contains(request.Filter.LastName)) &&

            (string.IsNullOrEmpty(request.Filter.Phone) ||
                c.Phone.Contains(request.Filter.Phone));

        return filter;
    }

    [HttpPut]
    [Route("{coachId}/addSport")]
    public async Task<IActionResult> AddSportToCoach([FromRoute] int coachId, [FromQuery] int sportId)
    {
        return await ReturnFromValidationModel<int, CoachSports>(
            await new FullEntityRequestValidator<Coach, CoachService, Sport, SportService>()
                .Validate(coachId, sportId),
            new TwoIdsDelegate<CoachSports>(async (cId, sId) =>
            {
                CoachService coachService = new CoachService();
                CoachSports newPair = await coachService.AddSportAsync(cId, sId);

                if (newPair == null)
                {
                    return ResultSetGenerator<CoachSports>.Failure(newPair, new List<ResultSetError>
                    {
                        new ResultSetError
                        {
                            Name = "CoachSports",
                            Mesages = new List<string>() { "This coach already has this sport assigned!" }
                        }
                    });
                }

                return ResultSetGenerator<CoachSports>.Success(newPair);
            }));
    }
}
