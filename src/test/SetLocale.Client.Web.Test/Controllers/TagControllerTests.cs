using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
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
            var tagService = new Mock<ITagService>();
            var list = new List<WordModel> { new WordModel { Key = "set-locale" } };
            tagService.Setup(x => x.GetWords("set-locale")).Returns(() => Task.FromResult(new List<Word>{new Word()}));

            // Act
            var controller = new TagController(tagService.Object, null, null);
            var view = controller.Detail("set-locale");
             
            // Assert
            Assert.NotNull(view);
            var model = view.Result.Model as List<WordModel>;

            Assert.NotNull(model);

            CollectionAssert.AllItemsAreUnique(model);
            Assert.AreEqual(model.Count, list.Count);       

            tagService.Verify(x => x.GetWords("set-locale"), Times.Once);
            controller.AssertGetAttribute("Detail", new[] { typeof(string) });
        }
    }
}