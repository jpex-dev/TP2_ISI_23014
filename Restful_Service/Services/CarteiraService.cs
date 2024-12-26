using Restful_Service.Models;
using System.ServiceModel;

namespace Restful_Service.Services
{


    public class CarteiraService : ICarteiraService
    {
        private readonly string _wcfEndpoint;

        public CarteiraService(string wcfEndpoint)
        {
            _wcfEndpoint = wcfEndpoint;
        }

        public async Task<CarteiraResponse> ObterCarteiraAsync(int usuarioId)
        {
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(new Uri(_wcfEndpoint));
                var channelFactory = new ChannelFactory<ICarteiraService>(binding, endpoint);

                var serviceClient = channelFactory.CreateChannel();
                return await serviceClient.ObterCarteiraAsync(usuarioId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao comunicar com o WCF: {ex.Message}");
            }
        }
    }
}
