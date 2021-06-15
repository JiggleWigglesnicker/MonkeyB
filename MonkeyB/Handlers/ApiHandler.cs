using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB
{
    class ApiHandler
    {
        public static readonly Uri ApiEndPoint = new Uri("https://api.coingecko.com/api/v3/");
        public static readonly string Coins = "coins";
        public static readonly string CoinList = "coins/list";
        public static readonly string CoinMarkets = "coins/markets";
        public static string AddCoinsIdUrl(string id) => "coins/" + id;
        
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
            string url = "https://api.coingecko.com/api/v3/coins/bitcoin?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false";
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
