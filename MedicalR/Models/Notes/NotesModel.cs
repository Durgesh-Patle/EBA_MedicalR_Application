using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Notes
{
    public class NotesModel
    {
        public int NoteID { get; set; }
        public int PageID { get; set; }
        public int RelevantID { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string CreatedDate { get; set; }
    }
}