using GeneralAuxiliary;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

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

                    var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipReportAsync(
                                new RetrieveSalarySlipReport(callContext, employeeId, payPeriodCode)).Result;
                    //var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipNewReportAsync(
                    //            new RetrieveSalarySlipNewReport(callContext, employeeId, payPeriodCode)).Result;

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

        //Haris Mod
        public string emailSalarySlipReport(string _employeeId, string _payPeriodCode)
        {
            string responseMessage = "Email not sent successfully";  // Default failure message
            try
            {
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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Call the D365 API to send the salary slip email
                    var result = ((RetrieveReportSvc)channel).sendSalarySlipEmailAsync(
                                new sendSalarySlipEmail(callContext, employeeId, payPeriodCode)).Result;

                    //// Check the result and set the response message
                    //if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent Successfully")
                    //{
                    //    responseMessage = "Email sent Successfully";
                    //}
                    responseMessage = result.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return responseMessage;
        }
        public MemoryStream viewAttendnaceRegisterReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
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

                    var result = ((RetrieveReportSvc)channel).viewAttendanceRegisterAsync(
                                new viewAttendanceRegister(callContext, employeeId, fromDate, toDate)).Result;
                    //var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipNewReportAsync(
                    //            new RetrieveSalarySlipNewReport(callContext, employeeId, payPeriodCode)).Result;

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

        public string emailAttendanceRegisterReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            string responseMessage = "Email not sent successfully";  // Default failure message
            try
            {
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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Call the D365 API to send the salary slip email
                    var result = ((RetrieveReportSvc)channel).sendTASAttendanceRegisterAsync(
                                new sendTASAttendanceRegister(callContext, employeeId, fromDate, toDate)).Result;

                    // Check the result and set the response message
                    if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent successfully")
                    {
                        responseMessage = "Email sent successfully";
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

            return responseMessage;
        }
      




        public string emailTaxCertificateReport(string _employeeId, int _payPeriodYear)
        {
            string responseMessage = "Error: Email not sent"; // Default failure message
            try
            {
                string employeeId = _employeeId;
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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Call the API to send the tax certificate email
                    var result = ((RetrieveReportSvc)channel).sendTaxCertificateEmailAsync(
                                new sendTaxCertificateEmail(callContext, employeeId, _payPeriodYear)).Result;

                    // Check the result and set the response message
                    //if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent Successfully")
                    //{
                    //    responseMessage = "Email sent Successfully";
                    //}

                    responseMessage = result.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return responseMessage;
        }

        //public MemoryStream attendaneRegisterReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        //{
        //    MemoryStream reportStream = null; // Ensure there's something to return

        //    try
        //    {
        //        string employeeId = _employeeId;
        //        DateTime fromDate = _fromDate;
        //        DateTime toDate = _toDate;
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();

        //        var client = new RetrieveReportSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext
        //            {
        //                MessageId = Guid.NewGuid().ToString(),
        //                Company = dataAreaId
        //            };

        //            // Assuming this API returns report content (you should replace this if it's different)
        //            var result = ((RetrieveReportSvc)channel).sendTaxCertificateEmailAsync(
        //                        new sendTaxCertificateEmail(callContext, employeeId, fromDate, toDate)).Result;

        //            if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent successfully")
        //            {
        //                // For demo: return a dummy stream
        //                byte[] dummyBytes = System.Text.Encoding.UTF8.GetBytes("Email sent successfully");
        //                reportStream = new MemoryStream(dummyBytes);
        //            }
        //            else
        //            {
        //                byte[] errorBytes = System.Text.Encoding.UTF8.GetBytes("Email sending failed");
        //                reportStream = new MemoryStream(errorBytes);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);

        //        // Optional: return exception message as stream
        //        byte[] exceptionBytes = System.Text.Encoding.UTF8.GetBytes("Exception: " + ex.Message);
        //        reportStream = new MemoryStream(exceptionBytes);
        //    }

        //    return reportStream;
        //}

        //mod

        public MemoryStream viewSalarySlipReport_del(string _employeeId, string _payPeriodCode)
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

                    var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipReportAsync(
                                new RetrieveSalarySlipReport(callContext, employeeId, payPeriodCode)).Result;

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

        public MemoryStream viewTaxCertificateReport(string _employeeId, int _payPeriodYear)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                string results = string.Empty;
                string employeeId = _employeeId;
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
                                new RetrieveTaxCertificateReport(callContext, employeeId, _payPeriodYear)).Result;

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

                    //var result = ((RetrieveReportSvc)channel).viewAttendanceInOutSheetAsync(
                    //            new viewAttendanceInOutSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    //memoryStream = result.result;
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

                    //var result = ((RetrieveReportSvc)channel).viewTimeSheetAsync(
                    //            new viewTimeSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    //memoryStream = result.result;
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

        public MemoryStream viewTransferOrderOverview(bool _showlines, bool _showReservation, bool _showTaxInformation, string _viewconfigid, string _inventlocationId, string _invenSizeid, string _inventStyleId, string _inventColorid, string _inventStatus, string _licensePlateId, string _wmsLocationId, string _transferId = "")
        {
            MemoryStream memoryStream = new MemoryStream();
            bool showlines = _showlines;
            bool showReservation = _showReservation;
            bool showTaxInformation = _showTaxInformation;
            string viewInventconfigid = _viewconfigid;
            string viewinventlocationid = _inventlocationId;
            string viewinventSizeid = _invenSizeid;
            string viewinventStyleid = _inventStyleId;
            string viewwmslocation = _wmsLocationId;
            string viewinventcolorid = _inventColorid;
            string viewinventstatus = _inventStatus;
            string viewinventlicenseplate = _licensePlateId;
            string transferId = _transferId;

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

                    // 🔁 Call the full method with all parameters
                    var result = ((RetrieveReportSvc)channel).viewTransferOrderOverViewReportAsync(
                        new viewTransferOrderOverViewReport(callContext, viewinventcolorid, viewinventlocationid, viewinventSizeid, viewinventstatus, viewinventStyleid, viewinventlicenseplate, showlines, showReservation, showTaxInformation, transferId,viewInventconfigid, viewwmslocation)).Result;

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

                    //var result = ((RetrieveReportSvc)channel).viewPreTimeSheetAsync(
                    //            new viewPreTimeSheet(callContext, employeeId, fromDate, reportees, toDate)).Result;

                    //memoryStream = result.result;
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

        public MemoryStream viewOnHandInventory(string _viewconfigid, string _inventlocationId, string _invenSizeid, string _inventStyleId, string _inventColorid, string _inventStatus, string _inventSerialId, string _licensePlateId, string _wmsLocationId, string _batchId, string _ownername, string _siteId, string _versionId, string _ItemId ="")
        {
            MemoryStream memoryStream = new MemoryStream();
            string viewInventconfigid = _viewconfigid;
            string viewinventlocationid = _inventlocationId;
            string viewinventSizeid = _invenSizeid;
            string viewinventStyleid = _inventStyleId;
            string viewwmslocation = _wmsLocationId;
            string viewinventcolorid = _inventColorid;
            string viewinventstatus = _inventStatus;
            string viewinventlicenseplate = _licensePlateId;
            string viewbatchId = _batchId;
            string viewOwnerName = _ownername;
            string viewSiteId = _siteId;
            string viewInventSerialId = _inventSerialId;
            string viewVersionId = _versionId;
            string ItemId = _ItemId;

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

                    // 🔁 Call the full method with all parameters
                    var result = ((RetrieveReportSvc)channel).viewOnHandInventoryAsync(
                        new viewOnHandInventory(callContext, ItemId ,viewInventconfigid, viewbatchId, viewinventcolorid, viewinventlocationid, viewOwnerName, viewInventSerialId, viewSiteId, viewinventSizeid, viewinventstatus, viewinventStyleid, viewVersionId, viewinventlicenseplate, viewwmslocation)).Result;

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

        
        public MemoryStream viewEmployeeWiseAttendanceDetailReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
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

                    var result = ((RetrieveReportSvc)channel).viewTASEmployeeWiseDetailAsync(
                                new viewTASEmployeeWiseDetail(callContext, employeeId, fromDate, toDate)).Result;


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
        public string emailEmployeeWiseAttendanceDetailReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            string responseMessage = "Email not sent successfully";  // Default failure message
            try
            {
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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Call the D365 API to send the salary slip email
                    var result = ((RetrieveReportSvc)channel).sendTASEmployeeWiseAttendanceDetailReportAsync(
                                new sendTASEmployeeWiseAttendanceDetailReport(callContext, employeeId, fromDate, toDate)).Result;

                    // Check the result and set the response message
                    if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent successfully")
                    {
                        responseMessage = "Email sent successfully";
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

            return responseMessage;
        }

        public string emailEmployeeWiseAttendanceSummaryReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            string responseMessage = "Email not sent successfully";  // Default failure message
            try
            {
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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Call the D365 API to send the salary slip email
                    var result = ((RetrieveReportSvc)channel).sendTASEmployeeWiseAttendanceSummaryReportAsync(
                                new sendTASEmployeeWiseAttendanceSummaryReport(callContext, employeeId, fromDate, toDate)).Result;

                    // Check the result and set the response message
                    if (!string.IsNullOrEmpty(result.result) && result.result == "Email sent successfully")
                    {
                        responseMessage = "Email sent successfully";
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

            return responseMessage;
        }
        public MemoryStream viewEmployeeWiseAttendanceSummaryReport(string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
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

                    var result = ((RetrieveReportSvc)channel).viewTASEmployeeAttendanceSummaryAsync(
                                new viewTASEmployeeAttendanceSummary(callContext, employeeId, fromDate, toDate)).Result;
                    //var result = ((RetrieveReportSvc)channel).RetrieveSalarySlipNewReportAsync(
                    //            new RetrieveSalarySlipNewReport(callContext, employeeId, payPeriodCode)).Result;

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

        public MemoryStream viewInventTransferJournalReport(string _viewconfigid, string _inventlocationId, string _invenSizeid, string _inventStyleId, string _inventColorid, string _inventStatus, string _inventSerialId, string _licensePlateId, string _wmsLocationId, string _batchId, string _ownername, string _siteId, string _versionId, string _journalID = "")
        {
            MemoryStream memoryStream = new MemoryStream();
            string viewInventconfigid = _viewconfigid;
            string viewinventlocationid = _inventlocationId;
            string viewinventSizeid = _invenSizeid;
            string viewinventStyleid = _inventStyleId;
            string viewwmslocation = _wmsLocationId;
            string viewinventcolorid = _inventColorid;
            string viewinventstatus = _inventStatus;
            string viewinventlicenseplate = _licensePlateId;
            string viewbatchId = _batchId;
            string viewOwnerName = _ownername;
            string viewSiteId = _siteId;
            string viewInventSerialId = _inventSerialId;
            string viewVersionId = _versionId;
            string journalId = _journalID;

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

                    // 🔁 Call the full method with all parameters
                    var result = ((RetrieveReportSvc)channel).viewInventTransferJournalReportAsync(
                        new viewInventTransferJournalReport(callContext, journalId, viewInventconfigid, viewbatchId, viewinventcolorid, viewinventlocationid, viewOwnerName, viewInventSerialId, viewSiteId, viewinventSizeid, viewinventstatus, viewinventStyleid, viewVersionId, viewinventlicenseplate, viewwmslocation)).Result;

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


        public MemoryStream viewTransferJournalShipmentReport(DateTime _postingDate, string _viewconfigid, string _inventlocationId, string _invenSizeid, string _inventStyleId, string _inventColorid, string _inventStatus, string _inventSerialId, string _licensePlateId, string _wmsLocationId, string _batchId, string _ownername, string _siteId, string _versionId, string _transferId, string _voucherId)
        {
            MemoryStream memoryStream = new MemoryStream();
            string viewInventconfigid = _viewconfigid;
            string viewinventlocationid = _inventlocationId;
            string viewinventSizeid = _invenSizeid;
            string viewinventStyleid = _inventStyleId;
            string viewwmslocation = _wmsLocationId;
            string viewinventcolorid = _inventColorid;
            string viewinventstatus = _inventStatus;
            string viewinventlicenseplate = _licensePlateId;
            string viewbatchId = _batchId;
            string viewOwnerName = _ownername;
            string viewSiteId = _siteId;
            string viewInventSerialId = _inventSerialId;
            string viewVersionId = _versionId;
           

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

                    // 🔁 Call the full method with all parameters
                    var result = ((RetrieveReportSvc)channel).viewTransferJournalShipmentAsync(
                        new viewTransferJournalShipment(callContext, _postingDate, _transferId, viewInventconfigid, viewbatchId, viewinventcolorid, viewinventlocationid, viewOwnerName, viewInventSerialId, viewSiteId, viewinventSizeid, viewinventstatus, viewinventStyleid, viewVersionId, viewinventlicenseplate, viewwmslocation, _voucherId)).Result;

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


        public MemoryStream viewTransferJournalReceiveReport(DateTime _postingDate, string _viewconfigid, string _inventlocationId, string _invenSizeid, string _inventStyleId, string _inventColorid, string _inventStatus, string _inventSerialId, string _licensePlateId, string _wmsLocationId, string _batchId, string _ownername, string _siteId, string _versionId, string _transferId, string _voucherId)
        {
            MemoryStream memoryStream = new MemoryStream();
            string viewInventconfigid = _viewconfigid;
            string viewinventlocationid = _inventlocationId;
            string viewinventSizeid = _invenSizeid;
            string viewinventStyleid = _inventStyleId;
            string viewwmslocation = _wmsLocationId;
            string viewinventcolorid = _inventColorid;
            string viewinventstatus = _inventStatus;
            string viewinventlicenseplate = _licensePlateId;
            string viewbatchId = _batchId;
            string viewOwnerName = _ownername;
            string viewSiteId = _siteId;
            string viewInventSerialId = _inventSerialId;
            string viewVersionId = _versionId;


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

                    // 🔁 Call the full method with all parameters
                    var result = ((RetrieveReportSvc)channel).viewTransferJournalReceiveAsync(
                        new viewTransferJournalReceive(callContext, _postingDate, _transferId, viewInventconfigid, viewbatchId, viewinventcolorid, viewinventlocationid, viewOwnerName, viewInventSerialId, viewSiteId, viewinventSizeid, viewinventstatus, viewinventStyleid, viewVersionId, viewinventlicenseplate, viewwmslocation, _voucherId)).Result;

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


