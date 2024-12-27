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

                //var serviceClient = channelFactory.CreateChannel();
                //// Criando o cliente WCF a partir do ChannelFactory
                var serviceClient = channelFactory.CreateChannel();

                // Executando a chamada assíncrona para obter a carteira
                var response = await serviceClient.ObterCarteiraAsync(usuarioId);

                // Fechar o cliente WCF após o uso
                ((IClientChannel)serviceClient).Close();

                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao comunicar com o WCF: {ex.Message}");
            }
        }
    }
}
