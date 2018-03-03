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
        
        public int defaultPageSize = 6;

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
                    IQueryable<Orders> query = null;
                    if (string.IsNullOrEmpty(filter))
                    {
                        query = db.Orders.Where(x => 1 == 1);
                    }
                    else
                    {
                        query = db.Orders.Where(x => x.CustomerName.Contains(filter));
                    }

                    orders.Orders = query.OrderByDescending(x => x.Order_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
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

        public JsonResult GetOperations(int Product_Id)
        {
            using (var db = new KTOtomasyonEntities())
            {
                var operationdata = db.Operations.Where(x => x.Product_Id == Product_Id).Select(x => new { Name = x.Name, UnitPrice = x.UnitPrice, Operation_Id = x.Operation_Id}).ToList();

                return Json(operationdata);
            }
                
        }

        //public ActionResult Users(int? p, string filter)
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
        //            IQueryable<Users> query = null;
        //            if (string.IsNullOrEmpty(filter))
        //            {
        //                query = db.Users.Where(x => 1 == 1 && x.IsDeleted == false);
        //            }
        //            else
        //            {
        //                query = db.Users.Where(x => x.IsDeleted == false && (x.Name.Contains(filter) || x.Surname.Contains(filter)));
        //            }

        //            mylist.Users = query.OrderByDescending(x => x.Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
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

        //public ActionResult UserAdder(Users user)
        //{
        //    Users useradd = new Users();
        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        useradd.Name = user.Name;
        //        useradd.Surname = user.Surname;
        //        useradd.Age = null;
        //        useradd.Gender = user.Gender;
        //        useradd.Mail = user.Mail;
        //        useradd.Password = user.Name;
        //        useradd.IsDeleted = false;
        //        useradd.Role = 1;

        //        db.Users.Add(useradd);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //public ActionResult UserDetail(int id)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        Users userdetail = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            userdetail = db.Users.Where(d => d.Id == id).First();
        //        }
        //        return View(userdetail);
        //    }
        //}

        //public ActionResult UserEdit(int id)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        Users useredit = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            useredit = db.Users.Where(d => d.Id == id).First();
        //        }
        //        return View(useredit);
        //    }
        //}

        //public ActionResult UserUpdate(Users user)
        //{
        //    Users userupdate = null;

        //    using (var db = new KTOtomasyonEntities())
        //    {
        //        userupdate = db.Users.Where(d => d.Id == user.Id).First();
        //        userupdate.Name = user.Name;
        //        userupdate.Surname = user.Surname;
        //        userupdate.Age = user.Age;
        //        userupdate.Gender = user.Gender;
        //        userupdate.Mail = user.Mail;
        //        userupdate.Password = user.Password;
        //        userupdate.Role = user.Role;
        //        userupdate.IsDeleted = user.IsDeleted;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("UserDetail", new { id = user.Id });

        //}

        //public ActionResult UserDelete(Users user)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        Users userdelete = null;

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            userdelete = db.Users.Where(d => d.Id == user.Id).First();
        //            userdelete.Name = user.Name;
        //            userdelete.Surname = user.Surname;
        //            userdelete.Age = user.Age;
        //            userdelete.Gender = user.Gender;
        //            userdelete.Mail = user.Mail;
        //            userdelete.Password = user.Password;
        //            userdelete.Role = user.Role;
        //            userdelete.IsDeleted = true;

        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}

        //public ActionResult NewOrder(string pfilter, string AccordionIndex)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {

        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            //Filtre
        //            IQueryable<Operations> query = null;
        //            if (string.IsNullOrEmpty(pfilter))
        //            {
        //                query = db.Operations.Where(x => x.PId == 1);
        //            }
        //            else
        //            {
        //                query = db.Operations.Where(x => x.IsActive == true && (x.PId.ToString() == pfilter));
        //            }

        //            //ViewModel
        //            DisplayOrderDetail vmDisplayOrderDetail = new DisplayOrderDetail();

        //            vmDisplayOrderDetail.CreatedUser = db.Users.First().Id;

        //        }

                
        //        if (AccordionIndex == "")
        //        {
        //            ViewBag.AccordionIndex = "0";
        //        }
        //        else
        //        {
        //            ViewBag.AccordionIndex = AccordionIndex;
        //        }


                
        //        return View(mylist);

        //    }
        //}


        //public ActionResult Orders(int? p, string filter)
        //{
        //    if (p == null)
        //        p = 1;

        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {

        //        //Görevleri alıyoruz
        //        using (var db = new KTOtomasyonEntities())
        //        {
        //            //Filter
        //            IQueryable<Orders> query = null;
        //            if (string.IsNullOrEmpty(filter))
        //            {
        //                query = db.Orders.Where(x => 1 == 1);
        //            }
        //            //else
        //            //{
        //            //    query = db.Orders.Where(x => x.Title.Contains(filter));
        //            //}

        //            mylist.Orders = query.OrderByDescending(x => x.Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
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



        //public ActionResult Customers(int? p, string filter)
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
        //            IQueryable<Customers> query = null;
        //            if (string.IsNullOrEmpty(filter))
        //            {
        //                query = db.Customers.Where(x => 1 == 1 && x.IsActive == true);
        //            }
        //            else
        //            {
        //                query = db.Customers.Where(x => x.IsActive == true && (x.Name.Contains(filter) || x.Surname.Contains(filter)));
        //            }

        //            mylist.Customers = query.OrderByDescending(x => x.Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
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

        //public ActionResult CustomerAdder(Customers customer)
        //{
        //    Customers customeradd = new Customers();
        //    using (var db = new KTOtomasyonEntities())
        //    {

        //        customeradd.Name = customer.Name;
        //        customeradd.Surname = customer.Surname;
        //        customeradd.Age = customer.Age;
        //        customeradd.Gender = customer.Gender;
        //        customeradd.PhoneNumber = customer.PhoneNumber;
        //        customeradd.Address = customer.Address;
        //        customeradd.Mail = customer.Mail;
        //        customeradd.IsActive = true;

        //        db.Customers.Add(customeradd);
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


        //public ActionResult Operations(int? p, string filter)
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
        //            IQueryable<Operations> query = null;
        //            if (string.IsNullOrEmpty(filter))
        //            {
        //                query = db.Operations.Where(x => 1 == 1 && x.IsActive == true);
        //            }
        //            else
        //            {
        //                query = db.Operations.Where(x => x.IsActive == true && (x.Name.Contains(filter)));
        //            }

        //            mylist.Operations = query.OrderByDescending(x => x.Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
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


    }
}