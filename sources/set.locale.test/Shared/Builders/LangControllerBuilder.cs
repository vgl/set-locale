using set.locale.Controllers;

namespace set.locale.test.Shared.Builders
{
    public class LangControllerBuilder : BaseBuilder
    {
        internal LangController Build()
        {
            return new LangController();
        }
    }
}