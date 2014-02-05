using System;

using NUnit.Framework;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewWordTests : BaseUITest
    {
        [Test]
        public void should_add_new_word()
        {
            LogOut();
            LoginAsAdmin();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_NEW_WORD));

            _browser.FindElementById("key").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("tag").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("description").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("frm").Submit();

            _browser.Close();
        }
    }
}