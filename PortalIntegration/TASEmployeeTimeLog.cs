using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TASEmployeeTimeLogSvcReference;
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
    public class TASEmployeeTimeLog
    {
        private readonly string serviceName = "TASEmployeeTimeLogSVCGroup";
        public string tableName = "TASEmployeeTimeLog";

        public DataTable getTimeLogHistoryByDate(DateTime _fromDate, DateTime _toDate)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeid = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TASEmployeeTimeLogSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TASEmployeeTimeLogSVC)channel).getTimeLogHistoryAsync(new getTimeLogHistory(callContext, employeeid, _fromDate, _toDate)).Result.result;

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
            }
            finally
            { }
            return createDataTable();
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new TASEmployeeTimeLogContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }

}
