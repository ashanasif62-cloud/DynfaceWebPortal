using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PortalIntegration.ESSPersonalContactsSvcReference;
using BussinessObject;
using System.Xml.XPath;

namespace PortalIntegration
{
    public class ESSHRPersonalContacts
    {
        private readonly string serviceName = "ESSPersonalContactsSvcGroup";
        public string tableName = "DirPartyTable";
        public string actionItem = "Personal Contacts";


        public DataTable retrieveAll(string _employeeId)
        {
            string employeeId = _employeeId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSPersonalContactsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSPersonalContactsSvc)channel).retrieveAllAsync(new retrieveAll(callContext, employeeId)).Result.result;

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



        public SysOperationResult_BOL   create(
              ESSPersonalContactsContract personalContactsContract)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(
                    serviceName,
                    ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSPersonalContactsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope scope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    var infolog = ((ESSPersonalContactsSvc)channel)
                        .createAsync(new create(callContext, personalContactsContract)).Result;
                    GeneralContract results = infolog.result;

                    string infologMessage = string.Empty;
                    if (infolog?.Infolog?.Entries != null && infolog.Infolog.Entries.Length > 0)
                        infologMessage = infolog.Infolog.Entries[0].Message;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                    if (!objBOL.isSuccess && !string.IsNullOrEmpty(infologMessage))
                        objBOL.Message = infologMessage;

                    SysLogUserActivity.logUserActivity(
                        "PersonalContacts",
                        "Create",
                        ActionType.Create,
                        objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                objErrorLog.write(currentMethod.DeclaringType.FullName, ex);
            }

            return objBOL;
        }

        public DataTable retrieveRelationShipTypeId()
        {
           
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSPersonalContactsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSPersonalContactsSvc)channel).retrieveRelationshipTypeIdAsync(new retrieveRelationshipTypeId(callContext)).Result.result;

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

        public SysOperationResult_BOL deletePersonalContact(DataTable _deleteRecords)
        {
            SysOperationResult_BOL result = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSPersonalContactsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    List<ESSPersonalContactsContract> contractList = new List<ESSPersonalContactsContract>();

                    foreach (DataRow dr in _deleteRecords.Rows)
                    {
                        ESSPersonalContactsContract contractRecord = new ESSPersonalContactsContract
                        {
                            EmployeeId = SessionVariables.getCurrentEmployeeId(),
                            RecId = Convert.ToInt64(dr["RecId"]),
                            RelationShipTypeId = dr["RelationShipTypeId"].ToString(),
                            isDependent = Convert.ToBoolean(dr["isDependent"]),
                            isBeneficiary = Convert.ToBoolean(dr["isBeneficiary"]),
                            EmergencyContact = Convert.ToBoolean(dr["EmergencyContact"])
                        };

                        contractList.Add(contractRecord);
                    }

                    var infolog = ((ESSPersonalContactsSvc)channel).deleteAsync(new delete(callContext, contractList.ToArray())).Result;
                    var results = infolog.result;

                    string infologMessage = string.Empty;
                    if (infolog?.Infolog?.Entries != null && infolog.Infolog.Entries.Length > 0)
                        infologMessage = infolog.Infolog.Entries[0].Message;

                    result = SysOperationResults.operationResults(results);
                    if (!result.isSuccess && !string.IsNullOrEmpty(infologMessage))
                        result.Message = infologMessage;
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

            return result;
        }

        public SysOperationResult_BOL updatePersonalContract(ESSPersonalContactsContract _updateRecord)
        {
            SysOperationResult_BOL operationResult = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSPersonalContactsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract result = ((ESSPersonalContactsSvc)channel).updateAsync(new update(callContext, _updateRecord)).Result.result;

                    GeneralContract[] results = new GeneralContract[] { result };

                    operationResult = SysOperationResults.operationResults(results);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                operationResult.isSuccess = false;
                operationResult.Message = ex.Message;
            }
            finally
            { }
            return operationResult;
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new ESSPersonalContactsContract[] { });
            return dataTable;
        }
    }
}
