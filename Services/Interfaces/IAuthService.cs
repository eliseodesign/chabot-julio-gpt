using ESFE.Chatbot.Models.DTOs;
namespace ESFE.Chatbot;

public interface IAuthService
{
  Task<AuthResponse> GetToken(AuthRequest auth);
}
