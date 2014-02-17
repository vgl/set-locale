using System.Collections.Generic;

namespace set.locale.Data.Entities
{
    public class App : BaseEntity
    {
        public string UserEmail { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}