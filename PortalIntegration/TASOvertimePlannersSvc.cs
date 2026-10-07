using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeneralAuxiliary;
using PortalIntegration.TASOvertimePlannerSvcReference;
using BussinessObject;

namespace PortalIntegration
{
    public class TASOvertimePlannersSvc
    {

        private readonly string serviceName = "OvertimePlannerServiceGroup";
        public string tableName = "TASEmployeeOvertimePlanner";
        public string actionItem = "ESSOvertimePlanner_ListPage";



        public DataTable retrieveAll(string employeeId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new OvertimePlannerSvcClient(binding, endpointAddress);
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

                    var results = ((OvertimePlannerSvc)channel).retrieveAllAsync(new retrieveAll(callContext, employeeId)).Result.result;

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



        public DataTable create(OvertimeContract contract)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new OvertimePlannerSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();

                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;

                    OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Service returns STRING message
                    string result = ((OvertimePlannerSvc)channel)
                        .createAsync(new create(callContext, contract))
                        .Result.result;

                    // Create DataTable manually
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Message");

                    DataRow row = dt.NewRow();
                    row["Message"] = result;

                    dt.Rows.Add(row);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();

                System.Reflection.MethodBase currentMethod =
                    System.Reflection.MethodBase.GetCurrentMethod();

                string currentMethodName =
                    currentMethod.DeclaringType.FullName;

                objErrorLog.write(currentMethodName, ex);

                return createDataTable();
            }
        }


        public SysOperationResult_BOL UpdateGeneral(DataTable _objDT)
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

                var client = new OvertimePlannerSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Create contract
                    OvertimeContract headerContract = new OvertimeContract();

                    // TODO: assign values to headerContract from dataTable here
                    // headerContract.parmXYZ = dataTable.Rows[0]["XYZ"].ToString();
                    headerContract.employeeId = dataTable.Rows[0]["employeeId"]?.ToString() ?? "";
                    headerContract.employeeName = dataTable.Rows[0]["employeeName"]?.ToString() ?? "";
                    headerContract.startTime = dataTable.Rows[0]["startTime"] == DBNull.Value ? 0 : Convert.ToInt32(dataTable.Rows[0]["startTime"]);
                    headerContract.endTime = dataTable.Rows[0]["endTime"] == DBNull.Value ? 0 : Convert.ToInt32(dataTable.Rows[0]["endTime"]);
                    headerContract.planDate = Convert.ToDateTime(dataTable.Rows[0]["planDate"]);
                    headerContract.requestDate = Convert.ToDateTime(dataTable.Rows[0]["requestDate"]);

                    // Most important line
                    headerContract.recId = dataTable.Rows[0]["recId"] == DBNull.Value ? 0 : Convert.ToInt64(dataTable.Rows[0]["recId"]);

                    // Optional: force check
                    if (headerContract.recId <= 0)
                    {
                        objBOL.isSuccess = false;
                        objBOL.Message = "RecId is missing or zero before calling service.";
                        return objBOL;
                    }




                    // Call service
                    GeneralContract results = ((OvertimePlannerSvc)channel)
                        .updateAsync(new update(callContext, headerContract))
                        .Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return objBOL;  // ✅ always returns a value
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
                var client = new OvertimePlannerSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((OvertimePlannerSvc)channel).deleteAsync(new delete(callContext, _recordsRecId)).Result.result;
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
            dataTable = RetrieveDatatable.createDataTable(new OvertimeContract[] { });
            dataTable.TableName = tableName;

            return dataTable; 

        }
    } 
    }
