using System.Collections.Generic;
using System.Linq;
using SetLocale.Client.Web.Entities;

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
        public int CreatedBy { get; set; }

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

        public static AppModel MapFromEntity(App entity)
        {
            var model = new AppModel();
            model.Id = entity.Id;
            model.Email = entity.UserEmail;
            model.IsActive = entity.IsActive;
            model.Name = entity.Name;
            model.Description = entity.Description;

            if (entity.Url.StartsWith("http"))
            {
                model.Url = entity.Url;               
            }
            else
            {
                model.Url = "http://" + entity.Url;
            }

            var tokens = entity.Tokens.Where(x => !x.IsDeleted);
            foreach (var token in tokens)
            {
                model.Tokens.Add(new TokenModel
                {
                    CreationDate = token.CreatedAt,
                    UsageCount = token.UsageCount,
                    Token = token.Key
                });
            }

            return model;
        }
    }
}