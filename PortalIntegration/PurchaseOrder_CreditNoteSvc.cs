using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PO_CreditNoteSvcReference;
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
    public class PurchaseOrder_CreditNoteSvc
    {
        private readonly string serviceName = "PO_CreditNoteSvcServiceGroup";
        public string tableName =   "VendInvoiceJour";
        public string actionItem = "PurchaseOrder_CreateNote";



        public DataTable retrieveByVendor(string _vendAccNumber = "")
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();



                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PO_CreditNoteSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the  parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)
                    string _vendAccNum = _vendAccNumber; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((PO_CreditNoteSvc)channel).retrieveByVendorAsync(new retrieveByVendor(callContext, _vendAccNumber)).Result.result;

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


        public DataTable retrieveByPurchId(string _PurchId = "")
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();



                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PO_CreditNoteSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the  parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)
                  // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((PO_CreditNoteSvc)channel).retrievePurchIdLinesAsync(new retrievePurchIdLines(callContext, _PurchId)).Result.result;

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

        //public string createCreditNote(string invoiceId, string orderAccount, string purchId)
        //{
        //    try
        //    {
        //        DataTable dataTable = _objDT;
        //        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //        var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //        var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //        var endpointAddress = new EndpointAddress(serviceUriString);
        //        var binding = SoapHelper.GetBinding();
        //        var client = new PO_CreditNoteSvcClient(binding, endpointAddress);
        //        var channel = client.InnerChannel;

        //        using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //        {
        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            CallContext callContext = new CallContext();
        //            callContext.MessageId = Guid.NewGuid().ToString();
        //            callContext.Company = dataAreaId;

        //            PO_CreditNoteContract contract = new PO_CreditNoteContract();
        //            contract.InvoiceId = dataTable.Rows[0]["PurchID"].ToString();
        //            contract.OrderAccount = dataTable.Rows[0]["PurchID"].ToString(); 

        //            contract.PurchId =  dataTable.Rows[0]["PurchID"].ToString(); (purchId);

        //            var result = ((PO_CreditNoteSvc)channel).createCreditNoteAsync(
        //                new createCreditNote(callContext, contract)).Result.result;

        //            return result; // newInvoiceId (VendInvoiceId) returned from X++
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        throw;
        //    }
        //}

        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PO_CreditNoteSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string purchid = dataRow["PurchID"].ToString();
                        string vendoraccount = dataRow["VendorAccount"].ToString();
                        string invoiceId = dataRow["InvoiceId"].ToString();

                        PO_CreditNoteContract contract = new PO_CreditNoteContract();
                        contract.PurchId = purchid;
                        contract.OrderAccount = vendoraccount;
                        contract.InvoiceId = invoiceId;

                        // createCreditNote returns VendInvoiceId (a plain string), not GeneralContract
                        GeneralContract response = ((PO_CreditNoteSvc)channel)
     .createCreditNoteAsync(
         new createCreditNote(callContext, contract))
     .Result.result;

                        if (response != null && response.IsSuccess)
                        {
                            objBOL.isSuccess = true;
                            objBOL.AlertType = AlertType.Success.ToString();
                            objBOL.Message = response.Message;
                        }
                        else
                        {
                            objBOL.isSuccess = false;
                            objBOL.AlertType = AlertType.Error.ToString();
                            objBOL.Message = response != null
                                ? response.Message
                                : "Credit note creation failed.";
                        }
                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
                    }
                }
            }
            catch (Exception ex)
            {
                objBOL.isSuccess = false;
                objBOL.AlertType = AlertType.Error.ToString();
                objBOL.Message = ex.Message;

                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            {
            }

            return objBOL;
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new PO_CreditNoteContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

    }
}
