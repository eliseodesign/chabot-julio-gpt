using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ESFE.Chatbot.Services.Statics;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using System.Net.Http;
using System.Text.Json;
using ESFE.Chatbot.Models.Chat;
using Microsoft.VisualBasic;
using dotenv.net;


namespace ESFE.Chatbot.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ChatController : ControllerBase
  {
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatController(IMemoryCache cache, IHttpClientFactory  httpClientFactory)
    {
      _cache = cache;
      _httpClientFactory = httpClientFactory;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetData([FromQuery] string query)
    {
      DotEnv.Load();
      
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

        try
        {
            HttpClient client = _httpClientFactory.CreateClient();
            var envVars = DotEnv.Read();
            string url = envVars["CHATBOT_API"];
            HttpResponseMessage response = await client.GetAsync(url+query);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var chat = JsonSerializer.Deserialize<Chat>(content);

                return Ok(Res.Provider(chat, "Datos obtenidos correctamente", true));
            }
            else
            {
                return BadRequest($"Error en la respuesta: {response}");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al intentar obtener el chat: {ex.Message}");
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"Error al procesar el token JWT: {ex.Message}");
      }
    }
  }
}
