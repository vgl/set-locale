using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

using Moq;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class UserControllerBuilder
    {
        private IAppService _appService;
        private IUserService _userService;
        private IWordService _wordService;
        private IFormsAuthenticationService _formsAuthenticationService;

        public UserControllerBuilder()
        {
            _appService = null;
            _formsAuthenticationService = null;
            _userService = null;
            _wordService = null;
        }

        internal UserControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal UserControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal UserControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;

        }

        internal UserControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }

        internal UserController BuildWithMockControllerContext()
        {
            var sut = Build();

            var controllerContext = new Mock<ControllerContext>();
            var httpContext = new Mock<HttpContextBase>();
            var httpRequest = new Mock<HttpRequestBase>();
            var httpResponse = new Mock<HttpResponseBase>();
            var user = new Mock<IPrincipal>();
            var currentUser = new Mock<IIdentity>();

            controllerContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            httpContext.Setup(x => x.User).Returns(user.Object);
            user.Setup(x => x.Identity).Returns(currentUser.Object);
            currentUser.Setup(x => x.IsAuthenticated).Returns(true);
            currentUser.Setup(x => x.Name).Returns(string.Format("{0}|{1}|{2}|{3}", 1, "name", "test@test.com", 1));    

            httpResponse.Setup(x => x.SetCookie(It.IsAny<HttpCookie>()));

            sut.ControllerContext = controllerContext.Object;
            return sut;
        }
        
        internal UserController Build()
        {
            return new UserController(_userService, _wordService, _formsAuthenticationService, _appService);
        }
    }
}