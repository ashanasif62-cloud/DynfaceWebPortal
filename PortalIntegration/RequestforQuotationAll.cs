using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.RequestforQuotationAllServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PortalIntegration
{
    public class RequestforQuotationAll
    {
        private readonly string serviceName = "RequestforQuotationAllSvcGroup";
        public string tableName = "PurchRFQCaseTable";
        public string actionItem = "RequestforQuotationAll_ListPage";


        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                //// Parse the date strings into DateTime objects
                //DateTime parsedFromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //DateTime parsedToDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new RequestforQuotationAllServiceClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)
                    var results = ((RequestforQuotationAllService)channel).retrieveAllAsync(new retrieveAll(callContext, _dataAreaId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new RequestforQuotationAllContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
