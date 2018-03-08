using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;


namespace KTOtomasyon.Controllers
{

    public class HomeController : Controller
    {
        //Bir sayfadaki kayıt sayısıdır.
        public int defaultPageSize = 12;

        public ActionResult ErrorPage()
        {
            return View();
        }

        //Tüm siparişleri listelenir.
        public ActionResult Index(int? p, string filter)
        {
            DisplayOrderDetail orders = new DisplayOrderDetail();

            if (p == null)
                p = 1;

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Siparişleri databeseden alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<vOrders> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.vOrders.Where(x => 1 == 1);
                        }
                        else
                        {
                            query = db.vOrders.Where(x => x.CustomerName.Contains(filter));
                        }

                        orders.OrdersList = query.OrderByDescending(x => x.Order_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();

                        orders.CurrentPage = p.Value;
                        orders.TotalCount = query.Count();

                        if ((orders.TotalCount % defaultPageSize) == 0)
                        {
                            orders.TotalPage = orders.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            orders.TotalPage = (orders.TotalCount / defaultPageSize) + 1;
                        }
                    }
                }
                catch (Exception)
                {
                    RedirectToAction("ErrorPage", "Home");
                }


                return View(orders);

            }


        }

        //Sipariş kaydet işlemini gerçekleştirir.
        public JsonResult OrderSave(OrderWithDetail orderWithDetail)
        {
            Orders orderadd = new Orders();
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }

            try
            {
                ret.success = false;

                //Sipariş id'sine göre kayıtları databaseden çeker
                using (var db = new KTOtomasyonEntities())
                {
                    if (orderWithDetail.OrderDetails == null || orderWithDetail.OrderDetails.Count == 0)
                    {
                        ret.requiredLogin = false;
                        ret.message = "Lütfen işlem giriniz.";
                        ret.success = false;
                        return Json(ret);
                    }

                    if (orderWithDetail.Order_Id != 0)
                    {
                        orderadd = db.Orders.Where(x => x.Order_Id == orderWithDetail.Order_Id).FirstOrDefault();
                    }

                    orderadd.CustomerName = orderWithDetail.CustomerName;
                    orderadd.PhoneNumber = orderWithDetail.PhoneNumber;
                    orderadd.Description = orderWithDetail.Description;
                    orderadd.OrderDate = orderWithDetail.OrderDate;
                    orderadd.CreatedDate = orderWithDetail.OrderDate;
                    orderadd.CreatedUser = Convert.ToInt32(Session["UserId"]);
                    orderadd.IsDelivered = orderWithDetail.IsDelivered;
                    orderadd.IsDeleted = orderWithDetail.IsDeleted;

                    if (orderWithDetail.Order_Id == 0)
                    {
                        db.Orders.Add(orderadd);
                    }
                    db.SaveChanges();

                    int id = orderadd.Order_Id;
                    foreach (var item in orderadd.OrderDetail.ToList())
                    {
                        db.OrderDetail.Remove(item);
                    }
                    db.SaveChanges();

                    foreach (var item in orderWithDetail.OrderDetails)
                    {
                        OrderDetail odetail = new OrderDetail();

                        odetail.Order_Id = id;
                        odetail.Operation_Id = item.Operation_Id;
                        odetail.Quantity = item.Quantity;
                        odetail.Price = item.Price;
                        odetail.TotalPrice = item.TotalPrice;


                        db.OrderDetail.Add(odetail);


                    }

                    db.SaveChanges();

                    ret.retObject = orderWithDetail;
                }
                ret.message = "Başarıyla kaydedildi.";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.message = ex.Message;
            }

            return Json(ret);
        }

        //Her ürünün tadilatlarının listesini çeker.
        public JsonResult GetOperations(int Product_Id)
        {
            try
            {
                using (var db = new KTOtomasyonEntities())
                {
                    var operationdata = db.Operations.Where(x => x.Product_Id == Product_Id).Select(x => new { Name = x.Name, Price = x.Price, Operation_Id = x.Operation_Id }).ToList();

                    return Json(operationdata);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        //Tıklanılan siparişin detaylarını getirir.
        public JsonResult GetOrderData(int Order_Id)
        {
            OrderWithDetail orderWithDetail = new OrderWithDetail();

            try
            {
                using (var db = new KTOtomasyonEntities())
                {
                    var orderdata = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();

                    orderWithDetail.Order_Id = orderdata.Order_Id;
                    orderWithDetail.CreatedUser = orderdata.CreatedUser;
                    orderWithDetail.CreatedUserText = orderdata.Users.Name;
                    orderWithDetail.CustomerName = orderdata.CustomerName;
                    orderWithDetail.PhoneNumber = orderdata.PhoneNumber;
                    orderWithDetail.Description = orderdata.Description;
                    orderWithDetail.OrderDate = orderdata.OrderDate;
                    orderWithDetail.CreatedDate = orderdata.CreatedDate;
                    orderWithDetail.IsDelivered = orderdata.IsDelivered;
                    orderWithDetail.IsDeleted = orderdata.IsDeleted;

                    orderWithDetail.OrderDetails = new List<OrderDetails>();

                    foreach (var item in orderdata.OrderDetail)
                    {
                        orderWithDetail.OrderDetails.Add(new OrderDetails
                        {
                            Order_Id = item.Order_Id,
                            Operation_Id = item.Operation_Id,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalPrice = item.TotalPrice,
                            OrderDetail_Id = item.OrderDetail_Id,
                            OperationText = item.Operations.Name

                        });
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(orderWithDetail);
        }

        //Makbuz görüntülenme ekranıdır.
        public ActionResult Receipt(int Order_Id)
        {           
            DisplayReceipt orderreceipt = new DisplayReceipt();

            

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        var orderdata = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();

                        orderreceipt.Order_Id = orderdata.Order_Id;
                        orderreceipt.CreatedUser = orderdata.CreatedUser;
                        orderreceipt.CreatedUserText = orderdata.Users.Name;
                        orderreceipt.CustomerName = orderdata.CustomerName;
                        orderreceipt.PhoneNumber = orderdata.PhoneNumber;
                        orderreceipt.Description = orderdata.Description;
                        orderreceipt.OrderDate = orderdata.OrderDate;
                        orderreceipt.CreatedDate = orderdata.CreatedDate;
                        orderreceipt.TTotalPrice = Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value);
                        orderreceipt.TQuantity = orderdata.OrderDetail.Sum(x => x.Quantity).Value;
                        orderreceipt.DetailList = new List<OrderDetails>();

                        foreach (var item in orderdata.OrderDetail.ToList())
                        {
                            orderreceipt.DetailList.Add(new OrderDetails
                            {
                                Operation_Id = item.Operation_Id,
                                Quantity = item.Quantity,
                                Price = Convert.ToInt32(item.Price),
                                TotalPrice = Convert.ToInt32(item.TotalPrice),
                                OrderDetail_Id = item.OrderDetail_Id,
                                OperationText = item.Operations.Name,
                                ProductName = item.Operations.Products.Name

                            });
                        }
                        
                    }
                }
                catch (Exception)
                {
                    RedirectToAction("ErrorPage", "Home");
                }
            }

            return PartialView(orderreceipt);
        }


        public ActionResult Users(int? p, string filter)
        {
            DisplayUsers user = new DisplayUsers();


            if (p == null)
                p = 1;

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Kullanıcıları alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<Users> query = null;

                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Users.Where(x => 1 == 1 && x.IsDeleted == false);
                        }
                        else
                        {
                            query = db.Users.Where(x => x.IsDeleted == false && (x.Name.Contains(filter)));
                        }

                        user.UsersList = query.OrderByDescending(x => x.User_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();

                        user.CurrentPage = p.Value;
                        user.TotalCount = query.Count();
                        if ((user.TotalCount % defaultPageSize) == 0)
                        {
                            user.TotalPage = user.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            user.TotalPage = (user.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(user);
            }

        }

        public ActionResult UserAdder(Users user)
        {
            Users useradd = new Users();
            using (var db = new KTOtomasyonEntities())
            {

                useradd.Name = user.Name;
                useradd.Gender = user.Gender;
                useradd.Mail = user.Mail;
                useradd.Password = user.Name;
                useradd.IsDeleted = false;
                useradd.UserType = 1;

                db.Users.Add(useradd);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserDetail(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users userdetail = null;

                using (var db = new KTOtomasyonEntities())
                {
                    userdetail = db.Users.Where(d => d.User_Id == id).First();
                }
                return View(userdetail);
            }
        }

        public ActionResult UserEdit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users useredit = null;

                using (var db = new KTOtomasyonEntities())
                {
                    useredit = db.Users.Where(d => d.User_Id == id).First();
                }
                return View(useredit);
            }
        }

        public ActionResult UserUpdate(Users user)
        {
            Users userupdate = null;

            using (var db = new KTOtomasyonEntities())
            {
                userupdate = db.Users.Where(d => d.User_Id == user.User_Id).First();
                userupdate.Name = user.Name;
                userupdate.Gender = user.Gender;
                userupdate.Mail = user.Mail;
                userupdate.Password = user.Password;
                userupdate.UserType = 1;
                userupdate.IsDeleted = user.IsDeleted;

                db.SaveChanges();
            }
            return RedirectToAction("UserDetail", new { id = user.User_Id });

        }

        public ActionResult UserDelete(Users user)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users userdelete = null;

                using (var db = new KTOtomasyonEntities())
                {
                    userdelete = db.Users.Where(d => d.User_Id == user.User_Id).First();
                    userdelete.IsDeleted = true;

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult Operations(int? p, string filter)
        {
            DisplayOperations operation = new DisplayOperations();

            if (p == null)
                p = 1;

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Tadilatları alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<Operations> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Operations.Where(x => 1 == 1 && x.IsActive == true);
                        }
                        else
                        {
                            query = db.Operations.Where(x => x.IsActive == true && (x.Name.Contains(filter)));
                        }

                        operation.OperationList = query.OrderByDescending(x => x.Operation_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();

                        operation.CurrentPage = p.Value;
                        operation.TotalCount = query.Count();
                        if ((operation.TotalCount % defaultPageSize) == 0)
                        {
                            operation.TotalPage = operation.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            operation.TotalPage = (operation.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(operation);
            }

        }

        public ActionResult Products(int? p, string filter)
        {
            DisplayProducts product = new DisplayProducts();

            if (p == null)
                p = 1;

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Ürünleri alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<Products> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Products.Where(x => 1 == 1 && x.IsActive == true);
                        }
                        else
                        {
                            query = db.Products.Where(x => x.IsActive == true && (x.Name.Contains(filter)));
                        }



                        product.ProductList = query.OrderByDescending(x => x.Product_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
                        product.CurrentPage = p.Value;
                        product.TotalCount = query.Count();
                        if ((product.TotalCount % defaultPageSize) == 0)
                        {
                            product.TotalPage = product.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            product.TotalPage = (product.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(product);
            }

        }

        public ActionResult ProductAdder(Products product)
        {
            Products newproduct = new Products();

            using (var db = new KTOtomasyonEntities())
            {

                newproduct.Name = product.Name;
                newproduct.PhotoUrl = product.PhotoUrl;
                newproduct.IsActive = true;

                db.Products.Add(newproduct);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SendMail()
        {
            SmtpClient client = new SmtpClient();

            //If you need to authenticate
            client.Credentials = new NetworkCredential("zubeyir.kocalioglu@gmail.com", "530549.zK");
            client.Host = "smtp.gmail.com";
            client.Port = 465;

            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("zubeyir.kocalioglu@gmail.com");
            mailMessage.To.Add("zubeyir.kocalioglu@gmail.com");
            mailMessage.Subject = "Hello There";
            mailMessage.Body = "Hello my friend!";

            client.Send(mailMessage);
            return RedirectToAction("Index","Home");
        }

        


    }
}