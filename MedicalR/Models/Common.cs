using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using MedicalR.Models;
using MedicalR.Models.UserManagement;
using MedicalR.CustomHelper;

namespace MedicalR.Models
{
    public class Common
    {
        List<DDLMODEL> ListModel = new List<DDLMODEL>();
        public List<DDLMODEL> GetTreatmentTypes()
        {
            List<DDLMODEL> Final_ListModel = new List<DDLMODEL>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pcategory_code", "#TRTTYPE#");
            parameters.Add("pid", UserManager.User.UserID);
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_userwise_treatment_type(:pcategory_code,:pid)", parameters);
            if (UserManager.User.is_maternity)
            {
                Final_ListModel = ListModel.Where(x => x.code == "#MATERNITY#").ToList();
            }
            else
            {
                Final_ListModel = ListModel;
            }

            return Final_ListModel;
        }
        public List<DDLMODEL> LoadDropdown(string pCategory_Code)
        {
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pcategory_code", pCategory_Code);
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_loaddropdown_lov(:pcategory_code)", parameters);
            return ListModel;
        }
        public List<DDLMODEL> GetPendingBatches_ForEditChecking()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_pending_batch_list_for_editchecking()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetPendingBatches_ForSanctionMedicals()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_pending_batch_list_for_sanctionmedicals()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetBandNames()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_band()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetQMBatches()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_querymodule_batches()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetQMLotNos()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_querymodule_lot_nos()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetDoctors()
        {
            DynamicParameters parameters = new DynamicParameters();
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_doctor_dropdown_list()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetObjectionCodes()
        {
            DynamicParameters parameters = new DynamicParameters();
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_objectioncodes_list()", null);
            return ListModel;
        }
        public List<DDLMODEL> GetPatients(int empl_id)
        {
            List<DDLMODEL> modify_ListModel = new List<DDLMODEL>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pid", empl_id);
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_family_details(:pid)", parameters);
            if (UserManager.User.is_maternity)
            {
                if (UserManager.User.gender == "Male")
                {
                    modify_ListModel = ListModel.Where(x => x.code == "Spouse").ToList();
                }
                else
                {
                    modify_ListModel = ListModel.Where(x => x.code == "Self").ToList();
                }
            }
            else
            {
                modify_ListModel = ListModel;
            }
            return modify_ListModel;
        }
        public List<DDLMODEL> GetCFHospitals()
        {
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_ex_ddl_credit_facilities_hospitals()", null);
            return ListModel;
        }


        public List<DDLMODEL> GetExpenseTypes(int treatment_type_id)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("plov_id", treatment_type_id);
            parameters.Add("p_employeeid", UserManager.User.UserID);
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_dropdown_sblov_data(:plov_id,:p_employeeid)", parameters);
            return ListModel;
        }


        public List<DDLMODEL> GetHeadOfExpenses(int exepense_type_id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("psub_lov_id", exepense_type_id);
            parameters.Add("p_employeeid", UserManager.User.UserID);
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_dropdown_headofexpenses_data(:psub_lov_id,:p_employeeid)", parameters);
            return ListModel;
        }
        public List<DDLMODEL> GetHospitalList()
        {
            DynamicParameters parameters = new DynamicParameters();
            ListModel = Helper.LOAD_DROPDOWN_DATA("select * from mdcl_sp_get_hospital_list()", null);
            return ListModel;
        }
        public SideBarinfoModel GeEmployeeSideBarDetails()
        {
            SideBarinfoModel sbm = new SideBarinfoModel();
            sbm = UserManager.SideBarInfor;
            return sbm;
        }
    }
}