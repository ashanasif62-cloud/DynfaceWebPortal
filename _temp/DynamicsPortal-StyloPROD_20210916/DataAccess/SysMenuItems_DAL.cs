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
        private List<object> parmList = new List<object>();
        
        public DataTable SysMenuItems_Retrieve()
        {
            try
            {
                return setConnection.executeProcedureRetriveDataTable("SysMenuItems_Retrieve", parmList);
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
            { }
        }

    }
}
