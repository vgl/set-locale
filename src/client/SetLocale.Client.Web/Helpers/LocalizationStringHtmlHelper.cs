using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SetLocale.Client.Web.Helpers
{
    public static class LocalizationStringHtmlHelper
    {
        public static string LocalizationString(this HtmlHelper helper, string key)
        {
            try
            {
                var dictionary = (Dictionary<string, string>)HttpContext.Current.Application[string.Format("{0}_txt", Thread.CurrentThread.CurrentUICulture.Name)];
                return dictionary[key];
            }
            catch (Exception)
            {
                return key;
            }
        }

        public static string LocalizationString(string key)
        {
            return LocalizationString(null, key);
        }
    }
}