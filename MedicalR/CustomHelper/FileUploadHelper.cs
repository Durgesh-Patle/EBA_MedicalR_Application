using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class FileUploadHelper
    {
        public static List<UploaderResponseModel> UploadFile(HttpFileCollectionBase Files,string DirectoryName)
        {
            List<UploaderResponseModel> UploadedFiles = new List<UploaderResponseModel>();
            try
            {
                foreach (string File in Files)
                {
                    HttpPostedFileBase file = Files[File];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        var filePath = "~/Uploads/" + DirectoryName + "/" + fileName;
                        var directoryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/" + DirectoryName));
                        if(!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                       
                        var path = HttpContext.Current.Server.MapPath(filePath);
                        file.SaveAs(path);
                        UploaderResponseModel newUpload = new UploaderResponseModel();
                        newUpload.FilePath = filePath.Replace("~","");
                        newUpload.GoogleDrive_ID = GoogleDriveHelper.Upload(file, DirectoryName);
                        UploadedFiles.Add(newUpload);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return UploadedFiles;
        }
        public class UploaderResponseModel
        {
   
            public string FilePath { get; set; }
            public int GoogleDrive_ID { get; set; }

        }
    }
}