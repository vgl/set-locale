using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLocale.Client.Web.Models
{
    public class NewKeyModel : BaseModel
    {

        public string Key { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Key)
                   && !string.IsNullOrEmpty(Tag)
                   && !string.IsNullOrEmpty(Description);
        }

    }
}