using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class TagControllerTests
    {
        [Test]
        public void index_should_return_key_model_list()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();
            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };
            demoService.Setup(x => x.GetMyKeys()).Returns(list);

            // Act
            var controller = new TagController(null, demoService.Object);
            var view = controller.Index();
            

            // Assert
            Assert.NotNull(view);
            var model = view.Model as List<KeyModel>;

            Assert.NotNull(model);

            CollectionAssert.AllItemsAreUnique(model);
            Assert.AreEqual(model.Count, list.Count);

            demoService.Verify(x => x.GetMyKeys(), Times.Once);
            controller.HasGetAttribute("Index", new[] { typeof(string) });
        }
    }
}