using System.Collections.Generic;
using System.Globalization;

namespace SetLocale.Util
{
    public static class ConstHelper
    {
        private static CultureInfo _cultureTR;
        public static CultureInfo CultureTR
        {
            get { return _cultureTR ?? (_cultureTR = new CultureInfo("tr-TR")); }
        }

        private static CultureInfo _cultureEN;
        public static CultureInfo CultureEN
        {
            get { return _cultureEN ?? (_cultureEN = new CultureInfo("en-US")); }
        }

        public const string Admin = "Admin";
        public const string Developer = "Developer";
        public const string User = "User";

        static List<string> BasicRoles = new List<string> { Admin, Developer, User };

        public const string tr_txt = "tr_txt";
        public const string en_txt = "en_txt";

        public const string tr = "tr";
        public const string en = "en";

        public const string __Lang = "__Lang";

    }
}
