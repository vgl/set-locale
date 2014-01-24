using System;
using System.Threading.Tasks;

using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewWordTests
    {
        private const string BaseUrl = "http://localhost:8011/";

        [Test]
        public async void should_add_new_word()
        {
            await Task.Factory.StartNew(() =>
            {
                var browser = new FirefoxDriver();

                browser.Navigate().GoToUrl(string.Format("{0}/user/logout", BaseUrl));
                browser.Navigate().GoToUrl(string.Format("{0}/user/login", BaseUrl));

                browser.FindElementById("email").SendKeys("mehmet.sabancioglu@gmail.com");
                browser.FindElementById("password").SendKeys("password");
                browser.FindElementById("frm").Submit();

                browser.Navigate().GoToUrl(string.Format("{0}/word/new", BaseUrl));

                browser.FindElementById("key").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
                browser.FindElementById("tag").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
                browser.FindElementById("description").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
                browser.FindElementById("frm").Submit();

                browser.Close();
            });
        }
    }
}