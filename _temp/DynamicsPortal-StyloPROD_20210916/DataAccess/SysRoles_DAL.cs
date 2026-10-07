using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysRoles_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();
        private List<object> parmList = new List<object>();

        public long sysRoles_Create(SysRoles_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@RoleId");
                parmList.Add(newBussinessObj.RoleId);
                parmList.Add("@Description");
                parmList.Add(newBussinessObj.Description);
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
                return setConnection.executeProcedure("SysRoles_Create", parmList, true);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
            finally
            { }
        }

        public long sysRoles_Update(SysRoles_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);
                parmList.Add("@Description");
                parmList.Add(newBussinessObj.Description);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                return setConnection.executeProcedure("SysRoles_Update", parmList);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
            finally
            { }
        }

        public long sysRoles_Delete(SysRoles_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);

                return setConnection.executeProcedure("SysRoles_Delete", parmList);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
            finally
            { }
        }

        public DataTable sysRoles_Retrieve(SysRoles_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysRoles_Retrieve", parmList);
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