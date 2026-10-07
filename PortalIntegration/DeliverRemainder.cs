using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
    public class DeliverRemainder
    {
        private readonly string serviceName = "TransferOrderLinesSvcGroup";
        public string tablename = "InventTransferLine";
        public string actionItem = "TransferOrderLines_ListPage";


        public SysOperationResult_BOL updateDeliverRemainder(TransferOrderUpdateRemainContract _contract)
        {
            SysOperationResult_BOL sysOperationResult_BOL = new SysOperationResult_BOL();

            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderLinesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((TransferOrderLinesSvc)channel).updateRemainAsync(new updateRemain(callContext,_contract,employeeId)).Result.result;
                    sysOperationResult_BOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tablename, actionItem, ActionType.Create, sysOperationResult_BOL);
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

            return sysOperationResult_BOL;
        }


        public SysOperationResult_BOL CancelDelivery(long[] _recordsRecId)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                // Get current company (data area)
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                // Prepare authentication and service setup
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderLinesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    // Attach OAuth header for authentication
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    // Create call context
                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Collect all results here
                    List<GeneralContract> resultList = new List<GeneralContract>();

                    // Loop through each record RecId
                    foreach (long recId in _recordsRecId)
                    {
                        // Call Dynamics 365 method for each record
                        var result = ((TransferOrderLinesSvc)channel)
                            .cancelRemainAsync(new cancelRemain(callContext,employeeId, recId))
                            .Result.result;

                        // Add to results list
                        resultList.Add(result);
                    }

                    // Convert list to array and build SysOperationResult_BOL
                    objBOL = SysOperationResults.operationResults(resultList.ToArray());

                    // Log user activity
                    SysLogUserActivity.logUserActivity(tablename, actionItem, ActionType.Delete, resultList.ToArray());
                }
            }
            catch (Exception ex)
            {
                // Log any errors
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return objBOL;
        }


    }
}
