using System;
using System.Collections.Generic;

namespace ESFE.Chatbot.Models;

public partial class FeedBack
{
    public int Id { get; set; }

    public int? ClientUserId { get; set; }

    public string? Message { get; set; }

    public virtual ClientUser? ClientUser { get; set; }
}
