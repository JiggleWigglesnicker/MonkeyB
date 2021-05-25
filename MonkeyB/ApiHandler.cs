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
