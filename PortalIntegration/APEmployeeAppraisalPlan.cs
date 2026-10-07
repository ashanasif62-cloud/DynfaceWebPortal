using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.APEmployeeAppraisalPlanSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class APEmployeeAppraisalPlan
    {
        private readonly string serviceName = "APEmployeeAppraisalPlanSvcGroup";
        public string tableName = "APAppraisalPlan";
        public string actionItem = "Employee Plan KPIs";

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

                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
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
                        string employeeId = dataRow["EmployeeId"].ToString();
                        decimal KPICode = 0;
                        Decimal.TryParse(dataRow["KPICode"].ToString(), out KPICode);
                        decimal KPIWeightage = 0;
                        Decimal.TryParse(dataRow["KPIWeightage"].ToString(), out KPIWeightage);
                        decimal Description = 0;
                        Decimal.TryParse(dataRow["Description"].ToString(), out Description);
                        long refrecid = 0; // neet to update from dt.
                        Int64.TryParse(dataRow["RefRecId"].ToString(), out refrecid);


                        //TODO Replace with actual contract
                        APEmployeeAppraisalPlanContract employeeAppraisalPlan = new APEmployeeAppraisalPlanContract();
                        employeeAppraisalPlan.KPICode = dataRow["KPICode"].ToString();
                        employeeAppraisalPlan.AppraisalCode = employeeId + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                        employeeAppraisalPlan.KPICode = dataRow["KPICode"].ToString();//- CU
                        employeeAppraisalPlan.Description = dataRow["Description"].ToString();//- CU
                        employeeAppraisalPlan.KPIWeightage = KPIWeightage;
                        employeeAppraisalPlan.RefRecId = refrecid;// refrecid;//- CU
                        employeeAppraisalPlan.__k_parmEmployeeId = employeeId;
                        employeeAppraisalPlan.EmployeeId = employeeId;//- C
                        employeeAppraisalPlan.__k_parmAppraisalCode = employeeId + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                        employeeAppraisalPlan.__k_parmRefRecId = refrecid;
                        employeeAppraisalPlan.__k_parmKPICode = dataRow["KPICode"].ToString(); 
                        employeeAppraisalPlan.__k_parmDescription = dataRow["Description"].ToString();
                        employeeAppraisalPlan.__k_parmKPIWeightage = KPIWeightage;

                        //employeeAppraisalPlan.
                        GeneralContract results = ((APEmployeeAppraisalPlanSVC)channel).createAsync(new create(callContext, employeeAppraisalPlan)).Result.result;
                        
                        if(results.RecId > 0)//remove after deployemnt
                        {
                            GeneralContract generalContract = new GeneralContract();
                            generalContract.IsSuccess = true;
                            generalContract.Message = "KPI created Successfully.";
                            generalContract.RecId = results.RecId;
                            
                            objBOL = SysOperationResults.operationResult<GeneralContract>(generalContract);
                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);

                        }

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
            {
            }
            return objBOL;
        }

        public SysOperationResult_BOL update(DataTable _objDT, long _KPIRecId)
        {
            long KPIRecId = _KPIRecId;
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
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
                        //Minor Modification
                  //      long refrecid = 0; // neet to update from dt.
                    //    Int64.TryParse(dataRow["RefRecId"].ToString(), out refrecid);
                        string employeeId = dataRow["EmployeeId"].ToString();
                        //decimal leaveDays = 0;
                        //Decimal.TryParse(dataRow["LeaveDays"].ToString(), out leaveDays);
                        decimal KPIWeightage = 0;
                        Decimal.TryParse(dataRow["KPIWeightage"].ToString(), out KPIWeightage);
                        long recid = 0;
                        long.TryParse(dataRow["recid"].ToString(), out recid);

                        APEmployeeAppraisalPlanContract employeeAppraisalPlan = new APEmployeeAppraisalPlanContract();

                        employeeAppraisalPlan.AppraisalCode = dataRow["AppraisalCode"].ToString();//- CU
                        employeeAppraisalPlan.KPICode = dataRow["KPICode"].ToString();//- CU
                        employeeAppraisalPlan.Description = dataRow["Description"].ToString();//- CU
                        employeeAppraisalPlan.KPIWeightage = KPIWeightage;
                        employeeAppraisalPlan.RefRecId = recid;
                        employeeAppraisalPlan.__k_parmEmployeeId = employeeId;
                        employeeAppraisalPlan.EmployeeId = employeeId;//- C
                        employeeAppraisalPlan.__k_parmAppraisalCode = dataRow["AppraisalCode"].ToString();
                        employeeAppraisalPlan.__k_parmRefRecId = recid;
                        employeeAppraisalPlan.__k_parmKPICode = dataRow["KPICode"].ToString();
                        employeeAppraisalPlan.__k_parmDescription = dataRow["Description"].ToString();
                        employeeAppraisalPlan.__k_parmKPIWeightage = KPIWeightage;
                        employeeAppraisalPlan.__k_parmKPIRecId = KPIRecId;
                        employeeAppraisalPlan.kpiRecId = KPIRecId;
                        //employeeAppraisalPlan.__k_parmKPIRecId

                        employeeAppraisalPlan.RefRecId = recid;

                        GeneralContract results = ((APEmployeeAppraisalPlanSVC)channel).updateAsync(new update(callContext,employeeAppraisalPlan)).Result.result;
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

        public SysOperationResult_BOL delete(long _KPIRecId)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {

                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((APEmployeeAppraisalPlanSVC)channel).deleteKPIAsync(new deleteKPI(callContext, _KPIRecId)).Result.result;
                    if (results.RecId > 0)//remove after deployemnt
                    {
                        GeneralContract generalContract = new GeneralContract();
                        generalContract.IsSuccess = true;
                        generalContract.Message = "KPI Deleted Successfully.";
                        generalContract.RecId = results.RecId;


                        objBOL = SysOperationResults.operationResult<GeneralContract>(generalContract);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, objBOL);
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

        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((APEmployeeAppraisalPlanSVC)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    //AppraisalCode appraisalCode = new AppraisalCode();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((APEmployeeAppraisalPlanSVC)channel).RetrieveByEmployeeAsync(new RetrieveByEmployee(callContext, employeeId)).Result.result;

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

        public DataTable retriveEmployeeAppraisalsPlan()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    //string tmpAppraisalCode = "000003795 - 2022";
                    //long employeeRecId = 5637146254;
                    var results = ((APEmployeeAppraisalPlanSVC)channel).RetrieveByEmployeeAsync(new RetrieveByEmployee(callContext, employeeId)).Result.result;

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

        public DataTable retriveAll()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalPlanSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;


                    var results = ((APEmployeeAppraisalPlanSVC)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new APEmployeeAppraisalPlanContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
