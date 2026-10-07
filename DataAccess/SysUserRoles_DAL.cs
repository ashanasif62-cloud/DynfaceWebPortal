using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserRoles_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();

        public DataTable SysUserRoles_RetrieveByRoleId(SysUserRole_BOL newBussinessObj)
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

                return setConnection.executeProcedureRetriveDataTable("SysUserRoles_RetrieveByRoleId", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public DataTable SysUserRoles_RetrieveByUserId(SysUserRole_BOL newBussinessObj)
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

                return setConnection.executeProcedureRetriveDataTable("SysUserRoles_Retrieve", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public DataTable SysUserRoles_RetrieveAll(SysUserRole_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysUserRoles_RetrieveAll", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public long sysUserRoles_Create(SysUserRole_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
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
                return setConnection.executeProcedure("SysUserRoles_Create", parmList, true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long sysUserRoles_Update(SysUserRole_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                return setConnection.executeProcedure("SysUserRoles_Update", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        public long sysUserRoles_Delete(SysUserRole_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);

                return setConnection.executeProcedure("SysUserRoles_Delete", parmList);
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

