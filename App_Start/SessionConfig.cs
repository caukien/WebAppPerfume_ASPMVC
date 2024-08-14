using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppPerfume.Models;

namespace WebAppPerfume.App_Start
{
    public static class SessionConfig
    {
        private const string SessionTokenKey = "daylabimat";

        //1. lưu user
        public static void SetUser(KHACHHANG kHACHHANG)
        {
            HttpContext.Current.Session[SessionTokenKey] = kHACHHANG;
        }
        public static KHACHHANG GetUser()
        {
            return (KHACHHANG)HttpContext.Current.Session[SessionTokenKey];
        }
        public static void ClearUser()
        {
            HttpContext.Current.Session.Remove(SessionTokenKey);
        }
    }
}