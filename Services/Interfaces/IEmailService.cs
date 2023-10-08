using ESFE.Chatbot.Models.DTOs;

namespace ESFE.Chatbot.Services.Interfaces;

public interface IEmailService
{
  bool SendEmail(EmailDTO emailDTO);
}
