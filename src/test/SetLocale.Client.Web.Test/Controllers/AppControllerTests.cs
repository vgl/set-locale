using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Test.TestHelpers;


namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AppControllerTests
    {
        [Test]
        public void detail_should_return_app_model()
        {
            // Arrange           
            var demoService = new Mock<IDemoDataService>();

            // Act
            var controller = new AppController(null,null, demoService.Object);
            var view = controller.Detail(1);

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Index");
            demoService.Verify(x => x.GetAnApp(), Times.Once);
        }

        [Test]
        public void new_should_return_app_model()
        {
            // Arrange           
            var demoService = new Mock<IDemoDataService>();

            // Act
            var controller = new AppController(null,null, demoService.Object);
            var view = controller.New();

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("New");
            demoService.Verify(x => x.GetAnApp(), Times.Once);
        }

        [Test]
        public void new_should_redirect_if_model_is_valid()
        {
            // Arrange
            var validModel = new AppModel { Name = "test name", Url = "test.com", Description = "test description" };
        
            // Act
            var controller = new AppController(null,null, null);
            var view = controller.New(validModel).Result as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            controller.AssertPostAttribute("New", new[] {typeof (AppModel)});
        }

        [Test]
        public void new_should_return_app_model_if_model_is_invalid()
        {
            // Arrange
            var inValidModel = new AppModel { Name = "test name", Url = "test.com" };

            // Act
            var controller = new AppController(null,null, null);
            var view = controller.New(inValidModel).Result as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            var model = view.Model as AppModel;

            Assert.NotNull(model);

            controller.AssertPostAttribute("New", new[] { typeof(AppModel) });
        }
    }
}
