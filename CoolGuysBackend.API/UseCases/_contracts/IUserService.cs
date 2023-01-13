namespace CoolGuysBackend.UseCases._contracts;

public interface IUserService
{
    Task<User> ShowCurrent();
    Task SaveImageFile();
    Task<List<User>> ShowFriends();
    Task AddFriend();
    Task RemoveFriend(int id);
}