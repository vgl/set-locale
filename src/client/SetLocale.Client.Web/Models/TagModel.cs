using System.Collections.Generic;
using SetLocale.Client.Web.Entities;

namespace SetLocale.Client.Web.Models
{
    public class TagModel : BaseModel
    {
        public string Name { get; set; }
        public string UrlName { get; set; }

        public static WordModel MapEntityToModel(Word entity)
        {
            throw new System.NotImplementedException();
        }
    }
}