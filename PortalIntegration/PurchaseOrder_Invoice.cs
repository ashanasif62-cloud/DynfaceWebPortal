using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PortalIntegration.PurchaseOrderInvoiceSvcReference;

namespace PortalIntegration
{
    public class PurchaseOrder_Invoice
    {
        private readonly string serviceName = "PurchaseOrder_InvoiceSvcServiceGroup";
        public string tableName = "VendInvoiceInfoTable";
        public string actionItem = "VendorInvoice";

            public DataTable retrieveAll(string _purchId)
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
                    var client = new PurchaseOrder_InvoiceSvcClient(binding, endpointAddress);
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
                        var results = ((PurchaseOrder_InvoiceSvc)channel).retrieveAllAsync(new retrieveAll(callContext, _purchId)).Result.result;

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

        public DataTable retrieveLines(string _purchId)
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
                var client = new PurchaseOrder_InvoiceSvcClient(binding, endpointAddress);
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
                    var results = ((PurchaseOrder_InvoiceSvc)channel).retrievelinesAsync(new retrievelines(callContext, _purchId)).Result.result;

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

        public GeneralContract post_InvoicePO(string _num, string _purchId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PurchaseOrder_InvoiceSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PurchaseOrder_InvoiceSvc)channel).post_InvoicePOAsync(new post_InvoicePO(callContext, _num, _purchId)).Result.result;

                    return results;
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
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new PurchaseOrder_InvoiceSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
