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
        
        public DataTable SysUserMenuItems_Retrieve(SysUserMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
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
                LogError(ex);
                return null;
            }
        }
        public DataTable validateUserMenuItems(SysUserMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
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

