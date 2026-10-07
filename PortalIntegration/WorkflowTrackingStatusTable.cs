using GeneralAuxiliary;
using PortalIntegration.WorkflowTrackingStatusTableSvcGroup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
    public class WorkflowTrackingStatusTable
    {


        private readonly string serviceName = "WorkflowTrackingStatusTableSvcGroup";


        public DataTable retrieveRecentActivities(string _email)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new WorkflowTrackingStatusTableSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var result = ((WorkflowTrackingStatusTableSvc)channel).retrieveMySubmittedRequestsByEmailAsync(new retrieveMySubmittedRequestsByEmail(callContext, _email)).Result.result;

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
                return createEmployeeRecentActivitiesTable();
            }
            finally
            { }
        }

        public DataTable createEmployeeRecentActivitiesTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new WorkflowTrackingStatusTableContract[] { });
            return dataTable;
        }
    }
}
