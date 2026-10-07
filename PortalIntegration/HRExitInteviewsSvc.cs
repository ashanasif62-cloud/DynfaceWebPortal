using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeneralAuxiliary;
using PortalIntegration.HRExitInterviewSvcRefrence;

namespace PortalIntegration
{
    public class HRExitInteviewsSvc
    {
        private readonly string serviceName = "HREmployeeExitInterviewSvcGroup";
        public string tableName = "HREmployeeExitInterviewQuestions";
        public string actionItem = "HRExitInterView_ListPage";



        public DataTable retrieveQuestions(string referenceId = "")
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                referenceId = referenceId ?? string.Empty;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRExitInterviewSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HRExitInterviewSvc)channel).retrieveEmployeeQuestionsWithMCQsAsync(new retrieveEmployeeQuestionsWithMCQs(callContext, employeeId, referenceId)).Result.result;

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


        //public DataTable retrieveMCQsByQuestion(long questionRecId)
        //{
        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //        string employeeId = SessionVariables.getCurrentEmployeeId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new HRExitInterviewSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            var results = ((HRExitInterviewSvc)channel)
        //                .retrieveMCQsByQuestionAsync(new retrieveMCQsByQuestion(callContext, employeeId, questionRecId))
        //                .Result.result;

        //            return RetrieveDatatable.createDataTable(results);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        objErrorLog.write(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex);
        //        return createDataTable();
        //    }
        //}


    
public bool saveAnswer(
    long questionRecId,
    long empQuestionRecId,
    long mcqRecId,
    string answer)
        {
            try
            {
                string dataAreaId =
                    SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader =
                    OAuthHelper.getAuthenticationHeader();

                var serviceUriString =
                    SoapHelper.GetSoapServiceUriString(
                        serviceName,
                        ClientConfiguration.Default.UriString);

                var endpointAddress =
                    new EndpointAddress(serviceUriString);

                var binding =
                    SoapHelper.GetBinding();

                var client =
                    new HRExitInterviewSvcClient(
                        binding,
                        endpointAddress);

                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope =
                       new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage =
                        new HttpRequestMessageProperty();

                    requestMessage.Headers[OAuthHelper.OAuthHeader] =
                        authenticationHeader;

                    OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name] =
                        requestMessage;

                    CallContext callContext =
                        new CallContext();

                    callContext.MessageId =
                        Guid.NewGuid().ToString();

                    callContext.Company =
                        dataAreaId;

                    // API:
                    // updateAnswerByQuestion(
                    //     RecId _questionRecId,
                    //     RecId _empQuetionRecId,
                    //     RecId _mcqRecId,
                    //     str   _answer
                    // )

                    var request = new updateAnswerByQuestion(
                        callContext,
                        answer,
                        empQuestionRecId,
                        mcqRecId,
                        questionRecId
                    );

                    var result =
                        ((HRExitInterviewSvc)channel)
                        .updateAnswerByQuestionAsync(request)
                        .Result
                        .result;

                    return result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog =
                    new SysErrorLog();

                string currentMethodName =
                    $"{this.GetType().FullName}.{nameof(saveAnswer)}";

                objErrorLog.write(
                    currentMethodName,
                    ex);

                return false;
            }
        }




        public DataTable retrieveEmployeeSurvay()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

             

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRExitInterviewSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((HRExitInterviewSvc)channel).rertrieveAEmployeeSurveysAsync(new rertrieveAEmployeeSurveys(callContext, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new HRExitInterviewContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
