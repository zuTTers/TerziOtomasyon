using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Controllers
{

    public class ReturnValue
    {
        public string error; //Hata mesajı içeriği
        public string message; //Server taraflı işlem sonu dönen mesaj içeriği
        public bool requiredLogin; //Session kontrolü
        public object retObject; //İşlem sonu dönen data objesi
        public bool success; //İşlem başarı kontrolü

        public ReturnValue() { }
    }

    public class Shared
    {
        public static bool CheckSession()
        {           
            if (HttpContext.Current.Session["UserId"] == null)
            {
                return false;
            }
            return true;
        }
    }

}