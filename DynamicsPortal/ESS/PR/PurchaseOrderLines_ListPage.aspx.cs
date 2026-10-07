using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Linq;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_ListPage : MainForm
    {
        private PurchaseOrderLines purchaselines = new PurchaseOrderLines();

        protected void Page_Init(object sender, EventArgs e)
        {
            // Always rebuild dynamic controls early
            showFinancialDimension(0);
            showFinancialDimensionHamza(0);

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "PurchaseOrderLines_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Purchase Order Details";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Purchase order details";
                        titleDiv.Style["font-weight"] = "bold";
                    }


                    BindHeaderValues();
                    getGridDataTable(); // Ensure DataTable is stored in session
                    bindGrid();

                    BindReasonCode();
                    BindTermsOfPayment();
                    BindMethodOfPayment();
                    BindSchedulePayment();
                    BindSite();
                    BindWarehouse();
                    BindTaxGroup();
                    BindBuyerGroup();
                    BindPool();
                    BindModeOfDelivery();
                    BindDeliveryTerm();
                    BindVenderAccount();
                    BindInvoiceAccount();
                    BindContactId();
                    BindLanguageId();
                    BindPostingProfile();
                    BindNumberSeq();
                    BindRequester();
                    BindEmail();
                    BindOrderer();
                    BindShippingCarrier();






                    string purchaseorderID = Session["PurchaseOrderId"] as string;
                    string vendoraccount = Session["VendorAccount"] as string;
                    string vendorname = Session["VendorName"] as string;
                    string status = Session["ApprovalStatus"] as string ?? string.Empty;

                    lblPurchaseOrderID.Text = purchaseorderID;
                    lblVendorAccount.Text = vendoraccount;
                    lblVendorName.Text = vendorname;
                    lblPurchStatus.Text = status;

                    // Apply consistent styling
                    lblPurchaseOrderID.Style["font-weight"] = "bold";
                    lblVendorAccount.Style["font-weight"] = "bold";
                    lblVendorName.Style["font-weight"] = "bold";
                    lblPurchaseOrderID.Style["font-size"] = "20px";

                    lblVendorAccount.Style["font-size"] = "20px";
                    lblVendorName.Style["font-size"] = "20px";

                    lblPurchStatus.Style["font-weight"] = "bold";
                    lblPurchStatus.Style["font-size"] = "20px";

                    if (status == "In review")
                    {
                        BtnAddLine.Enabled = false;
                        btnDelete.Enabled = false;
                        BtnAddLine.CssClass = "disabled-button";
                        btnDelete.CssClass = "disabled-button";
                        BtnDeleteHeader.Enabled = false;
                        btnSubmit.Enabled = false;
                        btnWorkflow.Enabled = false;
                        btnWorkflow.CssClass = "disabled-button;";
                        btnSubmit.CssClass = "disabled-button";

                    }
                    else if (status == "Approved")
                    {
                        BtnAddLine.Enabled = true;
                        btnDelete.Enabled = true;
                       // BtnAddLine.CssClass = "disabled-button";
                      //  btnDelete.CssClass = "disabled-button";
                        BtnDeleteHeader.Enabled = false;
                        btnSubmit.Enabled = false;
                        btnWorkflow.Enabled = false;
                        btnWorkflow.CssClass = "disabled-button;";
                        btnSubmit.CssClass = "disabled-button";


                    }
                    else
                    {
                        BtnAddLine.Enabled = true;
                        btnDelete.Enabled = true;
                        BtnDeleteHeader.Enabled = true;
                        btnWorkflow.Enabled = true;
                        btnSubmit.Enabled = true;

                    }


                    string purchReqId = Session["PurchaseOrderId"] as string;
                    DataTable dt = purchaselines.retrieveAll(purchReqId);

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



                    if (gridView.Rows.Count > 0)
                    {
                        GridViewRow firstRow = gridView.Rows[0];

                        // Tick the first row's checkbox
                        CheckBox chk = firstRow.FindControl("chk_SelectSingle") as CheckBox;
                        if (chk != null) chk.Checked = true;

                        chk_SelectSingle_CheckedChanged(chk, EventArgs.Empty);
                    }
                    bindHeaderPanelDeatils();
                    if (Session["VendorDefaultDimension"] != null)
                    {
                        long defaultDimension = Convert.ToInt64(Session["VendorDefaultDimension"]);

                        // ✅ Now call your method
                        showFinancialDimensionHamza(defaultDimension);
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
        }

        private void BindHeaderValues()
        {
            lblPurchaseOrderID.Text = Session["PurchaseOrderId"]?.ToString() ?? "";
            lblVendorAccount.Text = Session["VendorAccount"]?.ToString() ?? "";
            lblVendorName.Text = Session["VendorName"]?.ToString() ?? "";
            DateTime requestedReceiptDate;
            if (DateTime.TryParse(Session["RequestedReceiptDate"]?.ToString(), out requestedReceiptDate))
            {
                txtUniqueDeliveryHeaderRequestDate.Text = requestedReceiptDate.ToString("M/d/yyyy");
                txtCrossDockRequestedDate.Text = requestedReceiptDate.ToString("M/d/yyyy");
            }
            else
            {
                txtUniqueDeliveryHeaderRequestDate.Text = "";
                txtCrossDockRequestedDate.Text = "";
            }
        }

        private void getGridDataTable()
        {
            string purchReqId = Session["PurchaseOrderId"] as string;

            if (string.IsNullOrEmpty(purchReqId))
            {
                return;
            }

            DataTable dt = purchaselines.retrieveAll(purchReqId);

            if (dt != null)
            {
                SessionVariables.setSessionDataTable(dt);
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
                    PopulateLinesDetails(selectedRow); // now call with the row

                    string recIdText = GetLabelText(selectedRow, "lblRecId");
                    if (!string.IsNullOrEmpty(recIdText))
                    {
                        if (long.TryParse(recIdText, out long recId))
                        {
                            Session["RecIdline"] = recId;  // ✅ store as long, not string
                        }
                    }

                    // --- Financial Dimensions ---
                    string defaultDimensionRecIdStr = GetLabelText(selectedRow, "lblDefaultDimension");
                    long defaultDimensionRecId = 0;

                    if (!string.IsNullOrEmpty(defaultDimensionRecIdStr))
                        long.TryParse(defaultDimensionRecIdStr, out defaultDimensionRecId);

                    showFinancialDimension(defaultDimensionRecId);

                    if (defaultDimensionRecId > 0)
                    {
                        DataTable dimTable = new PurchaseRequisitionLine().retrievefinancialdimension(defaultDimensionRecId);
                        bindEmployeeDimension(defaultDimensionRecId);

                        if (dimTable != null && dimTable.Rows.Count > 0)
                        {
                            DataRow row = dimTable.Rows[0];

                            // Example: map values to labels if they exist
                            // FinancialDimensionBusinessUnit.Text = row.Table.Columns.Contains("BusinessUnit")
                            //                                        ? row["BusinessUnit"].ToString() : "N/A";
                            // FinancialDimensionCostCenter.Text = row.Table.Columns.Contains("CostCenter")
                            //                                        ? row["CostCenter"].ToString() : "N/A";
                            // etc...
                        }
                    }
                }
                else
                {
                    ClearLinesDetails(); // nothing selected
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                ClearLinesDetails();
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('An error occurred: {ex.Message}');", true);
            }
        }


        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void ddlsiteId_selection(object sender, EventArgs e)
        {
            DropDownList ddlSiteId = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlSiteId.NamingContainer;
            string selectedSiteId = ddlSiteId.SelectedValue;

            DropDownList ddlWarehouse = (DropDownList)gridRow.FindControl("ddlWarehouse");
            if (ddlWarehouse != null)
            {
                DataTable dt = purchaselines.retrievelocationId(selectedSiteId);

                dt.Columns.Add("DisplayText", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["InventLocationId"] + " - " + row["LocationName"];
                }

                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataValueField = "InventLocationId";
                ddlWarehouse.DataTextField = "DisplayText";
                ddlWarehouse.DataBind();
                ddlWarehouse.Items.Insert(0, new ListItem("", String.Empty));
                ddlWarehouse.CssClass += " filterable-dropdown";
            }
        }

        protected void ddlTemId_fillDimension(object sender, EventArgs e)
        {
            DropDownList ddlItemId = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlItemId.NamingContainer;
            string selectedItemId = ddlItemId.SelectedValue;

            if (!string.IsNullOrEmpty(selectedItemId))
            {
                Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
                {
                    { "Batch Number", ("", BindInventBatchId(selectedItemId, row)) },
                    { "WMS Location", ("", BindWmsLocationId(selectedItemId, row)) },
                    { "Serial Number", ("", BindInventSerialId(selectedItemId, row)) },
                    { "Configuration", ("", BindConfigId(selectedItemId, row)) },
                    { "Combinations", ("", BindCombination(selectedItemId, row)) },
                    { "Size", ("", BindInventSizeId(selectedItemId, row)) },
                    { "Color", ("", BindInventColorId(selectedItemId, row)) },
                    { "Style", ("", BindInventStyleId(selectedItemId, row)) },
                    //{ "WMS Pallet", ("", BindWmsPalletId(selectedItemId, row)) }
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

                PurchaseRequisitionLine line = new PurchaseRequisitionLine();
                DataTable dt = line.itemIdModified(selectedItemId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    TextBox txtProcurementCategory = row.FindControl("txtProcurementCategory") as TextBox;
                    if (txtProcurementCategory != null)
                        txtProcurementCategory.Text = dtrow["Category"].ToString();

                    TextBox txtProductName = row.FindControl("txtProductName") as TextBox;
                    if (txtProductName != null)
                        txtProductName.Text = dtrow["ItemName"].ToString();

                    TextBox txtUnit = row.FindControl("txtUnit") as TextBox;
                    if (txtUnit != null)
                        txtUnit.Text = dtrow["PurchUnitofMeasureCode"].ToString();


                    //TextBox txtUnitPrice = row.FindControl("txtUnitPrice") as TextBox;
                    //if (txtUnitPrice != null)
                    //    txtUnitPrice.Text = dtrow["PurchPrice"].ToString();

                    //TextBox txtNetAmount = row.FindControl("txtNetAmount") as TextBox;
                    //if (txtNetAmount != null)
                    //    txtNetAmount.Text = dtrow["LineAmount"].ToString();

                    //TextBox txtCurrencyCode = row.FindControl("txtCurrencyCode") as TextBox;
                    //if (txtCurrencyCode != null)
                    //    txtCurrencyCode.Text = dtrow["CurrencyCode"].ToString();

                    //TextBox txtVendAccount = row.FindControl("txtVendAccount") as TextBox;
                    //if (txtVendAccount != null)
                    //    txtVendAccount.Text = dtrow["VendAccount"].ToString();

                    //TextBox txtVendorName = row.FindControl("txtVendorName") as TextBox;
                    //if (txtVendorName != null)
                    //    txtVendorName.Text = dtrow["VendorName"].ToString();

                    TextBox txtPurchQty = row.FindControl("txtPurchQty") as TextBox;
                    if (txtPurchQty != null)
                        txtPurchQty.Text = "1";


                }
                // Default quantity
                TextBox txtQtyTransfer = (TextBox)row.FindControl("txtQuantity");
                if (txtQtyTransfer != null)
                    txtQtyTransfer.Text = "1";
            }
        }

        private bool BindInventBatchId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveinventBatchId(itemid);
            DropDownList ddlInventBatchId = (DropDownList)e.FindControl("ddlInventBatchId");
            if (ddlInventBatchId != null)
            {
                ddlInventBatchId.DataSource = dt;
                ddlInventBatchId.DataValueField = "InventBatchId";
                ddlInventBatchId.DataTextField = "InventBatchId";
                ddlInventBatchId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventBatchId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventBatchId.Items[0].Text)))
                {
                    ddlInventBatchId.Visible = false;
                    ddlInventBatchId.Items.Clear();
                    ddlInventBatchId.Enabled = false;
                    ddlInventBatchId.BackColor = System.Drawing.Color.LightGray;
                    ddlInventBatchId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlInventBatchId.Visible = true;
                    ddlInventBatchId.Enabled = true;
                    ddlInventBatchId.BackColor = System.Drawing.Color.White;
                    ddlInventBatchId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventBatchId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
        }

        private bool BindWmsLocationId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrievewmsLocationId(itemid);
            DropDownList ddlWmsLocationId = (DropDownList)e.FindControl("ddlWMSLocationId");
            if (ddlWmsLocationId != null)
            {
                ddlWmsLocationId.DataSource = dt;
                ddlWmsLocationId.DataValueField = "WmsLocationId";
                ddlWmsLocationId.DataTextField = "WmsLocationId";
                ddlWmsLocationId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlWmsLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsLocationId.Items[0].Text)))
                {
                    ddlWmsLocationId.Visible = false;
                    ddlWmsLocationId.Items.Clear();
                    ddlWmsLocationId.Enabled = false;
                    ddlWmsLocationId.BackColor = System.Drawing.Color.LightGray;
                    ddlWmsLocationId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlWmsLocationId.Visible = true;
                    ddlWmsLocationId.Enabled = true;
                    ddlWmsLocationId.BackColor = System.Drawing.Color.White;
                    ddlWmsLocationId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlWmsLocationId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
        }

        private bool BindInventSerialId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveinventSerialId(itemid);
            DropDownList ddlInventSerialId = (DropDownList)e.FindControl("ddlInventSerialId");
            if (ddlInventSerialId != null)
            {
                ddlInventSerialId.DataSource = dt;
                ddlInventSerialId.DataValueField = "InventSerialId";
                ddlInventSerialId.DataTextField = "InventSerialId";
                ddlInventSerialId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventSerialId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSerialId.Items[0].Text)))
                {
                    ddlInventSerialId.Visible = false;
                    ddlInventSerialId.Items.Clear();
                    ddlInventSerialId.Enabled = false;
                    ddlInventSerialId.BackColor = System.Drawing.Color.LightGray;
                    ddlInventSerialId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlInventSerialId.Visible = true;
                    ddlInventSerialId.Enabled = true;
                    ddlInventSerialId.BackColor = System.Drawing.Color.White;
                    ddlInventSerialId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventSerialId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
        }

        private bool BindConfigId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveconfigId(itemid);
            DropDownList ddlConfigId = (DropDownList)e.FindControl("ddlConfigId");
            DropDownList_ProductCombination ddlCombinationLookup = (DropDownList_ProductCombination)e.FindControl("ProductLookupControl");
            if (ddlConfigId != null)
            {
                ddlConfigId.DataSource = dt;
                ddlConfigId.DataValueField = "ConfigId";
                ddlConfigId.DataTextField = "ConfigId";
                ddlConfigId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlConfigId.Items.Count == 1 && string.IsNullOrEmpty(ddlConfigId.Items[0].Text)))
                {
                    ddlConfigId.Visible = false;
                    ddlConfigId.Items.Clear();
                    ddlConfigId.Enabled = false;
                    ddlConfigId.BackColor = System.Drawing.Color.LightGray;
                    ddlConfigId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    ddlCombinationLookup.Visible = false;

                    return ddlConfigId.Visible;
                }
                else
                {
                    ddlConfigId.Visible = true;

                    ddlCombinationLookup.Visible = true;

                    // Enable the dropdown
                    ddlConfigId.Enabled = true;
                    ddlConfigId.BackColor = System.Drawing.Color.White;
                    ddlConfigId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlConfigId.CssClass += "filterable-dropdown";

                    ddlCombinationLookup.Visible = true;

                    ddlCombinationLookup.Load(itemid);

                    return ddlConfigId.Visible;
                }
            }
            return false;
        }

        private bool BindInventSizeId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveinventSizeId(itemid);
            DropDownList ddlInventSizeId = (DropDownList)e.FindControl("ddlInventSizeId");
            if (ddlInventSizeId != null)
            {
                ddlInventSizeId.DataSource = dt;
                ddlInventSizeId.DataValueField = "InventSizeId";
                ddlInventSizeId.DataTextField = "InventSizeId";
                ddlInventSizeId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventSizeId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSizeId.Items[0].Text)))
                {
                    ddlInventSizeId.Visible = false;
                    ddlInventSizeId.Items.Clear();
                    ddlInventSizeId.Enabled = false;
                    ddlInventSizeId.BackColor = System.Drawing.Color.LightGray;
                    ddlInventSizeId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlInventSizeId.Visible = true;
                    ddlInventSizeId.Enabled = true;
                    ddlInventSizeId.BackColor = System.Drawing.Color.White;
                    ddlInventSizeId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventSizeId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
        }

        private bool BindInventColorId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveinventColorId(itemid);
            DropDownList ddlInventColorId = (DropDownList)e.FindControl("ddlInventColorId");
            if (ddlInventColorId != null)
            {
                ddlInventColorId.DataSource = dt;
                ddlInventColorId.DataValueField = "InventColorId";
                ddlInventColorId.DataTextField = "InventColorId";
                ddlInventColorId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventColorId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventColorId.Items[0].Text)))
                {
                    ddlInventColorId.Visible = false;
                    ddlInventColorId.Items.Clear();
                    ddlInventColorId.Enabled = false;
                    ddlInventColorId.BackColor = System.Drawing.Color.LightGray;
                    ddlInventColorId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlInventColorId.Visible = true;
                    ddlInventColorId.Enabled = true;
                    ddlInventColorId.BackColor = System.Drawing.Color.White;
                    ddlInventColorId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventColorId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
        }

        private bool BindInventStyleId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveinvetstyleid(itemid);
            DropDownList ddlInventStyle = (DropDownList)e.FindControl("ddlInventStyleId");
            if (ddlInventStyle != null)
            {
                ddlInventStyle.DataSource = dt;
                ddlInventStyle.DataValueField = "InventStyleId";
                ddlInventStyle.DataTextField = "InventStyleId";
                ddlInventStyle.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventStyle.Items.Count == 1 && string.IsNullOrEmpty(ddlInventStyle.Items[0].Text)))
                {
                    ddlInventStyle.Visible = false;
                    ddlInventStyle.Items.Clear();
                    ddlInventStyle.Enabled = false;
                    ddlInventStyle.BackColor = System.Drawing.Color.LightGray;
                    ddlInventStyle.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlInventStyle.Visible = true;
                    ddlInventStyle.Enabled = true;
                    ddlInventStyle.BackColor = System.Drawing.Color.White;
                    ddlInventStyle.Items.Insert(0, new ListItem("", string.Empty));
                    ddlInventStyle.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
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
        private bool BindWmsPalletId(string itemid, GridViewRow e)
        {
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrievewmsPalletId(itemid);
            DropDownList ddlWmsPalletId = (DropDownList)e.FindControl("ddlWMSPalletId");
            if (ddlWmsPalletId != null)
            {
                ddlWmsPalletId.DataSource = dt;
                ddlWmsPalletId.DataValueField = "WmsPalletId";
                ddlWmsPalletId.DataTextField = "WmsPalletId";
                ddlWmsPalletId.DataBind();

                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlWmsPalletId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsPalletId.Items[0].Text)))
                {
                    ddlWmsPalletId.Visible = false;
                    ddlWmsPalletId.Items.Clear();
                    ddlWmsPalletId.Enabled = false;
                    ddlWmsPalletId.BackColor = System.Drawing.Color.LightGray;
                    ddlWmsPalletId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
                    return false;
                }
                else
                {
                    ddlWmsPalletId.Visible = true;
                    ddlWmsPalletId.Enabled = true;
                    ddlWmsPalletId.BackColor = System.Drawing.Color.White;
                    ddlWmsPalletId.Items.Insert(0, new ListItem("", string.Empty));
                    ddlWmsPalletId.CssClass += "filterable-dropdown";
                    return true;
                }
            }
            return false;
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
                SysOperationResult_BOL operationResult_BOL = purchaselines.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                reBindGrid();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select at least one Purchase Order Line to proceed.');", true);
            }
        }

        protected void btnNew_Grid_Click(object sender, EventArgs e)
        {
            DataTable dt = purchaselines.retrieveAll(Session["PurchaseOrderId"] as string);
            DataRow newRow = dt.NewRow();
            dt.Rows.InsertAt(newRow, 0);
            gridView.EditIndex = 0;
            gridView.DataSource = dt;
            gridView.DataBind();

            // Mark this as Add mode
            

        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
                if (lblQuantity != null && !string.IsNullOrEmpty(lblQuantity.Text))
                {
                    decimal qty;
                    if (decimal.TryParse(lblQuantity.Text, out qty))
                    {
                        lblQuantity.Text = qty.ToString("N2"); // always 2 decimals
                    }
                }
                Label lblInventoryQuantity = (Label)e.Row.FindControl("lblInventoryQuantity");
                if (lblInventoryQuantity != null && !string.IsNullOrEmpty(lblInventoryQuantity.Text))
                {
                    decimal invQty;
                    if (decimal.TryParse(lblInventoryQuantity.Text, out invQty))
                    {
                        lblInventoryQuantity.Text = invQty.ToString("N2"); // always 2 decimals
                    }
                }
                Label lblDeliverRemainder = (Label)e.Row.FindControl("lblDeliverRemainder");
                if (lblDeliverRemainder != null && !string.IsNullOrEmpty(lblDeliverRemainder.Text))
                {
                    decimal invQty;
                    if (decimal.TryParse(lblDeliverRemainder.Text, out invQty))
                    {
                        lblDeliverRemainder.Text = invQty.ToString("N2"); // always 2 decimals
                    }
                }

                Label lblBudgetCheckResults = (Label)e.Row.FindControl("lblBudgetCheckResults");
                if (lblBudgetCheckResults != null)
                {
                    if (lblBudgetCheckResults.Text == "Budget check not performed")
                    {
                        lblBudgetCheckResults.Text = "";
                    }
                    else if (lblBudgetCheckResults.Text == "Budget check performed")
                    {
                        lblBudgetCheckResults.Text = "✅";
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    var dataItem = (DataRowView)e.Row.DataItem;

                    DropDownList ddlItemid = (DropDownList)e.Row.FindControl("ddlItemId");
                    if (ddlItemid != null)
                    {
                        DataTable dt = purchaselines.retrieveitemid();
                        dt.Columns.Add("DisplayText", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["DisplayText"] = row["ItemId"] + " - " + row["ItemName"];
                        }

                        ddlItemid.DataSource = dt;
                        ddlItemid.DataValueField = "ItemId";
                        ddlItemid.DataTextField = "DisplayText";
                        ddlItemid.DataBind();
                        ddlItemid.Items.Insert(0, new ListItem("", String.Empty));
                        ddlItemid.Attributes["onchange"] = "fetchProductName(this)";
                        ddlItemid.CssClass += " filterable-dropdown";
                    }

                    DropDownList ddlSiteId = (DropDownList)e.Row.FindControl("ddlSiteId");
                    if (ddlSiteId != null)
                    {
                        DataTable dt = purchaselines.retrievesiteId();
                        dt.Columns.Add("DisplayText", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["DisplayText"] = row["InventSiteId"] + " - " + row["SiteName"];
                        }

                        ddlSiteId.DataSource = dt;
                        ddlSiteId.DataValueField = "InventSiteId";
                        ddlSiteId.DataTextField = "DisplayText";
                        ddlSiteId.DataBind();
                        ddlSiteId.Items.Insert(0, new ListItem("", String.Empty));
                        ddlSiteId.CssClass += " filterable-dropdown";
                    }

                    DropDownList ddlWarehouse = (DropDownList)e.Row.FindControl("ddlWarehouse");
                    if (ddlWarehouse != null)
                    {
                        DataTable dt = purchaselines.retrievelocationId();

                        dt.Columns.Add("DisplayText", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["DisplayText"] = row["InventLocationId"] + " - " + row["LocationName"];
                        }

                        ddlWarehouse.DataSource = dt;
                        ddlWarehouse.DataValueField = "InventLocationId";
                        ddlWarehouse.DataTextField = "DisplayText";
                        ddlWarehouse.DataBind();
                        ddlWarehouse.Items.Insert(0, new ListItem("", String.Empty));
                        ddlWarehouse.CssClass += " filterable-dropdown";
                    }

                    // 🔹 Populate Site and Warehouse dropdowns with session values
                    DropDownList ListpageddlSiteId = (DropDownList)e.Row.FindControl("ddlSiteId");
                    DropDownList ListpageddlWarehouse = (DropDownList)e.Row.FindControl("ddlWarehouse");

                    if (ListpageddlSiteId != null && Session["ListPageSiteId"] != null)
                        ListpageddlSiteId.SelectedValue = Session["ListPageSiteId"].ToString();

                    if (ListpageddlWarehouse != null && Session["ListPageLocationId"] != null)
                        ListpageddlWarehouse.SelectedValue = Session["ListPageLocationId"].ToString();

                    TextBox txtItemIdNewMode = (TextBox)e.Row.FindControl("txtItemId");
                    txtItemIdNewMode.Visible = false;

                    TextBox txtProductNameNewMode = (TextBox)e.Row.FindControl("txtProductNameEdit");
                    txtProductNameNewMode.Visible = false;

                    TextBox txtQuantitynew = (TextBox)e.Row.FindControl("txtQuantity");
                    txtQuantitynew.Text = "0.00";
                    txtQuantitynew.Style["text-align"] = "right";

                    TextBox txtUnitPricenew = (TextBox)e.Row.FindControl("txtUnitPrice");
                    txtUnitPricenew.Text = "0.00";
                    txtUnitPricenew.Style["text-align"] = "right";

                    TextBox txtDiscountnew = (TextBox)e.Row.FindControl("txtDiscount");
                    txtDiscountnew.Text = "0.00";
                    txtDiscountnew.Style["text-align"] = "right";

                    TextBox txtDiscountPercentNew = (TextBox)e.Row.FindControl("txtDiscountPercent");
                    txtDiscountPercentNew.Text = "0.00";
                    txtDiscountPercentNew.Style["text-align"] = "right";

                    TextBox txtNetAmountNew = (TextBox)e.Row.FindControl("txtNetAmount");
                    txtNetAmountNew.Text = "0.00";
                    txtNetAmountNew.Style["text-align"] = "right";

                    // 🔹 CASE 1: Editing an existing row
                    if (dataItem["RecId"] != DBNull.Value && Convert.ToInt64(dataItem["RecId"]) > 0)
                    {
                        TextBox txtLineNumber = (TextBox)e.Row.FindControl("txtLineNumber");
                        if (txtLineNumber != null && dataItem["LineNumber"] != DBNull.Value)
                        {
                            txtLineNumber.Text = dataItem["LineNumber"].ToString();
                            txtLineNumber.Enabled = false;
                        }


                        //dimensions being hiddein
                        DropDownList ddlConfigId = (DropDownList)e.Row.FindControl("ddlConfigId");
                        ddlConfigId.Enabled = false;
                        ddlConfigId.Visible = false;
                        DropDownList ddlInventColorId = (DropDownList)e.Row.FindControl("ddlInventColorId");
                        ddlInventColorId.Enabled = false;
                        ddlInventColorId.Visible = false;
                        DropDownList ddlInventSizeId = (DropDownList)e.Row.FindControl("ddlInventSizeId");
                        ddlInventSizeId.Enabled = false;
                        ddlInventSizeId.Visible = false;
                        DropDownList ddlInventStyleId = (DropDownList)e.Row.FindControl("ddlInventStyleId");
                        ddlInventStyleId.Enabled = false;
                        ddlInventStyleId.Visible = false;
                        DropDownList ddlInventBatchId = (DropDownList)e.Row.FindControl("ddlInventBatchId");
                        ddlInventBatchId.Enabled = false;
                        ddlInventBatchId.Visible = false;
                        DropDownList ddlWMSLocationId = (DropDownList)e.Row.FindControl("ddlWMSLocationId");
                        ddlWMSLocationId.Enabled = false;
                        ddlWMSLocationId.Visible = false;
                        DropDownList ddlSerialNo = (DropDownList)e.Row.FindControl("ddlInventSerialId");
                        ddlSerialNo.Enabled = false;
                        ddlSerialNo.Visible = false;

                       


                        DropDownList ddlItemId = (DropDownList)e.Row.FindControl("ddlItemId");
                        if (ddlItemId != null && dataItem["ItemId"] != DBNull.Value)
                        {
                            ddlItemId.Visible = false;
                        }

                        TextBox txtItemId = (TextBox)e.Row.FindControl("txtItemId");
                        txtItemId.Enabled = false;
                        txtItemId.Visible = true;


                        TextBox txtProductName = (TextBox)e.Row.FindControl("txtProductName");
                        if (txtProductName != null && dataItem["ItemName"] != DBNull.Value)
                        {
                            txtProductName.Visible = false;
                        }

                        TextBox txtProductNameEdit = (TextBox)e.Row.FindControl("txtProductNameEdit");
                        txtProductNameEdit.Enabled = false;
                        txtProductNameEdit.Visible = true;

                        TextBox txtVariantNumber = (TextBox)e.Row.FindControl("txtVariantNumber");
                        txtVariantNumber.Enabled = false;


                        TextBox txtProcurementCategory = (TextBox)e.Row.FindControl("txtProcurementCategory");
                        if (txtProcurementCategory != null && dataItem["ProcurementCategory"] != DBNull.Value)
                        {
                            txtProcurementCategory.Text = dataItem["ProcurementCategory"].ToString();
                            txtProcurementCategory.Enabled = false;
                        }

                        TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                        if (txtQuantity != null && dataItem["PurchQty"] != DBNull.Value)
                        {
                            // Parse and format with 2 decimal places
                            if (decimal.TryParse(dataItem["PurchQty"].ToString(), out decimal qty))
                            {
                                txtQuantity.Text = qty.ToString("N2");
                            }
                            else
                            {
                                txtQuantity.Text = "0.00";
                            }

                            // Align text to the right (for safety even though markup already has it)
                            txtQuantity.Style["text-align"] = "right";
                        }

                        TextBox txtUnit = (TextBox)e.Row.FindControl("txtUnit");
                        txtUnit.Text = dataItem["PurchUnitofMeasureCode"].ToString();

                        // --- Unit Price ---
                        TextBox txtUnitPrice = (TextBox)e.Row.FindControl("txtUnitPrice");
                        if (txtUnitPrice != null && dataItem["PurchPrice"] != DBNull.Value)
                        {
                            if (decimal.TryParse(dataItem["PurchPrice"].ToString(), out decimal price))
                                txtUnitPrice.Text = price.ToString("N2");
                            else
                                txtUnitPrice.Text = "0.00";

                            txtUnitPrice.Style["text-align"] = "right";
                        }

                        // --- Discount ---
                        TextBox txtDiscount = (TextBox)e.Row.FindControl("txtDiscount");
                        if (txtDiscount != null && dataItem["LineDiscount"] != DBNull.Value)
                        {
                            if (decimal.TryParse(dataItem["LineDiscount"].ToString(), out decimal discount))
                                txtDiscount.Text = discount.ToString("N2");
                            else
                                txtDiscount.Text = "0.00";

                            txtDiscount.Style["text-align"] = "right";
                        }

                        // --- Discount Percent ---
                        TextBox txtDiscountPercent = (TextBox)e.Row.FindControl("txtDiscountPercent");
                        if (txtDiscountPercent != null && dataItem["LineDiscountPercent"] != DBNull.Value)
                        {
                            if (decimal.TryParse(dataItem["LineDiscountPercent"].ToString(), out decimal discountPct))
                                txtDiscountPercent.Text = discountPct.ToString("N2");
                            else
                                txtDiscountPercent.Text = "0.00";

                            txtDiscountPercent.Style["text-align"] = "right";
                        }

                        // --- Net Amount ---
                        TextBox txtNetAmount = (TextBox)e.Row.FindControl("txtNetAmount");
                        if (txtNetAmount != null && dataItem["LineAmount"] != DBNull.Value)
                        {
                            if (decimal.TryParse(dataItem["LineAmount"].ToString(), out decimal netAmt))
                                txtNetAmount.Text = netAmt.ToString("N2");
                            else
                                txtNetAmount.Text = "0.00";

                            txtNetAmount.Style["text-align"] = "right";
                        }

                        DropDownList ddlSiteIDEdit = (DropDownList)e.Row.FindControl("ddlSiteId");
                        ddlSiteIDEdit.SelectedValue = dataItem["InventSiteId"].ToString();
                        ddlSiteIDEdit.Enabled = false;
                        ddlSiteIDEdit.Visible = false;

                        DropDownList ddlWarehouseEdit = (DropDownList)e.Row.FindControl("ddlWarehouse");
                        ddlWarehouseEdit.SelectedValue = dataItem["InventLocationID"].ToString();
                        ddlWarehouseEdit.Enabled = false;
                        ddlWarehouseEdit.Visible = false;


                        string configId = dataItem["ConfigId"] != DBNull.Value ? dataItem["ConfigId"].ToString() : string.Empty;
                        string inventColorId = dataItem["InventColorId"] != DBNull.Value ? dataItem["InventColorId"].ToString() : string.Empty;
                        string inventSizeId = dataItem["InventSizeId"] != DBNull.Value ? dataItem["InventSizeId"].ToString() : string.Empty;
                        string inventStyleId = dataItem["InventStyleId"] != DBNull.Value ? dataItem["InventStyleId"].ToString() : string.Empty;
                        string inventBatchId = dataItem["InventBatchId"] != DBNull.Value ? dataItem["InventBatchId"].ToString() : string.Empty;
                        string inventSerialId = dataItem["InventSerialId"] != DBNull.Value ? dataItem["InventSerialId"].ToString() : string.Empty;
                        string wmsLocationId = dataItem["WMSLocationId"] != DBNull.Value ? dataItem["WMSLocationId"].ToString() : string.Empty;

                        // store in session
                        Session["EditModeConfigId"] = configId;
                        Session["EditModeInventColorId"] = inventColorId;
                        Session["EditModeInventSizeId"] = inventSizeId;
                        Session["EditModeInventStyleId"] = inventStyleId;
                        Session["EditModeInventBatchId"] = inventBatchId;
                        Session["EditModeInventSerialId"] = inventSerialId;
                        Session["EditModeWMSLocationId"] = wmsLocationId;
                    }
                }
            }
        }

        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
            {
                { "Batch Number", ("ddlInventBatchId", false) },
                { "WMS Location", ("ddlWMSLocationId", false) },
                { "Serial Number", ("ddlInventSerialId", false) },
                { "Configuration", ("ddlConfigId", false) },
                { "Combinations", ("", false) },
                { "Size", ("ddlInventSizeId", false) },
                { "Color", ("ddlInventColorId", false) },
                { "Style", ("ddlInventStyleId", false) },
                //{ "WMS Pallet", ("ddlWMSPalletId", false) }
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
            gridView.EditIndex = -1;
            reBindGrid();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL result = new SysOperationResult_BOL();
            GridViewRow gridRow = gridView.Rows[gridView.EditIndex];
            bool allGood = true;
            try
            {
                Label lblRecId = (Label)gridRow.FindControl("lblRecId");
                long recId = 0;
                if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
                {
                    long.TryParse(lblRecId.Text, out recId);
                }


                if (recId == 0)
                {

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Itemid");
                    dt.Columns.Add("PurchaseOrderId");
                    dt.Columns.Add("PurchQty");
                    dt.Columns.Add("PurchPrice");
                    dt.Columns.Add("LineDisc");
                    dt.Columns.Add("LinePercent");
                    dt.Columns.Add("NetAmount");
                    dt.Columns.Add("InventBatchId");
                    dt.Columns.Add("WmsLocationId");
                    //dt.Columns.Add("WmsPalletId");
                    dt.Columns.Add("InventSerialId");
                    dt.Columns.Add("InventLocationID");
                    dt.Columns.Add("ConfigId");
                    dt.Columns.Add("InventSizeId");
                    dt.Columns.Add("InventColorId");
                    dt.Columns.Add("InventSiteID");
                    dt.Columns.Add("InventStyle");

                    DataRow row = dt.NewRow();

                    DropDownList ddlItemId = (DropDownList)gridRow.FindControl("ddlItemId");
                    TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
                    TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
                    TextBox txtDiscount = (TextBox)gridRow.FindControl("txtDiscount");
                    TextBox txtDiscountPercent = (TextBox)gridRow.FindControl("txtDiscountPercent");
                    TextBox txtNetAmount = (TextBox)gridRow.FindControl("txtNetAmount");





                    DropDownList ddlConfigId = (DropDownList)gridRow.FindControl("ddlConfigId");
                    DropDownList ddlColorId = (DropDownList)gridRow.FindControl("ddlInventColorId");
                    DropDownList ddlSizeId = (DropDownList)gridRow.FindControl("ddlInventSizeId");
                    DropDownList ddlStyleId = (DropDownList)gridRow.FindControl("ddlInventStyleId");
                    DropDownList ddlInventSiteId = (DropDownList)gridRow.FindControl("ddlSiteId");
                    DropDownList ddlWarehouse = (DropDownList)gridRow.FindControl("ddlWarehouse");
                    DropDownList ddlInventBatchId = (DropDownList)gridRow.FindControl("ddlInventBatchId");
                    DropDownList ddlWMSLocationId = (DropDownList)gridRow.FindControl("ddlWMSLocationId");
                    //DropDownList ddlWmsPalletId = (DropDownList)gridRow.FindControl("ddlWMSPalletId");
                    DropDownList ddlInventSerialId = (DropDownList)gridRow.FindControl("ddlInventSerialId");

                    row["Itemid"] = ddlItemId.SelectedValue;
                    row["PurchaseOrderId"] = Session["PurchaseOrderId"] as string;
                    row["PurchQty"] = txtQuantity.Text.Trim();
                    row["PurchPrice"] = txtUnitPrice.Text.Trim();
                    row["LineDisc"] = txtDiscount.Text.Trim();
                    row["LinePercent"] = txtDiscountPercent.Text.Trim();
                    row["NetAmount"] = txtNetAmount.Text.Trim();
                    row["InventBatchId"] = ddlInventBatchId.SelectedValue;
                    row["WmsLocationId"] = ddlWMSLocationId.SelectedValue;
                    //row["WmsPalletId"] = ddlWmsPalletId.SelectedValue;
                    row["InventSerialId"] = ddlInventSerialId.SelectedValue;
                    row["InventLocationID"] = ddlWarehouse.SelectedValue;
                    row["ConfigId"] = ddlConfigId.SelectedValue;
                    row["InventSizeId"] = ddlSizeId.SelectedValue;
                    row["InventColorId"] = ddlColorId.SelectedValue;
                    row["InventSiteID"] = ddlInventSiteId.SelectedValue;
                    row["InventStyle"] = ddlStyleId.SelectedValue;

                    dt.Rows.Add(row);

                    Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
                {
                    { "Batch Number", ("ddlInventBatchId", BindInventBatchId(ddlItemId.SelectedValue, gridRow)) },
                    { "WMS Location", ("ddlWMSLocationId", BindWmsLocationId(ddlItemId.SelectedValue, gridRow)) },
                    { "Serial Number", ("ddlInventSerialId", BindInventSerialId(ddlItemId.SelectedValue, gridRow)) },
                    { "Configuration", ("ddlConfigId", BindConfigId(ddlItemId.SelectedValue, gridRow)) },
                    { "Combinations", ("", BindCombination(ddlItemId.SelectedValue, gridRow)) },
                    { "Size", ("ddlInventSizeId", BindInventSizeId(ddlItemId.SelectedValue, gridRow)) },
                    { "Color", ("ddlInventColorId", BindInventColorId(ddlItemId.SelectedValue, gridRow)) },
                    { "Style", ("ddlInventStyleId", BindInventStyleId(ddlItemId.SelectedValue, gridRow)) }
                    //{ "WMS Pallet", ("ddlWMSPalletId", BindWmsPalletId(ddlItemId.SelectedValue, gridRow)) }
                };

                    ddlInventBatchId.SelectedValue = row["InventBatchId"]?.ToString();
                    ddlWMSLocationId.SelectedValue = row["WmsLocationId"]?.ToString();
                    //ddlWmsPalletId.SelectedValue = row["WmsPalletId"]?.ToString();
                    ddlInventSerialId.SelectedValue = row["InventSerialId"]?.ToString();
                    ddlConfigId.SelectedValue = row["ConfigId"]?.ToString();
                    ddlSizeId.SelectedValue = row["InventSizeId"]?.ToString();
                    ddlColorId.SelectedValue = row["InventColorId"]?.ToString();
                    ddlStyleId.SelectedValue = row["InventStyle"]?.ToString();

                    if (string.IsNullOrEmpty(ddlItemId.SelectedValue) || string.IsNullOrEmpty(txtQuantity.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingVisibleFields", $"alert('Please Fill all the Fields');", true);
                        return;
                    }

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
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingVisibleFields", $"alert('Please Fill all the Fields');", true);
                                    allGood = false;
                                    return;
                                }
                            }
                        }
                    }

                    if (allGood)
                    {
                        PurchaseOrderLines purchaseorderline = new PurchaseOrderLines();
                        result = purchaseorderline.create(dt);

                        if (string.IsNullOrWhiteSpace(result.Message))
                        {
                            result.isSuccess = false;
                            result.Message = "Update has been cancelled";
                            result.AlertType = AlertType.Error.ToString();
                        }

                        NotificationMessage.showMessage(result);

                        if (result.isSuccess)
                        {
                            gridView.EditIndex = -1;
                            reBindGrid();
                        }
                    }
                }

                if (recId != 0)
                {
                    DataTable dtnew = new DataTable();
                    dtnew.Columns.Add("PurchQty", typeof(decimal));
                    dtnew.Columns.Add("PurchPrice", typeof(decimal));
                    dtnew.Columns.Add("LineDiscount", typeof(decimal));
                    dtnew.Columns.Add("LineDiscountPercent", typeof(decimal));
                    dtnew.Columns.Add("RecId", typeof(long));
                    dtnew.Columns.Add("LineAmount", typeof(decimal));
                    dtnew.Columns.Add("PurchaseOrderId", typeof(string));

                    //dtnew.Columns.Add("InventBatchId");
                    //dtnew.Columns.Add("WmsLocationId");
                    //dtnew.Columns.Add("WmsPalletId");
                    //dtnew.Columns.Add("InventSerialId");
                    //dtnew.Columns.Add("InventLocationId");
                    //dtnew.Columns.Add("ConfigId");
                    //dtnew.Columns.Add("InventSizeId");
                    //dtnew.Columns.Add("InventColorId");
                    //dtnew.Columns.Add("InventSiteId");
                    //dtnew.Columns.Add("InventStyle");
                    //dtnew.Columns.Add("InventLocation");

                    DataRow newrow = dtnew.NewRow();

                    TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
                    TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
                    TextBox txtDiscount = (TextBox)gridRow.FindControl("txtDiscount");
                    TextBox txtDiscountPercent = (TextBox)gridRow.FindControl("txtDiscountPercent");
                    TextBox txtNetAmount = (TextBox)gridRow.FindControl("txtNetAmount");

                    DropDownList ddlInventSiteId = (DropDownList)gridRow.FindControl("ddlSiteId");
                    DropDownList ddlInventLocationId = (DropDownList)gridRow.FindControl("ddlWarehouse");

                    newrow["PurchaseOrderId"] = Session["PurchaseOrderId"] as string;
                    newrow["RecId"] = recId;
                    newrow["PurchQty"] = txtQuantity.Text.Trim();
                    newrow["PurchPrice"] = txtUnitPrice.Text.Trim();
                    newrow["LineDiscount"] = txtDiscount.Text.Trim();
                    newrow["LineDiscountPercent"] = txtDiscountPercent.Text.Trim();
                    newrow["LineAmount"] = txtNetAmount.Text.Trim();

                    //newrow["InventBatchId"] = Session["EditModeInventBatchId"] as string;
                    //newrow["WmsLocationId"] = Session["EditModeWMSLocationId"] as string;
                    //// newrow["WmsPalletId"] = Session["EditModeWmsPalletId"] as string;
                    //newrow["InventSerialId"] = Session["EditModeInventSerialId"] as string;
                    //newrow["InventLocationID"] = ddlInventLocationId.SelectedValue;
                    //newrow["ConfigId"] = Session["EditModeConfigId"] as string;
                    //newrow["InventSizeId"] = Session["EditModeInventSizeId"] as string;
                    //newrow["InventColorId"] = Session["EditModeInventColorId"] as string;
                    //newrow["InventSiteID"] = ddlInventSiteId.SelectedValue;
                    //newrow["InventStyle"] = Session["EditModeInventStyleId"] as string;


                    dtnew.Rows.Add(newrow);

                    result = purchaselines.update(dtnew);


                    if (string.IsNullOrWhiteSpace(result.Message))
                    {
                        result.isSuccess = false;
                        result.Message = "Error while updating Purchase order line record.";
                        result.AlertType = AlertType.Error.ToString();
                    }

                    NotificationMessage.showMessage(result);

                    if (result.isSuccess)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = "An error occurred: " + ex.Message;
                result.AlertType = AlertType.Error.ToString();
                NotificationMessage.showMessage(result);
            }
        }

        private string GetLabelText(Control row, string labelId)
        {
            var label = row.FindControl(labelId) as Label;
            var text = label?.Text?.Trim();
            return string.IsNullOrEmpty(text) ? "\uFEFF" : text;

        }

        private void showFinancialDimension(long _defaultDimensionRecId)
        {
            Int64 workerDimension = 0;
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();

            if (workerDimension == 0)
            {
                try
                {
                    DataContract[] dimensions = finDim.retrieveActiveDimensions();

                    if (dimensions != null && dimensions.Length > 0)
                    {
                        // Container where you want to add rows
                        // (put a <div runat="server" id="financialDimensionsContainer"></div> in your .aspx)
                        financialDimensionsContainer.Controls.Clear();

                        int colCount = 0;
                        HtmlGenericControl rowDiv = null;

                        foreach (var dim in dimensions)
                        {
                            if (colCount % 2 == 0)
                            {
                                // Start a new row after every two blocks
                                rowDiv = new HtmlGenericControl("div");
                                rowDiv.Attributes["class"] = "info-row";
                                financialDimensionsContainer.Controls.Add(rowDiv);
                            }

                            // Create info-block
                            HtmlGenericControl blockDiv = new HtmlGenericControl("div");
                            blockDiv.Attributes["class"] = "info-block";

                            // Strong title
                            HtmlGenericControl strong = new HtmlGenericControl("strong");
                            strong.InnerText = dim.Code;   // e.g. Business Unit / Cost Center
                            blockDiv.Controls.Add(strong);

                            // Span wrapper
                            HtmlGenericControl span = new HtmlGenericControl("span");

                            //// Label
                            //Label lbl = new Label();
                            //lbl.ID = $"FinancialDimension{dim.Code}";
                            //lbl.Text = "N/A";
                            //span.Controls.Add(lbl);

                            // Dropdown
                            DropDownList ddl = new DropDownList();
                            ddl.ID = $"ddlFinancialDimension{dim.Code}";
                            ddl.CssClass = "filterable-dropdown";
                            ddl.Visible = true;

                            // Populate dropdown from SOAP lookup
                            DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                            ddl.Items.Add(new ListItem("", ""));
                            foreach (DataRow row in dimeVal.Rows)
                            {
                                string value = row["Value1"].ToString();
                                string valueName = row["Value2"].ToString();
                                ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                            }

                            span.Controls.Add(ddl);
                            blockDiv.Controls.Add(span);

                            // Add block to row
                            rowDiv.Controls.Add(blockDiv);

                            colCount++;
                        }
                        bindEmployeeDimension(_defaultDimensionRecId);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic dimensions", ex);
                }
            }

        }
        protected void bindEmployeeDimension(long __defaultDimensionRecId)
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();

            long empDimension = __defaultDimensionRecId;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(empDimension);
            foreach (DataContract dataContract in dataContracts)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                DropDownList ddl = financialDimensionsContainer.FindControl("ddlFinancialDimension" + dimName) as DropDownList;
                TextBox txt = financialDimensionsContainer.FindControl("txt_" + dimName) as TextBox;

                if (ddl != null)
                {
                    // Check if value exists in dropdown items
                    if (!string.IsNullOrEmpty(dimValue) && ddl.Items.FindByValue(dimValue) != null)
                    {
                        ddl.SelectedValue = dimValue;
                        if (txt != null)
                            txt.Text = dimDescription;
                    }

                }
            }
        }
        private void PopulateLinesDetails(GridViewRow row)
        {
            try
            {
                // Populate REQUEST FOR QUOTATION
                txtRFQNumber.Text = GetLabelText(row, "lblRfqNumber");
                txtRFQReplyNumber.Text = GetLabelText(row, "lblRfqReplyNumber");
                txtRFQLineNumber.Text = GetLabelText(row, "lblRfqLineNumber");

                // Populate ORDER LINE
                txtProcurementCategory.Text = GetLabelText(row, "lblOrderProcurementCategory");
                txtProductName.Text = GetLabelText(row, "lblOrderProductName");
                txtitemdescription.Text = GetLabelText(row, "lblItemDescription");

                // Populate PURCHASE REQUISITION
                txtPurchaseRequisition.Text = GetLabelText(row, "lblPrRequisition");
                txtRequisitionProductName.Text = GetLabelText(row, "lblPrProductName");
                txtSupplierAuxId.Text = GetLabelText(row, "lblPrSupplierAuxId");

                // Populate INTERCOMPANY
                txtIntercompanyOrigin.Text = GetLabelText(row, "lblIntercompanyOrigin");

                // Populate REFERENCE
                txtReferenceExternal.Text = GetLabelText(row, "lblReferenceExternal");
                txtReferenceOrigin.Text = GetLabelText(row, "lblReferenceOrigin");

                // Populate DELIVERY REFERENCE
                txtDeliveryCustomerRequisition.Text = GetLabelText(row, "lblDeliveryCustomerRequisition");
                txtDeliveryCustomerReference.Text = GetLabelText(row, "lblDeliveryCustomerReference");
                //txtDeliveryBudgetReservation.Text = GetLabelText(row, "lblDeliveryBudgetReservation");

                // Populate STATUS
                txtLineStatus.Text = GetLabelText(row, "lblStatusLineStatus");
                chkStopped.Checked = GetControlChecked(row, "chkStatusStopped");
                chkPreventPartial.Checked = GetControlChecked(row, "chkStatusPreventPartial");
                txtState.Text = GetLabelText(row, "lblStatusState");
                //txtQualityOrderStatus.Text = GetLabelText(row, "lblStatusQualityOrder");
                chkFinalized.Checked = GetControlChecked(row, "chkStatusFinalized");
                chkAddedByPOS.Checked = GetControlChecked(row, "chkStatusAddedByPOS");

                // Populate SETUP TAB
                setupLotId.Text = GetLabelText(row, "lblSetupLotId");
                lblMatchingPolicy.Text = GetLabelText(row, "lblSetupMatchingPolicy");
                lblReturnAction.Text = GetLabelText(row, "lblSetupReturnAction");
                chkScrap.Checked = GetControlChecked(row, "chkSetupScrap");
                //ddlItemSalestaxGrouplineDetail.Text = GetLabelText(row, "lblSetupItemSalesTaxGroup");
                //ddlSalesTaxGrouplineDetail.Text = GetLabelText(row, "lblSetupSalesTaxGroup");
                BindItemSalesTaxGroupLineDetail();
                BindSalesTaxGroupLineDetail();

                string itemSalesTaxGroupId = GetLabelText(row, "lblSetupItemSalesTaxGroup");
                if (ddlItemSalestaxGrouplineDetail.Items.FindByValue(itemSalesTaxGroupId) != null)
                {
                    ddlItemSalestaxGrouplineDetail.SelectedValue = itemSalesTaxGroupId;
                }

                string salesTaxGroupId = GetLabelText(row, "lblSetupSalesTaxGroup");
                if (ddlSalesTaxGrouplineDetail.Items.FindByValue(salesTaxGroupId) != null)
                {
                    ddlSalesTaxGrouplineDetail.SelectedValue = salesTaxGroupId;
                }








                //txtItemGroup.Text = GetLabelText(row, "lblSetupItemSalesTaxGroup");
                //ddlSaleTaxGroup.Text = GetLabelText(row, "lblSetupSalesTaxGroup");
                txt1099Amount.Text = GetLabelText(row, "lblSetup1099Amount");
                txt1099StateAmount.Text = GetLabelText(row, "lblSetup1099StateAmount");
                lbl1099Box.Text = GetLabelText(row, "lbl1099Box");
                txtLedgerAccount.Text = GetLabelText(row, "txtSetupLedgerAccount");
                if (Session["CreatedDate"] != null)
                {
                    DateTime createdDate;
                    if (DateTime.TryParse(Session["CreatedDate"].ToString(), out createdDate))
                    {
                        lblCreatedDateTime.Text = createdDate.ToString("M/d/yyyy");
                    }
                    else if (DateTime.TryParseExact(Session["CreatedDate"].ToString(),
                               "yyyy-MM-ddTHH:mm:ss", // adjust if needed
                               CultureInfo.InvariantCulture,
                               DateTimeStyles.None,
                               out createdDate))
                    {
                        lblCreatedDateTime.Text = createdDate.ToString("M/d/yyyy");
                    }
                    else
                    {
                        lblCreatedDateTime.Text = ""; // or display a default/error value
                    }
                }
                else
                {
                    lblCreatedDateTime.Text = ""; // session is null
                }
                txtInventoryQuantity.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(row, "lblQuantity")));
                txtInventoryRemainder.Text = GetLabelText(row, "lblSetupInvoiceRemainder");
                if (Session["RequestedReceiptDate"] != null)
                {
                    DateTime receiptDate;
                    // Try to parse the value safely
                    if (DateTime.TryParse(Session["RequestedReceiptDate"].ToString(), out receiptDate))
                    {
                        txtConfirmedReceiptDate.Text = receiptDate.ToString("M/d/yyyy");
                    }
                    else
                    {
                        // Handle invalid date format
                        txtConfirmedReceiptDate.Text = ""; // or show an error message
                    }
                }
                else
                {
                    txtConfirmedReceiptDate.Text = ""; // session is null
                }
                txtDeliveryType.Text = GetLabelText(row, "lblSetupDeliveryType");

                // Populate ADDRESS TAB
                txtDeliveryName.Text = GetLabelText(row, "lblDeliveryAddress");
                txtDeliveryAddress.Text = GetLabelText(row, "lblDeliveryAddress");
                txtAddress.Text = GetLabelText(row, "lblAddressFull");
                txtServiceAddress.Text = GetLabelText(row, "lblServiceCustomerRequisition");
                txtCustomerRequisition.Text = GetLabelText(row, "lblServiceCustomerReference");
                txtCustomerReference.Text = GetLabelText(row, "lblAttentionInformation");
                txtAttentionInformation.Text = GetLabelText(row, "lblAttentionInformation");
                txtRequester.Text = Session["SubmittedBy"] as string;

                // Populate PRODUCT TAB
                //txtConfiguration.Text = GetLabelText(row, "lblConfigId");
                //txtSize.Text = GetLabelText(row, "lblInventSizeId");
                //txtColor.Text = GetLabelText(row, "lblInventColorId");
                //txtStyle.Text = GetLabelText(row, "lblInventStyleId");
                txtVersion.Text = GetLabelText(row, "lblProductVersion");
                //txtSerialNumber.Text = GetLabelText(row, "lblProductSerialNumber");
                txtOwner.Text = GetLabelText(row, "lblProductOwner");
                //txtlocationWarehouse.Text = GetLabelText(row, "lblWarehouse");
                txtLicensePlate.Text = GetLabelText(row, "lblProductLicensePlate");
                txtInventoryStatus.Text = GetLabelText(row, "lblProductInventoryStatus");
                //txtBatchNumber.Text = GetLabelText(row, "lblInventBatchId");
                //txtSite.Text = GetLabelText(row, "lblSite");
                //txtWarehouse.Text = GetLabelText(row, "lblWarehouse");
                txtPlannedOrderNumber.Text = GetLabelText(row, "lblPlannedOrderNumber");
                txtMasterPlan.Text = GetLabelText(row, "lblPlannedOrderMasterPlan");
                txtReferenceType.Text = GetLabelText(row, "lblItemReferenceType");
                txtReferenceNumber.Text = GetLabelText(row, "lblItemReferenceNumber");
                txtReferenceLot.Text = GetLabelText(row, "lblItemReferenceLot");

                string ItemId = GetLabelText(row, "lblItemId");


                BindSiteLineDetail();
                BindConfigurationforLineDetail(ItemId);
                BindColorLineDetail(ItemId);
                BindSizeLineDetail(ItemId);
                BindStyleLineDetail(ItemId);
                BindWarehouseLineDetail(ItemId);
                BindBatchLineDetail(ItemId);
                BindWmsLocationLineDetail(ItemId);
                BindInventSerialLineDetail(ItemId);


                // PRODUCT DIMENSIONS (dropdowns)
                string configId = GetLabelText(row, "lblConfigId");
                if (ddlConfigurationLineDetail.Items.FindByValue(configId) != null)
                {
                    ddlConfigurationLineDetail.SelectedValue = configId;
                }
                else
                {
                    ddlConfigurationLineDetail.Attributes["readonly"] = "readonly";
                    ddlConfigurationLineDetail.CssClass += " custom-textbox";
                }

                string sizeId = GetLabelText(row, "lblInventSizeId");
                if (ddlSizeLineDetail.Items.FindByValue(sizeId) != null)
                {
                    ddlSizeLineDetail.SelectedValue = sizeId;
                }
                else
                {
                    ddlSizeLineDetail.Attributes["readonly"] = "readonly";
                    ddlSizeLineDetail.CssClass += " custom-textbox";
                }

                string styleId = GetLabelText(row, "lblInventStyleId");
                if (ddlStyleLineDetail.Items.FindByValue(styleId) != null)
                {
                    ddlStyleLineDetail.SelectedValue = styleId;
                }
                else
                {
                    ddlStyleLineDetail.Attributes["readonly"] = "readonly";
                    ddlStyleLineDetail.CssClass += " custom-textbox";
                }

                string colorId = GetLabelText(row, "lblInventColorId");
                if (ddlColorLineDetail.Items.FindByValue(colorId) != null)
                {
                    ddlColorLineDetail.SelectedValue = colorId;
                }
                else
                {
                    ddlColorLineDetail.Attributes["readonly"] = "readonly";
                    ddlColorLineDetail.CssClass += " custom-textbox";
                }


                // INVENTORY DIMENSIONS (dropdowns)
                string siteId = GetLabelText(row, "lblSite");
                if (ddlSiteLineDetail.Items.FindByValue(siteId) != null)
                {
                    ddlSiteLineDetail.SelectedValue = siteId;
                }
                else
                {
                    ddlSiteLineDetail.Attributes["readonly"] = "readonly";
                    ddlSiteLineDetail.CssClass += " custom-textbox";
                }

                string warehouseId = GetLabelText(row, "lblWarehouse");
                if (ddlWarehouseLineDetail.Items.FindByValue(warehouseId) != null)
                {
                    ddlWarehouseLineDetail.SelectedValue = warehouseId;
                }
                else
                {
                    ddlWarehouseLineDetail.Attributes["readonly"] = "readonly";
                    ddlWarehouseLineDetail.CssClass += " custom-textbox";
                }

                string batchId = GetLabelText(row, "lblInventBatchId");
                if (ddlBatchNumberLineDetail.Items.FindByValue(batchId) != null)
                {
                    ddlBatchNumberLineDetail.SelectedValue = batchId;
                }
                else
                {
                    ddlBatchNumberLineDetail.Attributes["readonly"] = "readonly";
                    ddlBatchNumberLineDetail.CssClass += " custom-textbox";
                }

                string wmsLocationId = GetLabelText(row, "lblWMSLocationId");
                if (ddlWmsLocationLineDetail.Items.FindByValue(wmsLocationId) != null)
                {
                    ddlWmsLocationLineDetail.SelectedValue = wmsLocationId;
                }
                else
                {
                    ddlWmsLocationLineDetail.Attributes["readonly"] = "readonly";
                    ddlWmsLocationLineDetail.CssClass += " custom-textbox";
                }

                string serialId = GetLabelText(row, "lblInventSerialId");
                if (ddlSerialNumberLineDetail.Items.FindByValue(serialId) != null)
                {
                    ddlSerialNumberLineDetail.SelectedValue = serialId;
                }
                else
                {
                    ddlSerialNumberLineDetail.Attributes["readonly"] = "readonly";
                    ddlSerialNumberLineDetail.CssClass += " custom-textbox";
                }




                // Populate DELIVERY TAB
                string rawDate = GetLabelText(row, "lblDeliveryRequestedDate");

                DateTime parsedDate;
                if (DateTime.TryParse(rawDate, out parsedDate))
                {
                    txtdeliveryrequestedreceiptdate.Text = parsedDate.ToString("M/d/yyyy");
                }
                string rawConfirmedDate = GetLabelText(row, "lblDeliveryConfirmedDate");
                DateTime confirmedDate;
                if (DateTime.TryParse(rawConfirmedDate, out confirmedDate))
                {
                    txtdeliveryconfirmedreceiptdate.Text = confirmedDate.ToString("M/d/yyyy");
                }
                txtPlanningPriority.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(row, "lblDeliveryPlanningPriority")));
                txtOverdelivery.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(row, "lblDeliveryOverDelivery")));
                txtModesOfDelivery.Text = GetLabelText(row, "lblDeliveryMode");
                txtUnderdelivery.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(row, "lblDeliveryUnderDelivery")));
                txtDeliveryTerms.Text = GetLabelText(row, "lblDeliveryTerms");
                TextBox3.Text = GetLabelText(row, "lblDeliveryType");
                txtDirectDeliveryStatus.Text = GetLabelText(row, "lblDeliveryDirectStatus");
                txtComments.Text = GetLabelText(row, "lblDeliveryComments");
                chkDirectDelivery.Checked = GetControlChecked(row, "chkDeliveryDirect");

                // Populate PICKING TAB
                txtBarCode.Text = GetLabelText(row, "lblBarCode");
                txtBarCodeSetup.Text = GetLabelText(row, "lblBarCodeSetup");
                chkCrossDocking.Checked = GetControlChecked(row, "chkCrossDocking");

                // Populate FIXED ASSETS TAB
                chkNewFixedAsset.Checked = GetControlChecked(row, "chkNewFixedAsset");
                txtFixedAssetGroup.Text = GetLabelText(row, "lblFixedAssetGroup");
                txtFixedAssetNumber.Text = GetLabelText(row, "lblFixedAssetNumber");
                txtBook.Text = GetLabelText(row, "lblBook");
                txtTransactionType.Text = GetLabelText(row, "lblTransactionType");

                Label isStockedProduct = (Label)row.FindControl("lblisStockedProduct");
                if (isStockedProduct.Text == "Yes")
                {
                    btnOnHand.Enabled = true;
                }
                else
                {
                    btnOnHand.Enabled = false;
                }



                ////Populate FINANCIAL DIMENSIONS TAB
                //dimensionField1.Text = GetLabelText(row, "lblDimensionField1");
                //dimensionField2.Text = GetLabelText(row, "lblDimensionField2");
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                ClearLinesDetails();
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('An error occurred while populating details: {ex.Message}');", true);
            }
        }
        private bool GetControlChecked(GridViewRow row, string controlId)
        {
            CheckBox chk = row.FindControl(controlId) as CheckBox;
            return (chk != null && chk.Checked);
        }


        private void ClearLinesDetails()
        {
            // Clear REQUEST FOR QUOTATION
            txtRFQNumber.Text = "";
            txtRFQReplyNumber.Text = "";
            txtRFQLineNumber.Text = "";

            // Clear ORDER LINE
            txtProcurementCategory.Text = "";
            txtProductName.Text = "";
            txtitemdescription.Text = "";

            // Clear PURCHASE REQUISITION
            txtPurchaseRequisition.Text = "";
            txtRequisitionProductName.Text = "";
            txtSupplierAuxId.Text = "";

            // Clear INTERCOMPANY
            txtIntercompanyOrigin.Text = "";

            // Clear REFERENCE
            txtReferenceExternal.Text = "";
            txtReferenceOrigin.Text = "";

            // Clear DELIVERY REFERENCE
            txtDeliveryCustomerRequisition.Text = "";
            txtDeliveryCustomerReference.Text = "";


            // Clear STATUS
            txtLineStatus.Text = "";
            chkStopped.Checked = false;
            chkPreventPartial.Checked = false;
            txtState.Text = "";
            txtQualityOrderStatus.Text = "";
            chkFinalized.Checked = false;
            chkAddedByPOS.Checked = false;

            // Clear SETUP TAB
            setupLotId.Text = "";
            lblMatchingPolicy.Text = "";
            lblReturnAction.Text = "";
            chkScrap.Checked = false;
            //txtItemGroup.Text = "";
            ddlSaleTaxGroup.Text = "";
            txt1099Amount.Text = "";
            txt1099StateAmount.Text = "";
            lbl1099Box.Text = "";
            txtLedgerAccount.Text = "";
            lblCreatedDateTime.Text = "";
            txtInventoryQuantity.Text = "";
            txtInventoryRemainder.Text = "";
            txtConfirmedReceiptDate.Text = "";
            txtDeliveryType.Text = "";

            // Clear ADDRESS TAB
            txtDeliveryName.Text = "";
            txtDeliveryAddress.Text = "";
            txtAddress.Text = "";
            txtServiceAddress.Text = "";
            txtCustomerRequisition.Text = "";
            txtCustomerReference.Text = "";
            txtAttentionInformation.Text = "";
            txtRequester.Text = "";

            // Clear PRODUCT TAB
            ddlConfigurationLineDetail.Text = "";
            ddlSizeLineDetail.Text = "";
            ddlColorLineDetail.Text = "";
            ddlStyleLineDetail.Text = "";
            txtVersion.Text = "";
            ddlStyleLineDetail.Text = "";
            txtOwner.Text = "";
            ddlWmsLocationLineDetail.Text = ""; // Location
            txtLicensePlate.Text = "";
            txtInventoryStatus.Text = "";
            ddlBatchNumberLineDetail.Text = "";
            ddlSiteLineDetail.Text = "";
            ddlWarehouse.Text = "";
            txtPlannedOrderNumber.Text = "";
            txtMasterPlan.Text = "";
            txtReferenceType.Text = "";
            txtReferenceNumber.Text = "";
            txtReferenceLot.Text = "";

            // Clear DELIVERY TAB
            txtdeliveryrequestedreceiptdate.Text = ""; // Requested receipt date
            txtdeliveryconfirmedreceiptdate.Text = ""; // Confirmed receipt date
            txtPlanningPriority.Text = "";
            txtOverdelivery.Text = "";
            txtModesOfDelivery.Text = "";
            txtUnderdelivery.Text = "";
            txtDeliveryTerms.Text = "";
            TextBox3.Text = ""; // Delivery type
            txtDirectDeliveryStatus.Text = "";
            txtComments.Text = "";
            chkDirectDelivery.Checked = false;

            // Clear PICKING TAB
            txtBarCode.Text = "";
            txtBarCodeSetup.Text = "";
            chkCrossDocking.Checked = false;

            // Clear FIXED ASSETS TAB
            chkNewFixedAsset.Checked = false;
            txtFixedAssetGroup.Text = "";
            txtFixedAssetNumber.Text = "";
            txtBook.Text = "";
            txtTransactionType.Text = "";

            // Clear FINANCIAL DIMENSIONS TAB
            //dimensionField1.Text = "";
            //dimensionField2.Text = "";
        }

        [System.Web.Services.WebMethod]
        public static string GetProductName(string itemId)
        {
            PurchaseOrderLines newlines = new PurchaseOrderLines();
            DataTable dt = newlines.retrieveitemid(itemId);
            string text = dt.Rows[0]["ItemName"].ToString();
            return text;
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

        protected void BtnDeleteHeader_Click(object sender, EventArgs e)
        {
            // Retrieve recId from Session
            if (Session["RecId"] != null)
            {
                long recId = 0;
                if (Int64.TryParse(Session["RecId"].ToString(), out recId) && recId > 0)
                {
                    PurchaseOrderHeader NewPurchaseOrder = new PurchaseOrderHeader();
                    SysOperationResult_BOL operationResult_BOL = NewPurchaseOrder.delete(new long[] { recId });
                    bool result = operationResults(operationResult_BOL);

                    if (result)
                    {
                        //// Redirect after successful deletion
                        //Response.Redirect("/ESS/PR/AllPurchaseOrder_ListPage.aspx", false);
                        //Context.ApplicationInstance.CompleteRequest();

                        string message = $"Purchase Order {Session["PurchaseOrderId"]} has been deleted.";
                        string script = $"alert('{message}'); window.location='/ESS/PR/AllPurchaseOrder_ListPage.aspx';";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccess", script, true);
                    }
                    else
                    {
                        bindGrid(); // refresh grid if delete failed
                    }
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            List<string> recordsId = new List<string>();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            string requestId = Session["PurchaseOrderId"] as string;
            recordsId.Add(requestId);
            SysOperationResult_BOL operationResult_BOL = eSSWorkflow.purchaseOrder_Submit(recordsId.ToArray());
            reBindGrid();

            if (operationResult_BOL.isSuccess) // Assuming there's an isSuccess property
            {
                //// Redirect to the list page after success
                //Response.Redirect("/ESS/PR/AllPurchaseOrder_ListPage.aspx", false);
                //Context.ApplicationInstance.CompleteRequest();
                string message = $"Purchase Order {requestId} has been submitted successfully.";
                string script = $"alert('{message}'); window.location='/ESS/PR/AllPurchaseOrder_ListPage.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SubmitSuccess", script, true);
            }
            else
            {
                // Optionally, show a message if submission failed
                NotificationMessage.showMessage("Submission failed. Please try again.");
            }

        }


        private bool BindSiteId(object sender, EventArgs e)
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveSite();

            // Correct way to find dropdown

            ddlSite.DataSource = dt;
            ddlSite.DataTextField = "InventSiteId";   // what user sees
            ddlSite.DataValueField = "Name";  // underlying value
            ddlSite.DataBind();

            // Optional: set selected value from first row
            ddlSite.SelectedValue = dt.Rows[0]["InventSiteId"].ToString();


            return true;
        }

        private bool BindWarehouse(object sender, EventArgs e)
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveWarehouse();

            // Correct way to find dropdown

            ddlWarehouse.DataSource = dt;
            ddlWarehouse.DataTextField = "InventLocationId";   // what user sees
            ddlWarehouse.DataValueField = "Name";  // underlying value
            ddlWarehouse.DataBind();

            // Optional: set selected value from first row
            ddlWarehouse.SelectedValue = dt.Rows[0]["InventLocationId"].ToString();


            return true;
        }

        private void bindHeaderPanelDeatils()
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            string requestId = Session["PurchaseOrderId"] as string;
            DataTable dt = header.retrieveByPurchID(requestId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtPurchaseOrder.Text = row["PurchaseOrderId"].ToString();
                txtPurchName.Text = row["VendorName"].ToString();
                txtPurchaseType.Text = row["PurchaseType"].ToString();
                ddlSite.SelectedValue = row["SiteID"].ToString();
                ddlWarehouse.SelectedValue = row["LocationID"].ToString();
                ddlReason.SelectedValue = row["editReasonCode"].ToString();
                ddlContactID.SelectedValue = row["editContactPersonName"].ToString();
                txtReasonComment.Text = row["editReasonComment"].ToString();
                ddlInvoiceAccount.SelectedValue = row["InvoiceAccount"].ToString();
                ddlVenderAccount.SelectedValue = row["VendorAccount"].ToString();
                txtInternetAddress.Text = row["URL"].ToString();
                ddlEmail.SelectedValue = row["Email"].ToString();
                txtPurchaseOrderStatus.Text = row["PurchaseOrderStatus"].ToString();
                txtDocumentStatus.Text = row["DocumentStatus"].ToString();
                txtApprovalStatus.Text = row["ApprovalStatus"].ToString();
                txtHeaderBudgetCheck.Text = row["PurchTableBudgetCheckResult"].ToString();
                //txtQualityOrderStatuses.Text = row["lblStatusQualityOrder"].ToString();
                txtCustomerReferences.Text = row["VendorRef"].ToString();
                txtRMANumber.Text = row["ReturnItemNum"].ToString();
                TxtOrigins.Text = row["PurchaseOrderHeaderCreationMethod"].ToString();
                txtCustomerRequisitions.Text = row["PurchOrderFormNum"].ToString();
                ddlPostingProfile.SelectedValue = row["PostingProfile"].ToString();
                //txtSubcontractDate.Text = row["SubConDate"].ToString();
                txtSettlementType.Text = row["SettleVoucher"].ToString();
                //txtAccountingDate.Text = Convert.ToDateTime(row["AccountingDate"]).ToString("MM-dd-yyyy");
                ddlOrderer.Text = row["PersonnelNumber"].ToString();
                ddlSaleTaxGroup.SelectedValue = row["TaxGroup"].ToString();
                ddlPool.SelectedValue = row["PurchPoolID"].ToString();
                txtHeaderBudgetCheck.Text = row["PurchTableBudgetCheckResult"].ToString();
                ddlBuyerGroup.SelectedValue = row["BuyerGroup"].ToString();
                ddlNumberSequenceGroup.SelectedValue = row["NumberSequenceGroup"].ToString();
                ddlheaderRequester.SelectedValue = row["Requester"].ToString();


                ddlLanguageId.SelectedValue = row["LanguageId"].ToString();
                chkActivateChangeManagement.Text = row["ChangeRequestRequired"].ToString();
                txtCreatedDateAndTime.Text = Convert.ToDateTime(row["Createddatetime"]).ToString("MM-dd-yyyy");
                txtDeliveryNames.Text = row["DeliveryName"].ToString();
                ddlModeOfDelivery.Text = row["DeliveryMode"].ToString();
                ddlDeliveryTerm.Text = row["DeliveryTerms"].ToString();
                txtUPSZone.Text = row["FreightZone"].ToString();
                txtCallTagType.Text = row["FreightSlipType"].ToString();
                ddlShippingCarrier.Text = row["CarrierCode"].ToString();
                txtCarrierGroup.Text = row["CarrierGroupCode"].ToString();
                txtDirectDeliveryStatus.Text = row["DirectDelivery"].ToString();
                txtDeliveryAddresss.Text = row["Address"].ToString();
                txtRequestedReceiptDates.Text = Convert.ToDateTime(row["DeliveryDate"]).ToString("MM-dd-yyyy");
                txtCurrency.Text = row["CurrencyCode"].ToString();
                ddlTermsOfPayment.Text = row["Payment"].ToString();
                txtCashDiscount.Text = row["CashDisc"].ToString();
                txtDiscountPercentage.Text = row["CashDiscPercent"].ToString();
                txtTotalDiscountPercent.Text = row["DiscPercent"].ToString();
                ddlMethodOfPayment.SelectedValue = row["PaymMode"].ToString();
                txtPriceGroup.Text = row["PriceGroupId"].ToString();
                ddlPaymentSchedule.SelectedValue = row["SchdName"].ToString();
                chkPricesIncludeSalesTax.Checked = row["InclTax"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);
                chkActivateChangeManagement.Checked = row["ChangeRequestRequired"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);
                chkSendPurchaseOrderViaCXML.Checked = row["CXMLOrderEnable"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);



            }
            else
            {
                txtPurchaseOrder.Text = "Not Found";
            }

        }

        //private bool BindReasonCode()
        //{
        //    PurchaseOrderHeader header = new PurchaseOrderHeader();
        //    DataTable dt = header.retrieveReasonCode();

        //    // Correct way to find dropdown
        //        ddlReason.DataSource = dt;
        //        ddlReason.DataTextField = "ReasonCode";   // what user sees
        //        ddlReason.DataValueField = "ReasonCode";  // underlying value
        //        ddlReason.DataBind();




        //    return true;
        //}
        private bool BindReasonCode(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveReasonCode();

            // Correct way to find dropdown
            ddlReason.DataSource = dt;
            ddlReason.DataTextField = "ReasonCode";   // what user sees
            ddlReason.DataValueField = "ReasonDescription";  // underlying value
            ddlReason.DataBind();

            if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["ReasonCode"].ToString()))
            {
                ddlReason.SelectedValue = dt.Rows[0]["ReasonCode"].ToString();
            }

            return true;
        }

      




        protected void ddlwarehouse_selection(object sender, EventArgs e)
        {
            DropDownList ddlWarehouse = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlWarehouse.NamingContainer;
            string selectedwarehouse = ddlWarehouse.SelectedValue;

            if (selectedwarehouse != null)
            {
                DataTable dt = purchaselines.findSitebyLocation(selectedwarehouse);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    DropDownList ddlSiteId = row.FindControl("ddlSiteId") as DropDownList;

                    ddlSiteId.SelectedValue = dtrow["InventSiteId"].ToString();

                }

            }
        }


        protected void btnOn_HandClick(object sender, EventArgs e)
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
                    string inventDimId = (row.FindControl("lblInventDimId") as Label)?.Text.Trim();
                    string itemId = (row.FindControl("lblItemId") as Label)?.Text.Trim();
                    string ItemName = (row.FindControl("lblProductName") as Label)?.Text.Trim();

                    // Inventory Dimensions
                    string configId = (row.FindControl("lblConfigId") as Label)?.Text.Trim();
                    string colorId = (row.FindControl("lblInventColorId") as Label)?.Text.Trim();
                    string sizeId = (row.FindControl("lblInventSizeId") as Label)?.Text.Trim();
                    string styleId = (row.FindControl("lblInventStyleId") as Label)?.Text.Trim();
                    string siteId = (row.FindControl("lblSite") as Label)?.Text.Trim();
                    string warehouse = (row.FindControl("lblWarehouse") as Label)?.Text.Trim();
                    string batchNum = (row.FindControl("lblInventBatchId") as Label)?.Text.Trim();
                    string wmsLocation = (row.FindControl("lblWMSLocationId") as Label)?.Text.Trim();
                    //string wmsPallet = (row.FindControl("lblWMSPalletId") as Label)?.Text.Trim();
                    string serialNum = (row.FindControl("lblInventSerialId") as Label)?.Text.Trim();
                    string transCode = (row.FindControl("lblTransctionCode") as Label)?.Text.Trim();
                    string unitId = (row.FindControl("lblUnit") as Label)?.Text.Trim();

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
                    //Session["WMSPalletId"] = wmsPallet;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select a Purchase order line to view On-hand.');", true);
            }
        }

        private void BindItemSalesTaxGroupLineDetail()
        {

            DataTable dt = purchaselines.retrieveItemTaxGroup(); // <-- create this method in service class

            if (ddlItemSalestaxGrouplineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlItemSalestaxGrouplineDetail.Items.Clear();
                    ddlItemSalestaxGrouplineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlItemSalestaxGrouplineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlItemSalestaxGrouplineDetail.DataSource = dt;
                ddlItemSalestaxGrouplineDetail.DataValueField = "TaxItemGroup";   // field from your [DataMember]
                ddlItemSalestaxGrouplineDetail.DataTextField = "TaxItemGroup";   // display same field
                ddlItemSalestaxGrouplineDetail.DataBind();

                ddlItemSalestaxGrouplineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlItemSalestaxGrouplineDetail.Enabled = true;
            }
        }


        private void BindSalesTaxGroupLineDetail()
        {

            DataTable dt = purchaselines.retrieveSalesTaxGroup(); // <-- implement this in your service

            if (ddlSalesTaxGrouplineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlSalesTaxGrouplineDetail.Items.Clear();
                    ddlSalesTaxGrouplineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlSalesTaxGrouplineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlSalesTaxGrouplineDetail.DataSource = dt;
                ddlSalesTaxGrouplineDetail.DataValueField = "TaxGroup";   // from [DataMember]
                ddlSalesTaxGrouplineDetail.DataTextField = "TaxGroup";    // display same field
                ddlSalesTaxGrouplineDetail.DataBind();

                ddlSalesTaxGrouplineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlSalesTaxGrouplineDetail.Enabled = true;
            }
        }

        private bool BindTermsOfPayment(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveTermsOfPayment();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlTermsOfPayment.DataSource = dt;
                ddlTermsOfPayment.DataTextField = "PaymTermId";   // what user sees
                ddlTermsOfPayment.DataValueField = "PaymTermId";  // underlying value
                ddlTermsOfPayment.DataBind();

                // Insert empty option at the top
                ddlTermsOfPayment.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlTermsOfPayment.Items.FindByValue(selectedValue) != null)
            {
                ddlTermsOfPayment.SelectedValue = selectedValue;
            }
            else
            {
                ddlTermsOfPayment.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        //private bool BindMethodOfPayment()
        //{
        //    PurchaseOrderHeader header = new PurchaseOrderHeader();
        //    DataTable dt = header.retrieveMethodOfPayment();

        //    // Correct way to find dropdown
        //    ddlMethodOfPayment.DataSource = dt;
        //    ddlMethodOfPayment.DataTextField = "PaymMode";   // what user sees
        //    ddlMethodOfPayment.DataValueField = "PaymMode";  // underlying value
        //    ddlMethodOfPayment.DataBind();
        //    return true;
        //}

        private bool BindMethodOfPayment(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveMethodOfPayment();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlMethodOfPayment.DataSource = dt;
                ddlMethodOfPayment.DataTextField = "PaymMode";   // what user sees
                ddlMethodOfPayment.DataValueField = "PaymMode";  // underlying value
                ddlMethodOfPayment.DataBind();

                // Insert empty option at the top
                ddlMethodOfPayment.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlMethodOfPayment.Items.FindByValue(selectedValue) != null)
            {
                ddlMethodOfPayment.SelectedValue = selectedValue;
            }
            else
            {
                ddlMethodOfPayment.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private void BindConfigurationforLineDetail(string itemid)
        {

            DataTable dt = purchaselines.retrieveconfigId(itemid);

            if (ddlConfigurationLineDetail != null)
            {
                // If no rows, just clear and show "No Selection Available"
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlConfigurationLineDetail.Items.Clear();
                    ddlConfigurationLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlConfigurationLineDetail.Enabled = false;
                    return; // stop here (skip binding)
                }

                // Only bind if data exists
                ddlConfigurationLineDetail.DataSource = dt;
                ddlConfigurationLineDetail.DataValueField = "ConfigId";
                ddlConfigurationLineDetail.DataTextField = "ConfigId";
                ddlConfigurationLineDetail.DataBind();

                ddlConfigurationLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlConfigurationLineDetail.Enabled = true;
            }
        }

        private void BindSiteLineDetail()
        {

            DataTable dt = purchaselines.retrievesiteId();

            if (ddlSiteLineDetail != null)
            {
                // If no rows, just clear and show "No Selection Available"
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlSiteLineDetail.Items.Clear();
                    ddlSiteLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlSiteLineDetail.Enabled = false;
                    return; // stop here (skip binding)
                }

                // Only bind if data exists
                ddlSiteLineDetail.DataSource = dt;
                ddlSiteLineDetail.DataValueField = "InventSiteId";
                ddlSiteLineDetail.DataTextField = "InventSiteId";
                ddlSiteLineDetail.DataBind();

                ddlSiteLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlSiteLineDetail.Enabled = true;
            }
        }

        private bool BindSchedulePayment(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveSchedulePayment();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPaymentSchedule.DataSource = dt;
                ddlPaymentSchedule.DataTextField = "SchdName";   // what user sees
                ddlPaymentSchedule.DataValueField = "SchdName";  // underlying value
                ddlPaymentSchedule.DataBind();

                // Insert empty option at the top
                ddlPaymentSchedule.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPaymentSchedule.Items.FindByValue(selectedValue) != null)
            {
                ddlPaymentSchedule.SelectedValue = selectedValue;
            }
            else
            {
                ddlPaymentSchedule.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindSite(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveSite();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSite.DataSource = dt;
                ddlSite.DataTextField = "SiteID";   // what user sees
                ddlSite.DataValueField = "SiteID";  // underlying value
                ddlSite.DataBind();

                // Insert empty option at the top
                ddlSite.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSite.Items.FindByValue(selectedValue) != null)
            {
                ddlSite.SelectedValue = selectedValue;
            }
            else
            {
                ddlSite.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        private bool BindWarehouse(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveWarehouse();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataTextField = "LocationID";   // what user sees
                ddlWarehouse.DataValueField = "LocationID";  // underlying value
                ddlWarehouse.DataBind();

                // Insert empty option at the top
                ddlWarehouse.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlWarehouse.Items.FindByValue(selectedValue) != null)
            {
                ddlWarehouse.SelectedValue = selectedValue;
            }
            else
            {
                ddlWarehouse.SelectedIndex = 0; // show empty
            }

            return true;
        }



        private bool BindTaxGroup(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveTaxGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSaleTaxGroup.DataSource = dt;
                ddlSaleTaxGroup.DataTextField = "TaxGroup";   // what user sees
                ddlSaleTaxGroup.DataValueField = "TaxGroup";  // underlying value
                ddlSaleTaxGroup.DataBind();

                // Insert empty option at the top
                ddlSaleTaxGroup.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSaleTaxGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlSaleTaxGroup.SelectedValue = selectedValue;
            }
            else
            {
                ddlSaleTaxGroup.SelectedIndex = 0; // keep it empty
            }

            return true;
        }






        private bool BindBuyerGroup(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveBuyerGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlBuyerGroup.DataSource = dt;
                ddlBuyerGroup.DataTextField = "BuyerGroup";   // what user sees
                ddlBuyerGroup.DataValueField = "BuyerGroup";  // underlying value
                ddlBuyerGroup.DataBind();

                // Insert empty option at the top
                ddlBuyerGroup.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlBuyerGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlBuyerGroup.SelectedValue = selectedValue;
            }
            else
            {
                ddlBuyerGroup.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        private bool BindPool(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrievePurchPool();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPool.DataSource = dt;
                ddlPool.DataTextField = "PurchPoolID";   // what user sees
                ddlPool.DataValueField = "PurchPoolID";  // underlying value
                ddlPool.DataBind();

                // Insert empty option at the top
                ddlPool.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPool.Items.FindByValue(selectedValue) != null)
            {
                ddlPool.SelectedValue = selectedValue;
            }
            else
            {
                ddlPool.SelectedIndex = 0; // keep it empty
            }

            return true;
        }




        private bool BindModeOfDelivery(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveModeOfDelivery();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlModeOfDelivery.DataSource = dt;
                ddlModeOfDelivery.DataTextField = "Code";   // what user sees
                ddlModeOfDelivery.DataValueField = "Code";  // underlying value
                ddlModeOfDelivery.DataBind();

                // Insert empty option at the top
                ddlModeOfDelivery.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlModeOfDelivery.Items.FindByValue(selectedValue) != null)
            {
                ddlModeOfDelivery.SelectedValue = selectedValue;
            }
            else
            {
                ddlModeOfDelivery.SelectedIndex = 0; // keep it empty
            }

            return true;
        }




        private bool BindDeliveryTerm(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveDeliveryTerms();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlDeliveryTerm.DataSource = dt;
                ddlDeliveryTerm.DataTextField = "TermCode";   // what user sees
                ddlDeliveryTerm.DataValueField = "TermCode";  // underlying value
                ddlDeliveryTerm.DataBind();

                // Insert empty option at the top
                ddlDeliveryTerm.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlDeliveryTerm.Items.FindByValue(selectedValue) != null)
            {
                ddlDeliveryTerm.SelectedValue = selectedValue;
            }
            else
            {
                ddlDeliveryTerm.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {
                // ------------------ GENERAL TAB ------------------
                if (Session["RecId"] != null)
                {
                    long recId = 0;
                    if (Int64.TryParse(Session["RecId"].ToString(), out recId) && recId > 0)
                    {
                        DataTable datatable = new DataTable();
                        datatable.Columns.Add("PurchID");
                        datatable.Columns.Add("PurchaseOrderId");
                        datatable.Columns.Add("VendorName");
                        datatable.Columns.Add("PurchaseType");
                        datatable.Columns.Add("VendorAccount");
                        datatable.Columns.Add("InvoiceAccount");
                        datatable.Columns.Add("editContactPersonName");
                        datatable.Columns.Add("URL");
                        datatable.Columns.Add("Email");
                        datatable.Columns.Add("PurchStatus");
                        datatable.Columns.Add("DocumentState");
                        datatable.Columns.Add("SiteID");
                        datatable.Columns.Add("LocationID");
                        datatable.Columns.Add("VendorRef");
                        datatable.Columns.Add("ReturnItemNum");
                        datatable.Columns.Add("PurchOrderFormNum");
                        datatable.Columns.Add("ContactPersonId");
                        datatable.Columns.Add("editReasonCode");
                        datatable.Columns.Add("editReasonComment");
                        datatable.Columns.Add("RecId");

                        DataRow row = datatable.NewRow();
                        row["PurchID"] = txtPurchaseOrder.Text.Trim();
                        row["PurchaseOrderId"] = txtPurchaseOrder.Text.Trim();
                        row["VendorName"] = txtPurchName.Text.Trim();
                        row["PurchaseType"] = txtPurchaseType.Text.Trim();
                        row["VendorAccount"] = ddlVenderAccount.SelectedValue;
                        row["InvoiceAccount"] = ddlInvoiceAccount.SelectedValue;
                        row["editContactPersonName"] = ddlContactID.Text.Trim();
                        row["URL"] = txtInternetAddress.Text.Trim();
                        row["Email"] = ddlEmail.SelectedValue;
                        row["SiteID"] = ddlSite.SelectedValue;
                        row["LocationID"] = ddlWarehouse.SelectedValue;
                        row["VendorRef"] = txtCustomerReferences.Text.Trim();
                        row["DocumentState"] = txtDocumentStatus.Text.Trim();
                        row["PurchOrderFormNum"] = txtCustomerRequisitions.Text.Trim();
                        row["ContactPersonId"] = ddlContactID.SelectedValue;
                        row["editReasonCode"] = ddlReason.SelectedValue;
                        row["editReasonComment"] = txtReasonComment.Text.Trim();
                        row["RecId"] = Session["RecId"];
                        datatable.Rows.Add(row);

                        PurchaseOrderHeader header = new PurchaseOrderHeader();
                        SysOperationResult_BOL result = header.UpdateGeneral(datatable);
                    }
                    else
                    {
                        // ⚠ result not defined in this scope
                        NotificationMessage.showMessage("Invalid RecId");
                        return;
                    }
                }

                // ------------------ SETUP TAB ------------------
                if (Session["RecId"] != null)
                {
                    long recId1 = 0;
                    if (Int64.TryParse(Session["RecId"].ToString(), out recId1) && recId1 > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("PurchID");
                        dt.Columns.Add("TaxGroup");
                        dt.Columns.Add("PostingProfile");
                        dt.Columns.Add("SettleVoucher");
                        dt.Columns.Add("AccountingDate");
                        dt.Columns.Add("PurchPoolID");
                        dt.Columns.Add("LanguageId");
                        dt.Columns.Add("ChangeRequestRequired");
                        dt.Columns.Add("CXMLOrderEnable");
                        dt.Columns.Add("InclTax");
                        dt.Columns.Add("NumberSequenceGroup");
                        dt.Columns.Add("BuyerGroup");
                        dt.Columns.Add("Requester");
                        dt.Columns.Add("PersonnelNumber");
                        dt.Columns.Add("RecId");

                        DataRow row1 = dt.NewRow();
                        row1["PurchID"] = txtPurchaseOrder.Text.Trim();
                        row1["TaxGroup"] = ddlSaleTaxGroup.SelectedValue;
                        row1["PostingProfile"] = ddlPostingProfile.SelectedValue;
                        row1["SettleVoucher"] = txtSettlementType.Text.Trim();
                        row1["AccountingDate"] = txtAccountingDate.Text.Trim();
                        row1["PurchPoolID"] = ddlPool.SelectedValue;
                        row1["LanguageId"] = ddlLanguageId.SelectedValue;
                        row1["ChangeRequestRequired"] = chkActivateChangeManagement.Checked ? "Yes" : "No";
                        row1["CXMLOrderEnable"] = chkSendPurchaseOrderViaCXML.Checked ? "Yes" : "No";
                        row1["InclTax"] = chkPricesIncludeSalesTax.Checked ? "Yes" : "No";
                        row1["NumberSequenceGroup"] = ddlNumberSequenceGroup.SelectedValue;
                        row1["BuyerGroup"] = ddlBuyerGroup.SelectedValue;
                        row1["Requester"] = ddlheaderRequester.SelectedValue;
                        row1["PersonnelNumber"] = ddlOrderer.SelectedValue;
                        row1["RecId"] = Session["RecId"];
                        dt.Rows.Add(row1);

                        PurchaseOrderHeader header1 = new PurchaseOrderHeader();
                        SysOperationResult_BOL result1 = header1.UpdateSetupRecord(dt);

                        if (result1 == null || !result1.isSuccess)
                        {
                            NotificationMessage.showMessage(result1);
                            return;
                        }
                    }
                }

                // ------------------ ADDRESS TAB ------------------
                if (Session["RecId"] != null)
                {
                    long recId2 = 0;
                    if (Int64.TryParse(Session["RecId"].ToString(), out recId2) && recId2 > 0)
                    {
                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("PurchID");
                        dt2.Columns.Add("DeliveryName");
                        dt2.Columns.Add("Address");
                        dt2.Columns.Add("ReqAttention");
                        dt2.Columns.Add("RecId");

                        DataRow row2 = dt2.NewRow();
                        row2["PurchID"] = txtPurchaseOrder.Text.Trim();
                        row2["DeliveryName"] = txtDeliveryNames.Text.Trim();
                        row2["Address"] = txtAddresss.Text.Trim();
                        row2["ReqAttention"] = txtAttentionInformations.Text.Trim();
                        row2["RecId"] = Session["RecId"];
                        dt2.Rows.Add(row2);

                        PurchaseOrderHeader header2 = new PurchaseOrderHeader();
                        SysOperationResult_BOL result2 = header2.UpdateAddressRecord(dt2);

                        if (result2 == null || !result2.isSuccess)
                            return;
                    }
                }

                // ------------------ DELIVERY TAB ------------------
                if (Session["RecId"] != null)
                {
                    long recId3 = 0;
                    if (Int64.TryParse(Session["RecId"].ToString(), out recId3) && recId3 > 0)
                    {
                        DataTable dt3 = new DataTable();
                        dt3.Columns.Add("PurchID");
                        dt3.Columns.Add("DeliveryDate");
                        dt3.Columns.Add("DeliveryMode");
                        dt3.Columns.Add("DeliveryTerms");
                        dt3.Columns.Add("FreightZone");
                        dt3.Columns.Add("FreightSlipType");
                        dt3.Columns.Add("RecId");

                        DataRow row3 = dt3.NewRow();
                        row3["PurchID"] = txtPurchaseOrder.Text.Trim();
                        row3["DeliveryDate"] = txtRequestedReceiptDates.Text.Trim();
                        row3["DeliveryMode"] = ddlModeOfDelivery.SelectedValue;
                        row3["DeliveryTerms"] = ddlDeliveryTerm.SelectedValue;
                        row3["RecId"] = Session["RecId"];
                        dt3.Rows.Add(row3);

                        PurchaseOrderHeader header3 = new PurchaseOrderHeader();
                        SysOperationResult_BOL result3 = header3.UpdateDeliveryRecord(dt3);

                        if (result3 == null || !result3.isSuccess)
                        {
                            NotificationMessage.showMessage(result3);
                            return;
                        }
                    }
                }

                // ------------------ PRICE & DISCOUNT TAB ------------------
                if (Session["RecId"] != null)
                {
                    long recId4 = 0;
                    if (Int64.TryParse(Session["RecId"].ToString(), out recId4) && recId4 > 0)
                    {
                        DataTable dt4 = new DataTable();
                        dt4.Columns.Add("PurchID");
                        dt4.Columns.Add("Payment");
                        dt4.Columns.Add("PaymMode");
                        dt4.Columns.Add("CashDisc");
                        dt4.Columns.Add("PriceGroupId");
                        dt4.Columns.Add("SchdName");
                        dt4.Columns.Add("RecId");

                        DataRow row4 = dt4.NewRow();
                        row4["PurchID"] = txtPurchaseOrder.Text.Trim();
                        row4["Payment"] = ddlTermsOfPayment.SelectedValue;
                        row4["PaymMode"] = ddlMethodOfPayment.SelectedValue;
                        row4["CashDisc"] = txtCashDiscount.Text.Trim();
                        row4["PriceGroupId"] = txtPriceGroup.Text.Trim();
                        row4["SchdName"] = ddlPaymentSchedule.SelectedValue;
                        row4["RecId"] = Session["RecId"];
                        dt4.Rows.Add(row4);

                        PurchaseOrderHeader header4 = new PurchaseOrderHeader();
                        SysOperationResult_BOL result4 = header4.UpdatePriceandDiscountRecord(dt4);

                        if (result4 == null || !result4.isSuccess)
                        {
                            NotificationMessage.showMessage(result4);
                            return;
                        }
                    }
                }



                // ------------------ FINANCIAL DIMENSIONS ------------------
                long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;
                List<DataContract> dimContracts = new List<DataContract>();
                ESSFinancialDimensions finDim = new ESSFinancialDimensions();

                DataContract[] activeDimensions = finDim.retrieveActiveDimensions();
                foreach (var dim in activeDimensions)
                {
                    DropDownList ddl = financialDimensionsContainer.FindControl("ddlFinancialDimension" + dim.Code) as DropDownList;
                    if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                    {
                        DataContract dc = new DataContract
                        {
                            Code = dim.Code,
                            Value1 = ddl.SelectedValue,
                            Value2 = ddl.SelectedItem.Text
                        };
                        dimContracts.Add(dc);
                    }
                }

                long defaultDimensionRecId = 0;
                if (dimContracts.Count > 0)
                {
                    ESSFinancialDimensions newdimension = new ESSFinancialDimensions();
                    defaultDimensionRecId = newdimension.setDimensionValues(dimContracts.ToArray());
                }

                SysOperationResult_BOL resultDim = purchaselines.updateFinancialDimension(defaultDimensionRecId, recIdLine);
                NotificationMessage.showMessage(resultDim);



                // ------------------ INVENTORY DIMENSIONS ------------------
                foreach (GridViewRow gridRow in gridView.Rows)
                {
                    Label lblItemId = (Label)gridRow.FindControl("lblItemId");
                    Label lblRecId = (Label)gridRow.FindControl("lblRecId");

                    string itemId = lblItemId != null ? lblItemId.Text.Trim() : string.Empty;
                    long recIdInv = 0;
                    if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
                        long.TryParse(lblRecId.Text, out recIdInv);

                    string configuration = ddlConfigurationLineDetail.SelectedValue;
                    string color = ddlColorLineDetail.SelectedValue;
                    string size = ddlSizeLineDetail.SelectedValue;
                    string style = ddlStyleLineDetail.SelectedValue;
                    string site = ddlSiteLineDetail.SelectedValue;
                    string warehouse = ddlWarehouseLineDetail.SelectedValue;
                    string batchNumber = ddlBatchNumberLineDetail.SelectedValue;
                    string wmsLocation = ddlWmsLocationLineDetail.SelectedValue;
                    string serialNumber = ddlSerialNumberLineDetail.SelectedValue;

                    bool hasDimension =
                        !string.IsNullOrEmpty(configuration) ||
                        !string.IsNullOrEmpty(color) ||
                        !string.IsNullOrEmpty(size) ||
                        !string.IsNullOrEmpty(style) ||
                        !string.IsNullOrEmpty(site) ||
                        !string.IsNullOrEmpty(warehouse) ||
                        !string.IsNullOrEmpty(batchNumber) ||
                        !string.IsNullOrEmpty(wmsLocation) ||
                        !string.IsNullOrEmpty(serialNumber);

                    if (hasDimension)
                    {
                        DataTable inventDt = new DataTable();
                        inventDt.Columns.Add("ItemId");
                        inventDt.Columns.Add("RecId", typeof(long));
                        inventDt.Columns.Add("Configuration");
                        inventDt.Columns.Add("Color");
                        inventDt.Columns.Add("Size");
                        inventDt.Columns.Add("Style");
                        inventDt.Columns.Add("Site");
                        inventDt.Columns.Add("Warehouse");
                        inventDt.Columns.Add("BatchNumber");
                        inventDt.Columns.Add("WmsLocation");
                        inventDt.Columns.Add("SerialNumber");

                        DataRow inventRow = inventDt.NewRow();
                        inventRow["ItemId"] = itemId;
                        inventRow["RecId"] = recIdInv;
                        inventRow["Configuration"] = configuration;
                        inventRow["Color"] = color;
                        inventRow["Size"] = size;
                        inventRow["Style"] = style;
                        inventRow["Site"] = site;
                        inventRow["Warehouse"] = warehouse;
                        inventRow["BatchNumber"] = batchNumber;
                        inventRow["WmsLocation"] = wmsLocation;
                        inventRow["SerialNumber"] = serialNumber;
                        inventDt.Rows.Add(inventRow);

                        SysOperationResult_BOL inventResult = purchaselines.UpdateInventDimRecord(inventDt);
                        NotificationMessage.showMessage(inventResult);
                    }
                }

                // ------------------ SALES TAX GROUP ------------------
                long recIdforSalesTax = 0;
                if (Session["RecIdline"] != null &&
                  Int64.TryParse(Session["RecIdline"].ToString(), out recIdforSalesTax) &&
                  recIdforSalesTax > 0)
                {
                    DataTable dttax = new DataTable();
                    dttax.Columns.Add("TaxItemGroup");
                    dttax.Columns.Add("TaxGroup");
                    dttax.Columns.Add("RecId");

                    DataRow rowtax = dttax.NewRow();
                    rowtax["TaxItemGroup"] = ddlItemSalestaxGrouplineDetail.SelectedValue;
                    rowtax["TaxGroup"] = ddlSalesTaxGrouplineDetail.SelectedValue;
                    rowtax["RecId"] = recIdforSalesTax;
                    dttax.Rows.Add(rowtax);

                    SysOperationResult_BOL resulttax = purchaselines.updateItemTaxGroup(dttax);
                    NotificationMessage.showMessage(resulttax);
                }

                // ------------------ FINANCIAL DIMENSIONS Header ------------------
                long recIdheader = Session["RecId"] != null ? (long)Session["RecId"] : 0;
                List<DataContract> dimContractsheader = new List<DataContract>();
                ESSFinancialDimensions finDimheader = new ESSFinancialDimensions();

                DataContract[] activeDimensionsvendor = finDim.retrieveActiveDimensions();
                foreach (var dim in activeDimensions)
                {
                    DropDownList ddl = VendorFinancialDimensionContainer.FindControl("ddlFinancialDimensionNew" + dim.Code) as DropDownList;
                    if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                    {
                        DataContract dc = new DataContract
                        {
                            Code = dim.Code,
                            Value1 = ddl.SelectedValue,
                            Value2 = ddl.SelectedItem.Text
                        };
                        dimContracts.Add(dc);
                    }
                }

                long defaultDimensionRecIdheader = 0;
                if (dimContracts.Count > 0)
                {
                    ESSFinancialDimensions newdimensionheader = new ESSFinancialDimensions();
                    defaultDimensionRecIdheader = newdimensionheader.setDimensionValues(dimContracts.ToArray());
                }

                PurchaseOrderHeader headerdimension = new PurchaseOrderHeader();
                SysOperationResult_BOL resultDimheader = headerdimension.updateFinancialDimensionHeader(defaultDimensionRecIdheader, recIdheader);
                NotificationMessage.showMessage(resultDimheader);
            }
            catch (Exception ex)
            {
                NotificationMessage.showMessage("Error occurred: " + ex.Message);
            }
        }



        private bool BindInvoiceAccount(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveInvoiceAccount();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlInvoiceAccount.DataSource = dt;
                ddlInvoiceAccount.DataTextField = "VendorAccount";   // what user sees
                ddlInvoiceAccount.DataValueField = "VendorAccount";  // underlying value
                ddlInvoiceAccount.DataBind();

                // Insert empty option at the top
                ddlInvoiceAccount.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlInvoiceAccount.Items.FindByValue(selectedValue) != null)
            {
                ddlInvoiceAccount.SelectedValue = selectedValue;
            }
            else
            {
                ddlInvoiceAccount.SelectedIndex = 0; // keep it empty
            }

            return true;
        }





        private bool BindVenderAccount(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveVenderAccount();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlVenderAccount.DataSource = dt;
                ddlVenderAccount.DataTextField = "VendorAccount";   // what user sees
                ddlVenderAccount.DataValueField = "VendorAccount";  // underlying value
                ddlVenderAccount.DataBind();

                // Insert empty option at the top
                ddlVenderAccount.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlVenderAccount.Items.FindByValue(selectedValue) != null)
            {
                ddlVenderAccount.SelectedValue = selectedValue;
            }
            else
            {
                ddlVenderAccount.SelectedIndex = 0; // keep empty
            }

            return true;
        }

        private bool BindContactId(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveContactId();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlContactID.DataSource = dt;
                ddlContactID.DataTextField = "ContactPersonId";   // what user sees
                ddlContactID.DataValueField = "ContactPersonId";  // underlying value
                ddlContactID.DataBind();

                // Insert empty option at the top
                ddlContactID.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlContactID.Items.FindByValue(selectedValue) != null)
            {
                ddlContactID.SelectedValue = selectedValue;
            }
            else
            {
                ddlContactID.SelectedIndex = 0; // keep empty
            }

            return true;
        }

        private bool BindLanguageId(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveLanguageId();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlLanguageId.DataSource = dt;
                ddlLanguageId.DataTextField = "LangaugeID";   // what user sees
                ddlLanguageId.DataValueField = "LangaugeID";  // underlying value
                ddlLanguageId.DataBind();

                // Insert empty option at the top
                ddlLanguageId.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlLanguageId.Items.FindByValue(selectedValue) != null)
            {
                ddlLanguageId.SelectedValue = selectedValue;
            }
            else
            {
                ddlContactID.SelectedIndex = 0; // keep empty
            }

            return true;
        }

        private bool BindPostingProfile(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrievePostingProfil();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPostingProfile.DataSource = dt;
                ddlPostingProfile.DataTextField = "PostingProfile";   // what user sees
                ddlPostingProfile.DataValueField = "PostingProfile";  // underlying value
                ddlPostingProfile.DataBind();

                // Insert empty option at the top
                ddlPostingProfile.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPostingProfile.Items.FindByValue(selectedValue) != null)
            {
                ddlPostingProfile.SelectedValue = selectedValue;
            }
            else
            {
                ddlPostingProfile.SelectedIndex = 0; // keep it empty
            }

            return true;
        }
        private bool BindNumberSeq(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveNumberSeq();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlNumberSequenceGroup.DataSource = dt;
                ddlNumberSequenceGroup.DataTextField = "NumberSequenceGroup";   // what user sees
                ddlNumberSequenceGroup.DataValueField = "NumberSequenceGroup";  // underlying value
                ddlNumberSequenceGroup.DataBind();

                // Insert empty option at the top
                ddlNumberSequenceGroup.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlNumberSequenceGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlNumberSequenceGroup.SelectedValue = selectedValue;
            }
            else
            {
                ddlNumberSequenceGroup.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindRequester(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveRequester();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlheaderRequester.DataSource = dt;
                ddlheaderRequester.DataTextField = "PersonnelNumber";   // what user sees
                ddlheaderRequester.DataValueField = "PersonnelNumber";  // underlying value
                ddlheaderRequester.DataBind();

                // Insert empty option at the top
                ddlheaderRequester.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlheaderRequester.Items.FindByValue(selectedValue) != null)
            {
                ddlheaderRequester.SelectedValue = selectedValue;
            }
            else
            {
                ddlheaderRequester.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindOrderer(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveOrderer();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlOrderer.DataSource = dt;
                ddlOrderer.DataTextField = "PersonnelNumber";   // what user sees
                ddlOrderer.DataValueField = "PersonnelNumber";  // underlying value
                ddlOrderer.DataBind();

                // Insert empty option at the top
                ddlOrderer.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlOrderer.Items.FindByValue(selectedValue) != null)
            {
                ddlOrderer.SelectedValue = selectedValue;
            }
            else
            {
                ddlOrderer.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        protected void ddlVenderAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vendAccount = ddlVenderAccount.SelectedValue;

            // Clear controls if vendAccount is null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(vendAccount))
            {
                //ClearControls();
                return; // Exit early
            }

            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveVendor(vendAccount);

            if (dt.Rows.Count > 0)
            {
                txtPurchName.Text = dt.Rows[0]["VendorName"].ToString();

                // ✅ set Invoice dropdown value (but only if it exists in the items)
                if (ddlInvoiceAccount.Items.FindByValue(vendAccount) != null)
                {
                    ddlInvoiceAccount.SelectedValue = vendAccount;
                }
            }
        }

        //protected void ddlSiteLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddlSiteId = (DropDownList)sender; // The site dropdown that triggered event
        //    GridViewRow row = (GridViewRow)ddlSiteId.NamingContainer;

        //    // Find ddlWarehouse in the same row
        //    DropDownList ddlWarehouse = (DropDownList)row.FindControl("ddlWarehouse");

        //    string selectedSiteId = ddlSiteId.SelectedValue;

        //    PurchaseOrderLines purchaseOrderLines = new PurchaseOrderLines();

        //    if (!string.IsNullOrEmpty(selectedSiteId))
        //    {
        //        DataTable dt = purchaseOrderLines.findSitebyLocation(selectedSiteId);

        //        // Add DisplayText column
        //        dt.Columns.Add("DisplayText", typeof(string));
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["DisplayText"] = dr["InventLocationId"] + " - " + dr["InventLocationName"];
        //        }

        //        // Bind to warehouse dropdown
        //        ddlWarehouse.DataSource = dt;
        //        ddlWarehouse.DataValueField = "InventLocationId";
        //        ddlWarehouse.DataTextField = "DisplayText";
        //        ddlWarehouse.DataBind();

        //        // Insert empty option at top
        //        ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty));
        //    }
        //    else
        //    {
        //        ddlWarehouse.Items.Clear();
        //        ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty)); // ✅ fixed
        //    }
        //}
        protected void ddlSiteLineDetailProductTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSiteId = ddlSite.SelectedValue;

            if (!string.IsNullOrEmpty(selectedSiteId))
            {
                PurchaseOrderLines purchaseOrderLines = new PurchaseOrderLines();
                DataTable dt = purchaseOrderLines.retrieveinventLocationId(selectedSiteId);

                // Add DisplayText column
                dt.Columns.Add("DisplayText", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["InventLocationId"] + " - " + row["LocationName"];
                }

                // Bind to warehouse dropdown
                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataValueField = "InventLocationId";
                ddlWarehouse.DataTextField = "DisplayText";
                ddlWarehouse.DataBind();

                // Insert empty option at top
                ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty));
            }
            else
            {
                ddlWarehouse.Items.Clear();
                ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty));
                ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty));
            }
        }


        //protected void ddlWarehouseLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddlWarehouse = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddlWarehouse.NamingContainer;

        //    DropDownList ddlSiteId = (DropDownList)row.FindControl("ddlSiteId");

        //    string selectedWarehouse = ddlWarehouse.SelectedValue;
        //    PurchaseOrderLines purchaseOrderLines = new PurchaseOrderLines();

        //    if (!string.IsNullOrEmpty(selectedWarehouse))
        //    {
        //        DataTable dt = purchaseOrderLines.findSitebyLocation(selectedWarehouse);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            DataRow dtrow = dt.Rows[0];
        //            ddlSiteId.SelectedValue = dtrow["InventSiteId"].ToString();
        //        }
        //    }
        //    else
        //    {
        //        ddlSiteId.ClearSelection();
        //    }
        //}

        protected void ddlWarehouseLineDetailHeaderTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWarehouse = ddlWarehouse.SelectedValue;

            if (!string.IsNullOrEmpty(selectedWarehouse))
            {
                PurchaseOrderLines purchaseOrderLines = new PurchaseOrderLines();

                DataTable dt = purchaseOrderLines.findSitebyLocation(selectedWarehouse);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    ddlSite.SelectedValue = dtrow["InventSiteId"].ToString();
                }
            }
            else
            {
                ddlSite.ClearSelection();
            }
        }

        private bool BindEmail(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveEmail();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlEmail.DataSource = dt;
                ddlEmail.DataTextField = "Locator";   // what user sees
                ddlEmail.DataValueField = "Locator";  // underlying value
                ddlEmail.DataBind();

                // Insert empty option at the top
                ddlEmail.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlEmail.Items.FindByValue(selectedValue) != null)
            {
                ddlEmail.SelectedValue = selectedValue;
            }
            else
            {
                ddlEmail.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        private void BindColorLineDetail(string itemid)
        {

            DataTable dt = purchaselines.retrieveinventColorId(itemid);

            if (ddlColorLineDetail != null)
            {
                // If no rows, just clear and show "No Selection Available"
                if (dt == null || dt.Rows.Count == 0)
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

            DataTable dt = purchaselines.retrieveinventSizeId(itemid);

            if (ddlSizeLineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0)
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

            DataTable dt = purchaselines.retrieveinvetstyleid(itemid);

            if (ddlStyleLineDetail != null)
            {
                // If no rows, just clear and show empty selection
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlStyleLineDetail.Items.Clear();
                    ddlStyleLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlStyleLineDetail.Enabled = false;
                    return; // skip binding
                }

                // Only bind if data exists
                ddlStyleLineDetail.DataSource = dt;
                ddlStyleLineDetail.DataValueField = "InventStyleId";
                ddlStyleLineDetail.DataTextField = "InventStyleId";
                ddlStyleLineDetail.DataBind();

                ddlStyleLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlStyleLineDetail.Enabled = true;
            }
        }

        private void BindWarehouseLineDetail(string itemid)
        {


            DataTable dt = purchaselines.retrieveinventLocationId();

            if (ddlWarehouseLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlWarehouseLineDetail.Items.Clear();
                    ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlWarehouseLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlWarehouseLineDetail.DataSource = dt;
                ddlWarehouseLineDetail.DataValueField = "InventLocationId";
                ddlWarehouseLineDetail.DataTextField = "InventLocationId";
                ddlWarehouseLineDetail.DataBind();

                ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlWarehouseLineDetail.Enabled = true;
            }
        }


        private void BindBatchLineDetail(string itemid)
        {

            DataTable dt = purchaselines.retrieveinventBatchId(itemid);

            if (ddlBatchNumberLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlBatchNumberLineDetail.Items.Clear();
                    ddlBatchNumberLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlBatchNumberLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlBatchNumberLineDetail.DataSource = dt;
                ddlBatchNumberLineDetail.DataValueField = "InventBatchId";
                ddlBatchNumberLineDetail.DataTextField = "InventBatchId";
                ddlBatchNumberLineDetail.DataBind();

                ddlBatchNumberLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlBatchNumberLineDetail.Enabled = true;
            }
        }

        private void BindWmsLocationLineDetail(string itemid)
        {

            DataTable dt = purchaselines.retrievewmsLocationId(itemid);

            if (ddlWmsLocationLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0)
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

            DataTable dt = purchaselines.retrieveinventSerialId(itemid);

            if (ddlSerialNumberLineDetail != null)
            {
                // If no rows, clear and disable
                if (dt == null || dt.Rows.Count == 0)
                {
                    ddlSerialNumberLineDetail.Items.Clear();
                    ddlSerialNumberLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                    ddlSerialNumberLineDetail.Enabled = false;
                    return;
                }

                // Bind if data exists
                ddlSerialNumberLineDetail.DataSource = dt;
                ddlSerialNumberLineDetail.DataValueField = "InventSerialId";
                ddlSerialNumberLineDetail.DataTextField = "InventSerialId";
                ddlSerialNumberLineDetail.DataBind();

                ddlSerialNumberLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlSerialNumberLineDetail.Enabled = true;
            }
        }

        protected void ddlSiteLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string selectedSiteId = ddlSiteLineDetail.SelectedValue;

            //if (!string.IsNullOrEmpty(selectedSiteId))
            //{

            //    DataTable dt = purchaselines.retrieveinventLocationId(selectedSiteId);

            //    // Add DisplayText column
            //    dt.Columns.Add("DisplayText", typeof(string));
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        row["DisplayText"] = row["InventLocationId"] + " - " + row["InventLocationName"];
            //    }

            //    // Bind to warehouse dropdown
            //    ddlWarehouseLineDetail.DataSource = dt;
            //    ddlWarehouseLineDetail.DataValueField = "InventLocationId";
            //    ddlWarehouseLineDetail.DataTextField = "DisplayText";
            //    ddlWarehouseLineDetail.DataBind();

            //    // Insert empty option at top
            //    ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
            //}
            //else
            //{
            //    ddlWarehouseLineDetail.Items.Clear();
            //    ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
            //}
        }

        protected void ddlWarehouseLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWarehouse = ddlWarehouseLineDetail.SelectedValue;

            if (!string.IsNullOrEmpty(selectedWarehouse))
            {

                DataTable dt = purchaselines.findSitebyLocation(selectedWarehouse);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    ddlSiteLineDetail.SelectedValue = dtrow["InventSiteId"].ToString();

                }
            }
            else
            {
                ddlSiteLineDetail.ClearSelection();
            }
        }

        private bool BindShippingCarrier(string selectedValue = "")
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();
            DataTable dt = header.retrieveShippingCarrier();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlShippingCarrier.DataSource = dt;
                ddlShippingCarrier.DataTextField = "CarrierCode";   // what user sees
                ddlShippingCarrier.DataValueField = "CarrierCode";  // underlying value
                ddlShippingCarrier.DataBind();

                // Insert empty option at the top
                ddlShippingCarrier.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlShippingCarrier.Items.FindByValue(selectedValue) != null)
            {
                ddlShippingCarrier.SelectedValue = selectedValue;
            }
            else
            {
                ddlShippingCarrier.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        private void showFinancialDimensionHamza(long _defaultDimensionRecId)
        {
            Int64 workerDimension = 0;
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();

            if (workerDimension == 0)
            {
                try
                {
                    DataContract[] dimensions = finDim.retrieveActiveDimensions();

                    if (dimensions != null && dimensions.Length > 0)
                    {
                        // Container where you want to add rows
                        // (put a <div runat="server" id="financialDimensionsContainer"></div> in your .aspx)
                        VendorFinancialDimensionContainer.Controls.Clear();

                        int colCount = 0;
                        HtmlGenericControl rowDiv = null;

                        foreach (var dim in dimensions)
                        {
                            if (colCount % 2 == 0)
                            {
                                // Start a new row after every two blocks
                                rowDiv = new HtmlGenericControl("div");
                                rowDiv.Attributes["class"] = "info-row";
                                VendorFinancialDimensionContainer.Controls.Add(rowDiv);
                            }

                            // Create info-block
                            HtmlGenericControl blockDiv = new HtmlGenericControl("div");
                            blockDiv.Attributes["class"] = "info-block";

                            // Strong title
                            HtmlGenericControl strong = new HtmlGenericControl("strong");
                            strong.InnerText = dim.Code;   // e.g. Business Unit / Cost Center
                            blockDiv.Controls.Add(strong);

                            // Span wrapper
                            HtmlGenericControl span = new HtmlGenericControl("span");

                            //// Label
                            //Label lbl = new Label();
                            //lbl.ID = $"FinancialDimension{dim.Code}";
                            //lbl.Text = "N/A";
                            //span.Controls.Add(lbl);

                            // Dropdown
                            DropDownList ddl = new DropDownList();
                            ddl.ID = $"ddlFinancialDimensionNew{dim.Code}";
                            ddl.CssClass = "filterable-dropdown";
                            ddl.Visible = true;

                            // Populate dropdown from SOAP lookup
                            DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                            ddl.Items.Add(new ListItem("", ""));
                            foreach (DataRow row in dimeVal.Rows)
                            {
                                string value = row["Value1"].ToString();
                                string valueName = row["Value2"].ToString();
                                ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                            }

                            span.Controls.Add(ddl);
                            blockDiv.Controls.Add(span);

                            // Add block to row
                            rowDiv.Controls.Add(blockDiv);

                            colCount++;
                        }
                        bindEmployeeDimensionHamza(_defaultDimensionRecId);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic dimensions", ex);
                }
            }

        }

        protected void bindEmployeeDimensionHamza(long __defaultDimensionRecId)
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();

            long empDimension = __defaultDimensionRecId;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(empDimension);
            foreach (DataContract dataContract in dataContracts)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                DropDownList ddl = VendorFinancialDimensionContainer.FindControl("ddlFinancialDimensionNew" + dimName) as DropDownList;
                TextBox txt = VendorFinancialDimensionContainer.FindControl("txt_" + dimName) as TextBox;

                if (ddl != null)
                {
                    // Check if value exists in dropdown items
                    if (!string.IsNullOrEmpty(dimValue) && ddl.Items.FindByValue(dimValue) != null)
                    {
                        ddl.SelectedValue = dimValue;
                        if (txt != null)
                            txt.Text = dimDescription;
                    }

                }
            }
        }
        protected void btnQualityOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Redirect to the Quality Order form
                Response.Redirect("~/ESS/PR/QualityOrder.aspx", false);
            }
            catch (Exception ex)
            {
                // Optional: Handle any unexpected errors gracefully
                // Example: log the error or show a notification message
                System.Diagnostics.Debug.WriteLine("Error redirecting to Quality Order: " + ex.Message);
            }
        }

        protected void btnSalesTax_Click(object sender, EventArgs e)
        {
            bool found = false;

            try
            {
                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        found = true;

                        // ✅ Get RecId from selected row
                        string recIdText = (row.FindControl("lblRecId") as Label)?.Text.Trim();
                        long recId = 0;
                        if (!string.IsNullOrEmpty(recIdText) && long.TryParse(recIdText, out recId))
                        {
                            Session["RecId"] = recId; // Store RecId for SalesTax page
                        }

                        // ✅ Get Purchase Order ID from Session (or grid if needed)
                        string purchaseOrderID = Session["PurchaseOrderId"] as string;
                        if (string.IsNullOrEmpty(purchaseOrderID))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "NoPOID",
                                "alert('Purchase Order ID not found in session.');", true);
                            return;
                        }

                        // ✅ Store POID for SalesTax page
                        Session["PurchaseOrderId"] = purchaseOrderID;
                        string returnUrl = Request.Url.PathAndQuery;

                        // ✅ Redirect to Sales Tax page (open popup)
                        // First, URL-encode the returnUrl in C#
                        string encodedReturnUrl = Server.UrlEncode(returnUrl);

                        // Then build the JavaScript correctly using string interpolation
                        string script = $"openPopupPanel('/ESS/PR/SalesTax.aspx?PurchId={purchaseOrderID}&returnUrl={encodedReturnUrl}&RecId={recId}', 1200);";

                        // Register the script
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupSalesTax", script, true);

                        break;
                    }
                }

                if (!found)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                        "alert('Please select a line to view Sales Tax.');", true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error opening Sales Tax page: " + ex.Message);
            }
        }


        protected void btnOnHand_Click(object sender, EventArgs e)
        {
            try
            {
                // Redirect to the Quality Order form
                Response.Redirect("/ESS/PR/PO_OnHand.aspx", false);
            }
            catch (Exception ex)
            {
                // Optional: Handle any unexpected errors gracefully
                // Example: log the error or show a notification message
                System.Diagnostics.Debug.WriteLine("Error redirecting to Quality Order: " + ex.Message);
            }


        }

        protected void btnUpdateLine_Click(object sender, EventArgs e)
        {
            bool found = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    found = true;

                    // ✅ Get RecId from selected row
                    string recIdText = (row.FindControl("lblRecId") as Label)?.Text.Trim();
                    long recId = 0;
                    if (!string.IsNullOrEmpty(recIdText) && long.TryParse(recIdText, out recId))
                    {
                        Session["RecId"] = recId; // ✅ Store RecId for cancel/update service call
                    }

                    // Retrieve other details
                    string lineNumber = (row.FindControl("lblLineNumber") as Label)?.Text.Trim();
                    string purchaseQty = (row.FindControl("lblDeliverRemainder") as Label)?.Text.Trim();
                    string inventoryQuantityText = (row.FindControl("lblInventoryQuantity") as Label)?.Text.Trim();
                    string purchaseOrderID = Session["PurchaseOrderId"] as string;

                    if (string.IsNullOrEmpty(purchaseOrderID))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoPOID",
                            "alert('Purchase Order ID not found in session.');", true);
                        return;
                    }

                    // ✅ Store for use in popup form
                    Session["LineNumber"] = lineNumber;
                    Session["DeliverRemainder"] = purchaseQty;
                    Session["InventoryQuantity"] = inventoryQuantityText;

                    // ✅ Open popup for Deliver Remainder
                    string script = "openPopupPanel('/ESS/PR/PurchaseOrder_DeliverRemainder.aspx', 450);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupDeliverRemainder", script, true);
                    break;
                }
            }

            if (!found)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select a line to update.');", true);
            }
        }


        protected void btnMaintainCharges_Click(object sender, EventArgs e)
        {
            try
            {
                bool found = false;

                // Loop through each row in the GridView
                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        found = true;

                        // ✅ Get RecId from the selected row
                        string recIdText = (row.FindControl("lblRecId") as Label)?.Text.Trim();
                        long recId = 0;

                        if (!string.IsNullOrEmpty(recIdText) && long.TryParse(recIdText, out recId))
                        {
                            // ✅ Store RecId in session for use in the Maintain Charges page
                            Session["RecId"] = recId;
                            string purchaseOrderID = Session["PurchaseOrderId"] as string;
                            Session["MaintainChargesSource"] = "LINE";


                            // ✅ Redirect to Maintain Charges page
                            Response.Redirect("/ESS/PR/PurchaseOrderLines_MaintainCharges.aspx", false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "InvalidRecId",
                                "alert('Invalid or missing RecId.');", true);
                        }

                        break; // Stop after the first checked row
                    }
                }

                // ✅ If no row is selected, show alert
                if (!found)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                        "alert('Please select a line to maintain charges.');", true);
                }
            }
            catch (Exception ex)
            {
                // Optional: log or show an error message
                System.Diagnostics.Debug.WriteLine("Error redirecting to Maintain Charges: " + ex.Message);
            }
        }

        protected void OnPurchQty_Changed(object sender, EventArgs e)
        {
            TextBox txtQuantity = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)txtQuantity.NamingContainer;

            // Parse quantity
            decimal qty = decimal.TryParse(txtQuantity.Text.Trim(), out decimal q) ? q : 0;

            TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
            decimal unitPrice = decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal p) ? p : 0;

            TextBox txtDiscount = (TextBox)gridRow.FindControl("txtDiscount");
            decimal discountValue = 0;
            decimal.TryParse(txtDiscount.Text, out discountValue);

            TextBox txtDiscountPercent = (TextBox)gridRow.FindControl("txtDiscountPercent");
            decimal discountValuePercentage = 0;
            decimal.TryParse(txtDiscountPercent.Text, out discountValuePercentage);


            TextBox txtNetAmount = (TextBox)gridRow.FindControl("txtNetAmount");
            decimal lineAmount = 0;
            decimal.TryParse(txtNetAmount.Text, out lineAmount);

            long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;

            DataTable dt = purchaselines.retrieveModifiedPurchQty(qty, recIdLine, discountValuePercentage, discountValue, unitPrice, lineAmount);

            if (dt != null && dt.Rows.Count > 0)
            {
                //txtNetAmount.Text = dt.Rows[0]["LineAmount"].ToString();
                txtNetAmount.Text = Convert.ToDecimal(dt.Rows[0]["LineAmount"]).ToString("0.00");

            }
        }

        protected void OnUnitPrice_Changed(object sender, EventArgs e)
        {
            TextBox txtUnitPrice = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)txtUnitPrice.NamingContainer;

            // Parse unit price
            decimal unitPrice = decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal p) ? p : 0;

            TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
            decimal Quantity = 0;
            decimal.TryParse(txtQuantity.Text, out Quantity);


            TextBox txtDiscount = (TextBox)gridRow.FindControl("txtDiscount");
            decimal discountValue = 0;
            decimal.TryParse(txtDiscount.Text, out discountValue);

            TextBox txtDiscountPercent = (TextBox)gridRow.FindControl("txtDiscountPercent");
            decimal discountValuePercentage = 0;
            decimal.TryParse(txtDiscountPercent.Text, out discountValuePercentage);

            TextBox txtNetAmount = (TextBox)gridRow.FindControl("txtNetAmount");

            long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;

            DataTable dt = purchaselines.retrieveModifiedPurchPrice(unitPrice, recIdLine, Quantity, discountValuePercentage, discountValue);

            if (dt != null && dt.Rows.Count > 0)
            {
                //txtNetAmount.Text = dt.Rows[0]["LineAmount"].ToString();
                txtNetAmount.Text = Convert.ToDecimal(dt.Rows[0]["LineAmount"]).ToString("0.00");
            }

        }

        protected void OnLineAmount_Changed(object sender, EventArgs e)
        {
            TextBox txtNetAmount = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)txtNetAmount.NamingContainer;

            TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
            if (txtUnitPrice != null)
            {
                txtUnitPrice.Text = string.Empty;
            }
        }

        protected void OnDiscountValue_Changed(object sender, EventArgs e)
        {
            TextBox txtDiscount = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)txtDiscount.NamingContainer;

            TextBox txtLineAmount = (TextBox)gridRow.FindControl("txtNetAmount");

            long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;

            decimal discountValue = 0;
            decimal.TryParse(txtDiscount.Text, out discountValue);

            TextBox txtDiscountPercent = (TextBox)gridRow.FindControl("txtDiscountPercent");
            decimal discountValuePercentage = 0;
            decimal.TryParse(txtDiscountPercent.Text, out discountValuePercentage);

            TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
            decimal UnitPrice = 0;
            decimal.TryParse(txtUnitPrice.Text, out UnitPrice);


            TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
            decimal Quantity = 0;
            decimal.TryParse(txtQuantity.Text, out Quantity);



            DataTable dt = purchaselines.retrieveModifiedDiscount(discountValue, recIdLine, discountValuePercentage, UnitPrice, Quantity);

            if (dt != null && dt.Rows.Count > 0)
            {
                //txtLineAmount.Text = dt.Rows[0]["LineAmount"].ToString();
                txtLineAmount.Text = Convert.ToDecimal(dt.Rows[0]["LineAmount"]).ToString("0.00");
            }

        }

        protected void OnDiscountPercent_Changed(object sender, EventArgs e)
        {
            TextBox txtDiscountPercent = (TextBox)sender;
            GridViewRow gridRow = (GridViewRow)txtDiscountPercent.NamingContainer;

            TextBox txtLineAmount = (TextBox)gridRow.FindControl("txtNetAmount");

            long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;

            decimal discountValuePercentage = 0;
            decimal.TryParse(txtDiscountPercent.Text, out discountValuePercentage);

            TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
            decimal UnitPrice = 0;
            decimal.TryParse(txtUnitPrice.Text, out UnitPrice);


            TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
            decimal Quantity = 0;
            decimal.TryParse(txtQuantity.Text, out Quantity);

            TextBox txtDiscount = (TextBox)gridRow.FindControl("txtDiscount");
            decimal discountValue = 0;
            decimal.TryParse(txtDiscount.Text, out discountValue);



            DataTable dt = purchaselines.retrieveModifiedDiscount(discountValue, recIdLine, discountValuePercentage, UnitPrice, Quantity);

            if (dt != null && dt.Rows.Count > 0)
            {
                //txtLineAmount.Text = dt.Rows[0]["LineAmount"].ToString();
                txtLineAmount.Text = Convert.ToDecimal(dt.Rows[0]["LineAmount"]).ToString("0.00");
            }
        }

    }
}
