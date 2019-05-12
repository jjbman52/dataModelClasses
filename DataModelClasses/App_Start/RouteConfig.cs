using System.Web.Mvc;
using System.Web.Routing;

namespace DataModelClasses
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProductsByCategory",
                url: "categories/{id}/products",
                defaults: new { controller = "Product", action = "ProductByCategory" }
            );
            routes.MapRoute(
                name: "SearchProduct",
                url: "{controller}/{action}/{order}",
                defaults: new { controller = "Product", action = "ProductSearch", order = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SpecificProduct",
                url: "product/{id}",
                defaults: new { controller = "Product", action = "Index" }
            );
        }
    }
}
