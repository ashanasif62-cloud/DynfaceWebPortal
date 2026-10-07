using BussinessObject;
using DynamicsPortal;
using GeneralAuxiliary;
using OfficeOpenXml; // Requires EPPlus package (version 8 or later)
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PortalIntegration;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq; // Added for LINQ operations in header validation
using System.Web;
using System.Web.DynamicData;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace DynamicsPortal
{
    public partial class TransferOrderLines_ListPage : MainForm
    {
        private TransferOrderLines newTransferorderlines = new TransferOrderLines();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = newTransferorderlines.tablename;
                pageMenuId = "TransferOrderLines_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;
                if (!isPageAuthorizated)
                    return;
                if (!IsPostBack)
                {
                    Page.Title = "Transfer order lines";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    string transferId = Session["TransferID"] as string;
                    string status = Session["Status"] as string;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Transfer order - " + transferId + " - lines";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    lblTransferID.Text = transferId;
                    lblTransferOrderStatus.Text = status;

                    // Add "Transfer order:" before ID
                    lblTransferID.Text = "Transfer order: " + transferId;

                    // Apply styles (bold + 20px)
                    lblTransferID.Style["font-weight"] = "bold";
                    lblTransferID.Style["font-size"] = "20px";

                    lblTransferOrderStatus.Style["font-weight"] = "bold";
                    lblTransferOrderStatus.Style["font-size"] = "20px";



                    btnShip_All.Enabled = false;
                    btnShip_All.CssClass = "disabled-button";

                    if (!string.IsNullOrEmpty(transferId))
                    {
                        hfTransferID.Value = transferId;
                        //btnNew.OnClientClick = $"return openPopupPanel('/ESS/PR/TransferOrderLines_Create.aspx?TransferID={HttpUtility.JavaScriptStringEncode(transferId)}');";

                        DataTable dt = newTransferorderlines.retrieveAll(transferId);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (status == null || !(status.Trim().ToLower() == "shipped" ||
                                            status.Trim().ToLower() == "received" ||
                                            status.Trim().ToLower() == "received and shipped"))
                            {
                                btnShip_All.Enabled = true;
                                btnShip_All.CssClass = "";
                            }
                            foreach (DataRow row in dt.Rows)
                            {
                               
                                if (dt.Columns.Contains("Qtyshipped") && dt.Columns.Contains("QtyRecieved"))
                                {
                                    var shippedQty = row["Qtyshipped"];
                                    var recievedQty = row["QtyRecieved"];
                                    decimal shippedQtyValue = shippedQty != DBNull.Value ? Convert.ToDecimal(shippedQty) : 0;
                                    decimal recievedQtyValue = recievedQty != DBNull.Value ? Convert.ToDecimal(recievedQty) : 0;
                                    decimal remainingValue = shippedQtyValue - recievedQtyValue;
                                    if (remainingValue > 0)
                                    {
                                        //btnRecieve_All.Enabled = true;
                                        btnRecieve.Enabled = true;
                                        btnRecieve.CssClass = string.Empty;
                                    }
                                }
                            }
                            gridView.DataSource = dt;
                            gridView.DataBind();
                        }
                        else
                        {
                            btnShip_All.Enabled=false;
                            btnShip_All.CssClass="";
                            gridView.DataSource = null;
                            gridView.DataBind();
                        }
                    }

                    BindHeader();
                    BindFromwareHouse();
                    BindTowareHouse();
                    BindDelivery();
                    BindLinesHeader();
                    setFieldsEmpty();
                    BindTransitWarehouse();

                    if (Request["__EVENTTARGET"] == "RefreshGrid")
                    {
                        reBindGrid();
                    }

                    if (!string.IsNullOrEmpty(status))
                    {

                        switch (status.Trim().ToLower())
                        {
                            case "shipped":
                                btnDelete.Enabled = false;
                                btnDelete.CssClass = "disabled-buttonLines";

                                btnUploadExcel.Enabled = false;
                                btnUploadExcel.CssClass = "disabled-button";
                                btnShip_All.Enabled = false;
                                btnShip_All.CssClass = "disabled-button";
                                BtnAddLine.Enabled = false;
                                BtnAddLine.CssClass = "disabled-buttonLines";

                                //  btnRecieve.Enabled = true;
                                btnRecieve.Enabled = true;
                                btnRecieve.CssClass = ""; // Optional: remove disabled class if present

                                //btnRecieve_All.Enabled = true;
                                //btnRecieve_All.CssClass = ""; // Optional: remove disabled class if present
                                break;

                            case "received":
                            case "received and shipped":
                                btnDelete.Enabled = false;
                                btnDelete.CssClass = "disabled-buttonLines";

                                btnUploadExcel.Enabled = false;
                                btnUploadExcel.CssClass = "disabled-button";
                                btnShip_All.Enabled = false;
                                btnShip_All.CssClass = "disabled-button";
                                BtnAddLine.Enabled = false;
                                BtnAddLine.CssClass = "disabled-buttonLines";
                                //btnRecieve_All.Enabled = false;
                                //btnRecieve_All.CssClass = "disabled-button";
                                btnRecieve.Enabled = false;
                                btnRecieve.CssClass = "disabled-button";

                                break;
                            default:
                                btnDelete.Enabled = true;

                                //btnNew.CssClass = "";
                                btnUploadExcel.Enabled = true;
                                btnUploadExcel.CssClass = "";
                                btnShip_All.Enabled = true;
                                btnShip_All.CssClass = "";
                                BtnAddLine.Enabled = true;
                                BtnAddLine.CssClass = "";
                                //btnRecieve_All.Enabled = false;
                                //btnRecieve_All.CssClass = "disabled-buttonLines";
                                btnRecieve.Enabled = false;
                                btnRecieve.CssClass = "disabled-button";
                                TransferOrderLines checkShipment = new TransferOrderLines();
                                DataTable linesData = checkShipment.retrieveAll(transferId);
                                foreach (DataRow row in linesData.Rows)
                                {
                                    if (linesData.Columns.Contains("Qtyshipped") && linesData.Columns.Contains("QtyRecieved"))
                                    {
                                        var shippedQty = row["Qtyshipped"];
                                        var recievedQty = row["QtyRecieved"];
                                        decimal shippedQtyValue = shippedQty != DBNull.Value ? Convert.ToDecimal(shippedQty) : 0;
                                        decimal recievedQtyValue = recievedQty != DBNull.Value ? Convert.ToDecimal(recievedQty) : 0;
                                        decimal remainingValue = shippedQtyValue - recievedQtyValue;
                                        if (remainingValue > 0)
                                        {
                                            //btnRecieve_All.Enabled = true;
                                            btnRecieve.Enabled = true;
                                            btnRecieve.CssClass = "";
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    //if (gridView.Rows.Count > 0)
                    //{
                    //    GridViewRow firstRow = gridView.Rows[0];

                    //    // Tick the first row's checkbox
                    //    CheckBox chk = firstRow.FindControl("chk_SelectSingle") as CheckBox;
                    //    if (chk != null) chk.Checked = true;

                    //    chk_SelectSingle_CheckedChanged(chk, EventArgs.Empty);
                    //}
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
                DisplayErrorMessage("Error loading page: " + ex.Message);
            }
        }

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            // This method is triggered after the user selects a file via the FileUpload control
            // and the JavaScript triggers a postback for btnUploadExcel
            try
            {

                DataTable dt = new DataTable();
                if (!fileUploadExcel.HasFile)
                {
                    DisplayErrorMessage("Please select an Excel file to upload.");
                    return;
                }

                if (!fileUploadExcel.FileName.EndsWith(".xlsx") && !fileUploadExcel.FileName.EndsWith(".xls"))
                {
                    DisplayErrorMessage("Only Excel files (.xlsx or .xls) are allowed.");
                    return;
                }

                // Note: EPPlus license should be set globally in application startup (e.g., Global.asax or Startup.cs).
                // For EPPlus 8+, setting ExcelPackage.License is required.

                ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

                using (var stream = fileUploadExcel.PostedFile.InputStream)
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assume first worksheet
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        DisplayErrorMessage("The Excel file is empty or invalid.");
                        return;
                    }

                    // Define expected headers based on DataTable columns
                    var expectedHeaders = new List<string>
                    {
                        "Itemid",
                        "TransferId",
                        "LinesShipDate",
                        "LinesReceiveDate",
                        "ReserveItem",
                        "IsCatchWeight",
                        "QtyTransfer",
                        "QtyShipped",
                        "InventBatchId",
                        "WmsLocationId",
                        "WmsPalletId",
                        "InventSerialId",
                        "InventLocationId",
                        "ConfigId",
                        "InventSizeId",
                        "InventColorId",
                        "InventSiteId",
                        "InventStyle"
                    };

                    // Read actual headers from the first row of the Excel file
                    var actualHeaders = new List<string>();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var header = worksheet.Cells[1, col].Text?.Trim();
                        if (!string.IsNullOrEmpty(header))
                        {
                            actualHeaders.Add(header);
                        }
                    }

                    // Validate headers
                    var missingHeaders = expectedHeaders.Except(actualHeaders).ToList();
                    var extraHeaders = actualHeaders.Except(expectedHeaders).ToList();
                    if (missingHeaders.Any() || extraHeaders.Any())
                    {
                        string errorMessage = "Invalid Excel file headers. ";
                        if (missingHeaders.Any())
                        {
                            errorMessage += $"Missing headers: {string.Join(", ", missingHeaders)}. ";
                        }
                        if (extraHeaders.Any())
                        {
                            errorMessage += $"Unexpected headers: {string.Join(", ", extraHeaders)}. ";
                        }
                        errorMessage += $"Expected headers: {string.Join(", ", expectedHeaders)}.";
                        DisplayErrorMessage(errorMessage);
                        return;
                    }

                    // Map Excel columns to DataTable columns by header name
                    var headerIndices = expectedHeaders.ToDictionary(
                        header => header,
                        header => actualHeaders.IndexOf(header) + 1 // 1-based index for EPPlus
                    );

                    foreach (var header in expectedHeaders)
                    {
                        dt.Columns.Add(header);
                    }

                    string transferId = Session["TransferID"] as string;
                    string fromWarehouse = Session["FromWarehouse"] as string;

                    if (string.IsNullOrEmpty(transferId))
                    {
                        DisplayErrorMessage("Transfer ID is missing. Please ensure a valid Transfer ID is provided.");
                        return;
                    }

                    // Start from row 2, assuming row 1 is headers
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow dataRow = dt.NewRow();

                        // Map Excel columns to DataTable columns using header indices
                        dataRow["Itemid"] = worksheet.Cells[row, headerIndices["Itemid"]].Text?.Trim() ?? "";
                        dataRow["TransferId"] = transferId; // Use session TransferID
                        dataRow["LinesShipDate"] = DateTime.Today;
                        dataRow["LinesReceiveDate"] = DateTime.Today;
                        dataRow["ReserveItem"] = false; // Default value
                        dataRow["IsCatchWeight"] = false; // Default value
                        string qtyTransfer = worksheet.Cells[row, headerIndices["QtyTransfer"]].Text?.Trim() ?? "0";
                        dataRow["QtyTransfer"] = int.TryParse(qtyTransfer, out int qty) && qty > 0 ? qty : 0;
                        dataRow["QtyShipped"] = 0; // Default value
                        dataRow["InventBatchId"] = worksheet.Cells[row, headerIndices["InventBatchId"]].Text?.Trim() ?? "";
                        dataRow["WmsLocationId"] = worksheet.Cells[row, headerIndices["WmsLocationId"]].Text?.Trim() ?? "";
                        dataRow["WmsPalletId"] = worksheet.Cells[row, headerIndices["WmsPalletId"]].Text?.Trim() ?? "";
                        dataRow["InventSerialId"] = worksheet.Cells[row, headerIndices["InventSerialId"]].Text?.Trim() ?? "";
                        dataRow["InventLocationId"] = fromWarehouse ?? "";
                        dataRow["ConfigId"] = worksheet.Cells[row, headerIndices["ConfigId"]].Text?.Trim() ?? "";
                        dataRow["InventSizeId"] = worksheet.Cells[row, headerIndices["InventSizeId"]].Text?.Trim() ?? "";
                        dataRow["InventColorId"] = worksheet.Cells[row, headerIndices["InventColorId"]].Text?.Trim() ?? "";
                        dataRow["InventSiteId"] = worksheet.Cells[row, headerIndices["InventSiteId"]].Text?.Trim() ?? "";
                        dataRow["InventStyle"] = worksheet.Cells[row, headerIndices["InventStyle"]].Text?.Trim() ?? "";

                        // Validate required fields
                        if (string.IsNullOrEmpty(dataRow["Itemid"].ToString()))
                        {
                            DisplayErrorMessage($"Invalid or missing Item ID in row {row}.");
                            return;
                        }

                        if (int.Parse(dataRow["QtyTransfer"].ToString()) <= 0)
                        {
                            DisplayErrorMessage($"Invalid or non-positive Transfer Quantity in row {row}.");
                            return;
                        }

                        dt.Rows.Add(dataRow);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        DisplayErrorMessage("No valid data found in the Excel file.");
                        return;
                    }
                }

                TransferOrderLines transferLines = new TransferOrderLines();
                SysOperationResult_BOL result = transferLines.create(dt);

                if (result != null && result.isSuccess)
                {
                    NotificationMessage.showMessage(result);
                    reBindGrid(); // Refresh the grid to show new lines

                    // Refresh parent GridView
                    string refreshScript = @"
                            if (window.opener && !window.opener.closed) {
                                parent.__doPostBack('', 'RefreshGrid');
                            }";
                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshGrid", refreshScript, true);
                }
                else
                {
                    NotificationMessage.showMessage(result);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
                DisplayErrorMessage("Error processing Excel file: " + ex.Message);
            }
        }

        private void DisplayErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
            lblMessage.Attributes["class"] = "alert alert-danger";
            lblMessage.Style["font-size"] = "12px";
            lblMessage.Style["padding"] = "6px";
        }

        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }

        private void FilterData()
        {
            string transferId = Session["TransferID"] as string;
            string inventdimId = Session["InventDimId"] as string;
            if (string.IsNullOrEmpty(transferId))
                return;

            DataTable filteredData = newTransferorderlines.retrieveAll(transferId);

            if (filteredData != null && filteredData.Rows.Count > 0)
            {
                gridView.DataSource = filteredData;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(filteredData);
            }
            else
            {
                gridView.DataSource = null;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(new DataTable());
            }
        }

        private void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
            {
                gridView.DataSource = null;
                gridView.DataBind();
            }
        }

        private void getGridDataTable()
        {
            string transferId = Request.QueryString["TransferID"] ?? (string)Session["TransferID"];

            if (string.IsNullOrEmpty(transferId))
            {
                return;
            }

            DataTable dt = newTransferorderlines.retrieveAll(transferId);

            if (dt != null)
            {
                SessionVariables.setSessionDataTable(dt);
            }
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    gridView.SelectedIndex = 0; // Select first row
                  
                }

                Label lblLinesShipDate = e.Row.FindControl("lblLinesShipDate") as Label;
                if (lblLinesShipDate != null && DateTime.TryParse(lblLinesShipDate.Text, out DateTime LinesShipDate))
                {
                    lblLinesShipDate.Text = LinesShipDate == new DateTime(1900, 1, 1)
                        ? ""
                        : LinesShipDate.ToString("M-d-yyyy");
                }
                Label lblLinesReceiveDate = e.Row.FindControl("lblLinesReceiveDate") as Label;
                if (lblLinesReceiveDate != null && DateTime.TryParse(lblLinesReceiveDate.Text, out DateTime LinesReceiveDate))
                {
                    lblLinesReceiveDate.Text = LinesReceiveDate == new DateTime(1900, 1, 1)
                        ? ""
                        : LinesReceiveDate.ToString("M-d-yyyy");
                }
                Label lblQtyshipped = e.Row.FindControl("lblQtyshipped") as Label;
                Label lblQtyRecieved = e.Row.FindControl("lblQtyRecieved") as Label;
                if (lblQtyshipped != null && lblQtyRecieved != null)
                {
                    decimal shippedQtyValue = !string.IsNullOrEmpty(lblQtyshipped.Text) ? Convert.ToDecimal(lblQtyshipped.Text) : 0;
                    decimal recievedQtyValue = !string.IsNullOrEmpty(lblQtyRecieved.Text) ? Convert.ToDecimal(lblQtyRecieved.Text) : 0;
                    decimal remainingValue = shippedQtyValue - recievedQtyValue;
                    if (remainingValue > 0)
                    {
                        //btnRecieve_All.Enabled = true;
                        btnRecieve.Enabled = true;
                    }
                }

            }

            //Label TransitWarehouse = (Label)e.Row.FindControl("lblNewTransitLocationName");
            //Session["TransitWarhouse"] = TransitWarehouse.Text;

            if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                var dataItem = (DataRowView)e.Row.DataItem;
               
                
                DropDownList ddlItemid = (DropDownList)e.Row.FindControl("ddlItemId");
                if (ddlItemid != null)
                {
                    TransferOrderLines newlines = new TransferOrderLines();
                    DataTable dt = newlines.retrieveitemid();

                    //ddlItemid.DataSource = dt;
                    //ddlItemid.DataValueField = "Itemid";   // value to pass
                    //ddlItemid.DataTextField = "ItemName";    // text to show
                    //ddlItemid.DataBind();


                    //ddlItemid.Items.Insert(0, new ListItem("", string.Empty));

                    //ddlItemid.CssClass += " filterable-dropdown";

                    // Add a combined display column
                    dt.Columns.Add("DisplayText", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        string id = row["Itemid"].ToString();
                        string name = row["ItemName"].ToString();
                        row["DisplayText"] = id.PadRight(15) + " | " + name;
                    }

                    ddlItemid.DataSource = dt;
                    ddlItemid.DataValueField = "Itemid";     // unchanged
                    ddlItemid.DataTextField = "DisplayText"; // changed from ItemName
                    ddlItemid.DataBind();

                    ddlItemid.Items.Insert(0, new ListItem("", string.Empty));
                    ddlItemid.CssClass += " filterable-dropdown";

                }
            }

            //if (e.Row.RowType == DataControlRowType.DataRow &&
            //    (e.Row.RowState & DataControlRowState.Edit) > 0)
            //{
            //    var dataItem = (DataRowView)e.Row.DataItem;

            //    DropDownList ddlItemid = (DropDownList)e.Row.FindControl("ddlItemId");
            //    if (ddlItemid != null)
            //    {
            //        TransferOrderLines newlines = new TransferOrderLines();
            //        DataTable dt = newlines.retrieveitemid();

            //        ddlItemid.Items.Clear(); // Clear any existing items

            //        // Add an empty item first (optional)
            //        ddlItemid.Items.Add(new ListItem("", string.Empty));

            //        foreach (DataRow row in dt.Rows)
            //        {
            //            string value = row["Itemid"].ToString(); // Value to pass


            //            ddlItemid.Items.Add(new ListItem(value));
            //        }

            //        ddlItemid.CssClass += " filterable-dropdown";
            //    }
            //}
        }

        protected void btnHistory_Click(object sender, EventArgs e)
        {
            Session["transferId"] = Session["TransferID"];
            Response.Redirect(ResolveUrl("~/ESS/PR/TransferOrder_History.aspx"), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = newTransferorderlines.deletelines(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                reBindGrid();

            }
            else
            {
                SysOperationResult_BOL error = new SysOperationResult_BOL();
                error.AlertType = AlertType.Error.ToString();
                error.Message = "Please select at least one transfer Line to proceed";
                error.isSuccess = false;
                NotificationMessage.showMessage(error);
            }
        }


        //protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chkSelectRow = sender as CheckBox;
        //    if (chkSelectRow == null) return;

        //    // If this checkbox was just checked, uncheck every other row so only one stays selected
        //    if (chkSelectRow.Checked)
        //    {
        //        foreach (GridViewRow row in gridView.Rows)
        //        {
        //            CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
        //            if (chk != null && chk != chkSelectRow)
        //            {
        //                chk.Checked = false;
        //            }
        //        }
        //    }

        //    // Determine the currently selected row by scanning for whichever checkbox is checked.
        //    // Doing it this way (instead of trusting "sender") makes the result deterministic
        //    // even if ASP.NET fires CheckedChanged for more than one checkbox in the same postback.
        //    GridViewRow selectedRow = null;
        //    foreach (GridViewRow row in gridView.Rows)
        //    {
        //        CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
        //        if (chk != null && chk.Checked)
        //        {
        //            selectedRow = row;
        //            break;
        //        }
        //    }

        //    bool anyChecked = selectedRow != null;

        //    if (anyChecked)
        //    {
        //        // Identification
        //        LineDetailTransferNumber.Text = GetLabelText(selectedRow, "lblTransferID");
        //        txtLineNumber.Text = GetLabelText(selectedRow, "lblLineNum");

        //        // Status
        //        txtRemaining.Text = GetLabelText(selectedRow, "lblRemainStatus");

        //        // Inventory
        //        txtShipmentLotID.Text = GetLabelText(selectedRow, "lblInventTransId");
        //        txtTransitShipmentLotID.Text = GetLabelText(selectedRow, "lblInventTransIdTo");
        //        txtTransitReceiveLotID.Text = GetLabelText(selectedRow, "lblInventTransIdFrom");
        //        txtReceiveLotID.Text = GetLabelText(selectedRow, "lblInventTransIdRecive");
        //        txtScrapLotID.Text = GetLabelText(selectedRow, "lblInventTransIdScrap");
        //        txtDimensionNumber.Text = GetLabelText(selectedRow, "lblInventDimId");

        //        // Voyages
        //        txtVoyage.Text = GetLabelText(selectedRow, "lblVoyage");
        //        txtVoyageStatus.Text = GetLabelText(selectedRow, "lblVoyageStatus");

        //        // Arrival / Shipping
        //        txtArrivalGroup.Text = GetLabelText(selectedRow, "lblArrivalGroup");
        //        txtShippingContainer.Text = GetLabelText(selectedRow, "lblShippingContainer");

        //        shipitemnumber.Text = GetLabelText(selectedRow, "lblItemID");
        //        shiptransferquantity.Text = GetLabelText(selectedRow, "lblQtyTransfer");
        //        shipnow.Text = GetLabelText(selectedRow, "lblQtyShipNow");
        //        shipshippedquantity.Text = GetLabelText(selectedRow, "lblQtyshipped");
        //        shipremaining.Text = GetLabelText(selectedRow, "lblQtyRemainShip");
        //        receiveitemnumber.Text = GetLabelText(selectedRow, "lblItemID");
        //        receivequantity.Text = GetLabelText(selectedRow, "lblQtyshipped");
        //        receivenow.Text = GetLabelText(selectedRow, "lblQtyReceiveNow");
        //        receivereceivedquantity.Text = GetLabelText(selectedRow, "lblQtyRecieved");
        //        receiveremaining.Text = GetLabelText(selectedRow, "lblQtyRemainReceive");

        //        // Inventory Dimensions
        //        //ddlConfiguration.Text = GetLabelText(selectedRow, "lblConfigId");
        //        //ddlColorLinesDetail.SelectedValue = GetLabelText(selectedRow, "lblInventColorId");
        //        //txtSize.Text = GetLabelText(selectedRow, "lblInventSizeId");
        //        //txtStyle.Text = GetLabelText(selectedRow, "lblInventStyleId");
        //        txtVersion.Text = "\uFEFF";
        //        txtSiteLineDetail.Text = GetLabelText(selectedRow, "lblInventSiteId");
        //        txtWarehouseLineDetail.Text = GetLabelText(selectedRow, "lblInventLocationId");
        //        //txtBatchNumber.Text = GetLabelText(selectedRow, "lblInventBatchId");
        //        //txtLocation.Text = GetLabelText(selectedRow, "lblWMSLocationId");
        //        //txtSerialNumber.Text = GetLabelText(selectedRow, "lblInventSerialId");

        //        string ItemId = GetLabelText(selectedRow, "lblItemID");
        //        //BindSiteLineDetail(); // populate ddl
        //        BindConfigurationforLineDetail(ItemId);
        //        BindColorLineDetail(ItemId);
        //        BindSizeLineDetail(ItemId);
        //        BindStyleLineDetail(ItemId);
        //        //BindWarehouseLineDetail(ItemId);
        //        BindBatchLineDetail(ItemId);
        //        BindWmsLocationLineDetail(ItemId);
        //        BindInventSerialLineDetail(ItemId);

        //        string configId = GetLabelText(selectedRow, "lblConfigId");
        //        if (!string.IsNullOrEmpty(configId) && ddlConfigurationLineDetail.Items.FindByValue(configId) != null)
        //        {
        //            ddlConfigurationLineDetail.SelectedValue = configId;
        //        }
        //        else
        //        {
        //            ddlConfigurationLineDetail.SelectedIndex = 0; // fallback to blank
        //            ddlConfigurationLineDetail.Attributes["readonly"] = "readonly";
        //            ddlConfigurationLineDetail.CssClass += " custom-textbox";
        //        }

        //        string sizeId = GetLabelText(selectedRow, "lblInventSizeId");
        //        if (!string.IsNullOrEmpty(sizeId) && ddlSizeLineDetail.Items.FindByValue(sizeId) != null)
        //        {
        //            ddlSizeLineDetail.SelectedValue = sizeId;
        //        }
        //        else
        //        {
        //            ddlSizeLineDetail.SelectedIndex = 0;
        //            ddlSizeLineDetail.Attributes["readonly"] = "readonly";
        //            ddlSizeLineDetail.CssClass += " custom-textbox";
        //        }

        //        string styleId = GetLabelText(selectedRow, "lblInventStyleId");
        //        if (!string.IsNullOrEmpty(styleId) && ddlStyleLineDetail.Items.FindByValue(styleId) != null)
        //        {
        //            ddlStyleLineDetail.SelectedValue = styleId;
        //        }
        //        else
        //        {
        //            ddlStyleLineDetail.SelectedIndex = 0;
        //            ddlStyleLineDetail.Attributes["readonly"] = "readonly";
        //            ddlStyleLineDetail.CssClass += " custom-textbox";
        //        }

        //        string colorId = GetLabelText(selectedRow, "lblInventColorId");
        //        if (!string.IsNullOrEmpty(colorId) && ddlColorLineDetail.Items.FindByValue(colorId) != null)
        //        {
        //            ddlColorLineDetail.SelectedValue = colorId;
        //        }
        //        else
        //        {
        //            ddlColorLineDetail.SelectedIndex = 0; // clear selection
        //            ddlColorLineDetail.Attributes["readonly"] = "readonly";
        //            ddlColorLineDetail.CssClass += " custom-textbox";
        //        }

        //        string batchId = GetLabelText(selectedRow, "lblInventBatchId");
        //        if (!string.IsNullOrEmpty(batchId) && ddlBatchLineDetail.Items.FindByValue(batchId) != null)
        //        {
        //            ddlBatchLineDetail.SelectedValue = batchId;
        //        }
        //        else
        //        {
        //            ddlBatchLineDetail.SelectedIndex = 0;
        //            ddlBatchLineDetail.Attributes["readonly"] = "readonly";
        //            ddlBatchLineDetail.CssClass += " custom-textbox";
        //        }

        //        string wmsLocationId = GetLabelText(selectedRow, "lblWMSLocationId");
        //        if (!string.IsNullOrEmpty(wmsLocationId) && ddlWmsLocationLineDetail.Items.FindByValue(wmsLocationId) != null)
        //        {
        //            ddlWmsLocationLineDetail.SelectedValue = wmsLocationId;
        //        }
        //        else
        //        {
        //            ddlWmsLocationLineDetail.SelectedIndex = 0;
        //            ddlWmsLocationLineDetail.Attributes["readonly"] = "readonly";
        //            ddlWmsLocationLineDetail.CssClass += " custom-textbox";
        //        }

        //        string serialId = GetLabelText(selectedRow, "lblInventSerialId");
        //        if (!string.IsNullOrEmpty(serialId) && ddlInventSerialLineDetail.Items.FindByValue(serialId) != null)
        //        {
        //            ddlInventSerialLineDetail.SelectedValue = serialId;
        //        }
        //        else
        //        {
        //            ddlInventSerialLineDetail.SelectedIndex = 0;
        //            ddlInventSerialLineDetail.Attributes["readonly"] = "readonly";
        //            ddlInventSerialLineDetail.CssClass += " custom-textbox";
        //        }

        //        switch (txtRemaining.Text)
        //        {
        //            case "Receive updates":
        //                txtReceiveNow.Visible = true;
        //                lblReceiveNow.Visible = false;
        //                txtshipNow.Visible = false;
        //                lblShipNow.Visible = true;
        //                break;
        //            case "Shipping updates":
        //                txtReceiveNow.Visible = true;
        //                lblReceiveNow.Visible = false;
        //                txtshipNow.Visible = true;
        //                lblShipNow.Visible = false;
        //                break;
        //            case "Nothing":
        //            case "":
        //                txtReceiveNow.Visible = false;
        //                lblReceiveNow.Visible = true;
        //                txtshipNow.Visible = false;
        //                lblShipNow.Visible = true;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        lblShipNow.Visible = true;
        //        txtshipNow.Visible = false;
        //        lblReceiveNow.Visible = true;
        //        txtReceiveNow.Visible = false;

        //        setFieldsEmpty();
        //    }
        //}

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectRow = sender as CheckBox;
            if (chkSelectRow == null) return;

            // No longer uncheck sibling rows here — that's what let a single click
            // wipe out "Select All" and any other manually-checked rows.
            // Multi-select now works correctly for Remove / Deliver Remainder / On-hand.

            // Line Details reflects whichever row just changed (this checkbox's row),
            // instead of scanning the grid for "the" checked row.
            GridViewRow selectedRow = chkSelectRow.NamingContainer as GridViewRow;
            bool anyChecked = chkSelectRow.Checked;

            if (anyChecked && selectedRow != null)
            {
                // Identification
                LineDetailTransferNumber.Text = GetLabelText(selectedRow, "lblTransferID");
                txtLineNumber.Text = GetLabelText(selectedRow, "lblLineNum");

                // Status
                txtRemaining.Text = GetLabelText(selectedRow, "lblRemainStatus");

                // Inventory
                txtShipmentLotID.Text = GetLabelText(selectedRow, "lblInventTransId");
                txtTransitShipmentLotID.Text = GetLabelText(selectedRow, "lblInventTransIdTo");
                txtTransitReceiveLotID.Text = GetLabelText(selectedRow, "lblInventTransIdFrom");
                txtReceiveLotID.Text = GetLabelText(selectedRow, "lblInventTransIdRecive");
                txtScrapLotID.Text = GetLabelText(selectedRow, "lblInventTransIdScrap");
                txtDimensionNumber.Text = GetLabelText(selectedRow, "lblInventDimId");

                // Voyages
                txtVoyage.Text = GetLabelText(selectedRow, "lblVoyage");
                txtVoyageStatus.Text = GetLabelText(selectedRow, "lblVoyageStatus");

                // Arrival / Shipping
                txtArrivalGroup.Text = GetLabelText(selectedRow, "lblArrivalGroup");
                txtShippingContainer.Text = GetLabelText(selectedRow, "lblShippingContainer");

                shipitemnumber.Text = GetLabelText(selectedRow, "lblItemID");
                shiptransferquantity.Text = GetLabelText(selectedRow, "lblQtyTransfer");
                shipnow.Text = GetLabelText(selectedRow, "lblQtyShipNow");
                shipshippedquantity.Text = GetLabelText(selectedRow, "lblQtyshipped");
                shipremaining.Text = GetLabelText(selectedRow, "lblQtyRemainShip");
                receiveitemnumber.Text = GetLabelText(selectedRow, "lblItemID");
                receivequantity.Text = GetLabelText(selectedRow, "lblQtyshipped");
                receivenow.Text = GetLabelText(selectedRow, "lblQtyReceiveNow");
                receivereceivedquantity.Text = GetLabelText(selectedRow, "lblQtyRecieved");
                receiveremaining.Text = GetLabelText(selectedRow, "lblQtyRemainReceive");

                // Inventory Dimensions
                txtVersion.Text = "\uFEFF";
                txtSiteLineDetail.Text = GetLabelText(selectedRow, "lblInventSiteId");
                txtWarehouseLineDetail.Text = GetLabelText(selectedRow, "lblInventLocationId");

                string ItemId = GetLabelText(selectedRow, "lblItemID");
                BindConfigurationforLineDetail(ItemId);
                BindColorLineDetail(ItemId);
                BindSizeLineDetail(ItemId);
                BindStyleLineDetail(ItemId);
                BindBatchLineDetail(ItemId);
                BindWmsLocationLineDetail(ItemId);
                BindInventSerialLineDetail(ItemId);

                string configId = GetLabelText(selectedRow, "lblConfigId");
                if (!string.IsNullOrEmpty(configId) && ddlConfigurationLineDetail.Items.FindByValue(configId) != null)
                {
                    ddlConfigurationLineDetail.SelectedValue = configId;
                }
                else
                {
                    ddlConfigurationLineDetail.SelectedIndex = 0; // fallback to blank
                    ddlConfigurationLineDetail.Attributes["readonly"] = "readonly";
                    ddlConfigurationLineDetail.CssClass += " custom-textbox";
                }

                string sizeId = GetLabelText(selectedRow, "lblInventSizeId");
                if (!string.IsNullOrEmpty(sizeId) && ddlSizeLineDetail.Items.FindByValue(sizeId) != null)
                {
                    ddlSizeLineDetail.SelectedValue = sizeId;
                }
                else
                {
                    ddlSizeLineDetail.SelectedIndex = 0;
                    ddlSizeLineDetail.Attributes["readonly"] = "readonly";
                    ddlSizeLineDetail.CssClass += " custom-textbox";
                }

                string styleId = GetLabelText(selectedRow, "lblInventStyleId");
                if (!string.IsNullOrEmpty(styleId) && ddlStyleLineDetail.Items.FindByValue(styleId) != null)
                {
                    ddlStyleLineDetail.SelectedValue = styleId;
                }
                else
                {
                    ddlStyleLineDetail.SelectedIndex = 0;
                    ddlStyleLineDetail.Attributes["readonly"] = "readonly";
                    ddlStyleLineDetail.CssClass += " custom-textbox";
                }

                string colorId = GetLabelText(selectedRow, "lblInventColorId");
                if (!string.IsNullOrEmpty(colorId) && ddlColorLineDetail.Items.FindByValue(colorId) != null)
                {
                    ddlColorLineDetail.SelectedValue = colorId;
                }
                else
                {
                    ddlColorLineDetail.SelectedIndex = 0; // clear selection
                    ddlColorLineDetail.Attributes["readonly"] = "readonly";
                    ddlColorLineDetail.CssClass += " custom-textbox";
                }

                string batchId = GetLabelText(selectedRow, "lblInventBatchId");
                if (!string.IsNullOrEmpty(batchId) && ddlBatchLineDetail.Items.FindByValue(batchId) != null)
                {
                    ddlBatchLineDetail.SelectedValue = batchId;
                }
                else
                {
                    ddlBatchLineDetail.SelectedIndex = 0;
                    ddlBatchLineDetail.Attributes["readonly"] = "readonly";
                    ddlBatchLineDetail.CssClass += " custom-textbox";
                }

                string wmsLocationId = GetLabelText(selectedRow, "lblWMSLocationId");
                if (!string.IsNullOrEmpty(wmsLocationId) && ddlWmsLocationLineDetail.Items.FindByValue(wmsLocationId) != null)
                {
                    ddlWmsLocationLineDetail.SelectedValue = wmsLocationId;
                }
                else
                {
                    ddlWmsLocationLineDetail.SelectedIndex = 0;
                    ddlWmsLocationLineDetail.Attributes["readonly"] = "readonly";
                    ddlWmsLocationLineDetail.CssClass += " custom-textbox";
                }

                string serialId = GetLabelText(selectedRow, "lblInventSerialId");
                if (!string.IsNullOrEmpty(serialId) && ddlInventSerialLineDetail.Items.FindByValue(serialId) != null)
                {
                    ddlInventSerialLineDetail.SelectedValue = serialId;
                }
                else
                {
                    ddlInventSerialLineDetail.SelectedIndex = 0;
                    ddlInventSerialLineDetail.Attributes["readonly"] = "readonly";
                    ddlInventSerialLineDetail.CssClass += " custom-textbox";
                }

                switch (txtRemaining.Text)
                {
                    case "Receive updates":
                        txtReceiveNow.Visible = true;
                        lblReceiveNow.Visible = false;
                        txtshipNow.Visible = false;
                        lblShipNow.Visible = true;
                        break;
                    case "Shipping updates":
                        txtReceiveNow.Visible = true;
                        lblReceiveNow.Visible = false;
                        txtshipNow.Visible = true;
                        lblShipNow.Visible = false;
                        break;
                    case "Nothing":
                    case "":
                        txtReceiveNow.Visible = false;
                        lblReceiveNow.Visible = true;
                        txtshipNow.Visible = false;
                        lblShipNow.Visible = true;
                        break;
                }
            }
            else
            {
                lblShipNow.Visible = true;
                txtshipNow.Visible = false;
                lblReceiveNow.Visible = true;
                txtReceiveNow.Visible = false;

                setFieldsEmpty();
            }
        }

        private string GetLabelText(Control row, string labelId)
        {
            var label = row.FindControl(labelId) as Label;
            var text = label?.Text?.Trim();
            return string.IsNullOrEmpty(text) ? "\uFEFF" : text;

        }

        protected void shipnow_updateLine(object sender, EventArgs e)
        {
            string shipNowValue = shipnow.Text;
            TransferOrderlinesContract contract = new TransferOrderlinesContract();
            TransferOrderLines lines = new TransferOrderLines();

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    contract.RecId = Convert.ToInt64(((Label)row.FindControl("lblRecId")).Text);
                    contract.TransferId = ((Label)row.FindControl("lblTransferID")).Text;
                    contract.Itemid = ((Label)row.FindControl("lblItemID")).Text;
                    contract.QtyTransfer = Convert.ToDecimal(((Label)row.FindControl("lblQtyTransfer")).Text);
                    contract.CWQtyTransfer = Convert.ToDecimal(((Label)row.FindControl("lblCWQtyTransfer")).Text);
                    contract.LinesShipDate = Convert.ToDateTime(((Label)row.FindControl("lblLinesShipDate")).Text);
                    contract.LinesReceiveDate = Convert.ToDateTime(((Label)row.FindControl("lblLinesReceiveDate")).Text);
                    contract.TransctionCode = ((Label)row.FindControl("lblTransctionCode")).Text;
                    contract.InventBatchId = ((Label)row.FindControl("lblInventBatchId")).Text;
                    contract.WMSLocationId = ((Label)row.FindControl("lblWMSLocationId")).Text;
                    contract.WMSPalletId = ((Label)row.FindControl("lblWMSPalletId")).Text;
                    contract.InventSerialId = ((Label)row.FindControl("lblInventSerialId")).Text;
                    contract.InventLocationId = ((Label)row.FindControl("lblInventLocationId")).Text;
                    contract.ConfigId = ((Label)row.FindControl("lblConfigId")).Text;
                    contract.InventSizeId = ((Label)row.FindControl("lblInventSizeId")).Text;
                    contract.InventColorId = ((Label)row.FindControl("lblInventColorId")).Text;
                    contract.InventStyle = ((Label)row.FindControl("lblInventStyleId")).Text;
                    contract.InventSiteId = ((Label)row.FindControl("lblInventSiteId")).Text;
                    contract.UnitId = ((Label)row.FindControl("lblUnitId")).Text;
                    contract.ReserveItem = ((Label)row.FindControl("lblReserveItem")).Text == "1" ? NoYes.Yes : NoYes.No;
                    contract.Qtyshipped = Convert.ToDecimal(((Label)row.FindControl("lblQtyshipped")).Text);
                    contract.Dimensionshipfrom = Convert.ToInt64(((Label)row.FindControl("lblDimensionshipfrom")).Text);
                    contract.Dimensionshipto = Convert.ToInt64(((Label)row.FindControl("lblDimensionshipto")).Text);
                    contract.InventDimId = ((Label)row.FindControl("lblInventDimId")).Text;
                    contract.LocationIdFrom = ((Label)row.FindControl("lblLocationIdFrom")).Text;
                    contract.LocationIdTo = ((Label)row.FindControl("lblLocationIdTo")).Text;
                    contract.LineNum = Convert.ToDecimal(((Label)row.FindControl("lblLineNum")).Text);
                    contract.QtyRecieved = Convert.ToDecimal(((Label)row.FindControl("lblQtyRecieved")).Text);
                    contract.QtyRecieveNow = Convert.ToDecimal(((Label)row.FindControl("lblQtyReceiveNow")).Text);
                    contract.QtyRemainRecieve = Convert.ToDecimal(((Label)row.FindControl("lblQtyRemainReceive")).Text);
                    contract.QtyRemainShip = Convert.ToDecimal(((Label)row.FindControl("lblQtyRemainShip")).Text);
                    contract.QtyScrapped = Convert.ToDecimal(((Label)row.FindControl("lblQtyScrapped")).Text);
                    contract.QtyShipNow = Convert.ToDecimal(shipNowValue);
                    contract.RemainStatus = ((Label)row.FindControl("lblRemainStatus")).Text;
                    contract.InventTransId = ((Label)row.FindControl("lblInventTransId")).Text;
                    contract.InventTransIdRecive = ((Label)row.FindControl("lblInventTransIdRecive")).Text;
                    contract.InventTransIdScrap = ((Label)row.FindControl("lblInventTransIdScrap")).Text;
                    contract.InventTransIdFrom = ((Label)row.FindControl("lblInventTransIdFrom")).Text;
                    contract.InventTransIdTo = ((Label)row.FindControl("lblInventTransIdTo")).Text;
                    contract.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    lines.update(contract);
                }
            }
        }

        protected void receivenow_updateLine(object sender, EventArgs e)
        {
            string receiveNowValue = receivenow.Text;
            TransferOrderlinesContract contract = new TransferOrderlinesContract();
            TransferOrderLines lines = new TransferOrderLines();

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    contract.RecId = Convert.ToInt64(((Label)row.FindControl("lblRecId")).Text);
                    contract.TransferId = ((Label)row.FindControl("lblTransferID")).Text;
                    contract.Itemid = ((Label)row.FindControl("lblItemID")).Text;
                    contract.QtyTransfer = Convert.ToDecimal(((Label)row.FindControl("lblQtyTransfer")).Text);
                    contract.CWQtyTransfer = Convert.ToDecimal(((Label)row.FindControl("lblCWQtyTransfer")).Text);
                    contract.LinesShipDate = Convert.ToDateTime(((Label)row.FindControl("lblLinesShipDate")).Text);
                    contract.LinesReceiveDate = Convert.ToDateTime(((Label)row.FindControl("lblLinesReceiveDate")).Text);
                    contract.TransctionCode = ((Label)row.FindControl("lblTransctionCode")).Text;
                    contract.InventBatchId = ((Label)row.FindControl("lblInventBatchId")).Text;
                    contract.WMSLocationId = ((Label)row.FindControl("lblWMSLocationId")).Text;
                    contract.WMSPalletId = ((Label)row.FindControl("lblWMSPalletId")).Text;
                    contract.InventSerialId = ((Label)row.FindControl("lblInventSerialId")).Text;
                    contract.InventLocationId = ((Label)row.FindControl("lblInventLocationId")).Text;
                    contract.ConfigId = ((Label)row.FindControl("lblConfigId")).Text;
                    contract.InventSizeId = ((Label)row.FindControl("lblInventSizeId")).Text;
                    contract.InventColorId = ((Label)row.FindControl("lblInventColorId")).Text;
                    contract.InventStyle = ((Label)row.FindControl("lblInventStyleId")).Text;
                    contract.InventSiteId = ((Label)row.FindControl("lblInventSiteId")).Text;
                    contract.UnitId = ((Label)row.FindControl("lblUnitId")).Text;
                    contract.ReserveItem = ((Label)row.FindControl("lblReserveItem")).Text == "1" ? NoYes.Yes : NoYes.No;
                    contract.Qtyshipped = Convert.ToDecimal(((Label)row.FindControl("lblQtyshipped")).Text);
                    contract.Dimensionshipfrom = Convert.ToInt64(((Label)row.FindControl("lblDimensionshipfrom")).Text);
                    contract.Dimensionshipto = Convert.ToInt64(((Label)row.FindControl("lblDimensionshipto")).Text);
                    contract.InventDimId = ((Label)row.FindControl("lblInventDimId")).Text;
                    contract.LocationIdFrom = ((Label)row.FindControl("lblLocationIdFrom")).Text;
                    contract.LocationIdTo = ((Label)row.FindControl("lblLocationIdTo")).Text;
                    contract.LineNum = Convert.ToDecimal(((Label)row.FindControl("lblLineNum")).Text);
                    contract.QtyRecieved = Convert.ToDecimal(((Label)row.FindControl("lblQtyRecieved")).Text);
                    contract.QtyRecieveNow = Convert.ToDecimal(receiveNowValue);
                    contract.QtyRemainRecieve = Convert.ToDecimal(((Label)row.FindControl("lblQtyRemainReceive")).Text);
                    contract.QtyRemainShip = Convert.ToDecimal(((Label)row.FindControl("lblQtyRemainShip")).Text);
                    contract.QtyScrapped = Convert.ToDecimal(((Label)row.FindControl("lblQtyScrapped")).Text);
                    contract.QtyShipNow = Convert.ToDecimal(((Label)row.FindControl("lblQtyShipNow")).Text);
                    contract.RemainStatus = ((Label)row.FindControl("lblRemainStatus")).Text;
                    contract.InventTransId = ((Label)row.FindControl("lblInventTransId")).Text;
                    contract.InventTransIdRecive = ((Label)row.FindControl("lblInventTransIdRecive")).Text;
                    contract.InventTransIdScrap = ((Label)row.FindControl("lblInventTransIdScrap")).Text;
                    contract.InventTransIdFrom = ((Label)row.FindControl("lblInventTransIdFrom")).Text;
                    contract.InventTransIdTo = ((Label)row.FindControl("lblInventTransIdTo")).Text;
                    contract.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    lines.update(contract);
                }
            }
        }

        protected void ddlTemId_fillDimension(object sender, EventArgs e)
        {
            DropDownList ddlItemId = (DropDownList)sender;
            // Get the row containing the dropdown
            GridViewRow row = (GridViewRow)ddlItemId.NamingContainer;

            string selectedItemId = ddlItemId.SelectedValue;

            if (!string.IsNullOrEmpty(selectedItemId))
            {
                Label lblItemName = (Label)row.FindControl("lblItemName");
                if (lblItemName != null)
                {
                    lblItemName.Text = ddlItemId.SelectedItem?.Text ?? string.Empty;
                }

                Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
                {
                    { "Batch number",("", BindInventBatchId(selectedItemId, row) )},
                    { "Location", ("", BindWmsLocationId(selectedItemId, row)) },
                    { "Serial number", ("", BindInventSerialId(selectedItemId, row)) },
                    { "Configuration", ("", BindConfigId(selectedItemId, row)) },
                    { "Combinations", ("", BindCombination(selectedItemId, row)) },
                    { "Size", ("", BindInventSizeId(selectedItemId, row)) },
                    { "Color", ("", BindInventColorId(selectedItemId, row)) },
                    { "Style", ("", BindInventStyleId(selectedItemId, row)) },
                    { "WMS pallet", ("", BindWmsPalletId(selectedItemId, row)) },
                    //{ "Warehouse", ("", BindInventLocationId(selectedItemId, row)) },
                    //{ "Site", ("", BindSiteId(selectedItemId, row)) }
                };

                foreach (DataControlField column in gridView.Columns)
                {
                    if (column is TemplateField && column.HeaderText != null)
                    {
                        string header = column.HeaderText;

                        if (columnVisibilities.ContainsKey(header))
                        {
                            columnVisibilities.TryGetValue(header, out var fieldMeta);
                            column.Visible = fieldMeta.IsVisible;
                        }
                    }
                }
                // Default quantity
                TextBox txtQtyTransfer = (TextBox)row.FindControl("txtQtyTransfer"); 
                if (txtQtyTransfer != null)
                    txtQtyTransfer.Text = "1";

                
            }
        }


        private bool BindInventBatchId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventBatchId(itemid);
            DropDownList ddlInventBatchId = (DropDownList)e.FindControl("ddlInventBatchId");
            if (ddlInventBatchId != null)
            {
                ddlInventBatchId.DataSource = dt;
                ddlInventBatchId.DataValueField = "InventBatchId";
                ddlInventBatchId.DataTextField = "InventBatchId";
                ddlInventBatchId.DataBind();

                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventBatchId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventBatchId.Items[0].Text)))
                {
                    ddlInventBatchId.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlInventBatchId.Items.Clear();

                    // Disable the dropdown
                    ddlInventBatchId.Enabled = false;

                    // Set grey background color
                    ddlInventBatchId.BackColor = System.Drawing.Color.LightGray;

                    // Add a default item to show it's empty
                    ddlInventBatchId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventBatchId.Visible;
                }
                else
                {
                    ddlInventBatchId.Visible = true;

                    // Enable the dropdown
                    ddlInventBatchId.Enabled = true;

                    // Reset to default background color
                    ddlInventBatchId.BackColor = System.Drawing.Color.White;

                    // Add empty item at the top for selection
                    ddlInventBatchId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventBatchId.CssClass += " filterable-dropdown";

                    return ddlInventBatchId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindWmsLocationId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrievewmsLocationId(itemid);
            DropDownList ddlWmsLocationId = (DropDownList)e.FindControl("ddlWMSLocationId");
            if (ddlWmsLocationId != null)
            {
                ddlWmsLocationId.DataSource = dt;
                ddlWmsLocationId.DataValueField = "WmsLocationId";
                ddlWmsLocationId.DataTextField = "WmsLocationId";
                ddlWmsLocationId.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlWmsLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsLocationId.Items[0].Text)))
                {
                    ddlWmsLocationId.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlWmsLocationId.Items.Clear();
                    // Disable the dropdown
                    ddlWmsLocationId.Enabled = false;
                    // Set grey background color
                    ddlWmsLocationId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlWmsLocationId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlWmsLocationId.Visible;
                }
                else
                {
                    ddlWmsLocationId.Visible = true;

                    // Enable the dropdown
                    ddlWmsLocationId.Enabled = true;
                    // Reset to default background color
                    ddlWmsLocationId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlWmsLocationId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlWmsLocationId.CssClass += " filterable-dropdown";

                    return ddlWmsLocationId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindInventSerialId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSerialId(itemid);
            DropDownList ddlInventSerialId = (DropDownList)e.FindControl("ddlInventSerialId");
            if (ddlInventSerialId != null)
            {
                ddlInventSerialId.DataSource = dt;
                ddlInventSerialId.DataValueField = "InventSerialId";
                ddlInventSerialId.DataTextField = "InventSerialId";
                ddlInventSerialId.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventSerialId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSerialId.Items[0].Text)))
                {
                    ddlInventSerialId.Visible = false;
                    // Clear the dropdown first to remove any empty items
                    ddlInventSerialId.Items.Clear();
                    // Disable the dropdown
                    ddlInventSerialId.Enabled = false;
                    // Set grey background color
                    ddlInventSerialId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlInventSerialId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventSerialId.Visible;
                }
                else
                {
                    ddlInventSerialId.Visible = true;
                    // Enable the dropdown
                    ddlInventSerialId.Enabled = true;
                    // Reset to default background color
                    ddlInventSerialId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlInventSerialId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventSerialId.CssClass += " filterable-dropdown";

                    return ddlInventSerialId.Visible;
                }
            }
            else
                return false;
        }

        //private bool BindInventLocationId(string itemid, GridViewRow e)
        //{
        //    PurchaseOrderLines purchaseorderlines = new PurchaseOrderLines();
        //    DataTable dt = purchaseorderlines.retrieveinventLocationId();
        //    DropDownList ddlInventLocationId = (DropDownList)e.FindControl("ddlInventLocationId");
        //    if (ddlInventLocationId != null)
        //    {
        //        ddlInventLocationId.DataSource = dt;
        //        ddlInventLocationId.DataValueField = "InventLocationId";
        //        ddlInventLocationId.DataTextField = "InventLocationId";
        //        ddlInventLocationId.DataBind();
        //        // Check if dropdown is empty and disable/style accordingly
        //        if (dt == null || dt.Rows.Count == 0 ||
        //            (ddlInventLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventLocationId.Items[0].Text)))
        //        {
        //            ddlInventLocationId.Visible = false;
        //            // Clear the dropdown first to remove any empty items
        //            ddlInventLocationId.Items.Clear();
        //            // Disable the dropdown
        //            ddlInventLocationId.Enabled = false;
        //            // Set grey background color
        //            ddlInventLocationId.BackColor = System.Drawing.Color.LightGray;
        //            // Add a default item to show it's empty
        //            ddlInventLocationId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

        //            return ddlInventLocationId.Visible;
        //        }
        //        else
        //        {
        //            ddlInventLocationId.Visible = true;
        //            // Enable the dropdown
        //            ddlInventLocationId.Enabled = true;
        //            // Reset to default background color
        //            ddlInventLocationId.BackColor = System.Drawing.Color.White;
        //            // Add empty item at the top for selection
        //            ddlInventLocationId.Items.Insert(0, new ListItem("", string.Empty));

        //            ddlInventLocationId.CssClass += " filterable-dropdown";

        //            return ddlInventLocationId.Visible;
        //        }
        //    }
        //    else
        //        return false;
        //}

        private bool BindConfigId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveconfigId(itemid);
            DropDownList_ProductCombination ddlCombinationLookup = (DropDownList_ProductCombination)e.FindControl("ProductLookupControl");
            DropDownList ddlConfigId = (DropDownList)e.FindControl("ddlConfigId");
            if (ddlConfigId != null)
            {
                ddlConfigId.DataSource = dt;
                ddlConfigId.DataValueField = "ConfigId";
                ddlConfigId.DataTextField = "ConfigId";
                ddlConfigId.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlConfigId.Items.Count == 1 && string.IsNullOrEmpty(ddlConfigId.Items[0].Text)))
                {
                    ddlConfigId.Visible = false;
                    // Clear the dropdown first to remove any empty items
                    ddlConfigId.Items.Clear();
                    // Disable the dropdown
                    ddlConfigId.Enabled = false;
                    // Set grey background color
                    ddlConfigId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlConfigId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    ddlCombinationLookup.Visible = false;

                    return ddlConfigId.Visible;
                }
                else
                {
                    ddlConfigId.Visible = true;

                    // Enable the dropdown
                    ddlConfigId.Enabled = true;
                    // Reset to default background color
                    ddlConfigId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlConfigId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlConfigId.CssClass += " filterable-dropdown";


                    ddlCombinationLookup.Visible = true;

                    ddlCombinationLookup.Load(itemid);

                    return ddlConfigId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindInventSizeId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSizeId(itemid);
            DropDownList ddlInventSizeId = (DropDownList)e.FindControl("ddlInventSizeId");
            if (ddlInventSizeId != null)
            {
                ddlInventSizeId.DataSource = dt;
                ddlInventSizeId.DataValueField = "InventSizeId";
                ddlInventSizeId.DataTextField = "InventSizeId";
                ddlInventSizeId.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventSizeId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSizeId.Items[0].Text)))
                {

                    ddlInventSizeId.Visible = false;
                    // Clear the dropdown first to remove any empty items
                    ddlInventSizeId.Items.Clear();
                    // Disable the dropdown
                    ddlInventSizeId.Enabled = false;
                    // Set grey background color
                    ddlInventSizeId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlInventSizeId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventSizeId.Visible;
                }
                else
                {
                    ddlInventSizeId.Visible = true;

                    // Enable the dropdown
                    ddlInventSizeId.Enabled = true;
                    // Reset to default background color
                    ddlInventSizeId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlInventSizeId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventSizeId.CssClass += " filterable-dropdown";

                    return ddlInventSizeId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindInventColorId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventColorId(itemid);
            DropDownList ddlInventColorId = (DropDownList)e.FindControl("ddlInventColorId");
            if (ddlInventColorId != null)
            {
                ddlInventColorId.DataSource = dt;
                ddlInventColorId.DataValueField = "InventColorId";
                ddlInventColorId.DataTextField = "InventColorId";
                ddlInventColorId.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventColorId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventColorId.Items[0].Text)))
                {
                    ddlInventColorId.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlInventColorId.Items.Clear();
                    // Disable the dropdown
                    ddlInventColorId.Enabled = false;
                    // Set grey background color
                    ddlInventColorId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlInventColorId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventColorId.Visible;
                }
                else
                {
                    ddlInventColorId.Visible = true;

                    // Enable the dropdown
                    ddlInventColorId.Enabled = true;
                    // Reset to default background color
                    ddlInventColorId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlInventColorId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventColorId.CssClass += " filterable-dropdown";

                    return ddlInventColorId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindInventStyleId(string itemid, GridViewRow e)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinvetstyleid(itemid);
            DropDownList ddlInventStyle = (DropDownList)e.FindControl("ddlInventStyleId");
            if (ddlInventStyle != null)
            {
                ddlInventStyle.DataSource = dt;
                ddlInventStyle.DataValueField = "InventStyle";
                ddlInventStyle.DataTextField = "InventStyle";
                ddlInventStyle.DataBind();
                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventStyle.Items.Count == 1 && string.IsNullOrEmpty(ddlInventStyle.Items[0].Text)))
                {
                    ddlInventStyle.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlInventStyle.Items.Clear();
                    // Disable the dropdown
                    ddlInventStyle.Enabled = false;
                    // Set grey background color
                    ddlInventStyle.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlInventStyle.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventStyle.Visible;
                }
                else
                {
                    ddlInventStyle.Visible = true;

                    // Enable the dropdown
                    ddlInventStyle.Enabled = true;
                    // Reset to default background color
                    ddlInventStyle.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlInventStyle.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventStyle.CssClass += " filterable-dropdown";

                    return ddlInventStyle.Visible;
                }
            }
            else
                return false;
        }


        private bool BindTransitWarehouse()
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveTransitWarehouse();

            // Correct way to find dropdown
            ddlTransitWarehouse.DataSource = dt;
            ddlTransitWarehouse.DataTextField = "InventLocationId";   // what user sees
            ddlTransitWarehouse.DataValueField = "Name";  // underlying value
            ddlTransitWarehouse.DataBind();

            // Optional: set selected value from first row
            ddlTransitWarehouse.SelectedValue = dt.Rows[0]["InventLocationId"].ToString();

            return true;
        }

        private bool BindWmsPalletId(string itemid, GridViewRow e)

        {

            TransferOrderLines lines = new TransferOrderLines();

            DataTable dt = lines.retrievewmsPalletId(itemid);

            DropDownList ddlWmsPalletId = (DropDownList)e.FindControl("ddlWMSPalletId");

            ddlWmsPalletId.DataSource = dt;

            ddlWmsPalletId.DataValueField = "WmsPalletId";

            ddlWmsPalletId.DataTextField = "WmsPalletId";

            ddlWmsPalletId.DataBind();

            // Check if dropdown is empty and disable/style accordingly

            if (dt == null || dt.Rows.Count == 0 ||

                (ddlWmsPalletId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsPalletId.Items[0].Text)))

            {

                ddlWmsPalletId.Visible = false;

                // Clear the dropdown first to remove any empty items

                ddlWmsPalletId.Items.Clear();

                // Disable the dropdown

                ddlWmsPalletId.Enabled = false;

                // Set grey background color

                ddlWmsPalletId.BackColor = System.Drawing.Color.LightGray;

                // Add a default item to show it's empty

                ddlWmsPalletId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                return ddlWmsPalletId.Visible;

            }

            else

            {

                ddlWmsPalletId.Visible = true;

                // Enable the dropdown

                ddlWmsPalletId.Enabled = true;

                // Reset to default background color

                ddlWmsPalletId.BackColor = System.Drawing.Color.White;

                // Add empty item at the top for selection

                ddlWmsPalletId.Items.Insert(0, new ListItem("", string.Empty));

                ddlWmsPalletId.CssClass += " filterable-dropdown";

                return ddlWmsPalletId.Visible;

            }

        }

        private bool BindCombination(string itemid, GridViewRow e)
        {

            DropDownList_ProductCombination ddlCombinationLookup = (DropDownList_ProductCombination)e.FindControl("ProductLookupControl");
            DropDownList ddlConfigId = (DropDownList)e.FindControl("ddlConfigId");
            DropDownList ddlInventSizeId = (DropDownList)e.FindControl("ddlInventSizeId");
            DropDownList ddlInventColorId = (DropDownList)e.FindControl("ddlInventColorId");
            DropDownList ddlInventStyle = (DropDownList)e.FindControl("ddlInventStyleId");
            if (ddlConfigId != null && ddlInventSizeId != null && ddlInventColorId != null && ddlInventStyle != null)
            {
                if (ddlConfigId.Visible || ddlInventSizeId.Visible || ddlInventColorId.Visible || ddlInventStyle.Visible)
                {
                    ddlCombinationLookup.Visible = true;

                    ddlCombinationLookup.Load(itemid);

                    return ddlCombinationLookup.Visible;
                }
                else
                {
                    ddlCombinationLookup.Visible = false;

                    return ddlCombinationLookup.Visible;
                }
            }
            else
                return false;
        }
        //private bool BindSiteId(string itemid, GridViewRow e)

        //{

        //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
        //    DataTable dt = service.retrievesiteId();

        //    DropDownList ddlInventSiteId = (DropDownList)e.FindControl("ddlInventSiteId");

        //    ddlInventSiteId.DataSource = dt;

        //    ddlInventSiteId.DataValueField = "InventSiteId";

        //    ddlInventSiteId.DataTextField = "InventSiteId";

        //    ddlInventSiteId.DataBind();

        //    // Check if dropdown is empty and disable/style accordingly

        //    if (dt == null || dt.Rows.Count == 0 ||

        //        (ddlInventSiteId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSiteId.Items[0].Text)))

        //    {

        //        ddlInventSiteId.Visible = false;

        //        // Clear the dropdown first to remove any empty items

        //        ddlInventSiteId.Items.Clear();

        //        // Disable the dropdown

        //        ddlInventSiteId.Enabled = false;

        //        // Set grey background color

        //        ddlInventSiteId.BackColor = System.Drawing.Color.LightGray;

        //        // Add a default item to show it's empty

        //        ddlInventSiteId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

        //        return ddlInventSiteId.Visible;

        //    }

        //    else

        //    {

        //        ddlInventSiteId.Visible = true;

        //        // Enable the dropdown

        //        ddlInventSiteId.Enabled = true;

        //        // Reset to default background color

        //        ddlInventSiteId.BackColor = System.Drawing.Color.White;

        //        // Add empty item at the top for selection

        //        ddlInventSiteId.Items.Insert(0, new ListItem("", string.Empty));

        //        ddlInventSiteId.CssClass += " filterable-dropdown";

        //        return ddlInventSiteId.Visible;

        //    }

        //}


        protected void btnNew_Grid_Click(object sender, EventArgs e)
        {
            DataTable dt = newTransferorderlines.retrieveAll(Session["TransferID"] as string); // Replace this with your actual data-fetching logic

            // 2. Add a new blank row at the top
            DataRow newRow = dt.NewRow();
            dt.Rows.InsertAt(newRow, 0);

            // 3. Put GridView in edit mode for the first row
            gridView.EditIndex = 0;

            // 5. Rebind the GridView
            gridView.DataSource = dt;
            gridView.DataBind();

            Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
        {
            { "Batch number",("ddlInventBatchId", false) },
            { "Location", ("ddlWMSLocationId", false) },
            { "Serial number", ("ddlInventSerialId", false) },
            { "Configuration", ("ddlConfigId", false) },
            { "Size", ("ddlInventSizeId", false) },
            { "Color", ("ddlInventColorId", false) },
            { "Style", ("ddlInventStyleId", false) },
            { "WMS pallet", ("ddlWMSPalletId", false) },
            { "Warehouse", ("ddlInventLocationId", false) },
            { "Site", ("ddlInventSiteId", false)}
        };

            foreach (DataControlField column in gridView.Columns)
            {
                if (column is TemplateField && column.HeaderText != null)
                {
                    string header = column.HeaderText;

                    if (columnVisibilities.ContainsKey(header))
                    {
                        columnVisibilities.TryGetValue(header, out var fieldMeta);
                        column.Visible = fieldMeta.IsVisible;
                    }
                }
            }

            btnDelete.Enabled = false;
            btnDelete.CssClass = "disabled-buttonLines";
            btnUploadExcel.Enabled = false;
            btnUploadExcel.CssClass = "disabled-button";
            btnShip_All.Enabled = false;
            btnShip_All.CssClass = "disabled-button";
            BtnAddLine.Enabled = false;
            BtnAddLine.CssClass = "disabled-buttonLines";
            btnRecieve.Enabled = false;
            btnRecieve.CssClass = "disabled-button";
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
        {
            { "Batch number",("ddlInventBatchId", true) },
            { "Location", ("ddlWMSLocationId", true) },
            { "Serial number", ("ddlInventSerialId", true) },
            { "Configuration", ("ddlConfigId", true) },
            { "Combinations", ("", false) },
            { "Size", ("ddlInventSizeId", true) },
            { "Color", ("ddlInventColorId", true) },
            { "Style", ("ddlInventStyleId", true) },
            { "WMS pallet", ("ddlWMSPalletId", false) },
            { "Warehouse", ("ddlInventLocationId", true) },
            { "Site", ("ddlInventSiteId", true)}
        };

            foreach (DataControlField column in gridView.Columns)
            {
                if (column is TemplateField && column.HeaderText != null)
                {
                    string header = column.HeaderText;

                    if (columnVisibilities.ContainsKey(header))
                    {
                        columnVisibilities.TryGetValue(header, out var fieldMeta);
                        column.Visible = fieldMeta.IsVisible;
                    }
                }
            }

            btnDelete.Enabled = true;
            btnDelete.CssClass = "";
            btnUploadExcel.Enabled = true;
            btnUploadExcel.CssClass = "";
            btnShip_All.Enabled = true;
            btnShip_All.CssClass = "";
            BtnAddLine.Enabled = true;
            BtnAddLine.CssClass = "";
            btnRecieve.Enabled = true;
            btnRecieve.CssClass = "";

            gridView.EditIndex = -1;
            reBindGrid();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            bool allGood = true;
            GridViewRow gridRow = gridView.Rows[gridView.EditIndex];
            // Prepare data
            DataTable dt = new DataTable();
            dt.Columns.Add("Itemid");
            dt.Columns.Add("TransferId");
            dt.Columns.Add("LinesShipDate");
            dt.Columns.Add("LinesReceiveDate");
            dt.Columns.Add("ReserveItem");
            dt.Columns.Add("IsCatchWeight");
            dt.Columns.Add("QtyTransfer");
            dt.Columns.Add("QtyShipped");


            dt.Columns.Add("InventBatchId");
            dt.Columns.Add("WmsLocationId");
            dt.Columns.Add("WmsPalletId");
            dt.Columns.Add("InventSerialId");
            dt.Columns.Add("InventLocationId");
            dt.Columns.Add("ConfigId");
            dt.Columns.Add("InventSizeId");
            dt.Columns.Add("InventColorId");
            dt.Columns.Add("InventSiteId");
            dt.Columns.Add("InventStyle");

            DataRow row = dt.NewRow();

            DropDownList ddlItemId = (DropDownList)gridRow.FindControl("ddlItemId");
            //TextBox txtshipLineDate = (TextBox)gridRow.FindControl("txtshipLineDate");
            //TextBox txtreceiveLineDate = (TextBox)gridRow.FindControl("txtreceiveLineDate");
            TextBox txtQtyTransfer = (TextBox)gridRow.FindControl("txtQtyTransfer");

            DropDownList ddlConfigId = (DropDownList)gridRow.FindControl("ddlConfigId");
            DropDownList ddlColorId = (DropDownList)gridRow.FindControl("ddlInventColorId");
            DropDownList ddlSizeId = (DropDownList)gridRow.FindControl("ddlInventSizeId");
            DropDownList ddlStyleId = (DropDownList)gridRow.FindControl("ddlInventStyleId");
            DropDownList ddlInventSiteId = (DropDownList)gridRow.FindControl("ddlInventSiteId");
            DropDownList ddlInventLocationId = (DropDownList)gridRow.FindControl("ddlInventLocationId");
            DropDownList ddlInventBatchId = (DropDownList)gridRow.FindControl("ddlInventBatchId");
            DropDownList ddlWMSLocationId = (DropDownList)gridRow.FindControl("ddlWMSLocationId");
            DropDownList ddlWmsPalletId = (DropDownList)gridRow.FindControl("ddlWMSPalletId");
            DropDownList ddlInventSerialId = (DropDownList)gridRow.FindControl("ddlInventSerialId");

            // Add form field values to DataRow
            row["Itemid"] = ddlItemId.SelectedValue;
            row["TransferId"] = Session["TransferID"] as string;
            //row["LinesShipDate"] = txtshipLineDate.Text;
            //row["LinesReceiveDate"] = txtreceiveLineDate.Text;
            row["QtyTransfer"] = txtQtyTransfer.Text;
            //row["ReserveItem"] = ddlReserveItem.SelectedValue;
            row["IsCatchWeight"] = false;

            row["InventBatchId"] = ddlInventBatchId.SelectedValue;
            row["WmsLocationId"] = ddlWMSLocationId.SelectedValue;
            row["WmsPalletId"] = ddlWmsPalletId.SelectedValue;
            row["InventSerialId"] = ddlInventSerialId.SelectedValue;
            //row["InventLocationId"] = ddlInventLocationId.SelectedValue;
            row["ConfigId"] = ddlConfigId.SelectedValue;
            row["InventSizeId"] = ddlSizeId.SelectedValue;
            row["InventColorId"] = ddlColorId.SelectedValue;
            //row["InventSiteId"] = ddlInventSiteId.SelectedValue;
            row["InventStyle"] = ddlStyleId.SelectedValue;

            dt.Rows.Add(row);

            Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
        {
            { "Batch number",("ddlInventBatchId", BindInventBatchId(ddlItemId.SelectedValue, gridRow) )},
            { "Location", ("ddlWMSLocationId", BindWmsLocationId(ddlItemId.SelectedValue, gridRow)) },
            { "Serial number", ("ddlInventSerialId", BindInventSerialId(ddlItemId.SelectedValue, gridRow)) },
            { "Configuration", ("ddlConfigId", BindConfigId(ddlItemId.SelectedValue, gridRow)) },
            { "Combinations", ("", BindCombination(ddlItemId.SelectedValue, gridRow)) },
            { "Size", ("ddlInventSizeId", BindInventSizeId(ddlItemId.SelectedValue, gridRow)) },
            { "Color", ("ddlInventColorId", BindInventColorId(ddlItemId.SelectedValue, gridRow)) },
            { "Style", ("ddlInventStyleId", BindInventStyleId(ddlItemId.SelectedValue, gridRow)) },
            { "WMS pallet", ("ddlWMSPalletId", BindWmsPalletId(ddlItemId.SelectedValue, gridRow)) },
            //{ "Warehouse", ("ddlInventLocationId", BindInventLocationId(ddlItemId.SelectedValue, gridRow)) },
            //{ "Site", ("ddlInventSiteId", BindSiteId(ddlItemId.SelectedValue, gridRow)) }
        };

            ddlInventBatchId.SelectedValue = row["InventBatchId"]?.ToString();
            ddlWMSLocationId.SelectedValue = row["WmsLocationId"]?.ToString();
            ddlWmsPalletId.SelectedValue = row["WmsPalletId"]?.ToString();
            ddlInventSerialId.SelectedValue = row["InventSerialId"]?.ToString();
            ddlConfigId.SelectedValue = row["ConfigId"]?.ToString();
            ddlSizeId.SelectedValue = row["InventSizeId"]?.ToString();
            ddlColorId.SelectedValue = row["InventColorId"]?.ToString();
            ddlStyleId.SelectedValue = row["InventStyle"]?.ToString();
            //ddlInventSiteId.SelectedValue = row["InventSiteId"].ToString();
            //ddlInventLocationId.SelectedValue = row["InventLocationId"].ToString();

            //if (string.IsNullOrEmpty(ddlItemId.SelectedValue) || string.IsNullOrEmpty(txtQtyTransfer.Text))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingVisibleFields", $"alert('Please Fill all the Fields');", true);
            //    return;
            //}
            //foreach (DataControlField column in gridView.Columns)
            //{
            //    if (column is TemplateField && column.HeaderText != null)
            //    {
            //        string header = column.HeaderText;

            //        if (columnVisibilities.TryGetValue(header, out var fieldMeta) && fieldMeta.IsVisible)
            //        {
            //            DropDownList validateField = (DropDownList)gridRow.FindControl(fieldMeta.FieldId);
            //            if (validateField != null && string.IsNullOrEmpty(validateField.SelectedValue))
            //            {
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingVisibleFields", $"alert('Please Fill all the Fields');", true);
            //                allGood = false;
            //                return;
            //            }
            //        }
            //    }
            //}

            if (string.IsNullOrEmpty(ddlItemId.SelectedValue) || string.IsNullOrEmpty(txtQtyTransfer.Text))
            {
                List<string> missingFields = new List<string>();

                if (string.IsNullOrEmpty(ddlItemId.SelectedValue))
                    missingFields.Add("Item");

                if (string.IsNullOrEmpty(txtQtyTransfer.Text))
                    missingFields.Add("Quantity");

                if (missingFields.Count > 0)
                {
                    string msg = "Please fill the following fields: " + string.Join("\n ", missingFields);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingFields", $"alert('{msg}');", true);
                    return;
                }
            }

            List<string> gridMissingFields = new List<string>();

            foreach (DataControlField column in gridView.Columns)
            {
                if (column is TemplateField && column.HeaderText != null)
                {
                    string header = column.HeaderText;

                    if (columnVisibilities.TryGetValue(header, out var fieldMeta) && fieldMeta.IsVisible)
                    {
                        DropDownList validateField = (DropDownList)gridRow.FindControl(fieldMeta.FieldId);
                        if (validateField != null && string.IsNullOrEmpty(validateField.SelectedValue))
                        {
                            gridMissingFields.Add(header); // store header name
                        }
                    }
                }
            }

            if (gridMissingFields.Count > 0)
            {
                string msg = "Please fill the following fields: " + string.Join("-", gridMissingFields);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingGridFields", $"alert('{msg}');", true);
                allGood = false;
                return;
            }

            if (string.IsNullOrEmpty(row["InventLocationId"].ToString()))
            {
                string fromWareHouse = Session["Fromwarehouse"] as string;
                row["InventLocationId"] = fromWareHouse;
            }

            if (allGood)
            {
                TransferOrderLines transferLines = new TransferOrderLines();
                SysOperationResult_BOL result = transferLines.create(dt);
                NotificationMessage.showMessage(result);
                if (result != null && result.isSuccess)
                {
                    Cancel_Click(sender, e); //called to revert the grid back to normal
                }
            }
        }

        protected void btnShip_Click(object sender, EventArgs e)
        {
            string transferId = Session["TransferID"] as string;

            string script = $"openPopupPanel('/ESS/PR/TransferOrder_Shipment.aspx?transferIds={transferId}', 1000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);
        }


        protected void btnRecieve_Click(object sender, EventArgs e)
        {
            string transferId = Session["TransferID"] as string;

            string script = $"openPopupPanel('/ESS/PR/TransferOrder_Recieve.aspx?transferIds={transferId}', 1000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);
        }


        protected void btnRecieve_All_Click(object sender, EventArgs e) //Addition By Hamza
        {
            TransferOrderHeader transfer = new TransferOrderHeader();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();
            bool selectedRecords = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {

                selectedRecords = true;
                string transferId = "";
                transferId = Session["TransferID"] as string;
                if (!string.IsNullOrEmpty(transferId))
                {
                    operationResult_BOL = transfer.recieveTransferOrder(transferId);
                }

            }
            if (selectedRecords)
            {
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select at least one transfer to proceed.');", true);
            }
        }

        protected void BindHeader()
        {
            string transferId = Request.QueryString["TransferID"] ?? (string)Session["TransferID"];
            string fromwarehouse = Request.QueryString["FromWarehouse"] ?? (string)Session["FromWarehouse"];
            string towarehouse = Request.QueryString["ToWarehouse"] ?? (string)Session["ToWarehouse"];



            if (Session["TransferID"] != null)
            {
                //lblTransferID.Text = Session["TransferID"].ToString();
            }

            string fromWarehouse = Session["FromWarehouse"] as string;

            if (Session["FromWarehouse"] != null)
            {
                txtfromwarehousegeneraltab.Text = fromwarehouse;
            }
            string toWarehouse = Session["ToWarehouse"] as string;

            //txtTransitWarehouse.Text = Session["TransitWarhouse"] as string;

            if (Session["ToWarehouse"] != null)
            {
                txttowarehousegeneraltab.Text = toWarehouse;
            }

            string status = Session["Status"] as string;

            if (Session["Status"] != null)
            {
                txttransferstatusforheader.Text = status;
            }
            if (Session["ShipDate"] != null)
            {
                DateTime shipDate;
                if (DateTime.TryParse(Session["ShipDate"].ToString(), out shipDate))
                {
                    // For example, set it to a label or use it as needed
                    txtshipdateheader.Text = shipDate.ToString("M-d-yyyy"); // Format as needed
                }
                
            }
            if (Session["ReceiveDate"] != null)
            {
                DateTime receiveDate;
                if (DateTime.TryParse(Session["ReceiveDate"].ToString(), out receiveDate))
                {
                    txtreceiptdateheader.Text = receiveDate.ToString("M-d-yyyy"); // or use your desired format
                }
                
            }





        }
        protected void BindFromwareHouse()
        {
           

            string transferId = Request.QueryString["TransferID"] ?? (string)Session["TransferID"];
            string fromwarehouse = Request.QueryString["FromWarehouse"] ?? (string)Session["FromWarehouse"];
            string fromWarehouseName = Session["FromWarehouseName"] as string;
            string fromAddress = Session["FromAddress"] as string;
            string towarehouse = Request.QueryString["ToWarehouse"] ?? (string)Session["ToWarehouse"];

                string fromWarehouse = Session["FromWarehouse"] as string;

                if (Session["FromWarehouse"] != null)
                {
                    txtfromwarehouseheader.Text = fromwarehouse;

                }

            txtAddressName.Text = fromWarehouseName;
            txtWarehouseAddress.Text = fromWarehouseName;
            txtfromwarehouseaddressdetail.Text = fromAddress;


        }

        protected void BindTowareHouse()
        {
            
            string transferId = Request.QueryString["TransferID"] ?? (string)Session["TransferID"];
            string toWarehouseName = Session["ToWarehouseName"] as string;
            string toAddress = Session["ToAddress"] as string;


           string toWarehouse = Session["ToWarehouse"] as string;

                if (Session["ToWarehouse"] != null)
                {
                   txttowarehouseheader.Text = toWarehouse;
                }

            txttowarehouseheadername.Text = toWarehouseName;
            txttowarehouseaddress.Text = toWarehouseName;
            txttowarehouseaddressdetail.Text = toAddress;



        }

        protected void BindDelivery()
        {

            if (Session["ShipDate"] != null)
            {
                DateTime shipDate;
                if (DateTime.TryParse(Session["ShipDate"].ToString(), out shipDate))
                {
                    // For example, set it to a label or use it as needed
                    txtshipdateheader.Text = shipDate.ToString("M-d-yyyy"); // Format as needed
                }

            }
            if (Session["ReceiveDate"] != null)
            {
                DateTime receiveDate;
                if (DateTime.TryParse(Session["ReceiveDate"].ToString(), out receiveDate))
                {
                    txtreceiptdateheader.Text = receiveDate.ToString("M-d-yyyy"); // or use your desired format
                }
               
            }


        }
        protected void BindLinesHeader()
        {
            string transferId = Request.QueryString["TransferID"] ?? (string)Session["TransferID"];
            string fromwarehouse = Request.QueryString["FromWarehouse"] ?? (string)Session["FromWarehouse"];
            string towarehouse = Request.QueryString["ToWarehouse"] ?? (string)Session["ToWarehouse"];

            if (Session["CreatedDateAndTime"] != null)
            {
                DateTime createdDateTime;
                if (DateTime.TryParse(Session["CreatedDateAndTime"].ToString(), out createdDateTime))
                {
                    // Format as M-d-yyyy h:mm tt (e.g., 9-10-2025 3:45 PM)
                    txtCreatedDateTime.Text = createdDateTime.ToString("M-d-yyyy h:mm tt");
                }
            }


            if (Session["TransferID"] != null)
            {
                txtTransferNumber.Text = Session["TransferID"].ToString();
            }

            string fromWarehouse = Session["FromWarehouse"] as string;

            if (Session["FromWarehouse"] != null)
            {
                txtFromWarehouse.Text = fromwarehouse;
            }
            string toWarehouse = Session["ToWarehouse"] as string;

            if (Session["ToWarehouse"] != null)
            {
                txtToWarehouse.Text = toWarehouse;
            }

            string status = Session["Status"] as string;

            if (Session["Status"] != null)
            {
                txtTransferStatus.Text = status;
            }
            if (Session["ShipDate"] != null)
            {
                DateTime shipDate;
                if (DateTime.TryParse(Session["ShipDate"].ToString(), out shipDate))
                {
                    // For example, set it to a label or use it as needed
                    txtShipDate.Text = shipDate.ToString("M-d-yyyy"); // Format as needed
                }
            }
            if (Session["ReceiveDate"] != null)
            {
                DateTime receiveDate;
                if (DateTime.TryParse(Session["ReceiveDate"].ToString(), out receiveDate))
                {
                    txtReceiptDate.Text = receiveDate.ToString("M-d-yyyy"); // or use your desired format
                }
            }
        }
        public void setFieldsEmpty()
        {
            string empty = "\uFEFF";

            shipnowLabel.Text = empty;

            receivenoelabel.Text = empty;

            // Identification / Status / Inventory
            LineDetailTransferNumber.Text = empty;
            txtLineNumber.Text = empty;
            txtRemaining.Text = empty;
            txtShipmentLotID.Text = empty;
            txtTransitShipmentLotID.Text = empty;
            txtTransitReceiveLotID.Text = empty;
            txtReceiveLotID.Text = empty;
            txtScrapLotID.Text = empty;
            txtDimensionNumber.Text = empty;

            // Voyages / Arrival / Shipping
            txtVoyage.Text = empty;
            txtVoyageStatus.Text = empty;
            txtArrivalGroup.Text = empty;
            txtShippingContainer.Text = empty;


            shipitemnumber.Text = empty;
            shiptransferquantity.Text = empty;
            shipnow.Text = empty;
            shipshippedquantity.Text = empty;
            shipremaining.Text = empty;
            receiveitemnumber.Text = empty;
            receivequantity.Text = empty;
            receivenow.Text = empty;
            receivereceivedquantity.Text = empty;
            receiveremaining.Text = empty;


            // Inventory Dimensions
            SetDropDownEmpty(ddlConfigurationLineDetail);
            SetDropDownEmpty(ddlColorLineDetail);
            SetDropDownEmpty(ddlSizeLineDetail);
            SetDropDownEmpty(ddlStyleLineDetail);
            txtVersion.Text = empty;
            txtSiteLineDetail.Text = empty;
            txtWarehouseLineDetail.Text = empty;
            SetDropDownEmpty(ddlBatchLineDetail);
            SetDropDownEmpty(ddlWmsLocationLineDetail);
            SetDropDownEmpty(ddlInventSerialLineDetail);
            txtInventoryStatus.Text = empty;
            txtLicensePlate.Text = empty;
            txtOwner.Text = empty;


        }

        private void SetDropDownEmpty(DropDownList ddl)
        {
            if (ddl == null) return;

            // If there's a blank item ("") already in the list, select it.
            if (ddl.Items.FindByValue(string.Empty) != null)
            {
                ddl.SelectedValue = string.Empty;
            }
            else if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = 0;
            }
        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        protected void ProductLookupControl_ProductSelected(object sender, DataRow selectedRow)
        {
            string configId = selectedRow["ConfigId"].ToString();
            string style = selectedRow["InventStyle"].ToString();
            string color = selectedRow["InventColorId"].ToString();
            string size = selectedRow["InventSizeId"].ToString();
            string inventDimId = selectedRow["InventDimId"].ToString();

            DropDownList_ProductCombination ddlProductCombination = (DropDownList_ProductCombination)sender;

            GridViewRow gridRow = (GridViewRow)ddlProductCombination.NamingContainer;

            DropDownList ddlConfigId = (DropDownList)gridRow.FindControl("ddlConfigId");
            if (ddlConfigId != null)
            {
                ddlConfigId.SelectedValue = configId;
            }

            // Style
            DropDownList ddlInventStyleId = (DropDownList)gridRow.FindControl("ddlInventStyleId");
            if (ddlInventStyleId != null && ddlInventStyleId.Items.FindByValue(style) != null)
            {
                ddlInventStyleId.SelectedValue = style;
            }

            // Color
            DropDownList ddlInventColorId = (DropDownList)gridRow.FindControl("ddlInventColorId");
            if (ddlInventColorId != null && ddlInventColorId.Items.FindByValue(color) != null)
            {
                ddlInventColorId.SelectedValue = color;
            }

            // Size
            DropDownList ddlInventSizeId = (DropDownList)gridRow.FindControl("ddlInventSizeId");
            if (ddlInventSizeId != null && ddlInventSizeId.Items.FindByValue(size) != null)
            {
                ddlInventSizeId.SelectedValue = size;
            }
        }


        protected void btnOnHand_Click(object sender, EventArgs e)
        {
            bool found = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {

                    found = true;


                    // Core IDs
                    string recId = (row.FindControl("lblRecId") as Label)?.Text.Trim();

                    if (!string.IsNullOrEmpty(recId))
                    {
                        Session["RecId"] = recId;  // ✅ Store RecId in session
                    }

                    string inventDimId = (row.FindControl("lblInventDimId") as Label)?.Text.Trim();
                    string itemId = (row.FindControl("lblItemID") as Label)?.Text.Trim();
                    string ItemName = (row.FindControl("lblItemName") as Label)?.Text.Trim();

                    // Inventory Dimensions
                    string configId = (row.FindControl("lblConfigId") as Label)?.Text.Trim();
                    string colorId = (row.FindControl("lblInventColorId") as Label)?.Text.Trim();
                    string sizeId = (row.FindControl("lblInventSizeId") as Label)?.Text.Trim();
                    string styleId = (row.FindControl("lblInventStyleId") as Label)?.Text.Trim();
                    string siteId = (row.FindControl("lblInventSiteId") as Label)?.Text.Trim();
                    string warehouse = (row.FindControl("lblInventLocationId") as Label)?.Text.Trim();
                    string batchNum = (row.FindControl("lblInventBatchId") as Label)?.Text.Trim();
                    string wmsLocation = (row.FindControl("lblWMSLocationId") as Label)?.Text.Trim();
                    string wmsPallet = (row.FindControl("lblWMSPalletId") as Label)?.Text.Trim();
                    string serialNum = (row.FindControl("lblInventSerialId") as Label)?.Text.Trim();
                    string transCode = (row.FindControl("lblTransctionCode") as Label)?.Text.Trim();
                    string unitId = (row.FindControl("lblUnitId") as Label)?.Text.Trim();

                    // Store in Session
                    Session["RecId"] = recId;
                    Session["InventDimId"] = inventDimId;
                    Session["ItemId"] = itemId;
                    Session["ItemName"] = ItemName;
                    Session["ConfigId"] = configId;
                    Session["ColorId"] = colorId;
                    Session["SizeId"] = sizeId;
                    Session["StyleId"] = styleId;
                    Session["SiteId"] = siteId;
                    Session["Warehouse"] = warehouse;
                    Session["BatchNum"] = batchNum;
                    Session["WMSLocationId"] = wmsLocation;
                    Session["WMSPalletId"] = wmsPallet;
                    Session["SerialNum"] = serialNum;
                    Session["TransactionCode"] = transCode;
                    Session["UnitId"] = unitId;

                    string script = "openPopupPanel('/ESS/PR/TransferOrder_OnHand.aspx', 1200);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupOnHand", script, true);
                    break;
                }
            }

            if (!found)
            {
                SysOperationResult_BOL error = new SysOperationResult_BOL();
                error.AlertType = AlertType.Error.ToString();
                error.Message = "Please select a transfer line to view On-hand.";
                error.isSuccess = false;
                NotificationMessage.showMessage(error);
            }
        }

        protected void btnViewReport(object sender, EventArgs e)
        {
            string transferId = Session["TransferID"] as string;

            if (!string.IsNullOrEmpty(transferId))
            {
                Response.Redirect("/ESS/PR/TransferOrderOverViewReport.aspx");
            }
        }
        protected void btnViewReport_Onhand(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    string itemId = (row.FindControl("lblItemID") as Label)?.Text.Trim();
                    Session["ItemId"] = itemId;

                    isSelected = true;
                    Response.Redirect("/ESS/PR/TransferOnHandItemsInventoryReport.aspx");
                    return; // stop loop after redirect
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('No Line Record was selected');", true);
            }
        }

        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {

                // Create DataTable for Inventory Dimensions
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemId");
                dt.Columns.Add("RecId", typeof(long));
                dt.Columns.Add("Configuration");
                dt.Columns.Add("Color");
                dt.Columns.Add("Size");
                dt.Columns.Add("Style");
                //dt.Columns.Add("Site");
                //dt.Columns.Add("Warehouse");
                dt.Columns.Add("BatchNumber");
                dt.Columns.Add("WmsLocation");
                dt.Columns.Add("SerialNumber");

                foreach (GridViewRow gridRow in gridView.Rows)
                {
                    // Find controls inside the current row
                    Label lblItemId = (Label)gridRow.FindControl("lblItemID");
                    Label lblRecId = (Label)gridRow.FindControl("lblRecId");

                    string itemId = lblItemId != null ? lblItemId.Text.Trim() : string.Empty;
                    long recId = 0;
                    if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
                        long.TryParse(lblRecId.Text, out recId);

                    // Get dimension values (these might be shared dropdowns outside grid)
                    string configuration = ddlConfigurationLineDetail.SelectedValue;
                    string color = ddlColorLineDetail.SelectedValue;
                    string size = ddlSizeLineDetail.SelectedValue;
                    string style = ddlStyleLineDetail.SelectedValue;
                    //string site = ddlSiteLineDetail.SelectedValue;
                    //string warehouse = ddlWarehouseLineDetail.SelectedValue;
                    string batchNumber = ddlBatchLineDetail.SelectedValue;
                    string wmsLocation = ddlWmsLocationLineDetail.SelectedValue;
                    string serialNumber = ddlInventSerialLineDetail.SelectedValue;

                    // Create a new row for each GridView row
                    DataRow row = dt.NewRow();
                    row["ItemId"] = itemId;
                    row["RecId"] = recId;
                    row["Configuration"] = configuration;
                    row["Color"] = color;
                    row["Size"] = size;
                    row["Style"] = style;
                    //row["Site"] = site;
                    //row["Warehouse"] = warehouse;
                    row["BatchNumber"] = batchNumber;
                    row["WmsLocation"] = wmsLocation;
                    row["SerialNumber"] = serialNumber;

                    dt.Rows.Add(row);

                    SysOperationResult_BOL results = newTransferorderlines.UpdateInventDimRecord(dt);

                    if (results != null && results.isSuccess)
                    {
                        NotificationMessage.showMessage(results);
                    }
                    else
                    {
                        NotificationMessage.showMessage(results);
                    }


                }


            }
            catch (Exception)
            {

            }
        }

        private void BindConfigurationforLineDetail(string itemId)
        {
            TransferOrderLines newlines = new TransferOrderLines();
            DataTable dt = newlines.retrieveconfigId(itemId);

            ddlConfigurationLineDetail.DataSource = dt;
            ddlConfigurationLineDetail.DataTextField = "ConfigId";
            ddlConfigurationLineDetail.DataValueField = "ConfigId";
            ddlConfigurationLineDetail.DataBind();

            // Always insert blank option at top
            ddlConfigurationLineDetail.Items.Insert(0, new ListItem("", ""));

            // Reset selection (prevents SelectedValue invalid errors)
            ddlConfigurationLineDetail.SelectedIndex = 0;

            // Disable if empty
            ddlConfigurationLineDetail.Enabled = dt != null && dt.Rows.Count > 0;
        }





        private void BindColorLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventColorId(itemid);

            if (ddlColorLineDetail != null)
            {
                // If no rows, just clear and show "No Selection Available"
                if (dt == null || dt.Rows.Count == 0 || (ddlColorLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlColorLineDetail.Items[0].Text)))
                {
                    ddlColorLineDetail.Items.Clear();
                    ddlColorLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlColorLineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlColorLineDetail.DataSource = dt;
                ddlColorLineDetail.DataValueField = "InventColorId";
                ddlColorLineDetail.DataTextField = "InventColorId";
                ddlColorLineDetail.DataBind();

                ddlColorLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlColorLineDetail.Enabled = true;
            }
        }

        private void BindSizeLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSizeId(itemid);

            if (ddlSizeLineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0 || (ddlSizeLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlSizeLineDetail.Items[0].Text)))
                {
                    ddlSizeLineDetail.Items.Clear();
                    ddlSizeLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlSizeLineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlSizeLineDetail.DataSource = dt;
                ddlSizeLineDetail.DataValueField = "InventSizeId";
                ddlSizeLineDetail.DataTextField = "InventSizeId";
                ddlSizeLineDetail.DataBind();

                ddlSizeLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlSizeLineDetail.Enabled = true;
            }
        }

        private void BindStyleLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinvetstyleid(itemid);

            if (ddlStyleLineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0 || (ddlStyleLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlStyleLineDetail.Items[0].Text)))
                {
                    ddlStyleLineDetail.Items.Clear();
                    ddlStyleLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlStyleLineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlStyleLineDetail.DataSource = dt;
                ddlStyleLineDetail.DataValueField = "InventStyle";
                ddlStyleLineDetail.DataTextField = "InventStyle";
                ddlStyleLineDetail.DataBind();

                ddlStyleLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlStyleLineDetail.Enabled = true;
            }
        }

       


        private void BindBatchLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventBatchId(itemid);

            if (ddlBatchLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0 || (ddlBatchLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlBatchLineDetail.Items[0].Text)))
                {
                    ddlBatchLineDetail.Items.Clear();
                    ddlBatchLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlBatchLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlBatchLineDetail.DataSource = dt;
                ddlBatchLineDetail.DataValueField = "InventBatchId";
                ddlBatchLineDetail.DataTextField = "InventBatchId";
                ddlBatchLineDetail.DataBind();

                ddlBatchLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlBatchLineDetail.Enabled = true;
            }
        }

        private void BindWmsLocationLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrievewmsLocationId(itemid);

            if (ddlWmsLocationLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0 || (ddlWmsLocationLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsLocationLineDetail.Items[0].Text)))
                {
                    ddlWmsLocationLineDetail.Items.Clear();
                    ddlWmsLocationLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlWmsLocationLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlWmsLocationLineDetail.DataSource = dt;
                ddlWmsLocationLineDetail.DataValueField = "WmsLocationId";
                ddlWmsLocationLineDetail.DataTextField = "WmsLocationId";
                ddlWmsLocationLineDetail.DataBind();

                ddlWmsLocationLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlWmsLocationLineDetail.Enabled = true;
            }
        }

        private void BindInventSerialLineDetail(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSerialId(itemid);

            if (ddlInventSerialLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0 || (ddlInventSerialLineDetail.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSerialLineDetail.Items[0].Text)))
                {
                    ddlInventSerialLineDetail.Items.Clear();
                    ddlInventSerialLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventSerialLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlInventSerialLineDetail.DataSource = dt;
                ddlInventSerialLineDetail.DataValueField = "InventSerialId";
                ddlInventSerialLineDetail.DataTextField = "InventSerialId";
                ddlInventSerialLineDetail.DataBind();

                ddlInventSerialLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlInventSerialLineDetail.Enabled = true;
            }
        }

        protected void btnRemainder_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // ✅ Get RecId from DataKey (recommended)
                    string recId = gridView.DataKeys[row.RowIndex]["RecId"].ToString();
                    Session["RecId"] = recId; // store in session for the modal form
                    Session["TransferID"] = (row.FindControl("lblTransferID") as Label)?.Text.Trim();

                    isSelected = true;

                    // Open the Deliver Remainder form
                    string script = "openPopupPanel('/ESS/PR/DeliverRemainder.aspx', 700);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupRemainder", script, true);

                    return; // stop loop after selecting one row
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('No record was selected');", true);
            }
        }

    }
}

    