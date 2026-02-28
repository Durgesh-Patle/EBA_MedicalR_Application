
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class FileConverter
    {
        public static string FileToPdf(string filePath)
        {
            try
            {
                string fullpath = filePath;

                Document document = new Document();

                document.LoadFromFile(fullpath);

                string FolderPath = System.Web.HttpContext.Current.Server.MapPath("/GoogleDriveFiles/");

                string FileName = Guid.NewGuid().ToString();

                string FilePath = System.IO.Path.Combine(FolderPath, FileName)+ ".PDF";

                document.SaveToFile(FilePath, FileFormat.PDF);

                return FilePath;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return filePath;
            }
        }
    }
}