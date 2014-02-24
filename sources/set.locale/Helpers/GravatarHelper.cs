using System.Security.Cryptography;
using System.Web.Mvc;

namespace set.locale.Helpers
{
    public static class GravatarHelper
    {
        public static string Gravatar(this HtmlHelper helper, string email, int size)
        {
            const string result = "<img src=\"{0}\" alt=\"gravatar\" class=\"gravatar\" />";
            var url = GetGravatarURL(email, size);
            return string.Format(result, url);
        }

        public static string GetGravatarURL(string email, int size)
        {
            return string.Format("//gravatar.com/avatar/{0}?s={1}&r=PG", EncryptMD5(email), size);
        }

        public static string GetGravatarURL(string email, int size, string defaultImagePath)
        {
            return string.Format("{0}&default={1}", GetGravatarURL(email, size), defaultImagePath);
        }

        static string EncryptMD5(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var valueArray = System.Text.Encoding.ASCII.GetBytes(value);
            valueArray = md5.ComputeHash(valueArray);
            var encrypted = string.Empty;
            for (var i = 0; i < valueArray.Length; i++)
            {
                encrypted += valueArray[i].ToString("x2").ToLower();
            }
            return encrypted;
        }
    }
}