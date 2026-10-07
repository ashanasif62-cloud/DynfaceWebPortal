using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PortalIntegration.ESSHRRecruitmentSvcReference;

namespace PortalIntegration
{
    public class ESSHRRecruitment
    {
        private readonly string serviceName = "HRMRecruitingProjectGroup";
        public string tablename = "HRMRecruitingTable";
        public string actionItem = "RecruitmentProjects";



        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRMRecruitingProjectSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    //callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((HRMRecruitingProjectSVC)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

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

     

        public bool updateByRecruitingId(HRMRecruitingProjectContract contract)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRMRecruitingProjectSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    // callContext.Company = dataAreaId; // enable if required

                    var response = ((HRMRecruitingProjectSVC)channel)
                        .updatebyrecruitingIdAsync(
                            new updatebyrecruitingId(callContext, contract)
                        ).Result;

                    return response.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
        }


        public bool deleteByRecruitingId(HRMRecruitingProjectContract contract)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new HRMRecruitingProjectSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    // callContext.Company = dataAreaId; // enable if required

                    var response = ((HRMRecruitingProjectSVC)channel)
                        .deletebyRecruitingIdAsync(
                            new deletebyRecruitingId(callContext, contract)
                        ).Result;

                    return response.result;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
        }



        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new HRMRecruitingProjectContract[] { });
            dataTable.TableName = tablename;
            return dataTable;
        }
    }
}
