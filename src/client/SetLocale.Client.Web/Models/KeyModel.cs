using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLocale.Client.Web.Models
{
    public class KeyModel : BaseModel
    {

        public string Key { get; set; }
        public List<TagModel> Tag { get; set; }
        public string Description { get; set; }
        public List<LanguageModel> Languages {get; set;}
        public bool IsTranslated { get; set; }

         
    }
}