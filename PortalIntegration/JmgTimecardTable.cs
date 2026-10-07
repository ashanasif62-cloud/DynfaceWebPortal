using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.JmgTimecardTableSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class JmgTimecardTable
    {
        private readonly string serviceName = "JmgTimecardTableSvcGroup";
        public string tableName = "JmgTimecardTable";
        public string actionItem = "Time and Attendance";

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

                var client = new JmgTimecardTableSvcClient(binding, endpointAddress);
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
                        Int64.TryParse(dataRow["RecId"].ToString(), out recId);

                        long clockInRecId = 0;
                        Int64.TryParse(dataRow["ClockInRecId"].ToString(), out clockInRecId);

                        long clockOutRecId = 0;
                        Int64.TryParse(dataRow["ClockOutRecId"].ToString(), out clockOutRecId);

                        JmgTimecardTableSvcContract jmgTimecardTableContract = new JmgTimecardTableSvcContract();
                        jmgTimecardTableContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        jmgTimecardTableContract.ProfileId = dataRow["ProfileId"].ToString();
                        jmgTimecardTableContract.Remarks = dataRow["Remarks"].ToString();
                        jmgTimecardTableContract.RecId = recId;

                        jmgTimecardTableContract.ClockInDateTime = dataRow["ClockInDateTime"].ToString().toDateTime(true).ToUniversalTime();
                        jmgTimecardTableContract.ClockInReasonCodeId = dataRow["ClockInReasonCodeId"].ToString();
                        jmgTimecardTableContract.ClockInGenerationType = JmgGenerationType.ESSPortal;
                        jmgTimecardTableContract.ClockInRecId = clockInRecId;

                        jmgTimecardTableContract.ClockOutDateTime = dataRow["ClockOutDateTime"].ToString().toDateTime(true).ToUniversalTime();
                        jmgTimecardTableContract.ClockOutReasonCodeId = dataRow["ClockOutReasonCodeId"].ToString();
                        jmgTimecardTableContract.ClockOutGenerationType = JmgGenerationType.ESSPortal;
                        jmgTimecardTableContract.ClockOutRecId = clockOutRecId;

                        GeneralContract results = ((JmgTimecardTableSvc)channel).updateAsync(new update(callContext, jmgTimecardTableContract)).Result.result;
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

        public DataTable retrive(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            try
            {
                DateTime fromDate = _fromDate;
                DateTime toDate = _toDate;
                string employeeId = _employeeId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgTimecardTableSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgTimecardTableSvc)channel).retrieveAsync(new retrieve(callContext, employeeId, fromDate, toDate)).Result.result;

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


        public DataTable retrieveSingleLine(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            try
            {
                DateTime fromDate = _fromDate;
                DateTime toDate = _toDate;
                string employeeId = _employeeId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgTimecardTableSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgTimecardTableSvc)channel).retrieveSingleLineAsync(new retrieveSingleLine(callContext, employeeId, fromDate, toDate)).Result.result;

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



        public DataTable employeeMissingEntries(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            try
            {
                DateTime fromDate = _fromDate;
                DateTime toDate = _toDate;
                string employeeId = _employeeId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgTimecardTableSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgTimecardTableSvc)channel).employeeMissingEntriesAsync(new employeeMissingEntries(callContext, employeeId, fromDate, toDate)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new JmgTimecardTableSvcContract[] { });
            return dataTable;
        }

    }
}
