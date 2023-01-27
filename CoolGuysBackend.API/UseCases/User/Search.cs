using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.UseCases.User;

public class Search
{
    private readonly IUserService userService;

    public Search(IUserService userService)
    {
        this.userService = userService;
    }

    public Task<int> Exec(string email)
    {
        return userService.SearchUser(email);
    }
}