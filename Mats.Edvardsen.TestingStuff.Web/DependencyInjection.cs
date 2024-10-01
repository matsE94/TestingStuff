using FluentValidation;
using Mats.Edvardsen.TestingStuff.Data;
using Mats.Edvardsen.TestingStuff.Data.Settings;
using Mats.Edvardsen.TestingStuff.Web.Settings;
using Mats.Edvardsen.TestingStuff.Web.UserFeature;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.RequestModels;
using Microsoft.Extensions.Options;

namespace Mats.Edvardsen.TestingStuff.Web;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        //swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Options+Settings
        services.Configure<ApplicationSettings>(configuration);
        services.AddTransient<ApplicationSettings>(p => p.GetRequiredService<IOptions<ApplicationSettings>>().Value);
        services.AddTransient<DatabaseSettings>(p => p.GetRequiredService<ApplicationSettings>().DatabaseSettings);
        services.AddTransient<MySettings>(p => p.GetRequiredService<ApplicationSettings>().MySettings);

        // mvc
        services.AddControllers().AddControllersAsServices();
        services.AddScoped<ExceptionMiddleware>();

        // providers, helpers, mapping, validators
        services.AddScoped<IUserServiceContext, UserServiceContext>();

        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddScoped<IValidator<UserInsertDtoJson>, UserInsertDtoJsonValidator>();

        services.AddSingleton(TimeProvider.System);


        // external project dependencies
        services.AddDataDependencies();

        return services;
    }
}