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
        public float profitLossValue { get; set; }
        public string percentageColor { get; set; }


        public TransactionHistoryModel(string coinName, float coinAmount, float oldCoinValue)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
            this.oldCoinValue = oldCoinValue;
        }

        public TransactionHistoryModel(string coinName, float coinAmount)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
        }

        public void calculateProfitOrLoss()
        {
            profitLossValue = (oldCoinValue - coinValue) * coinAmount;
            if (profitLossValue < 0 || profitLossValue < 0.0 || profitLossValue == 0)
            {
                percentageColor = "Red";
            }
            else
            {
                percentageColor = "Green";
            }

        }

        public void calculatePercentage()
        {
            coinPercentage = 100 * (coinValue - oldCoinValue) / oldCoinValue;
            if (coinPercentage < 0 || coinPercentage < 0.0 || coinPercentage == 0)
            {
                percentageColor = "Red";
            }
            else
            {
                percentageColor = "Green";
            }
            
        }
    }
}
