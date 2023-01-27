using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.UseCases._contracts;

public interface IUserService
{
    Task<User> ShowCurrent(int userId);
    Task<User> AddDetails(int userId, UserDto data);
    Task SaveAvatar(int userId, IFormFile image);
    Task<List<User>> ShowFriends(int userId);
    Task AddFriend(int userId, int friendId);
    Task RemoveFriend(int userId, int friendId);
    Task<int> SearchUser(string email);
}