using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.Domain;

public class UserService: IUserService
{
    public Task<User> ShowCurrent()
    {
        throw new NotImplementedException();
    }

    public Task SaveImageFile()
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> ShowFriends()
    {
        throw new NotImplementedException();
    }

    public Task AddFriend()
    {
        throw new NotImplementedException();
    }

    public Task RemoveFriend(int id)
    {
        throw new NotImplementedException();
    }
}