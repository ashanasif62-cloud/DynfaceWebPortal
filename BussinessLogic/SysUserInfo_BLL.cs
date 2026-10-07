using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserInfo_BLL
    {
        public string sysUserInfo_Create(SysUserInfo_BOL objBOL)
        {
            try
            {
                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();

                bool isValidated = validateCreate(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserInfo_Create(objBOL);
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

        public string sysUserInfo_UpdateStatus(SysUserInfo_BOL objBOL)
        {
            try
            {
                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();

                bool isValidated = validateUpdateStatus(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserInfo_UpdateStatus(objBOL);
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
        public string sysUserInfo_UpdatePassword(SysUserInfo_BOL objBOL)
        {
            try
            {
                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();

                bool isValidated = validateUpdatePassword(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserInfo_UpdatePassword(objBOL);
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
        public string sysUserInfo_ResetPassword(SysUserInfo_BOL objBOL)
        {
            try
            {
                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();

                bool isValidated = validateResetPassword(objBOL);
                string results = string.Empty;
                if (isValidated)
                {
                    long resultId = objDAL.sysUserInfo_ResetPassword(objBOL);
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
        public string sysUserDetails_Create(SysUserInfo_BOL objBOL)
        {
            try
            {
                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();

                bool isValidated = validateCreateDetails(objBOL);
                string results = string.Empty;
                int resultId = 0;
                if (isValidated)
                {
                    string result = objDAL.sysUserDetails_Create(objBOL);
                    if (result == AlertType.Success.ToString())
                        resultId = 1;
                    results = GetResults.createOperationResults(resultId, result);
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

        private bool validateCreate(SysUserInfo_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string password = objBOL.Password;
            string createdBy = objBOL.CreatedBy;
            string employeeId = objBOL.EmployeeId;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(createdBy))
            {
                isValid = false;
            }
            return isValid;
        }
        private bool validateUpdateStatus(SysUserInfo_BOL objBOL)
        {
            bool isValid = true;
            
            long recId = objBOL.RecId;
            bool status = objBOL.Status;
            string modifiedBy = objBOL.ModifiedBy;

            if (recId <= 0 || string.IsNullOrEmpty(modifiedBy))
            {
                isValid = false;
            }
            return isValid;
        }
        private bool validateUpdatePassword(SysUserInfo_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string password = objBOL.Password;
            string newPassword = objBOL.NewPassword;
            string modifiedBy = objBOL.ModifiedBy;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(modifiedBy))
            {
                isValid = false;
            }
            return isValid;
        }
        private bool validateResetPassword(SysUserInfo_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string newPassword = objBOL.NewPassword;
            string modifiedBy = objBOL.ModifiedBy;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(modifiedBy))
            {
                isValid = false;
            }
            return isValid;
        }

        private bool validateCreateDetails(SysUserInfo_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string password = objBOL.Password;
            string dataAreaId = objBOL.DataAreaId;
            long partition = objBOL.Partition;
            string createdBy = objBOL.CreatedBy;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(dataAreaId) || string.IsNullOrWhiteSpace(createdBy) || partition <= 0)
            {
                isValid = false;
            }
            return isValid;
        }

        #region CurrentUserInfo
        //private DataTable getSysUserInfo(SysUserInfo_BOL objBOL)
        //{
        //    DataTable dt = new DataTable();
        //    SysUserInfo_DAL objDAL = new SysUserInfo_DAL();
        //    dt = objDAL.getSysUserInfo(objBOL);
        //    return dt;
        //}
        //public DataTable getCurrentUserInfo()
        //{
        //    try
        //    {
        //        DataTable SysUserInfo = new DataTable();
        //        string userId = SessionVariables.getCurrentUserId();
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //        long partition = SessionVariables.getCurrentUserPartition();

        //        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(dataAreaId) && partition > 0)
        //        {
        //            SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
        //            objBOL.UserId = userId;
        //            objBOL.DataAreaId = dataAreaId;
        //            objBOL.Partition = partition;
        //            SysUserInfo_BLL objBLL = new SysUserInfo_BLL();
        //            SysUserInfo = objBLL.getSysUserInfo(objBOL);
        //        }
        //        else
        //        {
        //            SysUserInfo = null;
        //        }
        //        return SysUserInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return null;
        //    }
        //    finally
        //    { }
        //} 
        #endregion

        public DataTable retrieveAllSysUserInfo()
        {
            try
            {
                DataTable SysUserInfo = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(dataAreaId) && partition > 0)
                {
                    SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
                    objBOL.UserId = userId;
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;

                    SysUserInfo_DAL objDAL = new SysUserInfo_DAL();
                    SysUserInfo = objDAL.sysUserInfo_RetrieveAll(objBOL);
                }
                else
                {
                    SysUserInfo = null;
                }
                return SysUserInfo;
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


        public DataTable validateUser(SysUserInfo_BOL objBOL)
        {
            try
            {
                DataTable result = null;

                SysUserInfo_DAL objDAL = new SysUserInfo_DAL();
                string userId = objBOL.UserId;
                string password = objBOL.Password;

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
                    result = objDAL.ValidateUser(objBOL);

                return result;
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

