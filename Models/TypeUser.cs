using System;
using System.Collections.Generic;

namespace ESFE.Chatbot.Models;

public partial class TypeUser
{
    public string TypeName { get; set; } = null!;

    public virtual ICollection<ClientUser> ClientUsers { get; set; } = new List<ClientUser>();
}
