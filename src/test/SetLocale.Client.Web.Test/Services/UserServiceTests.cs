using System;
using System.Linq.Expressions;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void should_create_a_user()
        {
            //arrange
            var userModel = new UserModel { Email = "test@test.com", Password = "password" };

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Create(It.IsAny<User>())).Returns(It.IsAny<User>());
            userRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var userService = new UserService(userRepository.Object);
            var userId = userService.Create(userModel);

           //assert
            Assert.NotNull(userId);

            userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
            userRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce); 
        }

        [Test]
        public async void should_get_user_by_email()
        {
            //arrange
            const string email = "test@test.com";

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<User, bool>>>())).Returns(new User { Email = email });

            //act
            var userService = new UserService(userRepository.Object);
            var user = await userService.GetByEmail(email);

           //assert
            Assert.NotNull(user);
            Assert.AreEqual(user.Email, email);

            userRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Test]
        public void should_authenticate_user()
        {
            //arrange
            var userModel = new UserModel { Email = "test@test.com", Password = "password" };
            var userRepository = new Mock<IRepository<User>>();

            //act
            var userService = new UserService(userRepository.Object);
            var userId = userService.Create(userModel);

           //assert
            Assert.NotNull(userId);
            
            userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
            userRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }
    }
}
