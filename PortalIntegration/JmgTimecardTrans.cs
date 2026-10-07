using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.JmgTimecardTransSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class JmgTimecardTrans
    {
        private readonly string serviceName = "JmgTimecardTransSvcGroup";
        public string tableName = "JmgTimecardTrans";
        public string actionItem = "Time and Attendance - Lines";

        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new JmgTimecardTransSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        JmgTimecardTransSvcContract jmgTimecardTransContract = new JmgTimecardTransSvcContract();

                        jmgTimecardTransContract.RequestedBy = dataRow["RequestedBy"].ToString();
                        jmgTimecardTransContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        jmgTimecardTransContract.ProfileDate = Convert.ToDateTime(dataRow["ProfileDate"].ToString());
                        jmgTimecardTransContract.StartDateTime = dataRow["StartDateTime"].ToString().toDateTime(true).ToUniversalTime();
                        jmgTimecardTransContract.StopDateTime = jmgTimecardTransContract.StartDateTime;
                        jmgTimecardTransContract.ReasonCodeId = dataRow["ReasonCodeId"].ToString();

                        //jmgTimecardTransContract.ActualDateTime = dataRow["ActualDateTime"].ToString().toDateTime(true).ToUniversalTime();
                        jmgTimecardTransContract.GenerationType = JmgGenerationType.ESSPortal;
                        jmgTimecardTransContract.JourRegType = dataRow["JourRegType"] is DBNull ?
                                JmgJourRegTypeEnum.SignIn : (JmgJourRegTypeEnum)Enum.Parse(typeof(JmgJourRegTypeEnum), dataRow["JourRegType"].ToString());

                        //jmgTimecardTransContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((JmgTimecardTransSvc)channel).createAsync(new create(callContext, jmgTimecardTransContract)).Result.result;
                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
                    }
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
            return objBOL;
        }

        public SysOperationResult_BOL update(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new JmgTimecardTransSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        long recId = 0;
                        JmgTimecardTransSvcContract jmgTimecardTransContract = new JmgTimecardTransSvcContract();

                        Int64.TryParse(dataRow["RecId"].ToString(), out recId);
                        //jmgTimecardTransContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        //jmgTimecardTransContract.ProfileDate = Convert.ToDateTime(dataRow["ProfileDate"].ToString());//dataRow["ProfileDate"].ToString().toDateTime();
                        //jmgTimecardTransContract.GenerationType = JmgGenerationType.ESSPortal;
                        jmgTimecardTransContract.RequestedBy = dataRow["RequestedBy"].ToString();
                        jmgTimecardTransContract.StartDateTime = dataRow["StartDateTime"].ToString().toDateTime(true).ToUniversalTime();
                        jmgTimecardTransContract.StopDateTime = jmgTimecardTransContract.StartDateTime;
                        jmgTimecardTransContract.GenerationType = JmgGenerationType.ESSPortal;
                        jmgTimecardTransContract.ReasonCodeId = dataRow["ReasonCodeId"].ToString();

                        jmgTimecardTransContract.JourRegType = dataRow["JourRegType"] is DBNull ?
                                JmgJourRegTypeEnum.SignIn : (JmgJourRegTypeEnum)Enum.Parse(typeof(JmgJourRegTypeEnum), dataRow["JourRegType"].ToString());
                        jmgTimecardTransContract.RecId = recId;

                        GeneralContract results = ((JmgTimecardTransSvc)channel).updateAsync(new update(callContext, jmgTimecardTransContract)).Result.result;
                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
                    }
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
            return objBOL;
        }

        public SysOperationResult_BOL delete(long[] _recordsRecId)
        {
            long[] recordsRecId = _recordsRecId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgTimecardTransSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((JmgTimecardTransSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
                    objBOL = SysOperationResults.operationResults(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, results);
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
            return objBOL;
        }

        public DataTable retrieveByEmployeeProfileDate(string _employeeId, DateTime _profileDate)
        {
            DateTime profileDate = _profileDate;
            string employeeId = _employeeId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgTimecardTransSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgTimecardTransSvc)channel).retrieveByEmployeeProfileDateAsync(
                                    new retrieveByEmployeeProfileDate(callContext, employeeId, profileDate)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return createDataTable();
            }
            finally
            { }
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new JmgTimecardTransSvcContract[] { });
            return dataTable;
        }

    }
}
