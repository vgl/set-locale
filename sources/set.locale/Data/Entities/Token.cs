namespace set.locale.Data.Entities
{
    public class Token : BaseEntity
    {
        public string AppId { get; set; }
        public App App { get; set; }

        public string Key { get; set; }
        public int UsageCount { get; set; }
        public bool IsAppActive { get; set; }
    }
}