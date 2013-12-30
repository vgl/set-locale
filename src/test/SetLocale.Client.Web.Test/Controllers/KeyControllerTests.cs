using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class KeyControllerTests
    {
        [Test]
        public void new_should_return_key_model()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var key = new KeyModel { Key = "my-key" };
            demoService.Setup(x => x.GetAKey()).Returns(key);


            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.New();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as KeyModel;
            Assert.IsTrue(controller.HasGetAttribute("New", new[] { typeof(KeyModel) }), "HttpGet attribute not found on KeyController's New() action method");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }
        [Test]
        public void detail_should_return_key_model()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var key = new KeyModel { Key = "my-key" };
            demoService.Setup(x => x.GetAKey()).Returns(key);

            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.Detail();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as KeyModel;
            Assert.IsTrue(controller.HasGetAttribute("Detail", new[] { typeof(KeyModel) }), "HttpGet attribute not found on KeyController's Detail() action method");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }
        [Test]
        public void my_should_return_key_model_list()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };

            demoService.Setup(x => x.GetMyKeys()).Returns(list);

            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.My();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            Assert.IsTrue(controller.HasGetAttribute("My", new[] { typeof(List<KeyModel>) }), "HttpGet attribute not found on KeyController's My() action method");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }
        [Test]
        public void all_should_return_key_model_list()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };

            demoService.Setup(x => x.GetAllKeys()).Returns(list);

            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.All();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            Assert.IsTrue(controller.HasGetAttribute("All", new[] { typeof(List<KeyModel>) }), "HttpGet attribute not found on KeyController's All() action method");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }

        [Test]
        public void notTranslated_should_return_key_model_list()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };

            demoService.Setup(x => x.GetNotTranslatedKeys()).Returns(list);

            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.NotTranslated();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            Assert.IsTrue(controller.HasGetAttribute("NotTranslated", new[] { typeof(List<KeyModel>) }), "HttpGet attribute not found on KeyController's NotTranslated() action method");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }

        [Test]
        public void edit_should_return_translation_model()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var translated = new TranslationModel { Key = "my-key" };
            demoService.Setup(x => x.GetATranslation()).Returns(translated);


            // Act
            var controller = new KeyController(authService.Object, demoService.Object);
            var view = controller.Edit("id",ConstHelper.tr);

            // Assert
            Assert.NotNull(view);

            var model = view.Model as TranslationModel;
            
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);
            Assert.IsTrue(controller.HasGetAttribute("Edit", new[] { typeof(TranslationModel) }), "HttpGet attribute not found on KeyController's Edit() action method");

        }
 

    }
}

