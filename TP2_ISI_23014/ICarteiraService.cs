using System.Collections.Generic;
using System.ServiceModel;

namespace ServicoWCFSoap
{
    [ServiceContract]
    public interface ICarteiraService
    {
        [OperationContract]
        CarteiraResponse  GetCarteira(int usuarioId);
    }

    public class CarteiraResponse
    {
        public string MoedaDefault { get; set; }
        public List<Investimento> Investimentos { get; set; }
    }

    public class Investimento
    {
        public int Id { get; set; }
        public string TipoInvestimento { get; set; }
        public decimal ValorOriginal { get; set; }
        public string MoedaOriginal { get; set; }
        public decimal ValorConvertido { get; set; }
    }
}
