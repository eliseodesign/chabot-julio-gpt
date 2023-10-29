using System;
using System.Collections.Generic;

namespace ESFE.Chatbot.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int? Clientuserid { get; set; }

    public string? Message { get; set; }

    public virtual Clientuser? Clientuser { get; set; }
}
