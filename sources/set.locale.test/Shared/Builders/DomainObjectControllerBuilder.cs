using System.Web;
using Moq;

using set.locale.Controllers;
using set.locale.Data.Services;

namespace set.locale.test.Shared.Builders
{
    public class DomainObjectControllerBuilder : BaseBuilder
    {
        private IDomainObjectService _domainObjectService;

        public DomainObjectControllerBuilder()
        {
            _domainObjectService = new Mock<IDomainObjectService>().Object;
        }

        internal DomainObjectControllerBuilder WithDomainObjectService(IDomainObjectService domainObjectService)
        {
            _domainObjectService = domainObjectService;
            return this;
        }

        internal DomainObjectController BuildWithMockControllerContext(string id, string name, string email, string role)
        {
            var sut = Build();

            SetCurrentUser(id, name, email, role);

            HttpResponse.Setup(x => x.SetCookie(It.IsAny<HttpCookie>()));

            sut.ControllerContext = ControllerContext.Object;
            return sut;
        }

        internal DomainObjectController Build()
        {
            return new DomainObjectController(_domainObjectService);
        }
    }
}