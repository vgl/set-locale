using System;

using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewAppTests
    {
        private const string BaseUrl = "http://localhost:8011/";

        [Test]
        public void should_new_app()
        {
            var browser = new FirefoxDriver();

            browser.Navigate().GoToUrl(string.Format("{0}/user/logout", BaseUrl));
            browser.Navigate().GoToUrl(string.Format("{0}/user/login", BaseUrl));

            browser.FindElementById("email").SendKeys("mehmet.sabancioglu@gmail.com");
            browser.FindElementById("password").SendKeys("password");
            browser.FindElementById("frm").Submit();

            browser.Navigate().GoToUrl(string.Format("{0}/app/new", BaseUrl));

            browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            browser.FindElementById("url").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            browser.FindElementById("description").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            browser.FindElementById("frm").Submit();

            browser.Close();
        }
    }
}