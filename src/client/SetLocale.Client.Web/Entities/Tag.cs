namespace SetLocale.Client.Web.Entities
{
    public class Tag : BaseEntity
    {
        public int WordId { get; set; }
        public Word Word { get; set; }

        public string Name { get; set; }
        public string UrlName { get; set; }
    }
}