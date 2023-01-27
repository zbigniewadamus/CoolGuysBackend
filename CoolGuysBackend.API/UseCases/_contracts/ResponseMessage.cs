using System.Text.Json.Serialization;

namespace CoolGuysBackend.UseCases._contracts;

public class ResponseMessage
{
    public string message { get; set; }
    public object? data { get; set; }
}