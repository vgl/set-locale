using System;

using NUnit.Framework;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewAppTests : BaseUITest
    { 
        [Test]
        public void should_new_app()
        {  
            LogOut();
            LoginAsAdmin();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_APPLICATION_NEW));

            _browser.FindElementById("name").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("url").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("description").SendKeys(Guid.NewGuid().ToString().Replace("-", ""));
            _browser.FindElementById("frm").Submit();

            _browser.Close();
        }
    }
}