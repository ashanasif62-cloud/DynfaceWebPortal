using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PREmployeePFWithDrawlSvcReference;
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
    public class PREmployeePFWithDrawl
    {
        private readonly string serviceName = "PREmployeePFWithDrawlSvcGroup";
        public string tableName = "PREmployeePFWithDrawls";
        public string actionItem = "PF/CF WithDrawls";

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

                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
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
                        PREmployeePFWithDrawlSvcContract pREmployeePFWithDrawlContract = new PREmployeePFWithDrawlSvcContract();
                        pREmployeePFWithDrawlContract.employeeId                 = dataRow["employeeId"].ToString();
                        //pREmployeePFWithDrawlContract.employeeName               = dataRow["employeeName"].ToString();
                        //pREmployeePFWithDrawlContract.requestId                  = dataRow["requestId"].ToString();
                        pREmployeePFWithDrawlContract.pFDescription              = dataRow["pFDescription"].ToString();
                        pREmployeePFWithDrawlContract.advanceTypeCode            = dataRow["advanceTypeCode"].ToString();
                        //pREmployeePFWithDrawlContract.currency                   = dataRow["currency"].ToString();
                        //pREmployeePFWithDrawlContract.benDedCode                 = dataRow["benDedCode"].ToString();
                        //pREmployeePFWithDrawlContract.earningCode                = dataRow["earningCode;"].ToString();
                        pREmployeePFWithDrawlContract.requestDate                = dataRow["requestDate"].ToString().toDateTime();
                        //pREmployeePFWithDrawlContract.recoveryAmount             = Convert.ToDecimal(dataRow["recoveryAmount"]);
                        pREmployeePFWithDrawlContract.balance                    = Convert.ToDecimal(dataRow["balance"]);
                        pREmployeePFWithDrawlContract.requestAmount              = Convert.ToInt32(dataRow["requestAmount"]);
                        //pREmployeePFWithDrawlContract.earningRefRecId            = Convert.ToInt32(dataRow["earningRefRecId"]);
                        pREmployeePFWithDrawlContract.employerPFBalance          = Convert.ToDecimal(dataRow["employerPFBalance"]);
                        pREmployeePFWithDrawlContract.outStandingAmount          = Convert.ToDecimal(dataRow["outStandingAmount"]);
                        //pREmployeePFWithDrawlContract.payGroupCode               = dataRow["payGroupCode"].ToString();

                        //pREmployeePFWithDrawlContract.wFStatus = dataRow["wFStatus"] is DBNull ? PRWFStatus.Approved : (PRWFStatus)Enum.Parse(typeof(PRWFStatus), dataRow["wFStatus"].ToString());

                        pREmployeePFWithDrawlContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((PREmployeePFWithDrawlSvc)channel).createAsync(new create(callContext, pREmployeePFWithDrawlContract)).Result.result;
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

                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
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

                        PREmployeePFWithDrawlSvcContract pREmployeePFWithDrawlContract = new PREmployeePFWithDrawlSvcContract();

                        pREmployeePFWithDrawlContract.requestDate = dataRow["requestDate"].ToString().toDateTime();
                        pREmployeePFWithDrawlContract.requestAmount = Convert.ToInt32(dataRow["requestAmount"]);
                        pREmployeePFWithDrawlContract.pFDescription = dataRow["pFDescription"].ToString();
                        pREmployeePFWithDrawlContract.recoveryAmount = Convert.ToDecimal(dataRow["recoveryAmount"]);
                        pREmployeePFWithDrawlContract.outStandingAmount = Convert.ToDecimal(dataRow["outStandingAmount"]);

                        pREmployeePFWithDrawlContract.RecId = RecId;

                        GeneralContract results = ((PREmployeePFWithDrawlSvc)channel).updateAsync(new update(callContext, pREmployeePFWithDrawlContract)).Result.result;
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
                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((PREmployeePFWithDrawlSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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
                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeePFWithDrawlSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeePFWithDrawlSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

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
                var client = new PREmployeePFWithDrawlSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PREmployeePFWithDrawlSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new PREmployeePFWithDrawlSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
