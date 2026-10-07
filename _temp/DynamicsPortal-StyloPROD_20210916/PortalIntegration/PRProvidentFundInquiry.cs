using GeneralAuxiliary;
using PortalIntegration.PRProvidentFundInquirySvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PRProvidentFundInquiry
    {
        private readonly string serviceName = "PRProvidentFundInquirySvcGroup";

        public DataTable retrieveAllPRProvidentFundInquiry(bool _checkTeam)
        {
            //PRProvidentFundInquirySvcContract aa = new PRProvidentFundInquirySvcContract();
            //aa.BenDedCode;
            //aa.EmployeeAmount;
            //aa.EmployeeId;
            //aa.EmployeeName;
            //aa.EmployerAmount;
            //aa.PayPeriodCode;
            bool checkTeam = _checkTeam;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PRProvidentFundInquirySvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((PRProvidentFundInquirySvc)channel).retriveAsync(new retrive(callContext, checkTeam, employeeId)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new PRProvidentFundInquirySvcContract[] { });
            return dataTable;
        }

    }
}
