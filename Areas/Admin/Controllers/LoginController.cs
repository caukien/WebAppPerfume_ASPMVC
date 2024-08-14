using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.App_Start;
using WebAppPerfume.Models;
namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            //var user = db.KHACHHANGs.Find();
            var user = db.KHACHHANGs.FirstOrDefault(s => s.USERNAME.Equals(username) && s.PASSWORD.Equals(password) && s.ISADMIN == true);
            if (user!=null)
            {
                SessionConfig.SetUser(user);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                TempData["err"] = "Tài khoản hoặc mật khẩu không đúng";
                return RedirectToAction("Index");
            }            
        }
        public ActionResult Logout()
        {
            SessionConfig.ClearUser();

            return RedirectToAction("Index", "Login");
        }
    }
}