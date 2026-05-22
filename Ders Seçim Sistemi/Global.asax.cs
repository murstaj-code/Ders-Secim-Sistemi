using Ders_Seçim_Sistemi.Models.Entities;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ders_Seçim_Sistemi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<AppDbContext>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var logPath = Server.MapPath("~/Logs");

            if (!System.IO.Directory.Exists(logPath))
                System.IO.Directory.CreateDirectory(logPath);

            log4net.GlobalContext.Properties["LogPath"] = logPath;

            XmlConfigurator.Configure();
        }
    }
}