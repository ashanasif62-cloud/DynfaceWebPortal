using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.JmgAttendanceIntegrationSvcReference;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class JmgAttendanceIntegration
    {
        private readonly string serviceName = "JmgAttendanceIntegrationSvcGroup";
        public string tableName = "JmgAttendanceIntegration";
        public string actionItem = "Machine Attendance";

        public SysOperationResult_BOL updateEmpLocationAttendance(string _employeeId)
        {
            DateTime toDate = DateTime.Now;
            DateTime fromDate = toDate.AddHours(-24);

            return updateEmpLocationAttendance(fromDate, toDate, _employeeId);
        }
        public SysOperationResult_BOL updateEmpLocationAttendance(DateTime _fromDate, DateTime _toDate, string _employeeId)
        {
            DateTime fromDate, toDate;
            string employeeId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                if (!string.IsNullOrEmpty(_employeeId))
                {
                    fromDate = _fromDate;
                    toDate = _toDate;
                    employeeId = _employeeId;

                    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                    var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                    var endpointAddress = new EndpointAddress(serviceUriString);
                    var binding = SoapHelper.GetBinding();
                    var client = new JmgAttendanceIntegrationSvcClient(binding, endpointAddress);
                    var channel = client.InnerChannel;

                    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                        CallContext callContext = new CallContext();
                        callContext.MessageId = Guid.NewGuid().ToString();
                        callContext.Company = dataAreaId;

                        GeneralContract results = ((JmgAttendanceIntegrationSvc)channel).updateEmpLocationAttendanceAsync(
                            new updateEmpLocationAttendance(callContext, employeeId, fromDate, toDate)).Result.result;

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

        public SysOperationResult_BOL updateLocationAttendance(DateTime _fromDate, DateTime _toDate, string _locationId)
        {
            DateTime fromDate, toDate;
            string locationId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                fromDate = _fromDate;
                toDate = _toDate;
                locationId = _locationId;

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgAttendanceIntegrationSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((JmgAttendanceIntegrationSvc)channel).updateLocationAttendanceAsync(new updateLocationAttendance(callContext, fromDate, locationId, toDate)).Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);

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

    }
}
