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
    [HttpGet]
    [Route("user")]
    public async Task<IActionResult> GetData([FromQuery] string query)
    {

      try
      {
        HttpClient client = _httpClientFactory.CreateClient();

        var url = _config.GetValue<string>("ApisUrls:ChaBot");
        HttpResponseMessage response = await client.GetAsync(url + query);

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

    [HttpGet]
    [Route("playground")]
    public async Task<IActionResult> Playground([FromQuery] string query)
    {

      try
      {
        HttpClient client = _httpClientFactory.CreateClient();

        var url = _config.GetValue<string>("ApisUrls:ChaBot");
        HttpResponseMessage response = await client.GetAsync(url + query);

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

    
  }
}
