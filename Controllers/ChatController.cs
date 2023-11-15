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
using System.Text;


namespace ESFE.Chatbot.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ChatController : ControllerBase
  {
    //   private readonly IConfiguration _config;
    // public EmailService(IConfiguration config)
    // {
    //   _config = config;
    // }

    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatController(IMemoryCache cache, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
      _cache = cache;
      _httpClientFactory = httpClientFactory;
      _config = config;
    }

    [HttpGet]
    [Route("test")]
    public async Task<IActionResult> Test()
    {
      return Ok(Res.Provider(new { }, "Test", true));
    }
    [Authorize]
    [HttpPost]
    [Route("user")]
    public async Task<IActionResult> ChatUser([FromBody] ChatRequest chatRequest)
    {
      try
      {
        HttpClient client = _httpClientFactory.CreateClient();

        var url = _config.GetValue<string>("ApisUrls:ChaBot");

        // Modificamos la petición a POST y enviamos el body
        HttpResponseMessage response = await client.PostAsJsonAsync(url, chatRequest);
          var content = await response.Content.ReadAsStringAsync();
          var chat = JsonSerializer.Deserialize<Chat>(content);

          return Ok(Res.Provider(chat, "Datos obtenidos correctamente", true));
      }
      catch (Exception ex)
      {
        return BadRequest(Res.Provider(new { }, $"Error al conectar con el chat: {ex.Message}", true));
      }
    }

    [HttpPost]
    [Route("playground")]
    public async Task<IActionResult> ChatPlayground([FromBody] ChatRequest chatRequest)
    {
      try
      {
        HttpClient client = _httpClientFactory.CreateClient();

        var url = _config.GetValue<string>("ApisUrls:ChaBot");

        // Modificamos la petición a POST y enviamos el body
        HttpResponseMessage response = await client.PostAsJsonAsync(url, chatRequest);
          var content = await response.Content.ReadAsStringAsync();
          var chat = JsonSerializer.Deserialize<Chat>(content);

          return Ok(Res.Provider(chat, "Datos obtenidos correctamente", true));
      }
      catch (Exception ex)
      {
        return BadRequest(Res.Provider(new { }, $"Error al conectar con el chat: {ex.Message}", true));
      }
    }
  }
}
