using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.UseCases._contracts;

public interface IAuthService
{
    string Login(LoginDto data);
    bool Logout(string token);
    bool Register(RegisterDto data);

}