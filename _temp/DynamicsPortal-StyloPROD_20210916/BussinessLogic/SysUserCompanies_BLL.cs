using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserCompanies_BLL
    {
        private DataTable SysUserCompanies_Retrieve(SysUserCompanies_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserCompanies_DAL objDAL = new SysUserCompanies_DAL();
                dt = objDAL.SysUserCompanies_Retrieve(objBOL);
                return dt;
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

        public static DataTable SysUserCompanies_Retrieve()
        {
            try
            {
                DataTable userCompanies = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                //string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();
                if (!string.IsNullOrEmpty(userId) && partition > 0)
                {
                    SysUserCompanies_BOL objBOL = new SysUserCompanies_BOL();
                    objBOL.UserId = userId;
                    objBOL.Partition = partition;
                    SysUserCompanies_BLL objBLL = new SysUserCompanies_BLL();
                    userCompanies = objBLL.SysUserCompanies_Retrieve(objBOL);
                }
                else
                {
                    userCompanies = null;
                }
                return userCompanies;
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
