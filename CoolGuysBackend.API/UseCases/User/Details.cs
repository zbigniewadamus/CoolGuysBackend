using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.UseCases.User;

public class Details
{
    private readonly IUserService userService;

    public Details(IUserService userService)
    {
        this.userService = userService;
    }

    public Task<_contracts.User> Add(int userId, UserDto data)
    {
        return userService.AddDetails(userId, data);
    }
}