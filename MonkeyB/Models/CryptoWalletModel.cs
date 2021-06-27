namespace MonkeyB.Models
{
    public class CryptoWalletModel
    {
        public CryptoWalletModel(string coinName, float coinAmount, float euroAmount)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
            this.euroAmount = euroAmount;
        }

        public string coinName { get; set; }
        public float coinAmount { get; set; }
        public float euroAmount { get; set; }
    }
}