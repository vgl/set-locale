using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
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
            var wordService = new Mock<IWordService>();
             
            wordService.Setup(x => x.GetAll());
             
            // Act
            var controller = new WordController(null, null, wordService.Object);
            var view = controller.New();
             
            // Assert
            Assert.NotNull(view);
            var model = view.Model as WordModel;

            Assert.NotNull(model);
            controller.AssertGetAttribute("New"); 
        }

        [Test]
        public async void detail_should_return_key_model()
        {
            // Arrange
            var wordService = new Mock<IWordService>(); 
            wordService.Setup(x => x.GetAll());

            // Act
            var controller = new WordController(null, null, wordService.Object);
            var view = await controller.Detail("menu_words") as ViewResult;

            // Assert
            Assert.NotNull(view);

            var model = view.Model as WordModel;
            controller.AssertGetAttribute("Detail");
            Assert.NotNull(model);

            wordService.Verify(x => x.GetByKey("test"), Times.Once);

        }

       
        [Test]
        public async void all_should_return_key_model_list()
        {
            // Arrange
            var wordService = new Mock<IWordService>(); 
            wordService.Setup(x => x.GetAll());

            // Act
            var controller = new WordController(null, null, wordService.Object);
            var view = await controller.All();
            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<WordModel>;
            Assert.NotNull(model);
            controller.AssertGetAttribute("All");
            wordService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async void notTranslated_should_return_key_model_list()
        {
            // Arrange
            var wordService = new Mock<IWordService>();

            var list = new List<WordModel> { new WordModel { Key = "my-key" } };

            wordService.Setup(x => x.GetNotTranslated());

            // Act
            var controller = new WordController(null, null, null);
            var view = await controller.NotTranslated() as ViewResult;

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<WordModel>;
            controller.AssertGetAttribute("NotTranslated");
            Assert.NotNull(model);

            wordService.Verify(x => x.GetNotTranslated(), Times.Once);
        }
    }
}

