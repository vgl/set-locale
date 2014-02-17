using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace set.locale.Models
{
    public class TranslationModel : BaseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public string Name { get; set; }

        public LanguageModel Language { get; set; }

        internal bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        internal bool IsNotValid()
        {
            return !IsValid();
        }

    }
}