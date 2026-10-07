using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysMenuItems_BLL
    {
        
        public DataTable SysMenuItems_Retrieve()
        {
            try
            {
                DataTable dt = new DataTable();
                SysMenuItems_DAL objDAL = new SysMenuItems_DAL();
                dt = objDAL.SysMenuItems_Retrieve();
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
            { }
        }
        
    }
}
