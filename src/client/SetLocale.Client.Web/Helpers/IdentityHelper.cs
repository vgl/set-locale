using System;
using System.Security.Principal;

namespace SetLocale.Client.Web.Helpers
{
    public static class IdentityHelper
    {
        public static int GetUserId(this IIdentity identity)
        {
            return identity.IsAuthenticated ? Convert.ToInt32(identity.Name.Split('|')[0]) : 0;
        }

        public static string GetUserFullName(this IIdentity identity)
        {
            return identity.IsAuthenticated ? identity.Name.Split('|')[1] : string.Empty;
        }
    }
}