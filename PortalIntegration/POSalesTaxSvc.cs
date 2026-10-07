using System;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using GeneralAuxiliary;
using PortalIntegration.POSalesTaxSvcReference;

namespace PortalIntegration
{
    public class POSalesTaxSvc
    {
        private readonly string serviceName = "POSalesTaxSvcGroup";

        public DataTable createDataTable()
        {
            return RetrieveDatatable.createDataTable(new POSalesTaxSvcContract[] { });
        }

        public Infolog retrieveByPurchLineRecId(CallContext CallContext, POSalesTaxSvcContract _contract, out POSalesTaxSvcContract[] result)
        {
            result = null;
            try
            {
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new POSalesTaxSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    Infolog infolog = client.retrieveByPurchLineRecId(CallContext, _contract, out result);
                    return infolog;
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

        // Convenience helper that builds CallContext and contract, calls service async and returns a DataTable
        public DataTable retrieveByPurchLineRecId_DT(long purchLineRecId)
        {
            try
            {
                CallContext callContext = new CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();
                callContext.Company = SessionVariables.getUserCurrentDataAreaId();

                POSalesTaxSvcContract contract = new POSalesTaxSvcContract();
                contract.purchLineRecId = purchLineRecId;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new POSalesTaxSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    var resp = client.retrieveByPurchLineRecIdAsync(callContext, contract).Result;
                    POSalesTaxSvcContract[] results = resp.result;

                    DataTable dt = results != null ? RetrieveDatatable.createDataTable(results) : createDataTable();
                    return dt;
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
        }
    }
}
