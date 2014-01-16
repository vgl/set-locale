using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.Builders;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class WordControllerTests
    {
        const string ActionNameAll = "All";
        const string ActionNameNew = "New";
        const string ActionNameDetail = "Detail";
        const string ActionNameNotTranslated = "NotTranslated";

        [Test]
        public void new_should_return_key_model()
        {
            //arrange 

            //act
            var sut = new WordControllerBuilder().Build();
            var view = sut.New();

            //assert
            Assert.NotNull(view);
            var model = view.Model as WordModel;

            Assert.NotNull(model);
            sut.AssertGetAttribute(ActionNameNew);
        }

        [Test]
        public async void detail_id_is_null_or_empty_should_return_key_model()
        {
            //arrange

            //act
            var sut = new WordControllerBuilder().Build();
            var view = await sut.Detail("") as RedirectResult;

            //assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/");
            sut.AssertGetAttribute(ActionNameDetail, new[] { typeof(string) });

        }

        [Test]
        public async void detail_id_is_not_null_should_return_key_model()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key")).Returns(() => Task.FromResult(new Word()));

            //act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                 .Build();

            var view = await sut.Detail("key") as ViewResult;
            var model = view.Model as WordModel;

            //assert
            Assert.NotNull(view); 
            Assert.NotNull(model);

            sut.AssertGetAttribute(ActionNameDetail, new[] { typeof(string) });
            wordService.Verify(x => x.GetByKey("key"), Times.Once);
        }

        [Test]
        public void all_words_return_with_paged_list_word()
        {
            //arrange
            const string tag = "set-locale";

            var wordService = new Mock<IWordService>();
            var list = new List<Word> { new Word { Id = 1, Key = tag }, new Word { Id = 2, Key = tag } };
            wordService.Setup(x => x.GetWords(1)).Returns(Task.FromResult(new PagedList<Word>(1, 2, 3, list)));

            //act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                .Build();
            var view = sut.All(1);
            var model = view.Result.Model as PageModel<WordModel>;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsInstanceOf<BaseController>(sut);
            Assert.IsAssignableFrom<PageModel<WordModel>>(model);
            Assert.AreEqual(model.Items.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model.Items);

            wordService.Verify(x => x.GetWords(1), Times.Once);

            sut.AssertGetAttribute(ActionNameAll, new[] { typeof(int) });
            sut.AssertAllowAnonymousAttribute(ActionNameAll, new[] { typeof(int) });
        }

        [Test]
        public async void notTranslated_should_return_key_model_list()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            var list = new List<Word> { new Word { Id = 1}, new Word { Id = 2} };
            wordService.Setup(x => x.GetNotTranslated(1)).Returns(Task.FromResult(new PagedList<Word>(1, 2, 3, list)));

            //act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var view = await sut.NotTranslated(1);
            var model = view.Model as PageModel<WordModel>; ;

            //assert
            Assert.NotNull(view); 
            Assert.NotNull(model);
            Assert.IsInstanceOf<BaseController>(sut);
            Assert.IsAssignableFrom<PageModel<WordModel>>(model);
            Assert.AreEqual(model.Items.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model.Items);

            wordService.Verify(x => x.GetNotTranslated(1), Times.Once);

            sut.AssertGetAttribute(ActionNameNotTranslated, new []{ typeof(int)});
        }

        [Test]
        public async void translate_should_return_with_response_model()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.Translate("key", "EN", "translation")).Returns(Task.FromResult(true));

            //act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var json = await sut.Translate("key", "EN", "translation");
            var model = json.Data as ResponseModel;

            //assert
            Assert.NotNull(model);
            Assert.AreEqual(true, model.Ok);

        }

        [Test]
        public async void tag_should_return_with_response_model()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.Tag("key", "tag")).Returns(Task.FromResult(true));

            //act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var json = await sut.Tag("key", "tag");
            var model = json.Data as ResponseModel;

            //assert
            Assert.NotNull(model);
            Assert.AreEqual(true, model.Ok);
        }
    }
}

