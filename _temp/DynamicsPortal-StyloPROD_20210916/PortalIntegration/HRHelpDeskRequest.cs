using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ESSHRHelpDeskRequestSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class ESSHRHelpDeskRequest
    {
        private readonly string serviceName = "ESSHRHelpDeskRequestSvcGroup";
        public string tableName = "ESSHRHelpDeskRequest";
        public string actionItem = "Help Desk Request";

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

                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
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
                        Int64.TryParse(dataRow["EmpId"].ToString(), out employeeId);

                        ESSHRHelpDeskRequestSvcContract eSSHRHelpDeskRequestContract = new ESSHRHelpDeskRequestSvcContract();
                        eSSHRHelpDeskRequestContract.EmpId = employeeId;
                        eSSHRHelpDeskRequestContract.ESSHRHelpDeskRequestId = dataRow["ESSHRHelpDeskRequestId"].ToString();
                        //eSSHRHelpDeskRequestContract.EmployeeID = dataRow["EmployeeID"].ToString();
                        //eSSHRHelpDeskRequestContract.EmployeeName = dataRow["EmployeeName"].ToString();
                        eSSHRHelpDeskRequestContract.ESSDivision = dataRow["ESSDivision"].ToString();
                        eSSHRHelpDeskRequestContract.Detail = dataRow["Detail"].ToString();

                        eSSHRHelpDeskRequestContract.ESSDate = dataRow["ESSDate"].ToString().toDateTime();

                        eSSHRHelpDeskRequestContract.ESSTypeOfProblem = dataRow["ESSTypeOfProblem"] is DBNull ? ESSTypeOfProblem.Others : (ESSTypeOfProblem)Enum.Parse(typeof(ESSTypeOfProblem), dataRow["ESSTypeOfProblem"].ToString());
                        eSSHRHelpDeskRequestContract.TransactionStatus = dataRow["TransactionStatus"] is DBNull ? ESSTransactionStatus.Requested : (ESSTransactionStatus)Enum.Parse(typeof(ESSTransactionStatus), dataRow["TransactionStatus"].ToString());

                        eSSHRHelpDeskRequestContract.RequestedBy = SessionVariables.getCurrentEmployeeId();

                        GeneralContract results = ((ESSHRHelpDeskRequestSvc)channel).createAsync(new create(callContext, eSSHRHelpDeskRequestContract)).Result.result;
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

                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
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

                        ESSHRHelpDeskRequestSvcContract eSSHRHelpDeskRequestContract = new ESSHRHelpDeskRequestSvcContract();

                        eSSHRHelpDeskRequestContract.Detail = dataRow["Detail"].ToString();
                        eSSHRHelpDeskRequestContract.ESSDate = dataRow["ESSDate"].ToString().toDateTime();
                        eSSHRHelpDeskRequestContract.ESSTypeOfProblem = dataRow["ESSTypeOfProblem"] is DBNull ? ESSTypeOfProblem.Others :
                                                                        (ESSTypeOfProblem)Enum.Parse(typeof(ESSTypeOfProblem), dataRow["ESSTypeOfProblem"].ToString());
                        //eSSHRHelpDeskRequestContract.TransactionStatus = dataRow["TransactionStatus"] is DBNull ? ESSTransactionStatus.Requested : (ESSTransactionStatus)Enum.Parse(typeof(ESSTransactionStatus), dataRow["TransactionStatus"].ToString());

                        eSSHRHelpDeskRequestContract.RecId = RecId;

                        GeneralContract results = ((ESSHRHelpDeskRequestSvc)channel).updateAsync(new update(callContext, eSSHRHelpDeskRequestContract)).Result.result;
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
                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((ESSHRHelpDeskRequestSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
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
                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSHRHelpDeskRequestSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSHRHelpDeskRequestSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

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
                var client = new ESSHRHelpDeskRequestSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSHRHelpDeskRequestSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new ESSHRHelpDeskRequestSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
        //public DataTable createDataTable()
        //{
        //    DataTable dataTable = new DataTable(tableName);
        //    dataTable.Columns.Add("ESSHRHelpDeskRequestId");
        //    dataTable.Columns.Add("EmpId");
        //    dataTable.Columns.Add("EmployeeID");
        //    dataTable.Columns.Add("EmployeeName");
        //    dataTable.Columns.Add("Detail");
        //    dataTable.Columns.Add("ESSDate");
        //    dataTable.Columns.Add("ESSDivision");
        //    dataTable.Columns.Add("ESSTypeOfProblem");
        //    dataTable.Columns.Add("ESSWorkFlowStatus");
        //    dataTable.Columns.Add("TransactionStatus");
        //    dataTable.Columns.Add("RecId");
        //    return dataTable;
        //    //ESSHRHelpDeskRequestSvcContract eSSHRHelpDeskRequestSvcContract = new ESSHRHelpDeskRequestSvcContract();
        //    //eSSHRHelpDeskRequestSvcContract.ESSHRHelpDeskRequestId;
        //    //eSSHRHelpDeskRequestSvcContract.EmpId;                //- C
        //    //eSSHRHelpDeskRequestSvcContract.Detail;               //- CU
        //    //eSSHRHelpDeskRequestSvcContract.ESSDate;              //- CU
        //    //eSSHRHelpDeskRequestSvcContract.ESSTypeOfProblem;     //- CU
        //    //eSSHRHelpDeskRequestSvcContract.TransactionStatus;
        //    //eSSHRHelpDeskRequestSvcContract.ESSDivision;
        //    //eSSHRHelpDeskRequestSvcContract.EmployeeID;
        //    //eSSHRHelpDeskRequestSvcContract.EmployeeName;
        //    //eSSHRHelpDeskRequestSvcContract.ESSWorkFlowStatus;
        //    //eSSHRHelpDeskRequestSvcContract.RecId;
        //}

    }

}
