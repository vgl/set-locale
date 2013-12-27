using NUnit.Framework;
using Rhino.Mocks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void should_create_a_user()
        {
            var userRepo = MockRepository.GenerateMock<IRepository<User>>();
            var entity = new User
                {
                    Email = "test@test.com",
                    PasswordHash = "passwordhash"
                };

            var returnEntity = new User
            {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = "passwordhash"
            };
            userRepo.Expect(x => x.Create(entity)).Return(returnEntity);
            userRepo.Expect(x => x.SaveChanges()).Return(true);

            var userService = new UserService(userRepo);
            var userId = userService.Create(new UserModel
            {
                Email = entity.Email,
                Password = entity.PasswordHash
            });

            Assert.AreEqual(entity.Id, userId);




        }
    }
}
