using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace VehicleRegistration.Utility
{
    public static class GlobalUtility
    {
        private static readonly string _BaseURL;

        static GlobalUtility()
        {
            _BaseURL = WebConfigurationManager.AppSettings["DefaultPagSize"];
        }

        public static bool IsLogin
        {
            get
            {
                return HttpContext.Current.Session["UserId"] != null ? true : false;
            }
        }
        public static long UserId
        {
            get
            {
                long uid = 0;
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    uid = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
                }
                return uid;
            }
        }
        public static string UserName
        {
            get
            {
                string uname = "";
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    uname = HttpContext.Current.Session["UserName"].ToString();
                }
                return uname;
            }
        }
        public static string EmailID
        {
            get
            {
                string uname = "";
                if (HttpContext.Current.Session["EmailID"] != null)
                {
                    uname = HttpContext.Current.Session["EmailID"].ToString();
                }
                return uname;
            }
        }
        public static string MobileNo
        {
            get
            {
                string uname = "";
                if (HttpContext.Current.Session["MobileNo"] != null)
                {
                    uname = HttpContext.Current.Session["MobileNo"].ToString();
                }
                return uname;
            }
        }

        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}