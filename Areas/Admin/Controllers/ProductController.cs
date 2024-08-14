using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;
using System.IO;

namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<PRODUCT> pRODUCTs = db.PRODUCTS.ToList();
            ViewBag.Title = "Sản phẩm";
            ViewBag.cate = db.CATEGORIES.ToList();
            return View(pRODUCTs);
        }
        public ActionResult Add()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            ViewBag.cate = db.CATEGORIES.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase pic, PRODUCT prod)
        {
            if (pic != null && pic.ContentLength > 0)
            {
                // Kiểm tra phần mở rộng của file
                string fileExtension = System.IO.Path.GetExtension(pic.FileName)?.ToLower();
                if (fileExtension != null && (fileExtension == ".jpg" || fileExtension == ".webp"))
                {
                    // Kiểm tra dung lượng của file
                    if (pic.ContentLength <= 5 * 1024 * 1024) // 5MB (trong byte)
                    {
                        string rootFolder = Server.MapPath("/Image/");
                        string pathImage = rootFolder + pic.FileName;
                        pic.SaveAs(pathImage);
                        prod.PIC = "/Image/" + pic.FileName;
                    }
                    else
                    {
                        ViewBag.Message = "File không quá 5MB";
                        return RedirectToAction("Add");
                    }
                }
                else
                {
                    ViewBag.Message = "Chỉ cho phép JPG or WEBP";
                    return RedirectToAction("Add");
                }
            }
            else
            {
                ViewBag.Message = "Bạn chưa chọn file";
                return RedirectToAction("Add");
            }
            NUOCHOAEntities db = new NUOCHOAEntities();
            db.PRODUCTS.Add(prod);
            db.SaveChanges();
            TempData["msg"] = "Thêm thành công";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            ViewBag.cate = db.CATEGORIES.ToList();
            ViewBag.Title = "Chỉnh sửa";
            return View(pRODUCT);
        }
        [HttpPost]
        public ActionResult Edit(PRODUCT prod, String old_pic, HttpPostedFileBase new_pic)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            PRODUCT pRODUCT = db.PRODUCTS.Find(prod.PRODID);
            // Kiểm tra xem có ảnh mới được chọn không
            if (new_pic != null && new_pic.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(new_pic.FileName)?.ToLower();

                // Kiểm tra định dạng và dung lượng của file
                if ((fileExtension == ".jpg" || fileExtension == ".webp") && new_pic.ContentLength <= 5 * 1024 * 1024)
                {
                    // Xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(old_pic))
                    {
                        string oldImagePath = Server.MapPath(old_pic);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu ảnh mới
                    string rootFolder = Server.MapPath("/Image/");
                    string newImagePath = rootFolder + new_pic.FileName;
                    new_pic.SaveAs(newImagePath);
                    prod.PIC = "/Image/" + new_pic.FileName;
                }
                else
                {
                    ViewBag.Message = "Chỉ cho phép JPG hoặc WEBP và dung lượng không quá 5MB";
                    return RedirectToAction("Add");
                }
            }
            else
            {
                // Nếu không có ảnh mới, giữ nguyên ảnh cũ
                prod.PIC = old_pic;
                pRODUCT.PRODID = prod.PRODID;
                pRODUCT.PRODNAME = prod.PRODNAME;
                pRODUCT.PRICE = prod.PRICE;
                pRODUCT.CAP = prod.CAP;
                pRODUCT.DESCRIPT = prod.DESCRIPT;
                pRODUCT.QTY = prod.QTY;
                pRODUCT.idCATE = prod.idCATE;
                pRODUCT.PIC = prod.PIC;
                //db.SaveChanges();
                //TempData["msg"] = "Sửa thành công";
                //return RedirectToAction("Index");
            }

            // Cập nhật thông tin sản phẩm và lưu vào cơ sở dữ liệu
            //pRODUCT.PRODID = prod.PRODID;
            //pRODUCT.PRODNAME = prod.PRODNAME;
            //pRODUCT.PRICE = prod.PRICE;
            //pRODUCT.CAP = prod.CAP;
            //pRODUCT.DESCRIPT = prod.DESCRIPT;
            ////pRODUCT.PIC = prod.PIC;
            //pRODUCT.QTY = prod.QTY;
            //pRODUCT.idCATE = prod.idCATE;
            // Các thuộc tính khác

            db.SaveChanges();
            TempData["msg"] = "Sửa thành công";
            return RedirectToAction("Index");
            //return RedirectToAction("Add");
            //pRODUCT.PRODID = prod.PRODID;
            //pRODUCT.PRODNAME = prod.PRODNAME;
            //pRODUCT.PRICE = prod.PRICE;
            //pRODUCT.CAP = prod.CAP;
            //pRODUCT.DESCRIPT = prod.DESCRIPT;
            //pRODUCT.PIC = prod.PIC;
            //prod.QTY = prod.QTY;
            //db.SaveChanges();
            //return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            if (pRODUCT != null)
            {
                string imagePath = Server.MapPath(pRODUCT.PIC);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                db.PRODUCTS.Remove(pRODUCT);
                db.SaveChanges();
                TempData["msg"] = "Xóa thành công";
            }

            return RedirectToAction("Index");
        }
        //public ActionResult Delete(int id)
        //{
        //    NUOCHOAEntities db = new NUOCHOAEntities();
        //    PRODUCT pRODUCT = db.PRODUCTS.Where(row => row.PRODID == id).FirstOrDefault();
        //    db.PRODUCTS.Remove(pRODUCT);
        //    db.SaveChanges();
        //    return RedirectToAction("index");
        //}
    }
}