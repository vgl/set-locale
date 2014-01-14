using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

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
            // Arrange
            const string tag = "set-locale";
            const string actionName = "Detail";

            var tagService = new Mock<ITagService>();
            var list = new List<WordModel> { new WordModel { Key = tag } };
            tagService.Setup(x => x.GetWords(tag)).Returns(() => Task.FromResult(new List<Word> { new Word() }));

            // Act
            var sut = new TagControllerBuilder().WithTagService(tagService.Object)
                                                .Build();
            var view = sut.Detail();
            var model = view.Result.Model as List<WordModel>;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.AreEqual(model.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model);

            tagService.Verify(x => x.GetWords(tag), Times.Once);

            sut.AssertGetAttribute(actionName, new[] { typeof(string) });
            sut.AssertAllowAnonymousAttribute(actionName, new[] { typeof(string) });
        }
    }
}