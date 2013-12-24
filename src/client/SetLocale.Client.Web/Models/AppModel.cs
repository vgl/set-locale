namespace SetLocale.Client.Web.Models
{
    public class AppModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public string Url { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }
    }
}