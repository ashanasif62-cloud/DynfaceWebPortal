using GeneralAuxiliary;
using PortalIntegration.APEmployeeAppraisalsSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class APEmployeeAppraisals
    {
        private readonly string serviceName = "APEmployeeAppraisalsSvcGroup";
        public string tableName = "APEmployeeAppraisals";

        public DataTable retriveAppraisalsAssignedToMe()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((APEmployeeAppraisalsSvc)channel).appraisalAssignedToMeAsync(new appraisalAssignedToMe(callContext, employeeId)).Result.result;

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

        public DataTable retriveDailyGroomingAppraisalsAssignedToMe()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                APReviewType reviewType = APReviewType.DailyGrooming;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APEmployeeAppraisalsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((APEmployeeAppraisalsSvc)channel)
                        .appraisalAssignedToMeByReviewTypeAsync(new appraisalAssignedToMeByReviewType(callContext, employeeId, reviewType)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new APEmployeeAppraisalsSvcContract[] { });
            return dataTable;
            //APEmployeeAppraisalsSvcContract aPEmployeeAppraisalsSvcContract = new APEmployeeAppraisalsSvcContract();
            //aPEmployeeAppraisalsSvcContract.Score;
            //aPEmployeeAppraisalsSvcContract.TotalScore;
        }

    }
}
