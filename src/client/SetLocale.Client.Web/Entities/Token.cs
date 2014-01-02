namespace SetLocale.Client.Web.Entities
{
    public class Token : BaseEntity
    {
        public int AppId { get; set; }
        public App App { get; set; }
        
        public string Key { get; set; }
        public int UsageCount { get; set; }
    }
}