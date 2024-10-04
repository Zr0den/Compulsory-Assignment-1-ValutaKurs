using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutaClient.Core.Entities
{
    public class Valuta
    {
        public int ValutaId { get; set; }
        public string Status { get; set; }
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public decimal Value { get; set; }
        public decimal Rate { get; set; }
    }
}
