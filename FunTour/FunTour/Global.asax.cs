using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Web.Http;
using FunTourBusinessLayer.Service;

namespace FunTour
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly DataService Service = new DataService();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Service.CargarTablasDeLectura();
        }
    }
}
