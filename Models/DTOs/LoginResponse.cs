namespace ESFE.Chatbot;

public class LoginResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? BadConduct { get; set; }

    public int Banned { get; set; }

    public string? TypeUserId { get; set; }
}
