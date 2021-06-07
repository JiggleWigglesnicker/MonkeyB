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
        public static string MarketChartRangeByCoinId(string id) => AddCoinsIdUrl(id) + "/market_chart/range";

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

    }
}
