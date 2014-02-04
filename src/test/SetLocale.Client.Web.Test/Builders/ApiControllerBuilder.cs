using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Controllers
{
    public class ApiControllerBuilder
    {
        private IFormsAuthenticationService _formsAuthenticationService;
        private IUserService _userService;
        private IWordService _wordService;
        private ITagService _tagService;
        private IAppService _appService;
        private IRequestLogService _requestLogService;

        public ApiControllerBuilder()
        {
            _wordService = null;
            _tagService = null;
            _appService = null;
            _requestLogService = null;
        }

        internal ApiControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal ApiControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal ApiControllerBuilder WithFormsAppService(IAppService appService)
        {
            _appService = appService;
            return this;
        }

        internal ApiControllerBuilder WithFormsRequestLogService(IRequestLogService requestLogService)
        {
            _requestLogService = requestLogService;
            return this;
        }

        internal ApiControllerBuilder WithTagService(ITagService tagService)
        {
            _tagService = tagService;
            return this;
        }

        internal ApiControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }

        internal ApiController Build()
        {
            return new ApiController(_userService,_wordService,_tagService,_appService,_requestLogService,_formsAuthenticationService);
        }
    }
}