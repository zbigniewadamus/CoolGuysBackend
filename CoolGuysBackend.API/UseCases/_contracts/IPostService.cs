using CoolGuysBackend.UseCases._contracts.dto;
namespace CoolGuysBackend.UseCases._contracts;

public interface IPostService
{
    Task<bool> Create(PostDto data);
    Task<List<Post>> ShowAll();
    Task<bool> Vote(bool positive);
}