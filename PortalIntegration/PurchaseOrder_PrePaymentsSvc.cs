using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.PurchaseOrderPrePaymentSvcReference;
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
    public class PurchaseOrder_PrePaymentsSvc
    {
        private readonly string serviceName = "PurchPrePaymentServiceGroup";
        public string tableName = "PurchPrepayTable";
        public string actionItem = "PurchaseOrder_Prepayment";



        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            bool isAnySuccess = false;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new PurchPrepaymentSVCClient(binding, endpointAddress);
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
                        string purchId = dataRow["PurchID"].ToString();
                        string categoryName = dataRow["CategoryName"].ToString();     // maps to EcoResCategory.Name lookup
                        string description = dataRow["Description"].ToString();

                        decimal prepaymentValue = 0m;
                        decimal.TryParse(dataRow["PrepaymentValue"]?.ToString(), out prepaymentValue);

                        // Optional fields — only present if your DataTable actually has these columns
                        //string taxGroupId = dataTable.Columns.Contains("TaxGroupId") ? dataRow["TaxGroupId"].ToString() : null;
                        //string itemTaxGroupId = dataTable.Columns.Contains("ItemTaxGroupId") ? dataRow["ItemTaxGroupId"].ToString() : null;

                        PurchPrepaymentCreateContract contract = new PurchPrepaymentCreateContract();

                        contract.purchId = purchId;
                        contract.categoryName = categoryName;
                        contract.prepaymentValue = prepaymentValue;
                        contract.dataAreaId = dataAreaId;
                        // contract.description = description;

                        GeneralContract results = ((PurchPrepaymentSVC)channel).createPrepaymentAsync(new createPrepayment(callContext, contract)).Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);



                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);


                        //PurchPrepaymentResponse results = ((PurchPrepaymentSVC)channel)
                        //    .createPrepaymentAsync(new createPrepayment(callContext, contract))
                        //    .Result.result;

                        //objBOL = SysOperationResults.operationResult<PurchPrepaymentResponse>(results);
                       


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
            finally
            {
            }
            return objBOL;
        }



        public SysOperationResult_BOL remove(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            bool isAnySuccess = false;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PurchPrepaymentSVCClient(binding, endpointAddress);
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
                        string purchId = dataRow["PurchID"].ToString();

                        //long prepaymentRecId = 0;
                        //long.TryParse(dataRow["PrepaymentRecId"]?.ToString(), out prepaymentRecId);

                        //bool forceRemove = false;
                        //if (dataTable.Columns.Contains("ForceRemove"))
                        //{
                        //    bool.TryParse(dataRow["ForceRemove"]?.ToString(), out forceRemove);
                        //}

                        PurchPrepaymentRemoveContract contract = new PurchPrepaymentRemoveContract();
                        contract.purchId = purchId;
                        //contract.prepaymentRecId = prepaymentRecId;
                        //contract.forceRemove = forceRemove;
                        contract.dataAreaId = dataAreaId;

                        GeneralContract results = ((PurchPrepaymentSVC)channel)
                            .removePrepaymentAsync(new removePrepayment(callContext, contract))
                            .Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);
                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, objBOL);
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
            finally
            {
            }
            return objBOL;
        }



        public DataTable retrieveCategoryId()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();



                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new PurchPrepaymentSVCClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)

                    var results = ((PurchPrepaymentSVC)channel).retrieveCategoryAsync(new retrieveCategory(callContext)).Result.result;

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
            dataTable = RetrieveDatatable.createDataTable(new PurchPrepaymentCreateContract[] { });
            dataTable.TableName = tableName;
            return dataTable;

        }
    }

}
