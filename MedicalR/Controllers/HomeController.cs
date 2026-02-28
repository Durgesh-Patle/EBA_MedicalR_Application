using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.ForgetPass;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.ForgetPass;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models;
using MedicalR.Models.UserManagement;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MedicalR.Controllers
{
    public class HomeController : Controller
    {
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        IDALForgetPassword objForgetPassword = new DALForgetPassword();
        public ActionResult Index(string ReturnUrl)
        {
            if (Session["UserDetails"] != null)
            {
                return RedirectToAction("Index", "dashboard");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel objModel)
        {
            string empCode = DecryptData(objModel.EmpCode);
            string password = DecryptData(objModel.Password);

            objModel.EmpCode = empCode;
            objModel.Password = password;

            string mappedEmpCode = objModel.EmpCode?.ToLower();

            switch (mappedEmpCode)
            {
                case "sohamp":
                    mappedEmpCode = "2242";
                    break;
                case "nikitap":
                    mappedEmpCode = "1521";
                    break;
                default:
                    mappedEmpCode = objModel.EmpCode;
                    break;
            }

            if (objModel.otp != null)
            {
                var otpDetails = objForgetPassword.GetOtpDetails(objModel.EmpCode);
                if (otpDetails != null)
                {
                    if (otpDetails.FailedAttempts == 2 && otpDetails.is_locked == 'Y')
                    {
                        objForgetPassword.UnlockEmployeeOtps(objModel.EmpCode);
                    }

                    var otpDetails2 = objForgetPassword.GetOtpDetails(objModel.EmpCode);
                    if (otpDetails2.FailedAttempts == 2 && otpDetails2.is_locked == 'Y')
                    {
                        TempData["ErrorMessage"] = "Your account is locked for 10 minutes due to multiple failed OTP attempts. Please try again later.";
                        TempData["LockStatus"] = true;
                        return View(objModel);
                    }
                }
                //if (!objForgetPassword.CheckRecentOtp(objModel.EmpCode))
                //{
                //    return RedirectToAction("Index");
                //}
                if (!objForgetPassword.VerifyEmployeeOtp(objModel.EmpCode, objModel.otp))
                {
                    objForgetPassword.UpdateEmployeeOtp(objModel.EmpCode, objModel.otp);
                    //Session.Remove("login_user2");
                    //Session.Remove("login_user");
                    TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
                    return View(objModel);
                }

                // After OTP verification, check Travel Desk users
                if (objModel.EmpCode.ToLower() == "traveldesk" || objModel.EmpCode.ToLower() == "abhishekm")
                {
                    bool is_travel_desk = objDALUserManagement.ValidateUserTravelDesk(objModel);
                    if (is_travel_desk)
                    {
                        objDALUserManagement.UpdateUserSession(objModel.EmpCode, true, false);
                        ViewBag.login_user = objModel.EmpCode.ToLower();
                        return RedirectToAction("Index", "IntermediateApp");
                    }
                    else
                    {
                        ViewBag.errorMessage = MessageHelper.InvalidCredentials;
                        return View();
                    }
                }

                if (AD_Authentication.CheckAccountLock_status(objModel.EmpCode))
                {
                    CommonHelper.write_log($"Account lock status --> : true | Employee code: {objModel.EmpCode}");
                    ViewBag.errorMessage = MessageHelper.User_AD_AccountLocked;
                }
                else
                {
                    var result = objDALUserManagement.ValidateUser(objModel);
                    if (result != null)
                    {
                        if (result.UserID != 0 && !result.IsLoggedIn)
                        {
                            // // 🔐 FIX 1: added new
                            //Session.Clear();
                            //Session.Abandon();
                            //System.Web.Security.FormsAuthentication.SignOut();
                            //System.Web.Security.FormsAuthentication.SetAuthCookie(result.UserName, false);
                            //added new


                            string userDetails = JsonConvert.SerializeObject(result);
                            Session["UserDetails"] = userDetails;
                            Session["EmployeeSideBarInfor"] = objDALUserManagement.GetEmplSidebarInfo(result.UserID);
                            Session["user_menus"] = objDALUserManagement.GetMenus();
                            Session["lst_ids"] = Helper.GetEmployeeWise_table_ids();
                            Session["last_activity_time"] = DateTime.Now;

                            // // 🔐 FIX 2: added new
                            Session["UserIP"] = Request.UserHostAddress;
                            Session["UserAgent"] = Request.UserAgent;
                            //added new

                            HelperLogs.LogUserActivity(UserManager.User.Employeecode, "Login", "");
                            objDALUserManagement.UpdateUserSession(objModel.EmpCode, true, false);
                            ViewBag.login_user = objModel.EmpCode;

                            if (!string.IsNullOrWhiteSpace(objModel.ReturnUrl))
                            {
                                return Redirect(objModel.ReturnUrl);
                            }

                            var menuHeaders = UserManager.UserMenus;
                            if (menuHeaders.Any(x => x.parentmenu_name == "Medical Reimbursement") && UserManager.User.role_code == "#CHCUser#")
                            {
                                ViewBag.eba_url = "MedicalRequest";
                                return RedirectToAction("Index", "IntermediateApp");
                            }

                            if (menuHeaders.Any(x => x.parentmenu_name == "CHC") && UserManager.User.role_code == "#CHCUser#")
                            {
                                ViewBag.eba_url = "CHCRequestform";
                                return RedirectToAction("Index", "IntermediateApp");
                            }

                            ViewBag.eba_url = "dashboard";
                            return RedirectToAction("Index", "IntermediateApp");
                        }
                        else if (result.IsLoggedIn)
                        {
                            ViewBag.errorMessage = MessageHelper.ConcurrentLoginMessage;

                            //TempData["Error"] = true;

                        }
                        else
                        {
                            ViewBag.errorMessage = MessageHelper.InvalidCredentials;
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = MessageHelper.InvalidCredentials;
                    }
                }
            }
            else
            {
                var otpDetails = objForgetPassword.GetOtpDetails(objModel.EmpCode);
                if (otpDetails != null)
                {
                    if (otpDetails.FailedAttempts == 2 && otpDetails.is_locked == 'Y')
                    {
                        objForgetPassword.UnlockEmployeeOtps(objModel.EmpCode);
                    }

                    var otpDetails2 = objForgetPassword.GetOtpDetails(objModel.EmpCode);
                    if (otpDetails2.FailedAttempts == 2 && otpDetails2.is_locked == 'Y')
                    {
                        TempData["ErrorMessage"] = "Your account is locked for 10 minutes due to multiple failed OTP attempts. Please try again later.";
                        return View(objModel);
                    }
                }
                TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
                return View(objModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendOtpForLogin(LoginViewModel objModel)
        {
            string mappedEmpCode = objModel.EmpCode?.ToLower();

            switch (mappedEmpCode)
            {
                case "sohamp":
                    mappedEmpCode = "2242";
                    break;
                case "nikitap":
                    mappedEmpCode = "1521";
                    break;
                default:
                    mappedEmpCode = objModel.EmpCode;
                    break;
            }
            objModel.EmpCode = DecryptData(objModel.EmpCode);
            objModel.Password = DecryptData(objModel.Password);


            var enc = CommonHelper.encrypt(objModel.EmpCode.ToLower());
            if (TempData["ErrorMessage"] != null)
            {
                var errorMessage = TempData["ErrorMessage"]?.ToString();
                return Json(new { success = false, message = errorMessage });
            }

            if (Session["sessionCaptcha"] == null)
            {
                return Json(new { success = false, message = "Captcha session expired. Please refresh and try again." });
            }

            string sessionCaptcha = Session["sessionCaptcha"].ToString();
            if (string.IsNullOrEmpty(objModel.CaptchaInput) || objModel.CaptchaInput != sessionCaptcha)
            {
                return Json(new { success = false, message = "Invalid captcha. Please try again." });
            }
            objDALUserManagement.UpdateUserSession(objModel.EmpCode, false, true);


            if (objModel != null)
            {
                Session["login_user2"] = objModel.EmpCode.ToLower();
                Session["login_user"] = CommonHelper.encrypt(objModel.EmpCode.ToLower());

                if (objModel.EmpCode.ToLower() == "traveldesk" || objModel.EmpCode.ToLower() == "abhishekm")
                {
                    bool is_travel_desk = objDALUserManagement.ValidateUserTravelDesk(objModel);
                    if (is_travel_desk)
                    {
                        objDALUserManagement.UpdateUserSession(objModel.EmpCode, true, false);
                        ViewBag.login_user = objModel.EmpCode.ToLower();
                        //return RedirectToAction("Index", "IntermediateApp");
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid Employee Code & Password." });

                        //ViewBag.errorMessage = MessageHelper.InvalidCredentials;
                        //return View();
                    }
                }
            }

            var result = objDALUserManagement.ValidateUser(objModel);
            if (result.EmailID != null)
            {
                //bool check2mint = objForgetPassword.IsOtpSentWithin2Minutes(objModel.EmpCode);
                //if (check2mint)
                //{
                //    return Json(new { success = false, message = "OTP already sent. Please wait for 2 minutes." });
                //}
                var otpDetails = objForgetPassword.GetOtpDetails(objModel.EmpCode);

                if (otpDetails != null)
                {
                    if (otpDetails.FailedAttempts == 2 && otpDetails.is_locked == 'Y')
                    {
                        int unlockstatus = objForgetPassword.UnlockEmployeeOtps(objModel.EmpCode);

                    }
                    var otpDetails2 = objForgetPassword.GetOtpDetails(objModel.EmpCode);
                    if (otpDetails.FailedAttempts == 2 && otpDetails2.is_locked == 'Y')
                    {
                        return Json(new { success = false, message = "Your account is locked for 10 minutes due to multiple failed OTP attempts. Please try again later." });
                    }
                }
                try
                {
                    string otp = new Random().Next(100000, 999999).ToString();
                    Session["otp"] = otp;
                    CommonHelper.write_log("otp exist or not : " + otp);
                    bool otpSent = false;
                    var monum = objForgetPassword.GetEmployeeMobileAndEmail(objModel.EmpCode);
                    bool otpSaved = objForgetPassword.ManageEmployeeOtp(objModel.EmpCode, null, otp);
                    CommonHelper.write_log("OTP saved or not: " + otpSaved);

                    //bool otpSentToMobile = false;
                    //bool otpSentToEmail = false;


                    if (string.IsNullOrEmpty(monum.Mobile) && string.IsNullOrEmpty(monum.Email))
                    {
                        CommonHelper.write_log($"Both Mobile and Email are null or empty. Cannot send OTP. Employee Code: {objModel.EmpCode}");
                        return Json(new { success = false, message = "Contact with HR" });
                    }


                    // Override mobile/email for specific employees
                    switch (objModel.EmpCode)
                    {
                        case "4421": // Mr Peshotan Dastur
                            CommonHelper.write_log($"Overriding Mobile for Employee {objModel.EmpCode}, Old: {monum.Mobile}, New: 9967632556");
                            //monum.Mobile = "7738749976"; 
                            monum.Mobile = "9967632556";
                            break;
                        case "4244":
                            CommonHelper.write_log($"Overriding Mobile for Employee {objModel.EmpCode}, New: 9967632556");
                            monum.Mobile = "9967632556";
                            break;
                        case "3195":
                            CommonHelper.write_log($"Overriding Email for Employee {objModel.EmpCode}, New: ceo@uti.co.in");
                            monum.Email = "ceo@uti.co.in";
                            break;

                        case "3933":
                            CommonHelper.write_log($"Overriding Email for Employee {objModel.EmpCode}, New: anita.dsouza@uti.co.in");
                            monum.Email = "anita.dsouza@uti.co.in";
                            break;

                        //case "5436":
                        case "4119":
                            CommonHelper.write_log($"Overriding Email for Employee {objModel.EmpCode}, New: gunwanti.ahuja@uti.co.in");
                            monum.Email = "durgesh.patle@cylsys.com";
                            monum.Mobile = "9009648050";
                            break;
                        //for local test
                        case "1548":
                            monum.Mobile = "9009648050";
                            monum.Email = "durgesh.patle@cylsys.com";
                            break;

                    }

                    if (!string.IsNullOrEmpty(monum.Mobile))
                    {
                        await SendOtpMobile(monum.Mobile, otp);
                    }

                    if (!string.IsNullOrEmpty(monum.Email))
                    {
                        await CommonHelper.SendOtpMailAsync(objModel.EmpCode, otp, monum.Email, monum.Mobile);
                    }

                    //CommonHelper.write_log("otp exist or not : " + otpSent);
                    return Json(new { success = true });


                }
                catch (Exception ex)
                {
                    CommonHelper.write_log("SendOtpForLogin() : " + ex.Message);
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false, message = "Invalid User and Password" });
        }

        public static string DecryptData(string cipher)
        {
            try
            {
                string key = "UTILoginKey12345";
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] fullCipher = Convert.FromBase64String(cipher);

                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = keyBytes.Take(16).ToArray();

                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(fullCipher))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("Error in async SendOtpMobile for Login() : " + ex.Message);
                return "";
            }

        }


        private async Task<bool> SendOtpMobile(string mobileNumber, string otp)
        {
            // Ensure modern TLS protocols are used
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string otpMessage;
                string baseUrl = "https://enterprise.smsgupshup.com/apps/apis/global/rest.php";
                otpMessage = $"{otp} is the OTP for the EBA Application and is valid for the next two minutes. UTIAMC";
                //string otpMessage = $"{otp} is the OTP for the Digicard Application and is valid for the next two minutes. UTIAMC";
                //   string otpMessage = $"{otp} is the OTP";
                string encodedOtpMessage = Uri.EscapeDataString(otpMessage);
                string userId = "UTIMF";
                string password = "ujgh55hs";
                string url = $"{baseUrl}?userid={userId}&password={password}&send_to={mobileNumber}&msg={encodedOtpMessage}";

                HttpClientHandler handler = new HttpClientHandler
                {
                    // Optional: Uncomment for debugging purposes
                    // ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //          _commonHelperService.LogError("Mobile Number: " + mobileNumber + " | Response: " + responseText);
                        CommonHelper.write_log("Mobile Number: " + mobileNumber + " | Response: " + responseText);

                        return responseText.Contains("SUCCESS");
                    }
                    else
                    {
                        //          _commonHelperService.LogError($"Failed to send OTP SMS. Status code: {response.StatusCode}");
                        CommonHelper.write_log($"Failed to send OTP SMS. Status code: {response.StatusCode}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //      _commonHelperService.LogError("Error in SendOtpMobileAsync() : " + ex.Message);
                CommonHelper.write_log("Error in async SendOtpMobile for Login() : " + ex.Message);
                return false;
            }
        }


        public ActionResult Logout()
        {
            var a = Session["UserDetails"];
            //Session["UserDetails"] = userDetails;
            //DALForgetPassword.LogoutOtp(a.emp);
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Session["UserDetails"] = null;
            return RedirectToAction("Index");
        }

        [HttpGet]
        //[Route("Home/UserLogout/{emp_code}")]
        public ActionResult Logout(string emp_code)
        {
            emp_code = CommonHelper.Decrypt(emp_code);
            DALForgetPassword.LogoutOtp(emp_code);
            objDALUserManagement.UpdateUserSession(emp_code, false, false);
            HelperLogs.LogUserActivity(UserManager.User.Employeecode, "Logout", "");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Session["UserDetails"] = null;
            return RedirectToAction("Index");
        }

        //To Genereate Captcha
        public ActionResult GenerateCaptcha()
        {
            Random random = new Random();
            Bitmap bitmap = new Bitmap(150, 90);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.DrawLine(Pens.Black, random.Next(0, 50), random.Next(10, 30), random.Next(0, 200), random.Next(0, 50));
            graphics.DrawRectangle(Pens.Blue, random.Next(0, 20), random.Next(0, 20), random.Next(50, 80), random.Next(0, 20));
            graphics.DrawLine(Pens.Blue, random.Next(0, 20), random.Next(10, 50), random.Next(100, 200), random.Next(0, 80));
            Brush disignBrush = default(Brush);
            //captcha background style  
            HatchStyle[] bkgStyle = new HatchStyle[]
            {
            HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
            HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
            HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
            };
            //create captcha rectangular area for ui 
            RectangleF rectagleArea = new RectangleF(0, 0, 250, 250);
            disignBrush = new HatchBrush(bkgStyle[random.Next(bkgStyle.Length - 3)], Color.FromArgb((random.Next(100, 255)), (random.Next(100, 255)), (random.Next(100, 255))), Color.White);
            graphics.FillRectangle(disignBrush, rectagleArea);
            //generate captcha code with random code
            string captchaCode = string.Format("{0:X}", random.Next(1000000, 9999999));
            //add catcha code into session for use
            Session["sessionCaptcha"] = captchaCode;
            Font objFont = new Font("Times New Roman", 25, FontStyle.Bold);
            //create image for captcha
            graphics.DrawString(captchaCode, objFont, Brushes.Black, 20, 20);
            //Save the image 
            bitmap.Save(Response.OutputStream, ImageFormat.Gif);
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byteArray = stream.ToArray();
            }

            return File(byteArray, "image/png");

        }



    }
}