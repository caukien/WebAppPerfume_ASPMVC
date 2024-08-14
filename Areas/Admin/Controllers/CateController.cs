using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;

namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class CateController : BaseController
    {
        // GET: Admin/Cate
        public ActionResult Index()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<CATEGORy> cATEGORies = db.CATEGORIES.ToList();
            ViewBag.Title = "Quản lý danh mục";
            return View(cATEGORies);
        }
        public ActionResult Add()
        {
            ViewBag.Title = "Thêm danh mục";
            return View();
        }
        [HttpPost]
        public ActionResult Add(CATEGORy c)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            db.CATEGORIES.Add(c);
            db.SaveChanges();
            TempData["msg"] = "Thêm thành công";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            CATEGORy cATEGORy = db.CATEGORIES.Where(row => row.CATEID == id).FirstOrDefault();
            ViewBag.Title = "Chỉnh sửa";
            return View(cATEGORy);
        }
        [HttpPost]
        public ActionResult Edit(CATEGORy cate)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            CATEGORy cATEGORy = db.CATEGORIES.Where(row => row.CATEID == cate.CATEID).FirstOrDefault();
            ViewBag.Title = "Chỉnh sửa";
            cATEGORy.CATEID = cate.CATEID;
            cATEGORy.NAME = cate.NAME;
            db.SaveChanges();
            TempData["msg"] = "Sửa thành công";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            CATEGORy cATEGORy = db.CATEGORIES.Where(row => row.CATEID == id).FirstOrDefault();
            db.CATEGORIES.Remove(cATEGORy);
            db.SaveChanges();
            TempData["msg"] = "Xóa thành công";
            return RedirectToAction("Index");
        }
    }
}