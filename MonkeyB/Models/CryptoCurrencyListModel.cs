using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class CryptoCurrencyListModel
    {
        public CurrencyList[] currencyList { get; set; }
    }

    public class CurrencyList
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
    }

}
