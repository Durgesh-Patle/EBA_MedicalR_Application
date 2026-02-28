using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalR.Models.MedicalR
{
    public class JsonNetResult : JsonResult
    {
        private JsonSerializerSettings SerializerSetttins { get; set; }
        private Formatting Formatting { get; set; }

        private string MimeType = "application/Json";

        public JsonNetResult(object data)
        {
            this.Data = data;
            SerializerSetttins = new JsonSerializerSettings();
            SerializerSetttins.Formatting = Newtonsoft.Json.Formatting.None;
            SerializerSetttins.NullValueHandling = NullValueHandling.Ignore;
            SerializerSetttins.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = MimeType;
            JsonSerializer serializer = JsonSerializer.Create(SerializerSetttins);
            using (var writer = new JsonTextWriter(context.HttpContext.Response.Output) { Formatting = Formatting })
                JsonSerializer.Create(SerializerSetttins).Serialize(writer, Data);
        }
    }
}