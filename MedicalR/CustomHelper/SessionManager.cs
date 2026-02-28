using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class SessionManager
    {
        public static void SetValue(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static object GetValue(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session[key];
            }

            return null;
        }
    }
}