using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class TransactionHistoryModel
    {
        public string coinName { get; set; }
        public float coinAmount { get; set; }
        public float coinValue { get; set; }
        public float coinPercentage { get; set; }
        public float oldCoinValue { get; set; }

        public TransactionHistoryModel(string coinName, float coinAmount)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
            //this.coinValue = coinValue;
        }

        public void calculatePercentage()
        {
            coinPercentage = 100 * ((coinValue - oldCoinValue) / oldCoinValue);

            //coinPercentage = 50;
        }
    }
}
