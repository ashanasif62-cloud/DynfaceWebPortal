using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PortalIntegration.PurchaseOrderLines_MaintainCharges;
using BussinessObject;
namespace PortalIntegration
{
    public class PurchaseOrderLine_MaintainCharges
    {
        private readonly string serviceName = "PurchaseOrderLines_MaintainChargesGroupSvc";
        public string tableName = "MarkupTrans";
        public string actionItem = "PurchaseOrderLines_MaintainCharges";


        public DataTable retrieveAll(long _recId)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

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


                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)
                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveAllAsync(new retrieveAll(callContext, _recId)).Result.result;

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
                            .createAsync(new create(callContext, contract))
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





        public DataTable retrieveMarkupCode()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();




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

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)


                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveMarkupCodeAsync(new retrieveMarkupCode(callContext, _dataAreaId)).Result.result;

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

        public DataTable retrieveMarkupCodeDescription(string _markupCode)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();




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

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)


                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveMarkupCodeDescriptionAsync(new retrieveMarkupCodeDescription(callContext, _dataAreaId,_markupCode)).Result.result;

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


        public DataTable retrieveUnit()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();




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

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)


                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveUnitAsync(new retrieveUnit(callContext)).Result.result;

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

        public DataTable retrieveSalesTaxGroup()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();




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

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)


                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveSalesTaxGroupAsync(new retrieveSalesTaxGroup (callContext,_dataAreaId)).Result.result;

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

        public DataTable retrieveItemSalesTaxGroup()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();




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

                    // Pass the _dataAreaId parameter to the RetrieveAll method
                    string _dataAreaId = dataAreaId; // Assign your dataAreaId to _dataAreaId (ensure it is initialized)


                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel).retrieveItemSalesTaxGroupAsync(new retrieveItemSalesTaxGroup(callContext, _dataAreaId)).Result.result;

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

        public SysOperationResult_BOL delete(long[] _recordsRecId)
        {
            long[] recordsRecId = _recordsRecId;
            string employeeId = SessionVariables.getCurrentEmployeeId();
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

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

                    GeneralContract[] results = ((PurchaseOrderLines_MaintainChargesSvc)channel).deleteAsync(new delete(callContext,employeeId, _recordsRecId)).Result.result;
                    objBOL = SysOperationResults.operationResults(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, results);
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
            { }
            return objBOL;
        }

        public DataTable retrieveCalculatedAmount(long recId, long recIdLine)
        {
            try
            {
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
                 

                    var results = ((PurchaseOrderLines_MaintainChargesSvc)channel)
                        .retrieveCalculatedAmountAsync(new retrieveCalculatedAmount(callContext, recId, recIdLine))
                        .Result.result;

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
        }


        public SysOperationResult_BOL UpdateGeneral(DataTable _objDT)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                DataTable dataTable = _objDT;
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

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

                    CallContext callContext = new CallContext
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Company = dataAreaId
                    };

                    // Create contract
                    PurchaseOrderLines_MaintainChargesContract Contract = new PurchaseOrderLines_MaintainChargesContract();

                    // TODO: assign values to headerContract from dataTable here
                    // headerContract.parmXYZ = dataTable.Rows[0]["XYZ"].ToString();

                   Contract.Value = Convert.ToDecimal(dataTable.Rows[0]["Value"]);
                    Contract.MarkupCategory = dataTable.Rows[0]["MarkupCategory"].ToString();
                    Contract.CurrencyCode = dataTable.Rows[0]["CurrencyCode"].ToString();
                  

                   
                    Contract.RecId = Convert.ToInt64(dataTable.Rows[0]["RecId"]);




                    // Call service
                    GeneralContract results = ((PurchaseOrderLines_MaintainChargesSvc)channel)
                        .updateAsync(new update(callContext, Contract))
                        .Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

            return objBOL;  // ✅ always returns a value
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
