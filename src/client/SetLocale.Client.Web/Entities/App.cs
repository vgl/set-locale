namespace SetLocale.Client.Web.Entities
{
    public class App : BaseEntity
    {
        public string OwnerEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }
    }
}