using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HRSendEmailSvcReference;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HRSendEmail
    {
        private readonly string serviceName = "HRSendEmailSvcGroup";
        public string tableName = "HRSendEmail";

        public SysOperationResult_BOL sendEmail(string[] _emailToList, string _subject, string _body, string[] _emailCCList, bool _isEmail)
        {

            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string body = _body;
                string[] emailCCList = _emailCCList;
                string[] emailToList = _emailToList;
                bool isEmail = _isEmail;
                string subject = _subject;


                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new HRSendEmailSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((HRSendEmailSvc)channel).sendEmailAsync(new sendEmail(callContext, body, emailCCList, emailToList, isEmail, subject)).Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                    

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

    }
}
