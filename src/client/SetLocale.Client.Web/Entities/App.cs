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
}