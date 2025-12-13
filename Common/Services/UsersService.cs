using System.Security.Cryptography.X509Certificates;
using Common.Entities.BEntities;
using Common.Entities.MTMTables;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UsersService : BaseService<User>
{
    public async Task AddRoleAsync(int userId, int roleId)
    {
        bool exists = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (!exists)
        {
            _context.UserRoles.Add(new UserRoles { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveRoleAsync(int userId, int roleId)
    {
        UserRoles entity = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (entity != null)
        {
            _context.UserRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeMembershipAsync(int userId, int membershipId)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        user.MembershipId = membershipId;
        await _context.SaveChangesAsync();
    }
}
