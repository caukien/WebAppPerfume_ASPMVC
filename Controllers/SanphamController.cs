using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;
namespace WebAppPerfume.Controllers
{
    public class SanphamController : Controller
    {
        // GET: Sanpham
        public ActionResult Index(string Brand = "", int page = 1)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            //List<PRODUCT> pRODUCTs = db.PRODUCTS.ToList();
            List<PRODUCT> pRODUCTs = db.PRODUCTS.Where(r => r.CATEGORy.NAME.Contains(Brand)).ToList();
            ViewBag.cate = db.CATEGORIES.ToList();
            ViewBag.Brand = Brand;
            //Phân trang
            int noofrecordperpage = 12;
            int noofpage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(pRODUCTs.Count) / Convert.ToDouble(noofrecordperpage)));
            int skip = (page - 1) * noofrecordperpage;
            ViewBag.page = page;
            ViewBag.noofpage = noofpage;
            pRODUCTs = pRODUCTs.Skip(skip).Take(noofrecordperpage).ToList();
            return View(pRODUCTs);
        }
        public ActionResult Chitiet(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            //ViewBag.rdpro = db.PRODUCTS.OrderBy(r => Guid.NewGuid()).Take(8).ToList();
            List<PRODUCT> pRODUCT1 = db.PRODUCTS.OrderBy(r => Guid.NewGuid()).Take(8).ToList();
            //C1: tạo class dùng chung or Tạo internal class=>Truy vấn bằng namespace
            //C2: tạo viewbag
            var viewModel = new Models.Class1
            {
                Product = pRODUCT,
                RelatedProducts = pRODUCT1
            };
            return View(viewModel);
        }
    }

    //internal class ProductDetailViewModel
    //{
    //    public PRODUCT Product { get; set; }
    //    public List<PRODUCT> RelatedProducts { get; set; }
    //}
}