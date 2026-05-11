namespace EnemPrep.Tests.Api;

public class QuestionsControllerTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateQuestion_WithoutSession_ReturnsUnauthorized()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task CreateQuestion_WithStudentSession_ReturnsUnauthorized()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task CreateQuestion_WithAdminSession_ReturnsCreated()
    {
        throw new NotImplementedException();
        await AuthenticateAsAdminAsync();
    }
}