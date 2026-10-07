using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ESSWorkerBankAccountSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.UI;

namespace PortalIntegration
{
    public class EssWorkerBankAccount
    {
        private readonly string serviceName = "HcmWorkerESSBankAccountSvcGroup";
        public string tableName = "HcmWorkerBankAccount";
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

                var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
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
                        Int64.TryParse(dataRow["Worker"].ToString(), out employeeId);

                        HcmWorkerESSBankAccountSvcContract eSSWorkerBankAccountContract = new HcmWorkerESSBankAccountSvcContract();
                        eSSWorkerBankAccountContract.__k_parmWorker = employeeId;
                        eSSWorkerBankAccountContract.__k_parmAccountId = dataRow["accountId"].ToString();
                        eSSWorkerBankAccountContract.__k_parmName = dataRow["name"].ToString();
                        eSSWorkerBankAccountContract.__k_parmAccountNum = dataRow["accountNum"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBankIBAN = dataRow["bankIBAN"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBranchNumber = dataRow["branchNumber"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBranchName = dataRow["branchName"].ToString();
                        eSSWorkerBankAccountContract.__k_parmAccountHolder = dataRow["accountHolder"].ToString();
                        eSSWorkerBankAccountContract.__k_parmWFStatus = "Not Submitted";
                        eSSWorkerBankAccountContract.__k_parmWFOperation = "Insert";
                        eSSWorkerBankAccountContract.__k_parmEmployeeId = SessionVariables.getCurrentEmployeeId();



                        GeneralContract results = ((HcmWorkerESSBankAccountSvc)channel).createAsync(new create(callContext, eSSWorkerBankAccountContract)).Result.result;
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

                var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
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

                        HcmWorkerESSBankAccountSvcContract eSSWorkerBankAccountContract = new HcmWorkerESSBankAccountSvcContract();

                        eSSWorkerBankAccountContract.__k_parmAccountId = dataRow["accountId"].ToString();
                        eSSWorkerBankAccountContract.__k_parmName = dataRow["name"].ToString();
                        eSSWorkerBankAccountContract.__k_parmAccountNum = dataRow["accountNum"].ToString();
                        eSSWorkerBankAccountContract.__k_parmAccountHolder = dataRow["accountHolder"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBankIBAN = dataRow["bankIBAN"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBranchNumber = dataRow["branchNumber"].ToString();
                        eSSWorkerBankAccountContract.__k_parmBranchName = dataRow["branchName"].ToString();
                        eSSWorkerBankAccountContract.__k_parmWFStatus = "NotSubmitted";
                        eSSWorkerBankAccountContract.__k_parmWFOperation = "Update";
                        eSSWorkerBankAccountContract.__k_parmEmployeeId = SessionVariables.getCurrentEmployeeId();


                        eSSWorkerBankAccountContract.recId = RecId;

                        GeneralContract results = ((HcmWorkerESSBankAccountSvc)channel).updateAsync(new update(callContext, eSSWorkerBankAccountContract)).Result.result;
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
                var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HcmWorkerESSBankAccountSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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

        public DataTable findbyEmployee(string _employeeId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HcmWorkerESSBankAccountSvc)channel).findByEmployeeAsync(new findByEmployee(callContext, _employeeId)).Result.result;

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

        //public DataTable retriveEmployeeRequestioner()
        //{
        //    try
        //    {

        //        string employeeId = SessionVariables.getCurrentEmployeeId();
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            var results = ((ESSWorkerBankAccountSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

        //            DataTable dataTable = RetrieveDatatable.createDataTable(results);
        //            return dataTable;
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
        //    finally
        //    { }
        //}

        //public DataTable retriveEmployeeReportees()
        //{
        //    try
        //    {

        //        string employeeId = SessionVariables.getCurrentEmployeeId();
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new HcmWorkerESSBankAccountSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            var results = ((ESSWorkerBankAccountSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

        //            DataTable dataTable = RetrieveDatatable.createDataTable(results);
        //            return dataTable;
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
        //    finally
        //    { }
        //}

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new HcmWorkerESSBankAccountSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
