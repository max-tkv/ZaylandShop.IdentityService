using Microsoft.Extensions.DependencyInjection;
using ZaylandShop.IdentityService.Abstractions;
using ZaylandShop.IdentityService.Services;


namespace ZaylandShop.IdentityService;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей уровня бизнес-логики
    /// </summary>
    /// <param name="serviceCollection">serviceCollection</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        return services;
    }
}