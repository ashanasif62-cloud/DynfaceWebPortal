using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PREmployeeLeavesSvcReference;


using System;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PREmployeeLeaveRequest
    {
        private readonly string serviceName = "PREmployeeLeavesSvcGroup";

        public string tableName = "PREmployeeLeaveRequest";
        public string actionItem = "Employee Leave Request";

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

                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
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
                        decimal leaveDays = 0;
                        Decimal.TryParse(dataRow["LeaveDays"].ToString(), out leaveDays);
                        decimal Balance = 0;
                        Decimal.TryParse(dataRow["Balance"].ToString(), out Balance);
                        decimal quota = 0;
                        decimal eligibility = 0;

                        Decimal.TryParse(dataRow["Quota"]?.ToString(), out quota);
                        Decimal.TryParse(dataRow["Eligibility"]?.ToString(), out eligibility);

                        PREmployeeLeavesSvcContract employeeLeaveContract = new PREmployeeLeavesSvcContract();

                        employeeLeaveContract.LeaveCode = dataRow["LeaveCode"].ToString();//- CU
                        employeeLeaveContract.Reason = dataRow["Reason"].ToString();//- CU

                        
                        employeeLeaveContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        employeeLeaveContract.EmployeeName = dataRow["EmployeeName"].ToString();
                        employeeLeaveContract.LeaveDays = leaveDays;//- CU

                        employeeLeaveContract.LeaveCategory = dataRow["LeaveCategory"] is DBNull ? PRLeaveCategory.FullLeave :
                                                                (PRLeaveCategory)Enum.Parse(typeof(PRLeaveCategory), dataRow["LeaveCategory"].ToString());

                        employeeLeaveContract.LeaveReqDate = dataRow["LeaveReqDate"].ToString().toDateTime();
                        employeeLeaveContract.LeaveStartDate = Convert.ToDateTime(dataRow["LeaveStartDate"]);
                        employeeLeaveContract.LeaveEndDate =  Convert.ToDateTime(dataRow["LeaveEndDate"]);

                        employeeLeaveContract.LeaveDays = leaveDays;
                        employeeLeaveContract.Balance = Balance;
                        employeeLeaveContract.Quota = quota;
                        employeeLeaveContract.Eligibility = eligibility;
                        employeeLeaveContract.ReplacementWorkerId = "";

                        employeeLeaveContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((PREmployeeLeavesSvc)channel).createAsync(new create(callContext, employeeLeaveContract)).Result.result;
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
            {
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

                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
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
                        decimal leaveDays = 0;
                        decimal eligibility = 0;
                        decimal Quota = 0;
                        decimal.TryParse(dataRow["Quota"].ToString(), out Quota);
                        Decimal.TryParse(dataRow["LeaveDays"].ToString(), out leaveDays);
                        decimal.TryParse(dataRow["Eligibility"]?.ToString(), out eligibility);

                        PREmployeeLeavesSvcContract employeeLeaveContract = new PREmployeeLeavesSvcContract();

                        employeeLeaveContract.LeaveReqId = dataRow["LeaveReqId"].ToString();
                        employeeLeaveContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        employeeLeaveContract.EmployeeName = dataRow["EmployeeName"].ToString();
                        employeeLeaveContract.Balance = eligibility;
                        employeeLeaveContract.Eligibility = eligibility;
                        employeeLeaveContract.Quota = Quota;
                        employeeLeaveContract.LeaveDays = leaveDays;
                        employeeLeaveContract.LeaveReqDate = Convert.ToDateTime(dataRow["LeaveReqDate"]);
                        employeeLeaveContract.LeaveStartDate = Convert.ToDateTime(dataRow["LeaveStartDate"]);
                        employeeLeaveContract.LeaveEndDate = Convert.ToDateTime(dataRow["LeaveEndDate"]);
                        employeeLeaveContract.LeaveCode = dataRow["LeaveCode"].ToString();
                        employeeLeaveContract.Reason = dataRow["Reason"].ToString();

                        employeeLeaveContract.RecId = recId;

                        GeneralContract results = ((PREmployeeLeavesSvc)channel).UpdateAsync(new Update(callContext, employeeLeaveContract)).Result.result;
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
                string employeeId = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((PREmployeeLeavesSvc)channel).DeleteAsync(new Delete(callContext, employeeId, _recordsRecId)).Result.result;
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
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
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

                    var results = ((PREmployeeLeavesSvc)channel).RetrieveAllAsync(new RetrieveAll(callContext)).Result.result;

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

        public DataTable retriveEmployeeRequestioner()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeeLeavesSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

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

        public DataTable retriveEmployeeReportees()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeeLeavesSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

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


        public decimal retrieveEmployeeLeaveBalance(string _employeeId, string _leaveCode, string _leaveStartDate, string _leaveRequestId = " ")
        {
            DateTime fixedLeaveStartDate = new DateTime(2025, 1, 1); //haris
            decimal leaveBalance = 0;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = _employeeId;
                string leaveCode = _leaveCode;
                DateTime leaveStartDate = DateTime.TryParse(_leaveStartDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out leaveStartDate) ? leaveStartDate : fixedLeaveStartDate;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    leaveBalance = ((PREmployeeLeavesSvc)channel).getLeaveBalanceAsync
                                                                    (new getLeaveBalance(callContext, employeeId, leaveCode, "", leaveStartDate)).Result.result;
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
            return leaveBalance;
        }

        public decimal retrieveEmployeeLeaveDays(long _employeeId, string _leaveCode, DateTime _leaveStartDate, DateTime _leaveEndDate)
        {
            DateTime fixedLeaveStartDate = new DateTime(2025, 1, 1); //haris
            decimal leaveBalance = 0;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    leaveBalance = ((PREmployeeLeavesSvc)channel).getLeaveDaysAsync
                                                                    (new getLeaveDays(callContext, _employeeId, _leaveCode, _leaveEndDate, _leaveStartDate)).Result.result;
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
            return leaveBalance;
        }

        //public decimal retrieveEmployeeQuota(string _employeeId, string _leaveCode, DateTime _leaveStartDate, long _recId = 0)
        //{
        //    DateTime fixedLeaveStartDate = new DateTime(2025, 1, 1); //haris
        //    decimal leaveBalance = 0;
        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            leaveBalance = ((PREmployeeLeavesSvc)channel).getEmployeeQuotaAsync
        //                                                            (new getEmployeeQuota(callContext, _employeeId, _leaveCode, _leaveStartDate, _recId)).Result.result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //    finally
        //    { }
        //    return leaveBalance;
        //}


        public DataTable retrieveLeaveCategory()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();



                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
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


                    var results = ((PREmployeeLeavesSvc)channel).retrieveLeaveCategoryAsync(new retrieveLeaveCategory(callContext)).Result.result;

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

        public decimal retrieveEmployeeQuota(string _employeeId, string _leaveCode, DateTime _leaveStartDate, long _recId = 0)
        {
            decimal quota = 0;

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREmployeeLeavesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // ✅ Correct parameter order (based on proxy)
                    quota = ((PREmployeeLeavesSvc)channel).getEmployeeQuotaAsync(
                        new getEmployeeQuota(callContext, _employeeId, _leaveCode, _recId, _leaveStartDate)
                    ).Result.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return quota;
        }


        public PREmployeeLeavesSvcContract applyLeaveCategoryLogic(
          PREmployeeLeavesSvcContract contract)
        {
            PREmployeeLeavesSvcContract result = null;

            try
            {
                string dataAreaId =
                    SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader =
                    OAuthHelper.getAuthenticationHeader();

                var serviceUriString =
                    SoapHelper.GetSoapServiceUriString(
                        serviceName,
                        ClientConfiguration.Default.UriString
                    );

                var endpointAddress =
                    new EndpointAddress(serviceUriString);

                var binding =
                    SoapHelper.GetBinding();

                var client =
                    new PREmployeeLeavesSvcClient(
                        binding,
                        endpointAddress
                    );

                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope =
                       new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage =
                        new HttpRequestMessageProperty();

                    requestMessage.Headers[
                        OAuthHelper.OAuthHeader
                    ] = authenticationHeader;

                    OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name
                    ] = requestMessage;

                    CallContext callContext = new CallContext();

                    callContext.MessageId =
                        Guid.NewGuid().ToString();

                    callContext.Company =
                        dataAreaId;

                    result =
                        ((PREmployeeLeavesSvc)channel)
                        .applyLeaveCategoryLogicAsync(
                            new applyLeaveCategoryLogic(
                                callContext,
                                contract
                            )
                        )
                        .Result
                        .result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog =
                    new SysErrorLog();

                System.Reflection.MethodBase currentMethod =
                    System.Reflection.MethodBase.GetCurrentMethod();

                string currentMethodName =
                    currentMethod.DeclaringType.FullName;

                objErrorLog.write(
                    currentMethodName,
                    ex
                );
            }

            return result;
        }



        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new PREmployeeLeavesSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
