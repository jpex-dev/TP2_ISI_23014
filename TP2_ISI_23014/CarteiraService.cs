﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Configuration;
namespace ServicoWCFSoap
{
    public class CarteiraService : IWcfCarteiraService
    {

        public CarteiraService(string wcfEndpoint)
        {
            WcfEndpoint = wcfEndpoint;
        }
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString;

        public string WcfEndpoint { get; }

        public CarteiraResponse GetCarteira(int usuarioId)
        {
            CarteiraResponse response = new CarteiraResponse();
            List<Investimento> investimentos = new List<Investimento>();

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
                                investimentos.Add(new Investimento
                                {
                                    Id = reader.GetInt32(0),
                                    TipoInvestimento = reader.GetString(1),
                                    ValorOriginal = reader.GetDecimal(2),
                                    MoedaOriginal = reader.GetString(3)
                                });
                            }
                        }
                    }
                }

                // 3. Simulação de conversão (para simplificar)
                foreach (var investimento in investimentos)
                {
                    investimento.ValorConvertido = investimento.ValorOriginal * 1.1m; // Exemplo de taxa de conversão
                }

                response.Investimentos = investimentos;
                return response;

            }
            catch (Exception ex)
            {
                throw new FaultException($"Erro ao processar a carteira: {ex.Message}");
            }

        }
    }
}
