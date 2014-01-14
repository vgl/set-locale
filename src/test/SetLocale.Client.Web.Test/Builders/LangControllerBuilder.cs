using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LangControllerBuilder
    {
        private IUserService _userService;
        private IFormsAuthenticationService _formsAuthenticationService;

        public LangControllerBuilder()
        {
            _formsAuthenticationService = null;
            _userService = null;
        }

        internal LangControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal LangControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal LangController Build()
        {
            return new LangController(_userService, _formsAuthenticationService);
        }
    }
}