using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mats.Edvardsen.ConfigurationResearch.Test;
// {
//     "SteamConfig": {
//         "UserName": "burger_KING",
//         "Password": "42691337"
//     }
// } // appsettings.json
public record SteamConfig
{
    public const string Section = nameof(SteamConfig);
    [Required] public required string UserName { get; set; }
    [Required] public required string Password { get; set; }
}

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json");

        var services = new ServiceCollection();
        services.AddTransient<IConfiguration>(_ => builder.Build());

        // Register our dependency on a config object and require its validation
        // here we basically declare our expectation that IConfiguration has entries 
        // that will satisfy the SteamConfig object.
        // i.e. "SteamConfig__UserName" && "SteamConfig__Password" must exist
        services.AddOptionsWithValidateOnStart<SteamConfig>()
            .BindConfiguration(SteamConfig.Section)
            .ValidateDataAnnotations();

        // inject inner config object from IOptions.
        // this will read the newest config from IConfiguration - which in turn can be updated
        // by keyvault/azconfig without having to restart the application
        services.AddTransient<SteamConfig>(p =>
            p.GetRequiredService<IOptionsSnapshot<SteamConfig>>().Value);

        var provider = services.BuildServiceProvider();


        //simulate app startup with validate
        var validator = provider.GetRequiredService<IStartupValidator>();
        validator.Validate();


        // Make sure we can access the object with all its properties
        var steamConfig = provider.GetRequiredService<SteamConfig>();
        steamConfig.Should().NotBeNull();
        steamConfig.UserName.Should().NotBeNull();
        steamConfig.Password.Should().NotBeNull();
    }
}

public static class BipCommonWorkdayDependencyInjection
{
    // dont need IConfiguration parameter here
    public static void AddBipCommonWorkdayClient(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<WorkdayConfig>()
            .BindConfiguration(WorkdayConfig.Section)
            .ValidateDataAnnotations();
        services.AddScoped<IWorkdayService, WorkdayService>();


        //but if we need it, it's accessible in via the serviceProvider
        services.AddScoped<IWorkdayService, WorkdayService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var config = new WorkdayConfig
            {
                ConnectionString = configuration["WorkdayConfig:ConnectionString"]
                                   ?? throw new Exception("It wasnt here..."),
            };
            return new WorkdayService(config);
        });
    }
}

public interface IWorkdayService;

public class WorkdayService(WorkdayConfig config) : IWorkdayService;

public record WorkdayConfig
{
    public const string Section = nameof(SteamConfig);
    [Required] public required string ConnectionString { get; set; }
}