using System;

namespace set.locale.Models
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
        public string CreatedBy { get; set; }
        public string AppId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token);
        }
    }
}