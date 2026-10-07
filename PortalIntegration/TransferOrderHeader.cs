using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.TransferOrderHeaderSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PortalIntegration
{
    public class TransferOrderHeader
    {
        private readonly string serviceName = "TransferOrderHeaderSvcGroup";
        public string tableName = "InventTransferTable";
        public string actionItem = "TransferOrder_ListPage";


        public SysOperationResult_BOL receiveTransferOrderMultiple(TransferOrderHeaderContract _headerContract)
        {
            GeneralContract contract = new GeneralContract();
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract result = ((TransferOrderHeaderSvc)channel).receiveTransferOrderMultipleAsync(new receiveTransferOrderMultiple(callContext, _headerContract)).Result.result;
                    objBOL = SysOperationResults.operationResult<GeneralContract>(result);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objBOL.Message = ex.InnerException.Message;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

            return objBOL;
        }

        public SysOperationResult_BOL shipTransferOrder(TransferOrderHeaderContract _headerContract)
        {
            GeneralContract contract = new GeneralContract();
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;
                    
                    GeneralContract result = ((TransferOrderHeaderSvc)channel).shipTransferOrderMultipleAsync(new shipTransferOrderMultiple(callContext, _headerContract)).Result.result;
                    objBOL = SysOperationResults.operationResult<GeneralContract>(result);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objBOL.Message = ex.InnerException.Message;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

            return objBOL;
        }

        public DataTable findTranferOrder(string _transferId)
        {
            GeneralContract contract = new GeneralContract();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((TransferOrderHeaderSvc)channel).FindByTransferIDAsync(new FindByTransferID(callContext, _transferId)).Result.result;
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
            dataTable = RetrieveDatatable.createDataTable(new TransferOrderHeaderContract[] { });
            dataTable.TableName = tableName;
            return dataTable;
        }

        //public TransferOrderHeaderContract prepareHeaderContract(string _transferId, DataTable _dtLines, string _transDate)
        //{
        //    TransferOrderHeaderContract contract = new TransferOrderHeaderContract();
        //    TransferOrderLines lines = new TransferOrderLines();
        //    TransferOrderHeader order = new TransferOrderHeader();
        //    List<TransferOrderlinesContract> linesList = new List<TransferOrderlinesContract>();
        //    DataTable dt = order.findTranferOrder(_transferId);

        //    foreach (DataRow dr in dt.Rows)
        //    {

        //        contract.FromWarehouse = dr["FromWarehouse"].ToString();
        //        contract.ToWarehouse = dr["FromWarehouse"].ToString();
        //        contract.TransferId = _transferId;
        //        contract.ShipDate = Convert.ToDateTime(_transDate);
        //        contract.ReceiveDate = Convert.ToDateTime(_transDate);
        //        contract.UpdatedByWorker = SessionVariables.getCurrentEmployeeId();
        //        long recId;
        //        if (long.TryParse(dr["RecId"]?.ToString(), out recId))
        //        {
        //            contract.RecId = recId;
        //        }
        //        contract.InventTransferStatus = dr["InventTransferStatus"].ToString();

        //        DataTable dtLines = _dtLines;


        //        foreach (DataRow drLines in dtLines.Rows)
        //        {
        //            TransferOrderlinesContract line = new TransferOrderlinesContract();
        //            line.TransferId = drLines["TransferId"]?.ToString();
        //            line.Itemid = drLines["Itemnumber"]?.ToString();
        //            line.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //            line.ItemName = drLines["ItemName"]?.ToString();

        //            if (DateTime.TryParse(drLines["LinesShipDate"]?.ToString(), out DateTime shipDate))
        //                line.LinesShipDate = shipDate;

        //            if (DateTime.TryParse(drLines["LinesReceiveDate"]?.ToString(), out DateTime receiveDate))
        //                line.LinesReceiveDate = receiveDate;

        //            line.ReserveItem = drLines["ReserveItem"] is DBNull ? NoYes.No :
        //                                                    (NoYes)Enum.Parse(typeof(NoYes), drLines["ReserveItem"].ToString());

        //            //line.Qtyshipped = Convert.ToDecimal(drLines["Qtyshipped"]);
        //            line.Qtyshipped = decimal.TryParse(drLines["Qtyshipped"]?.ToString(), out decimal qtyShipped) ? qtyShipped : 0;
        //            long Dimensionshipfrom;
        //            if (long.TryParse(drLines["Dimensionshipfrom"]?.ToString(), out Dimensionshipfrom))
        //            {
        //                line.Dimensionshipfrom = Dimensionshipfrom;
        //            }
        //            long Dimensionshipto;
        //            if (long.TryParse(drLines["Dimensionshipto"]?.ToString(), out Dimensionshipto))
        //            {
        //                line.Dimensionshipto = Dimensionshipto;
        //            }
        //            line.TransctionCode = drLines["TransctionCode"]?.ToString();
        //            line.IsCatchWeight = bool.TryParse(drLines["IsCatchWeight"]?.ToString(), out bool isCW) ? isCW : false;
        //            line.InventDimId = drLines["InventDimId"]?.ToString();
        //            line.LocationIdFrom = drLines["LocationIdFrom"]?.ToString();
        //            line.LocationIdTo = drLines["LocationIdTo"]?.ToString();
        //            if (drLines.Table.Columns.Contains("Shipquantity") && drLines["Shipquantity"] != DBNull.Value)
        //            {
        //                line.QtyTransfer = Convert.ToDecimal(drLines["Shipquantity"]); // Get from Form
        //            }
        //            else if (drLines.Table.Columns.Contains("Receivequantity") && drLines["Receivequantity"] != DBNull.Value)
        //            {
        //                line.QtyTransfer = Convert.ToDecimal(drLines["Receivequantity"]); // Get from Form
        //            }
        //            //line.CWQtyTransfer = Convert.ToDecimal(drLines["CWQtyTransfer"]);
        //            line.CWQtyTransfer = decimal.TryParse(drLines["CWQtyTransfer"]?.ToString(), out decimal cwQty) ? cwQty : 0;
        //            line.UnitId = drLines["UnitId"]?.ToString();

        //            //line.TransferStatus = drLines["TransferStatus"]?.ToString();
        //            line.InventTransferStatus = dr["InventTransferStatus"].ToString();
        //            if (drLines.Table.Columns.Contains("Scrapquantity") && drLines["Scrapquantity"] != DBNull.Value)
        //            {
        //                line.QtyScrap = Convert.ToDecimal(drLines["Scrapquantity"].ToString());
        //            }

        //            line.InventBatchId = drLines["InventBatchId"]?.ToString();

        //            long Linerecid;
        //            if (long.TryParse(drLines["RecId"]?.ToString(), out Linerecid))
        //                line.RecId = Linerecid;

        //            line.WMSLocationId = drLines["WMSLocationId"]?.ToString();
        //            line.WMSPalletId = drLines["WMSPalletId"]?.ToString();
        //            line.InventSerialId = drLines["InventSerialId"]?.ToString();
        //            line.InventLocationId = drLines["InventLocationId"]?.ToString();
        //            line.ConfigId = drLines["ConfigId"]?.ToString();
        //            line.InventSizeId = drLines["InventSizeId"]?.ToString();
        //            line.InventColorId = drLines["InventColorId"]?.ToString();
        //            line.InventSiteId = drLines["InventSiteId"]?.ToString();
        //            line.InventStyle = drLines["InventStyle"]?.ToString();
        //            line.NewTransitLocationName = drLines["NewTransitLocationName"]?.ToString();

        //            if (decimal.TryParse(drLines["LineNum"]?.ToString(), out decimal lineNum))
        //                line.LineNum = lineNum;

        //            if (DateTime.TryParse(drLines["CreatedDateTime"]?.ToString(), out DateTime createdDate))
        //                line.CreatedDateTime = createdDate;

        //            linesList.Add(line);
        //        }

        //        contract.TransferLines = linesList.ToArray();
        //    }
        //    return contract;
        //}


        public TransferOrderHeaderContract prepareHeaderContract(string _transferId, DataTable _dtLines, string _transDate)
        {
            TransferOrderHeaderContract contract = new TransferOrderHeaderContract();
            TransferOrderLines lines = new TransferOrderLines();
            TransferOrderHeader order = new TransferOrderHeader();
            List<TransferOrderlinesContract> linesList = new List<TransferOrderlinesContract>();
            DataTable dt = order.findTranferOrder(_transferId);

            foreach (DataRow dr in dt.Rows)
            {
                contract.FromWarehouse = dr["FromWarehouse"].ToString();
                contract.ToWarehouse = dr["ToWarehouse"].ToString();
                contract.TransferId = _transferId;

                DateTime transDate;
                if (DateTime.TryParse(_transDate, out transDate))
                {
                    contract.ShipDate = transDate;
                    contract.ReceiveDate = transDate;
                }
                else
                {
                    throw new FormatException($"Posting date '{_transDate}' is not a valid date.");
                }

                contract.UpdatedByWorker = SessionVariables.getCurrentEmployeeId();
                long recId;
                if (long.TryParse(dr["RecId"]?.ToString(), out recId))
                {
                    contract.RecId = recId;
                }
                contract.InventTransferStatus = dr["InventTransferStatus"].ToString();

                DataTable dtLines = _dtLines;

                foreach (DataRow drLines in dtLines.Rows)
                {
                    TransferOrderlinesContract line = new TransferOrderlinesContract();
                    line.TransferId = drLines["TransferId"]?.ToString();
                    line.Itemid = drLines["Itemnumber"]?.ToString();
                    line.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    line.ItemName = drLines["ItemName"]?.ToString();

                    if (DateTime.TryParse(drLines["LinesShipDate"]?.ToString(), out DateTime shipDate))
                        line.LinesShipDate = shipDate;

                    if (DateTime.TryParse(drLines["LinesReceiveDate"]?.ToString(), out DateTime receiveDate))
                        line.LinesReceiveDate = receiveDate;

                    line.ReserveItem = drLines["ReserveItem"] is DBNull || string.IsNullOrWhiteSpace(drLines["ReserveItem"]?.ToString())
      ? NoYes.No
      : (Enum.TryParse<NoYes>(drLines["ReserveItem"].ToString(), out NoYes reserveVal)
          ? reserveVal
          : NoYes.No);

                    line.Qtyshipped = decimal.TryParse(drLines["Qtyshipped"]?.ToString(), out decimal qtyShipped) ? qtyShipped : 0;

                    long Dimensionshipfrom;
                    if (long.TryParse(drLines["Dimensionshipfrom"]?.ToString(), out Dimensionshipfrom))
                    {
                        line.Dimensionshipfrom = Dimensionshipfrom;
                    }
                    long Dimensionshipto;
                    if (long.TryParse(drLines["Dimensionshipto"]?.ToString(), out Dimensionshipto))
                    {
                        line.Dimensionshipto = Dimensionshipto;
                    }
                    line.TransctionCode = drLines["TransctionCode"]?.ToString();
                    line.IsCatchWeight = bool.TryParse(drLines["IsCatchWeight"]?.ToString(), out bool isCW) ? isCW : false;
                    line.InventDimId = drLines["InventDimId"]?.ToString();
                    line.LocationIdFrom = drLines["LocationIdFrom"]?.ToString();
                    line.LocationIdTo = drLines["LocationIdTo"]?.ToString();

                    if (drLines.Table.Columns.Contains("Shipquantity") && drLines["Shipquantity"] != DBNull.Value &&
                        decimal.TryParse(drLines["Shipquantity"]?.ToString(), out decimal shipQty))
                    {
                        line.QtyTransfer = shipQty;
                    }
                    else if (drLines.Table.Columns.Contains("Receivequantity") && drLines["Receivequantity"] != DBNull.Value &&
                        decimal.TryParse(drLines["Receivequantity"]?.ToString(), out decimal recvQty))
                    {
                        line.QtyTransfer = recvQty;
                    }
                    else
                    {
                        line.QtyTransfer = 0;
                    }

                    line.CWQtyTransfer = decimal.TryParse(drLines["CWQtyTransfer"]?.ToString(), out decimal cwQty) ? cwQty : 0;
                    line.UnitId = drLines["UnitId"]?.ToString();

                    line.InventTransferStatus = dr["InventTransferStatus"].ToString();

                    if (drLines.Table.Columns.Contains("Scrapquantity") && drLines["Scrapquantity"] != DBNull.Value &&
                        decimal.TryParse(drLines["Scrapquantity"]?.ToString(), out decimal scrapQty))
                    {
                        line.QtyScrap = scrapQty;
                    }
                    else
                    {
                        line.QtyScrap = 0;
                    }

                    line.InventBatchId = drLines["InventBatchId"]?.ToString();

                    long Linerecid;
                    if (long.TryParse(drLines["RecId"]?.ToString(), out Linerecid))
                        line.RecId = Linerecid;

                    line.WMSLocationId = drLines["WMSLocationId"]?.ToString();
                    line.WMSPalletId = drLines["WMSPalletId"]?.ToString();
                    line.InventSerialId = drLines["InventSerialId"]?.ToString();
                    line.InventLocationId = drLines["InventLocationId"]?.ToString();
                    line.ConfigId = drLines["ConfigId"]?.ToString();
                    line.InventSizeId = drLines["InventSizeId"]?.ToString();
                    line.InventColorId = drLines["InventColorId"]?.ToString();
                    line.InventSiteId = drLines["InventSiteId"]?.ToString();
                    line.InventStyle = drLines["InventStyle"]?.ToString();
                    line.NewTransitLocationName = drLines["NewTransitLocationName"]?.ToString();

                    if (decimal.TryParse(drLines["LineNum"]?.ToString(), out decimal lineNum))
                        line.LineNum = lineNum;

                    if (DateTime.TryParse(drLines["CreatedDateTime"]?.ToString(), out DateTime createdDate))
                        line.CreatedDateTime = createdDate;

                    linesList.Add(line);
                }

                contract.TransferLines = linesList.ToArray();
            }
            return contract;
        }

        //  public TransferOrderHeaderContract prepareHeaderContract(string _transferId, DataTable _dtLines, string _transDate)
        //  {
        //      TransferOrderHeaderContract contract = new TransferOrderHeaderContract();
        //      TransferOrderLines lines = new TransferOrderLines();
        //      TransferOrderHeader order = new TransferOrderHeader();
        //      List<TransferOrderlinesContract> linesList = new List<TransferOrderlinesContract>();
        //      DataTable dt = order.findTranferOrder(_transferId);

        //      foreach (DataRow dr in dt.Rows)
        //      {
        //          contract.FromWarehouse = dr["FromWarehouse"].ToString();
        //          contract.ToWarehouse = dr["FromWarehouse"].ToString();
        //          contract.TransferId = _transferId;

        //          DateTime transDate;
        //          if (DateTime.TryParse(_transDate, out transDate))
        //          {
        //              contract.ShipDate = transDate;
        //              contract.ReceiveDate = transDate;
        //          }
        //          else
        //          {
        //              throw new FormatException($"Posting date '{_transDate}' is not a valid date.");
        //          }

        //          contract.UpdatedByWorker = SessionVariables.getCurrentEmployeeId();
        //          long recId;
        //          if (long.TryParse(dr["RecId"]?.ToString(), out recId))
        //          {
        //              contract.RecId = recId;
        //          }
        //          contract.InventTransferStatus = dr["InventTransferStatus"].ToString();

        //          DataTable dtLines = _dtLines;

        //          foreach (DataRow drLines in dtLines.Rows)
        //          {
        //              TransferOrderlinesContract line = new TransferOrderlinesContract();
        //              line.TransferId = drLines["TransferId"]?.ToString();
        //              line.Itemid = drLines["Itemnumber"]?.ToString();
        //              line.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
        //              line.ItemName = drLines["ItemName"]?.ToString();

        //              if (DateTime.TryParse(drLines["LinesShipDate"]?.ToString(), out DateTime shipDate))
        //                  line.LinesShipDate = shipDate;

        //              if (DateTime.TryParse(drLines["LinesReceiveDate"]?.ToString(), out DateTime receiveDate))
        //                  line.LinesReceiveDate = receiveDate;

        //              line.ReserveItem = drLines["ReserveItem"] is DBNull || string.IsNullOrWhiteSpace(drLines["ReserveItem"]?.ToString())
        //? NoYes.No
        //: (Enum.TryParse<NoYes>(drLines["ReserveItem"].ToString(), out NoYes reserveVal)
        //    ? reserveVal
        //    : NoYes.No);
        //              line.Qtyshipped = decimal.TryParse(drLines["Qtyshipped"]?.ToString(), out decimal qtyShipped) ? qtyShipped : 0;

        //              long Dimensionshipfrom;
        //              if (long.TryParse(drLines["Dimensionshipfrom"]?.ToString(), out Dimensionshipfrom))
        //              {
        //                  line.Dimensionshipfrom = Dimensionshipfrom;
        //              }
        //              long Dimensionshipto;
        //              if (long.TryParse(drLines["Dimensionshipto"]?.ToString(), out Dimensionshipto))
        //              {
        //                  line.Dimensionshipto = Dimensionshipto;
        //              }
        //              line.TransctionCode = drLines["TransctionCode"]?.ToString();
        //              line.IsCatchWeight = bool.TryParse(drLines["IsCatchWeight"]?.ToString(), out bool isCW) ? isCW : false;
        //              line.InventDimId = drLines["InventDimId"]?.ToString();
        //              line.LocationIdFrom = drLines["LocationIdFrom"]?.ToString();
        //              line.LocationIdTo = drLines["LocationIdTo"]?.ToString();

        //              if (drLines.Table.Columns.Contains("Shipquantity") && drLines["Shipquantity"] != DBNull.Value &&
        //                  decimal.TryParse(drLines["Shipquantity"]?.ToString(), out decimal shipQty))
        //              {
        //                  line.QtyTransfer = shipQty;
        //              }
        //              else if (drLines.Table.Columns.Contains("Receivequantity") && drLines["Receivequantity"] != DBNull.Value &&
        //                  decimal.TryParse(drLines["Receivequantity"]?.ToString(), out decimal recvQty))
        //              {
        //                  line.QtyTransfer = recvQty;
        //              }
        //              else
        //              {
        //                  line.QtyTransfer = 0;
        //              }

        //              line.CWQtyTransfer = decimal.TryParse(drLines["CWQtyTransfer"]?.ToString(), out decimal cwQty) ? cwQty : 0;
        //              line.UnitId = drLines["UnitId"]?.ToString();

        //              line.InventTransferStatus = dr["InventTransferStatus"].ToString();

        //              if (drLines.Table.Columns.Contains("Scrapquantity") && drLines["Scrapquantity"] != DBNull.Value &&
        //                  decimal.TryParse(drLines["Scrapquantity"]?.ToString(), out decimal scrapQty))
        //              {
        //                  line.QtyScrap = scrapQty;
        //              }
        //              else
        //              {
        //                  line.QtyScrap = 0;
        //              }

        //              line.InventBatchId = drLines["InventBatchId"]?.ToString();

        //              long Linerecid;
        //              if (long.TryParse(drLines["RecId"]?.ToString(), out Linerecid))
        //                  line.RecId = Linerecid;

        //              line.WMSLocationId = drLines["WMSLocationId"]?.ToString();
        //              line.WMSPalletId = drLines["WMSPalletId"]?.ToString();
        //              line.InventSerialId = drLines["InventSerialId"]?.ToString();
        //              line.InventLocationId = drLines["InventLocationId"]?.ToString();
        //              line.ConfigId = drLines["ConfigId"]?.ToString();
        //              line.InventSizeId = drLines["InventSizeId"]?.ToString();
        //              line.InventColorId = drLines["InventColorId"]?.ToString();
        //              line.InventSiteId = drLines["InventSiteId"]?.ToString();
        //              line.InventStyle = drLines["InventStyle"]?.ToString();
        //              line.NewTransitLocationName = drLines["NewTransitLocationName"]?.ToString();

        //              if (decimal.TryParse(drLines["LineNum"]?.ToString(), out decimal lineNum))
        //                  line.LineNum = lineNum;

        //              if (DateTime.TryParse(drLines["CreatedDateTime"]?.ToString(), out DateTime createdDate))
        //                  line.CreatedDateTime = createdDate;

        //              linesList.Add(line);
        //          }

        //          contract.TransferLines = linesList.ToArray();
        //      }
        //      return contract;
        //  }

        public SysOperationResult_BOL recieveTransferOrder(string _transferID)
        {

            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string transferId = _transferID;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract result = ((TransferOrderHeaderSvc)channel).recieveTransferOrderAsync(new recieveTransferOrder(callContext, transferId)).Result.result;
                    objBOL = SysOperationResults.operationResult<GeneralContract>(result);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
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

        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            bool isAnySuccess = false;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();
                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
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
                        //long TransferId = 0;
                        //Int64.TryParse(dataRow["TransferId"].ToString(), out TransferId);
                        string fromWarehouse = dataRow["FromWarehouse"].ToString();
                        string toWarehouse = dataRow["ToWarehouse"].ToString();
                        string ShippingDate = dataRow["ShippingDate"].ToString();
                        string ReceiptDate = dataRow["ReceiptDate"].ToString();

                        TransferOrderHeaderContract contract = new TransferOrderHeaderContract();

                        //transfercontract.TransferId = TransferId.ToString();
                        contract.FromWarehouse = fromWarehouse.ToString();
                        contract.ToWarehouse = toWarehouse.ToString();
                        contract.ShipDate = DateTime.Parse(ShippingDate); // assuming valid format
                        contract.ReceiveDate = DateTime.Parse(ReceiptDate);
                        contract.DataAreaId = dataAreaId;
                        contract.UpdatedByWorker = employeeId;

                        GeneralContract results = ((TransferOrderHeaderSvc)channel).createAsync(new create(callContext, contract)).Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
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

       

        public SysOperationResult_BOL update(DataTable _objDT, long _recId)
        {
            long recId = _recId;
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
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
                        TransferOrderHeaderContract transferOrderContract = new TransferOrderHeaderContract();

                        transferOrderContract.TransferId = dataRow["TransferID"].ToString();
                        //transferOrderContract.FromWarehouse = dataRow["FromWarehouse"].ToString();
                        //transferOrderContract.ToWarehouse = dataRow["ToWarehouse"].ToString();

                        transferOrderContract.ShipDate = dataRow["ShipDate"].ToString().toDateTime();
                        transferOrderContract.ReceiveDate = dataRow["ReceiveDate"].ToString().toDateTime();

                        transferOrderContract.RecId = recId;
                        

                        GeneralContract results = ((TransferOrderHeaderSvc)channel).updateAsync(new update(callContext, transferOrderContract)).Result.result;
                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
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
            { }
            return objBOL;
        }

        public DataTable retrieveAll(bool _allTransferOrder = false)
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = "";
                if (!_allTransferOrder)
                    employeeId = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
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
                    var results = ((TransferOrderHeaderSvc)channel).retrieveAllAsync(new retrieveAll(callContext, employeeId)).Result.result;

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

        public DataTable retrievefromwarehouse()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
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

                    var results = ((TransferOrderHeaderSvc)channel).retrieveFromwarehousesAsync(new retrieveFromwarehouses(callContext)).Result.result;

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

        public DataTable retrievetowarehouse()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
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

                    var results = ((TransferOrderHeaderSvc)channel).retrievetowarehousesAsync(new retrievetowarehouses(callContext)).Result.result;

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
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new TransferOrderHeaderSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((TransferOrderHeaderSvc)channel).deleteAsync(new delete(callContext, employeeId, _recordsRecId)).Result.result;
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

    }

}
