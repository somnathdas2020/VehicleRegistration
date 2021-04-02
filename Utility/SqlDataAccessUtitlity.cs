using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VehicleRegistration.Utility
{
    public static class SqlDataAccessUtitlity
    {
        private static readonly string _connString;
        static SqlDataAccessUtitlity()
        {
            _connString = WebConfigurationManager.ConnectionStrings[WebConfigurationManager.AppSettings["ConnString"]].ConnectionString;
        }

        public static string ConnectionString { get { return _connString; } }

        public static SqlDataReader ExecuteProcedure(string procedureName, SqlParameter[] dbParams)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            if (dbParams != null && dbParams.Length > 0)
                cmd.Parameters.AddRange(dbParams);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                if (reader == null && (conn != null && conn.State == ConnectionState.Open))
                    conn.Close();
            }
            return reader;
        }
        public static SqlDataReader ExecuteSingleResultProcedure(string procedureName, SqlParameter[] dbParams)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            if (dbParams != null && dbParams.Length > 0)
                cmd.Parameters.AddRange(dbParams);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult);
            }
            catch (Exception ex)
            {
                if (reader == null && (conn != null && conn.State == ConnectionState.Open))
                    conn.Close();
            }
            return reader;
        }
        public static SqlDataReader ExecuteSingleRowProcedure(string procedureName, SqlParameter[] dbParams)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (dbParams != null && dbParams.Length > 0)
                cmd.Parameters.AddRange(dbParams);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch
            {
                if (reader == null && (conn != null && conn.State == ConnectionState.Open))
                    conn.Close();
            }
            return reader;
        }
        public static SqlDataReader ExecuteSingleRowProcedure(string procedureName, SqlParameter[] dbParams, string con)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (dbParams != null && dbParams.Length > 0)
                cmd.Parameters.AddRange(dbParams);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch
            {
                if (reader == null && (conn != null && conn.State == ConnectionState.Open))
                    conn.Close();
            }
            return reader;
        }
        public static DataTable ExecuteDataTableProcedure(string procedureName, SqlParameter[] dbParams)

        {
            DataTable dtResult = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                    if (dbParams != null && dbParams.Length > 0)
                        cmd.Parameters.AddRange(dbParams);
                    dtResult = new DataTable();
                    conn.Open();
                    using (IDataReader idr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        dtResult.Load(idr);
                    }
                }
            }
            return dtResult;
        }
        public static DataSet ExecuteDataSetProcedure(string procedureName, SqlParameter[] dbParams)
        {
            DataSet dsResult = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(procedureName, _connString))
            {
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand.CommandTimeout = 120;
                if (dbParams != null && dbParams.Length > 0)
                    sda.SelectCommand.Parameters.AddRange(dbParams);
                sda.Fill(dsResult);
            }
            return dsResult;
        }
        public static void ExecuteNonResultProcedure(string procedureName, SqlParameter[] dbParams)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dbParams != null && dbParams.Length > 0)
                        cmd.Parameters.AddRange(dbParams);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void ExecuteNonResultCommand(string commandText, SqlParameter[] dbParams)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    if (dbParams != null && dbParams.Length > 0)
                        cmd.Parameters.AddRange(dbParams);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static SqlDataReader ExecuteSingleResultCommand(string commandText, SqlParameter[] dbParams)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(commandText, conn);
            if (dbParams != null && dbParams.Length > 0)
                cmd.Parameters.AddRange(dbParams);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult);
            }
            catch
            {
                if (reader == null && (conn != null && conn.State == ConnectionState.Open))
                    conn.Close();
            }
            return reader;
        }
        //Amit
        public static object ExecuteScalerResultProcedure(string procedureName, SqlParameter[] dbParams)
        {
            object obj = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dbParams != null && dbParams.Length > 0)
                        cmd.Parameters.AddRange(dbParams);
                    conn.Open();
                    obj = cmd.ExecuteScalar();
                }
            }
            return obj;
        }
        public static object ExecuteScalerResultCommand(string procedureName, SqlParameter[] dbParams)
        {
            object obj = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (dbParams != null && dbParams.Length > 0)
                        cmd.Parameters.AddRange(dbParams);
                    conn.Open();
                    obj = cmd.ExecuteScalar();
                }
            }
            return obj;
        }

    }
}