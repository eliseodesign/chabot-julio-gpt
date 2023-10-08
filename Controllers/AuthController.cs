using Microsoft.AspNetCore.Mvc;

namespace ESFE.Chatbot;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  [HttpGet]
  public IActionResult test(){
    return Ok(new {test = true});
  }
}
