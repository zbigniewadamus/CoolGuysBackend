using CoolGuysBackend.Contexts;
using CoolGuysBackend.Domain;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Moq;

namespace CoolGuysBackend.Tests.Unit.Auth;

public class LoginTest
{
    private AuthService authService;

    [SetUp]
    public void Setup()
    {
        var dbContext = new Mock<GlobalDbContext>();
        authService = new AuthService(dbContext.Object, string.Empty);
    }

    [Test]
    public async Task Login_Success()
    {
        //GIVEN
        var loginData = new LoginDto
        {
            Email = "user@user.com",
            Password = "Password"
        };
        
        //THEN
        Assert.DoesNotThrow(() => authService.Login(loginData));
    }

    [TestCase(null,null)]
    [TestCase(null,"password")]
    [TestCase("user@user.com",null)]
    public async Task Login_Fail(string? email, string? password)
    {
        //GIVEN
        var loginData = new LoginDto
        {
            Email = email,
            Password = password
        };

        //WHEN
        var result = authService.Login(loginData);

        //THEN
        //Assert.False(result);
    }
}