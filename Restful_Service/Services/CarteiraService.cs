using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Restful_Service.Models;

namespace Restful_Service.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly string _wcfEndpoint;

        public CarteiraService(string wcfEndpoint)
        {
            _wcfEndpoint = wcfEndpoint;
        }

        public async Task<CarteiraResponse> ObterCarteiraAsync(int usuarioId, string? moedaPretendida)
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(_wcfEndpoint);

            var channelFactory = new ChannelFactory<ServicoWCFSoap.ICarteiraService>(binding, endpoint);
            var wcfClient = channelFactory.CreateChannel();

            try
            {
                // Encapsula a chamada síncrona em uma tarefa assíncrona
                var carteiraResponse = await Task.Run(() =>
                {
                    return wcfClient.ObterCarteira(usuarioId, moedaPretendida);
                });

                // Mapeia o resultado para o modelo RESTful
                return new CarteiraResponse
                {
                    MoedaDefault = carteiraResponse.MoedaDefault,
                    Investimentos = carteiraResponse.Investimentos.ConvertAll(i => new Investimento
                    {
                        Id = i.Id,
                        TipoInvestimento = i.TipoInvestimento,
                        ValorOriginal = i.ValorOriginal,
                        MoedaOriginal = i.MoedaOriginal,
                        ValorConvertido = i.ValorConvertido
                    })
                };
            }
            catch (FaultException ex)
            {
                throw new ApplicationException($"Erro específico do serviço WCF: {ex.Message}");
            }
            catch (CommunicationException ex)
            {
                throw new ApplicationException($"Erro de comunicação com o serviço WCF: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro geral: {ex.Message}");
            }
            finally
            {
                try
                {
                    ((IClientChannel)wcfClient).Close();
                }
                catch
                {
                    ((IClientChannel)wcfClient).Abort();
                }
            }
        }
    }
}
