using PortalIntegration.HcmWorkerDetailsSvcReference;
using PortalIntegration.PREarningsSvcReference;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class PREarnings
    {
        private string serviceName = string.Empty;

        //public string getUserSession()
        //{
        //    //var response = OAuthHelper.AuthorizationHeader();
        //    UserSessionServiceName = "UserSessionService";
        //    var authenticationHeader = OAuthHelper.GetAuthenticationHeader();
        //    var serviceUriString = SoapHelper.GetSoapServiceUriString(UserSessionServiceName, ClientConfiguration.Default.UriString);

        //    //var request = HttpWebRequest.Create(ClientConfiguration.Default.UriString + "api/services/UserSessionService/AifUserSessionService/GetUserSessionInfo");
        //    //request.Headers[OAuthHelper.OAuthHeader] = OAuthHelper.GetAuthenticationHeader();
        //    //request.Method = "POST";
        //    //request.GetResponse();

        //    var endpointAddress = new EndpointAddress(serviceUriString);
        //    var binding = SoapHelper.GetBinding();

        //    var client = new UserSessionServiceClient(binding, endpointAddress);

        //    var channel = client.InnerChannel;
        //    UserSessionInfo sessionInfo = null;
        //    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //    {
        //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
        //        sessionInfo = ((UserSessionService)channel).GetUserSessionInfo(new GetUserSessionInfo()).result;
        //    }

        //    //sessionInfo;
        //    string results = sessionInfo.UserId;
        //    return results;
        //}

        private void businesstrip()
        {

            //ESSBusinessTripSvcReference.Date date = new Date();
            //date._value = DateTime.Today;

            //ESSBusinessTripSvcReference.utcdatetime utcdatetime = new utcdatetime();
            //utcdatetime._value = DateTime.Today;
        }

        public string pREarningsCreate(DataTable _objDT, string _commandMode)
        {
            string results = string.Empty;
            DataTable objDT = _objDT;
            string commandMode = _commandMode;
            
            DataTable dataTable = _objDT;
            int i = 0;


            serviceName = "PREarningsSVCGroup";

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new PREarningsSvcReference.PREarningsSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;



            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


                PREarningsSvcReference.CallContext callContext = new PREarningsSvcReference.CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();

                
                callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();

                int collectionLength = dataTable.Rows.Count;
                PREarningsSvcReference.PREarningsSvcContract[] pREarningsSvcContracts = new PREarningsSvcContract[collectionLength];

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PREarningsSvcReference.PREarningsSvcContract pREarningsSvcContract = new PREarningsSvcReference.PREarningsSvcContract();
                    pREarningsSvcContract.EarningCode = dataRow["EarningCode"].ToString();
                    pREarningsSvcContract.EarningDescription = dataRow["EarningDescription"].ToString();
                    pREarningsSvcContract.EarningType = PREarningsSvcReference.PREarningType.Regular;
                    pREarningsSvcContract.UnitType = dataRow["UnitType"] is DBNull ? PREarningsSvcReference.PRUnitType.Amount :
                                                     (PREarningsSvcReference.PRUnitType)Enum.Parse(typeof(PREarningsSvcReference.PRUnitType), dataRow["UnitType"].ToString());

                    pREarningsSvcContract.PrintSequence = dataRow["PrintSequence"] is DBNull ? 0 : Convert.ToInt32(dataRow["PrintSequence"]);

                    pREarningsSvcContract.PrintInReports = dataRow["PrintInReports"] is DBNull ? PREarningsSvcReference.PRPrintInReports.No :
                                                     (PREarningsSvcReference.PRPrintInReports)Enum.Parse(typeof(PREarningsSvcReference.PRPrintInReports), dataRow["PrintInReports"].ToString());

                    //pREarningsSvcContract.TaxableEarning = dataRow["TaxableEarning"] is DBNull ? PREarningsSvcReference.NoYes.No :
                    //                                 (PREarningsSvcReference.NoYes)Enum.Parse(typeof(PREarningsSvcReference.NoYes), dataRow["TaxableEarning"].ToString());
                    //pREarningsSvcContract.CalculationCode = dataRow[""].ToString();
                    pREarningsSvcContracts[i] = pREarningsSvcContract;
                    i++;
                }


                //client.Create(callContext, pREarningsSvcContract, out results);
                //PREarningsSvcReference.CreatePREarnings createPREarnings = new CreatePREarnings(callContext, list);
                //results = ((PREarningsSvcReference.PREarningsSvc)channel).Create(new Create(callContext, pREarningsSvcContract)).result;

                CreatePREarningsResponse result = ((PREarningsSvcReference.PREarningsSvc)channel).CreatePREarningsAsync(new CreatePREarnings(callContext, pREarningsSvcContracts)).Result;
                results = result.result;
                //results = GeneralLibrary.GetResults.createOperationResults(1, results);

            }

            return results;
        }

        public DataTable ViewData()
        {
            string results = string.Empty;
            //

            serviceName = "PREarningsSVCGroup";

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new PREarningsSvcReference.PREarningsSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;


            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


                PREarningsSvcReference.CallContext callContext = new PREarningsSvcReference.CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();

                
                callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();


                RetrieveResponse result = ((PREarningsSvcReference.PREarningsSvc)channel).RetrieveAsync(new Retrieve(callContext)).Result;

                PREarningsSvcContract[] listPREarnings = result.result;

                DataTable dataTable = createDataTable2(listPREarnings);

                return dataTable;
            }


        }

        public MemoryStream ViewReport()
        {
            string results = string.Empty;
            //

            serviceName = "PREarningsSVCGroup";

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new PREarningsSvcReference.PREarningsSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;


            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


                PREarningsSvcReference.CallContext callContext = new PREarningsSvcReference.CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();

                
                callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();


                ViewSalarySlipResponse result = ((PREarningsSvcReference.PREarningsSvc)channel).ViewSalarySlipAsync(new ViewSalarySlip(callContext)).Result;

                MemoryStream memoryStream = result.result;

                return memoryStream;
            }


        }

        public MemoryStream RetrieveReport(string _currentDataAreaId, string _employeeId, string _payPeriodCode)
        {
            string results = string.Empty;
            string currentDataAreaId = _currentDataAreaId;
            string employeeId = _employeeId;
            string payPeriodCode = _payPeriodCode;

            serviceName = "RetrieveReportsSvcReference";

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new RetrieveReportsSvcReference.RetrieveReportSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;


            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


                RetrieveReportsSvcReference.CallContext callContext = new RetrieveReportsSvcReference.CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();

                
                callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();

                var result = ((RetrieveReportsSvcReference.RetrieveReportSvc)channel).RetrieveSalarySlipReportAsync(
                            new RetrieveSalarySlipReport(callContext, employeeId, payPeriodCode)).Result;

                MemoryStream memoryStream = result.result;

                return memoryStream;
            }


        }

        public static DataTable createDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable createDataTable2(Object[] arr)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(arr.GetType());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            serializer.Serialize(sw, arr);

            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            System.IO.StringReader reader = new System.IO.StringReader(sw.ToString());

            ds.ReadXml(reader);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return new DataTable();
        }

        public DataSet retrieveRecords()
        {
            return null;
            //string results = string.Empty;
            ////

            //serviceName = "ExecuteQuerySvcGroup";

            //var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            //var endpointAddress = new EndpointAddress(serviceUriString);
            //var binding = SoapHelper.GetBinding();

            //var client = new ExecuteQuerySvcGroup.ExecuteQuerySvcClient(binding, endpointAddress);
            //var channel = client.InnerChannel;


            //using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //{
            //    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


            //    ExecuteQuerySvcGroup.CallContext callContext = new ExecuteQuerySvcGroup.CallContext();
            //    callContext.MessageId = Guid.NewGuid().ToString();

            //    
            //    callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();


            //    ExecuteQuerySvcGroup.ExecuteQuerySvcContract querySvcContract = new ExecuteQuerySvcContract();
            //    querySvcContract.TableName = "PREmployeeLeaveRequest";
            //    querySvcContract.FieldNames = new string[1] { "LeaveReqId" };//, "modifiedDateTime", "createdDateTime" };
            //    querySvcContract.FieldValues = new string[1] { "(LeaveReqId == \"USMF-000001\") || (LeaveReqId == \"USMF-000004\")" };//, "06/28/2018 10:11:02", ">06/28/2018 08:11:55" };
            //    //  "(LeaveReqId == \"USMF-000001\") || (LeaveReqId == \"USMF-000004\")"

            //    ////client.Create(callContext, pREarningsSvcContract, out results);
            //    //System.Data.DataSet dataset = client.ExecuteDynamicQuery(“MyInventTableQueryBuilder”, queryArgs, ref paging);
            //    DataSet result = ((ExecuteQuerySvcGroup.ExecuteQuerySvc)channel).RetrieveRecords(new RetrieveRecords(callContext, querySvcContract)).result;
            //    return null;

            //    //string xmlString = result;

            //    //DataSet dataSet = new DataSet(querySvcContract.TableName);
            //    //var ds = new DataSet();
            //    //using (var rdr = new StringReader(Properties.Resources.myschema))
            //    //{
            //    //    ds.ReadXmlSchema(rdr);
            //    //}
            //    //ds.ReadXml("mystuff.xml", XmlReadMode.IgnoreSchema);
            //    //dataSet.ReadXmlSchema(xmlString.);

            //    //object[] listPREarnings = result;
            //    //((object[])result[0])[2]

            //    //var aa = ((object[])result[0])[2];

            //    //var aaa = (System.Byte[])(((object[])((object[])result[0])[2])[0]);
            //    //System.IO.MemoryStream stream = new System.IO.MemoryStream(aaa);
            //    //object aa = objectSerialize.ByteArrayToObject(aaa);
            //    //DataTable dataTable = createDataTable2(listPREarnings);

            //    //return dataTable;
            //}


        }

        public long createRecord()
        {
            return 0;
            //string results = string.Empty;
            ////

            //serviceName = "ExecuteQuerySvcGroup";

            //var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            //var endpointAddress = new EndpointAddress(serviceUriString);
            //var binding = SoapHelper.GetBinding();

            //var client = new ExecuteQuerySvcGroup.ExecuteQuerySvcClient(binding, endpointAddress);
            //var channel = client.InnerChannel;

            //using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //{
            //    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


            //    ExecuteQuerySvcGroup.CallContext callContext = new ExecuteQuerySvcGroup.CallContext();
            //    callContext.MessageId = Guid.NewGuid().ToString();

            //    
            //    callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();

            //    ExecuteQuerySvcGroup.ExecuteQuerySvcContract querySvcContract = new ExecuteQuerySvcContract();
            //    querySvcContract.TableName = "PREmployeeLeaveRequest";                    //PREarnings
            //    querySvcContract.FieldNames = new string[]
            //    { "EmployeeId", "LeaveCode", "LeaveDays", "LeaveReqDate", "LeaveStartDate", "LeaveEndDate", "LeaveCategory", "Reason", "CreatedBy", "ModifiedBy" };
            //    //{ "EarningCode", "EarningDescription", "EarningType", "FixValue", "PrintInReports", "UnitType" };
            //    querySvcContract.FieldValues = new string[]
            //    { "68719486109", "Casual Leave", "2", "3/13/2018", "3/13/2018", "3/13/2018", "Full Day Leave", "Testing...01", "68719498103", "68719498103" };
            //    //{ "TestEarningCode", "TestEarningDesc", "Regular", "12.2", "No", "Hours" };

            //    //client.Create(callContext, pREarningsSvcContract, out results);

            //    long result = ((ExecuteQuerySvcGroup.ExecuteQuerySvc)channel).CreateRecord(new CreateRecord(callContext, querySvcContract)).result;

            //    return result;
            //}
        }

        public bool updateRecord()
        {
            return false;
            //string results = string.Empty;
            ////

            //serviceName = "ExecuteQuerySvcGroup";

            //var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            //var endpointAddress = new EndpointAddress(serviceUriString);
            //var binding = SoapHelper.GetBinding();

            //var client = new ExecuteQuerySvcGroup.ExecuteQuerySvcClient(binding, endpointAddress);
            //var channel = client.InnerChannel;


            //using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //{
            //    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


            //    ExecuteQuerySvcGroup.CallContext callContext = new ExecuteQuerySvcGroup.CallContext();
            //    callContext.MessageId = Guid.NewGuid().ToString();

            //    
            //    callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();


            //    ExecuteQuerySvcGroup.ExecuteQuerySvcContract querySvcContract = new ExecuteQuerySvcContract();
            //    querySvcContract.TableName = "PREmployeeLeaveRequest";
            //    querySvcContract.RecId = Convert.ToInt64("5637146827");
            //    querySvcContract.FieldNames = new string[] { "EmployeeName", "Reason", "LeaveCategory", "LeaveDays", "Balance", "LeaveEndDate" };             //
            //    querySvcContract.FieldValues = new string[] { "Fawad Azam", "Update Test With 5 Fields", "Half Day Leave", "3", "1", "3/15/2018" };       //

            //    //client.Create(callContext, pREarningsSvcContract, out results);

            //    Boolean result = ((ExecuteQuerySvcGroup.ExecuteQuerySvc)channel).UpdateRecord(new UpdateRecord(callContext, querySvcContract)).result;

            //    return result;

            //}


        }

        public bool deleteRecord()
        {
            return false;
            //string results = string.Empty;
            ////

            //serviceName = "ExecuteQuerySvcGroup";

            //var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            //var endpointAddress = new EndpointAddress(serviceUriString);
            //var binding = SoapHelper.GetBinding();

            //var client = new ExecuteQuerySvcGroup.ExecuteQuerySvcClient(binding, endpointAddress);
            //var channel = client.InnerChannel;


            //using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //{
            //    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


            //    ExecuteQuerySvcGroup.CallContext callContext = new ExecuteQuerySvcGroup.CallContext();
            //    callContext.MessageId = Guid.NewGuid().ToString();

            //    
            //    callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();


            //    ExecuteQuerySvcGroup.ExecuteQuerySvcContract querySvcContract = new ExecuteQuerySvcContract();
            //    querySvcContract.TableName = "PREmployeeLeaveRequest";
            //    querySvcContract.RecId = Convert.ToInt64("5637146827");

            //    //client.Create(callContext, pREarningsSvcContract, out results);
            //    Boolean result = ((ExecuteQuerySvcGroup.ExecuteQuerySvc)channel).DeleteRecord(new DeleteRecord(callContext, querySvcContract)).result;

            //    return result;

            //}


        }


        public DataTable retrieveWorkerDetails()
        {
            string results = string.Empty;
            //

            serviceName = "HcmWorkerDetailsSvcGroup";

            var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapHelper.GetBinding();

            var client = new HcmWorkerDetailsSvcReference.HcmWorkerDetailsSvcClient(binding, endpointAddress);
            var channel = client.InnerChannel;


            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;


                HcmWorkerDetailsSvcReference.CallContext callContext = new HcmWorkerDetailsSvcReference.CallContext();
                callContext.MessageId = Guid.NewGuid().ToString();

                
                callContext.Company = GeneralAuxiliary.SessionVariables.getUserCurrentDataAreaId();

                var result = ((HcmWorkerDetailsSvcReference.HcmWorkerDetailsSvc)channel).RetrieveWorkerDetails(new RetrieveWorkerDetails(callContext, "000017", DateTime.Today)).result;

                //DataTable dataTable = createDataTable2(result);

                return null;
            }


        }


    }

}
