using ESFE.Chatbot.Models.DTOs;
using ESFE.Chatbot.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ESFE.Chatbot.Services;

public class EmailService : IEmailService
{
  private readonly IConfiguration _config;
  public EmailService(IConfiguration config)
  {
    _config = config;
  }
  public bool SendEmail(EmailDTO emailDTO)
  {
    try
    {
      var Host = _config.GetSection("Email:Host").Value;
      int Port = Convert.ToInt32(_config.GetSection("Email:Port").Value);
      var UserName = _config.GetSection("Email:UserName").Value;
      var Password = _config.GetSection("Email:Password").Value;

      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse(UserName));
      email.To.Add(MailboxAddress.Parse(emailDTO.To));
      email.Subject = emailDTO.Subject;
      email.Body = new TextPart(TextFormat.Html)
      {
        Text = emailDTO.Content
      };

      using var smtp = new SmtpClient();
      smtp.Connect(Host, Port, SecureSocketOptions.StartTls);

      smtp.Authenticate(UserName, Password);
      smtp.Send(email);
      smtp.Disconnect(true);
      return true;
    }
    catch
    {
      return false;
    }
  }
}
