using System.Collections.Generic;
using System.Linq;

using set.locale.Data.Entities;

namespace set.locale.Models
{
    public class AppModel : BaseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public List<TokenModel> Tokens { get; set; }
        public List<WordModel> Words { get; set; }
        public int UsageCountToken
        {
            get { return Tokens.Sum(x => x.UsageCount); }
        }
        public int UsageCountWord
        {
            get { return Words.Count(); }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Url);

        }
        public bool IsNotValid()
        {
            return !IsValid();

        }

        public AppModel()
        {
            Tokens = new List<TokenModel>();
            Words = new List<WordModel>();
        }

        public static AppModel Map(App entity)
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

            var words = entity.Words.Where(x => x.IsActive && !x.IsDeleted);
            var wm = words.Select(WordModel.Map);
            model.Words.AddRange(wm.ToList());

            return model;
        }
    }
}