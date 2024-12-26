using Microsoft.AspNetCore.Mvc;
using Restful_Service.Models;
using Restful_Service.Services;

namespace Restful_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteiraController : ControllerBase
    {
        private readonly ICarteiraService _carteiraService;

        public CarteiraController(ICarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> ObterCarteiraAsync(int usuarioId)
        {
            try
            {
                var carteiraResponse = await _carteiraService.ObterCarteiraAsync(usuarioId);

                // Retornar a resposta com o código 200 OK
                return Ok(carteiraResponse);
            }
            catch (Exception ex)
            {
                // Caso ocorra algum erro, retorna o código 500 com a mensagem de erro
                return StatusCode(500, $"Erro ao obter carteira: {ex.Message}");
            }
        }
    }
}
