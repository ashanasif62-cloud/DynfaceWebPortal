using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserMenuItems_BLL
    {

        public static DataTable SysUserMenuItems_Retrieve()
        {
            try
            {
                DataTable userMenuItems = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();
                if (!string.IsNullOrEmpty(userId) && partition > 0 && !string.IsNullOrEmpty(dataAreaId))
                {
                    SysUserMenuItems_BOL objBOL = new SysUserMenuItems_BOL();
                    objBOL.UserId = userId;
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;
                    SysUserMenuItems_BLL objBLL = new SysUserMenuItems_BLL();
                    userMenuItems = objBLL.SysUserMenuItems_Retrieve(objBOL);
                }
                else
                    userMenuItems = null;
                return userMenuItems;
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

        private DataTable SysUserMenuItems_Retrieve(SysUserMenuItems_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserMenuItems_DAL objDAL = new SysUserMenuItems_DAL();
                dt = objDAL.SysUserMenuItems_Retrieve(objBOL);
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

        private DataTable validateUserMenuItems(SysUserMenuItems_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserMenuItems_DAL objDAL = new SysUserMenuItems_DAL();
                dt = objDAL.validateUserMenuItems(objBOL);
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
        
    }
}
