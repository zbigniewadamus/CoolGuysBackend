using System.Data.Entity.Core;
using CoolGuysBackend.Contexts;
using CoolGuysBackend.Entities;
using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolGuysBackend.Domain;

public class UserService: IUserService
{
    private readonly GlobalDbContext dbContext;
    private readonly IBlobStorageHelper storageHelper;

    public UserService(GlobalDbContext dbContext, IBlobStorageHelper storageHelper)
    {
        this.dbContext = dbContext;
        this.storageHelper = storageHelper;
    }

    public async Task<User> ShowCurrent(int userId)
    {
        var user = dbContext.UserDataEntities.Include(u=>u.User).SingleOrDefault(x=> x.Id == userId);
        if (user == null) throw new ObjectNotFoundException("Użytkownik nie został odnaleziony");

        return new User
        {
            Id = userId,
            Email = user.Email,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            AvatarUrl = user.User.AvatarUrl
        };
    }

    public async Task<User> AddDetails(int userId, UserDto data)
    {
        var user = dbContext.UserDataEntities.Include(u => u.User).SingleOrDefault(x => x.Id == userId);
        if (user == null) throw new Exception("Użytkownik nie został odnaleziony");

        user.User.FirstName = data.FirstName;
        user.User.LastName = data.LastName;
        dbContext.UserDataEntities.Update(user);
        dbContext.SaveChanges();
        return new User
        {
            Id = user.Id,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            Email = user.Email,
            AvatarUrl = user.User.AvatarUrl
        };
    }

    public async Task SaveAvatar(int userId, IFormFile image)
    {
        var userData = dbContext.UserDataEntities.SingleOrDefault(x => x.Id == userId);
        if (userData == null) throw new Exception("Użytkownik nie został odnaleziony");
        var user = dbContext.UserEntities.SingleOrDefault(x => x.Id == userId);
        if (user == null)
        {
            dbContext.UserEntities.Add(new UserEntity { Id = userId });
            user = dbContext.UserEntities.SingleOrDefault(x => x.Id == userId);
        }
        var avatar = await storageHelper.AddImage("avatar", userId, image, true);
        user.AvatarUrl = avatar;
        dbContext.UserEntities.Update(user);
        dbContext.SaveChanges();
    }

    public async Task<List<User>> ShowFriends(int userId)
    {
        var friends = dbContext.FriendEntities.Include(x=>x.User1.User).Include(x=>x.User2.User)
            .Where(f => f.User1Id == userId || f.User2Id == userId)
            .Select(f => f.User1Id == userId ? f.User2 : f.User1)
            .Select(u => new User{ Id = u.Id, Email = u.Email, FirstName = u.User.FirstName, LastName = u.User.LastName, AvatarUrl = u.User.AvatarUrl } )
            .ToList();
        return friends;
    }
    public async Task AddFriend(int userId, int friendId)
    {
        var possibleRelation = dbContext.FriendEntities
            .Where(x => x.User1Id == userId && x.User2Id == friendId || x.User1Id == friendId && x.User2Id == userId)
            .ToList() ?? new List<FriendEntity>();
        if (possibleRelation?.Count > 0)
        {
            throw new Exception("Jesteście już znajomymi.");
        }

        dbContext.FriendEntities.Add(new FriendEntity { User1Id = userId, User2Id = friendId });
        dbContext.SaveChanges();
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        List<FriendEntity> possibleRelation = dbContext.FriendEntities
            .Where(x => x.User1Id == userId && x.User2Id == friendId || x.User1Id == friendId && x.User2Id == userId)
            .ToList() ?? new List<FriendEntity>();
        if (!possibleRelation.Any())
        {
            throw new Exception("Nie znaleziono takiej relacji.");
        }

        dbContext.FriendEntities.RemoveRange(possibleRelation);

        dbContext.SaveChanges();
    }

    public async Task<int> SearchUser(string email)
    {
        var user = dbContext.UserDataEntities.Include(ud => ud.User).SingleOrDefault(x => x.Email == email);
        if (user == null) throw new Exception("Nie znaleziono użytkownika z podanym adresem email");
        return user.Id;
    }
}