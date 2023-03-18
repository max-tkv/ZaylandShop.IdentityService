using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZaylandShop.IdentityService.Invariants;
using ZaylandShop.IdentityService.Options;

namespace ZaylandShop.IdentityService.Extensions;

public static class JwtOptionsExtension
{
    /// <summary>
    /// Регистрация настроек JWT токена
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterJwtOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetSection(RegisterJwtInvariants.OptionsSectionPath)
            .Get<JwtOption>();
        
        Guard.Against.Null(options, RegisterJwtInvariants.OptionsSectionNotDefined);

        return services.AddSingleton(options);
    }
}