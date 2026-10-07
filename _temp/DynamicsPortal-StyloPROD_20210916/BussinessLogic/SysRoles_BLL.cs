using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysRoles_BLL
    {
        public DataTable sysRoles_Retrieve()
        {
            try
            {
                DataTable dt_sysRoles = new DataTable();

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();

                if (!string.IsNullOrEmpty(dataAreaId) && partition > 0)
                {
                    SysRoles_BOL objBOL = new SysRoles_BOL();
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;

                    SysRoles_DAL objDAL = new SysRoles_DAL();
                    dt_sysRoles = objDAL.sysRoles_Retrieve(objBOL);
                }
                else
                {
                    GetResults.incompleteDataMessage();
                    dt_sysRoles = null;
                }
                return dt_sysRoles;
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

        public string sysRoles_Create(SysRoles_BOL objBOL)
        {
            try
            {
                SysRoles_DAL objDAL = new SysRoles_DAL();
                objBOL.LicenseType = LicenseType.ESS.GetHashCode();

                bool isValidated = validateCreate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRoles_Create(objBOL);
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

        public string sysRoles_Update(SysRoles_BOL objBOL)
        {
            try
            {
                SysRoles_DAL objDAL = new SysRoles_DAL();

                bool isValidated = validateUpdate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRoles_Update(objBOL);
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

        public string sysRoles_Delete(SysRoles_BOL objBOL)
        {
            try
            {
                SysRoles_DAL objDAL = new SysRoles_DAL();

                bool isValidated = validateDelete(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysRoles_Delete(objBOL);
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

        
        private bool validateCreate(SysRoles_BOL objBOL)
        {
            bool isValid = true;

            string roleId = objBOL.RoleId;
            string dataAreaId = objBOL.DataAreaId;
            string createdBy = objBOL.CreatedBy;
            long partition = objBOL.Partition;

            if (string.IsNullOrWhiteSpace(roleId) || string.IsNullOrWhiteSpace(dataAreaId) ||
                string.IsNullOrWhiteSpace(createdBy) || partition <= 0)
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateUpdate(SysRoles_BOL objBOL)
        {
            bool isValid = true;

            long recId = objBOL.RecId;
            string modifiedBy = objBOL.ModifiedBy;

            if (recId <= 0 || string.IsNullOrEmpty(modifiedBy))
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateDelete(SysRoles_BOL objBOL)
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

