using CoolGuysBackend.UseCases._contracts.dto;
namespace CoolGuysBackend.UseCases._contracts;

public interface IPostService
{
    Task<int> Create(int userId, PostDto data);
    Task<List<Post>> ShowAll(int userId, int page);
    Task<bool> Vote(int postId, bool positive);
    Task<Post> AddImage(int postId, IFormFile data);
}