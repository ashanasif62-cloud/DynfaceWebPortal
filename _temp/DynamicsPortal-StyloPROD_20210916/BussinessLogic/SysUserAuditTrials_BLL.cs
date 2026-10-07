using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using System;
using System.Data;

namespace BussinessLogic
{
    public class SysUserAuditTrials_BLL
    {
        public string logActivity(string _userId, string _tableName, long _recordRecId, string _actionType, string _actionItem, bool _actionResult,
                                  DateTime _actionDateTime, string _actionMessage, string _dataAreaId, long _partition)
        {

            string userId = _userId;
            string actionItem = _actionItem;
            string tableName = _tableName;
            string actionType = _actionType;
            bool actionResult = _actionResult;
            string dataAreaId = _dataAreaId;
            long recordRecId = _recordRecId;
            DateTime actionDateTime = _actionDateTime;
            string actionMessage = _actionMessage;
            long partition = _partition;

            SysUserAuditTrial_BOL objBOL = new SysUserAuditTrial_BOL();
            objBOL.UserId = userId;
            objBOL.TableName = tableName;
            objBOL.RecordRecId = recordRecId;
            objBOL.ActionItem = actionItem;
            objBOL.ActionType = actionType;
            objBOL.ActionResult = actionResult;
            objBOL.ActionDateTime = actionDateTime;
            objBOL.DataAreaId = dataAreaId;
            objBOL.Partition = partition;
            objBOL.ActionMessage = actionMessage;

            string results = SysUserAuditTrials_Create(objBOL);
            return results;
        }

        public DataTable SysUserRecentActivities_Retrieve(SysUserAuditTrial_BOL objBOL)
        {
            try
            {
                DataTable dt = new DataTable();
                SysUserAuditTrials_DAL objDAL = new SysUserAuditTrials_DAL();
                dt = objDAL.SysUserRecentActivities_Retrieve(objBOL);
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

        public string SysUserAuditTrials_Create(SysUserAuditTrial_BOL objBOL)
        {
            try
            {
                bool isValidated = validateCreate(objBOL);
                SysUserAuditTrials_DAL objDAL = new SysUserAuditTrials_DAL();
                string results = string.Empty;
                if (isValidated)
                {
                    Int64 resultId = objDAL.SysUserAuditTrials_Create(objBOL);
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
                return string.Empty;
            }
            finally
            { }
        }

        private bool validateCreate(SysUserAuditTrial_BOL objBOL)
        {
            bool isValid = true;

            string userId = objBOL.UserId;
            string actionItem = objBOL.ActionItem;
            string tableName = objBOL.TableName;
            string dataAreaId = objBOL.DataAreaId;
            string actionType = objBOL.ActionType;
            long partition = objBOL.Partition;
            long recordId = objBOL.RecordRecId;

            //bool actionResult = objBOL.ActionResult;
            //DateTime actionDateTime = objBOL.ActionDateTime;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(actionItem) ||
                string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(actionType) ||
                string.IsNullOrWhiteSpace(dataAreaId) || partition <= 0 || recordId <= 0)
            {
                isValid = false;
            }
            return isValid;
        }
        
    }
}
