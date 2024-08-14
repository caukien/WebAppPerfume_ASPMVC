using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;

namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<KHACHHANG> kHACHHANGs = db.KHACHHANGs.ToList();
            return View(kHACHHANGs);
        }
    }
}