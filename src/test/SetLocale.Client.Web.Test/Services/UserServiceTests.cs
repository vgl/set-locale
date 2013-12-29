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
            // Arrange
            var userModel = new UserModel { Email = "test@test.com", Password = "password" };

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Create(It.IsAny<User>())).Returns(It.IsAny<User>());
            userRepository.Setup(x => x.SaveChanges()).Returns(true);

            // Act
            var userService = new UserService(userRepository.Object);
            var userId = userService.Create(userModel);

            // Assert
            userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
            userRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce);

            Assert.NotNull(userId);
        }
    }
}
