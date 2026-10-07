using GeneralAuxiliary;
using System;
using System.Data;

namespace DataAccess
{
    public class generateDataTable
    {
        private DataTable dt;

        public generateDataTable(string tableName, string fieldsList, string whereClause, bool companyWise = true)
        {
            try
            {

                getConnection_DAL objDA = new getConnection_DAL();
                dt = objDA.executeProc(tableName, fieldsList, whereClause, companyWise);

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }
        public DataTable dataTable()
        {

            return dt;
        }
    }
    
    //public class generateDataTableFromConfig
    //{
    //    public DataTable getDataTable()
    //    {
    //        DataTable dt;

    //        getConnection_DAL objDA = new getConnection_DAL();

    //        dt = objDA.executeProc(tableName, fieldList, whereClause);

    //        return dt;
    //    }


    //    public string tableName { get; set; }

    //    public string fieldList { get; set; }

    //    public string whereClause { get; set; }
    //}
    
}
