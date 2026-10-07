using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserQuickLinks_BLL
    {
        public DataTable SysUserQuickLinks_Retrieve(SysUserQuickLink_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserQuickLinks_DAL objDAL = new SysUserQuickLinks_DAL();
                dt = objDAL.SysUserQuickLinks_Retrieve(objBOL);
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

        public long UserQuickLinks_Create(SysUserQuickLink_BOL objBOL)
        {
            try
            {
                SysUserQuickLinks_DAL objDAL = new SysUserQuickLinks_DAL();

                return objDAL.UserQuickLink_Create(objBOL);
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

        public long UserQuickLinks_Delete(SysUserQuickLink_BOL objBOL)
        {
            try
            {
                SysUserQuickLinks_DAL objDAL = new SysUserQuickLinks_DAL();
                return objDAL.UserQuickLink_Delete(objBOL);
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

        //public DataTable getConfigsToAddInQuickLink(UserMenuItemsPermission_BOL objBOL)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        UserMenuItemsQuickLink_DAL objDAL = new UserMenuItemsQuickLink_DAL();
        //        dt = objDAL.getConfigToAddInQuickLinks(objBOL);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        PRErrorLog objErrorLog = new PRErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return null;
        //    }
        //    finally
        //    { }
        //}
        public DataTable checkAlreadyAdded(SysRolesMenuItem_BOL _objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserQuickLinks_DAL objDAL = new SysUserQuickLinks_DAL();
                dt = objDAL.checkAlreadyAddedInQuickLink(_objBOL);
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


        public static DataTable SysUserQuickLinks_Retrieve()
        {
            try
            {
                DataTable userQuickLinks = new DataTable();
                string userId = SessionVariables.getCurrentUserId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                long partition = SessionVariables.getCurrentUserPartition();
                if (!string.IsNullOrEmpty(userId) && partition > 0 && !string.IsNullOrEmpty(dataAreaId))
                {
                    SysUserQuickLink_BOL objBOL = new SysUserQuickLink_BOL();
                    objBOL.UserId = userId;
                    objBOL.DataAreaId = dataAreaId;
                    objBOL.Partition = partition;
                    SysUserQuickLinks_BLL objBLL = new SysUserQuickLinks_BLL();
                    userQuickLinks = objBLL.SysUserQuickLinks_Retrieve(objBOL);
                }
                else
                {
                    userQuickLinks = null;
                }
                return userQuickLinks;
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
