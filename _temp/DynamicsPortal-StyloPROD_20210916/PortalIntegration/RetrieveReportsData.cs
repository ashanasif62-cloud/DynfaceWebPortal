using GeneralAuxiliary;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class RetrieveReports
    {
        private readonly string serviceName = "RetrieveReportsSvcGroup";

        public MemoryStream viewSalarySlipReport(string _employeeId, string _payPeriodCode)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                string results = string.Empty;
                string employeeId = _employeeId;
                string payPeriodCode = _payPeriodCode;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RetrieveReportSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    
                    //var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipReportAsync(
                    //            new RetrieveSalarySlipReport(callContext, employeeId, payPeriodCode)).Result;
                    var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipNewReportAsync(
                                new RetrieveSalarySlipNewReport(callContext, employeeId, payPeriodCode)).Result;

                    memoryStream = result.result;
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

            return memoryStream;
        }

        public MemoryStream viewTaxCertificateReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                string results = string.Empty;
                string employeeId = _employeeId;
                DateTime fromDate = _fromDate;
                DateTime toDate = _toDate;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RetrieveReportSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((RetrieveReportSvc)channel).RetrieveTaxCertificateReportAsync(
                                new RetrieveTaxCertificateReport(callContext, employeeId, fromDate, toDate)).Result;

                    memoryStream = result.result;
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
            return memoryStream;
        }

        public MemoryStream viewAttendanceInOutSheet(string _employeeId, DateTime _fromDate, DateTime _toDate, bool _reportees)
        {
            MemoryStream memoryStream = new MemoryStream();
            string employeeId = _employeeId;
            DateTime fromDate = _fromDate;
            DateTime toDate = _toDate;
            bool reportees = _reportees;
            string results = string.Empty;

            try
            {

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RetrieveReportSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((RetrieveReportSvc)channel).viewAttendanceInOutSheetAsync(
                                new viewAttendanceInOutSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    memoryStream = result.result;
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
            return memoryStream;
        }

        public MemoryStream viewAttendanceTimeSheet(string _employeeId, DateTime _fromDate, DateTime _toDate, bool _reportees)
        {
            MemoryStream memoryStream = new MemoryStream();
            string employeeId = _employeeId;
            DateTime fromDate = _fromDate;
            DateTime toDate = _toDate;
            bool reportees = _reportees;
            string results = string.Empty;

            try
            {

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RetrieveReportSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((RetrieveReportSvc)channel).viewTimeSheetAsync(
                                new viewTimeSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    memoryStream = result.result;
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
            return memoryStream;
        }

        public MemoryStream viewAttendanceTimeSheet_pre(string _employeeId, DateTime _fromDate, DateTime _toDate, bool _reportees)
        {
            MemoryStream memoryStream = new MemoryStream();
            string employeeId = _employeeId;
            DateTime fromDate = _fromDate;
            DateTime toDate = _toDate;
            bool reportees = _reportees;
            string results = string.Empty;

            try
            {

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RetrieveReportSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((RetrieveReportSvc)channel).viewPreTimeSheetAsync(
                                new viewPreTimeSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    memoryStream = result.result;
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
            return memoryStream;
        }



    }
}
