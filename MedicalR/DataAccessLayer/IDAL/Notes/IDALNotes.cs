using MedicalR.Models;
using MedicalR.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.Notes
{
    public interface IDALNotes
    {
        List<NotesModel> GetNotesList(Int32 PageID, Int32 ReleventID);
        ResponseModel AddNotes(NotesModel objModel);
        ResponseModel UpdateNotes(NotesModel objModel);
        ResponseModel DeleteNote(Int32 NoteID, Int32 PageID);
    }
}