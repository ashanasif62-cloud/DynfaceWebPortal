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
    public class ExpenseReceipt
    {
        private readonly string serviceName = "ExpenseManagmentSvcGroup";
        public string tableName = "TrvExpTrans";
        public string actionItem = "Expense Receipt";

        public DataTable uploadReceipt(string _fileName, string _capturedReceipt, string _contentType, string _fileExtension)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ExpenseReceiptSvcClient(binding, endpointAddress);
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
                    var results = ((ExpenseReceiptSvc)channel).UploadReceiptAsync(new UploadReceipt(callContext, _capturedReceipt, _contentType, employeeid, _fileExtension, _fileName, dataAreaId)).Result.result;

                    DataTable dt = RetrieveDatatable.createDataTable(results);

                    return dt;
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
            return createDataTable();
        }

        public SysOperationResult_BOL attachReceiptToExpenseLine(long _docuRecId, string _expenseTransNumber)
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
                var client = new ExpenseReceiptSvcClient(binding, endpointAddress);
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
                    var results = ((ExpenseReceiptSvc)channel).AttachReceiptToExpenseLineAsync(new AttachReceiptToExpenseLine(callContext, _docuRecId, employeeid, _expenseTransNumber, true, dataAreaId)).Result.result;

                    operationsResults_BOL = SysOperationResults.operationResult<GeneralContract>(results);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                operationsResults_BOL.isSuccess = false;
                operationsResults_BOL.Message = ex.InnerException.Message;
                operationsResults_BOL.AlertType = "Error";
            }
            finally
            { }
            return operationsResults_BOL;
        }

        public SysOperationResult_BOL DeleteReceipts(long _docuRecId, string _expenseTransNumber)
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
                var client = new ExpenseReceiptSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    long[] docuRefRecIds = { _docuRecId };

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    // "000081" is Expense Report Number
                    var results = ((ExpenseReceiptSvc)channel).DeleteReceiptsAsync(new DeleteReceipts(callContext, docuRefRecIds)).Result;

                    operationsResults_BOL = SysOperationResults.operationResult<GeneralContract>(results);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                operationsResults_BOL.isSuccess = false;
                operationsResults_BOL.Message = ex.InnerException.Message;
                operationsResults_BOL.AlertType = "Error";
            }
            finally
            { }
            return operationsResults_BOL;
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new ExpenseReceiptContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
