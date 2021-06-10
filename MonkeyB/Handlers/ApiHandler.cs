﻿using Newtonsoft.Json;
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
            Uri test = new Uri(ApiEndPoint, AddCoinsIdUrl("butcoin"));
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
        
        public MarketGraph Marketgraph = new MarketGraph();
        public async Task<MarketGraph> GetMarketData()
        {
            Uri url = new Uri(
                "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range?vs_currency=eur&from=1392577232&to=1422577232");
            
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
            
            Trace.Write(Marketgraph.prices[0][1]);
            return await Task.FromResult(Marketgraph);

        }

    }
}
