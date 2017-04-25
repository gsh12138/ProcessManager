using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProcessManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "IndexWithId",
                url: "Form/{controller}/{id}",
                defaults:new {action="Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "two",
                url: "{controller}/{action}/{bid}/{detail}"

                );


            routes.MapRoute(
                name:"Api",
                url:"api/{controller}/{action}/{pid}/{order}"
                
                );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
