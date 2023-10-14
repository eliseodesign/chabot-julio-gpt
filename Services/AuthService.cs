using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ESFE.Chatbot.Models.DTOs;
using ESFE.Chatbot.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ESFE.Chatbot;

public class AuthService : IAuthService
{
  private readonly IConfiguration _configuration; // para acceder a valores appsettings
  private readonly IClienteUserService _userService;

  public AuthService(IConfiguration configuration, IClienteUserService userService)
  {
    _configuration = configuration;
    _userService = userService;
  }

  private string GenerarToken(string idUsuario, string typeUser)
  {

    var key = _configuration.GetValue<string>("JwtSettings:key");
    var keyBytes = Encoding.ASCII.GetBytes(key);

    var claims = new ClaimsIdentity();
    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));
    claims.AddClaim(new Claim(ClaimTypes.Role, typeUser));

    var credencialesToken = new SigningCredentials(
        new SymmetricSecurityKey(keyBytes),
        SecurityAlgorithms.HmacSha256Signature
        );

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = claims,
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credencialesToken
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

    string tokenCreado = tokenHandler.WriteToken(tokenConfig);

    return tokenCreado;
  }
  public async Task<AuthResponse> GetToken(AuthRequest auth)
  {
    var usuario_encontrado = await _userService.Validate(auth.Email, auth.Password);

    if (usuario_encontrado == null)
    {
      return await Task.FromResult<AuthResponse>(null);
    }

    string tokenCreado = GenerarToken(usuario_encontrado.Id.ToString(), usuario_encontrado.TypeUserId);

    //string refreshTokenCreado = GenerarRefreshToken();

    return new AuthResponse() { Token = tokenCreado, Result = true, Message = "Ok", User = usuario_encontrado };

    //return await GuardarHistorialRefreshToken(usuario_encontrado.IdUsuario, tokenCreado, refreshTokenCreado);


  }
}
