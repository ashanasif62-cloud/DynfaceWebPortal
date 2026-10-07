using GeneralAuxiliary;
using PortalIntegration.APAppraisalsRatingSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class APAppraisalsRating
    {
        private readonly string serviceName = "APAppraisalsRatingSvcGroup";
        public string tableName = "APAppraisalsRating";

        public DataTable retriveByAppraisalCode(string _appraisalCode)
        {
            try
            {
                string appraisalCode = _appraisalCode;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new APAppraisalsRatingSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((APAppraisalsRatingSvc)channel).findByAppraisalCodeAsync(new findByAppraisalCode(callContext, appraisalCode)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new APAppraisalsRatingSvcContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

        //public DataTable createDataTable()
        //{
        //    //APAppraisalsRatingSvcContract aPAppraisalsRatingSvcContract = new APAppraisalsRatingSvcContract();
        //    //aPAppraisalsRatingSvcContract.FromRange;
        //    //aPAppraisalsRatingSvcContract.ToRange;
        //    //aPAppraisalsRatingSvcContract.Rating;

        //    //aPAppraisalsRatingSvcContract.AppraisalCode;
        //    //aPAppraisalsRatingSvcContract.Description;
        //    //aPAppraisalsRatingSvcContract.Grade;
        //    //aPAppraisalsRatingSvcContract.RatingScale;
        //    //aPAppraisalsRatingSvcContract.RecId;

        //    DataTable dataTable = new DataTable(tableName);
        //    dataTable.Columns.Add("FromRange");
        //    dataTable.Columns.Add("ToRange");
        //    dataTable.Columns.Add("Rating");
        //    dataTable.Columns.Add("AppraisalCode");
        //    dataTable.Columns.Add("Description");
        //    dataTable.Columns.Add("Grade");
        //    dataTable.Columns.Add("RatingScale");
        //    dataTable.Columns.Add("RecId");
        //    return dataTable;
        //}

    }
}
