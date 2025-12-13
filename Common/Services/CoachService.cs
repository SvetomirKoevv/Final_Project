using Common.Entities.BEntities;
using Common.Entities.MTMTables;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class CoachService : BaseService<Coach>
{
    public async Task<CoachSports> AddSportAsync(int coachId, int sportId)
    {
        CoachSports exists = await _context.CoachSports
            .FirstOrDefaultAsync(cs => cs.CoachId == coachId && cs.SportId == sportId);

        if (exists != null)
        {
            return null;
        }
        CoachSports entity = new CoachSports
        {
            CoachId = coachId,
            SportId = sportId
        };

        _context.CoachSports.Add(entity);

        await _context.SaveChangesAsync();
        
        return entity;
    }
}
