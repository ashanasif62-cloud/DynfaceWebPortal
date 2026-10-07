using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmPersonIdentificationNumberSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HcmPersonIdentificationNumber
    {
        private readonly string serviceName = "HcmPersonIdentificationNumberSvcGroup";
        public string tableName = "HcmPersonIdentificationNumber";
        public string actionItem = "Identification Number";

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

                var client = new HcmPersonIdentificationNumberSvcClient(binding, endpointAddress);
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
                        long identificationType = 0;
                        Int64.TryParse(dataRow["IdentificationType"].ToString(), out identificationType);
                        long issuingAgency = 0;
                        Int64.TryParse(dataRow["IssuingAgency"].ToString(), out issuingAgency);

                        HcmPersonIdentificationNumberSvcContract hcmPersonIdentificationNumberSvcContract = new HcmPersonIdentificationNumberSvcContract();

                        hcmPersonIdentificationNumberSvcContract.EmployeeId         = employeeId;
                        hcmPersonIdentificationNumberSvcContract.IdentificationType = identificationType;
                        hcmPersonIdentificationNumberSvcContract.IssuingAgency      = issuingAgency;

                        hcmPersonIdentificationNumberSvcContract.Classification     = dataRow["Classification"].ToString();
                        hcmPersonIdentificationNumberSvcContract.Description        = dataRow["Description"].ToString();
                        hcmPersonIdentificationNumberSvcContract.IdentificationNumber = dataRow["IdentificationNumber"].ToString();
                        hcmPersonIdentificationNumberSvcContract.WorkflowOperation  = dataRow["WorkflowOperation"].ToString();
                        hcmPersonIdentificationNumberSvcContract.ApprovalStatus = dataRow["ApprovalStatus"].ToString();

                        hcmPersonIdentificationNumberSvcContract.IsPrimary = dataRow["IsPrimary"] is DBNull ? NoYes.No :
                                                                            (NoYes)Enum.Parse(typeof(NoYes), dataRow["IsPrimary"].ToString());

                        hcmPersonIdentificationNumberSvcContract.IssuedDate = (dataRow["IssuedDate"].ToString() == string.Empty) ?
                                                                              DateTime.MinValue : dataRow["IssuedDate"].ToString().toDateTime();
                        hcmPersonIdentificationNumberSvcContract.ExpirationDate = (dataRow["ExpirationDate"].ToString() == string.Empty) ?
                                                                              DateTime.MinValue : dataRow["ExpirationDate"].ToString().toDateTime();

                        //hcmPersonIdentificationNumberSvcContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((HcmPersonIdentificationNumberSvc)channel).createAsync
                                     (new create(callContext, hcmPersonIdentificationNumberSvcContract)).Result.result;

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

                var client = new HcmPersonIdentificationNumberSvcClient(binding, endpointAddress);
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
                        long identificationType = 0;
                        Int64.TryParse(dataRow["IdentificationType"].ToString(), out identificationType);
                        long issuingAgency = 0;
                        Int64.TryParse(dataRow["IssuingAgency"].ToString(), out issuingAgency);

                        HcmPersonIdentificationNumberSvcContract hcmPersonIdentificationNumberSvcContract = new HcmPersonIdentificationNumberSvcContract();

                        hcmPersonIdentificationNumberSvcContract.EmployeeId         = employeeId;
                        hcmPersonIdentificationNumberSvcContract.IdentificationType = identificationType;
                        hcmPersonIdentificationNumberSvcContract.IssuingAgency      = issuingAgency;

                        hcmPersonIdentificationNumberSvcContract.Classification = dataRow["Classification"].ToString();
                        hcmPersonIdentificationNumberSvcContract.Description = dataRow["Description"].ToString();
                        hcmPersonIdentificationNumberSvcContract.IdentificationNumber = dataRow["IdentificationNumber"].ToString();
                        hcmPersonIdentificationNumberSvcContract.WorkflowOperation = dataRow["WorkflowOperation"].ToString();
                        hcmPersonIdentificationNumberSvcContract.ApprovalStatus = dataRow["ApprovalStatus"].ToString();

                        hcmPersonIdentificationNumberSvcContract.IsPrimary = dataRow["IsPrimary"] is DBNull ? NoYes.No :
                                                                            (NoYes)Enum.Parse(typeof(NoYes), dataRow["IsPrimary"].ToString());

                        hcmPersonIdentificationNumberSvcContract.IssuedDate = dataRow["IssuedDate"].ToString().toDateTime();
                        hcmPersonIdentificationNumberSvcContract.ExpirationDate = dataRow["ExpirationDate"].ToString().toDateTime();

                        hcmPersonIdentificationNumberSvcContract.RecId = recId;

                        GeneralContract results = ((HcmPersonIdentificationNumberSvc)channel).updateAsync
                                    (new update(callContext, hcmPersonIdentificationNumberSvcContract)).Result.result;

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
                var client = new HcmPersonIdentificationNumberSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HcmPersonIdentificationNumberSvc)channel).deleteAsync
                                                (new delete(callContext, recordsRecId)).Result.result;
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

        public DataTable retrieveByEmployee(string _employeeId)
        {
            string employeeId = _employeeId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmPersonIdentificationNumberSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HcmPersonIdentificationNumberSvc)channel).findByEmployeeAsync(new findByEmployee(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new HcmPersonIdentificationNumberSvcContract[] { });
            return dataTable;
        }

    }
}
