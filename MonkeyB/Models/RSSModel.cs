namespace MonkeyB.Models
{
    class RSSModel
    {
        public RSSModel(string subject, string summary)
        {
            Title = subject;
            Description = summary;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}