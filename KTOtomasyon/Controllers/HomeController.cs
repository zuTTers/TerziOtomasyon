using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;


namespace KTOtomasyon.Controllers
{

    public class HomeController : Controller
    {

        public int defaultPageSize = 12;

        //Tüm siparişleri listeler.
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
                return View(orders);

            }


        }

        //Sipariş kaydet işlemini gerçekleştirir.
        public JsonResult OrderSave(OrderWithDetail orderWithDetail)
        {
            Orders orderadd = new Orders();


            using (var db = new KTOtomasyonEntities())
            {

                if (orderWithDetail.Order_Id != 0)
                {
                    orderadd = db.Orders.Where(x => x.Order_Id == orderWithDetail.Order_Id).FirstOrDefault();
                }

                orderadd.CustomerName = orderWithDetail.CustomerName;
                orderadd.PhoneNumber = orderWithDetail.PhoneNumber;
                orderadd.Description = orderWithDetail.Description;
                orderadd.OrderDate = orderWithDetail.OrderDate;
                orderadd.CreatedDate = DateTime.Now;
                orderadd.CreatedUser = Convert.ToInt32(Session["UserId"]);
                orderadd.IsDelivered = orderWithDetail.IsDelivered;
                orderadd.IsDeleted = orderWithDetail.IsDeleted;

                if (orderWithDetail.Order_Id == 0)
                {
                    db.Orders.Add(orderadd);
                }
                 db.SaveChanges();

                int id = orderadd.Order_Id;

                foreach (var item in orderWithDetail.OrderDetails)
                {
                    OrderDetail odetail = new OrderDetail();

                    if (item.OrderDetail_Id != 0)
                    {
                        odetail = db.OrderDetail.Where(x => x.OrderDetail_Id == item.OrderDetail_Id).FirstOrDefault();
                    }

                    odetail.Order_Id = id;
                    odetail.Operation_Id = item.Operation_Id;
                    odetail.Quantity = item.Quantity;
                    odetail.Price = item.Price;
                    odetail.TotalPrice = item.TotalPrice;

                    if (item.OrderDetail_Id == 0)
                    {                       
                        db.OrderDetail.Add(odetail);
                    }
                    
                }

                db.SaveChanges();
            }


            return Json("test");
        }

        //Her ürünün tadilatlarının listesini çekiyoruz.
        public JsonResult GetOperations(int Product_Id)
        {
            using (var db = new KTOtomasyonEntities())
            {
                var operationdata = db.Operations.Where(x => x.Product_Id == Product_Id).Select(x => new { Name = x.Name, Price = x.Price, Operation_Id = x.Operation_Id }).ToList();

                return Json(operationdata);
            }

        }

        //Tıklanılan siparişin detayları
        public JsonResult GetOrderData(int Order_Id)
        {
            OrderWithDetail orderWithDetail = new OrderWithDetail();

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

            return Json(orderWithDetail);
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
                //Kullanıcıları alıyoruz
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
                return View(operation);
            }

        }

        //public ActionResult OrderAdder(Orders order)
        //{
        //    Orders orderadd = new Orders();

        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        orderadd.CId = order.CId;
        //        orderadd.UId = order.UId;
        //        orderadd.Date = order.Date;
        //        orderadd.Time = order.Time;
        //        orderadd.CreatedDate = order.CreatedDate;

        //        db.Orders.Add(orderadd);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index", "Home");

        //}

        //public ActionResult OrderDetail(int id)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        OrderDetail orderdetail = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            orderdetail = db.OrderDetail.Where(d => d.Id == id).First();
        //        }
        //        return View(orderdetail);

        //    }
        //}

        //public ActionResult OrderEdit(int id)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        OrderDetail orderedit = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            orderedit = db.OrderDetail.Where(d => d.OrderId == id).First();
        //        }
        //        return View(orderedit);
        //    }
        //}

        //public ActionResult OrderUpdate(Orders order)
        //{
        //    Orders orderupdate = null;

        //    using (var db = new KTOtomasyonEntities())
        //    {
        //        orderupdate = db.Orders.Where(d => d.Id == order.Id).First();
        //        orderupdate.CId = order.CId;
        //        orderupdate.UId = order.UId;
        //        orderupdate.Date = order.Date;
        //        orderupdate.Time = order.Time;
        //        orderupdate.CreatedDate = order.CreatedDate;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("OrderDetail", new { id = order.Id });

        //}//no

        //public ActionResult OrderDelete(Orders order)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        Orders orderdelete = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            orderdelete = db.Orders.Where(d => d.Id == order.Id).First();
        //            orderdelete.CId = order.CId;
        //            orderdelete.UId = order.UId;
        //            orderdelete.Date = order.Date;
        //            orderdelete.Time = order.Time;
        //            orderdelete.CreatedDate = order.CreatedDate;

        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}//no

        //public ActionResult OrderAdd(Orders order)
        //{
        //    Orders orderadd = new Orders();
        //    OrderDetail orderadd1 = new OrderDetail();

        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        orderadd.CId = order.CId;
        //        orderadd.UId = order.UId;
        //        orderadd.Date = order.Date;
        //        orderadd.Time = order.Time;
        //        orderadd.CreatedDate = order.CreatedDate;


        //        db.Orders.Add(orderadd);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Tasks", "Home");

        //}

        //public ActionResult OrderDetailAdder(OrderDetail orderdetail)
        //{
        //    OrderDetail orderdetailadd = new OrderDetail();
        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        orderdetailadd.OperationId = orderdetail.OperationId;
        //        orderdetailadd.Quantity = orderdetail.Quantity;
        //        orderdetailadd.OrderId = orderdetail.OrderId;
        //        orderdetailadd.Discount = orderdetail.Discount;
        //        orderdetailadd.NetPrice = orderdetail.NetPrice;
        //        orderdetailadd.Description = orderdetail.Description;
        //        orderdetailadd.IsDeleted = false;
        //        orderdetailadd.IsDelivered = false;


        //        db.OrderDetail.Add(orderdetailadd);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //public ActionResult Products(int? p, string filter)
        //{
        //    if (p == null)
        //        p = 1;

        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        //Kullanıcıları alıyoruz
        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            //Filter
        //            IQueryable<Products> query = null;
        //            if (string.IsNullOrEmpty(filter))
        //            {
        //                query = db.Products.Where(x => 1 == 1 && x.IsActive == true);
        //            }
        //            else
        //            {
        //                query = db.Products.Where(x => x.IsActive == true && (x.Name.Contains(filter)));
        //            }

        //            mylist.Products = query.OrderByDescending(x => x.Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
        //            mylist.CurrentPage = p.Value;
        //            mylist.TotalCount = query.Count();
        //            if ((mylist.TotalCount % defaultPageSize) == 0)
        //            {
        //                mylist.TotalPage = mylist.TotalCount / defaultPageSize;
        //            }
        //            else
        //            {
        //                mylist.TotalPage = (mylist.TotalCount / defaultPageSize) + 1;
        //            }

        //        }
        //        return View(mylist);
        //    }

        //}

        //public ActionResult ProductAdder(Products product)
        //{
        //    Products newproduct = new Products();

        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        newproduct.Name = product.Name;
        //        newproduct.PhotoUrl = product.PhotoUrl;
        //        newproduct.IsActive = true;

        //        db.Products.Add(newproduct);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //public ActionResult ProductSelect(int id)
        //{

        //    OrderDetail addproduct = null;

        //    using (var db = new KTOtomasyonEntities())
        //    {
        //        addproduct = db.OrderDetail.Where(d => d.Id == id).First();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}




    }
}