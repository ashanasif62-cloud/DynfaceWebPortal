using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ESSWorkerBankAccountSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class EssWorkerBankAccount
    {
        private readonly string serviceName = "ESSWorkerBankAccountSvcGroup";
        public string tableName = "ESSWorkerBankAccount";
        public string actionItem = "Worker Bank Account";

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

                var client = new ESSWorkerBankAccountSvcClient (binding, endpointAddress);
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
                        Int64.TryParse(dataRow["worker"].ToString(), out employeeId);

                        ESSWorkerBankAccountSvcContract eSSWorkerBankAccountContract = new ESSWorkerBankAccountSvcContract();
                        eSSWorkerBankAccountContract.worker                 = employeeId;
                        eSSWorkerBankAccountContract.accountId              = dataRow["accountId"].ToString();
                        eSSWorkerBankAccountContract.name                   = dataRow["name"].ToString();
                        eSSWorkerBankAccountContract.accountNum             = dataRow["accountNum"].ToString();
                        eSSWorkerBankAccountContract.bankIBAN               = dataRow["bankIBAN"].ToString();
                        eSSWorkerBankAccountContract.branchNumber           = dataRow["branchNumber"].ToString();
                        eSSWorkerBankAccountContract.branchName             = dataRow["branchName"].ToString();
                        eSSWorkerBankAccountContract.accountHolder          = dataRow["accountHolder"].ToString();

                        //eSSWorkerBankAccountContract.accountHolderOld       = dataRow["accountHolderOld"].ToString();
                        //eSSWorkerBankAccountContract.accountNumOld          = dataRow["accountNumOld"].ToString();
                        //eSSWorkerBankAccountContract.nameOld                = dataRow["nameOld"].ToString();
                        //eSSWorkerBankAccountContract.accountIdOld           = dataRow["accountIdOld"].ToString();
                        //eSSWorkerBankAccountContract.workerBankAccountRecId = Convert.ToInt64(dataRow["workerBankAccountRecId"]);
                        //eSSWorkerBankAccountContract.bankIBANOld            = dataRow["bankIBANOld"].ToString();
                        //eSSWorkerBankAccountContract.branchNumberOld        = dataRow["branchNumberOld"].ToString();
                        //eSSWorkerBankAccountContract.branchNameOld          = dataRow["branchNameOld"].ToString();
                        
                        //eSSWorkerBankAccountContract.wFStatus = dataRow["wFStatus"] is DBNull ? HRWFStatus.Approved : (HRWFStatus)Enum.Parse(typeof(HRWFStatus), dataRow["wFStatus"].ToString());
                        //eSSWorkerBankAccountContract.wFOperation = dataRow["wFOperation"] is DBNull ? HRWorkflowOperation.Insert : (HRWorkflowOperation)Enum.Parse(typeof(HRWorkflowOperation), dataRow["wFOperation"].ToString());

                        eSSWorkerBankAccountContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((ESSWorkerBankAccountSvc)channel).createAsync(new create(callContext, eSSWorkerBankAccountContract)).Result.result;
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
            { }
            return objBOL;
        }

        public SysOperationResult_BOL update(DataTable _objDT, long RecId)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                DataTable dataTable = _objDT;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new ESSWorkerBankAccountSvcClient(binding, endpointAddress);
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

                        ESSWorkerBankAccountSvcContract eSSWorkerBankAccountContract = new ESSWorkerBankAccountSvcContract();

                        eSSWorkerBankAccountContract.name = dataRow["name"].ToString();
                        eSSWorkerBankAccountContract.accountNum = dataRow["accountNum"].ToString();
                        eSSWorkerBankAccountContract.accountHolder = dataRow["accountHolder"].ToString();
                        //eSSWorkerBankAccountContract.workerBankAccountRecId = Convert.ToInt64(dataRow["workerBankAccountRecId"]);
                        eSSWorkerBankAccountContract.bankIBAN = dataRow["bankIBAN"].ToString();
                        eSSWorkerBankAccountContract.branchNumber = dataRow["branchNumber"].ToString();
                        eSSWorkerBankAccountContract.branchName = dataRow["branchName"].ToString();
                        //eSSWorkerBankAccountContract.bankIBANOld = dataRow["bankIBANOld"].ToString();
                        //eSSWorkerBankAccountContract.branchNumberOld = dataRow["branchNumberOld"].ToString();
                        //eSSWorkerBankAccountContract.branchNameOld = dataRow["branchNameOld"].ToString();

                        //eSSWorkerBankAccountContract.wFOperation = dataRow["wFOperation"] is DBNull ? HRWorkflowOperation.Update : (HRWorkflowOperation)Enum.Parse(typeof(HRWorkflowOperation), dataRow["wFOperation"].ToString());
                        
                        eSSWorkerBankAccountContract.RecId = RecId;

                        GeneralContract results = ((ESSWorkerBankAccountSvc)channel).updateAsync(new update(callContext, eSSWorkerBankAccountContract)).Result.result;
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
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                long[] recordsRecId = _recordsRecId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSWorkerBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((ESSWorkerBankAccountSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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
                var client = new ESSWorkerBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSWorkerBankAccountSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
                var client = new ESSWorkerBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSWorkerBankAccountSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

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
                var client = new ESSWorkerBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSWorkerBankAccountSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new ESSWorkerBankAccountSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
