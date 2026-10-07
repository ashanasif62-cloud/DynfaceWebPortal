using BussinessObject;
using System;
using System.Data;
using System.Web;

namespace GeneralAuxiliary
{
    public class SessionVariables
    {

        public static bool clearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            return true;
        }

        public static bool clearCompanyRelatedSession()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                for (int i = 0; i < HttpContext.Current.Session.Contents.Count; i++)
                {
                    var key = HttpContext.Current.Session.Keys[i];
                    //var value = HttpContext.Current.Session[i];
                    if (key.Contains("dataTable_"))
                    {
                        HttpContext.Current.Session.Remove(key);
                        i--;
                    }
                }
                return true;
            }

            return false;
        }

        #region UserRelated

        public static bool setUserInfo(SysUserInfo_BOL _userInfo)
        {
            try
            {
                bool isStored = false;
                if (_userInfo != null)
                {
                    string dataAreaId = _userInfo.DataAreaId;
                    string dataAreaName = _userInfo.DataAreaName;
                    long partition = _userInfo.Partition;
                    string userId = _userInfo.UserId;
                    string userName = _userInfo.UserName;
                    string employeeId = _userInfo.EmployeeId;
                    string employeeName = _userInfo.EmployeeName;

                    bool isStoredDataAreaId = setUserCurrentDataAreaId(dataAreaId);
                    bool isStoredDataAreaName = setUserCurrentDataAreaName(dataAreaName);
                    bool isStoredPartition = setCurrentUserPartition(partition);
                    bool isStoredUserId = setCurrentUserId(userId);
                    bool isStoredUserName = setCurrentUserName(userName);
                    bool isStoredEmployeeName = setCurrentEmployeeName(employeeName);
                    bool isStoredEmployeeId = setCurrentEmployeeId(employeeId);
                    setCurrentUserLoginTime(DateTime.Now);

                    if (isStoredDataAreaId && isStoredDataAreaName && isStoredPartition && isStoredUserId && isStoredUserName && isStoredEmployeeId && isStoredEmployeeName)
                        isStored = true;

                    #region commented
                    //if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    //{
                    //    HttpContext.Current.Session["userInfo"] = _userInfo;
                    //    isStored = true;
                    //} 
                    #endregion
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setUserCurrentDataAreaId(string _dataAreaId)
        {
            try
            {
                string dataAreaId = _dataAreaId;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_DataAreaId"] = dataAreaId;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setUserCurrentDataAreaName(string _dataAreaName)
        {
            try
            {
                string dataAreaName = _dataAreaName;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_DataAreaName"] = dataAreaName;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentUserPartition(long _partition)
        {
            try
            {
                long partition = _partition;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_Partition"] = partition;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentUserId(string _userId)
        {
            try
            {
                string userId = _userId;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_UserId"] = userId;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentUserName(string _userName)
        {
            try
            {
                string userName = _userName;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_UserName"] = userName;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentEmployeeName(string _employeeName)
        {
            try
            {
                string employeeName = _employeeName;
                bool isStored = false;

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_EmployeeName"] = employeeName;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentEmployeeId(string _employeeId)
        {
            try
            {
                string employeeId = _employeeId;
                bool isStored = false;

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_EmployeeId"] = employeeId;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentUserLoginTime(DateTime _loginTime)
        {
            try
            {
                DateTime loginTime = _loginTime;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_LoginTime"] = loginTime;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static bool setCurrentUserImage(string _userImageData)
        {
            try
            {
                string userImageData = _userImageData;
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userInfo_ImageData"] = userImageData;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }

        public static string getCurrentUserImage()
        {
            try
            {
                string userImageData = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_ImageData"] != null)
                    {
                        userImageData = HttpContext.Current.Session["userInfo_ImageData"] as string;
                    }
                }
                return userImageData;
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
        public static string getUserCurrentDataAreaId()
        {
            try
            {
                string dataAreaId = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_DataAreaId"] != null)
                    {
                        dataAreaId = HttpContext.Current.Session["userInfo_DataAreaId"] as string;
                    }
                }
                return dataAreaId;
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
        public static string getUserCurrentDataAreaName()
        {
            try
            {
                string dataAreaName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_DataAreaName"] != null)
                    {
                        dataAreaName = HttpContext.Current.Session["userInfo_DataAreaName"] as string;
                    }
                }
                return dataAreaName;
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
        public static long getCurrentUserPartition()
        {
            try
            {
                long currentUserPartition = 0;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_Partition"] != null)
                    {
                        Int64.TryParse(HttpContext.Current.Session["userInfo_Partition"].ToString(), out currentUserPartition);
                    }
                }
                return currentUserPartition;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return -1;
            }
            finally
            { }
        }
        public static string getCurrentUserId()
        {
            try
            {
                string currentUserId = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_UserId"] != null)
                    {
                        currentUserId = HttpContext.Current.Session["userInfo_UserId"].ToString();
                    }
                }
                return currentUserId;
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
        public static string getCurrentUserName()
        {
            try
            {
                string currentUserName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_UserName"] != null)
                    {
                        currentUserName = HttpContext.Current.Session["userInfo_UserName"] as string;
                    }
                }
                return currentUserName;
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
        public static string getCurrentEmployeeName()
        {
            try
            {
                string employeeName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_EmployeeName"] != null)
                    {
                        employeeName = HttpContext.Current.Session["userInfo_EmployeeName"] as string;
                    }
                }
                return employeeName;
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
        public static string getCurrentEmployeeId()
        {
            try
            {
                string employeeId = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_EmployeeId"] != null)
                    {
                        employeeId = HttpContext.Current.Session["userInfo_EmployeeId"] as string;
                    }
                }
                return employeeId;
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

        public static DateTime getCurrentUserLoginTime()
        {
            try
            {
                DateTime loginTime = DateTime.Now;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userInfo_LoginTime"] != null)
                    {
                        loginTime = (DateTime)(HttpContext.Current.Session["userInfo_LoginTime"]);
                    }
                }
                return loginTime;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return DateTime.Now;
            }
            finally
            { }
        }

        //public static User_BOL getCurrentUser()
        //{
        //    try
        //    {
        //        User_BOL currentUser = null;
        //        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        //        {
        //            if (HttpContext.Current.Session["userInfo"] != null)
        //            {
        //                currentUser = HttpContext.Current.Session["userInfo"] as User_BOL;
        //            }
        //        }
        //        return currentUser;
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.message = ex.Message;
        //        Alert.showMessage(AlertType.Error);

        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return null;
        //    }
        //    finally
        //    { }
        //}


        public static long getSelectedUserId()
        {
            long userId = 0;
            if (HttpContext.Current != null || HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["selectedUserId"] != null)
                {
                    Int64.TryParse(HttpContext.Current.Session["selectedUserId"].ToString(), out userId);
                }
            return userId;
        }





        public static bool setUserMenuItems(DataTable _userMenuItems)
        {
            try
            {
                bool isStored = false;
                if (_userMenuItems != null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session["userMenuItems"] = _userMenuItems;
                        isStored = true;
                    }
                }
                else
                {
                    isStored = false;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static DataTable getUserMenuItems()
        {
            try
            {
                DataTable currentUserMenuItems = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["userMenuItems"] != null)
                    {
                        currentUserMenuItems = HttpContext.Current.Session["userMenuItems"] as DataTable;
                    }
                }
                return currentUserMenuItems;
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


        public static bool setUserCompanies(DataTable _userCompanies)
        {
            try
            {
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userCompanies"] = _userCompanies;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static DataTable getUserCompanies()
        {
            try
            {
                DataTable userCompanies = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session["userCompanies"] != null)
                    {
                        userCompanies = HttpContext.Current.Session["userCompanies"] as DataTable;
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


        public static bool setUserQuickLinks(DataTable _userQuickLinks)
        {
            try
            {
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["userQuickLinks"] = _userQuickLinks;
                    isStored = true;
                }
                return isStored;

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static DataTable getCurrentUserQuickLinks()
        {
            try
            {

                DataTable currentUserQuickLinks = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session["userQuickLinks"] != null)
                    {
                        currentUserQuickLinks = HttpContext.Current.Session["userQuickLinks"] as DataTable;
                    }
                return currentUserQuickLinks;

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

        public static bool setUserRecentMenuItems(DataTable _userRecentMenuItems)
        {
            try
            {
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserRecentMenuItems"] = _userRecentMenuItems;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static DataTable getUserRecentMenuItems()
        {
            try
            {

                DataTable UserRecentMenuItems = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session["UserRecentMenuItems"] != null)
                    {
                        UserRecentMenuItems = HttpContext.Current.Session["UserRecentMenuItems"] as DataTable;
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


        public static bool setUserRoles(DataTable _userRoles)
        {
            try
            {
                bool isStored = false;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserRoles"] = _userRoles;
                    isStored = true;
                }
                return isStored;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }
        public static DataTable getUserRoles()
        {
            try
            {

                DataTable UserRoles = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session["UserRoles"] != null)
                    {
                        UserRoles = HttpContext.Current.Session["UserRoles"] as DataTable;
                    }
                return UserRoles;
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
        #endregion

        public static bool setSessionParmList(DataTable _parmList)
        {
            bool isStored = false;
            DataTable parmList = _parmList;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["Menu_ParameterList"] = parmList;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionParmList()
        {
            DataTable parmList = null;

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["Menu_ParameterList"] != null)
                {
                    parmList = new DataTable();
                    parmList = HttpContext.Current.Session["Menu_ParameterList"] as DataTable;
                }
            return parmList;
        }


        public static bool setSessionDataSet(DataSet _dataSet)
        {
            bool isStored = false;
            DataSet dataSet = new DataSet();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataSet = _dataSet.Copy();
                HttpContext.Current.Session["dataSet"] = dataSet;
                isStored = true;
            }
            return isStored;
        }
        public static DataSet getSessionDataSet()
        {
            DataSet dataSet = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataSet"] != null)
                {
                    dataSet = new DataSet();
                    dataSet = (HttpContext.Current.Session["dataSet"] as DataSet).Copy();
                }
            return dataSet;
        }

        public static bool setSessionDataTable(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable"] as DataTable).Copy();
                }
            return dataTable;
        }



        #region GridControls

        public static bool setSessionDataTable_LogisticsElectroonicAddress(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_LogisticsElectroonicAddress"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_LogisticsElectroonicAddress()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_LogisticsElectroonicAddress"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_LogisticsElectroonicAddress"] as DataTable).Copy();
                }
            return dataTable;
        }





        public static bool setSessionDataTable_PRLeaveCodes(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_PRLeaveCodes"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_PRLeaveCodes()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_PRLeaveCodes"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_PRLeaveCodes"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_EmployeeRequestioner(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_EmployeeRequestioner"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static bool setSessionDataTable_EmployeeReportees(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_EmployeeReportees"] = dataTable;
                isStored = true;
            }
            return isStored;
        }


        public static DataTable getSessionDataTable_EmployeeRequestioner()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_EmployeeRequestioner"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_EmployeeRequestioner"] as DataTable).Copy();
                }
            return dataTable;
        }

        public static DataTable getSessionDataTable_EmployeeReportees()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_EmployeeReportees"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_EmployeeReportees"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_AllEmployee(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_AllEmployee"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_AllEmployee()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_AllEmployee"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_AllEmployee"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_PRAdvanceTypes(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_PRAdvanceTypes"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_PRAdvanceTypes()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_PRAdvanceTypes"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_PRAdvanceTypes"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_PRLoanTypes(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_PRLoanTypes"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_PRLoanTypes()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_PRLoanTypes"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_PRLoanTypes"] as DataTable).Copy();
                }
            return dataTable;
        }

        public static bool setSessionDataTable_CurrencyDetails(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_CurrencyDetails"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_CurrencyDetails()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_CurrencyDetails"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_CurrencyDetails"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_ActivePayPeriod(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_ActivePayPeriod"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_ActivePayPeriod()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_ActivePayPeriod"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_ActivePayPeriod"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_PREOSNoticePeriods(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_PREOSNoticePeriods"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_PREOSNoticePeriods()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_PREOSNoticePeriods"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_PREOSNoticePeriods"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_HcmReasonCodes(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_HcmReasonCodes"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_HcmReasonCodes()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_HcmReasonCodes"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_HcmReasonCodes"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_ESSRequestedFor(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_ESSRequestedFor"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_ESSRequestedFor()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_ESSRequestedFor"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_ESSRequestedFor"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_DimLocation(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_DimLocation"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_DimLocation()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_DimLocation"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_DimLocation"] as DataTable).Copy();
                }
            return dataTable;
        }



        public static bool setSessionDataTable_HcmJob(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_HcmJob"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_HcmJob()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_HcmJob"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_HcmJob"] as DataTable).Copy();
                }
            return dataTable;
        }


        public static bool setSessionDataTable_HcmEducationDiscipline(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                dataTable = _dataTable.Copy();
                HttpContext.Current.Session["dataTable_HcmEducationDiscipline"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public static DataTable getSessionDataTable_HcmEducationDiscipline()
        {
            DataTable dataTable = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["dataTable_HcmEducationDiscipline"] != null)
                {
                    dataTable = new DataTable();
                    dataTable = (HttpContext.Current.Session["dataTable_HcmEducationDiscipline"] as DataTable).Copy();
                }
            return dataTable;
        }








        #endregion






    }
}