using System;
using System.Linq;
using System.Web.Mvc;

using NUnit.Framework;

namespace SetLocale.Client.Web.Test.TestHelpers
{
    public static class ControllerTestHelper
    {
        public static void AssertGetAttribute(this Controller controller, string actionMethodName, Type[] parameterTypes = null)
        {
            var type = controller.GetType();
            var methodInfo = type.GetMethod(actionMethodName, parameterTypes ?? new Type[0]);
            var attributes = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), true);

            Assert.IsTrue(attributes.Any(), "HttpGet attribute not found");
        }

        public static void AssertPostAttribute(this Controller controller, string actionMethodName, Type[] parameterTypes = null)
        {
            var type = controller.GetType();
            var methodInfo = type.GetMethod(actionMethodName, parameterTypes ?? new Type[0]);
            var postAttributes = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), true);
            var validateTokenAttributes = methodInfo.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true);

            Assert.IsTrue(postAttributes.Any(), "HttpGet attribute not found");
            Assert.IsTrue(validateTokenAttributes.Any(), "ValidateAntiForgeryToken attribute not found");
        }
    }
}