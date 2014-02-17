using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using set.locale.Data.Services;
using set.locale.Models;
using set.locale.test.Shared;
using set.locale.test.Shared.Builders;

namespace set.locale.test.Behaviour
{
    public class OtherBehaviourTests : BaseBehaviourTest
    {
        [Test]
        public async void any_user_can_add_new_domain_object()
        {
            //arrange 
            var model = new DomainObjectModel { Name = "name" };

            var domainObjectService = new Mock<IDomainObjectService>();
            domainObjectService.Setup(x => x.Create(model.Name, ValidUserModel.Id)).Returns(Task.FromResult(true));

            //act
            var sut = new DomainObjectControllerBuilder().WithDomainObjectService(domainObjectService.Object)
                                                         .BuildWithMockControllerContext(ValidUserModel.Id, ValidUserModel.Name, ValidUserModel.Email, ValidUserModel.RoleName);

            var result = await sut.New(model);

            ////assert
            Assert.IsNotNull(result);

            domainObjectService.Verify(x => x.Create(model.Name, ValidUserModel.Id), Times.Once);

            sut.AssertPostAttribute(ACTION_NEW, new[] { typeof(DomainObjectModel) });
        }

    }
}