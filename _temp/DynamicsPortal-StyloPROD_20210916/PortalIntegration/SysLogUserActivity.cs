using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace PortalIntegration
{
    public class SysLogUserActivity
    {
        public static void logUserActivity(string _tableName, string _actionItem, ActionType _actionType, SysOperationResult_BOL _actionResults)
        {
            SysOperationResult_BOL actionResults = _actionResults;

            string tableName = _tableName;
            string actionItem = _actionItem;
            string actionType = _actionType.ToString();
            long recordRecId = actionResults.RecId;
            bool actionResult = actionResults.isSuccess;
            string actionMessage = actionResults.Message;
            DateTime actionDateTime = DateTime.Now;
            string userId = SessionVariables.getCurrentUserId();
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            long partition = SessionVariables.getCurrentUserPartition();

            SysUserAuditTrial_BOL objBOL = new SysUserAuditTrial_BOL();
            objBOL.UserId = userId;
            objBOL.TableName = tableName;
            objBOL.RecordRecId = recordRecId;
            objBOL.ActionItem = actionItem;
            objBOL.ActionType = actionType;
            objBOL.ActionResult = actionResult;
            objBOL.ActionMessage = actionMessage;
            objBOL.ActionDateTime = actionDateTime;
            objBOL.DataAreaId = dataAreaId;
            objBOL.Partition = partition;
            try
            {
                SysUserAuditTrials_BLL sysUserAuditTrials = new SysUserAuditTrials_BLL();
                string results = sysUserAuditTrials.SysUserAuditTrials_Create(objBOL);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

        }

        //public static void logUserActivity(string _tableName, string _actionItem, ActionType _actionType, SysOperationResult_BOL _actionResults, long _recordRecId)
        //{
        //    long recordRecId = _recordRecId;
        //    SysOperationResult_BOL actionResults = _actionResults;
        //    actionResults.RecId = recordRecId;

        //    logUserActivity(_tableName, _actionItem, _actionType, actionResults);
        //}

        //public static void logUserActivity(string _tableName, string _actionItem, ActionType _actionType, SysOperationResult_BOL _actionResults, long[] _recordsRecId)
        //{
        //    long[] recordsRecId = _recordsRecId;
        //    foreach (long recordRecId in recordsRecId)
        //    {
        //        logUserActivity(_tableName, _actionItem, _actionType, _actionResults, recordRecId);
        //    }
        //}


        public static void logUserActivity<T>(string _tableName, string _actionItem, ActionType _actionType, IEnumerable<T> _generalContract)
        {
            try
            {
                DataTable dt = RetrieveDatatable.createDataTable(_generalContract);
                foreach (DataRow recordRow in dt.Rows)
                {
                    SysOperationResult_BOL objBOL = SysOperationResults.operationResults(recordRow);
                    logUserActivity(_tableName, _actionItem, _actionType, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }

    }
}
