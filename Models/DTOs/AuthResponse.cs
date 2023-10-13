using ESFE.Chatbot.Models;
namespace ESFE.Chatbot.Models.DTOs;

public class AuthResponse
{
    public required string Token { get; set; }
    public bool Result { get; set; }
    public required string Message { get; set; }
    public ClientUser? User {get;set;}
}
