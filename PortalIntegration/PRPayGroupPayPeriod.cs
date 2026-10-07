using GeneralAuxiliary;
using PortalIntegration.PRPayGroupPayPeriodSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PRPayGroupPayPeriod
    {
        private readonly string serviceName = "PRPayGroupPayPeriodSvcGroup";
        public string tableName = "PRPayGroupPayPeriod";

        public DataTable retrieveAll()
        {
            try
            {
                string results = string.Empty;
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PRPayGroupPayPeriodSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = SessionVariables.getUserCurrentDataAreaId()
                    };

                    var result = ((PRPayGroupPayPeriodSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result;

                    PRPayGroupPayPeriodSvcContract[] list = result.result;
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

        public DataTable findByEmployee(string _employeeId)
        {
            try
            {
                string employeeId = _employeeId;

                string results = string.Empty;
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PRPayGroupPayPeriodSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = SessionVariables.getUserCurrentDataAreaId()
                    };

                    var result = ((PRPayGroupPayPeriodSvc)channel).findByEmployeeAsync(new findByEmployee(callContext, employeeId)).Result;

                    PRPayGroupPayPeriodSvcContract[] list = result.result;
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

        public DataTable find(long _recId)
        {
            try
            {
                long recId = _recId;

                string results = string.Empty;
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PRPayGroupPayPeriodSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = SessionVariables.getUserCurrentDataAreaId()
                    };

                    var result = ((PRPayGroupPayPeriodSvc)channel).findAsync(new find(callContext, recId)).Result;

                    PRPayGroupPayPeriodSvcContract[] list = result.result;
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
            dataTable = RetrieveDatatable.createDataTable(new PRPayGroupPayPeriodSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
            //PRPayGroupPayPeriodSvcContract pRPayGroupPayPeriodSvcContract = new PRPayGroupPayPeriodSvcContract();
            //pRPayGroupPayPeriodSvcContract.PayPeriodCode;
        }



    }
}
