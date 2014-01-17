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
    public class LocaleControllerTest
    {
        [Test]
        public void get_should_return_throw_exception_if_key_is_not_exist()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key")).Returns(() => null);   

            //act
            var sut = new LocaleControllerBuilder().WithWordService(wordService.Object)
                                                   .Build();

            var task = sut.Get("invalidLang", "***invalidKey***");
              
            //assert
            Assert.IsNotNull(task);
            Assert.IsAssignableFrom<HttpResponseException>(task.Exception.InnerException);  
        }
        
        [Test]
        public void get_should_return_http_result_ok_if_lang_is_valid()
        {
            //arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByKey("key")).Returns(Task.FromResult(new Word()));

            //act
            var sut = new LocaleControllerBuilder().WithWordService(wordService.Object)
                                                   .Build();
            var task = sut.Get("en", "key");
            var result = task.Result;
             
            //assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IHttpActionResult>(result);
            Assert.IsAssignableFrom<OkNegotiatedContentResult<LocaleModel>>(result); 
            Assert.IsAssignableFrom<LocaleController>(sut);
            Assert.IsInstanceOf<BaseApiController>(sut);

        }
    }
}