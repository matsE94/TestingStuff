using Mats.Edvardsen.TestingStuff.Data.AuditFeature;
using Mats.Edvardsen.TestingStuff.Data.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mats.Edvardsen.TestingStuff.Data;

interface IDataAssemblyMarker;

public static class DependencyInjection
{
    public static IServiceCollection AddDataDependencies(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>((provider, options) =>
            options.UseInMemoryDatabase(provider.GetRequiredService<DatabaseSettings>().ConnectionString)
                .AddInterceptors(new EntityAuditInterceptor(provider.GetRequiredService<IGuidProvider>())));
        services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(IDataAssemblyMarker).Assembly); });

        services.AddScoped<IGuidProvider, GuidProvider>();

        return services;
    }
}