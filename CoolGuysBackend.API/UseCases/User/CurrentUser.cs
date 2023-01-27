using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.UseCases.User;

public class CurrentUser
{
    private readonly IUserService userService;

    public CurrentUser(IUserService userService)
    {
        this.userService = userService;
    }

    public Task<_contracts.User> Show(int id)
    {
        return userService.ShowCurrent(id);
    }
}