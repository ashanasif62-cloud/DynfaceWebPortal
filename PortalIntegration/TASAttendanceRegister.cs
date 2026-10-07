using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TASAttendanceRegisterSvcReference;


namespace PortalIntegration
{
    public class TASAttendanceRegister
    {
        private readonly string serviceName = "TASAttendanceRegisterSvcGroup";
        public string tableName = "TASAttendanceRegister";
        public string actionItem = "TASAttendaneRegister_ListPage";
        bool isAnySuccess = false;




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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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
                        string shiftId = dataRow["ShiftId"].ToString();
                        TASAttendanceRegisterSvcContract AttendanceRegisterSvcContract = new TASAttendanceRegisterSvcContract();
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

        public DataTable getRegisterDetailByDate(DateTime _fromDate, DateTime _toDate)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASAttendanceRegisterSvc)channel).getRegisterDetailsByDateAsync(new getRegisterDetailsByDate(callContext, employeeid, _fromDate, _toDate)).Result.result;

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
            }
            finally
            { }
            return new DataTable();
        }



        //public SysOperationResult_BOL markAttendance(DataTable _objDT)
        //{
        //    SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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

        //            // List to hold the contract data
        //            List<PortalIntegration.TASAttendanceRegisterSvcReference.TASAttendanceRegisterSvcContract> attendanceList = new List<PortalIntegration.TASAttendanceRegisterSvcReference.TASAttendanceRegisterSvcContract>();

        //            foreach (DataRow dataRow in _objDT.Rows)
        //            {
        //                // Mapping DataRow to TASAttendanceRegisterSvcContract (adjust property names based on service contract)
        //                var attendanceContract = new PortalIntegration.TASAttendanceRegisterSvcReference.TASAttendanceRegisterSvcContract
        //                {
        //                    // Assuming these properties are defined in your service contract. Adjust based on actual contract.
        //                    EMPLOYEEID = dataRow["EMPLOYEEID"].ToString(),
        //                    EmployeeName = dataRow["EmployeeName"].ToString(),
        //                    ClockIn = ConvertClockInOutToMinutes(dataRow["ClockIn"].ToString()),
        //                    ClockOut = ConvertClockInOutToMinutes(dataRow["ClockOut"].ToString())
        //                };

        //                attendanceList.Add(attendanceContract);
        //            }

        //            // Ensure to convert to an array of the correct contract type
        //            PortalIntegration.TASAttendanceRegisterSvcReference.TASAttendanceRegisterSvcContract[] attendanceArray = attendanceList.ToArray();

        //            // GeneralContract is assumed to be the response contract, ensure correct namespace
        //            PortalIntegration.TASAttendanceRegisterSvcReference.GeneralContract generalContract = new PortalIntegration.TASAttendanceRegisterSvcReference.GeneralContract();

        //            // Make the actual service call
        //           // var result = client.markAttendance(callContext, attendanceArray, out generalContract);

        //            // Check result and set success or failure message (adjust based on actual GeneralContract)
        //            objBOL.isSuccess = generalContract != null && generalContract.IsSuccess;
        //          //  objBOL.Message = generalContract != null ? generalContract.InfologMessage : "Unknown error";
        //        }

        //        // Log user activity
        //        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }

        //    return objBOL;
        //}
        public SysOperationResult_BOL markAttendance(DataTable _objDT)
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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string employeeId = dataRow["EmployeeId"]?.ToString();
                        string employeeName = dataRow["EmployeeName"]?.ToString();
                        string clockInStr = dataRow["ClockIn"]?.ToString();
                        string clockOutStr = dataRow["ClockOut"]?.ToString();
                        string attendanceDateStr = dataRow["AttendanceDate"]?.ToString();
                        string remarks = dataRow["Remarks"]?.ToString();


                        if (string.IsNullOrWhiteSpace(employeeId) ||
                            string.IsNullOrWhiteSpace(clockInStr) ||
                            string.IsNullOrWhiteSpace(clockOutStr) ||
                            string.IsNullOrWhiteSpace(attendanceDateStr))
                        {
                            continue; // Skip incomplete rows
                        }

                        if (!DateTime.TryParse(attendanceDateStr, out DateTime attendanceDate))
                        {
                            continue; // Skip if AttendanceDate is invalid
                        }

                        // Convert time strings to seconds
                        int clockInSeconds = ConvertClockInOutToSeconds(clockInStr);
                        int clockOutSeconds = ConvertClockInOutToSeconds(clockOutStr);

                        // Initialize attendance contract
                        TASAttendanceRegisterSvcContract attRegister = new TASAttendanceRegisterSvcContract
                        {
                            EMPLOYEEID = employeeId,
                            EmployeeName = employeeName,
                            ClockIn = clockInSeconds,
                            ClockOut = clockOutSeconds,
                            AttendanceDate = attendanceDate,
                            Remarks = remarks// ISO format
                        };
                        attRegister.GenerationType =
                           (TASGenerationType)Enum.Parse(typeof(TASGenerationType), "Dynaface", true);

                        // Call the service
                        var request = new markAttendance(callContext, attRegister);
                        GeneralContract results = ((TASAttendanceRegisterSvc)channel).markAttendanceAsync(request).Result.result;
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
                objBOL.Message = "Employee Adjustments is created successfully.";
            }
            else if (string.IsNullOrEmpty(objBOL.Message))
            {
                objBOL.Message = "Employee Adjustments creation failed or no data processed.";
                objBOL.isSuccess = false;
            }
            return objBOL;
        }
        public SysOperationResult_BOL editAttendance(DataTable _objDT)
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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string employeeId = dataRow["EmployeeId"]?.ToString();
                        string employeeName = dataRow["EmployeeName"]?.ToString();
                        string clockInStr = dataRow["ClockIn"]?.ToString();
                        string clockOutStr = dataRow["ClockOut"]?.ToString();
                        string attendanceDateStr = dataRow["AttendanceDate"]?.ToString();
                        string remarks = dataRow["Remarks"]?.ToString();
                        string generationType = "Dynaface";

                        if (string.IsNullOrWhiteSpace(employeeId) ||
                            string.IsNullOrWhiteSpace(clockInStr) ||
                            string.IsNullOrWhiteSpace(clockOutStr) ||
                            string.IsNullOrWhiteSpace(attendanceDateStr)) 
                        {
                            continue; // Skip incomplete rows
                        }

                        if (!DateTime.TryParse(attendanceDateStr, out DateTime attendanceDate))
                        {
                            continue; // Skip if AttendanceDate is invalid
                        }

                        // Convert time strings to seconds
                        int clockInSeconds = ConvertClockInOutToSeconds(clockInStr);
                        int clockOutSeconds = ConvertClockInOutToSeconds(clockOutStr);

                        // Initialize attendance contract
                        TASAttendanceRegisterSvcContract attRegister = new TASAttendanceRegisterSvcContract
                        {
                            EMPLOYEEID = employeeId,
                            EmployeeName = employeeName,
                            ClockIn = clockInSeconds,
                            ClockOut = clockOutSeconds,
                            AttendanceDate = attendanceDate,// ISO format
                            Remarks = remarks,
                        };
                        attRegister.GenerationType =
                            (TASGenerationType)Enum.Parse(typeof(TASGenerationType), "Dynaface", true);

                        // Call the service
                        var request = new EditAttendance(callContext, attRegister);
                        GeneralContract results = ((TASAttendanceRegisterSvc)channel).EditAttendanceAsync(request).Result.result;
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
                objBOL.Message = "Attendance is edit successfully";
            }
            else if (string.IsNullOrEmpty(objBOL.Message))
            {
                objBOL.Message = "Failed";
                objBOL.isSuccess = false;
            }
            return objBOL;
        }

        private int ConvertClockInOutToSeconds(string timeString)
        {
            if (TimeSpan.TryParse(timeString, out TimeSpan time))
            {
                return (int)time.TotalSeconds;
            }
            return 0; // or throw error/log if needed
        }


        private int ConvertClockInOutToMinutes(string time)
        {
            DateTime timeParsed;
            if (DateTime.TryParse(time, out timeParsed))
            {
                return (int)(timeParsed - DateTime.MinValue).TotalMinutes;
            }
            return 0; // Default value if invalid format
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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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
                    var results = ((TASAttendanceRegisterSvc)channel).retrieveAllwithFiltersAsync(new retrieveAllwithFilters(callContext, _dataAreaId, employeeId, parsedFromDate, parsedToDate)).Result.result;

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


        public DataTable retriveEmployeeReportees(string fromDate, string toDate)
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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
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
                    var results = ((TASAttendanceRegisterSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId, parsedFromDate, parsedToDate)).Result.result;

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


        public DataTable retriveEmployeeID(string fromDate, string toDate, string employeeId = null)
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
                var client = new TASAttendanceRegisterSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASAttendanceRegisterSvc)channel)
                        .retrieveAllwithFiltersAsync(new retrieveAllwithFilters(callContext, dataAreaId, employeeId, parsedFromDate, parsedToDate))
                        .Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = $"{this.GetType().FullName}.{nameof(retriveEmployeeID)}";
                objErrorLog.write($"{currentMethodName} - Error retrieving data", ex);
                return createDataTable();
            }
        }




        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new TASAttendanceRegisterSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
        }
}
