using System;

using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class MembershipTests
    {
        private const string BaseUrl = "http://localhost:8011/";

        [Test]
        public void should_login()
        {
            var browser = new FirefoxDriver();

            browser.Navigate().GoToUrl(string.Format("{0}/user/logout", BaseUrl));
            browser.Navigate().GoToUrl(string.Format("{0}/user/login", BaseUrl));

            browser.FindElementById("email").SendKeys("hserdarb@gmail.com");
            browser.FindElementById("password").SendKeys("password");
            browser.FindElementById("frm").Submit();

            browser.Close();
        }

        [Test]
        public void should_signup()
        {
            var browser = new FirefoxDriver();

            browser.Navigate().GoToUrl(string.Format("{0}/user/new", BaseUrl));

            browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            browser.FindElementById("email").SendKeys(Guid.NewGuid().ToString().Replace("-", "") + "@gmail.com");
            browser.FindElementById("password").SendKeys("password");
            browser.FindElementById("frm").Submit();

            browser.Close();
        }
    }
}