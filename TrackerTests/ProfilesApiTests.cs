using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TrackerTests;

// Integration tests for the /api/profiles endpoints. WebApplicationFactory<Program>
// spins up the real TrackerAPI app in-memory so requests exercise the actual
// routing/model-binding pipeline, not mocks.
public class ProfilesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProfilesApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static SensitivityProfile SampleProfile() => new()
    {
        GameName = "Overwatch 2",
        FieldOfView = 103,
        MouseDPI = 800,
        InGameSensitivity = 5.0,
        CmPer360 = 25.0,
        Notes = "Test profile"
    };

    [Fact]
    public async Task Post_CreatesProfile_ReturnsCreated()
    {
        // Exercises: POST /api/profiles
        // Expect: 201 Created, with the response body echoing the submitted data
        // and a server-assigned, non-zero Id.
        var response = await _client.PostAsJsonAsync("/api/profiles", SampleProfile());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<SensitivityProfile>();
        Assert.NotNull(created);
        Assert.True(created!.Id > 0);
        Assert.Equal("Overwatch 2", created.GameName);
    }

    [Fact]
    public async Task Get_AllProfiles_ReturnsSuccessAndJsonList()
    {
        // Exercises: GET /api/profiles
        // Expect: 200 OK with a JSON array containing at least the seeded profiles.
        var response = await _client.GetAsync("/api/profiles");

        response.EnsureSuccessStatusCode();

        var profiles = await response.Content.ReadFromJsonAsync<List<SensitivityProfile>>();
        Assert.NotNull(profiles);
        Assert.NotEmpty(profiles!);
    }

    [Fact]
    public async Task Get_ById_ValidId_ReturnsOk()
    {
        // Exercises: GET /api/profiles/{id} with an Id known to exist (seed data).
        // Expect: 200 OK with the matching profile in the body.
        var response = await _client.GetAsync("/api/profiles/1");

        response.EnsureSuccessStatusCode();

        var profile = await response.Content.ReadFromJsonAsync<SensitivityProfile>();
        Assert.NotNull(profile);
        Assert.Equal(1, profile!.Id);
    }

    [Fact]
    public async Task Get_ById_InvalidId_ReturnsNotFound()
    {
        // Exercises: GET /api/profiles/{id} with an Id that doesn't exist.
        // Expect: 404 Not Found.
        var response = await _client.GetAsync("/api/profiles/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_ValidId_UpdatesProfile()
    {
        // Exercises: POST to create a profile, then PUT /api/profiles/{id} to update it.
        // Expect: 200 OK, and the returned profile reflects the new values.
        var createResponse = await _client.PostAsJsonAsync("/api/profiles", SampleProfile());
        var created = await createResponse.Content.ReadFromJsonAsync<SensitivityProfile>();

        var updated = SampleProfile();
        updated.GameName = "Overwatch 2 (Updated)";
        updated.InGameSensitivity = 6.5;

        var putResponse = await _client.PutAsJsonAsync($"/api/profiles/{created!.Id}", updated);

        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var result = await putResponse.Content.ReadFromJsonAsync<SensitivityProfile>();
        Assert.Equal("Overwatch 2 (Updated)", result!.GameName);
        Assert.Equal(6.5, result.InGameSensitivity);
    }

    [Fact]
    public async Task Put_InvalidId_ReturnsNotFound()
    {
        // Exercises: PUT /api/profiles/{id} with an Id that doesn't exist.
        // Expect: 404 Not Found, and no profile is created or modified.
        var response = await _client.PutAsJsonAsync("/api/profiles/999999", SampleProfile());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ValidId_RemovesProfile()
    {
        // Exercises: POST to create a profile, then DELETE /api/profiles/{id}.
        // Expect: 204 No Content, and a follow-up GET for the same Id returns 404.
        var createResponse = await _client.PostAsJsonAsync("/api/profiles", SampleProfile());
        var created = await createResponse.Content.ReadFromJsonAsync<SensitivityProfile>();

        var deleteResponse = await _client.DeleteAsync($"/api/profiles/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/profiles/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsNotFound()
    {
        // Exercises: DELETE /api/profiles/{id} with an Id that doesn't exist.
        // Expect: 404 Not Found.
        var response = await _client.DeleteAsync("/api/profiles/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
