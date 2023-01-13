using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.UseCases.Auth;

public class Login
{
    private readonly IAuthService authService;

    public Login(IAuthService authService)
    {
        this.authService = authService;
    }

    public string Exec(LoginDto data)
    {
        return authService.Login(data);
    }  
}