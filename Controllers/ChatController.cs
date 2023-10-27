using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace ESFE.Chatbot.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ChatController : ControllerBase
  {
    private readonly IMemoryCache _cache;

    public ChatController(IMemoryCache cache)
    {
      _cache = cache;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetData()
    {
      // Obtiene el token JWT del encabezado de la solicitud
      var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
   
      if (string.IsNullOrEmpty(jwtToken))
      {
        return BadRequest("Token JWT no válido");
      }

      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);

        // Accede al ID de usuario desde las reclamaciones (claims) del token
        var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "nameid");

        if (userIdClaim == null)
        {
          return BadRequest("El JWT no tiene un Id de usuario");
        }

        string userId = userIdClaim.Value;

        if (string.IsNullOrEmpty(userId))
        {
          return BadRequest("Usuario no válido");
        }

        // Crear una clave única para rastrear el límite de solicitudes del usuario
        string cacheKey = "request_limit_" + userId;

        // Verificar si el usuario ha alcanzado el límite
        if (_cache.TryGetValue(cacheKey, out int requestCount) && requestCount >= 5)
        {
          // Aquí, el límite es de 5 solicitudes, pero puedes ajustarlo según tus necesidades.
          return BadRequest("Has alcanzado el límite de solicitudes permitidas.");
        }

        // Incrementar el contador de solicitudes y establecerlo en la caché
        requestCount++;
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Establece un tiempo de expiración
        };
        _cache.Set(cacheKey, requestCount, cacheEntryOptions);

        // Realiza la lógica de tu acción aquí
        // return Ok(new { test = true });

        return Ok(new { UserId = userId, Message = "Datos obtenidos correctamente." });
      }
      catch (Exception ex)
      {
        return BadRequest($"Error al procesar el token JWT: {ex.Message}");
      }
    }
  }
}
