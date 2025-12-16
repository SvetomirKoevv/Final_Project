using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Entities.BEntities;
using Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class AuthService
{
    public string CreateToken(User user)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Roles.OrderBy(r => r.Id).First().Name),
            new Claim("roleId", user.Roles.OrderBy(r => r.Id).First().Id.ToString()),
            new Claim("userId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SuperSecretKeyThatNooneWillEverGuess123!"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "svetomir",
            audience: "plvduni",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: cred
        );
        string tokenData = new JwtSecurityTokenHandler()
                                            .WriteToken(token);

        return tokenData;
    }

    public static string HashPassword(User user, string password)
    {
        var hasher = new PasswordHasher<User>();
        return hasher.HashPassword(user, password);
    }

    public async Task<string> Login(User user, string password)
    {
        var hasher = new PasswordHasher<User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.Password, password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return CreateToken(user);
    }   
    
    public async Task<string> Register(User user)
    {
        user.Password = HashPassword(user, user.Password);
        user.Roles = new List<Role>
        {
            new Role { Id = 4 } 
        };

        UsersService service = new UsersService();
        service.Create(user);

        return CreateToken(user);
    }
}