using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PurchaseOrderLines_MaintainCharges;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
   public class PurchaseOrderHeader_MaintainCharges
    {
        private readonly string serviceName = "PurchaseOrderLines_MaintainChargesGroupSvc";
        public string tableName = "MarkupTrans";
        public string actionItem = "PurchaseOrderLines_MaintainCharges";


        //public SysOperationResult_BOL createHeader(DataTable _objDT, string purchaseOrderId, long recId)
        //{
        //    DataTable dataTable = _objDT;
        //    SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

        //    try
        //    {
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        // Set up authentication and service connection
        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new PurchaseOrderLines_MaintainChargesSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            // Loop through datatable rows
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                // Safely parse decimals
        //                decimal value = 0;
        //                decimal calculatedAmount = 0;
        //                decimal mcrBrokerContractFee = 0;

        //                decimal.TryParse(row["Value"]?.ToString(), out value);
        //                decimal.TryParse(row["CalculatedAmount"]?.ToString(), out calculatedAmount);
        //                decimal.TryParse(row["MCRBrokerContractFee"]?.ToString(), out mcrBrokerContractFee);

        //                // Create contract object
        //                PurchaseOrderLines_MaintainChargesContract contract = new PurchaseOrderLines_MaintainChargesContract();

        //                // ✅ Match X++ parm methods: parmPurchId() and parmRecId()
        //                contract.PurchaseOrderID = purchaseOrderId;
        //                contract.RecId = recId;
        //                contract.MarkupCode = row["MarkupCode"]?.ToString();
        //                contract.Txt = row["Txt"]?.ToString();
        //                contract.MarkupCategory = row["MarkupCategory"]?.ToString();
        //                contract.SpecificUnitSymbol = "day";
        //                contract.Value = value;
        //                contract.AllowEdit = row["AllowEdit"]?.ToString();
        //                contract.CurrencyCode = row["CurrencyCode"]?.ToString();
        //                contract.CalculatedAmount = calculatedAmount;
        //                contract.TaxGroup = row["TaxGroup"]?.ToString();
        //                contract.TaxItemGroup = row["TaxItemGroup"]?.ToString();

        //                // ✅ Send to service
        //                GeneralContract results = ((PurchaseOrderLines_MaintainChargesSvc)channel)
        //                    .createHeader(new createHeader(callContext, contract))
        //                    .Result.result;

        //                objBOL = SysOperationResults.operationResult<GeneralContract>(results);

        //                SysLogUserActivity.logUserActivity("PurchaseOrderMaintainCharges", "Create", ActionType.Create, objBOL);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }

        //    return objBOL;
        //}

        public SysOperationResult_BOL create(DataTable _objDT, string purchaseOrderId, long recId)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                // Set up authentication and service connection
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PurchaseOrderLines_MaintainChargesSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Loop through datatable rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Safely parse decimals
                        decimal value = 0;
                        decimal calculatedAmount = 0;
                        decimal mcrBrokerContractFee = 0;

                        decimal.TryParse(row["Value"]?.ToString(), out value);
                        decimal.TryParse(row["CalculatedAmount"]?.ToString(), out calculatedAmount);
                        decimal.TryParse(row["MCRBrokerContractFee"]?.ToString(), out mcrBrokerContractFee);

                        // Create contract object
                        PurchaseOrderLines_MaintainChargesContract contract = new PurchaseOrderLines_MaintainChargesContract();

                        // ✅ Match X++ parm methods: parmPurchId() and parmRecId()
                        contract.PurchaseOrderID = purchaseOrderId;
                        contract.RecId = recId;
                        contract.MarkupCode = row["MarkupCode"]?.ToString();
                        contract.Txt = row["Txt"]?.ToString();
                        contract.MarkupCategory = row["MarkupCategory"]?.ToString();
                        contract.SpecificUnitSymbol = "day";
                        contract.Value = value;
                        contract.AllowEdit = row["AllowEdit"]?.ToString();
                        contract.CurrencyCode = row["CurrencyCode"]?.ToString();
                        contract.CalculatedAmount = calculatedAmount;
                        contract.TaxGroup = row["TaxGroup"]?.ToString();
                        contract.TaxItemGroup = row["TaxItemGroup"]?.ToString();

                        // ✅ Send to service
                        GeneralContract results = ((PurchaseOrderLines_MaintainChargesSvc)channel)
                            .createHeaderAsync(new createHeader(callContext, contract))
                            .Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity("PurchaseOrderMaintainCharges", "Create", ActionType.Create, objBOL);
                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return objBOL;
        }


        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new PurchaseOrderLines_MaintainChargesContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }
    }
}
