namespace Restful_Service.Models
{
    public class Investimento
    {
        public int Id { get; set; }
        public string TipoInvestimento { get; set; }
        public decimal ValorOriginal { get; set; }
        public string MoedaOriginal { get; set; }
        public decimal ValorConvertido { get; set; }
    }
}
