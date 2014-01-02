using System;

namespace SetLocale.Client.Web.Models
{
    public class TokenModel : BaseModel
    {
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateStr
        {
            get
            {
                return CreationDate.ToString("f");
            }
        }

        public int UsageCount { get; set; }
        public int CreatedBy { get; set; }
        public int AppId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token);
        }
    }
}