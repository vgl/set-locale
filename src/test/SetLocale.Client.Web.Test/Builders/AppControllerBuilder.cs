using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
namespace SetLocale.Client.Web.Test.Builders
{
    public class AppControllerBuilder
    {
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;
        private IAppService _appService;
        public AppControllerBuilder()
        {
            _formAuthenticationService = null;
            _userService = null;
            _appService = null;   
        }


        internal AppControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formAuthenticationService)
        {
            _formAuthenticationService = formAuthenticationService;
            return this;
        }

        internal AppControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal AppControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;

        }

        internal AppController Build()
        {
            return new AppController(_userService, _formAuthenticationService, _appService);
        }
    }
}