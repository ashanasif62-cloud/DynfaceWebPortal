using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysRolesMenuItems_BLL
    {
        public DataTable sysRoleMenuItems_RetrieveByRoleId(SysRolesMenuItem_BOL objBOL)
        {
            try
            {
                DataTable dt_sysRolesMenuItems = new DataTable();

                string roleId = objBOL.RoleId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();

                if (!string.IsNullOrEmpty(roleId) && !string.IsNullOrEmpty(dataAreaId) && partition > 0)
                {
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;

                    SysRolesMenuItems_DAL objDAL = new SysRolesMenuItems_DAL();
                    dt_sysRolesMenuItems = objDAL.sysRoleMenuItems_RetrieveByRoleId(objBOL);
                }
                else
                {
                    GetResults.incompleteDataMessage();
                    dt_sysRolesMenuItems = null;
                }
                return dt_sysRolesMenuItems;
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

        public DataTable sysRoleMenuItems_Retrieve()
        {
            try
            {
                DataTable dt_sysRolesMenuItems = new DataTable();
                
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();

                if (!string.IsNullOrEmpty(dataAreaId) && partition > 0)
                {
                    SysRolesMenuItem_BOL objBOL = new SysRolesMenuItem_BOL();
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;

                    SysRolesMenuItems_DAL objDAL = new SysRolesMenuItems_DAL();
                    dt_sysRolesMenuItems = objDAL.sysRoleMenuItems_Retrieve(objBOL);
                }
                else
                {
                    GetResults.incompleteDataMessage();
                    dt_sysRolesMenuItems = null;
                }
                return dt_sysRolesMenuItems;
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

        public string sysRolesMenuItems_Create(SysRolesMenuItem_BOL objBOL)
        {
            try
            {
                SysRolesMenuItems_DAL objDAL = new SysRolesMenuItems_DAL();

                objBOL.AccessLevel = AccessLevel.Delete.GetHashCode();
                objBOL.LicenseType = LicenseType.ESS.GetHashCode();
                objBOL.ModifiedBy = objBOL.CreatedBy;

                bool isValidated = validateCreate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRolesMenuItems_Create(objBOL);
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

        public string sysRolesMenuItems_Update(SysRolesMenuItem_BOL objBOL)
        {
            try
            {
                SysRolesMenuItems_DAL objDAL = new SysRolesMenuItems_DAL();

                bool isValidated = validateUpdate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRolesMenuItems_Update(objBOL);
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

        public string sysRolesMenuItems_Delete(SysRolesMenuItem_BOL objBOL)
        {
            try
            {
                SysRolesMenuItems_DAL objDAL = new SysRolesMenuItems_DAL();

                bool isValidated = validateDelete(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRolesMenuItems_Delete(objBOL);
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

        
        private bool validateCreate(SysRolesMenuItem_BOL objBOL)
        {
            bool isValid = true;
            string roleId = objBOL.RoleId;
            string menuItemId = objBOL.MenuItemId;
            string dataAreaId = objBOL.DataAreaId;
            string createdBy = objBOL.CreatedBy;
            long partition = objBOL.Partition;

            if (string.IsNullOrWhiteSpace(roleId) || string.IsNullOrWhiteSpace(menuItemId) || string.IsNullOrWhiteSpace(dataAreaId) ||
                string.IsNullOrWhiteSpace(createdBy) || partition <= 0)
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateUpdate(SysRolesMenuItem_BOL objBOL)
        {
            bool isValid = true;

            long recId = objBOL.RecId;
            string menuItemId = objBOL.MenuItemId;
            string modifiedBy = objBOL.ModifiedBy;

            if (recId <= 0 || string.IsNullOrEmpty(modifiedBy) || string.IsNullOrEmpty(menuItemId))
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateDelete(SysRolesMenuItem_BOL objBOL)
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

