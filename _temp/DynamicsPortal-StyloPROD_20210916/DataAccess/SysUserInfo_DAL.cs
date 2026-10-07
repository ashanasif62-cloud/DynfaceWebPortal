using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserInfo_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();
        private List<object> parmList = new List<object>();

        public long sysUserInfo_Create(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserName");
                parmList.Add(newBussinessObj.UserName);
                parmList.Add("@Password");
                parmList.Add(newBussinessObj.Password);
                parmList.Add("@EmployeeId");
                parmList.Add(newBussinessObj.EmployeeId);
                parmList.Add("@DefaultCompany");
                parmList.Add(newBussinessObj.DefaultCompany);
                //parmList.Add("@Image");
                //parmList.Add(newBussinessObj.Image);
                parmList.Add("@Status");
                parmList.Add(newBussinessObj.Status);
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@LicenseType");
                parmList.Add(newBussinessObj.LicenseType);
                parmList.Add("@CreatedBy");
                parmList.Add(newBussinessObj.CreatedBy);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                //parmList.Add("@DataAreaId");
                //parmList.Add(newBussinessObj.DataAreaId);
                return setConnection.executeProcedure("SysUserInfo_Create", parmList, true);
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

        public long sysUserInfo_UpdateStatus(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@RecId");
                parmList.Add(newBussinessObj.RecId);
                parmList.Add("@Status");
                parmList.Add(newBussinessObj.Status);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                return setConnection.executeProcedure("SysUserInfo_UpdateStatus", parmList);
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

        public long sysUserInfo_UpdatePassword(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@OldPassword");
                parmList.Add(newBussinessObj.Password);
                parmList.Add("@NewPassword");
                parmList.Add(newBussinessObj.NewPassword);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedure("SysUserInfo_UpdatePassword", parmList);
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

        public long sysUserInfo_ResetPassword(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@NewPassword");
                parmList.Add(newBussinessObj.NewPassword);
                parmList.Add("@ModifiedBy");
                parmList.Add(newBussinessObj.ModifiedBy);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedure("SysUserInfo_ResetPassword", parmList);
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

        public string sysUserDetails_Create(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@Password");
                parmList.Add(newBussinessObj.Password);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                parmList.Add("@CreatedBy");
                parmList.Add(newBussinessObj.CreatedBy);

                return setConnection.validateUsers("SysUserDetails_Create", parmList);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return string.Empty;
            }
            finally
            { }
        }

        public DataTable sysUserInfo_RetrieveAll(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysUserInfo_RetrieveAll", parmList);
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

        public DataTable ValidateUser(SysUserInfo_BOL newBussinessObj)
        {
            try
            {
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@Password");
                parmList.Add(newBussinessObj.Password);

                return setConnection.executeProcedureRetriveDataTable("validateUser", parmList);
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