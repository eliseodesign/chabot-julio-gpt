namespace ESFE.Chatbot.Schemas;

public class ClientUserSchema
{
  public class Create
  {
    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string TypeUserId { get; set; }
  }
}
