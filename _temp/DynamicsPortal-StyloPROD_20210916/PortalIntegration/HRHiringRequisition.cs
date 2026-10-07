using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HRHiringRequisitionSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HRHiringRequisition
    {
        private readonly string serviceName = "HRHiringRequisitionSvcGroup";
        public string tableName = "HRHiringRequisition";
        public string actionItem = "Hiring Requisition";

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

                var client = new HRHiringRequisitionSvcClient(binding, endpointAddress);
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

                        long requestedBy = 0;
                        Int64.TryParse(dataRow["RequestedBy"].ToString(), out requestedBy);

                        HRHiringRequisitionSvcContract hRHiringRequisitionContract = new HRHiringRequisitionSvcContract();
                        hRHiringRequisitionContract.RequestedBy = requestedBy;
                        //hRHiringRequisitionContract.DepartmentID = dataRow["DepartmentID"].ToString();
                        hRHiringRequisitionContract.Remarks = dataRow["Remarks"].ToString();
                        hRHiringRequisitionContract.RequestDate = dataRow["RequestDate"].ToString().toDateTime();

                        //hRHiringRequisitionContract.HRSerialNumber = dataRow["HRSerialNumber"].ToString();
                        //hRHiringRequisitionContract.Experience = dataRow["Experience"].ToString();
                        //hRHiringRequisitionContract.HRWFStatus = dataRow["HRWFStatus"].ToString();
                        //hRHiringRequisitionContract.RecId = dataRow["RecId"].ToString();
                        //hRHiringRequisitionContract.Education;
                        //hRHiringRequisitionContract.EstimatedMonthlySalary;
                        //hRHiringRequisitionContract.Designation;
                        //hRHiringRequisitionContract.Vacancies;
                        //hRHiringRequisitionContract.Year;
                        //hRHiringRequisitionContract.MonthsOfYear;
                        //hRHiringRequisitionContract.BudgetYear;
                        //hRHiringRequisitionContract.Location;
                        //hRHiringRequisitionContract.DepartmentName;
                        //hRHiringRequisitionContract.RequestedByName;

                        hRHiringRequisitionContract.RequestedByWF = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((HRHiringRequisitionSvc)channel).createAsync
                                     (new create(callContext, hRHiringRequisitionContract)).Result.result;

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

        /*
        
        public SysOperationResult_BOL update(DataTable _objDT, long _recId)
        {
            DataTable dataTable = _objDT;
            long recId = _recId;
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            SysOperationResult_BOL objBOL = null;

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new HRHiringRequisitionSvcClient(binding, endpointAddress);
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
                    objBOL = new SysOperationResult_BOL();

                    HRHiringRequisitionSvcContract hRHiringRequisitionContract = new HRHiringRequisitionSvcContract();
                    long requestedBy = 0;
                    Int64.TryParse(dataRow["RequestedBy"].ToString(), out requestedBy);
                    
                    hRHiringRequisitionContract.RequestedBy = requestedBy;
                    //hRHiringRequisitionContract.DepartmentID = dataRow["DepartmentID"].ToString();
                    hRHiringRequisitionContract.Remarks = dataRow["Remarks"].ToString();
                    hRHiringRequisitionContract.RequestDate = dataRow["RequestDate"].ToString().toDateTime();

                    //hRHiringRequisitionContract.HRSerialNumber = dataRow["HRSerialNumber"].ToString();
                    //hRHiringRequisitionContract.HRWFStatus = dataRow["HRWFStatus"].ToString();
                    //hRHiringRequisitionContract.Experience = dataRow["Experience"].ToString();
                    //hRHiringRequisitionContract.RecId = dataRow["RecId"].ToString();
                    //hRHiringRequisitionContract.Education;
                    //hRHiringRequisitionContract.EstimatedMonthlySalary;
                    //hRHiringRequisitionContract.Designation;
                    //hRHiringRequisitionContract.Vacancies;
                    //hRHiringRequisitionContract.Year;
                    //hRHiringRequisitionContract.MonthsOfYear;
                    //hRHiringRequisitionContract.BudgetYear;
                    //hRHiringRequisitionContract.Location;

                    hRHiringRequisitionContract.RecId = recId;

                    GeneralContract results = ((HRHiringRequisitionSvc)channel).updateAsync
                                (new update(callContext, hRHiringRequisitionContract)).Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                }
            }
            return objBOL;
        }
     */

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
                var client = new HRHiringRequisitionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HRHiringRequisitionSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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

        public DataTable retrieveAllHiringRequisition()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRHiringRequisitionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HRHiringRequisitionSvc)channel).retrieveAllAsync
                                                                    (new retrieveAll(callContext)).Result.result;

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

        public DataTable retrieveHiringRequisitions(string _employeeId)
        {
            string employeeId = _employeeId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRHiringRequisitionSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HRHiringRequisitionSvc)channel).retrieveAsync
                                                                    (new retrieve(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new HRHiringRequisitionSvcContract[] { });
            return dataTable;
        }

    }
}
