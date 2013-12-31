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
            var demoService = new Mock<IDemoDataService>();

            var key = new KeyModel { Key = "my-key" };
            demoService.Setup(x => x.GetAKey()).Returns(key);

            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.New();
            

            // Assert
            Assert.NotNull(view);
            var model = view.Model as KeyModel;

            Assert.NotNull(model);
            controller.AssertGetAttribute("New");
            demoService.Verify(x => x.GetAKey(), Times.Once);
        }

        [Test]
        public void detail_should_return_key_model()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();

            var key = new KeyModel { Key = "my-key" };
            demoService.Setup(x => x.GetAKey()).Returns(key);

            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.Detail();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as KeyModel;
            controller.AssertGetAttribute("Detail");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetAKey(), Times.Once);

        }
        [Test]
        public void my_should_return_key_model_list()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();

            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };

            demoService.Setup(x => x.GetMyKeys()).Returns(list);

            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.My();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            controller.AssertGetAttribute("My");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetMyKeys(), Times.Once);

        }
        [Test]
        public void all_should_return_key_model_list()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();
            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };
            demoService.Setup(x => x.GetAllKeys()).Returns(list);

            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.All();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            Assert.NotNull(model);
            controller.AssertGetAttribute("All");
            demoService.Verify(x => x.GetAllKeys(), Times.Once);
        }

        [Test]
        public void notTranslated_should_return_key_model_list()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();

            var list = new List<KeyModel> { new KeyModel { Key = "my-key" } };

            demoService.Setup(x => x.GetNotTranslatedKeys()).Returns(list);

            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.NotTranslated();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<KeyModel>;
            controller.AssertGetAttribute("NotTranslated");
            Assert.NotNull(model);

            demoService.Verify(x => x.GetNotTranslatedKeys(), Times.Once);
        }

        [Test]
        public void edit_should_return_translation_model()
        {
            // Arrange
            var demoService = new Mock<IDemoDataService>();

            var translated = new TranslationModel { Key = "my-key" };
            demoService.Setup(x => x.GetATranslation()).Returns(translated);


            // Act
            var controller = new KeyController(null, demoService.Object);
            var view = controller.Edit("id",ConstHelper.tr);

            // Assert
            Assert.NotNull(view);

            var model = view.Model as TranslationModel;
            
            Assert.NotNull(model);

            demoService.Verify(x => x.GetATranslation(), Times.Once);
            controller.AssertPostAttribute("Edit", new[] { typeof(TranslationModel) });
        }
    }
}

