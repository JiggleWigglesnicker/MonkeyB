using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonkeyB
{
    class ApiHandler
    {
        public static readonly Uri ApiEndPoint = new("https://api.coingecko.com/api/v3/");
        public static readonly string Coins = "coins";
        public static readonly string CoinList = "coins/list";
        public static readonly string CoinMarkets = "coins/markets";

        /// <summary>
        ///     Retrieves the market data (price points) for a specific cryptocurrency over a certain period of days
        /// </summary>
        private MarketGraph Marketgraph = new();

        public CryptoCurrencyModel model = new CryptoCurrencyModel();
        public static string AddCoinsIdUrl(string id) => "simple/price?ids=" + id + "&vs_currencies=eur";

        /// <summary>
        ///     MarketChart data by coin id
        /// </summary>
        /// <param name="id">crypto id (e.g bitcoin,dogecoin)</param>
        /// <param name="currency">usd, eur, jpy, etc</param>
        /// <param name="days">amount of days for data, data is hourly til 90 days</param>
        /// <returns></returns>
        public static string MarketChartByCoinId(string id, string currency, int days) =>
            $"coins/{id}/market_chart?vs_currency={currency}&days={days}&interval=daily";

        public static string MarketChartRangeByCoinId(string id, string currency, int startdate, int enddate) =>
            AddCoinsIdUrl(id) + "/market_chart/range?vs_currency=" + currency + "&from=" + startdate + "&to=" + enddate;

        /// <summary>
        ///     Retrieves the coinvalue of a certain cryptocurrency and stores it in a CryptoCurrencyModel
        /// </summary>
        /// <param name="coincode">name of cryptocurrency</param>
        /// <returns>returns a CryptoCurrencyModel</returns>
        public async Task<CryptoCurrencyModel> GetCoinValue(string coincode)
        {
            CryptoCurrencyModel ccm = new();

            Uri test = new(ApiEndPoint, AddCoinsIdUrl(coincode));
            HttpClient httpclient = new();
            var response = await httpclient.GetStringAsync(test);

            try
            {
                ccm = JsonConvert.DeserializeObject<CryptoCurrencyModel>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return await Task.FromResult(ccm);
        }

        public async Task<MarketGraph> GetMarketData(string id, String currency, int days)
        {
            Uri url = new Uri(ApiEndPoint, MarketChartByCoinId(id, currency, days));

            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetStringAsync(url);

            try
            {
                Marketgraph = JsonConvert.DeserializeObject<MarketGraph>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return await Task.FromResult(Marketgraph);
        }
    }
}