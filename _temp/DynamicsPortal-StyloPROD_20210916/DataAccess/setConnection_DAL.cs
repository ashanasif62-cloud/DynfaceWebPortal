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
        private SqlConnection conn;
        public string getConnectionString()
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
            //connectionString = "Data Source=ServerName; Initial Catalog = DatabaseName; User ID=styloshoes.local\\ess.admin; Password = Server@159951";
            //ConnectionString = "Data Source=IPKDCSRVEBS;Initial Catalog=DynamicsPortalStylo;Integrated Security=True;";
            //MCS-LEN-44\\MSSQLSERVER2  //.STYLOSHOES\\IPKDCSRVEBS  //STYLOSHOES\\ess.admin  -- styloshoes.local\ess.admin Server@159951
            //MCS (local)
            return connectionString;
        }
        public SqlConnection openConection()
        {
            string ConnectionString;
            ConnectionString = getConnectionString();

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
        public SqlDataReader dataReader(string _query)
        {
            openConection();

            SqlCommand cmd = new SqlCommand(_query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            closeConnection();

            return dr;
        }
        public void closeConnection()
        {
            conn.Close();
        }
        public int executeQueries(string _query)
        {
            int iRowseffected;

            openConection();

            SqlCommand cmd = new SqlCommand(_query, conn);
            iRowseffected = cmd.ExecuteNonQuery();

            closeConnection();

            return iRowseffected;
        }
        public DataTable executeWhereClauseProceduresWithParameters(string _procedure, string _whereClauseExpression, bool _companyWise = true)
        {
            openConection();
            string whereClauseExpression = _whereClauseExpression;
            string procedure = _procedure;

            SqlCommand cmd = new SqlCommand(procedure, conn);
            SqlDataAdapter dr = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(whereClauseExpression))
                whereClauseExpression = string.Empty;

            if (_companyWise)
                whereClauseExpression = embedDataAreaId(whereClauseExpression);

            cmd.Parameters.Add("@WhereClause", SqlDbType.VarChar).Value = whereClauseExpression;
            //cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = txtLastName.Text;

            dr.SelectCommand = cmd;
            DataSet ds = new DataSet();
            DataTable dt;

            dr.Fill(ds);
            dt = ds.Tables[0];
            closeConnection();

            return dt;

        }
        public DataTable retriveDataTable_Query(string _query, bool _companyWise = true)
        {
            openConection();
            string query = _query;

            if (_companyWise)
                query = embedDataAreaId(query);

            SqlDataAdapter dr = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            DataTable dt;

            dr.Fill(ds);
            dt = ds.Tables[0];

            closeConnection();

            return dt;
        }
        public DataTable retriveDataTable_Procedures(string _Procedure)
        {
            openConection();
            SqlCommand cmd = new SqlCommand(_Procedure, conn);
            SqlDataAdapter dr = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            dr.SelectCommand = cmd;
            DataSet ds = new DataSet();
            DataTable dt;

            dr.Fill(ds);
            dt = ds.Tables[0];
            closeConnection();

            return dt;
        }
        public long executeProcedure(string _procedureName, List<object> _parmList, bool _scalar = false)
        {
            try
            {
                int Length = _parmList.Count;
                object[] parmList = _parmList.ToArray();
                long result = 0;
                bool scalar = _scalar;
                openConection();
                SqlCommand cmd = new SqlCommand(_procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                string parmName1, parmValue1;

                for (int j = 0; j < Length; j++)
                {
                    parmName1 = parmList[j] == null ? string.Empty : parmList[j].ToString();
                    j = ++j;
                    parmValue1 = parmList[j] == null ? string.Empty : parmList[j].ToString();

                    if (!String.IsNullOrWhiteSpace(parmName1) && !String.IsNullOrWhiteSpace(parmValue1))
                        cmd.Parameters.AddWithValue(parmName1, parmValue1);
                }
                if (scalar)
                {
                    Int64.TryParse((cmd.ExecuteScalar()).ToString(), out result);
                }
                else
                {
                    result = cmd.ExecuteNonQuery();
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public int executeNonQueryProcedure(string _procedureName, List<object> _parmList)
        {
            try
            {
                int Length = _parmList.Count;
                object[] parmList = _parmList.ToArray();
                int result = 0;
                openConection();
                SqlCommand cmd = new SqlCommand(_procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                string parmName1, parmValue1;

                for (int j = 0; j < Length; j++)
                {
                    parmName1 = parmList[j] == null ? string.Empty : parmList[j].ToString();
                    j = ++j;
                    parmValue1 = parmList[j] == null ? string.Empty : parmList[j].ToString();

                    if (!String.IsNullOrWhiteSpace(parmName1) && !String.IsNullOrWhiteSpace(parmValue1))
                        cmd.Parameters.AddWithValue(parmName1, parmValue1);
                }

                result = cmd.ExecuteNonQuery();

                return result;

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public DataTable executeProcedureRetriveDataTable(string _procedureName, List<object> _parmList)
        {
            try
            {
                int Length = _parmList.Count;
                object[] parmList = _parmList.ToArray();

                openConection();
                SqlCommand cmd = new SqlCommand(_procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                string parmName1, parmValue1;

                for (int j = 0; j < Length; j++)
                {
                    parmName1 = parmList[j] == null ? string.Empty : parmList[j].ToString();
                    j = ++j;
                    parmValue1 = parmList[j] == null ? string.Empty : parmList[j].ToString();

                    if (!String.IsNullOrWhiteSpace(parmName1) && !String.IsNullOrWhiteSpace(parmValue1))
                        cmd.Parameters.AddWithValue(parmName1, parmValue1);
                }

                SqlDataAdapter dr = new SqlDataAdapter();
                dr.SelectCommand = cmd;
                DataSet ds = new DataSet();
                DataTable dt;

                dr.Fill(ds);
                dt = ds.Tables[0];
                closeConnection();

                return dt;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public DataTable executeProc(string _tableName, string _fieldList, string _whereClause, bool _companyWise = true)
        {
            openConection();
            string whereClause = _whereClause;

            if (_companyWise)
                whereClause = embedDataAreaId(whereClause);

            SqlCommand cmd = new SqlCommand("generateDataTableFromConfig", conn);
            SqlDataAdapter dr = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = _tableName;
            cmd.Parameters.Add("@FieldList", SqlDbType.VarChar).Value = _fieldList;
            cmd.Parameters.Add("@WhereClause", SqlDbType.VarChar).Value = whereClause;
            dr.SelectCommand = cmd;
            DataSet ds = new DataSet();
            DataTable dt;

            dr.Fill(ds);
            dt = ds.Tables[0];

            ds.Tables.Remove(ds.Tables[0]);

            closeConnection();

            return dt;

        }
        public string validateUsers(string _procedureName, List<object> _parmList)
        {
            try
            {
                int Length = _parmList.Count;
                object[] parmList = _parmList.ToArray();
                string result;

                openConection();
                SqlCommand cmd = new SqlCommand(_procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                string parmName1, parmValue1;

                for (int j = 0; j < Length; j++)
                {
                    parmName1 = parmList[j] == null ? string.Empty : parmList[j].ToString();
                    j = ++j;
                    parmValue1 = parmList[j] == null ? string.Empty : parmList[j].ToString();

                    if (!String.IsNullOrWhiteSpace(parmName1) && !String.IsNullOrWhiteSpace(parmValue1))
                        cmd.Parameters.AddWithValue(parmName1, parmValue1);
                }

                result = cmd.ExecuteScalar().ToString();

                return result;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return string.Empty;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public DataTable getRoleConfigs(string _userId)
        {
            try
            {
                openConection();
                SqlCommand cmd = new SqlCommand("getRoleConfigs", conn);
                SqlDataAdapter dr = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = _userId;
                cmd.Parameters.AddWithValue("@UserId", _userId);
                dr.SelectCommand = cmd;
                DataSet ds = new DataSet();
                DataTable dt;

                dr.Fill(ds);
                dt = ds.Tables[0];

                ds.Tables.Remove(ds.Tables[0]);

                closeConnection();

                return dt;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private string embedDataAreaId(string _whereClauseExpression)
        {
            string whereClauseExpression = _whereClauseExpression;

            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            whereClauseExpression = " AND DataAreaId = '" + dataAreaId + "' " + whereClauseExpression;

            return whereClauseExpression;
        }

    }
}
