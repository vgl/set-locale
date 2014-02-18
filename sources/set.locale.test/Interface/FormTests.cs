using System;
using System.Drawing.Imaging;

using NUnit.Framework;

using set.locale.test.Shared;

namespace set.locale.test.Interface
{
    [TestFixture]
    public class FormTests : BaseInterfaceTest
    {
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

        private void WaitHack()
        {
            Browser.GetScreenshot().SaveAsFile(string.Format("{0}.png", Guid.NewGuid()), ImageFormat.Png);
        }

        [Test]
        public void should_save_domainobject_and_new_one_after()
        {
            LoginAsUser();

            var url = string.Format("{0}{1}", BASE_URL, ACTION_NEW_DOMAIN_OBJECT);

            GoTo(url);

            Browser.FindElementById("Name").SendKeys("test domain obj with save and new");
            Browser.FindElementById("btnSaveAndNew").Click();

            Assert.IsNotNull(Browser);
            Assert.AreEqual(Browser.Url, url);

            CloseBrowser();
        }

        [Test]
        public void should_save_domainobject_and_redirect_to_list()
        {
            LoginAsUser();

            var url = string.Format("{0}{1}", BASE_URL, ACTION_NEW_DOMAIN_OBJECT);
            var domainObjListUrl = string.Format("{0}{1}", BASE_URL, ACTION_LIST_DOMAIN_OBJECTS);

            GoTo(url);

            Browser.FindElementById("Name").SendKeys("test domain obj");
            Browser.FindElementById("btnSave").Click();

            Assert.IsNotNull(Browser);
            Assert.AreEqual(Browser.Url, domainObjListUrl);

            CloseBrowser();
        }

        [Test]
        public void should_save_new_word()
        {
            LoginAsAdmin();

            var url = string.Format("{0}{1}", BASE_URL, ACTION_NEW_WORD);
            var returnUrl = string.Format("{0}{1}", BASE_URL, ACTION_NEW_WORD);

            GoTo(url);

            Browser.FindElementById("key").SendKeys("test_word");
            Browser.FindElementById("tag").SendKeys("test, tag");
            Browser.FindElementById("description").SendKeys("test desc");
            Browser.FindElementById("btn_save").Click();

            Assert.IsNotNull(Browser);
            // todo: detail id ile yapay test
            //Assert.Contains(returnUrl);

            CloseBrowser();
        }

    }
}