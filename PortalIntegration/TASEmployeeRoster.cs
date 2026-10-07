using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TASEmployeeRosterSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace PortalIntegration
{
    public class TASEmployeeRoster
    {
        private readonly string serviceName = "TASEmployeeRosterGroup";
        public string tableName = "TASROSTER";
        public string actionItem = "TASEmployeeRoster_ListPage";
        public string actionitem = "TASEmployeeRoster_Create";

        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            bool isAnySuccess = false;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
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
                        //long employeeId = 0;
                        //Int64.TryParse(dataRow["EmployeeId"].ToString(), out employeeId);
                        //long shiftId = 0;
                        //Int64.TryParse(dataRow["ShiftId"].ToString(), out shiftId);
                        string shiftId = dataRow["ShiftId"].ToString();
                        string fromDate = dataRow["fromDate"].ToString();
                        string toDate = dataRow["toDate"].ToString();
                        string type = dataRow["GENERATIONTYPE"].ToString();  




                        TASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract employeeRosterContract = new TASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract();
                        employeeRosterContract.EMPLOYEEID = dataRow["EmployeeId"].ToString();
                       // employeeRosterContract.EMPLOYEEID = employeeId.ToString();
                        employeeRosterContract.fromDate = DateTime.Parse(fromDate);
                        employeeRosterContract.toDate = DateTime.Parse(toDate);
                        employeeRosterContract.shiftId =  shiftId.ToString();
                        employeeRosterContract.DataAreaId = dataAreaId;// shiftId.ToString();
                        employeeRosterContract.GenerationType = type.ToString();// shiftId.ToString();
                        employeeRosterContract.Type = TASEmployeeRosterSvcReference.TASProfileType.StandardTime;

                        GeneralContract results = ((TASEmployeeRosterSvc)channel).createAsync(new create(callContext, employeeRosterContract)).Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                        if (objBOL != null && objBOL.isSuccess)
                        {
                            isAnySuccess = true;
                        }


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
            if (isAnySuccess)
            {
                objBOL.Message = "Roster is created successfully.";
            }
            else if (string.IsNullOrEmpty(objBOL.Message))
            {
                objBOL.Message = "Roster creation failed or no data processed.";
                objBOL.isSuccess = false;
            }
            return objBOL;
        }



        public SysOperationResult_BOL update(DataTable _objDT, long _recId)
        {
            long recId = _recId;
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
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
                        string shiftCode = dataRow["ShiftCode"].ToString();
                        DateTime rosterDate = dataRow["RosterDate"].ToString().toDateTime();
                        string shiftType = dataRow["ShiftType"].ToString();

                        TASEmployeeRosterSvcContract employeeRosterContract = new TASEmployeeRosterSvcContract();

              //          employeeRosterContract.ShiftCode = shiftCode;
              //          employeeRosterContract.ShiftDate = rosterDate;
 //                       employeeRosterContract.ProfileType = (TASEmployeeRosterSvcReference.TASProfileType)Enum.Parse(
 //    typeof(TASEmployeeRosterSvcReference.TASProfileType), shiftType
 //);

                   //     employeeRosterContract.RecId = recId;

                   //     GeneralContract results = ((TASEmployeeRosterSvc)channel).updateAsync(new update(callContext, employeeRosterContract)).Result.result;
                   //    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

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
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((TASEmployeeRosterSvc)channel).deleteAsync(new delete(callContext, _recordsRecId)).Result.result;
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

        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
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

                    var results = ((TASEmployeeRosterSvc)channel).retrieveAllAsync(new retrieveAll(callContext, _dataAreaId)).Result.result;

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

        public DataTable retrieveAllEmployees()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
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

                    var results = ((TASEmployeeRosterSvc)channel).retrieveEmployeesAsync(new retrieveEmployees(callContext)).Result.result;

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
        public DataTable retrieveShiftIds()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
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

                    var results = ((TASEmployeeRosterSvc)channel).retrieveShiftIdsAsync(new retrieveShiftIds(callContext)).Result.result;

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
        public DataTable retrieveAllwithFilters(string fromDate, string toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();
                // Parse the date strings into DateTime objects
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    string _dataAreaId = dataAreaId;

                    // Pass DateTime objects instead of strings
                    var results = ((TASEmployeeRosterSvc)channel).retrieveAllwithFiltersAsync(new retrieveAllwithFilters(callContext, _dataAreaId,employeeId, parsedFromDate, parsedToDate)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                    //return createDataTable();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveAll)}";
                objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
                return createDataTable();
            }
        }


        private void LogError(string errorType, Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveAll)}";
            objErrorLog.write($"{currentMethodName} - {errorType}", ex);
        }

        private DataTable CreateEmptyDataTable()
        {
            // Implement your empty DataTable creation logic
            return new DataTable();
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new TASEmployeeRosterSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
        public DataRow retrieveEmployeeRowById(string employeeId)
        {
            // You can fetch from DB or existing data source
            DataTable allEmployees = retrieveAllEmployees();
            DataRow[] result = allEmployees.Select($"EmployeeId = '{employeeId}'");

            return result.Length > 0 ? result[0] : null;
        }
        public DataTable findByRecordId(long _recordsRecId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASEmployeeRosterSvc)channel).findByRecordIdAsync(new findByRecordId(callContext, _recordsRecId)).Result.result;

                    // Ensure results is not null and is an IEnumerable<T>
                    if (results == null)
                    {
                        throw new InvalidOperationException("The results from the service call are null.");
                    }

                    // Create a DataTable from the results
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
                return RetrieveDatatable.createDataTable(new List<TASEmployeeRosterSvcContract>()); // Return an empty DataTable
            }
            finally
            {
                // Any cleanup code can go here if needed
            }
        }
        public SysOperationResult_BOL UpdateRecord(long recId, string employeeId, string employeeName, string flexClockInStartTime, string flexClockOutStartTime, string offDay, string shiftId)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Parse the time strings
                    DateTime flexClockInTime = DateTime.ParseExact(flexClockInStartTime, "h:mm:ss tt", CultureInfo.InvariantCulture);
                    DateTime flexClockOutTime = DateTime.ParseExact(flexClockOutStartTime, "h:mm:ss tt", CultureInfo.InvariantCulture);

                    // Convert times to seconds since midnight
                    int flexClockInSeconds = flexClockInTime.Hour * 3600 + flexClockInTime.Minute * 60 + flexClockInTime.Second;
                    int flexClockOutSeconds = flexClockOutTime.Hour * 3600 + flexClockOutTime.Minute * 60 + flexClockOutTime.Second;

                    TASEmployeeRosterSvcContract employeeRosterContract = new TASEmployeeRosterSvcContract
                    {
                        RecId = recId,
                        EMPLOYEEID = employeeId,
                        EmployeeName = employeeName,
                        FlexClockInStartTime = flexClockInSeconds,
                        FlexClockOutStartTime = flexClockOutSeconds,
                        OffDay = offDay,
                        shiftId = shiftId
                    };

                    GeneralContract results = ((TASEmployeeRosterSvc)channel).updateAsync(new update(callContext, employeeRosterContract)).Result.result;
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
        public DataTable retrieveClockDateTime(string _employeeId, string _attendanceDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                // Parse the date strings into DateTime objects
                DateTime parsedAttendanceDate = DateTime.ParseExact(_attendanceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASEmployeeRosterSvc)channel).retrieveClockDateTimeAsync(new retrieveClockDateTime(callContext, parsedAttendanceDate, _employeeId)).Result.result;

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

        public DataTable retrieveEmployeeReportees(string fromDate, string toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                // Parse the date strings into DateTime objects
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    string _dataAreaId = dataAreaId;

                    // Pass DateTime objects instead of strings
                    var results = ((TASEmployeeRosterSvc)channel).retrieveEmployeeReporteesAsync(new retrieveEmployeeReportees(callContext, employeeId, parsedFromDate, parsedToDate)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveAll)}";
                objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
                return createDataTable();
            }
        }



        public DataTable retrieveEmployeeData(string fromDate, string toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                // Parse the date strings into DateTime objects
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    string _dataAreaId = dataAreaId;

                    // Pass DateTime objects instead of strings
                    var results = ((TASEmployeeRosterSvc)channel).retrieveAllwithFiltersAsync(new retrieveAllwithFilters(callContext, dataAreaId, employeeId, parsedFromDate, parsedToDate)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveAll)}";
                objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
                return createDataTable();
            }
        }


        public DataTable retrieveEmployeeId(string fromDate, string toDate, string employeeId = null)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                // Use the passed employeeId, otherwise fallback to current user
                if (string.IsNullOrEmpty(employeeId))
                {
                    employeeId = SessionVariables.getCurrentEmployeeId();
                }

                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeRosterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASEmployeeRosterSvc)channel)
                        .retrieveAllwithFiltersAsync(new retrieveAllwithFilters(callContext, dataAreaId, employeeId, parsedFromDate, parsedToDate))
                        .Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = $"{this.GetType().FullName}.{nameof(retrieveEmployeeData)}";
                objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
                return createDataTable();
            }
        }


    }
}
