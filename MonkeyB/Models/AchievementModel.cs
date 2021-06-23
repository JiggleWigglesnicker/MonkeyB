using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Models
{
    public class AchievementModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public AchievementModel(string name, string description, bool completed)
        {
            Name = name;
            Description = description;
            IsCompleted = completed;
        }

    }
}
