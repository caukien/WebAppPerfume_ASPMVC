using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;
namespace WebAppPerfume.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        public ActionResult Index()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<PRODUCT> pRODUCTs = db.PRODUCTS.OrderBy(r => Guid.NewGuid()).Take(4).ToList();
            return View(pRODUCTs);
        }
    }
}