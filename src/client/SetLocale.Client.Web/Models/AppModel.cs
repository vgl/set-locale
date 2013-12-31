using System.Collections.Generic;

namespace SetLocale.Client.Web.Models
{
    public class AppModel : BaseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }

        public List<TokenModel> Tokens { get; set; }

        public AppModel()
        {
            Tokens = new List<TokenModel>();
        }

        public bool IsValidForNew()
        {
            return !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Url)
                   && !string.IsNullOrEmpty(Description);

        }
    }
}