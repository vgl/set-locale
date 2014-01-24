using System;
using System.Threading.Tasks;

using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewTranslatorTests
    {
        private const string BaseUrl = "http://localhost:8011/";

        [Test]
        public async void should_add_new_translator()
        {
            await Task.Factory.StartNew(() =>
            {
                var browser = new FirefoxDriver();

                browser.Navigate().GoToUrl(string.Format("{0}/user/logout", BaseUrl));
                browser.Navigate().GoToUrl(string.Format("{0}/user/login", BaseUrl));

                browser.FindElementById("email").SendKeys("mehmet.sabancioglu@gmail.com");
                browser.FindElementById("password").SendKeys("password");
                browser.FindElementById("frm").Submit();

                browser.Navigate().GoToUrl(string.Format("{0}/admin/newtranslator", BaseUrl));

                browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
                browser.FindElementById("email").SendKeys(Guid.NewGuid().ToString().Replace("-", "") + "@gmail.com");
                browser.FindElementById("frm").Submit();

                browser.Close();
            });
        }
    }
}