using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using PortalIntegration.PROutstandingAdvancesSummarySvcReference;

namespace PortalIntegration
{
    public class PROutstandingAdvances
    {
        private readonly string serviceName = "PROutstandingAdvancesSummaryGroup";
        public string tableName = "PROutstandingAdvancesSummary";
        

        public DataTable getOutstandingAdvances()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PROutstandingAdvancesSummarySvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PROutstandingAdvancesSummarySvc)channel).getOutstandingAdvancesAsync
                                 (new getOutstandingAdvances(callContext, employeeId, "", "")).Result.result;

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
                return new DataTable();
            }
            finally
            { }
        }
    }
}
