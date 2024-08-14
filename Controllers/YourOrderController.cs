using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;
using WebAppPerfume.App_Start;

namespace WebAppPerfume.Controllers
{
    public class YourOrderController : Controller
    {
        // GET: YourOrder
        public ActionResult Index()
        {
            var user = SessionConfig.GetUser();
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<ORDER> os = db.ORDERs.Where(o => o.USERID == user.USERID).ToList();
            return View(os);
        }
        public ActionResult Detail(int id)
        {
            var user = SessionConfig.GetUser();
            NUOCHOAEntities db = new NUOCHOAEntities();
            List<OrderDetail> orders = db.OrderDetails.Where(o => o.orderID == id).ToList();
            //decimal totalAmount = 0;
            //foreach (var order in orders)
            //{
            //    (decimal)items = order.qty * order.proPrice;
            //    totalAmount += order.TotalPrice;
            //}

            //// Truyền danh sách sản phẩm và tổng tiền vào View
            //ViewBag.TotalAmount = totalAmount;
            return View(orders);
        }
    }
}