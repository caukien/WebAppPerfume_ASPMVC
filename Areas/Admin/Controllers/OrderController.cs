using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;

namespace WebAppPerfume.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<ORDER> oRDERs = db.ORDERs.ToList();
            ViewBag.Title = "Đơn hàng";
            return View(oRDERs);
        }

        public ActionResult Detail(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<OrderDetail> orderDetail = db.OrderDetails.Where(i=> i.orderID== id).ToList();
            ViewBag.Title = "Đơn hàng";
            return View(orderDetail);
        }
        [HttpPost]
        public ActionResult deleteOrder(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            OrderDetail detail = db.OrderDetails.FirstOrDefault(i => i.orderID == id);
            db.OrderDetails.Remove(detail);
            db.SaveChanges();
            ORDER oRDER = db.ORDERs.FirstOrDefault(o => o.ORDERID == id);
            db.ORDERs.Remove(oRDER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}