using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysMenuItems_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();
        
        public DataTable SysMenuItems_Retrieve()
        {
            try
            {
                List<object> parmList = new List<object>();
                return setConnection.executeProcedureRetriveDataTable("SysMenuItems_Retrieve", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        private void LogError(Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            string currentMethodName = currentMethod?.DeclaringType?.FullName ?? "Unknown";
            objErrorLog.write(currentMethodName, ex);
        }

    }
}

