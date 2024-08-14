using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.App_Start;
namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra Session ở đây
            var user = SessionConfig.GetUser();
            if (user == null || user.ISADMIN == false)
            {
                // Redirect hoặc thực hiện các hành động khác khi không có Session
                filterContext.Result = new RedirectResult("~/Admin/Login/Index");
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            Controller = "Login",
                            action = "Index",
                            area = "Admin"
                        }
                        )
                    );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}