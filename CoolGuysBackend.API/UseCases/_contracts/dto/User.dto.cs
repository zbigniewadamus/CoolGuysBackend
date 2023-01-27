using System.Text.Json.Serialization;

namespace CoolGuysBackend.UseCases._contracts.dto;

public class UserDto
{
    [JsonPropertyName("firstname")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastname")]
    public string? LastName { get; set; }
}