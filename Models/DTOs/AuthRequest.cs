namespace ESFE.Chatbot.Models.DTOs;

public class AuthRequest
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}