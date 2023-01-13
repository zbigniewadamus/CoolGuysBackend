using System.Text.Json.Serialization;

namespace CoolGuysBackend.UseCases._contracts.dto;

public class LoginDto
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
}