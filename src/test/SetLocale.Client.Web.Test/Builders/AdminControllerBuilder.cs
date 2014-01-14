using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class AdminControllerBuilder
    {
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;
        private IAppService _appService;
    
        public AdminControllerBuilder()
        {
            _formAuthenticationService = null;
            _userService = null;
            _appService = null;
        }

        internal AdminControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formAuthenticationService)
        {
            _formAuthenticationService = formAuthenticationService;
            return this;
        }

        internal AdminControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal AdminControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;
        }

        internal AdminController Build()
        {
            return new AdminController(_userService, _formAuthenticationService, _appService);
        }
    }
}
