using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using PortalIntegration.PREmployeeAdvancesSvcReference;

namespace PortalIntegration
{
    public class PRAdvancesRequest
    {
        private readonly string serviceName = "PREmployeeAdvancesSvcGroup";
        public string tableName = "PREmployeeAdvance";

        public DataTable getEmployeeAdvances()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PREmployeeAdvancesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var response = ((PREmployeeAdvancesSvc)channel).getEmployeeAdvancesAsync
                                 (new getEmployeeAdvances(callContext, employeeId, "", "")).Result.result;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("RecoveryDate", typeof(DateTime));
                    dt.Columns.Add("RecoveryAmount", typeof(decimal));
                    dt.Columns.Add("PayGroupCode", typeof(string));
                    dt.Columns.Add("AdvanceTypeCode", typeof(string));


                    if (response != null && response.Length > 0)
                    {
                        foreach (var advance in response)
                        {
                            if (advance.Recoveries != null)
                            {
                                foreach (var recovery in advance.Recoveries)
                                {
                                    // Dynamics WCF object[] often contains child contract objects
                                    // based on the response, each item in Recoveries should ideally be cast to the contract type
                                    // if it's available, otherwise dynamic/reflection is used.
                                    // Given the object[] type, we can use dynamic or direct property access if it's a known contract.
                                    dynamic rec = recovery;
                                    DataRow dr = dt.NewRow();
                                    dr["RecoveryDate"] = rec.RecoveryDate;
                                    dr["RecoveryAmount"] = rec.RecoveryAmount;
                                    dr["PayGroupCode"] = (advance.PayGroupCode ?? string.Empty);
                                    dr["AdvanceTypeCode"] = (advance.AdvanceTypeCode ?? string.Empty);

                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return new DataTable();
            }
            finally
            { }
        }
    }
}
