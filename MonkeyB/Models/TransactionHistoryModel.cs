namespace MonkeyB.Models
{
    public class TransactionHistoryModel
    {
        /// <summary>
        ///     Large constructor of TransactionHistoryModel with 3 arguments
        /// </summary>
        /// <param name="coinName"></param>
        /// <param name="coinAmount"></param>
        /// <param name="oldCoinValue"></param>
        public TransactionHistoryModel(string coinName, float coinAmount, float oldCoinValue)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
            this.oldCoinValue = oldCoinValue;
        }

        /// <summary>
        ///     Small constructor of TransactionHistoryModel with 2 arguments
        /// </summary>
        /// <param name="coinName"></param>
        /// <param name="coinAmount"></param>
        public TransactionHistoryModel(string coinName, float coinAmount)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
        }

        public string coinName { get; set; }
        public float coinAmount { get; set; }
        public float coinValue { get; set; } // the real-time value of a certain coin present in wallet
        public float coinPercentage { get; set; }
        public float oldCoinValue { get; set; } // the coinvalue as it was when it was purchased by the user
        public float profitLossValue { get; set; }
        public string percentageColor { get; set; }

        /// <summary>
        ///     Calculates the percentage of the profit or loss with coinAmount
        /// </summary>
        public void calculateProfitOrLoss()
        {
            profitLossValue = (oldCoinValue - coinValue) * coinAmount;

            percentageColor = returnPercentageColor(coinPercentage);

            percentageColor = returnPercentageColor(coinPercentage);
        }

        /// <summary>
        ///     Calculates the percentage of the profit or loss with oldCoinValue
        /// </summary>
        public void calculatePercentage()
        {
            coinPercentage = 100 * (coinValue - oldCoinValue) / oldCoinValue;

            percentageColor = returnPercentageColor(coinPercentage);

            percentageColor = returnPercentageColor(coinPercentage);
        }

        /// <summary>
        ///     Returns the right color to be given to the label of the profit or loss amount
        /// </summary>
        /// <param name="coinPercentage"></param>
        /// <returns></returns>
        public string returnPercentageColor(float coinPercentage)
        {
            if (coinPercentage < 0 || coinPercentage < 0.0 || coinPercentage == 0)
            {
                return "Red";
            }
            else
            {
                return "Green";
            }
        }
    }
}