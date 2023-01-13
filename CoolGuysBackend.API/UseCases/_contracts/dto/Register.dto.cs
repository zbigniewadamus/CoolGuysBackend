namespace CoolGuysBackend.UseCases._contracts.dto;

public class RegisterDto: LoginDto
{
    public string? PasswordConfirm { get; set; }
}