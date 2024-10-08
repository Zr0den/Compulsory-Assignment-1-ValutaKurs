namespace SysAPI
{
    public class ValutaItems
    {
        public int ValutaId { get; set; }
        public string Status { get; set; }
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public decimal Value { get; set; }
    }
}
