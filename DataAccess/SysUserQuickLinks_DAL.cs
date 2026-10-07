using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserQuickLinks_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();

        public DataTable SysUserQuickLinks_Retrieve(SysUserQuickLink_BOL newBussinessObj)
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

                return setConnection.executeProcedureRetriveDataTable("SysUserQuickLinks_Retrieve", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public long UserQuickLink_Create(SysUserQuickLink_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@CreatedBy");
                parmList.Add(newBussinessObj.CreatedBy);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedure("SysUserQuickLink_Create", parmList, true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long UserQuickLink_Delete(SysUserQuickLink_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);

                return setConnection.executeProcedure("SysUserQuickLink_Delete", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public DataTable checkAlreadyAddedInQuickLink(SysRolesMenuItem_BOL newBussinessObj)
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
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);

                return setConnection.executeProcedureRetriveDataTable("checkAlreadyAddedInQuickLink", parmList);
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

