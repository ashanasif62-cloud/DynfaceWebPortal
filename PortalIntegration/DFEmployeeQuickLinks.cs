using GeneralAuxiliary;
using PortalIntegration.DFEmployeeQuickLinksSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class DFEmployeeQuickLinks
    {
        private readonly string serviceName = "DFEmployeeQuickLinksSvcGroup";
        public string tablename = "DFEmployeeQuickLinks";
        public DataTable retrieveEmployeeQuickLinks()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DFEmployeeQuickLinksSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((DFEmployeeQuickLinksSvc)channel).retrieveEmployeeQuickLinksAsync(new retrieveEmployeeQuickLinks(callContext, employeeid)).Result.result;

                    DataTable results = RetrieveDatatable.createDataTable(result);

                    return results;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return createEmployeeQuickLinksTable();
            }
            finally
            { }
        }

        public GeneralContract createEmployeeQuickLinks(string _menuitemId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DFEmployeeQuickLinksSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    DFEmployeeQuickLinksSvcContract contract = new DFEmployeeQuickLinksSvcContract();
                    contract.EmployeeId = employeeid;
                    contract.MenuItemId = _menuitemId;

                    var result = ((DFEmployeeQuickLinksSvc)channel).createEmployeeQuickLinksAsync(new createEmployeeQuickLinks(callContext, contract)).Result.result;

                    return result;
                }
            }
            catch (Exception ex)
            {
                GeneralContract generalContract = new GeneralContract();
                generalContract.IsSuccess = false;
                generalContract.Message = ex.Message;
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return generalContract;
            }
            finally
            { }
        }

        public GeneralContract delete(string _menuitemId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DFEmployeeQuickLinksSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    DFEmployeeQuickLinksSvcContract contract = new DFEmployeeQuickLinksSvcContract();

                    var result = ((DFEmployeeQuickLinksSvc)channel).deleteAsync(new delete(callContext, employeeid, _menuitemId)).Result.result;

                    return result;
                }
            }
            catch (Exception ex)
            {
                GeneralContract generalContract = new GeneralContract();
                generalContract.IsSuccess = false;
                generalContract.Message = ex.Message;
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return generalContract;
            }
            finally
            { }
        }

        public DataTable createEmployeeQuickLinksTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new DFEmployeeQuickLinks[] { });
            dataTable.TableName = tablename;
            return dataTable;
        }
    }
}
