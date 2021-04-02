using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VehicleRegistration.Abstract;
using VehicleRegistration.Models;
using VehicleRegistration.Utility;

namespace VehicleRegistration.Concrete
{
    public class VehicleRegistration : IVehicleRegistration
    {
        public VehicleRegistrationList GetAllVehicleRegistration()
        {
            VehicleRegistrationList model = new VehicleRegistrationList();

            SqlDataReader dr = null;
            try
            {
                dr = SqlDataAccessUtitlity.ExecuteProcedure("[getVehicleRegistrationDetails]", null);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        model._RegistrationList.Add(new VehicleRegistrationViewModel
                        {
                            VehicleRegistrationID = Convert.ToInt32(dr["RegistrationID"]),
                            CustomerName = dr["CustomerName"].ToString(),
                            VehicleName = dr["VehicleName"].ToString(),
                            RegistrationAmount = Convert.ToDecimal(dr["RegistrationAmount"]),
                            RegistrationYear = Convert.ToInt32(dr["RegistrationYear"]),
                            AllAmountPaid = Convert.ToBoolean(Convert.ToInt32(dr["AllRegAmountPaid"])),
                            PaidAmount = Convert.ToDecimal(dr["PaidAmount"].ToString()),
                            DueAmount = Convert.ToDecimal(dr["DueAmount"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }
        public VehicleRegistrationViewModel GetVehicleRegistrationDetail(int regID)
        {
            VehicleRegistrationViewModel model = new VehicleRegistrationViewModel();

            SqlDataReader dr = null;
            try
            {
                dr = SqlDataAccessUtitlity.ExecuteProcedure("[getVehicleRegistrationDetail]", 
                    new SqlParameter[] { new SqlParameter("@RegistrationID", SqlDbType.Int, 4) { Value = regID } });
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        model.VehicleRegistrationID = regID;
                        model.CustomerName = dr["CustomerName"].ToString();
                        model.RegistrationAmount = Convert.ToDecimal(dr["RegistrationAmount"]);
                        model.AllAmountPaid = Convert.ToBoolean(Convert.ToInt32(dr["AllRegAmountPaid"]));
                        model.PaidAmount = Convert.ToDecimal(dr["PaidAmount"].ToString());
                        model.DueAmount = Convert.ToDecimal(dr["DueAmount"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }
        public bool PayRegistrationAmount(int regID, decimal paymentAmount)
        {
            try
            {
                object obj = SqlDataAccessUtitlity.ExecuteScalerResultProcedure("[payRegistrationAmount]",
                    new SqlParameter[] { 
                        new SqlParameter("@RegistrationID", SqlDbType.Int, 4) { Value = regID },
                        new SqlParameter("@PaymentAmount", SqlDbType.Decimal) { Value = paymentAmount }
                    });
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToBoolean(obj);
                }
                
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}