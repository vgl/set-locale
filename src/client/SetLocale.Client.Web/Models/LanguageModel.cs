namespace SetLocale.Client.Web.Models
{
    public class LanguageModel : BaseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public static LanguageModel TR()
        {
            return new LanguageModel
            {
                Key = "tr",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/tr.png"
            };
        }
         
        public static LanguageModel EN()
        {
            return new LanguageModel
            {
                Key = "en",
                Name = "English",
                ImageUrl = "/public/img/flag/en.png"
            };
        }

        public static LanguageModel AZ()
        {
            return new LanguageModel
            {
                Key = "az",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/az.png"
            };
        }

        public static LanguageModel CN()
        {
            return new LanguageModel
            {
                Key = "cn",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/cn.png"
            };
        }

        public static LanguageModel FR()
        {
            return new LanguageModel
            {
                Key = "fr",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/fr.png"
            };
        }

        public static LanguageModel GR()
        {
            return new LanguageModel
            {
                Key = "gr",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/gr.png"
            };
        }

        public static LanguageModel IT()
        {
            return new LanguageModel
            {
                Key = "it",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/it.png"
            };
        }

        public static LanguageModel KZ()
        {
            return new LanguageModel
            {
                Key = "kz",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/kz.png"
            };
        }

        public static LanguageModel RU()
        {
            return new LanguageModel
            {
                Key = "ru",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/ru.png"
            };
        }

        public static LanguageModel SP()
        {
            return new LanguageModel
            {
                Key = "sp",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/sp.png"
            };
        }

        public static LanguageModel TK()
        {
            return new LanguageModel
            {
                Key = "tk",
                Name = "Türkçe",
                ImageUrl = "/public/img/flag/tk.png"
            };
        }

        public static bool IsValidLanguageKey(string lang)
        {
            switch (lang)
            {
                case "tr":
                    return true;
                case "en":
                    return true;
            }

            return false;
        }
    }
}