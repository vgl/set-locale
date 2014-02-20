using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Collections.Generic;

namespace set.locale.Helpers
{
    public static class LocalizationHelper
    {
        public static string LocalizeString(this HtmlHelper helper, string key)
        {
            try
            {
                return ((Dictionary<string, string>)HttpContext.Current.Application[Thread.CurrentThread.CurrentUICulture.Name])[key];
            }
            catch { return key; }
        }

        public static string LocalizeString(string key)
        {
            return LocalizeString(null, key);
        }
    }
}