using System;

namespace SetLocale.Client.Web.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateStr { get; set; }

        public int UsageCount { get; set; }
    }
}