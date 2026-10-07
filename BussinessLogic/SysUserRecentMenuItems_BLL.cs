using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserRecentMenuItems_BLL
    {
        public DataTable SysUserRecentMenuItems_Retrieve(SysUserRecentMenuItems_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserRecentMenuItems_DAL objDAL = new SysUserRecentMenuItems_DAL();
                dt = objDAL.SysUserRecentMenuItems_Retrieve(objBOL);
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

        public long UserRecentMenuItems_CreateOrUpdate(SysUserRecentMenuItems_BOL objBOL)
        {
            try
            {
                SysUserRecentMenuItems_DAL objDAL = new SysUserRecentMenuItems_DAL();

                return objDAL.UserRecentMenuItems_CreateOrUpdate(objBOL);
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

        public static DataTable SysUserRecentMenuItems_Retrieve()
        {
            try
            {
                DataTable UserRecentMenuItems = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();
                if (!string.IsNullOrEmpty(userId) && partition > 0 && !string.IsNullOrEmpty(dataAreaId))
                {
                    SysUserRecentMenuItems_BOL objBOL = new SysUserRecentMenuItems_BOL();
                    objBOL.UserId = userId;
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;
                    SysUserRecentMenuItems_BLL objBLL = new SysUserRecentMenuItems_BLL();
                    UserRecentMenuItems = objBLL.SysUserRecentMenuItems_Retrieve(objBOL);
                }
                else
                {
                    UserRecentMenuItems = null;
                }
                return UserRecentMenuItems;
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
