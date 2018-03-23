using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
