using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class WordControllerBuilder
    {
        private IUserService _userService;
        private IFormsAuthenticationService _formsAuthenticationService;
        private IWordService _wordService;

        public WordControllerBuilder()
        {
            _userService = null;
            _formsAuthenticationService = null;
            _wordService = null;
        }

        internal WordControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal WordControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal WordControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }

        internal WordController Build()
        {
            return new WordController(_userService, _formsAuthenticationService, _wordService);
        }
    }
}