using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Controllers
{

    public class ReturnValue
    {
        public string error;
        public string message; //Server taraflı işlem sonu dönen mesaj
        public bool requiredLogin;
        public object retObject;
        public bool success;

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