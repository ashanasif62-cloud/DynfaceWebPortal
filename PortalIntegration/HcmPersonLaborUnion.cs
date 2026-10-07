using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmPersonLaborUnionSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HcmPersonLaborUnion
    {
        private readonly string serviceName = "HcmPersonLaborUnionSvcGroup";
        public string tableName = "HcmPersonLaborUnion";
        public string actionItem = "Sub Department";

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

                var client = new HcmPersonLaborUnionSvcClient(binding, endpointAddress);
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
                        HcmPersonLaborUnionSvcContract hcmPersonLaborUnionSvcContract = new HcmPersonLaborUnionSvcContract();

                        long person = 0;
                        Int64.TryParse(dataRow["Person"].ToString(), out person);
                        long laborUnion = 0;
                        Int64.TryParse(dataRow["LaborUnion"].ToString(), out laborUnion);

                        hcmPersonLaborUnionSvcContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        hcmPersonLaborUnionSvcContract.StartDate = dataRow["StartDate"].ToString().toDateTime();
                        hcmPersonLaborUnionSvcContract.EndDate = dataRow["EndDate"].ToString().toDateTime();

                        hcmPersonLaborUnionSvcContract.LaborUnion = laborUnion;
                        hcmPersonLaborUnionSvcContract.Person = person;

                        GeneralContract results = ((HcmPersonLaborUnionSvc)channel).createAsync
                                     (new create(callContext, hcmPersonLaborUnionSvcContract)).Result.result;

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

                var client = new HcmPersonLaborUnionSvcClient(binding, endpointAddress);
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
                        HcmPersonLaborUnionSvcContract hcmPersonLaborUnionSvcContract = new HcmPersonLaborUnionSvcContract();

                        long person = 0;
                        Int64.TryParse(dataRow["Person"].ToString(), out person);
                        long laborUnion = 0;
                        Int64.TryParse(dataRow["LaborUnion"].ToString(), out laborUnion);

                        hcmPersonLaborUnionSvcContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        hcmPersonLaborUnionSvcContract.StartDate = dataRow["StartDate"].ToString().toDateTime();
                        hcmPersonLaborUnionSvcContract.EndDate = dataRow["EndDate"].ToString().toDateTime();

                        hcmPersonLaborUnionSvcContract.LaborUnion = laborUnion;
                        hcmPersonLaborUnionSvcContract.Person = person;

                        GeneralContract results = ((HcmPersonLaborUnionSvc)channel).updateAsync(new update(callContext, hcmPersonLaborUnionSvcContract)).Result.result;

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
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmPersonLaborUnionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HcmPersonLaborUnionSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;

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

        public DataTable findByEmployee(string _employeeId)
        {
            try
            {
                string employeeId = _employeeId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmPersonLaborUnionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    HcmPersonLaborUnionSvcContract[] results = ((HcmPersonLaborUnionSvc)channel).findByEmployeeAsync
                                                    (new findByEmployee(callContext, employeeId)).Result.result;
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

        public DataTable retriveEmployeeReportees(string _employeeId)
        {
            try
            {
                string employeeId = _employeeId;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmPersonLaborUnionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    HcmPersonLaborUnionSvcContract[] results = ((HcmPersonLaborUnionSvc)channel).retriveEmployeeReporteesAsync
                                                    (new retriveEmployeeReportees(callContext, employeeId)).Result.result;
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
            dataTable = RetrieveDatatable.createDataTable(new HcmPersonLaborUnionSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;

            //HcmPersonLaborUnionSvcContract hcmPersonLaborUnionSvcContract = new HcmPersonLaborUnionSvcContract();
            //hcmPersonLaborUnionSvcContract.LaborUnionName;
            //hcmPersonLaborUnionSvcContract.LaborUnion;
            //hcmPersonLaborUnionSvcContract.StartDate;
            //hcmPersonLaborUnionSvcContract.EndDate;
            //hcmPersonLaborUnionSvcContract.EmployeeId;
            //hcmPersonLaborUnionSvcContract.EmployeeName;
            //hcmPersonLaborUnionSvcContract.Person;
            //hcmPersonLaborUnionSvcContract.RecId;
        }



    }
}
