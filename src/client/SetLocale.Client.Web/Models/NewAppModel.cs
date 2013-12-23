using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLocale.Client.Web.Models
{
    public class NewAppModel : BaseModel
    {
        public string AppName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AppName)
                   && !string.IsNullOrEmpty(Url)
                   && !string.IsNullOrEmpty(Description);

        }
    }
}