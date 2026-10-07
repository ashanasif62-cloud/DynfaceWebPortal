using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TASRosterChangeRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
    public class TASRosterChangeRequestsSvc
    {

        private readonly string serviceName = "TASRosterChangeRequestSvcGroup";
        public string tableName = "TASRosterRequestHeader";
        public string actionItem = "TASRosterChangeRequest";
        private SysOperationResult_BOL objBOL = new SysOperationResult_BOL();


        //public DataTable retrieveAllwithFilters(string fromDate, string toDate)
        //{
        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //        string employeeId = SessionVariables.getCurrentEmployeeId();
        //        // Parse the date strings into DateTime objects
        //        DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            string _dataAreaId = dataAreaId;

        //            // Pass DateTime objects instead of strings
        //            var results = ((TASRosterChangeRequestSvc)channel).retrieveByEmployeeIdAsync(new retrieveByEmployeeId(callContext, employeeId, parsedFromDate, parsedToDate)).Result.result;

        //            DataTable dataTable = RetrieveDatatable.createDataTable(results);
        //            return dataTable;
        //            //return createDataTable();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return createDataTable();
        //    }
        //}

        public DataTable retrieveAllwithFilters(string fromDate, string toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                       DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((TASRosterChangeRequestSvc)channel).retrieveByEmployeeIdAsync(new retrieveByEmployeeId(callContext, employeeId,parsedFromDate, parsedToDate)).Result.result;

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

        public DataTable retrieveEmployeeId(string searchText = "", int page = 1)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Call the lookup method with Search + Page
                    var results = ((TASRosterChangeRequestSvc)channel)
                        .lookupEmployeeIdAsync(new lookupEmployeeId(callContext, page, searchText ))
                        .Result.result;

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
        }


        //public DataTable retrieveAllwithFilters(string fromDate, string toDate)
        //{
        //    string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveAllwithFilters)}";
        //    SysErrorLog objErrorLog = new SysErrorLog();

        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //        string employeeId = SessionVariables.getCurrentEmployeeId();

        //        if (string.IsNullOrWhiteSpace(employeeId))
        //        {
        //            objErrorLog.write($"{currentMethodName} - employeeId from session is NULL/EMPTY. dataAreaId={dataAreaId}", new Exception("Missing employeeId"));
        //            return createDataTable();
        //        }

        //        if (string.IsNullOrWhiteSpace(fromDate) || string.IsNullOrWhiteSpace(toDate))
        //        {
        //            objErrorLog.write($"{currentMethodName} - fromDate/toDate textbox value is empty. fromDate='{fromDate}', toDate='{toDate}'", new Exception("Missing date filter"));
        //            return createDataTable();
        //        }

        //        // Parse the date strings into DateTime objects
        //        DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //        // TEMP DEBUG LOG - remove once confirmed working
        //        objErrorLog.write($"{currentMethodName} - Calling AX with employeeId='{employeeId}', dataAreaId='{dataAreaId}', fromDate={parsedFromDate:yyyy-MM-dd}, toDate={parsedToDate:yyyy-MM-dd}", new Exception("DEBUG TRACE"));

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            var response = ((TASRosterChangeRequestSvc)channel)
        //                .retrieveByEmployeeIdAsync(new retrieveByEmployeeId(callContext, employeeId, parsedFromDate, parsedToDate))
        //                .Result;

        //            var results = response?.result;

        //            // TEMP DEBUG LOG - remove once confirmed working
        //            objErrorLog.write($"{currentMethodName} - Service returned {(results == null ? "NULL" : results.Length.ToString())} row(s)", new Exception("DEBUG TRACE"));

        //            if (results == null || results.Length == 0)
        //            {
        //                return createDataTable();
        //            }

        //            DataTable dataTable = RetrieveDatatable.createDataTable(results);
        //            dataTable.TableName = tableName;
        //            return dataTable;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
        //        return createDataTable();
        //    }
        //}



        public DataTable retrieveLines(long recid )
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASRosterChangeRequestSvc)channel).retrieveLinesAsync(new retrieveLines(callContext, recid)).Result.result;

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


        public DataTable retrieveShiftId()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASRosterChangeRequestSvc)channel).retrieveShiftIdAsync(new retrieveShiftId(callContext, 0, 0)).Result.result;

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


        public long createRoster(string employeeId, string shiftId, string fromDate, string toDate,
                           string requestDate, string shiftStartTime, string shiftEndTime, long recId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedRequestDate = DateTime.ParseExact(requestDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    TASRosterChangeRequestSvcContract contract = new TASRosterChangeRequestSvcContract();
                    contract.employeeId = employeeId;
                    contract.shiftId = shiftId;
                    contract.fromDate = parsedFromDate;
                    contract.toDate = parsedToDate;
                    contract.requestDate = parsedRequestDate;
                    contract.RecId = recId;
                    // Only set these if your contract actually exposes them
                    // contract.ShiftStartTime = ...
                    // contract.ShiftEndTimeTime = ...

                    var result = ((TASRosterChangeRequestSvc)channel)
                        .changeRosterAsync(new changeRoster(callContext, contract)).Result.result;

                    return result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return 0;
            }
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
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((TASRosterChangeRequestSvc)channel).deleteAsync(new delete(callContext, _recordsRecId)).Result.result;
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



        public SysOperationResult_BOL deleteline(long[] _recordsRecId)
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
                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((TASRosterChangeRequestSvc)channel).deletelineAsync(new deleteline(callContext, _recordsRecId)).Result.result;
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



        public SysOperationResult_BOL RosterChange_Submit(long[] _recordsRecId)
        {
            try
            {
                long[] recordsRecId = _recordsRecId;

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TASRosterChangeRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Call the service once and store the result
                    var response = ((TASRosterChangeRequestSvc)channel).submitRosterChangeRequestAsync(
                        new submitRosterChangeRequest(callContext, recordsRecId)).Result;

                    // Get the results
                    GeneralContract[] results = response.result;

                    // Extract infolog message if available
                    string infologMessage = string.Empty;
                    if (response?.Infolog?.Entries != null && response.Infolog.Entries.Length > 0)
                    {
                        infologMessage = response.Infolog.Entries[0].Message;
                    }

                    objBOL = SysOperationResults.operationResults(results);

                    // Only override message with infolog message if result failed
                    if (!objBOL.isSuccess && !string.IsNullOrEmpty(infologMessage))
                    {
                        objBOL.Message = infologMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                // Unwrap to get the real error message
                Exception innerEx = ex;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;

                objBOL.isSuccess = false;
                objBOL.AlertType = AlertType.Error.ToString();
                objBOL.Message = innerEx.Message;
            }
            finally
            { }
            return objBOL;
        }




        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new TASRosterChangeRequestSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
