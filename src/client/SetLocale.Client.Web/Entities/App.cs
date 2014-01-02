using System.Collections.Generic;

namespace SetLocale.Client.Web.Entities
{
    public class App : BaseEntity
    {
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Token> Tokens { get; set; }
    }

    public class Token : BaseEntity
    {
        public int AppId { get; set; }
        public App App { get; set; }
        
        public string Key { get; set; }
        public int UsageCount { get; set; }
    }
}