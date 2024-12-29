using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ServicoWCFSoap
{
    public class CarteiraService : ICarteiraService
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["TP2_ISI_23014.Properties.Settings.TP_ISI_GestaoInvestimentosConnectionString"].ConnectionString;

        public string WcfEndpoint { get; }

        public CarteiraService(string wcfEndpoint)
        {
            WcfEndpoint = wcfEndpoint;
        }

        public CarteiraService()
        {
            // Define um valor padrão ou recupera de configuração
            WcfEndpoint = ConfigurationManager.AppSettings["WcfEndpoint"]; // Exemplo de configuração
        }

        public async Task<CarteiraResponse> ObterCarteira(int usuarioId, string moedaPretendida)
        {
            CarteiraResponse response = new CarteiraResponse
            {
                MoedaDefault = "",
                Investimentos = new List<Investimento>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Buscar moeda padrão do usuário
                    string queryMoeda = "SELECT MoedaDefault FROM Utilizadores WHERE UserId = @UsuarioId";
                    using (SqlCommand cmd = new SqlCommand(queryMoeda, conn))
                    {
                        cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                        response.MoedaDefault = cmd.ExecuteScalar()?.ToString();

                        if (string.IsNullOrEmpty(response.MoedaDefault))
                        {
                            throw new FaultException("Usuário não encontrado ou sem moeda padrão configurada.");
                        }
                    }

                    // 2. Buscar investimentos do usuário
                    string queryInvestimentos = "SELECT InvestmentId, TipoInvestimento, ValorOriginal, MoedaOriginal FROM Investimentos WHERE UserId = @UsuarioId";
                    using (SqlCommand cmd = new SqlCommand(queryInvestimentos, conn))
                    {
                        cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Investimentos.Add(new Investimento
                                {
                                    Id = reader.GetInt32(0),
                                    TipoInvestimento = reader.GetString(1),
                                    ValorOriginal = reader.GetDecimal(2),
                                    MoedaOriginal = reader.GetString(3),
                                    ValorConvertido = reader.GetDecimal(2) // Valor temporário
                                });
                            }
                        }
                    }
                }

                // 3. Verificar se a moeda pretendida é diferente da padrão
                if (!string.IsNullOrEmpty(moedaPretendida) && moedaPretendida != response.MoedaDefault)
                {
                    decimal taxaConversao = await ObterTaxaConversaoAsync(response.MoedaDefault, moedaPretendida);

                    // Converter os valores dos investimentos
                    foreach (var investimento in response.Investimentos)
                    {
                        investimento.ValorConvertido = investimento.ValorOriginal * taxaConversao;
                    }

                    // Atualizar a moeda padrão na resposta
                    response.MoedaDefault = moedaPretendida;
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a carteira: {ex.Message}");
                throw new FaultException("Ocorreu um erro ao processar a carteira. Por favor, tente novamente.");
            }
        }

        // Método para obter a taxa de conversão online
        // Método para obter a taxa de conversão online


        private async Task<decimal> ObterTaxaConversaoAsync(string moedaOrigem, string moedaDestino)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Substitua pela chave da API correta
                    string apiKey = "75fe206b78d1ba5e31943f956560e2fc";
                    string url = $"https://api.exchangeratesapi.io/v1/latest?access_key=75fe206b78d1ba5e31943f956560e2fc&format=1&symbols={moedaDestino}";

                    // Adiciona o cabeçalho de autenticação
                    httpClient.DefaultRequestHeaders.Add("apikey", apiKey);

                    // Faz a requisição à API
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ApplicationException($"Erro na API de conversão: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    // Parse da resposta JSON
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extrai a taxa de conversão
                    if (jsonResponse.success != true)
                    {
                        throw new ApplicationException("Erro ao processar a resposta da API.");
                    }

                    decimal taxa = (decimal)jsonResponse.rates[moedaDestino];
                    return taxa;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao obter taxa de conversão: {ex.Message}");
            }
        }

    }
}
