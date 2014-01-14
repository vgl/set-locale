using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class HomeControllerBuilder
    {
        private IReportService _reportService;
        private IUserService _userService;
        private IFormsAuthenticationService _formsAuthenticationService;

        public HomeControllerBuilder()
        {
            _formsAuthenticationService = null;
            _userService = null;
            _reportService = null;
        }

        internal HomeControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal HomeControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal HomeControllerBuilder WithReportService(IReportService reportService)
        {
            _reportService = reportService;
            return this;
        }

        internal HomeController Build()
        {
            return new HomeController(_reportService, _userService, _formsAuthenticationService);
        }
    }
}