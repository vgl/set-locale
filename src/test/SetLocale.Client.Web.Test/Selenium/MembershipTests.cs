using System;

using NUnit.Framework;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class MembershipTests : BaseUITest
    {
        [Test]
        public void should_login()
        { 
            LogOut();

            LoginAsAdmin();

            _browser.Close();
        }

        [Test]
        public void should_signup()
        {
            LogOut();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_NEW));

            _browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("email").SendKeys(Guid.NewGuid().ToString().Replace("-", "") + "@gmail.com");
            _browser.FindElementById("password").SendKeys("password");
            _browser.FindElementById("frm").Submit();

            _browser.Close();
        }
    }
}