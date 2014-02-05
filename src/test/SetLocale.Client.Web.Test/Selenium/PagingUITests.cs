using NUnit.Framework;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class PagingUITests : BaseUITest
    {
        [Test]
        public void word_all_should_open_next_page_if_exist()
        {
            LogOut();
            LoginAsAdmin();

            NextPageButtonTest(string.Format("{0}{1}", BASE_URL, ACTION_WORD_ALL));

            _browser.Close();
        }

        [Test]
        public void word_all_should_open_previous_page_if_exist()
        {
            LogOut();
            LoginAsAdmin();

            PrevPageButtonTest(string.Format("{0}{1}", BASE_URL, ACTION_WORD_ALL));

            _browser.Close(); 
        }
    }
}