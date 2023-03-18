using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZaylandShop.IdentityService.Abstractions;
using ZaylandShop.IdentityService.Entities;
using ZaylandShop.IdentityService.Options;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ZaylandShop.IdentityService.Services;

public class TokenService : ITokenService
{
    private readonly JwtOption _option;

    public TokenService(JwtOption option)
    {
        _option = option;
    }
    
    public async Task<string> CreateTokenAsync(AppUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _option.Issuer,
            audience: _option.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_option.ExpirationMinutes)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}