using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Messages
{
    public class ValutaResponseMessage
    {
        public int ValutaId { get; set; }
        public string Status { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Value { get; set; }
    }
}
