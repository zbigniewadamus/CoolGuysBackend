using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.UseCases.User;

public class Friend
{
    private readonly IUserService userService;

    public Friend(IUserService userService)
    {
        this.userService = userService;
    }

    public Task<List<_contracts.User>> Show(int userId)
    {
        return userService.ShowFriends(userId);
    }

    public Task Add(int userId, int friendId)
    {
        return userService.AddFriend(userId, friendId);
    }

    public Task Remove(int userId, int friendId)
    {
        return userService.RemoveFriend(userId, friendId);
    }
}