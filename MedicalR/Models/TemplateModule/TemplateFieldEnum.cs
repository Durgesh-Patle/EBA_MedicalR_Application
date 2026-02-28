using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.TemplateModule
{
    public class TemplateFieldEnum
    {
        public enum TemplateField
        {
           InterviewStage,
           InterviewPanel,
           InterviewType,
           InterviewMode,
           InterviewOwner,
           InterviewOwnerMobile,
           InterviewOwnerEmail,
           InterviewDate,
           InterviewStatus,
           InterviewFeedbackLink,
           InterviewLocation,
           CandidateName,
           CandidateMobile,
           CandidateEmail,
           CandidateSkillSet,
           CandidateExperience,
           CandidateCurrentSalary,
           CandidateExpectedSalary,
           CandidateNoticePeriod,
           CandidateAdditionalInfo,
           ResumeLink,
           ClientName ,
           ClientPocName,
           JobDesignation,
           JobSalary,
           JobType,
           RequiredExperience,
           JobLocation,
           UserName,
           UserEmail,

        }

        public class TemplateFieldList
        {
            public string InterviewStage { get; set; }
            public string InterviewPanel { get; set; }
            public string InterviewType { get; set; }
            public string InterviewMode { get; set; }
            public string InterviewOwner { get; set; }
            public string InterviewOwnerMobile { get; set; }
            public string InterviewOwnerEmail { get; set; }
            public string InterviewDate { get; set; }
            public string InterviewStatus { get; set; }
            public string InterviewFeedbackLink { get; set; }
            public string InterviewLocation { get; set; }
            public string CandidateName { get; set; }
            public string CandidateMobile { get; set; }
            public string CandidateEmail { get; set; }
            public string CandidateSkillSet { get; set; }
            public string CandidateExperience { get; set; }
            public string CandidateCurrentSalary { get; set; }
            public string CandidateExpectedSalary { get; set; }
            public string CandidateNoticePeriod { get; set; }
            public string CandidateAdditionalInfo { get; set; }
            public string ClientName { get; set; }
            public string ClientPocName { get; set; }
            public string JobDesignation { get; set; }
            public string JobSalary { get; set; }
            public string JobType { get; set; }
            public string RequiredExperience { get; set; }
            public string JobLocation { get; set; }
            public string ResumeLink { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
        }
    }
}