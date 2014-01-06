using System.Collections.Generic;

namespace SetLocale.Client.Web.Entities
{
    public class Word : BaseEntity
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public bool IsTranslated { get; set; } 
        public int TranslationCount { get; set; } 
        public string Translation_TR { get; set; }
        public string Translation_EN { get; set; }
        public string Translation_AZ { get; set; }
        public string Translation_CN { get; set; }
        public string Translation_FR { get; set; }
        public string Translation_GR { get; set; }
        public string Translation_IT { get; set; }
        public string Translation_KZ { get; set; }
        public string Translation_RU { get; set; }
        public string Translation_SP { get; set; }
        public string Translation_TK { get; set; }
        
        public virtual ICollection<Tag> Tags { get; set; }
    }
}