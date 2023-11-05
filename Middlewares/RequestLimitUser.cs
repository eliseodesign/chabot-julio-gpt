using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using ESFE.Chatbot.Services.Statics;

namespace ESFE.Chatbot
{
    public class RequestLimitUser
    {

        private readonly RequestDelegate _next;

        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        public RequestLimitUser(RequestDelegate next, IMemoryCache cache, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _next = next;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }


        public async Task Invoke(HttpContext context)
        {
            // Verifica si la solicitud está dirigida a un endpoint específico
            if (context.Request.Path == "/api/chat/user")
            {
                // Obtiene el token JWT del encabezado de la solicitud
                var jwtToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(jwtToken))
                {
                    await BadRequest(context, "Token JWT no válido");
                    return;
                }

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwtToken);

                    // Accede al ID de usuario desde las reclamaciones (claims) del token
                    var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "nameid");

                    if (userIdClaim == null)
                    {
                        await BadRequest(context, "El JWT no tiene un Id de usuario");
                        return;
                    }

                    string userId = userIdClaim.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        await BadRequest(context, "Usuario no válido");
                    }

                    // Crear una clave única para rastrear el límite de solicitudes del usuario
                    string cacheKey = "req_user_limit_" + userId;

                    // Verificar si el usuario ha alcanzado el límite
                    if (_cache.TryGetValue(cacheKey, out int requestCount) && requestCount >= 5)
                    {
                        // Aquí, el límite es de 5 solicitudes, pero puedes ajustarlo según tus necesidades.
                        await BadRequest(context, "Has alcanzado el límite de solicitudes permitidas.");
                        return;
                    }

                    // Incrementar el contador de solicitudes y establecerlo en la caché
                    requestCount++;
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) // Establece un tiempo de expiración
                    };
                    _cache.Set(cacheKey, requestCount, cacheEntryOptions);

                    try
                    {
                        await _next(context);
                        return;
                    }
                    catch (Exception ex)
                    {
                        await BadRequest(context, $"Error al intentar obtener el chat: {ex.Message}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await BadRequest(context, $"Error al procesar el token JWT: {ex.Message}");
                    return;
                }
            }
            await _next(context);

        }
        private async Task BadRequest(HttpContext context, string error)
        {
            Console.WriteLine("BadRequest MIDDLEWARE");
            context.Response.StatusCode = 400; // Código de respuesta prohibido (puedes usar otro código según tus necesidades)
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var data = Res.Provider(new { }, error, false);
            await context.Response.WriteAsJsonAsync(data);
            return;
        }

    }

}
