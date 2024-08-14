using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPerfume.Models;
using WebAppPerfume.App_Start;
using PayPal.Api;

namespace WebAppPerfume.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var user = SessionConfig.GetUser();

            if (user != null)
            {
                NUOCHOAEntities db = new NUOCHOAEntities();
                List<CART> cARTs = db.CARTs.Where(item => item.idUSER == user.USERID).ToList();

                decimal totalAmount = (decimal)cARTs.Sum(item => item.PRICE * item.QTY);

                // Lưu giá trị TotalAmount vào TempData
                TempData["TotalAmount"] = totalAmount;

                // Đưa tổng tiền vào ViewBag để sử dụng trong View
                ViewBag.TotalAmount = totalAmount;

                return View(cARTs);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult CreateCart(/*string productid, string name, decimal price, int qty, int userid*/CART cART)
        {
            try
            {
                var user = SessionConfig.GetUser();
                if (user != null)
                {
                    NUOCHOAEntities db = new NUOCHOAEntities();
                    var existingCartItem = db.CARTs.FirstOrDefault(item => item.idUSER == user.USERID && item.idPROD == cART.idPROD);
                    if (existingCartItem != null)
                    {
                        // Nếu có sản phẩm trùng, tăng số lượng lên
                        existingCartItem.QTY += cART.QTY;
                    }
                    else
                    {
                        // Nếu không có sản phẩm trùng, thêm mới vào giỏ hàng
                        cART.idUSER = user.USERID;
                        db.CARTs.Add(cART);
                    }

                    //cART.idUSER = user.USERID;
                    // Tạo một đối tượng CART mới
                    //CART cartItem = new CART
                    //{
                    //    ProductID = productid,
                    //    Name = name,
                    //    Price = price,
                    //    Quantity = qty,
                    //    UserID = userid
                    //};

                    // Thêm đối tượng CART vào cơ sở dữ liệu
                    //db.CARTs.Add(cART);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
                }
                else
                {
                    TempData["err"] = "Bạn chưa đăng nhập";
                    return RedirectToAction("Chitiet", "Sanpham");
                }

            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm vào giỏ hàng." });
            }
        }
        public ActionResult Delete(int id)
        {
            NUOCHOAEntities db = new NUOCHOAEntities();
            CART cART = db.CARTs.Where(row => row.CARTID == id).FirstOrDefault();
            db.CARTs.Remove(cART);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Confirm(ORDER oRDER)
        {
            //var user = SessionConfig.GetUser();
            //NUOCHOAEntities db = new NUOCHOAEntities();
            //oRDER.USERID = user.USERID;
            //db.ORDERs.Add(oRDER);
            //db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult Success(string name, string address)
        {
            try
            {
                // Lấy người dùng hiện tại từ session
                var user = SessionConfig.GetUser();

                // Kiểm tra xem người dùng có tồn tại không
                if (user != null)
                {
                    // Lấy danh sách sản phẩm trong giỏ hàng của người dùng
                    using (var db = new NUOCHOAEntities())
                    {
                        var cartItems = db.CARTs.Where(c => c.idUSER == user.USERID).ToList();

                        // Kiểm tra xem giỏ hàng có sản phẩm không
                        if (cartItems.Any())
                        {
                            // Lấy giá trị TotalAmount từ TempData
                            double totalAmount = Convert.ToDouble(TempData["TotalAmount"]);
                            // Tạo một đối tượng ORDER cho người dùng
                            var order = new ORDER
                            {
                                USERID = user.USERID,
                                STATUS = "CHƯA THANH TOÁN", // Hoặc trạng thái khác tùy thuộc vào yêu cầu của bạn,
                                CREATETIME = DateTime.Now,
                                Receiver = name,
                                Address = address,
                                Total = totalAmount
                            };
                            db.ORDERs.Add(order);
                            db.SaveChanges();

                            // Lấy orderid của đơn hàng vừa tạo
                            var orderId = order.ORDERID;

                            // Thêm các sản phẩm trong giỏ hàng vào bảng OrderDetail
                            foreach (var cartItem in cartItems)
                            {
                                var orderDetail = new OrderDetail
                                {
                                    orderID = orderId,
                                    proName = cartItem.NAME,
                                    proID = cartItem.idPROD,
                                    proPrice = cartItem.PRICE,
                                    proPIC = cartItem.PIC,
                                    qty = cartItem.QTY,
                                };
                                db.OrderDetails.Add(orderDetail);
                            }

                            // Xóa các sản phẩm đã được thêm vào đơn hàng khỏi giỏ hàng
                            db.CARTs.RemoveRange(cartItems);
                            db.SaveChanges();

                            // Chuyển hướng đến trang thông báo thành công
                            return View();
                        }
                        else
                        {
                            // Nếu giỏ hàng trống, trả về trang thông báo lỗi
                            TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                            return RedirectToAction("Index", "Cart");
                        }
                    }
                }
                else
                {
                    // Nếu người dùng không tồn tại, trả về trang đăng nhập
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu cần
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xử lý đơn hàng.";
                return RedirectToAction("Index", "Cart");
            }
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult Pay2PayPal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/Pay2PayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    // Lấy thông tin từ executedPayment
                    string idOrder = executedPayment.payer.payer_info.payer_id;
                    string state = executedPayment.state;
                    string payer = executedPayment.payer.payer_info.shipping_address.recipient_name;
                    string add = executedPayment.payer.payer_info.shipping_address.line1;
                    string method = executedPayment.payer.payment_method;
                    DateTime createTime = DateTime.Parse(executedPayment.create_time);

                    var user = SessionConfig.GetUser();
                    NUOCHOAEntities db = new NUOCHOAEntities();
                    var cartItems = db.CARTs.Where(c => c.idUSER == user.USERID).ToList();
                    double totalAmount = Convert.ToDouble(cartItems.Sum(item => item.PRICE * item.QTY));
                    // Thêm thông tin đơn hàng
                    var order = new ORDER
                    {
                        STATUS = state,
                        CREATETIME = createTime,
                        USERID = user.USERID,
                        Address = add,
                        Receiver = payer,
                        Total = totalAmount,
                        Method = method
                    };
                    db.ORDERs.Add(order);
                    var orderId = order.ORDERID;
                    // Thêm thông tin chi tiết đơn hàng
                    foreach (var item in cartItems)
                    {
                        var orderDetail = new OrderDetail
                        {
                            orderID = orderId,
                            proName = item.NAME,
                            proPIC = item.PIC,
                            proID = item.idPROD,
                            proPrice = item.PRICE,
                            qty = item.QTY
                            // Thêm các thông tin khác nếu cần
                        };
                        db.OrderDetails.Add(orderDetail);
                    }

                    // Xóa các mục trong giỏ hàng của người dùng
                    db.CARTs.RemoveRange(cartItems);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var user = SessionConfig.GetUser();
            NUOCHOAEntities db = new NUOCHOAEntities();
            var items = db.CARTs.Where(c => c.idUSER == user.USERID).ToList();

            // Tính toán subtotal của các sản phẩm
            decimal subtotal = 0;

            // Tính toán total của đơn hàng (bao gồm subtotal, thuế và phí vận chuyển)
            //var total = subtotal + 1 + 1;
            
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            foreach (var i in items)
            {
                decimal priceUSD = Math.Round((decimal)i.PRICE / 25000, 2);
                itemList.items.Add(new Item()
                {
                    name = i.NAME,
                    currency = "USD",
                    price = Math.Round((decimal)i.PRICE / 25000).ToString(),
                    quantity = i.QTY.ToString(),
                    sku = i.idPROD.ToString()
                });
                subtotal += priceUSD * (decimal)i.QTY;
            }

            decimal tax = 0;
            decimal shipping = 1;
            decimal total = subtotal + tax + shipping;

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = tax.ToString(),
                shipping = shipping.ToString(),
                subtotal = subtotal.ToString("0.00")
            };
            //Final amount with details
            //var amount = new Amount()
            //{
            //    currency = "USD",
            //    total = total.ToString(),
            //    details = new Details()
            //    {
            //        tax = "1", // Thay đổi giá trị thuế tùy thuộc vào yêu cầu của bạn
            //        shipping = "1", // Thay đổi giá trị phí vận chuyển tùy thuộc vào yêu cầu của bạn
            //        subtotal = subtotal.ToString()
            //    }
            //};

            var amount = new Amount()
            {
                currency = "USD",
                total = total.ToString("0.00"), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}