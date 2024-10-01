using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Mats.Edvardsen.TestingStuff.Web.Common;
using Mats.Edvardsen.TestingStuff.Web.UserFeature;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.RequestModels;
using Mats.Edvardsen.TestingStuff.Web.UserFeature.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Mats.Edvardsen.TestingStuff.Web.Tests;

public class UserControllerTests : WebApplicationFactory<IWebAssemblyMarker>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly System.Net.Http.HttpClient _client;

    public UserControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = CreateClient();
    }

    [Theory]
    [InlineData(nameof(ViewModelType.Identifier))]
    [InlineData(nameof(ViewModelType.Display))]
    [InlineData(nameof(ViewModelType.Full))]
    public async Task Get_ShouldReturn200OK(string viewModelType)
    {
        var result = await _client.GetAsync($"User?viewModelType={viewModelType}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_NewUser_WithId_ShouldReturn200OK()
    {
        var json = CreateJsonContent(new UserInsertDtoJson { Id = Guid.NewGuid(), Age = 2, Name = "Two" });
        var result = await _client.PostAsync("User", json);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_NewUser_WithOutId_ShouldReturn200OK()
    {
        var json = CreateJsonContent(new UserInsertDtoJson { Age = 1, Name = "One" });
        var result = await _client.PostAsync("User", json);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostingExistingUser_WithProvidedId_ShouldUpdateExistingUser()
    {
        var user = new UserInsertDtoJson { Id = Guid.NewGuid(), Age = 1, Name = "One" };
        var result = await _client.PostAsync("User", CreateJsonContent(user));
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        user.Name = "UpdatedName";
        var response = await _client.PostAsync("User", CreateJsonContent(user));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedUser = await response.Content.ReadFromJsonAsync<UserFullViewModel>();
        updatedUser.Name.Should().Be(user.Name);
    }
    
    [Fact]
    public async Task PostingExistingUser_WithGeneratedId_ShouldUpdateExistingUser()
    {
        var user = new UserInsertDtoJson { Age = 1, Name = "One" };
        var result = await _client.PostAsync("User", CreateJsonContent(user));
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var insertedUser = await result.Content.ReadFromJsonAsync<UserFullViewModel>();
        user.Id = insertedUser.Id;
        user.Name = "UpdatedName";
        var response = await _client.PostAsync("User", CreateJsonContent(user));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedUser = await response.Content.ReadFromJsonAsync<UserFullViewModel>();
        updatedUser.Name.Should().Be(user.Name);
    }


    private StringContent CreateJsonContent(object content)
    {
        return new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    }
}