using System.Security.Cryptography.X509Certificates;
using Common.Entities.BEntities;
using Common.Entities.MTMTables;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UsersService : BaseService<User>
{
    public override void Create(User item)
    {
        Role defaultRole = _context.Roles.Find(item.Roles.First().Id);
        item.Roles = new List<Role> { defaultRole };

        _context.Users.Add(item);
        _context.SaveChanges();
    }
    public void AddRole(int userId, int roleId)
    {
        bool exists = _context.UserRoles
            .Any(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (!exists)
        {
            _context.UserRoles.Add(new UserRoles { UserId = userId, RoleId = roleId });
            _context.SaveChanges();
        }
    }

    public void RemoveRole(int userId, int roleId)
    {
        UserRoles entity = _context.UserRoles
            .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (entity != null)
        {
            _context.UserRoles.Remove(entity);
            _context.SaveChanges();
        }
    }

    public void ChangeMembership(int userId, int membershipId)
    {
        User user = _context.Users.FirstOrDefault(u => u.Id == userId);
        user.MembershipId = membershipId;
        _context.SaveChanges();
    }
}
