using ZaylandShop.IdentityService.Entities;

namespace ZaylandShop.IdentityService.Abstractions;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
}