using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Shapes;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferJournalLines_ListPage : MainForm
    {
        private PurchaseOrderLines purchaselines = new PurchaseOrderLines();
        private TransferJournalLines transferJournallines = new TransferJournalLines();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferJournalLines_ListPage";
                if (!IsPostBack)
                {
                    Page.Title = "Transfer";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Transfer";
                        titleDiv.Style["font-weight"] = "bold";
                    }
                    string journalId = Session["JournalID"] as string;
                    string journalDescription = Session["JournalDescription"] as string;

                    lblJournalID.Text = journalId;
                    lblJournalDescription.Text = journalDescription;
                    genJournal.Text = journalId;

                    // Add "Transfer order:" before ID
                    lblJournalID.Text = journalId + " :";

                    // Apply styles (bold + 20px)
                    lblJournalID.Style["font-weight"] = "bold";
                    lblJournalID.Style["font-size"] = "20px";
                    lblJournalDescription.Style["font-weight"] = "bold";
                    lblJournalDescription.Style["font-size"] = "20px";

                    BindLinesHeader();
                    reBindGrid();

                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally { }
        }
        private string GetLabelText(Control row, string labelId)
        {
            var label = row.FindControl(labelId) as Label;
            var text = label?.Text?.Trim();
            return string.IsNullOrEmpty(text) ? "\uFEFF" : text;

        }
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool anyChecked = false;
                GridViewRow selectedRow = null;

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;

                    if (chkSelectRow != null && chkSelectRow != sender)
                    {
                        chkSelectRow.Checked = false; // uncheck others
                    }
                    else if (chkSelectRow != null && chkSelectRow == sender && chkSelectRow.Checked)
                    {
                        anyChecked = true;
                        selectedRow = row;
                    }
                }

                if (anyChecked && selectedRow != null)
                {
                    //General 
                    genLineNumber.Text = GetLabelText(selectedRow, "lblLineNum");
                    //genDate.Text       = GetLabelText(selectedRow, "txtReceiptDate");
                    genVoucher.Text = GetLabelText(selectedRow, "lblVoucher");
                    genCWQty.Text = GetLabelText(selectedRow, "txtCWQuantity");
                    genCWUnit.Text = GetLabelText(selectedRow, "txtCWUnit");
                    genUnityQty.Text = GetLabelText(selectedRow, "txtUnitQuantity");
                    genUnit.Text = GetLabelText(selectedRow, "lblUnit");
                    genQuantity.Text = GetLabelText(selectedRow, "txtQuantity");
                    genCostPrice.Text = GetLabelText(selectedRow, "lblCostPrice");
                    genPriceQty.Text = GetLabelText(selectedRow, "");
                    genChargesOnCost.Text = GetLabelText(selectedRow, "");
                    genCostAmount.Text = GetLabelText(selectedRow, "lblCostAmount");
                    genLotID.Text = GetLabelText(selectedRow, "");
                    genReceiveLotID.Text = GetLabelText(selectedRow, "");
                    genToDimensionNo.Text = GetLabelText(selectedRow, "");

                    string dateText = GetLabelText(selectedRow, "txtReceiptDate");

                    if (DateTime.TryParse(dateText, out DateTime parsedDate))
                    {
                        genDate.Text = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        genDate.Text = string.Empty;
                    }


                    //Inventory Dimensions
                    txtFromConfig.Text = GetLabelText(selectedRow, "lblFromInventConfigId");
                    txtFromSize.Text = GetLabelText(selectedRow, "lblSize");
                    txtFromColor.Text = GetLabelText(selectedRow, "lblFromColor");
                    txtFromStyle.Text = GetLabelText(selectedRow, "lblFromStyle");
                    txtFromVersion.Text = GetLabelText(selectedRow, "lblFromVersion");
                    txtFromSite.Text = GetLabelText(selectedRow, "lblFromInventSiteId");
                    ddlFromWarehouse.Text = GetLabelText(selectedRow, "lblFromWarehouse");
                    txtFromBatchNumber.Text = GetLabelText(selectedRow, "lblFromBatchNumber");
                    txtFromLocation.Text = GetLabelText(selectedRow, "lblFromLocationId");
                    txtFromSerialNumber.Text = GetLabelText(selectedRow, "lblFromSerialNumber");
                    txtFromInventoryStatus.Text = GetLabelText(selectedRow, "lblFromInventoryStatus");
                    txtFromLicensePlate.Text = GetLabelText(selectedRow, "lblFromLicensePlate");
                    txtFromOwner.Text = GetLabelText(selectedRow, "lblFromOwner");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                //ClearLinesDetails();
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('An error occurred: {ex.Message}');", true);
            }
        }
        protected void btnNew_Grid_Click(object sender, EventArgs e)
        {
            DataTable dt = transferJournallines.retrieveAll(Session["JournalID"] as string);
            DataRow newRow = dt.NewRow();
            dt.Rows.InsertAt(newRow, 0);
            gridView.EditIndex = 0;
            gridView.DataSource = dt;
            gridView.DataBind();
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
                SysOperationResult_BOL operationResult_BOL = transferJournallines.delete(recordsId.ToArray());
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
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        private void getGridDataTable()
        {
            string journalId = Session["JournalID"] as string;

            if (string.IsNullOrEmpty(journalId))
            {
                return;
            }

            DataTable dt = transferJournallines.retrieveAll(journalId);

            if (dt != null)
            {
                SessionVariables.setSessionDataTable(dt);
            }
        }
        protected void ddlFromSiteId_selection(object sender, EventArgs e)
        {
            DropDownList ddlFromSiteId = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlFromSiteId.NamingContainer;
            string selectedSiteId = ddlFromSiteId.SelectedValue;

            DropDownList ddlFromWarehouse = (DropDownList)gridRow.FindControl("lddlFromWarehouse");
            if (ddlFromWarehouse != null)
            {

                DataTable dt = transferJournallines.filterFromWarehousebyFromSite(selectedSiteId);



                ddlFromWarehouse.DataSource = dt;
                ddlFromWarehouse.DataValueField = "FromWarehouse";
                ddlFromWarehouse.DataTextField = "FromWarehouse";
                ddlFromWarehouse.DataBind();
                ddlFromWarehouse.Items.Insert(0, new ListItem("", ""));
                ddlFromWarehouse.CssClass += " filterable-dropdown";

            }
        }


        protected void ddlToSiteId_selection(object sender, EventArgs e)
        {

            DropDownList ddlToSiteId = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlToSiteId.NamingContainer;
            string selectedSiteId = ddlToSiteId.SelectedValue;

            DropDownList ddlToWarehouse = (DropDownList)gridRow.FindControl("ddlToWarehouse");
            if (ddlToWarehouse != null)
            {

                DataTable dt = transferJournallines.filterToWarehousebyToSite(selectedSiteId);



                ddlToWarehouse.DataSource = dt;
                ddlToWarehouse.DataValueField = "ToWarehouse";
                ddlToWarehouse.DataTextField = "ToWarehouse";
                ddlToWarehouse.DataBind();
                ddlToWarehouse.Items.Insert(0, new ListItem("", ""));
                ddlToWarehouse.CssClass += " filterable-dropdown";

            }
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                    var dataItem = (DataRowView)e.Row.DataItem;

                    TextBox txtReceiptDateEdit = (TextBox)e.Row.FindControl("txtReceiptDateEdit");
                    txtReceiptDateEdit.Text = DateTime.Today.ToString();
                    txtReceiptDateEdit.Enabled = false;

                    DropDownList ddlItemNumber = (DropDownList)e.Row.FindControl("ddlItemNumber");
                    DataTable dt = transferJournallines.retrieveItemId();

                    dt.Columns.Add("DisplayText", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        row["DisplayText"] = row["ItemId"] + " - " + row["ProductName"];
                    }


                    ddlItemNumber.DataSource = dt;
                    ddlItemNumber.DataValueField = "ItemId";
                    ddlItemNumber.DataTextField = "DisplayText";
                    ddlItemNumber.DataBind();
                    ddlItemNumber.Items.Insert(0, new ListItem("", String.Empty));

                    ddlItemNumber.CssClass += "filterable-dropdown";

                    // FROM SITE DROPDOWN
                    DropDownList ddlFromSiteId = (DropDownList)e.Row.FindControl("ddlFromSiteId");
                    DataTable dtFromSites = transferJournallines.retrieveAllSites();
                    ddlFromSiteId.DataSource = dtFromSites;
                    ddlFromSiteId.DataValueField = "AllSiteId";
                    ddlFromSiteId.DataTextField = "AllSiteId";
                    ddlFromSiteId.DataBind();
                    ddlFromSiteId.Items.Insert(0, new ListItem("", ""));

                    ddlFromSiteId.CssClass += "filterable-dropdown";


                    // TO SITE DROPDOWN
                    DropDownList ddlToSiteId = (DropDownList)e.Row.FindControl("ddlToSiteId");
                    DataTable dtToSites = transferJournallines.retrieveAllSites();
                    ddlToSiteId.DataSource = dtToSites;
                    ddlToSiteId.DataValueField = "AllSiteId";
                    ddlToSiteId.DataTextField = "AllSiteId";
                    ddlToSiteId.DataBind();
                    ddlToSiteId.Items.Insert(0, new ListItem("", ""));

                    ddlToSiteId.CssClass += "filterable-dropdown";


                    // FROM WAREHOUSE DROPDOWN
                    DropDownList ddlFromWarehouse = (DropDownList)e.Row.FindControl("lddlFromWarehouse");
                    DataTable dtFromWh = transferJournallines.retrieveAllWarehouse();
                    ddlFromWarehouse.DataSource = dtFromWh;
                    ddlFromWarehouse.DataValueField = "AllLocationId";
                    ddlFromWarehouse.DataTextField = "AllLocationId";
                    ddlFromWarehouse.DataBind();
                    ddlFromWarehouse.Items.Insert(0, new ListItem("", ""));

                    ddlFromWarehouse.CssClass += "filterable-dropdown";


                    // TO WAREHOUSE DROPDOWN
                    DropDownList ddlToWarehouse = (DropDownList)e.Row.FindControl("ddlToWarehouse");
                    DataTable dtToWh = transferJournallines.retrieveAllWarehouse();
                    ddlToWarehouse.DataSource = dtToWh;
                    ddlToWarehouse.DataValueField = "AllLocationId";
                    ddlToWarehouse.DataTextField = "AllLocationId";
                    ddlToWarehouse.DataBind();
                    ddlToWarehouse.Items.Insert(0, new ListItem("", ""));

                    ddlToWarehouse.CssClass += "filterable-dropdown";

                    //Unit DropDown
                    //DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
                    //DataTable dtUnit = transferJournallines.retrieveUnitofMeasure();
                    //ddlUnit.DataSource = dtUnit;
                    //ddlUnit.DataValueField = "Unit";
                    //ddlUnit.DataTextField = "Unit";
                    //ddlUnit.DataBind();
                    //ddlUnit.Items.Insert(0, new ListItem("", ""));

                    //ddlUnit.CssClass += "filterable-dropdown";

                    Label inventdimId = (Label)e.Row.FindControl("lblInventdimId");
                    string inventdimIdString = inventdimId.Text.ToString();
                    Session["InventDimId"] = inventdimIdString;

                }
            }
        }

        private void BindLinesHeader()
        {
            string journalId = Session["JournalID"] as string;
            string journalDescription = Session["JournalDescription"] as string;
            string VoucherSeries = Session["VoucherSeries"] as string;
            string SelectionBy = Session["SelectionBy"] as string;
            string NewVoucherBy = Session["NewVoucherBy"] as string;
            string Detaillevel = Session["Detaillevel"] as string;
            string DeletePostedLines = Session["DeletePostedLines"] as string;
            string OffsetAccount = Session["OffsetAccount"] as string;

            //Lines
            lblVoucherSeries.Text = VoucherSeries;
            ddlSelectionBy.Text = SelectionBy;
            ddlNewVoucherBy.Text = NewVoucherBy;
            ddlDetailLevel.Text = Detaillevel;
            //lblOffsetAccount .Text = OffsetAccount;
            //DeleteLinesAfterPosting. = DeletePostedLines;
            //Header
            lblJournal.Text = journalId;
            lblDescription.Text = journalDescription;
            txtVoucherSeries.Text = VoucherSeries;
            txtSelectionBy.Text = SelectionBy;
            txtNewVoucherBy.Text = NewVoucherBy;
            txtDetailLevel.Text = Detaillevel;
            //txtOffsetAccount.Text = OffsetAccount;
            //txtDeleteLinesAfterPosting = DeletePostedLines;
            bool deleteLines = false;
            if (!string.IsNullOrEmpty(DeletePostedLines))
            {
                bool.TryParse(DeletePostedLines, out deleteLines);
            }

            // Update the checkbox (for visual toggle)
            DeleteLinesAfterPostingBox.Checked = deleteLines;

            // Update the Yes/No text
            DeleteLinesAfterPosting.InnerText = deleteLines ? "Yes" : "No";

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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    gridView.SelectedIndex = 0; // Select first row

                }

                Label txtReceiptDate = e.Row.FindControl("txtReceiptDate") as Label;
                if (txtReceiptDate != null && DateTime.TryParse(txtReceiptDate.Text, out DateTime ReceiptDate))
                {
                    txtReceiptDate.Text = ReceiptDate == new DateTime(1900, 1, 1)
                        ? string.Empty
                        : ReceiptDate.ToString("MM-dd-yyyy");
                }

            }
        }


        //private void BindLinesJournalLines()
        //{
        //    string journalId = Session["JournalID"] as string;

        //    DataTable dt = transferJournallines.retrieveAll(journalId);
        //    if (dt.Rows.Count > 0)
        //    {
        //        txtReceiptDate.Text = dt.Rows[0]["NumberSequence"].ToString();
        //        lblProductName.Text = dt.Rows[0]["NumberSequence"].ToString();
        //        ddlProductName.Text = dt.Rows[0]["NumberSequence"].ToString();
        //        lblFromSite.Text = dt.Rows[0]["NumberSequence"].ToString();
        //    }
        //}

        protected void onItemModified(object sender, EventArgs e)
        {
            DropDownList ddlItemNumber = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlItemNumber.NamingContainer;

            string journalId = Session["JournalID"] as string;

            string inventdimIdString = Session["InventDimId"] as string;

            string selectedItemId = ddlItemNumber.SelectedValue;


            if (!string.IsNullOrEmpty(selectedItemId))
            {

                Dictionary<string, bool> columnVisibilities = new Dictionary<string, bool>
                {
                    { "From Batch number", BindFromInventBatchId(selectedItemId, row) },
                    { "To Batch number", BindToInventBatchId(selectedItemId, row) },

                    { "From Serial number", BindFromInventSerialId(selectedItemId, row) },
                    { "To Serial number", BindToInventSerialId(selectedItemId, row) },

                    { "From Configuration", BindFromInventConfigId(selectedItemId, row) },
                    { "To Configuration", BindToInventConfigId(selectedItemId, row) },

                    { "From Size", BindFromInventSizeId(selectedItemId, row) },
                    { "To Size", BindToInventSizeId(selectedItemId, row) },

                    { "From Color", BindFromInventColorId(selectedItemId, row) },
                    { "To Color", BindToInventColorId(selectedItemId, row) },

                    { "From Style", BindFromInventStyleId(selectedItemId, row) },
                    { "To Style", BindToInventStyleId(selectedItemId, row) },

                };


                foreach (DataControlField column in gridView.Columns)
                {
                    if (column is TemplateField && column.HeaderText != null)
                    {
                        string header = column.HeaderText;

                        if (columnVisibilities.ContainsKey(header))
                            column.Visible = columnVisibilities[header];
                    }
                }
            }


            DataTable dt = transferJournallines.onItemModified(selectedItemId, inventdimIdString, journalId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dtrow = dt.Rows[0];

                TextBox txtProductName = row.FindControl("txtProductName") as TextBox;
                txtProductName.Text = dtrow["ProductName"].ToString();


                string inventSiteId = Session["InventSiteId"]?.ToString();
                string inventLocationId = Session["InventLocationId"]?.ToString();


                DropDownList ddlFromSiteId = row.FindControl("ddlFromSiteId") as DropDownList;
                ddlFromSiteId.SelectedValue = inventSiteId;

                DropDownList ddlFromWarehouse = row.FindControl("lddlFromWarehouse") as DropDownList;
                ddlFromWarehouse.SelectedValue = inventLocationId;

                TextBox txtUnit = row.FindControl("txtUnit") as TextBox;
                //string unitValue = dtrow["Unit"].ToString();

                //txtUnit.Text = unitValue.ToString.ToLower();    

                ////// Loop through items and select ignoring case
                ////foreach (ListItem item in txtUnit.Items)
                ////{
                ////    if (item.Value.Equals(unitValue, StringComparison.OrdinalIgnoreCase))
                ////    {
                ////        ddlUnit.SelectedValue = item.Value;
                ////        break;
                ////    }
                ////}

                ////ddlUnit.Enabled = false;
                ///
                string unitValue = dtrow["Unit"].ToString();
                string textboxValue = txtUnit.Text;
                txtUnit.Text = unitValue; 

                
               

            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            GridViewRow gridRow = gridView.Rows[gridView.EditIndex];
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("ItemId", typeof(string));
                dt.Columns.Add("FromSite", typeof(string));
                dt.Columns.Add("FromWarehouse");
                dt.Columns.Add("ToSite");
                dt.Columns.Add("ToWarehouse");
                dt.Columns.Add("Qty", typeof(decimal)); // Quantity as decimal
                dt.Columns.Add("UnitQuantity", typeof(decimal));
                dt.Columns.Add("RecId");


                dt.Columns.Add("FromInventBatchId");
                dt.Columns.Add("FromInventSerialId");
                dt.Columns.Add("FromInventConfigId");
                dt.Columns.Add("FromInventSizeId");
                dt.Columns.Add("FromInventColorId");
                dt.Columns.Add("FromInventStyleId");
                dt.Columns.Add("FromWMSLocationId");


                dt.Columns.Add("ToInventBatchId");
                dt.Columns.Add("ToInventSerialId");
                dt.Columns.Add("ToInventConfigId");
                dt.Columns.Add("ToInventSizeId");
                dt.Columns.Add("ToInventColorId");
                dt.Columns.Add("ToInventStyleId");
                dt.Columns.Add("ToWMSLocationId");

                // Get RecId from session
                long recId = Session["RecId"] != null ? (long)Session["RecId"] : 0;

                DataRow row = dt.NewRow();

                DropDownList itemId = (DropDownList)gridRow.FindControl("ddlItemNumber");
                DropDownList ddlFromSiteId = (DropDownList)gridRow.FindControl("ddlFromSiteId");
                DropDownList ddlFromWarehouse = (DropDownList)gridRow.FindControl("lddlFromWarehouse");
                DropDownList ddlToSiteId = (DropDownList)gridRow.FindControl("ddlToSiteId");
                DropDownList ddlToWarehouse = (DropDownList)gridRow.FindControl("ddlToWarehouse");

                DropDownList ddlFromBatch = (DropDownList)gridRow.FindControl("ddlFromInventBatchId");
                DropDownList ddlToBatch = (DropDownList)gridRow.FindControl("ddlToInventBatchId");

                DropDownList ddlFromSerial = (DropDownList)gridRow.FindControl("ddlFromInventSerialId");
                DropDownList ddlToSerial = (DropDownList)gridRow.FindControl("ddlToInventSerialId");

                DropDownList ddlFromConfig = (DropDownList)gridRow.FindControl("ddlFromInventConfigId");
                DropDownList ddlToConfig = (DropDownList)gridRow.FindControl("ddlToInventConfigId");

                DropDownList ddlFromSize = (DropDownList)gridRow.FindControl("ddlFromInventSizeId");
                DropDownList ddlToSize = (DropDownList)gridRow.FindControl("ddlToInventSizeId");

                DropDownList ddlFromColor = (DropDownList)gridRow.FindControl("ddlFromInventColorId");
                DropDownList ddlToColor = (DropDownList)gridRow.FindControl("ddlToInventColorId");

                DropDownList ddlFromStyle = (DropDownList)gridRow.FindControl("ddlFromInventStyleId");
                DropDownList ddlToStyle = (DropDownList)gridRow.FindControl("ddlToInventStyleId");

                DropDownList ddlFromLocation = (DropDownList)gridRow.FindControl("ddlFromLocationId");
                DropDownList ddlToLocation = (DropDownList)gridRow.FindControl("ddlToLocationId");

                TextBox txtQuantityEdit = (TextBox)gridRow.FindControl("txtQuantityEdit");
                TextBox txtUnitQuantityEdit = (TextBox)gridRow.FindControl("txtUnitQuantityEdit");
                string qtyText = txtQuantityEdit.Text.Trim();
                string unitQtyText = txtUnitQuantityEdit.Text.Trim();


                row["RecId"] = recId;  // assign RecId from session
                row["ItemId"] = itemId.SelectedValue;
                row["FromSite"] = ddlFromSiteId.SelectedValue;
                row["ToSite"] = ddlToSiteId.SelectedValue;
                row["FromWarehouse"] = ddlFromWarehouse.SelectedValue;
                row["ToWarehouse"] = ddlToWarehouse.SelectedValue;

                row["FromInventBatchId"] = ddlFromBatch?.SelectedValue ?? "";
                row["ToInventBatchId"] = ddlToBatch?.SelectedValue ?? "";

                row["FromInventSerialId"] = ddlFromSerial?.SelectedValue ?? "";
                row["ToInventSerialId"] = ddlToSerial?.SelectedValue ?? "";

                row["FromInventConfigId"] = ddlFromConfig?.SelectedValue ?? "";
                row["ToInventConfigId"] = ddlToConfig?.SelectedValue ?? "";

                row["FromInventSizeId"] = ddlFromSize?.SelectedValue ?? "";
                row["ToInventSizeId"] = ddlToSize?.SelectedValue ?? "";

                row["FromInventColorId"] = ddlFromColor?.SelectedValue ?? "";
                row["ToInventColorId"] = ddlToColor?.SelectedValue ?? "";

                row["FromInventStyleId"] = ddlFromStyle?.SelectedValue ?? "";
                row["ToInventStyleId"] = ddlToStyle?.SelectedValue ?? "";

                row["FromWMSLocationId"] = ddlFromLocation?.SelectedValue ?? "";
                row["ToWMSLocationId"] = ddlToLocation?.SelectedValue ?? "";

                // Parse quantity safely
                decimal qty = 0;
                decimal.TryParse(txtQuantityEdit.Text.Trim(), out qty);
                row["Qty"] = qty;

                decimal unitQty = 0;
                decimal.TryParse(txtUnitQuantityEdit.Text.Trim(), out unitQty);
                row["UnitQuantity"] = unitQty;

                dt.Rows.Add(row);

                createResult = transferJournallines.create(dt);


                if (string.IsNullOrWhiteSpace(createResult.Message))
                {
                    createResult.isSuccess = false;
                    createResult.Message = "Error while creating Transfer Journal line";
                    createResult.AlertType = AlertType.Error.ToString();
                }

                NotificationMessage.showMessage(createResult);

                if (createResult.isSuccess)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }

            }
            catch (Exception ex)
            {
                createResult.isSuccess = false;
                createResult.Message = "An error occurred: " + ex.Message;
                createResult.AlertType = AlertType.Error.ToString();
                NotificationMessage.showMessage(createResult);
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {

            gridView.EditIndex = -1;
            reBindGrid();
        }


        private bool BindFromInventBatchId(string itemid, GridViewRow e)
        {

            DataTable dtFrom = transferJournallines.retrieveFromInventBatchId(itemid);
            DropDownList ddlFromInventBatchId = (DropDownList)e.FindControl("ddlFromInventBatchId");
            if (ddlFromInventBatchId != null)
            {
                ddlFromInventBatchId.DataSource = dtFrom;
                ddlFromInventBatchId.DataValueField = "FromInventBatchId";
                ddlFromInventBatchId.DataTextField = "FromInventBatchId";
                ddlFromInventBatchId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventBatchId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventBatchId.Items[0].Text)))
                {
                    ddlFromInventBatchId.Visible = false;
                    return ddlFromInventBatchId.Visible;
                }
                else
                {
                    ddlFromInventBatchId.Visible = true;
                    ddlFromInventBatchId.Enabled = true;
                    ddlFromInventBatchId.BackColor = System.Drawing.Color.White;
                    ddlFromInventBatchId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventBatchId.CssClass += " filterable-dropdown";
                    return ddlFromInventBatchId.Visible;
                }
            }
            return false;
        }

        private bool BindFromInventSerialId(string itemid, GridViewRow e)
        {
            DataTable dtFrom = transferJournallines.retrieveFromInventSerialId(itemid);
            DropDownList ddlFromInventSerialId = (DropDownList)e.FindControl("ddlFromInventSerialId");

            if (ddlFromInventSerialId != null)
            {
                ddlFromInventSerialId.DataSource = dtFrom;
                ddlFromInventSerialId.DataValueField = "FromInventSerialId";
                ddlFromInventSerialId.DataTextField = "FromInventSerialId";
                ddlFromInventSerialId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventSerialId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventSerialId.Items[0].Text)))
                {
                    ddlFromInventSerialId.Visible = false;
                    return ddlFromInventSerialId.Visible;
                }
                else
                {
                    ddlFromInventSerialId.Visible = true;
                    ddlFromInventSerialId.Enabled = true;
                    ddlFromInventSerialId.BackColor = System.Drawing.Color.White;
                    ddlFromInventSerialId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventSerialId.CssClass += " filterable-dropdown";
                    return ddlFromInventSerialId.Visible;
                }
            }
            return false;
        }

        private bool BindFromInventConfigId(string itemid, GridViewRow e)
        {
            DataTable dtFrom = transferJournallines.retrieveFromConfigId(itemid);
            DropDownList ddlFromInventConfigId = (DropDownList)e.FindControl("ddlFromInventConfigId");

            if (ddlFromInventConfigId != null)
            {
                ddlFromInventConfigId.DataSource = dtFrom;
                ddlFromInventConfigId.DataValueField = "FromInventConfigId";
                ddlFromInventConfigId.DataTextField = "FromInventConfigId";
                ddlFromInventConfigId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventConfigId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventConfigId.Items[0].Text)))
                {
                    ddlFromInventConfigId.Visible = false;
                    return ddlFromInventConfigId.Visible;
                }
                else
                {
                    ddlFromInventConfigId.Visible = true;
                    ddlFromInventConfigId.Enabled = true;
                    ddlFromInventConfigId.BackColor = System.Drawing.Color.White;
                    ddlFromInventConfigId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventConfigId.CssClass += " filterable-dropdown";
                    return ddlFromInventConfigId.Visible;
                }
            }
            return false;
        }

        private bool BindFromInventColorId(string itemid, GridViewRow e)
        {
            DataTable dtFrom = transferJournallines.retrieveFromInventColorId(itemid);
            DropDownList ddlFromInventColorId = (DropDownList)e.FindControl("ddlFromInventColorId");

            if (ddlFromInventColorId != null)
            {
                ddlFromInventColorId.DataSource = dtFrom;
                ddlFromInventColorId.DataValueField = "FromInventColorId";
                ddlFromInventColorId.DataTextField = "FromInventColorId";
                ddlFromInventColorId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventColorId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventColorId.Items[0].Text)))
                {
                    ddlFromInventColorId.Visible = false;
                    return ddlFromInventColorId.Visible;
                }
                else
                {
                    ddlFromInventColorId.Visible = true;
                    ddlFromInventColorId.Enabled = true;
                    ddlFromInventColorId.BackColor = System.Drawing.Color.White;
                    ddlFromInventColorId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventColorId.CssClass += " filterable-dropdown";
                    return ddlFromInventColorId.Visible;
                }
            }
            return false;
        }

        private bool BindFromInventSizeId(string itemid, GridViewRow e)
        {
            DataTable dtFrom = transferJournallines.retrieveFromInventSizeId(itemid);
            DropDownList ddlFromInventSizeId = (DropDownList)e.FindControl("ddlFromInventSizeId");

            if (ddlFromInventSizeId != null)
            {
                ddlFromInventSizeId.DataSource = dtFrom;
                ddlFromInventSizeId.DataValueField = "FromInventSizeId";
                ddlFromInventSizeId.DataTextField = "FromInventSizeId";
                ddlFromInventSizeId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventSizeId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventSizeId.Items[0].Text)))
                {
                    ddlFromInventSizeId.Visible = false;
                    return ddlFromInventSizeId.Visible;
                }
                else
                {
                    ddlFromInventSizeId.Visible = true;
                    ddlFromInventSizeId.Enabled = true;
                    ddlFromInventSizeId.BackColor = System.Drawing.Color.White;
                    ddlFromInventSizeId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventSizeId.CssClass += " filterable-dropdown";
                    return ddlFromInventSizeId.Visible;
                }
            }
            return false;
        }

        private bool BindFromInventStyleId(string itemid, GridViewRow e)
        {
            DataTable dtFrom = transferJournallines.retrieveFromInventStyleId(itemid);
            DropDownList ddlFromInventStyleId = (DropDownList)e.FindControl("ddlFromInventStyleId");

            if (ddlFromInventStyleId != null)
            {
                ddlFromInventStyleId.DataSource = dtFrom;
                ddlFromInventStyleId.DataValueField = "FromInventStyleId";
                ddlFromInventStyleId.DataTextField = "FromInventStyleId";
                ddlFromInventStyleId.DataBind();

                if (dtFrom == null || dtFrom.Rows.Count == 0 ||
                    (ddlFromInventStyleId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromInventStyleId.Items[0].Text)))
                {
                    ddlFromInventStyleId.Visible = false;
                    return ddlFromInventStyleId.Visible;
                }
                else
                {
                    ddlFromInventStyleId.Visible = true;
                    ddlFromInventStyleId.Enabled = true;
                    ddlFromInventStyleId.BackColor = System.Drawing.Color.White;
                    ddlFromInventStyleId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlFromInventStyleId.CssClass += " filterable-dropdown";
                    return ddlFromInventStyleId.Visible;
                }
            }
            return false;
        }

        private bool BindToInventBatchId(string itemid, GridViewRow e)
        {

            DataTable dtTo = transferJournallines.retrieveToInventBatchId(itemid);
            DropDownList ddlToInventBatchId = (DropDownList)e.FindControl("ddlToInventBatchId");
            if (ddlToInventBatchId != null)
            {
                ddlToInventBatchId.DataSource = dtTo;
                ddlToInventBatchId.DataValueField = "ToInventBatchId";
                ddlToInventBatchId.DataTextField = "ToInventBatchId";
                ddlToInventBatchId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventBatchId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventBatchId.Items[0].Text)))
                {
                    ddlToInventBatchId.Visible = false;
                    return ddlToInventBatchId.Visible;
                }
                else
                {
                    ddlToInventBatchId.Visible = true;
                    ddlToInventBatchId.Enabled = true;
                    ddlToInventBatchId.BackColor = System.Drawing.Color.White;
                    ddlToInventBatchId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventBatchId.CssClass += " filterable-dropdown";
                    return ddlToInventBatchId.Visible;
                }
            }
            return false;
        }


        private bool BindToInventSerialId(string itemid, GridViewRow e)
        {
            DataTable dtTo = transferJournallines.retrieveToInventSerialId(itemid);
            DropDownList ddlToInventSerialId = (DropDownList)e.FindControl("ddlToInventSerialId");

            if (ddlToInventSerialId != null)
            {
                ddlToInventSerialId.DataSource = dtTo;
                ddlToInventSerialId.DataValueField = "ToInventSerialId";
                ddlToInventSerialId.DataTextField = "ToInventSerialId";
                ddlToInventSerialId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventSerialId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventSerialId.Items[0].Text)))
                {
                    ddlToInventSerialId.Visible = false;
                    return ddlToInventSerialId.Visible;
                }
                else
                {
                    ddlToInventSerialId.Visible = true;
                    ddlToInventSerialId.Enabled = true;
                    ddlToInventSerialId.BackColor = System.Drawing.Color.White;
                    ddlToInventSerialId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventSerialId.CssClass += " filterable-dropdown";
                    return ddlToInventSerialId.Visible;
                }
            }
            return false;
        }

        private bool BindToInventConfigId(string itemid, GridViewRow e)
        {
            DataTable dtTo = transferJournallines.retrieveToConfigId(itemid);
            DropDownList ddlToInventConfigId = (DropDownList)e.FindControl("ddlToInventConfigId");

            if (ddlToInventConfigId != null)
            {
                ddlToInventConfigId.DataSource = dtTo;
                ddlToInventConfigId.DataValueField = "ToInventConfigId";
                ddlToInventConfigId.DataTextField = "ToInventConfigId";
                ddlToInventConfigId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventConfigId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventConfigId.Items[0].Text)))
                {
                    ddlToInventConfigId.Visible = false;
                    return ddlToInventConfigId.Visible;
                }
                else
                {
                    ddlToInventConfigId.Visible = true;
                    ddlToInventConfigId.Enabled = true;
                    ddlToInventConfigId.BackColor = System.Drawing.Color.White;
                    ddlToInventConfigId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventConfigId.CssClass += " filterable-dropdown";
                    return ddlToInventConfigId.Visible;
                }
            }
            return false;
        }

        private bool BindToInventColorId(string itemid, GridViewRow e)
        {
            DataTable dtTo = transferJournallines.retrieveToInventColorId(itemid);
            DropDownList ddlToInventColorId = (DropDownList)e.FindControl("ddlToInventColorId");

            if (ddlToInventColorId != null)
            {
                ddlToInventColorId.DataSource = dtTo;
                ddlToInventColorId.DataValueField = "ToInventColorId";
                ddlToInventColorId.DataTextField = "ToInventColorId";
                ddlToInventColorId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventColorId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventColorId.Items[0].Text)))
                {
                    ddlToInventColorId.Visible = false;
                    return ddlToInventColorId.Visible;
                }
                else
                {
                    ddlToInventColorId.Visible = true;
                    ddlToInventColorId.Enabled = true;
                    ddlToInventColorId.BackColor = System.Drawing.Color.White;
                    ddlToInventColorId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventColorId.CssClass += " filterable-dropdown";
                    return ddlToInventColorId.Visible;
                }
            }
            return false;
        }

        private bool BindToInventSizeId(string itemid, GridViewRow e)
        {
            DataTable dtTo = transferJournallines.retrieveToInventSizeId(itemid);
            DropDownList ddlToInventSizeId = (DropDownList)e.FindControl("ddlToInventSizeId");

            if (ddlToInventSizeId != null)
            {
                ddlToInventSizeId.DataSource = dtTo;
                ddlToInventSizeId.DataValueField = "ToInventSizeId";
                ddlToInventSizeId.DataTextField = "ToInventSizeId";
                ddlToInventSizeId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventSizeId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventSizeId.Items[0].Text)))
                {
                    ddlToInventSizeId.Visible = false;
                    return ddlToInventSizeId.Visible;
                }
                else
                {
                    ddlToInventSizeId.Visible = true;
                    ddlToInventSizeId.Enabled = true;
                    ddlToInventSizeId.BackColor = System.Drawing.Color.White;
                    ddlToInventSizeId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventSizeId.CssClass += " filterable-dropdown";
                    return ddlToInventSizeId.Visible;
                }
            }
            return false;
        }


        private bool BindToInventStyleId(string itemid, GridViewRow e)
        {
            DataTable dtTo = transferJournallines.retrieveToInventStyleId(itemid);
            DropDownList ddlToInventStyleId = (DropDownList)e.FindControl("ddlToInventStyleId");

            if (ddlToInventStyleId != null)
            {
                ddlToInventStyleId.DataSource = dtTo;
                ddlToInventStyleId.DataValueField = "ToInventStyleId";
                ddlToInventStyleId.DataTextField = "ToInventStyleId";
                ddlToInventStyleId.DataBind();

                if (dtTo == null || dtTo.Rows.Count == 0 ||
                    (ddlToInventStyleId.Items.Count == 1 && string.IsNullOrEmpty(ddlToInventStyleId.Items[0].Text)))
                {
                    ddlToInventStyleId.Visible = false;
                    return ddlToInventStyleId.Visible;
                }
                else
                {
                    ddlToInventStyleId.Visible = true;
                    ddlToInventStyleId.Enabled = true;
                    ddlToInventStyleId.BackColor = System.Drawing.Color.White;
                    ddlToInventStyleId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlToInventStyleId.CssClass += " filterable-dropdown";
                    return ddlToInventStyleId.Visible;
                }
            }
            return false;
        }

        //private bool BindFromWMSLocationId(string itemid, GridViewRow e)
        //{
        //    DataTable dtTo = transferJournallines.retrieveFromWMSLocationId(itemid);
        //    DropDownList ddlFromLocationId = (DropDownList)e.FindControl("ddlFromLocationId");

        //    if (ddlFromLocationId != null)
        //    {
        //        ddlFromLocationId.DataSource = dtTo;
        //        ddlFromLocationId.DataValueField = "FromWMSLocationId";
        //        ddlFromLocationId.DataTextField = "FromWMSLocationId";
        //        ddlFromLocationId.DataBind();

        //        if (dtTo == null || dtTo.Rows.Count == 0 ||
        //            (ddlFromLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlFromLocationId.Items[0].Text)))
        //        {
        //            ddlFromLocationId.Visible = false;
        //            return ddlFromLocationId.Visible;
        //        }
        //        else
        //        {
        //            ddlFromLocationId.Visible = true;
        //            ddlFromLocationId.Enabled = true;
        //            ddlFromLocationId.BackColor = System.Drawing.Color.White;
        //            ddlFromLocationId.Items.Insert(0, new ListItem("", string.Empty));
        //            ddlFromLocationId.CssClass += " filterable-dropdown";
        //            return ddlFromLocationId.Visible;
        //        }
        //    }
        //    return false;
        //}

        //private bool BindToWMSLocationId(string itemid, GridViewRow e)
        //{
        //    DataTable dtTo = transferJournallines.retrieveToWMSLocationId(itemid);
        //    DropDownList ddlToLocationId = (DropDownList)e.FindControl("ddlToLocationId");

        //    if (ddlToLocationId != null)
        //    {
        //        ddlToLocationId.DataSource = dtTo;
        //        ddlToLocationId.DataValueField = "ToWMSLocationId";
        //        ddlToLocationId.DataTextField = "ToWMSLocationId";
        //        ddlToLocationId.DataBind();

        //        if (dtTo == null || dtTo.Rows.Count == 0 ||
        //            (ddlToLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlToLocationId.Items[0].Text)))
        //        {
        //            ddlToLocationId.Visible = false;
        //            return ddlToLocationId.Visible;
        //        }
        //        else
        //        {
        //            ddlToLocationId.Visible = true;
        //            ddlToLocationId.Enabled = true;
        //            ddlToLocationId.BackColor = System.Drawing.Color.White;
        //            ddlToLocationId.Items.Insert(0, new ListItem("", string.Empty));
        //            ddlToLocationId.CssClass += " filterable-dropdown";
        //            return ddlToLocationId.Visible;
        //        }
        //    }
        //    return false;
        //}

        protected void onFromOnlyWarehouseSelection(object sender, EventArgs e)
        {
            DropDownList lddlFromWarehouse = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)lddlFromWarehouse.NamingContainer;
            string selectedFromWarehouse = lddlFromWarehouse.SelectedValue;

            DropDownList ddlFromLocationId = (DropDownList)gridRow.FindControl("ddlFromLocationId");
            if (ddlFromLocationId != null)
            {

                DataTable dt = transferJournallines.retrieveFromWMSLocationId(selectedFromWarehouse);



                ddlFromLocationId.DataSource = dt;
                ddlFromLocationId.DataValueField = "FromWMSLocationId";
                ddlFromLocationId.DataTextField = "FromWMSLocationId";
                ddlFromLocationId.DataBind();
                ddlFromLocationId.Items.Insert(0, new ListItem("", ""));
                ddlFromLocationId.CssClass += " filterable-dropdown";

            }
        }

        protected void onToOnlyWarehouseSelection(object sender, EventArgs e)
        {
            DropDownList ddlToWarehouse = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlToWarehouse.NamingContainer;
            string selectedToWarehouse = ddlToWarehouse.SelectedValue;

            DropDownList ddlToLocationId = (DropDownList)gridRow.FindControl("ddlToLocationId");
            if (ddlToLocationId != null)
            {

                DataTable dt = transferJournallines.retrieveToWMSLocationId(selectedToWarehouse);



                ddlToLocationId.DataSource = dt;
                ddlToLocationId.DataValueField = "ToWMSLocationId";
                ddlToLocationId.DataTextField = "ToWMSLocationId";
                ddlToLocationId.DataBind();
                ddlToLocationId.Items.Insert(0, new ListItem("", ""));
                ddlToLocationId.CssClass += " filterable-dropdown";

            }
        }

        protected void OnQuantityModified(object sender, EventArgs e)
        {
            TextBox ddlQuantity = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)ddlQuantity.NamingContainer;
            string quantity = ddlQuantity.Text;

            TextBox ddlUnitQuantity = (TextBox)gridRow.FindControl("txtUnitQuantityEdit");

            ddlUnitQuantity.Text = quantity;

        }
    }
}