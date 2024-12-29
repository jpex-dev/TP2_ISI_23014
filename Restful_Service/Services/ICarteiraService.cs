using Restful_Service.Models;
using System.ServiceModel;

namespace Restful_Service.Services
{
    [ServiceContract]

    public interface ICarteiraService
    {
        [OperationContract]
        //[s(UriTemplate = "/obterCarteira/{usuarioId}", ResponseFormat = WebMessageFormat.Json)]

        Task<CarteiraResponse> ObterCarteiraAsync(int usuarioId,string moedaPretendida);
    }
}
