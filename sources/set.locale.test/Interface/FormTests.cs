using System;
using System.Drawing.Imaging;

using NUnit.Framework;

using set.locale.test.Shared;

namespace set.locale.test.Interface
{
    [TestFixture]
    public class FormTests : BaseInterfaceTest
    {
        private void WaitHack()
        {
            Browser.GetScreenshot().SaveAsFile(string.Format("{0}.png", Guid.NewGuid()), ImageFormat.Png);
        }

        [Test]
        public void should_save_new_feedback_via_popup_form()
        {
            var homeUrl = string.Format("{0}{1}", BASE_URL, ACTION_HOME);

            GoTo(homeUrl);

            Browser.FindElementById("btnOpenFeedBack").Click();

            WaitHack();

            Browser.FindElementById("FeedbackMessage").SendKeys("test feedback");
            Browser.FindElementById("btnSaveFeedback").Click();

            CloseBrowser();
        }

        [Test]
        public void should_save_contact_form()
        {
            var url = string.Format("{0}{1}", BASE_URL, ACTION_CONTACT);

            GoTo(url);

            Browser.FindElementById("Email").SendKeys("john@doe.com");
            Browser.FindElementById("Subject").SendKeys("Message from John Doe");
            Browser.FindElementById("Message").SendKeys("Hi, this is test message.");
            Browser.FindElementByClassName("btn-success").Click();

            Assert.IsNotNull(Browser);
            Assert.AreEqual(Browser.PageSource.Contains("alert-success"), true);

            CloseBrowser();
        }

        [Test]
        public void should_save_new_user()
        {
            var url = string.Format("{0}{1}", BASE_URL, ACTION_SIGNUP);
            var returnUrl = string.Format("{0}{1}", BASE_URL, ACTION_NEW_APP);

            GoTo(url);

            Browser.FindElementById("Name").SendKeys("John Doe");
            Browser.FindElementById("Email").SendKeys("john@doe.com");
            Browser.FindElementById("Password").SendKeys("123456");
            Browser.FindElementByClassName("btn-success").Click();

            Assert.IsNotNull(Browser);
            Assert.AreEqual(Browser.Url, returnUrl);

            CloseBrowser();
        }

        [Test]
        public void should_save_new_app()
        {
            LoginAsUser();

            var url = string.Format("{0}{1}", BASE_URL, ACTION_NEW_APP);

            GoTo(url);

            Browser.FindElementById("name").SendKeys("New App");
            Browser.FindElementById("url").SendKeys("testapp.com");
            Browser.FindElementById("description").SendKeys("test app");
            Browser.FindElementByClassName("btn-success").Click();

            Assert.IsNotNull(Browser);
            Assert.AreNotEqual(Browser.Url, url);

            CloseBrowser();
        }

        [Test]
        public void should_save_new_word()
        {
            LoginAsAdmin();

            var url = string.Format("{0}{1}", BASE_URL, ACTION_NEW_WORD);

            GoTo(url);

            Browser.FindElementById("key").SendKeys("test_word");
            Browser.FindElementById("tag").SendKeys("test, tag");
            Browser.FindElementById("description").SendKeys("test desc");
            Browser.FindElementById("btn_save").Click();

            Assert.IsNotNull(Browser);
            Assert.AreNotEqual(Browser.Url, url);

            CloseBrowser();
        }

    }
}