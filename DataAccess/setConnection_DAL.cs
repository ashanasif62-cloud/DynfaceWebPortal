using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class getConnection_DAL
    {
        private string getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SqlServices"]?.ToString() ?? string.Empty;
        }

        // Modified to be internal or helper to get a new opened connection
        private SqlConnection CreateOpenedConnection()
        {
            string connectionString = getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public SqlDataReader dataReader(string _query)
        {
            // Note: Returning DataReader requires the connection to remain open.
            // Caller is responsible for disposing the reader and connection.
            SqlConnection conn = CreateOpenedConnection();
            SqlCommand cmd = new SqlCommand(_query, conn);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int executeQueries(string _query)
        {
            using (SqlConnection conn = CreateOpenedConnection())
            using (SqlCommand cmd = new SqlCommand(_query, conn))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable executeWhereClauseProceduresWithParameters(string _procedure, string _whereClauseExpression, bool _companyWise = true)
        {
            string whereClauseExpression = _whereClauseExpression ?? string.Empty;
            if (_companyWise)
                whereClauseExpression = embedDataAreaId(whereClauseExpression);

            using (SqlConnection conn = CreateOpenedConnection())
            using (SqlCommand cmd = new SqlCommand(_procedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@WhereClause", SqlDbType.VarChar).Value = whereClauseExpression;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable retriveDataTable_Query(string _query, bool _companyWise = true)
        {
            string query = _query;
            if (_companyWise)
                query = embedDataAreaId(query);

            using (SqlConnection conn = CreateOpenedConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable retriveDataTable_Procedures(string _Procedure)
        {
            using (SqlConnection conn = CreateOpenedConnection())
            using (SqlCommand cmd = new SqlCommand(_Procedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public long executeProcedure(string _procedureName, List<object> _parmList, bool _scalar = false)
        {
            try
            {
                using (SqlConnection conn = CreateOpenedConnection())
                using (SqlCommand cmd = new SqlCommand(_procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddParameters(cmd, _parmList);

                    if (_scalar)
                    {
                        object result = cmd.ExecuteScalar();
                        long longResult = 0;
                        if (result != null && result != DBNull.Value)
                            Int64.TryParse(result.ToString(), out longResult);
                        return longResult;
                    }
                    else
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public int executeNonQueryProcedure(string _procedureName, List<object> _parmList)
        {
            try
            {
                using (SqlConnection conn = CreateOpenedConnection())
                using (SqlCommand cmd = new SqlCommand(_procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddParameters(cmd, _parmList);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public DataTable executeProcedureRetriveDataTable(string _procedureName, List<object> _parmList)
        {
            try
            {
                using (SqlConnection conn = CreateOpenedConnection())
                using (SqlCommand cmd = new SqlCommand(_procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddParameters(cmd, _parmList);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public DataTable executeProc(string _tableName, string _fieldList, string _whereClause, bool _companyWise = true)
        {
            string whereClause = _whereClause;
            if (_companyWise)
                whereClause = embedDataAreaId(whereClause);

            using (SqlConnection conn = CreateOpenedConnection())
            using (SqlCommand cmd = new SqlCommand("generateDataTableFromConfig", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = _tableName;
                cmd.Parameters.Add("@FieldList", SqlDbType.VarChar).Value = _fieldList;
                cmd.Parameters.Add("@WhereClause", SqlDbType.VarChar).Value = whereClause;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public string validateUsers(string _procedureName, List<object> _parmList)
        {
            try
            {
                using (SqlConnection conn = CreateOpenedConnection())
                using (SqlCommand cmd = new SqlCommand(_procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddParameters(cmd, _parmList);

                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return string.Empty;
            }
        }

        public DataTable getRoleConfigs(string _userId)
        {
            try
            {
                using (SqlConnection conn = CreateOpenedConnection())
                using (SqlCommand cmd = new SqlCommand("getRoleConfigs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", _userId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        private void AddParameters(SqlCommand cmd, List<object> _parmList)
        {
            if (_parmList == null) return;
            for (int j = 0; j < _parmList.Count; j++)
            {
                string parmName = _parmList[j]?.ToString() ?? string.Empty;
                j++;
                if (j < _parmList.Count)
                {
                    object parmValue = _parmList[j] ?? DBNull.Value;
                    if (!string.IsNullOrWhiteSpace(parmName))
                    {
                        cmd.Parameters.AddWithValue(parmName, parmValue);
                    }
                }
            }
        }

        private string embedDataAreaId(string _whereClauseExpression)
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            return " AND DataAreaId = '" + dataAreaId + "' " + (_whereClauseExpression ?? string.Empty);
        }

        private void LogError(Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            string currentMethodName = currentMethod?.DeclaringType?.FullName ?? "Unknown";
            objErrorLog.write(currentMethodName, ex);
        }

        // Legacy compatibility - these should be phased out
        public SqlConnection openConection() => CreateOpenedConnection();
        public void closeConnection() { /* No longer needed as we use 'using' blocks */ }
    }
}

