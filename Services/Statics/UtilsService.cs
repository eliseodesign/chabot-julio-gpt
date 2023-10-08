using System.Security.Cryptography;
using System.Text;

namespace ESFE.Chatbot.Statics;

public class UtilsService
{
  private static Random random = new Random();

  public static string ConvertSHA256(string text)
  {
    string hash = string.Empty;
    using (SHA256 sha256 = SHA256.Create())
    {
      //obtener el hash recibido
      byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

      // convertir array de bytes en cadena de texto
      foreach (byte b in hashValue)
      {
        hash += $"{b:X2}";
      }
    }
    return hash;
  }

  public static string RandomCode()
  {
    const int length = 6;
    const string chars = "0123456789";
    char[] code = new char[length];

    for (int i = 0; i < length; i++)
    {
      code[i] = chars[random.Next(chars.Length)];
    }

    return new string(code);
  }
}
