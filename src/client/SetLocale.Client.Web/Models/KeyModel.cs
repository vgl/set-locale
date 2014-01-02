using SetLocale.Client.Web.Entities;
using System.Collections.Generic;

namespace SetLocale.Client.Web.Models
{
    public class KeyModel : BaseModel
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public bool IsTranslated { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<LanguageModel> Languages { get; set; }
        public List<TranslationModel> Translations { get; set; }
        public int CreatedBy { get; set; }

        public KeyModel()
        {
            Tags = new List<TagModel>();
            Languages = new List<LanguageModel>();
            Translations = new List<TranslationModel>();
        }

        public bool IsValidForNew()
        {
            return !string.IsNullOrEmpty(Key)
                   && !string.IsNullOrEmpty(Tag);
        }
        public static List<KeyModel> MapWordToKeyModel(IEnumerable<Word> words)
        {
            var model = new List<KeyModel>();
            foreach (var key in words)
            {
                model.Add(new KeyModel
                {
                    Key = key.Key,
                    Description = key.Description,
                    IsTranslated = key.IsTranslated,
                    CreatedBy = key.CreatedBy
                });
            }
            return model;
        }
        public static KeyModel MapIdToKeyModel(Word word)
        {
            var model = new KeyModel
            {
                Key = word.Key,
                Description=word.Description,
                IsTranslated = word.IsTranslated

            };
            return model;
        }
    }
}