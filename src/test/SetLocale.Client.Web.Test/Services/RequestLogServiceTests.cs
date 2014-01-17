using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class RequestLogServiceTests
    {
        [Test]
        public async void should_Log_check_inputs_for_IsNullOrEmpty()
        {
            //arrange
            var token = string.Empty;
            var url = string.Empty;

            //act
            var sut = new RequestLogServiceBuilder().Build();
            var result = await sut.Log(token, "ip", url);

            //assert
            Assert.AreEqual(false, result);
        }
        
        [Test]
        public async void should_Log_return_false_if_token_not_found()
        {
            //arrange
            var tokenRepository = new Mock<IRepository<Token>>();
            tokenRepository.Setup(x=>x.FindOne(It.IsAny<Expression<Func<Token, bool>>>())).Returns((Token)null);
            
            //act
            var sut = new RequestLogServiceBuilder().WithTokenRepository(tokenRepository.Object).Build();
            var result = await sut.Log("token", "ip", "url");

            //assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async void should_Log_calls_create_and_update()
        {
            //arrange
            var tokenRepository = new Mock<IRepository<Token>>();
            tokenRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Token, bool>>>())).Returns(new Token());

            var requestLogRepository = new Mock<IRepository<RequestLog>>();
            requestLogRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var sut = new RequestLogServiceBuilder().WithTokenRepository(tokenRepository.Object)
                                                    .WithRequestLogRepository(requestLogRepository.Object)
                                                    .Build();
            var result = await sut.Log("token", "ip", "url");

            //assert
            Assert.AreEqual(true, result);
            
            requestLogRepository.Verify(x=>x.Create(It.IsAny<RequestLog>()), Times.Once);
            tokenRepository.Verify(x => x.Update(It.IsAny<Token>()), Times.Once);

            requestLogRepository.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
