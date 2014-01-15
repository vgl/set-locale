using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public class TagControllerTests
    {
        [Test]
        public void detail_should_return_key_model_list()
        {
            //arrange
            const string tag = "set-locale";
            const string actionName = "Detail";

            var tagService = new Mock<ITagService>();
            var list = new List<Word> { new Word { Id = 1, Key = tag }, new Word { Id = 2, Key = tag } };
            tagService.Setup(x => x.GetWords(tag, 1)).Returns(() => Task.FromResult(new PagedList<Word>(1, 2, 3, list)));

            //act
            var sut = new TagControllerBuilder().WithTagService(tagService.Object)
                                                .Build();
            var view = sut.Detail();
            var model = view.Result.Model as PageModel<WordModel>;

           //assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsInstanceOf<BaseController>(sut);
            Assert.IsAssignableFrom<PageModel<WordModel>>(model);
            Assert.AreEqual(model.Items.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model.Items);

            tagService.Verify(x => x.GetWords(tag, 1), Times.Once);

            sut.AssertGetAttribute(actionName, new[] { typeof(string), typeof(int) });
            sut.AssertAllowAnonymousAttribute(actionName, new[] { typeof(string), typeof(int) });
        }
    }
}