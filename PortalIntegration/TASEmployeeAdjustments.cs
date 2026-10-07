using BussinessObject;
using GeneralAuxiliary;
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
using CallContext = PortalIntegration.TASEmployeeAdjustmentsSvcReference.CallContext;
using GeneralContract = PortalIntegration.TASEmployeeAdjustmentsSvcReference.GeneralContract;

namespace PortalIntegration
{
    public class TASEmployeeAdjustments
    {
        private readonly string serviceName = "TASEmployeeAdjustments";
        public string tableName = "TASEmployeeAdjustment";
        public string actionItem = "TASEmployeeAdjustmentLines_ListPage";
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

                var client = new TASEmployeeAdjustmentSvcClient(binding, endpointAddress);
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
                        long employeeId = 0;
                        Int64.TryParse(dataRow["EmployeeId"].ToString(), out employeeId);
                        //long shiftId = 0;
                        //Int64.TryParse(dataRow["ShiftId"].ToString(), out shiftId);
                        string shiftId = dataRow["ShiftId"].ToString();
                        string clockIn = dataRow["ClockIn"].ToString();
                        string clockOut = dataRow["ClockOut"].ToString();
                        string AttendanceDate = dataRow["AttendanceDate"].ToString();



                        // TASEmployeeRosterSvcReferenceTASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract employeeRosterContract = new TASEmployeeRosterSvcReference.TASEmployeeRosterSvcContract();
                        TASEmployeeAdjustmentsSvcReference.TASEmployeeAdjustmentsSvcContract employeeAdjustmentsSvcContract = new TASEmployeeAdjustmentsSvcReference.TASEmployeeAdjustmentsSvcContract();

                        employeeAdjustmentsSvcContract.EmployeeId = employeeId.ToString();
                        employeeAdjustmentsSvcContract.AttendanceDate = DateTime.Parse(AttendanceDate);
                        employeeAdjustmentsSvcContract.ClockIn = DateTime.Parse(clockIn);
                        employeeAdjustmentsSvcContract.ClockOut= DateTime.Parse(clockOut);
                        employeeAdjustmentsSvcContract.ShiftId= shiftId.ToString();
                      

                        GeneralContract results = ((TASEmployeeAdjustmentSvc)channel).CreateAsync(new Create(callContext, employeeAdjustmentsSvcContract)).Result.result;

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


        public DataTable retrieveAllAdjustmentsHeader(string fromDate, string toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                // Parse the date strings into DateTime objects
                //DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;
                DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeAdjustmentSvcClient(binding, endpointAddress);
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

                    var results = ((TASEmployeeAdjustmentSvc)channel).retrieveAllAdjustmentswithFiltersAsync(new retrieveAllAdjustmentswithFilters(callContext, _dataAreaId, parsedFromDate, parsedToDate)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new TASEmployeeAdjustmentsSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
