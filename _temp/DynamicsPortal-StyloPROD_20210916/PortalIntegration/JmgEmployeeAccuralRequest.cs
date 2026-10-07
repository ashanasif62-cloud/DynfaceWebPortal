using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.JmgEmployeeAccuralRequestSvcReference;
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
    public class JmgEmployeeAccuralRequest
    {
        private readonly string serviceName = "JmgEmployeeAccuralRequestSvcGroup";
        public string tableName = "JmgEmployeeAccuralRequests";
        public string actionItem = "Compensatory Leave Requests";

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

                var client = new JmgEmployeeAccuralRequestSvcClient (binding, endpointAddress);
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

                        JmgEmployeeAccuralRequestSvcContract jmgEmployeeAccuralRequestContract = new JmgEmployeeAccuralRequestSvcContract();
                        jmgEmployeeAccuralRequestContract.worker                 = employeeId;
                        //jmgEmployeeAccuralRequestContract.profileId              = dataRow["profileId"].ToString();
                        jmgEmployeeAccuralRequestContract.profileDate            = dataRow["profileDate"].ToString().toDateTime();
                        jmgEmployeeAccuralRequestContract.requestDate            = dataRow["requestDate"].ToString().toDateTime();
                        jmgEmployeeAccuralRequestContract.description            = dataRow["description"].ToString();
                        //jmgEmployeeAccuralRequestContract.entitlementRecId       = Convert.ToInt64(dataRow["entitlementRecId"]);
                        //jmgEmployeeAccuralRequestContract.earningsRecId          = Convert.ToInt32(dataRow["earningsRecId"]);
                        //jmgEmployeeAccuralRequestContract.period                 = Convert.ToInt64(dataRow["period"]);
                        //jmgEmployeeAccuralRequestContract.timeDeductionDate      = dataRow["timeDeductionDate"].ToString().toDateTime();

                        //jmgEmployeeAccuralRequestContract.wFStatus = dataRow["wFStatus"] is DBNull ? JmgWFStatus.Approved : (JmgWFStatus)Enum.Parse(typeof(JmgWFStatus), dataRow["wFStatus"].ToString());
                        jmgEmployeeAccuralRequestContract.requestType = dataRow["requestType"] is DBNull ? JmgAccuralRequestType.CompensatoryLeave : (JmgAccuralRequestType)Enum.Parse(typeof(JmgAccuralRequestType), dataRow["requestType"].ToString());

                        jmgEmployeeAccuralRequestContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((JmgEmployeeAccuralRequestSvc)channel).createAsync(new create(callContext, jmgEmployeeAccuralRequestContract)).Result.result;
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

                var client = new JmgEmployeeAccuralRequestSvcClient(binding, endpointAddress);
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
                        JmgEmployeeAccuralRequestSvcContract jmgEmployeeAccuralRequestContract = new JmgEmployeeAccuralRequestSvcContract();

                        jmgEmployeeAccuralRequestContract.profileDate = dataRow["profileDate"].ToString().toDateTime();
                        jmgEmployeeAccuralRequestContract.description = dataRow["description"].ToString();

                        jmgEmployeeAccuralRequestContract.RecId = RecId;

                        GeneralContract results = ((JmgEmployeeAccuralRequestSvc)channel).updateAsync(new update(callContext, jmgEmployeeAccuralRequestContract)).Result.result;
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
                var client = new JmgEmployeeAccuralRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((JmgEmployeeAccuralRequestSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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
                var client = new JmgEmployeeAccuralRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgEmployeeAccuralRequestSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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

        public DataTable retriveEmployeeRequestioner_compensatryLeaveRequest()
        {
            return this.retriveEmployeeRequestioner(JmgAccuralRequestType.CompensatoryLeave);
        }

        public DataTable retriveEmployeeRequestioner_OffDayAllowance()
        {
            return this.retriveEmployeeRequestioner(JmgAccuralRequestType.OffDayAllowance);
        }

        public DataTable retriveEmployeeReportees_compensatryLeaveRequest()
        {
            return this.retriveEmployeeReportees(JmgAccuralRequestType.CompensatoryLeave);
        }

        public DataTable retriveEmployeeReportees_OffDayAllowance()
        {
            return this.retriveEmployeeReportees(JmgAccuralRequestType.OffDayAllowance);
        }

        public DataTable retriveEmployeeRequestioner(JmgAccuralRequestType _requestType)
        {
            try
            {

                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgEmployeeAccuralRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgEmployeeAccuralRequestSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId, _requestType)).Result.result;

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

        public DataTable retriveEmployeeReportees(JmgAccuralRequestType _requestType)
        {
            try
            {

                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new JmgEmployeeAccuralRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((JmgEmployeeAccuralRequestSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId, _requestType)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new JmgEmployeeAccuralRequestSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
