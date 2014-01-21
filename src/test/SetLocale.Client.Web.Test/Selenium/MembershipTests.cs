using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class MembershipTests
    {
        FirefoxDriver _browser;
        private const string BaseUrl = "http://localhost:8011/";

        [TestFixtureSetUp]
        public void Setup()
        {
            _browser = new FirefoxDriver();
        }

        [Test]
        public void should_login()
        {
            _browser.Navigate().GoToUrl(string.Format("{0}/user/logout", BaseUrl));

            _browser.Navigate().GoToUrl(string.Format("{0}/user/login", BaseUrl));

            _browser.FindElementById("email").SendKeys("hserdarb@gmail.com");
            _browser.FindElementById("password").SendKeys("password");
            _browser.FindElementById("frm").Submit();

            _browser.Close();
        }
    }
}