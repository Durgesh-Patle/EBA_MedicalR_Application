using MedicalR.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class GoogleDriveHelper
    {
        public static int Upload(HttpPostedFileBase File,string folderName)
        {
            try
            {
                var ProjectID = CommonHelper.GetProjectID;
                var CompanyID = UserManager.User.CompanyID.ToString();
                var CompanyUserID = UserManager.User.UserID.ToString();
                var GDUploadURL = CommonHelper.GetGDServiceUploadURL;

                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] Bytes = new byte[File.InputStream.Length + 1];
                        File.InputStream.Read(Bytes, 0, Bytes.Length);
                        var fileContent = new ByteArrayContent(Bytes);
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("File") { FileName = File.FileName };
                        content.Add(fileContent);
                        content.Add(new StringContent(ProjectID), "ProjectID");
                        content.Add(new StringContent(folderName), "FolderName");
                        content.Add(new StringContent(CompanyUserID), "CompanyUserID");
                        content.Add(new StringContent(CompanyID), "CompanyID");
                        var requestUri = GDUploadURL;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var result = client.PostAsync(requestUri, content).Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var ResultString = result.Content.ReadAsStringAsync().Result;
                            var ResultObj = JsonConvert.DeserializeObject<UploadResponseModel>(ResultString);
                            if(ResultObj.Status==true)
                            {
                                return ResultObj.FileID;
                            }
                            else
                            {
                                return 0;
                            }
                        } 
                        else
                        {
                            return 0;
                        }
                    }
                }

              
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return 0;
            }
        }
        public static string Download(int FileID, bool RelativePath=false, string CompanyID="")
        {
            try
            {
                var ProjectID = CommonHelper.GetProjectID;
             
                var GDDownLoadURL = CommonHelper.GetGDServiceDownloadURL;
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StringContent(ProjectID), "ProjectID");
                        content.Add(new StringContent(FileID.ToString()), "FileID");
                        content.Add(new StringContent(CompanyID), "CompanyID");
                        var requestUri = GDDownLoadURL;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var result = client.PostAsync(requestUri, content).Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var ResultString = result.Content.ReadAsStringAsync().Result;
                            var ResultObj = JsonConvert.DeserializeObject<DownLoadResponseModel>(ResultString);
                            if (ResultObj.Status == true)
                            {
                                if (RelativePath == true)
                                {
                                    return CommonHelper.GetGDServiceAbsolutePath + ResultObj.FileRelativePath;
                                }
                                else
                                {
                                    return CommonHelper.GetGDServiceBaseURL + ResultObj.FileRelativePath;
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return "";
            }
        }
        public class UploadResponseModel
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public int FileID { get; set; }

        }
        public class DownLoadResponseModel
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public string FileRelativePath { get; set; }
        }
    }
}