using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models;
using MedicalR.Models.MedicalR;
using Npgsql;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALEmployeFamilyDetails : IDALEmployeFamilyDetails
    {
        public ResponseModel ReadExcel(string filePath)
        {
            ResponseModel res = new ResponseModel();

            DataTable dataTable = CommonHelper.GetDataTableFromExcel(filePath);
            CommonHelper.write_log("dt count :" + dataTable.Rows.Count);
            try
            {
                res = AddingDataInDatabase(dataTable);
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = $"An error occurred: {ex.Message}";
                CommonHelper.write_log($"An error occurred: {ex.Message} \n Stack Trace: {ex.StackTrace}");
            }
            return res;
        }
        private string GetRelationID(string relation)
        {
            switch (relation)
            {
                case "Husband":
                    return "7";
                case "Wife":
                    return "7";
                case "Spouse":
                    return "7";
                case "Self":
                    return "0";
                case "Son":
                    return "11";
                case "Mother":
                    return "2";
                case "Daughter":
                    return "12";
                case "Father":
                    return "1";
                default:
                    return ""; // or handle default case accordingly
            }
        }
        private string GetRelationCode(string relation)
        {
            switch (relation)
            {
                case "7":
                    return "Spouse";
                case "0":
                    return "Self";
                case "11":
                    return "Son";
                case "2":
                    return "Mother";
                case "12":
                    return "Daughter";
                case "1":
                    return "Father";
                default:
                    return ""; // or handle default case accordingly
            }
        }
        public ResponseModel AddingDataInDatabase(DataTable FamilyDetailsList)
        {
            ResponseModel res = new ResponseModel();
            int counter = 0;


            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        foreach (DataRow dtRow in FamilyDetailsList.Rows)
                        {

                            string str_relationid = GetRelationID(dtRow["RELATION"].ToString());
                            string str_relationdec = GetRelationCode(str_relationid);

                            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM mdcl_tbl_upload_family_details(@p_employee_id, @p_first_name, @p_date_of_birth, @p_family_relation_id, @p_family_relation_code, @p_family_relation_desc)", connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@p_employee_id", dtRow["EMPLID"].ToString());
                                cmd.Parameters.AddWithValue("@p_first_name", dtRow["NAME"].ToString());
                                cmd.Parameters.AddWithValue("@p_date_of_birth",Convert.ToDateTime(dtRow["DOB"].ToString()).ToString("dd-MMM-yyyy"));
                                cmd.Parameters.AddWithValue("@p_family_relation_id", str_relationid);
                                cmd.Parameters.AddWithValue("@p_family_relation_code", str_relationdec);
                                cmd.Parameters.AddWithValue("@p_family_relation_desc", str_relationdec);
                                cmd.ExecuteNonQuery();
                                counter++;

                            }
                        }

                        transaction.Commit();
                        CommonHelper.write_log($"{counter} rows added successfully.");
                        res.Status = true;
                        res.Message = $"{counter} Records uploaded Successfully";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res.Status = false;
                        res.Message = ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                CommonHelper.write_log($"An error occurred: {ex.Message} \n Stack Trace: {ex.StackTrace}");
                res.Status = false;
                res.Message = $"An error occurred: {ex.Message}";

            }

            return res;
        }


        public List<UploadFamilyDetailsModel> GetEmployeeFamilyDetails()
        {
            try
            {
                List<UploadFamilyDetailsModel> familyDetails = new List<UploadFamilyDetailsModel>();

                using (NpgsqlConnection connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_family_details()", connection))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UploadFamilyDetailsModel detail = new UploadFamilyDetailsModel
                                {
                                    EMPID = reader["p_employeeid"].ToString(),
                                    Name = reader["p_first_name"].ToString(),
                                    DOB = Convert.ToDateTime(reader["p_date_of_birth"]),
                                    Relation = reader["p_family_relation_desc"].ToString(),
                                    // Assuming other properties
                                };
                                familyDetails.Add(detail);
                            }
                        }
                    }
                }

                return familyDetails;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                throw new Exception("Error occurred while retrieving data from the stored function.", ex);
            }
        }
    }
}