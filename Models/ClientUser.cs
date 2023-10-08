using System;
using System.Collections.Generic;

namespace ESFE.Chatbot.Models;

public partial class ClientUser
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? RestartAccount { get; set; }

    public bool? ConfirmAccount { get; set; }

    public string? Token { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? BadConduct { get; set; }

    public int Banned { get; set; }

    public string? TypeUserId { get; set; }

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    public virtual TypeUser? TypeUser { get; set; }
}
