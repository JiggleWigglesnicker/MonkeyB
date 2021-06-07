using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    class AchievementModel
    {

        public String Name { get; set; }
        public String Description { get; set; }
        public bool IsCompleted { get; set; }

        public AchievementModel(string name, string description, bool completed)
        {
            Name = name;
            Description = description;
            IsCompleted = completed;
        }

    }
}
