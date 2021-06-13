using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class CryptoWalletModel
    {

        public string coinName { get; set; }
        public float coinAmount { get; set; }

        public CryptoWalletModel(string coinName, float coinAmount)
        {
            this.coinName = coinName;
            this.coinAmount = coinAmount;
        }

    }
}
