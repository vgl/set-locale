using System;

using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    public class BaseUITest
    {
        public const string BASE_URL = "http://localhost:3881";

        public const string ACTION_LOGIN = "/user/login";
        public const string ACTION_LOGOUT = "/user/logout";
        public const string ACTION_NEW = "/user/new";
        public const string ACTION_APPLICATION_NEW = "/app/new";
        public const string ACTION_NEW_TRANSLATOR = "/admin/newtranslator";
        public const string ACTION_NEW_WORD = "/word/new";
        public const string ACTION_WORD_ALL = "/word/all";
        public const string ERROR_PREV_PAGE_NOT_EXIS = "There is no prevPageButton in page.";

        public FirefoxDriver _browser;

        [SetUp]
        public void Setup()
        {
            _browser = new FirefoxDriver();
        }

        public void UrlTest(string url)
        {
            GoTo(url);

            if (_browser.Url != url)
            {
                try
                {
                    Assert.AreEqual(_browser.Url, url);
                }
                finally
                {
                    _browser.Close();
                }
            }
        }

        public void NextPageButtonTest(string url)
        { 
            GoTo(string.Format("{0}{1}", url, "/1"));

            var nextPageButton = _browser.FindElementById("nextPage"); 
            if (nextPageButton != null)
            {
                nextPageButton.Click();
                UrlTest(string.Format("{0}{1}", url, "/2"));
            } 
        }

        public void PrevPageButtonTest(string url)
        { 
            GoTo(string.Format("{0}{1}", url, "/1"));
              
            var nextPageButton = _browser.FindElementById("nextPage");
            if (nextPageButton != null)
            {
                UrlTest(string.Format("{0}{1}", url, "/2"));

                var prevPage = _browser.FindElementById("prevPage");
                if (prevPage != null)
                {
                    UrlTest(string.Format("{0}{1}", url, "/1"));
                }
                else
                {
                    var err = new AssertionException(ERROR_PREV_PAGE_NOT_EXIS);
                    throw err;
                } 
            } 
        }

        public void ClickSave()
        {
            _browser.FindElementById("btn_save").Click();
        }

        public void ClickSaveAndNew()
        {
            _browser.FindElementById("btn_save_and_new").Click();
        }

        public void ClickCancel()
        {
            _browser.FindElementByCssSelector("button[type=reset]").Click();
        }

        public void LoginAsAdmin()
        {
            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_LOGIN));

            _browser.FindElementById("email").SendKeys("hserdarb@gmail.com");
            _browser.FindElementById("password").SendKeys("password");
            _browser.FindElementById("frm").Submit();
        }

        public void LogOut()
        {
            GoTo(string.Format("{0}/user/logout", BASE_URL));
        }

        public void CloseBrowser()
        {
            _browser.Close();
        }

        public void Wait(int second)
        {
            _browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(second));
        }

        public void GoTo(string url)
        {
            _browser.Navigate().GoToUrl(url);
        }

    }
}