using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmPersonEducationSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HcmPersonEducation
    {
        private readonly string serviceName = "HcmPersonEducationSvcGroup";
        public string tableName = "HcmPersonEducation";
        public string actionItem = "Education";

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

                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
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
                        HcmPersonEducationSvcContract hcmPersonEducationSvcContract = new HcmPersonEducationSvcContract();

                        long person = 0;
                        Int64.TryParse(dataRow["Person"].ToString(), out person);
                        long educationDiscipline = 0;
                        Int64.TryParse(dataRow["EducationDiscipline"].ToString(), out educationDiscipline);
                        long duration = 0;
                        Int64.TryParse(dataRow["Duration"].ToString(), out duration);

                        //hcmPersonEducationSvcContract.EducationDisciplineId = dataRow["EducationDisciplineId"].ToString();
                        hcmPersonEducationSvcContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        hcmPersonEducationSvcContract.Description = dataRow["Description"].ToString();
                        hcmPersonEducationSvcContract.Notes = dataRow["Notes"].ToString();
                        //hcmPersonEducationSvcContract.WorkflowOperation = dataRow["WorkflowOperation"].ToString();
                        hcmPersonEducationSvcContract.ApprovalStatus = dataRow["ApprovalStatus"].ToString();

                        hcmPersonEducationSvcContract.DurationUnit = dataRow["DurationUnit"] is DBNull ? PeriodUnitPI.Day :
                                                                                 (PeriodUnitPI)Enum.Parse(typeof(PeriodUnitPI), dataRow["DurationUnit"].ToString());
                        
                        hcmPersonEducationSvcContract.StartDate = dataRow["StartDate"].ToString().toDateTime();
                        hcmPersonEducationSvcContract.EndDate = dataRow["EndDate"].ToString().toDateTime();

                        hcmPersonEducationSvcContract.Person = person;
                        hcmPersonEducationSvcContract.Duration = duration;
                        hcmPersonEducationSvcContract.EducationDiscipline = educationDiscipline;
                        hcmPersonEducationSvcContract.WorkflowOperation = HcmWorkflowOperation.Insert;

                        GeneralContract results = ((HcmPersonEducationSvc)channel).createAsync
                                     (new create(callContext, hcmPersonEducationSvcContract)).Result.result;

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

                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
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
                        HcmPersonEducationSvcContract hcmPersonEducationSvcContract = new HcmPersonEducationSvcContract();

                        long person = 0;
                        Int64.TryParse(dataRow["Person"].ToString(), out person);
                        long educationDiscipline = 0;
                        Int64.TryParse(dataRow["EducationDiscipline"].ToString(), out educationDiscipline);
                        decimal duration = 0;
                        Decimal.TryParse(dataRow["Duration"].ToString(), out duration);

                        hcmPersonEducationSvcContract.EducationDisciplineId = dataRow["EducationDisciplineId"].ToString();
                        hcmPersonEducationSvcContract.EmployeeId = dataRow["EmployeeId"].ToString();
                        hcmPersonEducationSvcContract.Description = dataRow["Description"].ToString();
                        hcmPersonEducationSvcContract.Notes = dataRow["Notes"].ToString();
                        //hcmPersonEducationSvcContract.WorkflowOperation = dataRow["WorkflowOperation"].ToString();
                        hcmPersonEducationSvcContract.ApprovalStatus = dataRow["ApprovalStatus"].ToString();

                        hcmPersonEducationSvcContract.DurationUnit = dataRow["DurationUnit"] is DBNull ? PeriodUnitPI.Day :
                                                                                 (PeriodUnitPI)Enum.Parse(typeof(PeriodUnitPI), dataRow["DurationUnit"].ToString());

                        hcmPersonEducationSvcContract.StartDate = dataRow["StartDate"].ToString().toDateTime();
                        hcmPersonEducationSvcContract.EndDate = dataRow["EndDate"].ToString().toDateTime();

                        hcmPersonEducationSvcContract.Person = person;
                        hcmPersonEducationSvcContract.Duration = duration;
                        hcmPersonEducationSvcContract.EducationDiscipline = educationDiscipline;

                        hcmPersonEducationSvcContract.RecId = recId;
                        hcmPersonEducationSvcContract.WorkflowOperation = HcmWorkflowOperation.Update;



                        GeneralContract results = ((HcmPersonEducationSvc)channel).updateAsync(new update(callContext, hcmPersonEducationSvcContract)).Result.result;

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
                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HcmPersonEducationSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;

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

        public SysOperationResult_BOL deleteRecords(GeneralContract[] _recordsDetails)
        {
            GeneralContract[] recordsDetails = _recordsDetails;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((HcmPersonEducationSvc)channel).deleteMapAsync(new deleteMap(callContext, recordsDetails)).Result.result;

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
                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    HcmPersonEducationSvcContract[] results = ((HcmPersonEducationSvc)channel).findByEmployeeAsync
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
                var client = new HcmPersonEducationSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    HcmPersonEducationSvcContract[] results = ((HcmPersonEducationSvc)channel).retriveEmployeeReporteesAsync
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
            dataTable = RetrieveDatatable.createDataTable(new HcmPersonEducationSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
