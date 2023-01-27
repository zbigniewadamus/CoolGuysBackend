using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.UseCases.User;

public class Avatar
{
    private readonly IUserService userService;

    public Avatar(IUserService userService)
    {
        this.userService = userService;
    }

    public Task Add(int userId, IFormFile image)
    {
        return userService.SaveAvatar(userId, image);
    }
}