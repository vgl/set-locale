using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

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
    public class WordControllerTests
    {
        [Test]
        public void new_should_return_key_model()
        {
            // Arrange 

            // Act
            var sut = new WordControllerBuilder().Build();
            var view = sut.New();
             
            // Assert
            Assert.NotNull(view);
            var model = view.Model as WordModel;

            Assert.NotNull(model);
            sut.AssertGetAttribute("New"); 
        }

        [Test]
        public async void detail_id_is_null_or_empty_should_return_key_model()
        {
            // Arrange
            
            // Act
            var sut = new WordControllerBuilder().Build();
            var view = await sut.Detail("") as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/home/index");
            sut.AssertGetAttribute("Detail",new []{ typeof(string)}); 
              
        }

        [Test]
        public async void detail_id_is_not_null_should_return_key_model()
        {
            // Arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key")).Returns(() => Task.FromResult(new Word()));

            // Act
            var sut = new WordControllerBuilder().WithWordService(wordService.Object)
                                                 .Build();

            var view = await sut.Detail("key") as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model as WordModel;
            Assert.NotNull(model);

            sut.AssertGetAttribute("Detail", new[] { typeof(string) });
            wordService.Verify(x => x.GetByKey("key"), Times.Once);
        }
         
        //[Test]
        //public async void all_should_return_key_model_list()
        //{
        //    // Arrange
        //    var wordService = new Mock<IWordService>(); 
        //    wordService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<Word>()));

        //    // Act
        //    var sut = new WordControllerBuilder().WithWordService(wordService.Object)
        //                                         .Build();
        //    var view = await sut.All();
        //    // Assert
        //    Assert.NotNull(view);

        //    var model = view.Model as List<WordModel>;
        //    Assert.NotNull(model);
        //    sut.AssertGetAttribute("All");
        //    wordService.Verify(x => x.GetAll(), Times.Once);
        //}

        //[Test]
        //public async void notTranslated_should_return_key_model_list()
        //{
        //    // Arrange
        //    var wordService = new Mock<IWordService>();  
        //    wordService.Setup(x => x.GetNotTranslated()).Returns(() => Task.FromResult(new List<Word>()));
             
        //    // Act
        //    var sut = new WordControllerBuilder().WithWordService(wordService.Object)
        //                                        .Build();

        //    var view = await sut.NotTranslated();

        //    // Assert
        //    Assert.NotNull(view);

        //    var model = view.Model as List<WordModel>;
        //    sut.AssertGetAttribute("NotTranslated");
        //    Assert.NotNull(model);

        //    wordService.Verify(x => x.GetNotTranslated(), Times.Once);
        //}

        [Test]
        public async void translate_should_return_with_response_model()
        {
            //todo: Translate(string key, string language, string translation) kontrolü yapılacak.
        }

        [Test]
        public async void tag_should_return_with_response_model()
        {
            //todo: Tag(string key, string tag) kontrolü yapılacak.
        }
    }
}

