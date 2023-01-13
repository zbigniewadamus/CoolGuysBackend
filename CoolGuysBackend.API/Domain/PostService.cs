using CoolGuysBackend.UseCases._contracts;
using CoolGuysBackend.UseCases._contracts.dto;

namespace CoolGuysBackend.Domain;

public class PostService: IPostService
{
    public Task<bool> Create(PostDto data)
    {
        throw new NotImplementedException();
    }

    public Task<List<Post>> ShowAll()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Vote(bool positive)
    {
        throw new NotImplementedException();
    }
}