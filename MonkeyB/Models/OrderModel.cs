using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class OrderModel
    {
        public int ID { get; set; }
        public string CoinName { get; set; }
        public float Amount { get; set; }
        public float EuroAmount { get; set; }
        public bool Outstanding { get; set; }
        public int UserID { get; set; }


        public OrderModel(int id,string coinType, float coin_amount, float euro_amount, bool outstanding, int UserID)
        {
            this.ID = id;
            this.CoinName = coinType;
            this.Amount = coin_amount;
            this.EuroAmount = euro_amount;
            this.Outstanding = outstanding;
            this.UserID = UserID;
        }
    }
}
