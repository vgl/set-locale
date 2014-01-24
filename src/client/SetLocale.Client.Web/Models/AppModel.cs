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

        public int UsageCount
        {
            get { return Tokens.Sum(x => x.UsageCount); }
        }

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
            var model = new AppModel
            {
                Id = entity.Id,
                Email = entity.UserEmail,
                IsActive = entity.IsActive,
                Name = entity.Name,
                Description = entity.Description,
                Url = entity.Url != null && entity.Url.StartsWith("http") ? entity.Url
                                                                          : string.Format("http://{0}", entity.Url)
            };

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