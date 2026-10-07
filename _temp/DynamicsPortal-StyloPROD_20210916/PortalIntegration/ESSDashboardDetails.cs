using GeneralAuxiliary;
using PortalIntegration.ESSDashboardSvcReference;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class ESSDashboardDetails
    {
        private readonly string serviceName = "ESSDashboardSvcGroup";

        public string[] retrieveAdvancesDetails(string _employeeId)
        {
            string employeeId = _employeeId;
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();
            var client = new ESSDashboardSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                CallContext callContext = new CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();
                callContext.Company = dataAreaId;

                ESSDashboardSvcContract[] resultsSet = ((ESSDashboardSvc)channel).employeeAdvancesAsync(new employeeAdvances(callContext, employeeId)).Result.result;

                List<string> dataSource = new List<string>();
                string dataLabels = string.Empty;
                string dataValues = string.Empty;
                foreach (ESSDashboardSvcContract dataContract in resultsSet)
                {
                    dataLabels += "'" + dataContract.dataLables + "'" + ",";
                    dataValues += "'" + dataContract.dataSet1 + "'" + ",";
                }

                dataSource.Add(dataLabels);
                dataSource.Add(dataValues);
                return dataSource.ToArray();
            }
        }

        public string[] retrieveLeavesDetails(string _employeeId)
        {
            string employeeId = _employeeId;
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();
            var client = new ESSDashboardSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                CallContext callContext = new CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();
                callContext.Company = dataAreaId;

                ESSDashboardSvcContract[] resultsSet = ((ESSDashboardSvc)channel).employeeLeavesAsync(new employeeLeaves(callContext, employeeId)).Result.result;

                List<string> dataSource = new List<string>();
                string dataLabels = string.Empty;
                string dataSet01 = string.Empty;
                string dataSet02 = string.Empty;
                foreach (ESSDashboardSvcContract dataContract in resultsSet)
                {
                    dataLabels += "'" + dataContract.dataLables + "'" + ",";
                    dataSet01 += "'" + dataContract.dataSet1 + "'" + ",";
                    dataSet02 += "'" + dataContract.dataSet2 + "'" + ",";
                }

                dataSource.Add(dataLabels);
                dataSource.Add(dataSet01);
                dataSource.Add(dataSet02);
                return dataSource.ToArray();
            }
        }



    }
}
