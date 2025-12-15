using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Entities.BEntities;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService
{
    public string CreateToken(User user)
    {
        
        Claim[] claims = new Claim[]
        {
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Roles.OrderBy(r => r.Id).First().Name),
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
}
