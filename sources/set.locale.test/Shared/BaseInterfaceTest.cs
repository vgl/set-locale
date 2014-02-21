using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace set.locale.test.Shared
{
    public class BaseInterfaceTest
    {
        public const string BASE_URL = "http://localhost:8033";

        public const string ACTION_HOME = "/";
        public const string ACTION_CONTACT = "/home/contact";

        public const string ACTION_LOGIN = "/user/login";
        public const string ACTION_LOGOUT = "/user/logout";
        public const string ACTION_SIGNUP = "/user/new";
        public const string ACTION_PASSWORD_RESET = "/user/passwordreset";
        public const string ACTION_USER_PROFILE = "/user/detail";

        public const string ACTION_NEW_DOMAIN_OBJECT = "/domainobject/new";
        public const string ACTION_LIST_DOMAIN_OBJECTS = "/domainobject/list";
        public const string ACTION_DOMAIN_OBJECT_DETAIL = "/domainobject/detail";

        public const string ACTION_NEW_WORD = "/word/new";
        public const string ACTION_LIST_ALL_WORDS = "/word/all";
        public const string ACTION_LIST_NOT_TRANSLATED = "/word/nottranslated";
        public const string ACTION_LIST_USER_WORDS = "/user/words";
        public const string ACTION_NEW_APP = "/app/new";
        public const string ACTION_LIST_USER_APP = "/user/apps";

        public const string ACTION_NEW_TRANSLATOR = "/admin/newtranslator";
        public const string ACTION_LIST_ADMIN_APPS = "/admin/apps";
        public const string ACTION_ADMIN_EXCEL_IMPORT = "/admin/import";

        public const string ACTION_ADMIN_USER_LISTING = "/admin/users";
        public const string ACTION_ADMIN_FEEDBACK_LISTING = "/admin/feedbacks";
        public const string ACTION_ADMIN_CONTACT_MESSAGES_LISTING = "/admin/contactmessages";

        public const string ACTION_SEARCH_QUERY = "/search/query?text=key";


        public FirefoxDriver Browser;

        [SetUp]
        public void Setup()
        {
            Browser = new FirefoxDriver();
        }

        public void LoginAsUser()
        {
            LogOut();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_LOGIN));

            Browser.FindElementById("Email").SendKeys("user@test.com");
            Browser.FindElementById("Password").SendKeys("password");
            Browser.FindElementByClassName("btn-success").Submit();
        }

        public void LoginAsAdmin()
        {
            LogOut();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_LOGIN));

            Browser.FindElementById("Email").SendKeys("admin@test.com");
            Browser.FindElementById("Password").SendKeys("password");
            Browser.FindElementByClassName("btn-success").Submit();
        }

        public void LogOut()
        {
            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_LOGOUT));
        }

        public void GoTo(string url)
        {
            Browser.Navigate().GoToUrl(url);
        }

        public void AssertUrl(string url)
        {
            Assert.IsNotNull(Browser);
            Assert.AreEqual(Browser.Url, url);
        }

        public void AssertNotUrl(string url)
        {
            Assert.IsNotNull(Browser);
            Assert.AreNotEqual(Browser.Url, url);
        }

        public void CloseBrowser()
        {
            Browser.Close();
        }
    }
}