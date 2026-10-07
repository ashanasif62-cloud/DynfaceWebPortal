using GeneralAuxiliary;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HcmWorkerDetails
    {
        private readonly string serviceName = "HcmWorkerDetailsSvcGroup";
        public string tableName = "HcmWorker";
        
        public HcmWorkerDetailsSvcContract retrieveWorkerDetails(string _employeeId)
        {
            string employeeId = _employeeId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                DateTime lastWorkingDate = DateTime.Today;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HcmWorkerDetailsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    HcmWorkerDetailsSvcContract results = ((HcmWorkerDetailsSvc)channel).RetrieveWorkerDetailsAsync(new RetrieveWorkerDetails(callContext, employeeId, lastWorkingDate)).Result.result;
                    return results;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return null;
            }
            finally
            { }
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new HcmWorkerDetailsSvcContract[] { });
            return dataTable;
        }
    }
}
