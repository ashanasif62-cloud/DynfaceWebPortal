using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ESSWorkflowSvcReference;
using PortalIntegration.TASEmployeeAdjustmentsLineSvcReference;
using PortalIntegration.TASEmployeeAdjustmentsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using CallContext = PortalIntegration.TASEmployeeAdjustmentsLineSvcReference.CallContext;
using GeneralContract = PortalIntegration.TASEmployeeAdjustmentsLineSvcReference.GeneralContract;

namespace PortalIntegration
{
    public class TASEmployeeAdjustmentLines
    {
        private readonly string serviceName = "TASEmployeeAdjustmentLinesGroup";
        public readonly string tablename = "TASEmployeeAdjustmentLines";
        public string actionItem = "TASEmployeeAdjustmentLines_ListPage";
        private SysOperationResult_BOL objBOL = new SysOperationResult_BOL();



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

                var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
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
                        string clockIn = dataRow["ClockIn"].ToString();
                        string clockOut = dataRow["ClockOut"].ToString();
                        string AttendanceDate = dataRow["AttendanceDate"].ToString();
                        string Remarks = dataRow["Remarks"].ToString();
                        string ReasonCode = dataRow["ReasonCode"].ToString();


                        TASEmployeeAdjustmentLinesSvcContract tASEmployeeAdjustmentLinesSvc = new TASEmployeeAdjustmentLinesSvcContract();
                        // TASEmployeeRosterSvcReferenceTASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract employeeRosterContract = new TASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract();
                        tASEmployeeAdjustmentLinesSvc.EmployeeId = dataRow["EmployeeId"].ToString();
                        tASEmployeeAdjustmentLinesSvc.AttendanceDate = DateTime.Parse(AttendanceDate);
                        tASEmployeeAdjustmentLinesSvc.ClockIn = DateTime.Parse(clockIn);
                        tASEmployeeAdjustmentLinesSvc.ClockOut = DateTime.Parse(clockOut);
                        tASEmployeeAdjustmentLinesSvc.ShiftId = shiftId.ToString();
                       // tASEmployeeAdjustmentLinesSvc.GenerationType = "Dynaface";


                        GeneralContract results = ((TASEmployeeAdjustmentLinesSvc)channel).CreateAsync(new TASEmployeeAdjustmentsLineSvcReference.Create (callContext, tASEmployeeAdjustmentLinesSvc)).Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                        if (objBOL != null && objBOL.isSuccess)
                        {
                            isAnySuccess = true;
                        }


                        SysLogUserActivity.logUserActivity(tablename, actionItem, ActionType.Create, objBOL);
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


        public SysOperationResult_BOL pREmployeeAdjustmentRequest_Submit(long[] _recordId)
        {
            try
            {
                long[] recId = _recordId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();


                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {

                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((TASEmployeeAdjustmentLinesSvc)channel).submitEmployeeAdjustmentAsync(new submitEmployeeAdjustment(callContext, recId)).Result.result;
                  //  GeneralContract[] results = { new GeneralContract() };
                    objBOL = SysOperationResults.operationResults(results);
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





        public DataTable retrieveAll(string fromDate, string toDate)
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
                var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
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
                    var results = ((TASEmployeeAdjustmentLinesSvc)channel).retrieveAllAdjustmentlinesAsync(new retrieveAllAdjustmentlines(callContext, _dataAreaId, employeeId, parsedFromDate, parsedToDate)).Result.result;

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

        public DataTable retrieveReasonCode()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();
                // Parse the date strings into DateTime objects
              

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
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
                    var results = ((TASEmployeeAdjustmentLinesSvc)channel).lookupReasonCodeAsync(new lookupReasonCode(callContext)).Result.result;

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

        //public DataTable retrieveEmployeeAdjustment(string attendanceDate)
        //{
        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //        string employeeId = SessionVariables.getCurrentEmployeeId();
        //        DateTime parsedAttendanceDate;
        //        if (!DateTime.TryParse(attendanceDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedAttendanceDate))
        //        {
        //            parsedAttendanceDate = DateTime.Today; // fallback to today
        //        }
        //        DateTime dateOnly = parsedAttendanceDate.Date;
        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            // Pass the _dataAreaId parameter to the RetrieveAll method
        //            string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)
        //            var results = ((TASEmployeeAdjustmentLinesSvc)channel).retrieveEmployeeAdjustmentlinesAsync(new retrieveEmployeeAdjustmentlines(callContext, employeeId, dateOnly  )).Result.result;

        //            DataTable dataTable = RetrieveDatatable.createDataTable(results);
        //            return dataTable;

        //        }
        //    }
        //    catch (Exception ex
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return createDataTable();
        //    }
        //    finally
        //    { }
        //}



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
                var client = new TASEmployeeAdjustmentLinesSvcClient(binding, endpointAddress);
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
                    var results = ((TASEmployeeAdjustmentLinesSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId, parsedFromDate, parsedToDate)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new TASEmployeeAdjustmentLinesSvcContract[] { });
            dataTable.TableName = tablename;
            return dataTable;
        }

    }
}
