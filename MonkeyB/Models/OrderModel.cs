using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class OrderModel
    {
        public string Coin { get; set; }
        public float Amount { get; set; }
        public float EuroAmount { get; set; }
        public bool Outstanding { get; set; }

        public OrderModel(string coinType, float coin_amount, float euro_amount, bool outstanding)
        {
            this.Coin = coinType;
            this.Amount = coin_amount;
            this.EuroAmount = euro_amount;
            this.Outstanding = outstanding;
        }
    }
}
