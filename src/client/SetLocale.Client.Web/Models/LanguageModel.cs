using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLocale.Client.Web.Models
{
    public class LanguageModel : BaseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public string Language { get; set; }
        public string Value { get; set; }
        public string FlagImageUrl { get; set; }

    }
}