//using BussinessObject;
//using GeneralAuxiliary;
//using PortalIntegration.APEmployeeAppraisalsKPIAppraisersSvcReference;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.ServiceModel;
//using System.ServiceModel.Channels;

//namespace PortalIntegration
//{
//    public class APEmployeeAppraisalsKPIAppraisers
//    {
//        private readonly string serviceName = "APEmployeeAppraisalsKPIAppraisersSvcGroup";
//        public string tableName = "APEmployeeAppraisalsKPIAppraisers";
//        public string actionItem = "Employee Appraisals";

//        public SysOperationResult_BOL update(DataTable _objDT, long _employeeAppraisalId)
//        {
//            DataTable dataTable = _objDT;
//            long employeeAppraisalId = _employeeAppraisalId;
//            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
//            try
//            {
//                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

//                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
//                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

//                var endpointAddress = new EndpointAddress(serviceUriString);
//                var binding = SoapHelper.GetBinding();

//                var client = new APEmployeeAppraisalsKPIAppraisersSvcClient(binding, endpointAddress);
//                var channel = client.InnerChannel;

//                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
//                {
//                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
//                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
//                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

//                    CallContext callContext = new CallContext();
//                    callContext.MessageId = Guid.NewGuid().ToString();
//                    callContext.Company = dataAreaId;

//                    List<APEmployeeAppraisalsKPIAppraisersSvcContract> employeeAppraisalsKPIAppraisersList = new List<APEmployeeAppraisalsKPIAppraisersSvcContract>();

//                    foreach (DataRow dataRow in dataTable.Rows)
//                    {
//                        decimal score = 0;
//                        Decimal.TryParse(dataRow["Score"].ToString(), out score);
//                        long recId = 0;
//                        Int64.TryParse(dataRow["RecId"].ToString(), out recId);
                    
//                        //Decimal.TryParse(dataRow["Comments"].ToString(), out comments);
//                        //Modification

//                        APEmployeeAppraisalsKPIAppraisersSvcContract employeeAppraisalsKPIAppraisersSvcContract = new APEmployeeAppraisalsKPIAppraisersSvcContract();
//                        employeeAppraisalsKPIAppraisersSvcContract.Score = score;
//                        employeeAppraisalsKPIAppraisersSvcContract.RecId = recId;
//                        employeeAppraisalsKPIAppraisersSvcContract.Comments = dataRow["Comments"].ToString();

//                        employeeAppraisalsKPIAppraisersList.Add(employeeAppraisalsKPIAppraisersSvcContract);
//                    }

//                    GeneralContract[] results = ((APEmployeeAppraisalsKPIAppraisersSvc)channel).updateAsync(new update(callContext, employeeAppraisalsKPIAppraisersList.ToArray(), employeeAppraisalId)).Result.result;
//                    objBOL = SysOperationResults.operationResults(results);

//                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
//                string currentMethodName = currentMethod.DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//            finally
//            { }
//            return objBOL;
//        }

//        public SysOperationResult_BOL review( long _employeeAppraisalId, int _currentAppraiserSequence)
//        {
//            long employeeAppraisalId = _employeeAppraisalId;
//            String assignedTo = SessionVariables.getCurrentEmployeeId();

//            int currentAppraiserSequence = _currentAppraiserSequence;
//            //long employeeAppraisalRecId = _employeeAppraisalRecId;
          
//            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
//            try
//            {
//                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
             

//                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
//                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

//                var endpointAddress = new EndpointAddress(serviceUriString);
//                var binding = SoapHelper.GetBinding();

//                var client = new APEmployeeAppraisalsKPIAppraisersSvcClient(binding, endpointAddress);
//                var channel = client.InnerChannel;

//                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
//                {
//                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
//                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
//                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

//                    CallContext callContext = new CallContext();
//                    callContext.MessageId = Guid.NewGuid().ToString();
//                    callContext.Company = dataAreaId;
//                    //review<APEmployeeAppraisalsKPIAppraisersSvcContract> employeeAppraisalsKPIAppraisersList = new review<APEmployeeAppraisalsKPIAppraisersSvcContract>();


//                    // pass curent employee id as assigned to, and apprasersequence from below grid.
//                    GeneralContract results = ((APEmployeeAppraisalsKPIAppraisersSvc)channel).reviewedAsync
//                                                (new reviewed(callContext,assignedTo,_currentAppraiserSequence, _employeeAppraisalId)).Result.result;

//                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
//                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
//                    //}
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
//                string currentMethodName = currentMethod.DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//            finally
//            { }
//            return objBOL;
//        }

//        public SysOperationResult_BOL sendToLineManager(long _employeeAppraisalId)
//        {
//            long employeeAppraisalId = _employeeAppraisalId;
//            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
//            try
//            {
//                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
//                string employeeId = SessionVariables.getCurrentEmployeeId();

//                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
//                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

//                var endpointAddress = new EndpointAddress(serviceUriString);
//                var binding = SoapHelper.GetBinding();

//                var client = new APEmployeeAppraisalsKPIAppraisersSvcClient(binding, endpointAddress);
//                var channel = client.InnerChannel;

//                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
//                {
//                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
//                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
//                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

//                    CallContext callContext = new CallContext();
//                    callContext.MessageId = Guid.NewGuid().ToString();
//                    callContext.Company = dataAreaId;

//                    GeneralContract results = ((APEmployeeAppraisalsKPIAppraisersSvc)channel).sendToLineManagerAsync
//                                                (new sendToLineManager(callContext, employeeAppraisalId, employeeId)).Result.result;
//                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

//                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
//                string currentMethodName = currentMethod.DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//            finally
//            { }
//            return objBOL;
//        }

//        public DataTable reteriveEmployeeKPIAppraisers(long _employeeAppraisalId, string _employeeId)
//        {
//            long employeeAppraisalId = _employeeAppraisalId;
//            string employeeId = _employeeId;
//            try
//            {
//                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

//                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
//                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
//                var endpointAddress = new EndpointAddress(serviceUriString);
//                var binding = SoapHelper.GetBinding();
//                var client = new APEmployeeAppraisalsKPIAppraisersSvcClient(binding, endpointAddress);
//                var channel = client.InnerChannel;

//                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
//                {
//                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
//                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
//                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

//                    CallContext callContext = new CallContext();
//                    callContext.MessageId = Guid.NewGuid().ToString();
//                    callContext.Company = dataAreaId;

//                    var results = ((APEmployeeAppraisalsKPIAppraisersSvc)channel).reteriveKPIAppraisersAsync(
//                                    new reteriveKPIAppraisers(callContext, employeeAppraisalId, employeeId)).Result.result;

//                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
//                    return dataTable;
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
//                string currentMethodName = currentMethod.DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//                return createDataTable();
//            }
//            finally
//            { }
//        }

//        public DataTable createDataTable()
//        {
//            DataTable dataTable = new DataTable();
//            dataTable = RetrieveDatatable.createDataTable(new APEmployeeAppraisalsKPIAppraisersSvcContract[] { });
//            return dataTable;
//        }

//        //public DataTable createDataTable()
//        //{
//        //    DataTable dataTable = new DataTable(tableName);
//        //    dataTable.Columns.Add("Appraisers");
//        //    dataTable.Columns.Add("AppraiserStatus");
//        //    dataTable.Columns.Add("AssignedTo");
//        //    dataTable.Columns.Add("Comments");
//        //    dataTable.Columns.Add("FinalScore");
//        //    dataTable.Columns.Add("KPIDescription");
//        //    dataTable.Columns.Add("KPIWeightage");
//        //    dataTable.Columns.Add("Name");
//        //    dataTable.Columns.Add("Score");
//        //    dataTable.Columns.Add("Weightage");
//        //    dataTable.Columns.Add("RecId");
//        //    dataTable.Columns.Add("RefRecId");
//        //    return dataTable;
//        //    //APEmployeeAppraisalsKPIAppraisersSvcContract employeeAppraisalsKPIAppraisersSvcContract = new APEmployeeAppraisalsKPIAppraisersSvcContract();
//        //    //employeeAppraisalsKPIAppraisersSvcContract.KPIDescription;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.KPIWeightage;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.Score;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.FinalScore;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.Comments;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.Appraisers;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.Weightage;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.AssignedTo;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.Name;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.AppraiserStatus;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.RefRecId;
//        //    //employeeAppraisalsKPIAppraisersSvcContract.RecId;
//        //}
//    }
//}
