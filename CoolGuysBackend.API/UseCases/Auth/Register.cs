using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.Win32;

namespace CoolGuysBackend.UseCases.Auth;

public class Register
{
    private readonly IAuthService authService;

    public Register(IAuthService authService)
    {
        this.authService = authService;
    }

    public string Exec(RegisterDto data)
    {
        return authService.Register(data);
    }
}