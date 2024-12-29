using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServicoWCFSoap
{
    [ServiceContract]
    public interface ICarteiraService
    {
        [OperationContract]
       Task<CarteiraResponse> ObterCarteira(int usuarioId,string moedaPretendida = null);
    }

    [DataContract] // Define que esta classe será usada em serialização WCF
    public class CarteiraResponse
    {
        [DataMember] // Indica que esta propriedade será serializada
        public string MoedaDefault { get; set; }

        [DataMember]
        public List<Investimento> Investimentos { get; set; }
    }

    [DataContract]
    public class Investimento
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TipoInvestimento { get; set; }

        [DataMember]
        public decimal ValorOriginal { get; set; }

        [DataMember]
        public string MoedaOriginal { get; set; }

        [DataMember]
        public decimal ValorConvertido { get; set; }
    }
}
