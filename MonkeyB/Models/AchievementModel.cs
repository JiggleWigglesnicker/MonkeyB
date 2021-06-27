namespace MonkeyB.Models
{
    public class AchievementModel
    {
        public AchievementModel(string name, string description, bool completed)
        {
            Name = name;
            Description = description;
            IsCompleted = completed;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}