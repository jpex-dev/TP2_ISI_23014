using Microsoft.AspNetCore.Mvc;
using Restful_Service.Services;

namespace Restful_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICarteiraService _carteiraService;

        public AuthController(ICarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _carteiraService.LoginAsync(request.Email, request.password);
                return Ok(new { token });
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string password { get; set; }
    }
}
