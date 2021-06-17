using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    class RSSModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public RSSModel(string subject, string summary)
        {
            this.Title = subject;
            this.Description = summary;
        }


    }
}
