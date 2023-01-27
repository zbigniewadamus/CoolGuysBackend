using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.UseCases._contracts;

public interface IAuthService
{
    string Login(LoginDto data);
    string Register(RegisterDto data);

}