using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class SysUserAuditTrials_DAL
    {
        private getConnection_DAL setConnection = new getConnection_DAL();

        public DataTable SysUserRecentActivities_Retrieve(SysUserAuditTrial_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);

                return setConnection.executeProcedureRetriveDataTable("SysUserRecentActivities_Retrieve", parmList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public long SysUserAuditTrials_Create(SysUserAuditTrial_BOL newBussinessObj)
        {
            try
            {
                List<object> parmList = new List<object>();
                parmList.Add("@UserId");
                parmList.Add(newBussinessObj.UserId);
                parmList.Add("@ActionDateTime");
                parmList.Add(newBussinessObj.ActionDateTime);
                parmList.Add("@ActionItem");
                parmList.Add(newBussinessObj.ActionItem);
                parmList.Add("@TableName");
                parmList.Add(newBussinessObj.TableName);
                parmList.Add("@RecordRecId");
                parmList.Add(newBussinessObj.RecordRecId);
                parmList.Add("@ActionResult");
                parmList.Add(newBussinessObj.ActionResult);
                parmList.Add("@ActionMessage");
                parmList.Add(newBussinessObj.ActionMessage);
                parmList.Add("@ActionType");
                parmList.Add(newBussinessObj.ActionType);
                parmList.Add("@DataAreaId");
                parmList.Add(newBussinessObj.DataAreaId);
                parmList.Add("@Partition");
                parmList.Add(newBussinessObj.Partition);
                return setConnection.executeProcedure("SysUserAuditTrails_Create", parmList, true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return 0;
            }
        }

        private void LogError(Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            string currentMethodName = currentMethod?.DeclaringType?.FullName ?? "Unknown";
            objErrorLog.write(currentMethodName, ex);
        }

    }
}

