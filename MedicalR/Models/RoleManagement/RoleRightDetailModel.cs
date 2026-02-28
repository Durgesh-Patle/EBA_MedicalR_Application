using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RoleManagement
{
    public class RoleRightDetailModel
    {
        public RoleRightDetailModel()
        {
            Clients= new ClientsRoleModel();
            ClientPoc = new ClientPOCRoleModel();
            Candidates = new CandidatesRoleModel();
            JobOpenings = new JobOpeningRoleModel();
            Interviews = new InterviewsRoleModel();
            Reports = new ReportsRoleModel();
            Settings = new SettingsRoleModel();
        }
        public ClientsRoleModel Clients { get; set; }
        public ClientPOCRoleModel ClientPoc { get; set; }
        public CandidatesRoleModel Candidates { get; set; }
        public JobOpeningRoleModel JobOpenings { get; set; }
        public InterviewsRoleModel Interviews { get; set; }
        public ReportsRoleModel Reports { get; set; }
        public SettingsRoleModel Settings { get; set; }

    }
    public class ClientsRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class ClientPOCRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class CandidatesRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class JobOpeningRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class InterviewsRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class ReportsRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }

    public class SettingsRoleModel
    {
        public bool FullAccess { get; set; }
        public bool AssignedRecords { get; set; }

    }
}