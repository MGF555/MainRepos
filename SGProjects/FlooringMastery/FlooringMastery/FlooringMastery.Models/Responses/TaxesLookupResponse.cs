using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models.Responses
{
    public class TaxesLookupResponse
    {
        public IEnumerable<Taxes> Taxes { get; set; }
    }
}
