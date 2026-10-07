using GeneralAuxiliary;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class ESSEmployeeLegalEntities
    {
        private readonly string serviceName = "HcmWorkerDetailsSvcGroup";

        public string[] retrieveEmployeeLegalEntities(string _employeeId)
        {
            string employeeId = _employeeId;
            string[] resultsSet = new string[] { };
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

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

                    resultsSet = ((HcmWorkerDetailsSvc)channel)
                        .employeeLegalEntitiesAsync(new employeeLegalEntities(callContext, employeeId)).Result.result;
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
            return resultsSet;
        }

    }
}
