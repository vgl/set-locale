using System;
using NUnit.Framework;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewTranslatorTests : BaseUITest
    {
        [Test]
        public void should_add_new_translator()
        {
            LogOut();
            LoginAsAdmin();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_NEW_TRANSLATOR));

            _browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("email").SendKeys(Guid.NewGuid().ToString().Replace("-", "") + "@gmail.com");
            _browser.FindElementById("frm").Submit();

            _browser.Close();
        }
    }
}