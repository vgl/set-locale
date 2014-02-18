namespace set.locale.Models
{
    public class HomeStatsModel : BaseModel
    {
        public int DeveloperCount { get; set; }
        public int ApplicationCount { get; set; }
        public int TranslatorCount { get; set; }
        public int KeyCount { get; set; }
        public int TranslationCount { get; set; }

        public string Summary { get; set; }
    }
}