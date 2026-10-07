using GeneralAuxiliary;
using PortalIntegration.PREOSNoticePeriodsSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PREOSNoticePeriods
    {
        private readonly string serviceName = "PREOSNoticePeriodsSvcGroup";
        public string tableName = "PREOSNoticePeriods";

        public DataTable retrieveAllPREOSNoticePeriods()
        {
            try
            {
                string results = string.Empty;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PREOSNoticePeriodsSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((PREOSNoticePeriodsSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result;

                    PREOSNoticePeriodsSvcContract[] list = result.result;
                    DataTable dataTable = RetrieveDatatable.createDataTable(list);
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
            dataTable = RetrieveDatatable.createDataTable(new PREOSNoticePeriodsSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
