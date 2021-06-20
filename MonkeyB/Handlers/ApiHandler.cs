using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using MonkeyB.Models;
using Microsoft.Data.Sqlite;

namespace MonkeyB
{
    class ApiHandler
    {
        public static readonly Uri ApiEndPoint = new("https://api.coingecko.com/api/v3/");
        public static readonly string Coins = "coins";
        public static readonly string CoinList = "coins/list";
        public static readonly string CoinMarkets = "coins/markets";
        public static string AddCoinsIdUrl(string id) => "simple/price?ids=" + id + "&vs_currencies=eur";

        /// <summary>
        /// MarketChart data by coin id
        /// </summary>
        /// <param name="id">crypto id (e.g bitcoin,dogecoin)</param>
        /// <param name="currency">usd, eur, jpy, etc</param>
        /// <param name="days">amount of days for data, data is hourly til 90 days</param>
        /// <returns></returns>
        public static string MarketChartByCoinId(string id, string currency, int days) =>
            AddCoinsIdUrl(id) + "/market_chart?vs_currency=" + currency + "&days=" + days + "&interval=daily";
        public static string MarketChartRangeByCoinId(string id, string currency, int startdate, int enddate) =>
            AddCoinsIdUrl(id) + "/market_chart/range?vs_currency=" + currency + "&from=" + startdate + "&to=" + enddate;

        public CryptoCurrencyModel model = new CryptoCurrencyModel();

        public async Task<CryptoCurrencyModel> GetApiData()
        {
            string url = "https://api.coingecko.com/api/EuroAmount/coins/bitcoin?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false";
            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetStringAsync(url);

            try
            {
                model = JsonConvert.DeserializeObject<CryptoCurrencyModel>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return await Task.FromResult(model);
        }



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

        private MarketGraph Marketgraph = new ();
        public async Task<MarketGraph> GetMarketData(string id, String currency, int days)
        {
            Uri url = new Uri(ApiEndPoint, MarketChartByCoinId(id, currency,days));

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
            
            // Trace.Write(Marketgraph.prices[0][1]);
            return await Task.FromResult(Marketgraph);
        }



    }
}
