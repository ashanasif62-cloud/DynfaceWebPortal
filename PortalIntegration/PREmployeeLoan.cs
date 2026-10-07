using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PREmployeeLoanRequestSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class PREmployeeLoan
    {
        private readonly string serviceName = "PREmployeeLoanRequestSvcGroup";
        public string tableName = "PREmployeeLoanRequests";
        public string actionItem = "Employee Loan Request";

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

                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
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
                        int requestedInstallments = 0;
                        Int32.TryParse(dataRow["RequestedInstallments"].ToString(), out requestedInstallments);
                        decimal requestedInstallmentAmount = 0;
                        Decimal.TryParse(dataRow["RequestedInstallmentAmount"].ToString(), out requestedInstallmentAmount);
                        decimal loanAmount = 0;
                        Decimal.TryParse(dataRow["LoanAmount"].ToString(), out loanAmount);

                        PREmployeeLoanRequestSvcContract employeeLoanContract = new PREmployeeLoanRequestSvcContract();
                        employeeLoanContract.Currency = dataRow["Currency"].ToString();//- C
                      employeeLoanContract.LoanDescription = dataRow["LoanDescription"].ToString();//- CU
                        employeeLoanContract.LoanTypeCode = dataRow["LoanTypeCode"].ToString();//- CU
                        employeeLoanContract.Guarantor1 = dataRow["Guarantor1"].ToString();//- CU
                        employeeLoanContract.Guarantor2 = dataRow["Guarantor2"].ToString();//- CU

                        employeeLoanContract.EmployeeId = employeeId;   //- C
                        employeeLoanContract.LoanAmount = loanAmount;//- CU
                        employeeLoanContract.RequestedInstallmentAmount = requestedInstallmentAmount;//- CU
                        employeeLoanContract.RequestedInstallments = requestedInstallments;

                        employeeLoanContract.RequestDate = dataRow["RequestDate"].ToString().toDateTime();//- CU
                        employeeLoanContract.RecoveryStartDate = dataRow["RecoveryStartDate"].ToString().toDateTime();//- CU
                        employeeLoanContract.RequestedPaymentDate = dataRow["RequestedPaymentDate"].ToString().toDateTime();//- CU

                        employeeLoanContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        var infolog = ((PREmployeeLoanRequestSvc)channel).createAsync(new create(callContext, employeeLoanContract)).Result;
                        GeneralContract results = infolog.result;

                        string infologMessage = string.Empty;
                        if (infolog?.Infolog?.Entries != null && infolog.Infolog.Entries.Length > 0)
                            infologMessage = infolog.Infolog.Entries[0].Message;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                        if (!objBOL.isSuccess && !string.IsNullOrEmpty(infologMessage))
                            objBOL.Message = infologMessage;

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
            { }
            return objBOL;
        }

        public SysOperationResult_BOL update(DataTable _objDT, long _recId)
        {
            DataTable dataTable = _objDT;
            long recId = _recId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
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

                        int requestedInstallments = 0;
                        Int32.TryParse(dataRow["RequestedInstallments"].ToString(), out requestedInstallments);
                        decimal requestedInstallmentAmount = 0;
                        Decimal.TryParse(dataRow["RequestedInstallmentAmount"].ToString(), out requestedInstallmentAmount);
                        decimal loanAmount = 0;
                        Decimal.TryParse(dataRow["LoanAmount"].ToString(), out loanAmount);

                        PREmployeeLoanRequestSvcContract employeeLoanContract = new PREmployeeLoanRequestSvcContract();

                        employeeLoanContract.LoanDescription = dataRow["LoanDescription"].ToString();//- CU
                       employeeLoanContract.LoanTypeCode = dataRow["LoanTypeCode"].ToString();//- CU
                        employeeLoanContract.Guarantor1 = dataRow["Guarantor1"].ToString();//- CU
                        employeeLoanContract.Guarantor2 = dataRow["Guarantor2"].ToString();//- CU

                        employeeLoanContract.LoanAmount = loanAmount;//- CU
                        employeeLoanContract.RequestedInstallmentAmount = requestedInstallmentAmount;//- CU
                        employeeLoanContract.RequestedInstallments = requestedInstallments;

                        employeeLoanContract.RequestDate = dataRow["RequestDate"].ToString().toDateTime();//- CU
                        employeeLoanContract.RecoveryStartDate = dataRow["RecoveryStartDate"].ToString().toDateTime();//- CU
                        employeeLoanContract.RequestedPaymentDate = dataRow["RequestedPaymentDate"].ToString().toDateTime();//- CU

                        employeeLoanContract.RecId = recId;

                        var infolog = ((PREmployeeLoanRequestSvc)channel).updateAsync(new update(callContext, employeeLoanContract)).Result;
                        GeneralContract results = infolog.result;

                        string infologMessage = string.Empty;
                        if (infolog?.Infolog?.Entries != null && infolog.Infolog.Entries.Length > 0)
                            infologMessage = infolog.Infolog.Entries[0].Message;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                        if (!objBOL.isSuccess && !string.IsNullOrEmpty(infologMessage))
                            objBOL.Message = infologMessage;

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
                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var infolog = ((PREmployeeLoanRequestSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result;
                    GeneralContract[] results = infolog.result;

                    string infologMessage = string.Empty;
                    if (infolog?.Infolog?.Entries != null && infolog.Infolog.Entries.Length > 0)
                        infologMessage = infolog.Infolog.Entries[0].Message;

                    objBOL = SysOperationResults.operationResults(results);
                    if (!objBOL.isSuccess && !string.IsNullOrEmpty(infologMessage))
                        objBOL.Message = infologMessage;

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
                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeeLoanRequestSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeeLoanRequestSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

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
                var client = new PREmployeeLoanRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeeLoanRequestSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new PREmployeeLoanRequestSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
        
    }
}
