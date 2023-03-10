using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ZaylandShop.IdentityService.Storage;

public static class Entry
{
    private static readonly Action<DbContextOptionsBuilder> DefaultOptionsAction = (options) => { };
        
    /// <summary>
    /// Добавления зависимостей для работы с БД
    /// </summary>
    /// <param name="serviceCollection">serviceCollection</param>
    /// <param name="optionsAction">optionsAction</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddSqlStorage(
        this IServiceCollection serviceCollection, 
        Action<DbContextOptionsBuilder> optionsAction)
    {
        serviceCollection.AddDbContext<AuthDbContext>(optionsAction ?? DefaultOptionsAction);
        return serviceCollection;
    }
}