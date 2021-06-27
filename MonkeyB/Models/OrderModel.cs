namespace MonkeyB.Models
{
    public class OrderModel
    {
        public OrderModel(int id, string coinType, float coin_amount, float euro_amount, bool outstanding, int UserID)
        {
            ID = id;
            CoinName = coinType;
            Amount = coin_amount;
            EuroAmount = euro_amount;
            Outstanding = outstanding;
            this.UserID = UserID;
        }

        public int ID { get; set; }
        public string CoinName { get; set; }
        public float Amount { get; set; }
        public float EuroAmount { get; set; }
        public bool Outstanding { get; set; }
        public int UserID { get; set; }
    }
}