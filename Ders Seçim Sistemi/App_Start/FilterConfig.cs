using System.Web;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Ders_Seçim_Sistemi.Filters.NoCache());
        }

    }
}
