using System;
using System.Linq;
using System.Web.Mvc;

namespace SetLocale.Client.Web.Test.TestHelpers
{
    public static class ControllerTestHelper
    {
        public static bool HasGetAttribute(this Controller controller, string actionMethodName, Type[] parameterTypes = null)
        {
            var type = controller.GetType();
            var methodInfo = type.GetMethod(actionMethodName, parameterTypes ?? new Type[0]);
            var attributes = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), true);
            return attributes.Any();
        }

        public static bool HasPostAttribute(this Controller controller, string actionMethodName, Type[] parameterTypes = null)
        {
            var type = controller.GetType();
            var methodInfo = type.GetMethod(actionMethodName, parameterTypes ?? new Type[0]);
            var postAttributes = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), true);
            var validateTokenAttributes = methodInfo.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true);

            return postAttributes.Any()
                   && validateTokenAttributes.Any();
        }
    }
}