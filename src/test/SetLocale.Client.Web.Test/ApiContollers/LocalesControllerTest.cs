using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.ApiContollers
{
    [TestFixture]
    public class LocalesControllerTest
    {  
        [Test]
        public void get_should_return_word_item_model_list_if_lang_is_valid()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetWords(1)).Returns(Task.FromResult(new PagedList<Word>(1,1,1,new List<Word>())));

            //act
            var sut = new LocalesControllerBuilder().WithWordService(wordService.Object)
                                                    .Build();
            var task = sut.Get("tr", 1);
            var result = task.Result;

            //assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IHttpActionResult>(result);
            Assert.IsAssignableFrom<OkNegotiatedContentResult<List<WordItemModel>>>(result);
            Assert.IsAssignableFrom<LocalesController>(sut);
            Assert.IsInstanceOf<BaseApiController>(sut);

        }

        [Test]
        public void get_should_return_word_item_model_list_if_lang_is_invalid()
        {
            //arrange
            
            //act
            var sut = new LocalesControllerBuilder().Build();
            var task = sut.Get("invalidLang", 1);
            var result = task.Result;

            //assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IHttpActionResult>(result);
            Assert.IsAssignableFrom<OkNegotiatedContentResult<List<WordItemModel>>>(result);  
            Assert.IsAssignableFrom<LocalesController>(sut);
            Assert.IsInstanceOf<BaseApiController>(sut);

        }
    }
}