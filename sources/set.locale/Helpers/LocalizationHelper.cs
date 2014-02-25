using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Collections.Generic;

namespace set.locale.Helpers
{
    public static class LocalizationHelper
    {
        public static string Localize(this HtmlHelper helper, string key)
        {
            try
            {
                var localize = ((Dictionary<string, string>)HttpContext.Current.Application[Thread.CurrentThread.CurrentUICulture.Name])[key];
                return string.IsNullOrEmpty(localize) ? key : localize;
            }
            catch { return key; }
        }

        public static string Localize(string key)
        {
            return Localize(null, key);
        }
    }
}