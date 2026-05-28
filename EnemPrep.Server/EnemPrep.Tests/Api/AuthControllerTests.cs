using System.Net;
using EnemPrep.Domain.DTOS;

namespace EnemPrep.Tests.Api;

public class AuthControllerTests(CustomWebApplicationFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Login_WithValidCredentials_ShouldCreateSessionAndReturnOk()
    {
        PostUserRequest registerRequest = new PostUserRequest();
        
        registerRequest.Password = "Password0123";
        registerRequest.Email = "user@login.com";
        registerRequest.FullName = "Valid Login User Full Name";
        registerRequest.Username = "login_user";
        
        await SeedUserAsync(registerRequest);

        LoginUserRequest loginRequest = new LoginUserRequest();
        loginRequest.Username = registerRequest.Username;
        loginRequest.Password = registerRequest.Password;

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            loginRequest
        );

        response.EnsureSuccessStatusCode();
        Assert.Contains(
            ".EnemPrep.TestSession", 
            response.Headers.GetValues("Set-Cookie").FirstOrDefault()
        );
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturnUnauthorized()
    {
        PostUserRequest registerRequest = new PostUserRequest();
        
        registerRequest.Password = "wrong_pass_user";
        registerRequest.Email = "wrong@pass.com";
        registerRequest.FullName = "Wrong Pass User Full Name";
        registerRequest.Username = "correct_pass134";
        
        await SeedUserAsync(registerRequest);

        LoginUserRequest loginRequest = new LoginUserRequest();

        loginRequest.Username = registerRequest.Username;
        loginRequest.Password = "wrong_pass321";

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            loginRequest
        );
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_WhenAuthenticated_ShouldInvalidateSession()
    {
        await AuthenticateAsAdminAsync();
        var response = await Client.PostAsJsonAsync("/api/auth/logout", new { });

        response.EnsureSuccessStatusCode();
        
        var protectedRouteResponse = await Client.GetAsync("/api/questions/6bdcce1f-aa3b-4a21-9ad1-9c5bf07dfc58");
        Assert.Equal(HttpStatusCode.Unauthorized, protectedRouteResponse.StatusCode);
    }

    [Fact]
    public async Task AccessAdminRoute_WithStudentRole_ShouldReturnForbidden()
    {
        PostUserRequest registerRequest = new PostUserRequest();
        registerRequest.Password = "student-pass";
        registerRequest.Email = "student@test.com";
        registerRequest.FullName = "Student Test User Full Name";
        registerRequest.Username = "student-test";
        
        await SeedUserAsync(registerRequest);

        LoginUserRequest loginRequest = new LoginUserRequest();
        loginRequest.Username = registerRequest.Username;
        loginRequest.Password = registerRequest.Password;
        
        await Client.PostAsJsonAsync(
            "/api/auth/login", 
            loginRequest
        );

        var response = await Client.DeleteAsync("/api/questions/6bdcce1f-aa3b-4a21-9ad1-9c5bf07dfc58");
        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}