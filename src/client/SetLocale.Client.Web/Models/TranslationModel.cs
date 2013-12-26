using System.Collections.Generic;

namespace SetLocale.Client.Web.Models
{
    public class TranslationModel : BaseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public string LanguageKey { get; set; }
        public string Language { get; set; }
        public string LanguageImageUrl { get; set; }

        public string Description { get; set; }
        public List<TagModel> Tags { get; set; }
        

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Key)
                   && !string.IsNullOrEmpty(Value);
        }
    }
}