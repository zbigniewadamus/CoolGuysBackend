using System.Text.Json.Serialization;

namespace CoolGuysBackend.UseCases._contracts;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("firstname")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastname")]
    public string? LastName { get; set; }
    [JsonPropertyName("avatar")]
    public string? AvatarUrl { get; set; }
}