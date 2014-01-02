using System.Collections.Generic;

namespace SetLocale.Client.Web.Entities
{
    public class Word : BaseEntity
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public bool IsTranslated { get; set; }

        public string Translation_TR { get; set; }
        public string Translation_EN { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}