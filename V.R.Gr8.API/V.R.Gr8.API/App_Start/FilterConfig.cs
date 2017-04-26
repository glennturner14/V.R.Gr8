using System.Web;
using System.Web.Mvc;

namespace V.R.Gr8.API {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
