using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HRHiringRequisitionDetailsSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HRHiringRequisitionDetails
    {
        private readonly string serviceName = "HRHiringRequisition_DetailsSvcGroup";
        public string tableName = "HRHiringRequisition_Details";
        public string actionItem = "Hiring Requisition Details";

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

                var client = new HRHiringRequisition_DetailsSvcClient(binding, endpointAddress);
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
                        int vacancies = 0;
                        Int32.TryParse(dataRow["Vacancies"].ToString(), out vacancies);

                        HRHiringRequisition_DetailsSvcContract hRHiringRequisitionDetailsContract = new HRHiringRequisition_DetailsSvcContract();
                        hRHiringRequisitionDetailsContract.Designation = dataRow["Designation"].ToString();
                        hRHiringRequisitionDetailsContract.Education = dataRow["Education"].ToString();
                        hRHiringRequisitionDetailsContract.Experience = dataRow["Experience"].ToString();
                        hRHiringRequisitionDetailsContract.HRSerialNumber = dataRow["HRSerialNumber"].ToString();
                        hRHiringRequisitionDetailsContract.JobDescription = dataRow["JobDescription"].ToString();
                        hRHiringRequisitionDetailsContract.Location = dataRow["Location"].ToString();
                        hRHiringRequisitionDetailsContract.Remarks = dataRow["Remarks"].ToString();
                        hRHiringRequisitionDetailsContract.Vacancies = vacancies;
                        //hRHiringRequisitionDetailsContract.ExpectedHiringDate = dataRow["ExpectedHiringDate"].ToString().toDateTime();
                        ////hRHiringRequisitionDetailsContract.RecId;
                        ////hRHiringRequisitionDetailsContract.MonthsOfYear;
                        ////hRHiringRequisitionDetailsContract.DepartmentID;
                        ////hRHiringRequisitionDetailsContract.BudgetYear;
                        ////hRHiringRequisitionDetailsContract.EstimatedMonthlySalary;
                        ////hRHiringRequisitionDetailsContract.AdditionalBenifts;

                        //hRHiringRequisitionDetailsContract.RequestedBy = SessionVariables.getCurrentEmployeeId();


                        GeneralContract results = ((HRHiringRequisition_DetailsSvc)channel).createAsync
                                     (new create(callContext, hRHiringRequisitionDetailsContract)).Result.result;

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

                var client = new HRHiringRequisition_DetailsSvcClient(binding, endpointAddress);
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

                        int vacancies = 0;
                        Int32.TryParse(dataRow["Vacancies"].ToString(), out vacancies);

                        HRHiringRequisition_DetailsSvcContract hRHiringRequisitionDetailsContract = new HRHiringRequisition_DetailsSvcContract();
                        hRHiringRequisitionDetailsContract.Designation = dataRow["Designation"].ToString();
                        hRHiringRequisitionDetailsContract.Education = dataRow["Education"].ToString();
                        hRHiringRequisitionDetailsContract.Experience = dataRow["Experience"].ToString();
                        hRHiringRequisitionDetailsContract.JobDescription = dataRow["JobDescription"].ToString();
                        hRHiringRequisitionDetailsContract.Location = dataRow["Location"].ToString();
                        hRHiringRequisitionDetailsContract.Remarks = dataRow["Remarks"].ToString();
                        hRHiringRequisitionDetailsContract.Vacancies = vacancies;
                        hRHiringRequisitionDetailsContract.RecId = recId;
                        //hRHiringRequisitionDetailsContract.HRSerialNumber = dataRow["HRSerialNumber"].ToString();
                        //hRHiringRequisitionDetailsContract.MonthsOfYear;
                        //hRHiringRequisitionDetailsContract.DepartmentID;
                        //hRHiringRequisitionDetailsContract.BudgetYear;
                        //hRHiringRequisitionDetailsContract.EstimatedMonthlySalary;
                        //hRHiringRequisitionDetailsContract.AdditionalBenifts;

                        GeneralContract results = ((HRHiringRequisition_DetailsSvc)channel).updateAsync
                                    (new update(callContext, hRHiringRequisitionDetailsContract)).Result.result;

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
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            SysOperationResult_BOL objBOL = null;
            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();
            var client = new HRHiringRequisition_DetailsSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                objBOL = new SysOperationResult_BOL();
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                CallContext callContext = new CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();
                callContext.Company = dataAreaId;

                GeneralContract[] results = ((HRHiringRequisition_DetailsSvc)channel).deleteAsync
                                            (new delete(callContext, recordsRecId)).Result.result;
                objBOL = SysOperationResults.operationResults(results);

                SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, results);

            }
            return objBOL;
        }


        public DataTable retrieveAllHiringRequisitionDetails(string _serialNumber)
        {
            string serialNumber = _serialNumber;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRHiringRequisition_DetailsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HRHiringRequisition_DetailsSvc)channel).findAsync(new find(callContext, serialNumber)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new HRHiringRequisition_DetailsSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
