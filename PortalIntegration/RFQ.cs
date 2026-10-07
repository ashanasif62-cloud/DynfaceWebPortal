using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.RFQServiceReference;
using System;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace PortalIntegration
{
    public class RFQ
    {
        private readonly string serviceName = "RFQSvcGroup";
        public string tablename = "PurchRFQCaseTable";
        public string actionItem = "AllRequestforQuotations_ListPage";

        public SysOperationResult_BOL createrfq(RFQContract _headerContract)
        {
            GeneralContract contract = new GeneralContract();
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new RFQServiceClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract result = ((RFQService)channel).createRFQFromPRAsync(new createRFQFromPR(callContext, _headerContract)).Result.result;
                    objBOL = SysOperationResults.operationResult<GeneralContract>(result);

                    SysLogUserActivity.logUserActivity(tablename, actionItem, ActionType.Create, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objBOL.Message = ex.InnerException.Message;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

            return objBOL;
        }
    }
}
