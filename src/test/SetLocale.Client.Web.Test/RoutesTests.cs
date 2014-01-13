using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

using MvcContrib.TestHelper;
using NUnit.Framework;

using SetLocale.Client.Web.Configurations;
using SetLocale.Client.Web.Controllers;

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
            "~/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index().Result);
            "~/home".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index().Result);
            "~/home/index".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(action => action.Index().Result);
        }

        [Test]
        public void TagControllerRoutes()
        {
            "~/tag/index/deneme".WithMethod(HttpVerbs.Get).ShouldMapTo<TagController>(action => action.Detail("deneme").Result);
        }

        [Test]
        public void AdminControllerRoutes()
        {
            "~/admin/newtranslator".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(action => action.NewTranslator());
            "~/admin/index".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(action => action.Index());

            "~/admin/users".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(action => action.Users(0).Result);
            "~/admin/users/1".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(action => action.Users(1).Result);
            "~/admin/apps".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(action => action.Apps().Result);
        }

        [Test]
        public void KeyControllerRoutes()
        {
            "~/word/all".WithMethod(HttpVerbs.Get).ShouldMapTo<WordController>(action => action.All(1).Result);
            "~/word/nottranslated".WithMethod(HttpVerbs.Get).ShouldMapTo<WordController>(action => action.NotTranslated().Result);

            "~/word/new".WithMethod(HttpVerbs.Get).ShouldMapTo<WordController>(action => action.New());

            "~/word/detail/sign_up".WithMethod(HttpVerbs.Get).ShouldMapTo<WordController>(action => action.Detail("sign_up").Result);
        }

        [Test]
        public void AppControllerRoutes()
        {
            "~/app/new".WithMethod(HttpVerbs.Get).ShouldMapTo<AppController>(action => action.New());

            var appDetail = "~/app/detail".WithMethod(HttpVerbs.Get);
            appDetail.Values["id"] = 1;
            appDetail.ShouldMapTo<AppController>(action => action.Detail(1).Result);
        }

        [Test]
        public void UserControllerRoutes()
        {
            "~/user/new".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.New());
            "~/user/reset".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Reset());
            "~/user/login".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Login());
            "~/user/logout".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Logout());

            "~/user/apps".WithMethod(HttpVerbs.Get).ShouldMapTo<UserController>(action => action.Apps(1).Result);
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

