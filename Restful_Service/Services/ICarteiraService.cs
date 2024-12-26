using Restful_Service.Models;
using System.Threading.Tasks;

namespace Restful_Service.Services
{
    public interface ICarteiraService
    {
        Task<CarteiraResponse> ObterCarteiraAsync(int usuarioId);
    }
}
