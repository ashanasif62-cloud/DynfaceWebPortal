using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserRoles_BLL
    {
        public DataTable SysUserRoles_RetrieveByRoleId(SysUserRole_BOL objBOL)
        {
            DataTable dt = new DataTable();
            SysUserRoles_DAL objDAL = new SysUserRoles_DAL();
            dt = objDAL.SysUserRoles_RetrieveByRoleId(objBOL);
            return dt;
        }

        public DataTable SysUserRoles_RetrieveAll(SysUserRole_BOL objBOL)
        {
            DataTable dt = new DataTable();
            SysUserRoles_DAL objDAL = new SysUserRoles_DAL();
            dt = objDAL.SysUserRoles_RetrieveAll(objBOL);
            return dt;
        }

        public DataTable SysUserRoles_RetrieveByUserId(SysUserRole_BOL objBOL)
        {
            DataTable dt = new DataTable();
            SysUserRoles_DAL objDAL = new SysUserRoles_DAL();
            dt = objDAL.SysUserRoles_RetrieveByUserId(objBOL);
            return dt;
        }

        public static DataTable SysUserRoles_RetrieveByUserId()
        {
            try
            {
                DataTable userRoles = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(dataAreaId) && partition > 0)
                {
                    SysUserRole_BOL objBOL = new SysUserRole_BOL();
                    objBOL.UserId = userId;
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;
                    SysUserRoles_BLL objBLL = new SysUserRoles_BLL();
                    userRoles = objBLL.SysUserRoles_RetrieveByUserId(objBOL);
                }
                else
                {
                    userRoles = null;
                }
                return userRoles;
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


        public string sysUserRoles_Create(SysUserRole_BOL objBOL)
        {
            try
            {
                SysUserRoles_DAL objDAL = new SysUserRoles_DAL();
                objBOL.RecVersion = 1;
                objBOL.LicenseType = LicenseType.ESS.GetHashCode();


                bool isValidated = validateCreate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserRoles_Create(objBOL);
                    results = GetResults.createOperationResults(resultId);
                }
                else
                    results = GetResults.incompleteDataMessage();

                return results;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return GetResults.xmlErrorMessage(ex.Message);
            }
            finally
            { }
        }

        public string sysUserRoles_Update(SysUserRole_BOL objBOL)
        {
            try
            {
                SysUserRoles_DAL objDAL = new SysUserRoles_DAL();

                bool isValidated = validateUpdate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserRoles_Update(objBOL);
                    results = GetResults.updateOperationResults(resultId);
                }
                else
                    results = GetResults.incompleteDataMessage();

                return results;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return GetResults.xmlErrorMessage(ex.Message);

            }
            finally
            { }
        }

        public string sysUserRoles_Delete(SysUserRole_BOL objBOL)
        {
            try
            {
                SysUserRoles_DAL objDAL = new SysUserRoles_DAL();

                bool isValidated = validateDelete(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserRoles_Delete(objBOL);
                    results = GetResults.deleteOperationResults(resultId);
                }
                else
                    results = GetResults.incompleteDataMessage();

                return results;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return GetResults.xmlErrorMessage(ex.Message);
            }
            finally
            { }
        }


        private bool validateCreate(SysUserRole_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string roleId = objBOL.RoleId;
            string dataAreaId = objBOL.DataAreaId;
            string createdBy = objBOL.CreatedBy;
            long partition = objBOL.Partition;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId) ||
                string.IsNullOrWhiteSpace(dataAreaId) || string.IsNullOrWhiteSpace(createdBy) || partition <= 0)
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateUpdate(SysUserRole_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string roleId = objBOL.RoleId;
            long recId = objBOL.RecId;
            string modifiedBy = objBOL.ModifiedBy;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId) ||
                string.IsNullOrEmpty(modifiedBy) || recId <= 0)
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateDelete(SysUserRole_BOL objBOL)
        {
            bool isValid = true;

            long recId = objBOL.RecId;

            if (recId <= 0)
            {
                isValid = false;
            }
            return isValid;
        }







    }
}
