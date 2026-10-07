using GeneralAuxiliary;
using PortalIntegration.PRLeaveSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PRLeaves
    {
        private readonly string serviceName = "PRLeaveSvcGroup";
        public string tableName = "PRLeave";

        public DataTable retrieveAllPRLeaveCodes()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PRLeaveSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = (((PRLeaveSvc)channel).retrieveAllAsync(new retrieveAll(callContext, employeeId)).Result).result;
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
            dataTable = RetrieveDatatable.createDataTable(new PRLeaveSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
