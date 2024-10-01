using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Mats.Edvardsen.TestingStuff.Web.Tests;

public class SystemControllerTests(ITestOutputHelper testOutputHelper)
    : WebApplicationFactory<IWebAssemblyMarker>
{
    [Fact]
    public async Task Get_ShouldReturn200OK()
    {
        var client = CreateClient();
        var result = await client.GetAsync("System");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        testOutputHelper.WriteLine(content);
    }
}