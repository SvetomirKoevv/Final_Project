using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.TrainingSession;
using API.Infrastructure.ResponseDTOs.TrainingSessionP;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingSessionsController : BaseController<TrainingSession, TrainingSessionService, TrainingSessionRequest, TrainingSessionGetRequest, TrainingSessionPostResponse>
{
    protected override void PopulateRequest(TrainingSession entity, TrainingSessionRequest request)
    {
        entity.CoachId = request.CoachId;
        entity.SportId = request.SportId;
        entity.HallId = request.HallId;
        entity.StartTime = request.StartTime;
        entity.EndTime = request.EndTime;
        entity.MaxParticipants = request.MaxParticipants;
    }

    protected override void PopulateResponse(TrainingSession entity, TrainingSessionPostResponse response)
    {
        response.CoachId = entity.CoachId;
        response.SportId = entity.SportId;
        response.HallId = entity.HallId;
        response.StartTime = entity.StartTime;
        response.EndTime = entity.EndTime;
        response.MaxParticipants = entity.MaxParticipants;
    }

    protected override Expression<Func<TrainingSession, bool>> GetFilter(TrainingSessionGetRequest request)
    {
        Expression<Func<TrainingSession, bool>> filter = ts =>
            (request.Filter.CoachId == null ||
                ts.CoachId == request.Filter.CoachId) &&

            (request.Filter.SportId == null ||
                ts.SportId == request.Filter.SportId) &&

            (request.Filter.HallId == null ||
                ts.HallId == request.Filter.HallId) &&

            (request.Filter.FromStartTime == null ||
                ts.StartTime >= request.Filter.FromStartTime) &&

            (request.Filter.ToStartTime == null ||
                ts.StartTime <= request.Filter.ToStartTime) &&

            (request.Filter.FromEndTime == null ||
                ts.EndTime >= request.Filter.FromEndTime) &&

            (request.Filter.ToEndTime == null ||
                ts.EndTime <= request.Filter.ToEndTime) &&

            (request.Filter.MaxParticipants == null ||
                ts.MaxParticipants == request.Filter.MaxParticipants);

        return filter;
    }

}
