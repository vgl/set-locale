using System.Web.Mvc;
using System.Web.Routing;

using MvcContrib.TestHelper;
using NUnit.Framework;

using SetLocale.Client.Web.Configurations;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Helpers;

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
        public void TagControllerRoutes()
        {
            "~/tag/index/deneme".WithMethod(HttpVerbs.Get).ShouldMapTo<TagController>(action => action.Detail("deneme").Result);
        }

        [Test]
        public void AdminControllerRoutes()
        {
            "~/admin/newtranslator".ShouldMapTo<AdminController>(action => action.NewTranslator());
            "~/admin/index".ShouldMapTo<AdminController>(action => action.Index());
            "~/admin/users".ShouldMapTo<AdminController>(action => action.Users(0).Result);
            "~/admin/users/1".ShouldMapTo<AdminController>(action => action.Users(1).Result);
            "~/admin/apps".ShouldMapTo<AdminController>(action => action.Apps().Result);
        }

        [Test]
        public void KeyControllerRoutes()
        {
            "~/key/all".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.All());
            "~/key/nottranslated".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.NotTranslated());

            "~/key/new".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.New());
           

            var keyEditRoute = "~/key/edit/sign_up".WithMethod(HttpVerbs.Get);
            keyEditRoute.Values["lang"] = ConstHelper.tr;
            keyEditRoute.ShouldMapTo<KeyController>(action => action.Edit("sign_up", ConstHelper.tr));

            "~/key/detail/sign_up".WithMethod(HttpVerbs.Get).ShouldMapTo<KeyController>(action => action.Detail());
        }

        [Test]
        public void AppControllerRoutes()
        {
            "~/app/new".WithMethod(HttpVerbs.Get).ShouldMapTo<AppController>(action => action.New());
            "~/app/detail".WithMethod(HttpVerbs.Get).ShouldMapTo<AppController>(action => action.Detail(1).Result);
        }

        [Test]
        public void UserControllerRoutes()
        {
            "~/user/index".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Index());
            "~/user/apps".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Apps(1).Result);
            "~/user/new".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.New());
            "~/user/reset".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Reset());
            "~/user/login".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Login());
            "~/user/logout".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Logout());
        }


        [Test]
        public void LangControllerRoutes()
        {
            var langChangeRoute = "~/lang/change".WithMethod(HttpVerbs.Get);
            langChangeRoute.Values["id"] = "1";
            langChangeRoute.ShouldMapTo<LangController>(action => action.Change("1"));
        }



    }
}

