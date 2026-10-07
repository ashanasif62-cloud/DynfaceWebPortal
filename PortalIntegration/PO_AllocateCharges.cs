using GeneralAuxiliary;
using PortalIntegration.PurchaseOrderAllocateChargesSvcReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
namespace PortalIntegration
{
    public class PO_AllocateCharges
    {
        private readonly string serviceName = "POAllocateChargesServiceGruop";
        public string tableName = "MarkupAllocation";
        public string actionItem = "PurchaseOrder_AllocateCharges";


       

        public GeneralContract AllocateCharges(PO_AllocateChargesSvcContract contract)
        {
            GeneralContract result = new GeneralContract();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(
                    serviceName,
                    ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new POAllocateChargesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope scope = new OperationContextScope(channel))
                {
                    // 🔐 OAuth
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name] = requestMessage;

                    // 🧠 CallContext
                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // 📡 SERVICE CALL
                    var response = ((POAllocateChargesSvc)channel)
                        .AllocateChargesAsync(
                            new AllocateCharges(callContext, contract))
                        .Result;

                    result = response.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(this.GetType().FullName, ex);

                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }



    }
}
