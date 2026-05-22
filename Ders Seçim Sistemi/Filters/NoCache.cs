using System.Web;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Filters
{
    public class NoCache : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.Cache.SetExpires(System.DateTime.UtcNow.AddDays(-1));
            base.OnResultExecuting(filterContext);
        }
    }
}