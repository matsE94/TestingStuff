using Mats.Edvardsen.TestingStuff.Data.AuditFeature;
using Mats.Edvardsen.TestingStuff.Data.AuditFeature.Requests;
using Mats.Edvardsen.TestingStuff.Data.UserFeature;
using Mats.Edvardsen.TestingStuff.Data.UserFeature.Requests;
using Mats.Edvardsen.TestingStuff.Web.SystemFeature;
using Mats.Edvardsen.TestingStuff.Web.UserFeature;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Mats.Edvardsen.TestingStuff.Web.Tests;

public class DependencyInjectionTests : WebApplicationFactory<IWebAssemblyMarker>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _provider;

    public DependencyInjectionTests()
    {
        _scope = Services.CreateScope();
        _provider = _scope.ServiceProvider;
    }

    public new void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }


    [Theory]
    [MemberData(nameof(RequiredServiceTypes))]
    public void GetRequiredService_ShouldNotThrow(Type type) =>
        _provider.Invoking(x => x.GetRequiredService(type))
            .Should().NotThrow();

    public static IEnumerable<object[]> RequiredServiceTypes =>
    [
        [typeof(UserController)],
        [typeof(SystemController)],
        [typeof(IRequestHandler<InsertUserRequest, UserEntity>)],
        [typeof(IRequestHandler<GetUsersRequest, IEnumerable<UserEntity>>)],
        [typeof(IRequestHandler<GetUserRequest, UserEntity?>)],
        [typeof(IRequestHandler<GetAuditsRequest, IEnumerable<EntityAudit>>)],
        [typeof(ExceptionMiddleware)]
    ];
}