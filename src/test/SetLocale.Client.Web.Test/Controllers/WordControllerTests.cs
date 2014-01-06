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
    public class WordControllerTests
    {
        [Test]
        public void new_should_return_key_model()
        {
            // Arrange 

            // Act
            var controller = new WordController(null, null, null);
            var view = controller.New();
             
            // Assert
            Assert.NotNull(view);
            var model = view.Model as WordModel;

            Assert.NotNull(model);
            controller.AssertGetAttribute("New"); 
        }

        [Test]
        public async void detail_id_is_null_or_empty_should_return_key_model()
        {
            // Arrange
            
            // Act
            var controller = new WordController(null, null, null);
            var view = await controller.Detail("") as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/home/index");
            controller.AssertGetAttribute("Detail",new []{ typeof(string)}); 
              
        }

        [Test]
        public async void detail_id_is_not_null_should_return_key_model()
        {
            // Arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key")).Returns(() => Task.FromResult(new Word()));

            // Act
            var controller = new WordController(null, null, wordService.Object);
            var view = await controller.Detail("key") as ViewResult;

            // Assert
            Assert.NotNull(view);

            var model = view.Model as WordModel;
            Assert.NotNull(model);
            controller.AssertGetAttribute("Detail", new[] { typeof(string) });

            wordService.Verify(x => x.GetByKey("key"), Times.Once);

        }

       
        [Test]
        public async void all_should_return_key_model_list()
        {
            // Arrange
            var wordService = new Mock<IWordService>(); 
            wordService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<Word>()));

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
            wordService.Setup(x => x.GetNotTranslated()).Returns(() => Task.FromResult(new List<Word>()));
             
            // Act
            var controller = new WordController(null, null, wordService.Object);
            var view = await controller.NotTranslated();

            // Assert
            Assert.NotNull(view);

            var model = view.Model as List<WordModel>;
            controller.AssertGetAttribute("NotTranslated");
            Assert.NotNull(model);

            wordService.Verify(x => x.GetNotTranslated(), Times.Once);
        }
    }
}

