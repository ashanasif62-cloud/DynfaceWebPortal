using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserMenuItems_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();
        private List<object> parmList = new List<object>();
        
        public DataTable SysUserMenuItems_Retrieve(SysUserMenuItems_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysUserMenuItems_Retrieve", parmList);
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
        public DataTable validateUserMenuItems(SysUserMenuItems_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("validateUserRoleMenuItems", parmList);
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
