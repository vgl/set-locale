using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;

using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Test.Selenium
{
    [TestFixture]
    public class NewTranslationTests : BaseUITest
    {
        [Test]
        public void should_add_new_translation()
        {
            var wordDetailUrl = "";
            var translatedLangCount = 0;

            LogOut();
            LoginAsAdmin();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_WORD_ALL));

            var tbodyWords = _browser.FindElement(By.Id("tbodyModelItems"));
            var rows = tbodyWords.FindElements(By.TagName("tr"));

            foreach (var row in rows)
            {
                translatedLangCount = row.FindElements(By.TagName("img")).Count();

                if (translatedLangCount < ConstHelper.MaxLanguageCount)
                {
                    var aHref = row.FindElement(By.TagName("a"));
                    wordDetailUrl = aHref.GetAttribute("href");
                    break;
                }
            }
             
            GoTo(string.Format("{0}", wordDetailUrl));
              
            _browser.FindElement(By.Id("btnNewTranslate")).Click();
            _browser.FindElement(By.Id("txtTranslation")).SendKeys("UnitTest");
            _browser.FindElement(By.Id("btnSave")).Click();

            var newtranslatedLangCount = _browser.FindElement(By.Id("tbodyLanguage")).FindElements(By.TagName("tr")).Count();

            Assert.AreEqual(translatedLangCount + 1, newtranslatedLangCount); 

            _browser.Close();
        }

        [Test]
        public void should_add_new_tag()
        {
            var wordDetailUrl = ""; 

            LogOut();
            LoginAsAdmin();

            GoTo(string.Format("{0}{1}", BASE_URL, ACTION_WORD_ALL));

            var tbodyWords = _browser.FindElement(By.Id("tbodyModelItems"));
            var rows = tbodyWords.FindElements(By.TagName("tr"));

            foreach (var row in rows)
            { 
                    var aHref = row.FindElement(By.TagName("a"));
                    wordDetailUrl = aHref.GetAttribute("href");
                    break; 
            }

            GoTo(string.Format("{0}", wordDetailUrl));

            var tagCount = _browser.FindElement(By.Id("dvTag")).FindElements(By.TagName("a")).Count();
             
            _browser.FindElement(By.Id("btnNewTag")).Click();
            _browser.FindElement(By.Id("txtTag")).SendKeys("UnitTest");
            _browser.FindElement(By.Id("btnSave")).Click();

            var newtagCount = _browser.FindElement(By.Id("dvTag")).FindElements(By.TagName("a")).Count();

            Assert.AreEqual(tagCount + 1, newtagCount);

            _browser.Close();
        }
    }
}