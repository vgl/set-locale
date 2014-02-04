using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
    public class ApiControllerTests
    {
        const string ActionNameAddKey = "AddKey";
        const string ActionNameAddKeys = "AddKeys";
        const string ActionNameLocale = "Locale";
        const string ActionNameLocales = "Locales";
        
        [Test]
        public async void add_key_should_create_new_word_and_return_bool()
        {
            //arrange           
            var validModel = new WordModel
            {
                Key = "key", Tag = "tag" , Description = "desc"
            };
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.Create(validModel)).Returns(Task.FromResult(validModel.Key));

            //act
            var sut = new ApiControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var result = await sut.AddKey(validModel.Key,validModel.Tag,validModel.Description);

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.IsAssignableFrom(typeof(bool), result.Data);

            sut.AssertPostAttribute(ActionNameAddKey, new[] { typeof(string), typeof(string), typeof(string) }); 
            
            wordService.Verify(x => x.Create(It.IsAny<WordModel>()), Times.Once); 
        }

        [Test]
        public async void add_keys_should_create_new_word_and_return_bool()
        {
            //arrange           
            var validModel = new WordModel
            {
                Key = "key",
                Tag = "tag",
                Description = "desc"
            };
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.Create(validModel)).Returns(Task.FromResult(validModel.Key));

            //act
            var sut = new ApiControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var result = await sut.AddKeys(validModel.Key, validModel.Tag);

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.IsAssignableFrom(typeof(bool), result.Data);

            sut.AssertPostAttribute(ActionNameAddKeys, new[] { typeof(string), typeof(string) }); 

            wordService.Verify(x => x.Create(It.IsAny<WordModel>()), Times.AtLeastOnce); 
        }

        [Test]
        public async void locale_should_return_locale_model()
        {
            //arrange            
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key"))
                       .Returns(Task.FromResult(new Word()));

            //act
            var sut = new ApiControllerBuilder().WithWordService(wordService.Object)
                                                .Build();

            var result = await sut.Locale("tr", "key");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.IsAssignableFrom(typeof(LocaleModel), result.Data);

            sut.AssertGetAttribute(ActionNameLocale, new[] { typeof(string), typeof(string) });

            wordService.Verify(x => x.GetByKey("key"), Times.Once);
        } 

        [Test]
        public async void locales_should_return_name_value_model()
        {
            //arrange           
            var validModel = new WordModel
            {
                Key = "key",
                Tag = "tag",
                Languages = new List<LanguageModel>
                {
                    new LanguageModel
                    {
                        Key="tr"
                    }
                },
                Description = "desc"
            };

            var list = new List<Word> { new Word { Id = 1 }, new Word { Id = 2 } }; 

            var tagService = new Mock<ITagService>();
            tagService.Setup(x => x.GetWords(validModel.Tag, 1))
                      .Returns(Task.FromResult(new PagedList<Word>(1, 2, 3, list)));
             
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetWords(1))
                       .Returns(Task.FromResult(new PagedList<Word>(1, 2, 3, list)));
              
            //act
            var sut = new ApiControllerBuilder().WithWordService(wordService.Object)
                                                .WithTagService(tagService.Object)
                                                .Build();

            var result = await sut.Locales(validModel.Tag, "tr",1);

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.IsAssignableFrom(typeof(List<NameValueModel>), result.Data);

            sut.AssertGetAttribute(ActionNameLocales, new[] { typeof(string), typeof(string) ,typeof(int) });

            wordService.Verify(x => x.GetWords(1), Times.AtMostOnce);
            tagService.Verify(x => x.GetWords(validModel.Tag,1), Times.AtMostOnce); 
        } 
    }
}