using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;

using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using SetLocale.Client.Web.Configurations;
using SetLocale.Client.Web.Controllers;
using SetLocale.Util;

namespace SetLocale.Client.Web.Test
{
    [TestFixture]
    public class RoutesTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void HomeControllerRoutes()
        {
            "~/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index());
            "~/home".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index());
            "~/home/index".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index());
        }

        [Test]
        public void AdminControllerRoutes()
        {
            "~/admin/newtranslator".ShouldMapTo<AdminController>(action => action.NewTranslator());

            "~/admin/users".ShouldMapTo<AdminController>(action => action.Users());
            "~/admin/apps".ShouldMapTo<AdminController>(action => action.Apps());


        }

        [Test]
        public void KeyControllerRoutes()
        {
            "~/key/all".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.All());
            "~/key/my".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.My());
            "~/key/nottranslated".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.NotTranslated());

            "~/key/new".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.New());

            var keyEditRoute = "~/key/edit/sign_up".WithMethod(HttpVerbs.Get);
            keyEditRoute.Values["lang"] = ConstHelper.tr;
            keyEditRoute.ShouldMapTo<KeyController>(action => action.Edit("sign_up", ConstHelper.tr));

            "~/key/detail/sign_up".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.Detail());
        }



    }
}

