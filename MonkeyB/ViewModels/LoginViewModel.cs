using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.ViewModels
{
    class LoginViewModel
    {
        public ApiHandler api = new ApiHandler();

        public LoginViewModel() {
            api.GetApiData();
        }

    }
}
