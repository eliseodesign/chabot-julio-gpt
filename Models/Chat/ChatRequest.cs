
using System.Text.Json.Serialization;

namespace ESFE.Chatbot.Models.Chat;

public class ChatRequest
{
    public List<string> History { get; set; }
    public string Question { get; set; }
}
