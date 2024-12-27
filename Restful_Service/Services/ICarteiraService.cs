using Restful_Service.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Restful_Service.Services
{
    [ServiceContract]

    public interface ICarteiraService
    {
        [OperationContract]

        Task<CarteiraResponse> ObterCarteiraAsync(int usuarioId);
    }
}
