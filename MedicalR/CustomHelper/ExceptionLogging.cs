using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class ExceptionLogging
    {
        public static void LogException(Exception ex)
        {
            CommonHelper.write_log("error :" + ex.Message);
        }
        public static void LogException(string msg, Exception ex)
        {
            CommonHelper.write_log(msg + " |" + ex.Message);
        }
    }
}