using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.App_Start;
using WebAppPerfume.Models;

namespace WebAppPerfume.Controllers
{
    public class AccController : Controller
    {
        // GET: Acc
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username, string password)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            //var user = db.KHACHHANGs.Find();
            var user = db.KHACHHANGs.FirstOrDefault(s => s.USERNAME.Equals(username) && s.PASSWORD.Equals(password) && s.ISADMIN == false);
            if (user != null)
            {
                SessionConfig.SetUser(user);
                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                TempData["errLogin"] = "Tài khoản hoặc mật khẩu không đúng";
                return RedirectToAction("Index");
            }
        }
        public ActionResult logout()
        {
            SessionConfig.ClearUser();

            return RedirectToAction("Index", "Acc");
        }
        [HttpPost]
        public ActionResult register(string username, string email, string password)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();

            // Kiểm tra xem username đã tồn tại trong cơ sở dữ liệu chưa
            var existingUser = db.KHACHHANGs.FirstOrDefault(s => s.USERNAME.Equals(username));

            if (existingUser != null)
            {
                TempData["errRegis"] = "Username đã tồn tại. Vui lòng chọn username khác.";
                return RedirectToAction("Index");
            }
            else
            {
                // Tạo một đối tượng KHACHHANG mới và thêm vào cơ sở dữ liệu
                KHACHHANG newUser = new KHACHHANG
                {
                    USERNAME = username,
                    EMAIL = email,
                    PASSWORD = password,
                    ISADMIN = false
                };

                db.KHACHHANGs.Add(newUser);
                db.SaveChanges();
                SessionConfig.SetUser(newUser);

                // Bạn có thể thực hiện thêm các hành động khác ở đây, ví dụ như gửi email xác nhận, v.v.

                // Sau khi đăng kí thành công, chuyển hướng người dùng đến trang chủ hoặc trang đăng nhập
                return RedirectToAction("Index", "TrangChu");
            }
        }

    }
}