using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysRolesMenuItems_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();

        public long sysRolesMenuItems_Create(SysRolesMenuItem_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@AccessLevel");
                parmList.Add(newBussinessObj.AccessLevel);
                parmList.Add("@LicenseType");
                parmList.Add(newBussinessObj.LicenseType);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@RecVersion");
                parmList.Add(newBussinessObj.RecVersion);
                parmList.Add("@CreatedBy");
                parmList.Add(newBussinessObj.CreatedBy);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);
                return setConnection.executeProcedure("SysRoleMenuItems_Create", parmList, true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long sysRolesMenuItems_Update(SysRolesMenuItem_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                return setConnection.executeProcedure("SysRoleMenuItems_Update", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long sysRolesMenuItems_Delete(SysRolesMenuItem_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);

                return setConnection.executeProcedure("SysRoleMenuItems_Delete", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public DataTable sysRoleMenuItems_RetrieveByRoleId(SysRolesMenuItem_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysRoleMenuItems_RetrieveByRoleId", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public DataTable sysRoleMenuItems_Retrieve(SysRolesMenuItem_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysRoleMenuItems_Retrieve", parmList);
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