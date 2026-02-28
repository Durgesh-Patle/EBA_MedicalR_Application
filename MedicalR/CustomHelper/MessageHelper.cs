using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class MessageHelper
    {
        public static string ExceptionMessage
        {
            get
            {
                return "Something went wrong. Please try after some time.";
            }
        }
        public static string ErroeMsg
        {
            get
            {
                return "Something went wrong. Data Not Saved.";
            }
        }
        public static string RequestStatus
        {
            get
            {
                return "Data Saved Successfully !";
            }
        }
        public static string AlrExitsStatus
        {
            get
            {
                return "Emp Code Already Exit !";
            }
        }
        public static string ClaimStatus
        {
            get
            {
                return "Already you have claimed for this Year";
            }
        }
        public static string Verified
        {
            get
            {
                return "Checking Verified Successfully.";
            }
        }
        public static string Sanctioned
        {
            get
            {
                return "Claim Sanctioned Successfully";
            }
        }
        public static string Claim_Rejected
        {
            get
            {
                return "Claim Rejected Successfully";
            }
        }
        public static string CommonUpdateStatus
        {
            get
            {
                return "Data updated successfully";
            }
        }
        public static string AcknowledgeStatus
        {
            get
            {
                return "Acknowledge done Successfully !";
            }
        }
        public static string BatchCreationStatus
        {
            get
            {
                return @"Batch Created successfully !";
            }
        }
        #region role management
        public static string RoleAdded
        {
            get
            {
                return "Role Added Successfully.";
            }
        }
        public static string RoleUpdated
        {
            get
            {
                return "Role Updated Successfully.";
            }
        }
        public static string RoleStatusUpdated
        {
            get
            {
                return "Role Status Updated Successfully.";
            }
        }
        #endregion

        #region user management

        public static string UserRegister
        {
            get
            {
                return "User Register Successfully.";
            }
        }
        public static string UserAdded
        {
            get
            {
                return "User Added Successfully.";
            }
        }
        public static string UserUpdated
        {
            get
            {
                return "User Updated Successfully.";
            }
        }
        public static string HospitalUpdated
        {
            get
            {
                return "Hpspital Updated Successfully.";
            }
        }
        public static string Doctorupdate
        {
            get
            {
                return "Doctor Updated Successfully.";
            }
        }
        public static string ExpenseUpdated
        {
            get
            {
                return "Expense Updated Successfully.";
            }
        }
        public static string ObjectionUpdated
        {
            get
            {
                return "Objection Updated Successfully.";
            }
        }
        public static string HospitalStatus
        {
            get
            {
                return "Hpspital deleted Successfully.";
            }
        }
        public static string DoctorStatus
        {
            get
            {
                return "Doctor deleted Successfully.";
            }
        }
        public static string REMPADD
        {
            get
            {
                return "Retired Employee Added Successfully.";
            }
        }
        public static string Doctoradd
        {
            get
            {
                return "New Doctor Added Successfully.";
            }
        }
        public static string PendingLot
        {
            get
            {
                return "Lot no is pending for Approval";
            }
        }
        public static string REMPTRANS
        {
            get
            {
                return "Retired Employee Transaction Added Successfully.";
            }
        }
        public static string Approved
        {
            get
            {
                return "Lot No Approved Successfully.";
            }
        }
        public static string REMPAMTADD
        {
            get
            {
                return "Reimbursement Amount Added Successfully.";
            }
        }
        public static string REMPDEL
        {
            get
            {
                return "Retired Employee Deleted Successfully.";
            }
        }
        public static string REMPTRANSDEL
        {
            get
            {
                return "Retired Employee Transaction Deleted Successfully.";
            }
        }
        public static string RAMTDEL
        {
            get
            {
                return "Reimburse amount Deleted Successfully.";
            }
        }
        public static string ExpenseStatus
        {
            get
            {
                return "Expense Type deleted Successfully.";
            }
        }
        public static string ObjectionStatus
        {
            get
            {
                return "Objection deleted Successfully.";
            }
        }
        public static string UserStatusUpdated
        {
            get
            {
                return "User Status Updated Successfully.";
            }
        }

        public static string UserEmailExists
        {
            get
            {
                return "Email ID Already Registered.";
            }
        }
        //public static string ErroeMsg
        //{
        //    get
        //    {
        //        return "Please Check Input Data.";
        //    }
        //}
        public static string InvalidCredentials
        {
            get
            {
                return "Invalid Employee Code & Password.";
            }
        }
        public static string ConcurrentLoginMessage
        {
            get
            {
                return "You are currently logged in on another device or browser \n\n The browser has been closed forcefully. Please try again after some time.";
            }
        }
        public static string User_AD_AccountLocked
        {
            get
            {
                return "Your account has been locked.";
            }
        }
        public static string wrongotp
        {
            get
            {
                return "Invalid OTP. Please try again.";
            }
        }
        public static string EmailNotRegistered
        {
            get
            {
                return "Email ID Not Registered";
            }
        }
        public static string ResetLinkSent
        {
            get
            {
                return "Reset password link has been sent to your email id";
            }
        }
        #endregion

        #region common setting
        public static string CompanyDetailsUpdate
        {
            get
            {
                return "Company Details Updated Successfully.";
            }
        }

        public static string PasswordUpdated
        {
            get
            {
                return "Password Updated Successfully.";
            }
        }

        public static string CurrentPasswordNotValid
        {
            get
            {
                return "Current Password is Not Valid.";
            }
        }
        #endregion


        #region vendor management
        public static string VendorAdded
        {
            get
            {
                return "Vendor Added Successfully.";
            }
        }
        public static string VendorUpdated
        {
            get
            {
                return "Vendor Updated Successfully.";
            }
        }

        #endregion

        #region Email Configuration
        public static string EmailConfigurationAdded
        {
            get
            {
                return "Email Configuration Added Successfully.";
            }
        }
        public static string EmailConfigurationUpdated
        {
            get
            {
                return "Email Configuration Updated Successfully.";
            }
        }

        public static string EmailConfigurationStatusUpdated
        {
            get
            {
                return "Email Configuration Status Updated Successfully.";
            }
        }

        public static string EmailConfigurationExists
        {
            get
            {
                return "Email Configuration for user already exists.";
            }
        }

        public static string EmailConfigurationTestFail
        {
            get
            {
                return "Email Test failed. Please verify your configuartion.";
            }
        }

        #endregion

        #region client
        public static string ClientAdded
        {
            get
            {
                return "Client Added Successfully.";
            }
        }
        public static string ClientUpdated
        {
            get
            {
                return "Client Updated Successfully.";
            }
        }

        #endregion

        #region client poc
        public static string ClientPOCAdded
        {
            get
            {
                return "Client POC Added Successfully.";
            }
        }
        public static string ClientPOCUpdated
        {
            get
            {
                return "Client POC Updated Successfully.";
            }
        }

        #endregion

        #region candidates
        public static string CandidateAdded
        {
            get
            {
                return "Candidate Added Successfully.";
            }
        }
        public static string CandidateUpdated
        {
            get
            {
                return "Candidate Updated Successfully.";
            }
        }

        #endregion

        #region job opening
        public static string JobOpeningAdded
        {
            get
            {
                return "Job Opening Added Successfully.";
            }
        }
        public static string JobOpeningUpdated
        {
            get
            {
                return "Job Opening Updated Successfully.";
            }
        }

        #endregion

        #region interview
        public static string InterviewAdded
        {
            get
            {
                return "Interview Added Successfully.";
            }
        }
        public static string InterviewUpdated
        {
            get
            {
                return "Interview Updated Successfully.";
            }
        }
        public static string InterviewFeedbackUpdated
        {
            get
            {
                return "Interview Feedback Updated Successfully.";
            }
        }

        public static string InterviewAlreadyExists
        {
            get
            {
                return "Interview Already Exists.";
            }
        }

        #endregion

        #region attachment
        public static string AttachmentAdded
        {
            get
            {
                return "Attachment Added Successfully.";
            }
        }

        public static string AttachmentDeleted
        {
            get
            {
                return "Attachment Deleted Successfully.";
            }
        }

        #endregion

        #region notes
        public static string NoteAdded
        {
            get
            {
                return "Note Added Successfully.";
            }
        }

        public static string NoteUpdated
        {
            get
            {
                return "Note Updated Successfully.";
            }
        }

        public static string NoteDeleted
        {
            get
            {
                return "Note Deleted Successfully.";
            }
        }

        #endregion

        #region email
        public static string EmailSent
        {
            get
            {
                return "Email Sent Successfully.";
            }
        }

        public static string EmailConfiguartionNotFound
        {
            get
            {
                return "Email Configuartion Not Found.";
            }
        }

        #endregion

        #region template module
        public static string TemplateAdded
        {
            get
            {
                return "Template Added Successfully.";
            }
        }
        public static string TemplateUdated
        {
            get
            {
                return "Template Updated Successfully.";
            }
        }

        #endregion

        #region Skill
        public static string SkillAdded
        {
            get
            {
                return "Skill Added Successfully.";
            }
        }
        public static string SkillUpdated
        {
            get
            {
                return "Skill Updated Successfully.";
            }
        }

        #endregion
    }
}