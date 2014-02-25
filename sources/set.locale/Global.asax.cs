using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;
using Castle.Windsor.Installer;

using set.locale.Configurations;
using set.locale.Controllers;
using set.locale.Data.Services;
using set.locale.Helpers;

namespace set.locale
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            PrepareIocContainer();

            PrepareLocalizationStrings();
        }

        private static void PrepareIocContainer()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
        }

        private async void PrepareLocalizationStrings()
        { 
            var enTexts = new Dictionary<string, string>();
            var trTexts = new Dictionary<string, string>();

            var wordService = new WordService(new AppService());
            var translations = await wordService.GetByAppName("set-locale");
            foreach (var item in translations)
            {
                enTexts.Add(item.Key, item.Translation_EN);
                trTexts.Add(item.Key, item.Translation_TR);
            }

            Application.Add(ConstHelper.CultureNameTR, trTexts);
            Application.Add(ConstHelper.CultureNameEN, enTexts);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");

            HttpContext.Current.Response.Headers.Set("Server", string.Format("Web Server ({0}) ", Environment.MachineName));
        }
    }
}