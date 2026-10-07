using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeneralAuxiliary;
using PortalIntegration.ESSComplainAndSuggestionSvcReference;
using BussinessObject;

namespace PortalIntegration
{
    public class ESSComplainAndSuggestionsSvc
    {

        private readonly string serviceName = "ESSSuggestionComplainSvcGroup";
        public string tableName = "ESSComplainSuggesstionRequest";
        public string actionItem = "ESSComplainAndSuggestion_ListPage";




        public DataTable retrieveAll(string employeeId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSComplainSuggesstionSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((ESSComplainSuggesstionSVC)channel).FindByEmployeeAsync(new FindByEmployee(callContext, employeeId)).Result.result;

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
                var client = new ESSComplainSuggesstionSVCClient(binding, endpointAddress);
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
                        ESSComplainSuggesstionSvcContract contract = new ESSComplainSuggesstionSvcContract();

                        // ========== Map ALL fields from DataTable ==========
                        contract.PersonnelNumber = dataRow["PersonnelNumber"] != DBNull.Value
                            ? dataRow["PersonnelNumber"].ToString()
                            : string.Empty;

                        contract.RequestDate = dataRow["RequestDate"] != DBNull.Value
                            ? Convert.ToDateTime(dataRow["RequestDate"])
                            : DateTime.Today;

                        contract.TypeCode = dataRow["TypeCode"] != DBNull.Value
                            ? dataRow["TypeCode"].ToString()
                            : string.Empty;

                        // This is the important one for the enum
                        contract.RequestType = dataRow["RequestType"] != DBNull.Value
                            ? dataRow["RequestType"].ToString()   // "Complaint" or "Suggestion"
                            : string.Empty;

                        contract.Description = dataRow["Description"] != DBNull.Value
                            ? dataRow["Description"].ToString()
                            : string.Empty;


                        contract.Remarks = dataRow["Remarks"] != DBNull.Value
                            ? dataRow["Remarks"].ToString()
                            : string.Empty;

                        if (dataRow["ApprovalStatus"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ApprovalStatus"].ToString()))
                        {
                            contract.ApprovalStatus = (PRWFStatus)Convert.ToInt32(dataRow["ApprovalStatus"]);
                        }
                        else
                        {
                            contract.ApprovalStatus = (PRWFStatus)0;   // Default
                        }

                        contract.RequestedBy = dataRow["RequestedBy"] != DBNull.Value
                            ? dataRow["RequestedBy"].ToString()
                            : SessionVariables.getCurrentEmployeeName();

                        // Optional: if you have more fields later
                        // contract.Remarks = dataRow.Table.Columns.Contains("Remarks") && dataRow["Remarks"] != DBNull.Value 
                        //     ? dataRow["Remarks"].ToString() : string.Empty;

                        // ========== Call the AX service ==========
                        GeneralContract results = ((ESSComplainSuggesstionSVC)channel)
                            .CreateAsync(new Create(callContext, contract))
                            .Result.result;

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

                objBOL.isSuccess = false;
                objBOL.Message = ex.Message;
            }

            return objBOL;
        }

        public DataTable retrieveTypeCode(string _type, string _searchtext)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSComplainSuggesstionSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((ESSComplainSuggesstionSVC)channel).retrieveComplainSuggestionTypesAsync(new retrieveComplainSuggestionTypes(callContext,_searchtext,_type)).Result.result;

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
                var client = new ESSComplainSuggesstionSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((ESSComplainSuggesstionSVC)channel).DeleteAsync(new Delete(callContext, recordsRecId)).Result.result;
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






        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new ESSComplainSuggesstionSvcContract[] { });
            dataTable.TableName = tableName;

            return dataTable;

        }

    }
}
