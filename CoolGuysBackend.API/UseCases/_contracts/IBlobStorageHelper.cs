namespace CoolGuysBackend.UseCases._contracts;

public interface IBlobStorageHelper
{
    public Task<string> AddImage(string containerName, int id, IFormFile image, bool overwrite = true);
}