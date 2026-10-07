using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserRecentMenuItems_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();

        public long UserRecentMenuItems_Create(SysUserRecentMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@VisitDateTime");
                parmList.Add(newBussinessObj.VisitDateTime);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@MODIFIEDBY");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@CREATEDBY");
                parmList.Add(newBussinessObj.CreatedBy);
                parmList.Add("@RECVERSION");
                parmList.Add(newBussinessObj.RecVersion);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                parmList.Add("@RecId");
                parmList.Add("");

                return setConnection.executeProcedure("SysUserRecentMenuItems_Create", parmList, true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long UserRecentMenuItems_Update(SysUserRecentMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@VisitDateTime");
                parmList.Add(newBussinessObj.VisitDateTime);
                parmList.Add("@MODIFIEDBY");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);

                return setConnection.executeProcedure("SysUserRecentMenuItems_Update", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long UserRecentMenuItems_Delete(SysUserRecentMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);

                return setConnection.executeProcedure("SysUserRecentMenuItems_Delete", parmList);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public DataTable SysUserRecentMenuItems_Retrieve(SysUserRecentMenuItems_BOL newBussinessObj)
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

                return setConnection.executeProcedureRetriveDataTable("SysUserRecentMenuItems_Retrieve", parmList);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public long UserRecentMenuItems_CreateOrUpdate(SysUserRecentMenuItems_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@MenuItemId");
                parmList.Add(newBussinessObj.MenuItemId);
                parmList.Add("@ModifiedDateTime");
                parmList.Add(newBussinessObj.ModifiedDateTime);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@CreatedDateTime");
                parmList.Add(newBussinessObj.CreatedDateTime);
                parmList.Add("@VisitDateTime");
                parmList.Add(newBussinessObj.VisitDateTime);

                parmList.Add("@CreatedBy");
                parmList.Add(newBussinessObj.CreatedBy);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@RecVersion");
                parmList.Add(newBussinessObj.RecVersion);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);


                return setConnection.executeProcedure("SysUserRecentMenuItems_CreateOrUpdate", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
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

