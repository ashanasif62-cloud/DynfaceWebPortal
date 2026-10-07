using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ExpenseManagmentSvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
    public class ExpenseReport
    {
        private readonly string serviceName = "ExpenseManagmentSvcGroup";
        public string tableName = "TrvExpTable";
        public string actionItem = "Expense Report Request";
        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    // "000081" is Expense Report Number
                    var results = ((ExpenseReportsServiceSvc)channel).GetAllExpenseReportsAsync(new GetAllExpenseReports(callContext, employeeid, dataAreaId)).Result.result;

                    DataTable dataTable = RetrieveDatatable.CreateDataTableFromMap(results, "fields");
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

        public SysOperationResult_BOL createExpenseReport(List<ExpenseReportRequestSvc> _expenseReportFields)
        {
            SysOperationResult_BOL operationsResults_BOL = new SysOperationResult_BOL();

            try 
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    // "000081" is Expense Report Number
                    var results = ((ExpenseReportsServiceSvc)channel).CreateExpenseReportAsync(new CreateExpenseReport(callContext, employeeid, _expenseReportFields.ToArray(), dataAreaId)).Result.result;

                    List<ExpenseReportResponse> list = new List<ExpenseReportResponse> { results };

                    if (list != null || list.Any())
                    {
                        operationsResults_BOL.isSuccess = true;
                        operationsResults_BOL.Message = "Records Created Successfully";
                        operationsResults_BOL.AlertType = AlertType.Success.ToString();
                        return operationsResults_BOL;
                    }
                    else
                    {
                        operationsResults_BOL.isSuccess = false;
                        operationsResults_BOL.Message = "Error Creating Record";
                        operationsResults_BOL.AlertType = AlertType.Error.ToString();
                        return operationsResults_BOL;

                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                operationsResults_BOL.isSuccess = false;
                operationsResults_BOL.Message = "Error Creating Record";
                operationsResults_BOL.AlertType = AlertType.Error.ToString();
                return operationsResults_BOL;

            }

        }

        public SysOperationResult_BOL submitExpenseReportToWorkflow(string[] _expenseReportNumber)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
             
            try 
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string[] expenseReportNumber = _expenseReportNumber;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    // "000081" is Expense Report Number
                    GeneralContract[] results = ((ExpenseReportsServiceSvc)channel).SubmitExpenseReportsToWorkflowAsync(new SubmitExpenseReportsToWorkflow(callContext, expenseReportNumber, dataAreaId)).Result.result;
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
            return objBOL;
        }

        public void updateExpenseReport(List<ExpenseReportRequestSvc> _expenseReportFields, string _expenseReportNumber)
        {
            SysOperationResult_BOL operationsResults_BOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    var results = ((ExpenseReportsServiceSvc)channel).UpdateExpenseReportAsync(new UpdateExpenseReport(callContext, employeeid, _expenseReportFields.ToArray(), _expenseReportNumber, dataAreaId)).Result.result;

                    List<ExpenseReportResponse> list = new List<ExpenseReportResponse> { results };
                    DataTable dataTable = RetrieveDatatable.CreateDataTableFromMap(list, "fields");

                    if (dataTable != null)
                    {
                        operationsResults_BOL.isSuccess = true;
                        operationsResults_BOL.Message = "Records Updated Successfully";
                        operationsResults_BOL.AlertType = "Success";
                        NotificationMessage.showMessage(operationsResults_BOL);
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
        }

        public SysOperationResult_BOL deleteExpenseReport(string[] _expenseReportNumbers)
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
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    var results = ((ExpenseReportsServiceSvc)channel).DeleteExpenseReportsAsync(new DeleteExpenseReports(callContext, employeeid, _expenseReportNumbers, dataAreaId)).Result.result;

                    GeneralContract[] list = new GeneralContract[] { results };

                    objBOL = SysOperationResults.operationResults(list);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, list);
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

        public DataTable getPurposeLookup()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ExpenseReportsServiceSvc)channel).PurposeLookupAsync(new PurposeLookup(callContext, dataAreaId)).Result.result;

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

        public DataTable getLocationLookup()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReportsServiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ExpenseReportsServiceSvc)channel).LocationLookupAsync(new LocationLookup(callContext, dataAreaId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new ExpenseReportResponse[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
