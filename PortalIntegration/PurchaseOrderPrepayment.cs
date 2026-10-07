using BussinessObject;
using GeneralAuxiliary;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PurchaseOrderPrepayment
    {
        public static SysOperationResult_BOL createPrepayment(string purchId, string description, decimal value, string category)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString("PurchPrePaymentSvcGroup", ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PurchPrePaymentSvcReference.PurchPrePaymentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    PurchPrePaymentSvcReference.CallContext callContext = new PurchPrePaymentSvcReference.CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    PurchPrePaymentSvcReference.PurchPrepaymentCreateContract contract = new PurchPrePaymentSvcReference.PurchPrepaymentCreateContract();
                    contract.purchId = purchId;
                    contract.dataAreaId = dataAreaId;
                    contract.description = description;
                    contract.prepaymentValue = value;
                    contract.categoryName = category;

                    var response = ((PurchPrePaymentSvcReference.PurchPrePaymentSvc)channel)
                        .createPrepaymentAsync(new PurchPrePaymentSvcReference.createPrepayment(callContext, contract)).Result.result;

                    objBOL.isSuccess = response.success;
                    objBOL.Message = response.message;
                    objBOL.AlertType = response.success ? AlertType.Success.ToString() : AlertType.Error.ToString();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                objBOL.isSuccess = false;
                objBOL.Message = $"Error creating prepayment for {purchId}: {ex.Message}";
                objBOL.AlertType = AlertType.Error.ToString();
            }
            return objBOL;
        }

        public static SysOperationResult_BOL removePrepayment(string purchId, long prepaymentRecId = 0, bool forceRemove = true)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString("PurchPrePaymentSvcGroup", ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PurchPrePaymentSvcReference.PurchPrePaymentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    PurchPrePaymentSvcReference.CallContext callContext = new PurchPrePaymentSvcReference.CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    PurchPrePaymentSvcReference.PurchPrepaymentRemoveContract contract = new PurchPrePaymentSvcReference.PurchPrepaymentRemoveContract();
                    contract.purchId = purchId;
                    contract.dataAreaId = dataAreaId;
                    contract.prepaymentRecId = prepaymentRecId;
                    contract.forceRemove = forceRemove;

                    var response = ((PurchPrePaymentSvcReference.PurchPrePaymentSvc)channel)
                        .removePrepaymentAsync(new PurchPrePaymentSvcReference.removePrepayment(callContext, contract)).Result.result;

                    objBOL.isSuccess = response.success;
                    objBOL.Message = response.message;
                    objBOL.AlertType = response.success ? AlertType.Success.ToString() : AlertType.Error.ToString();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                objBOL.isSuccess = false;
                objBOL.Message = $"Error removing prepayment for {purchId}: {ex.Message}";
                objBOL.AlertType = AlertType.Error.ToString();
            }
            return objBOL;
        }
    }
}
