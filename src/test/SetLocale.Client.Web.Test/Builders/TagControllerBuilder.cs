using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
namespace SetLocale.Client.Web.Test.Builders
{
    public class TagControllerBuilder
    {
        private IFormsAuthenticationService _formsAuthenticationService;
        private IUserService _userService;
        private ITagService _tagService;


        public TagControllerBuilder()
        {
            _formsAuthenticationService = null;
            _tagService = null;
            _userService = null;
        }


        internal TagControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formsAuthenticationService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            return this;
        }

        internal TagControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal TagControllerBuilder WithTagService(ITagService tagService)
        {
            _tagService = tagService;
            return this;

        }

        internal TagController Build()
        {
            return new TagController(_tagService, _userService, _formsAuthenticationService);
        }
    }
}