namespace set.locale.Data.Entities
{
    public class Tag : BaseEntity
    {
        public string AppId { get; set; }
        public virtual App App { get; set; }

        public string UrlName { get; set; }
    }
}