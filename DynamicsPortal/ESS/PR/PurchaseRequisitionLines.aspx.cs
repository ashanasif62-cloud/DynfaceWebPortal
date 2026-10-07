using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSDashboardSvcReference;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.ExpenseManagmentSvc;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequisitionLines : MainForm
    {
        private PurchaseRequisitionLine purchaseRequisitionLine = new PurchaseRequisitionLine();

        protected void Page_Init(object sender, EventArgs e)
        {
            // Always rebuild dynamic controls early
            showFinancialDimension(0);
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {

                tableId = purchaseRequisitionLine.tableName;
                pageMenuId = "PurchasedRequisitionLine";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Purchase requisition details";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    string newpurchReqId = Session["PurchReqId"] as string;
                    string purchReqName = Session["PurchReqName"] as string;
                    string requisitionStatus = Session["RequisitionStatus"] as string;

                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Purchase requisition details";
                        titleDiv.Style["font-weight"] = "bold"; // Set the style to bold

                    }
                    lblRequisitionName.Text = purchReqName;
                    lblRequisitionNumber.Text = newpurchReqId;
                    lblRequisitionStatus.Text = requisitionStatus;
                    // Make text bold and increase font size
                    lblRequisitionName.Style["font-weight"] = "bold";
                    lblRequisitionName.Style["font-size"] = "20px";

                    lblRequisitionNumber.Style["font-weight"] = "bold";
                    lblRequisitionNumber.Style["font-size"] = "20px";

                    lblRequisitionStatus.Style["font-weight"] = "bold";
                    lblRequisitionStatus.Style["font-size"] = "20px";

                    BindHeaderValues();
                    BindReason();


                    string status = headerStatus.Text.Trim();

                    if (status == "In review")
                    {
                        btnGoToRFQ.Enabled = true;
                        BtnAddLine.Enabled = false;
                        btnDelete.Enabled = false;
                        BtnAddLine.CssClass = "disabled-button";
                        btnDelete.CssClass = "disabled-button";
                        BtnDeleteHeader.Enabled = false;
                        ddlReason.Enabled = false;
                        btnSubmit.Enabled = false;
                        btnWorkflow.Enabled = false;
                        btnWorkflow.CssClass = "disabled-button;";
                        btnSubmit.CssClass = "disabled-button";
                        btnCancel.Enabled = false;
                        btnCancel.CssClass = "disabled-button";

                    }
                    else if (status == "Approved")
                    {
                        BtnAddLine.Enabled = false;
                        btnGoToRFQ.Enabled = false;
                        btnDelete.Enabled = false;
                        BtnAddLine.CssClass = "disabled-button";
                        btnDelete.CssClass = "disabled-button";
                        BtnDeleteHeader.Enabled = false;
                        ddlReason.Enabled = false;
                        btnSubmit.Enabled = false;
                        btnWorkflow.Enabled = false;
                        btnWorkflow.CssClass = "disabled-button;";
                        btnSubmit.CssClass = "disabled-button";
                        btnCancel.Enabled = true;


                    }
                    else
                    {
                        btnGoToRFQ.Enabled = false;
                        BtnAddLine.Enabled = true;
                        btnDelete.Enabled = true;
                        BtnDeleteHeader.Enabled = true;
                        ddlReason.Enabled = true;
                        btnWorkflow.Enabled = true;
                        btnSubmit.Enabled = true;
                        btnCancel.Enabled = false;
                        btnCancel.CssClass = "disabled-button";

                    }


                    string purchReqId = Session["PurchReqId"] as string;
                    DataTable dt = purchaseRequisitionLine.retrieveAll(purchReqId);

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

                    setFieldsEmpty();
                    if (gridView.Rows.Count > 0)
                    {
                        GridViewRow firstRow = gridView.Rows[0];

                        // Tick the first row's checkbox
                        CheckBox chk = firstRow.FindControl("chk_SelectSingle") as CheckBox;
                        if (chk != null) chk.Checked = true;

                        chk_SelectSingle_CheckedChanged(chk, EventArgs.Empty);
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
        }

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        private void getGridDataTable()
        {
            string purchReqId = Session["PurchReqId"] as string;
            DataTable dt = purchaseRequisitionLine.retrieveAll(purchReqId);
            SessionVariables.setSessionDataTable(dt);
        }


        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string purchReqId = Session["PurchReqId"] as string;
            DataTable dt = purchaseRequisitionLine.retrieveAll(purchReqId);

            gridView.EditIndex = e.NewEditIndex;
            gridView.DataSource = dt;
            gridView.DataBind();

            // Mark this as Edit mode
            EditMode.Value = "Edit";
        }

        protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridView.EditIndex = -1;
            EditMode.Value = string.Empty;
            reBindGrid(); // refresh grid from DB
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                    var dataItem = (DataRowView)e.Row.DataItem;


                    DateTime requiredDate = Convert.ToDateTime(Session["RequiredDate"]);



                    DropDownList ddlReceivingUnitRecId = (DropDownList)e.Row.FindControl("ddlReceivingOperatingUnitRecId");
                    long receivingUnitRecId = 0;
                    if (ddlReceivingUnitRecId != null && !string.IsNullOrEmpty(ddlReceivingUnitRecId.SelectedValue))
                        Int64.TryParse(ddlReceivingUnitRecId.SelectedValue, out receivingUnitRecId);


                    DropDownList ddlItemid = (DropDownList)e.Row.FindControl("ddlItemId");
                    if (ddlItemid != null)
                    {
                        PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                        DataTable dt = service.retrieveAllItemID(receivingUnitRecId, requiredDate);

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

                        ddlItemid.CssClass += "filterable-dropdown";

                        // 👇 Preselect the existing value when editing
                        if (dataItem["ItemId"] != DBNull.Value)
                        {
                            Label Itemid = (Label)e.Row.FindControl("lblItemIdEdit");
                            string currentValue = dataItem["ItemId"].ToString();
                            if (ddlItemid.Items.FindByValue(currentValue) != null)
                            {

                                // 👇 hide dropdown and show label
                                ddlItemid.Visible = false;
                                Itemid.Text = currentValue;
                                Itemid.Visible = true;
                            }
                        }
                    }


                    TextBox txtQuantity = (TextBox)e.Row.FindControl("txtPurchQty");
                    if (txtQuantity != null && dataItem["PurchQty"] != DBNull.Value)
                    {
                        txtQuantity.Text = dataItem["PurchQty"].ToString();
                    }


                    TextBox txtUnitPrice = (TextBox)e.Row.FindControl("txtUnitPrice");
                    if (txtUnitPrice != null && dataItem["PurchPrice"] != DBNull.Value)
                    {
                        txtUnitPrice.Text = Convert.ToDecimal(dataItem["PurchPrice"]).ToString("0.00");
                    }


                    TextBox txtPurchUnitofMeasure = (TextBox)e.Row.FindControl("txtPurchUnitofMeasure");
                    if (txtPurchUnitofMeasure != null && dataItem["PurchUnitofMeasure"] != DBNull.Value)
                    {
                        txtPurchUnitofMeasure.Text = dataItem["PurchUnitofMeasure"].ToString();
                    }


                    TextBox txtCurrencyCode = (TextBox)e.Row.FindControl("txtCurrencyCode");
                    if (txtCurrencyCode != null && dataItem["CurrencyCode"] != DBNull.Value)
                    {
                        txtCurrencyCode.Text = dataItem["CurrencyCode"].ToString();
                    }


                    //DropDownList ddlDepartment = (DropDownList)e.Row.FindControl("ddlReceivingOperatingUnit");
                    //if (ddlDepartment != null)
                    //{
                    //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                    //    DataTable dt = service.retrievedepartment();

                    //    ddlDepartment.DataSource = dt;
                    //    ddlDepartment.DataTextField = "DepartmentName";
                    //    ddlDepartment.DataValueField = "DepartmentName";
                    //    ddlDepartment.DataBind();

                    //    ddlDepartment.Items.Insert(0, new ListItem("", String.Empty));
                    //    ddlDepartment.CssClass += "filterable-dropdown";
                    //    ddlDepartment.Enabled = false;  
                    //}

                    //DropDownList ddlReceivingOperatingUnitRecId = (DropDownList)e.Row.FindControl("ddlReceivingOperatingUnitRecId");
                    //if (ddlReceivingOperatingUnitRecId != null)
                    //{
                    //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                    //    DataTable dt = service.retrievedepartment();

                    //    ddlReceivingOperatingUnitRecId.DataSource = dt;
                    //    ddlReceivingOperatingUnitRecId.DataTextField = "DepartmentName";
                    //    ddlReceivingOperatingUnitRecId.DataValueField = "DepartmentRecId";
                    //    ddlReceivingOperatingUnitRecId.DataBind();

                    //    ddlReceivingOperatingUnitRecId.Items.Insert(0, new ListItem("", String.Empty));
                    //    ddlReceivingOperatingUnitRecId.Enabled = false;
                    //}

                    //DropDownList ddlUnitofMeasure = (DropDownList)e.Row.FindControl("ddlProductUnit");
                    //if (ddlItemid != null)
                    //{
                    //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                    //    DataTable dt = service.retrieveunitofmeasure();

                    //    ddlUnitofMeasure.DataSource = dt;
                    //    ddlUnitofMeasure.DataTextField = "Unitofmeasuresymbol";
                    //    ddlUnitofMeasure.DataValueField = "Unitofmeasuresymbol";
                    //    ddlUnitofMeasure.DataBind();

                    //    ddlUnitofMeasure.Items.Insert(0, new ListItem("", String.Empty));
                    //    ddlUnitofMeasure.CssClass += " filterable-dropdown";

                    //}

                    //DropDownList ddlvendoraccnum = (DropDownList)e.Row.FindControl("ddlVendAccount");
                    //if (ddlItemid != null)
                    //{
                    //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                    //    DataTable dt = service.retrieveVendor();

                    //    ddlvendoraccnum.DataSource = dt;
                    //    ddlvendoraccnum.DataTextField = "VendAccount";
                    //    ddlvendoraccnum.DataValueField = "VendAccount";
                    //    ddlvendoraccnum.DataBind();
                    //    ddlvendoraccnum.Items.Insert(0, new ListItem("", String.Empty));
                    //    ddlvendoraccnum.Attributes["onchange"] = "fetchVendorName(this)";
                    //    ddlvendoraccnum.CssClass += " filterable-dropdown";
                    //}
                    TextBox txtVendAccount = (TextBox)e.Row.FindControl("txtVendAccount");
                    DropDownList ddlVendAccount = (DropDownList)e.Row.FindControl("ddlVendAccount");

                    // 🔹 CASE 1: Editing an existing row
                    if (dataItem["RecId"] != DBNull.Value && Convert.ToInt64(dataItem["RecId"]) > 0)
                    {


                        // Show dropdown, hide textbox
                        txtVendAccount.Visible = false;
                        ddlVendAccount.Visible = true;

                        PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                        DataTable dt = service.retrieveVendor();

                        ddlVendAccount.DataSource = dt;
                        ddlVendAccount.DataTextField = "VendAccount";
                        ddlVendAccount.DataValueField = "VendAccount";
                        ddlVendAccount.DataBind();
                        ddlVendAccount.Items.Insert(0, new ListItem("", String.Empty));

                        // Preselect current vendor
                        if (dataItem["VendAccount"] != DBNull.Value)
                        {
                            string currentVend = dataItem["VendAccount"].ToString();
                            if (ddlVendAccount.Items.FindByValue(currentVend) != null)
                            {
                                ddlVendAccount.SelectedValue = currentVend;
                            }
                        }

                        ddlVendAccount.Enabled = true;
                        ddlVendAccount.CssClass += " filterable-dropdown";
                    }
                    // 🔹 CASE 2: Adding a new row
                    else
                    {
                        // Show textbox, hide dropdown
                        txtVendAccount.Visible = true;
                        ddlVendAccount.Visible = false;
                    }

                    //DropDownList ddlCurrencyCode = (DropDownList)e.Row.FindControl("ddlCurrencyCode");
                    //if (ddlItemid != null)
                    //{
                    //    DataTable dt = ControlsHelper.retrieveAllCurrencyDetails();

                    //    ddlCurrencyCode.DataSource = dt;
                    //    ddlCurrencyCode.DataTextField = "CurrencyCode";
                    //    ddlCurrencyCode.DataValueField = "CurrencyCode";
                    //    ddlCurrencyCode.DataBind();
                    //    ddlCurrencyCode.Items.Insert(0, new ListItem("", String.Empty));
                    //    ddlCurrencyCode.CssClass += " filterable-dropdown";
                    //}



                    DropDownList ddlRequisitioner = (DropDownList)e.Row.FindControl("ddlRequisitioner");
                    if (ddlRequisitioner != null)
                    {
                        string requisitioner = Session["Originator"] as string;
                        ddlRequisitioner.Items.Insert(0, new ListItem(requisitioner, requisitioner));
                        ddlRequisitioner.Enabled = false;
                    }

                    DropDownList ddlBuyingLegalEntity = (DropDownList)e.Row.FindControl("ddlBuyingLegalEntity");
                    if (ddlBuyingLegalEntity != null)
                    {
                        string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                        ddlBuyingLegalEntity.Items.Insert(0, new ListItem(dataAreaId, dataAreaId));
                        ddlBuyingLegalEntity.Enabled = false;
                    }


                }

                if (string.Equals(lblRequisitionStatus.Text, "Draft"))
                {
                    LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                    if (btnEdit != null)
                        btnEdit.Enabled = true;
                }
                else
                {
                    LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                    if (btnEdit != null)
                        btnEdit.Enabled = false;
                }
            }
        }

        //protected void btnNewLines_Click(object sender, EventArgs e)
        //{
        //    string requisitioner = string.Empty;

        //    if (gridView.Rows.Count > 0)
        //    {
        //        GridViewRow firstRow = gridView.Rows[0];
        //        Label lblRequisitioner = (Label)firstRow.FindControl("lblRequisitioner");

        //        if (lblRequisitioner != null)
        //        {
        //            requisitioner = lblRequisitioner.Text.Trim();
        //        }
        //    }

        //    string targetUrl = "/ESS/PR/PurchaseRequisitionLineCreate.aspx?requisitioner=" + Server.UrlEncode(requisitioner);

        //    // Open in popup using JavaScript injected from server
        //    string script = $@"
        //    <script type='text/javascript'>
        //        window.open('{targetUrl}', 'popupWindow', 'width=900,height=600,resizable=yes,scrollbars=yes');
        //    </script>";

        //    ClientScript.RegisterStartupScript(this.GetType(), "openPopup", script);
        //}


        //}


        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool anyChecked = false;
            GridViewRow selectedRow = null;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow == sender && chkSelectRow.Checked)
                {
                    anyChecked = true;
                    selectedRow = row;
                }
            }

            if (anyChecked && selectedRow != null)
            {


                // Item Tab
                txtItemNumber.Text = GetLabelText(selectedRow, "ItemId");
                txtItemDescription.Text = GetLabelText(selectedRow, "PrdouctName");

                txtLineType.Text = GetLabelText(selectedRow, "lblLineType");
                txtInternetAddress.Text = GetLabelText(selectedRow, "lblInternetAddress");
                txtExternalItemNumber.Text = GetLabelText(selectedRow, "itemExternalItemId");
                txtProductName.Text = GetLabelText(selectedRow, "PrdouctName");
                txtSupplierPartAuxID.Text = GetLabelText(selectedRow, "lblPurchSupplierAuxId");

                txtCategory.Text = GetLabelText(selectedRow, "lblCategory");
                txtCode.Text = GetLabelText(selectedRow, "CategoryCode");

                txtQuantity.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "PurchQty")));
                txtUnit.Text = GetLabelText(selectedRow, "ProductUnit");
                txtUnitPrice.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "UnitPrice")));
                txtNetAmount.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "NetAmount")));
                txtCurrency.Text = GetLabelText(selectedRow, "CurrencyCode");
                txtPriceUnit.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "lblPriceUnit")));

                txtDiscountPercent.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "lblDiscountPercent")));
                txtDiscount.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "lblDiscount")));
                txtChargesOnPurchases.Text = string.Format("{0:N2}", Convert.ToDecimal(GetLabelText(selectedRow, "lblChargesOnPurchases")));

                txtVendorName.Text = GetLabelText(selectedRow, "VendorName");
                txtVendorAccount.Text = GetLabelText(selectedRow, "VendAccount");
                txtVendorStatus.Text = GetLabelText(selectedRow, "VendorStatus");
                txtRFQRequirement.Text = GetLabelText(selectedRow, "RFQRequirement");


                // General Tab
                txtRequester.Text = GetLabelText(selectedRow, "lblRequisitioner");
                txtBuyingLegalEntity.Text = GetLabelText(selectedRow, "lblBuyingLegalEntity");

                txtReceivingOperatingUnit.Text = GetLabelText(selectedRow, "lblReceivingOperatingUnit");
                txtStatus.Text = GetLabelText(selectedRow, "RequisitionStatus");

                txtRequestedDate.Text = FormatDateLabel(Session["RequiredDate"].ToString());
                txtAccountingDate.Text = FormatDateLabel(Session["TransDate"].ToString());

                ddlReason.Text = Session["Reason"] != null ? Session["Reason"].ToString() : string.Empty;
                txtDetails.Text = Session["Details"] != null ? Session["Details"].ToString() : string.Empty;
                // DELIVERY TERMS
                chkPreventPartialDelivery.Checked = GetLabelText(selectedRow, "lblPreventPartialDeliveryFlag") == "Yes";
                chkPrepaymentRequired.Checked = GetLabelText(selectedRow, "lblPrePaymentRequired") == "Yes";
                txtPrepaymentDetails.Text = GetLabelText(selectedRow, "lblPrepaymentDetails");

                // PURCHASE ORDER CREATION
                txtPriceDiscountTransfer.Text = GetLabelText(selectedRow, "lblPriceDiscountTransfer");
                // Notes (greyed-out multiline field)
                txtPurchaseOrderNotes.Text = GetLabelText(selectedRow, "lblPurchaseOrderNotes");

                // REFERENCES
                txtRFQCase.Text = GetLabelText(selectedRow, "lblRequestforQuotationCase");
                txtRFQCaseStatus.Text = GetLabelText(selectedRow, "lblRequestforQuotationCaseStatus");
                txtConsolidationOpportunity.Text = GetLabelText(selectedRow, "lblConsolidationOppurtunityId");

                // CONSOLIDATION & PURCHASE ORDER
                txtConsolidationOpportunityStatus.Text = GetLabelText(selectedRow, "lblConsolidationOppurtunityIdStatus");
                txtPurchaseOrder.Text = GetLabelText(selectedRow, "lblPurchaseOrder");
                txtPurchaseOrderStatus.Text = GetLabelText(selectedRow, "lblPurchaseOrderStatus");

                // INVOICE & AGREEMENT
                txtInvoice.Text = GetLabelText(selectedRow, "lblInvoice");
                txtInvoiceStatus.Text = GetLabelText(selectedRow, "lblInvoiceStatus");
                txtPurchaseAgreement.Text = GetLabelText(selectedRow, "lblPurchaseAgreementId");
                txtPurchaseAgreementStatus.Text = GetLabelText(selectedRow, "lblPurchaseAgreementStatus");

                // SALES TAX
                txtItemSalesTaxGroup.Text = GetLabelText(selectedRow, "lblItemSalesTaxGroup");
                txtSalesTaxGroup.Text = GetLabelText(selectedRow, "lblProjectTaxGroupId");

                // Delivery Tab
                txtDeliveryName.Text = GetLabelText(selectedRow, "lblDeliveryAddress");
                txtDeliveryAddress.Text = GetLabelText(selectedRow, "lblDeliveryPostalAddress");
                txtAddress.Text = GetLabelText(selectedRow, "lblDeliveryPostalAddress");

                // Project Identification
                txtProjectID.Text = GetLabelText(selectedRow, "ProjId");
                txtActivityNumber.Text = GetLabelText(selectedRow, "ActivityNumber");

                // Project Details
                txtProjectCategory.Text = GetLabelText(selectedRow, "ProjectCategory");
                txtProjectItemNumber.Text = GetLabelText(selectedRow, "ItemId");
                txtProjectLineProperty.Text = GetLabelText(selectedRow, "lblLineType");

                // TRANSACTION
                txtTransactionID.Text = GetLabelText(selectedRow, "lblTransactionID");

                // COST PRICE
                txtCostPriceQuantity.Text = GetLabelText(selectedRow, "PurchQty");
                txtlineUnitPrice.Text = GetLabelText(selectedRow, "UnitPrice");
                TxtlinenetAmount.Text = GetLabelText(selectedRow, "NetAmount");

                // SALES PRICE
                txtSalesCurrency.Text = GetLabelText(selectedRow, "CurrencyCode");
                txtSalesPrice.Text = GetLabelText(selectedRow, "ProjSalesPrice");
                txtSalesUnit.Text = GetLabelText(selectedRow, "ProjSalesUnit");

                // PROJECT - SALES TAX
                txtProjectSalesTaxGroup.Text = GetLabelText(selectedRow, "ProjectTaxGroupId");
                txtProjectSalesItemTaxGroup.Text = GetLabelText(selectedRow, "ProjectItemTaxGroupId");

                ////PRODUCT DIMENSIONS
                //txtConfiguration.Text = GetLabelText(selectedRow, "lblConfigId");
                //txtSize.Text = GetLabelText(selectedRow, "lblInventSizeId");
                //txtStyle.Text = GetLabelText(selectedRow, "lblInventStyleId");
                //txtColor.Text = GetLabelText(selectedRow, "lblInventColorId");
                txtVersionLineDetail.Text = GetLabelText(selectedRow, "InventName");
                // ⚠️ Note: Your original code had "Style" – no Style textbox exists in markup, so I mapped to Version

                //// INVENTORY DIMENSIONS
                //txtSite.Text = GetLabelText(selectedRow, "lblInventSiteId");
                //txtWarehouse.Text = GetLabelText(selectedRow, "lblInventLocationId");

                //txtBatchNumber.Text = GetLabelText(selectedRow, "lblInventBatchId");
                //txtLocation.Text = GetLabelText(selectedRow, "lblWMSLocationId");

                //txtSerialNumber.Text = GetLabelText(selectedRow, "lblInventSerialId");
                //txtInventoryStatus.Text = GetLabelText(selectedRow, "InventStatusId");

                //txtLicensePlate.Text = GetLabelText(selectedRow, "LicensePlateId");
                //txtOwner.Text = GetLabelText(selectedRow, "OwnerId");
                //txtInventoryProfile.Text = GetLabelText(selectedRow, "InventoryProfileId");

                string ItemId = GetLabelText(selectedRow, "ItemId");


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
                string configId = GetLabelText(selectedRow, "lblConfigId");
                if (ddlConfigurationLineDetail.Items.FindByValue(configId) != null)
                {
                    ddlConfigurationLineDetail.SelectedValue = configId;
                }
                else
                {
                    ddlConfigurationLineDetail.Attributes["readonly"] = "readonly";
                    ddlConfigurationLineDetail.CssClass += " custom-textbox";
                }

                string sizeId = GetLabelText(selectedRow, "lblInventSizeId");
                if (ddlSizeLineDetail.Items.FindByValue(sizeId) != null)
                {
                    ddlSizeLineDetail.SelectedValue = sizeId;
                }
                else
                {
                    ddlSizeLineDetail.Attributes["readonly"] = "readonly";
                    ddlSizeLineDetail.CssClass += " custom-textbox";
                }

                string styleId = GetLabelText(selectedRow, "lblInventStyleId");
                if (ddlStyleLineDetail.Items.FindByValue(styleId) != null)
                {
                    ddlStyleLineDetail.SelectedValue = styleId;
                }
                else
                {
                    ddlStyleLineDetail.Attributes["readonly"] = "readonly";
                    ddlStyleLineDetail.CssClass += " custom-textbox";
                }

                string colorId = GetLabelText(selectedRow, "lblInventColorId");
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
                string siteId = GetLabelText(selectedRow, "lblInventSiteId");
                if (ddlSiteLineDetail.Items.FindByValue(siteId) != null)
                {
                    ddlSiteLineDetail.SelectedValue = siteId;
                }
                else
                {
                    ddlSiteLineDetail.Attributes["readonly"] = "readonly";
                    ddlSiteLineDetail.CssClass += " custom-textbox";
                }

                string warehouseId = GetLabelText(selectedRow, "lblInventLocationId");
                if (ddlWarehouseLineDetail.Items.FindByValue(warehouseId) != null)
                {
                    ddlWarehouseLineDetail.SelectedValue = warehouseId;
                }
                else
                {
                    ddlWarehouseLineDetail.Attributes["readonly"] = "readonly";
                    ddlWarehouseLineDetail.CssClass += " custom-textbox";
                }

                string batchId = GetLabelText(selectedRow, "lblInventBatchId");
                if (ddlBatchNumberLineDetail.Items.FindByValue(batchId) != null)
                {
                    ddlBatchNumberLineDetail.SelectedValue = batchId;
                }
                else
                {
                    ddlBatchNumberLineDetail.Attributes["readonly"] = "readonly";
                    ddlBatchNumberLineDetail.CssClass += " custom-textbox";
                }

                string wmsLocationId = GetLabelText(selectedRow, "lblWMSLocationId");
                if (ddlWmsLocationLineDetail.Items.FindByValue(wmsLocationId) != null)
                {
                    ddlWmsLocationLineDetail.SelectedValue = wmsLocationId;
                }
                else
                {
                    ddlWmsLocationLineDetail.Attributes["readonly"] = "readonly";
                    ddlWmsLocationLineDetail.CssClass += " custom-textbox";
                }

                string serialId = GetLabelText(selectedRow, "lblInventSerialId");
                if (ddlSerialNumberLineDetail.Items.FindByValue(serialId) != null)
                {
                    ddlSerialNumberLineDetail.SelectedValue = serialId;
                }
                else
                {
                    ddlSerialNumberLineDetail.Attributes["readonly"] = "readonly";
                    ddlSerialNumberLineDetail.CssClass += " custom-textbox";
                }


                string recIdText = GetLabelText(selectedRow, "lblRecId");
                if (!string.IsNullOrEmpty(recIdText))
                {
                    if (long.TryParse(recIdText, out long recId))
                    {
                        Session["RecIdline"] = recId;  // ✅ store as long, not string
                    }
                }

                string defaultDimensionRecIdStr = GetLabelText(selectedRow, "lblDefaultDimension");
                long defaultDimensionRecId = 0;
                showFinancialDimension(defaultDimensionRecId);


                if (!string.IsNullOrEmpty(defaultDimensionRecIdStr))
                    long.TryParse(defaultDimensionRecIdStr, out defaultDimensionRecId);

                if (defaultDimensionRecId > 0)
                {
                    DataTable dimTable = purchaseRequisitionLine.retrievefinancialdimension(defaultDimensionRecId);
                    bindItemDimension(defaultDimensionRecId);
                    if (dimTable != null && dimTable.Rows.Count > 0)
                    {
                        DataRow row = dimTable.Rows[0];

                        //FinancialDimensionBusinessUnit.Text = row.Table.Columns.Contains("BusinessUnit")
                        //                                       ? row["BusinessUnit"].ToString() : "N/A";

                        //FinancialDimensionCostCenter.Text = row.Table.Columns.Contains("CostCenter")
                        //                                       ? row["CostCenter"].ToString() : "N/A";

                        //FinancialDimensionDepartment.Text = row.Table.Columns.Contains("Department")
                        //                                       ? row["Department"].ToString() : "N/A";

                        //FinancialDimensionItemGroup.Text = row.Table.Columns.Contains("ItemGroup")
                        //                                       ? row["ItemGroup"].ToString() : "N/A";

                        //FinancialDimensionMainAccount.Text = row.Table.Columns.Contains("MainAccount")
                        //                                       ? row["MainAccount"].ToString() : "N/A";

                        //FinancialDimensionProject.Text = row.Table.Columns.Contains("Project")
                        //                                       ? row["Project"].ToString() : "N/A";
                    }
                }
            }

            else
            {
                setFieldsEmpty();
            }
        }


        private string GetLabelText(Control row, string labelId)
        {

            var label = row.FindControl(labelId) as Label;

            var text = label?.Text?.Trim();

            return string.IsNullOrEmpty(text) ? "\uFEFF" : text;

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string purchReqId = Session["PurchReqId"] as string;
            DataTable dt = purchaseRequisitionLine.retrieveAll(purchReqId);

            // 2. Add a new blank row at the top
            DataRow newRow = dt.NewRow();
            dt.Rows.InsertAt(newRow, 0);

            // 3. Put GridView in edit mode for the first row
            gridView.EditIndex = 0;

            // 5. Rebind the GridView
            gridView.DataSource = dt;
            gridView.DataBind();

            // Mark this as Add mode
            EditMode.Value = "Add";


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
                SysOperationResult_BOL operationResult_BOL = purchaseRequisitionLine.deletelines(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    reBindGrid();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                "alert('Please select at least one Purchase line to proceed.');", true);
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            GridViewRow gridRow = gridView.Rows[gridView.EditIndex];
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
                    DropDownList ddlRequisitioner = (DropDownList)gridRow.FindControl("ddlRequisitioner");
                    string requisitioner = ddlRequisitioner.Text.Trim();

                    TextBox txtPurchQty = (TextBox)gridRow.FindControl("txtPurchQty");
                    string qtyText = txtPurchQty.Text.Trim();

                    DropDownList ddlBuyingLegalEntity = (DropDownList)gridRow.FindControl("ddlBuyingLegalEntity");
                    string dataAreaId = ddlBuyingLegalEntity.Text.Trim();

                    if (string.IsNullOrWhiteSpace(requisitioner))
                    {
                        NotificationMessage.showMessage("Requester is required.");
                        return;
                    }

                    if (!decimal.TryParse(qtyText, out decimal purchQty))
                    {
                        NotificationMessage.showMessage("Quantity must be a valid number.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(dataAreaId))
                    {
                        NotificationMessage.showMessage("DataArea Id is required.");
                        return;
                    }


                    // Step 2: Prepare the DataTable
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Requisitioner", typeof(string));
                    dt.Columns.Add("PurchReqId", typeof(string));
                    dt.Columns.Add("ItemId");
                    dt.Columns.Add("PurchQty", typeof(decimal));
                    dt.Columns.Add("DataAreaId", typeof(string)); // ✅ New
                    dt.Columns.Add("DepartmentName", typeof(string));
                    dt.Columns.Add("VendAccount", typeof(string));
                    dt.Columns.Add("VendorName", typeof(string));
                    dt.Columns.Add("OperatingUnitNumber", typeof(string));
                    dt.Columns.Add("DepartmentRecId", typeof(string));
                    dt.Columns.Add("Unitofmeasuresymbol", typeof(string));
                    //dt.Columns.Add("PurchUnitOfMeasure", typeof(decimal));
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
                    dt.Columns.Add("InventLocation");

                    DataRow row = dt.NewRow();

                    DropDownList ddlItemId = (DropDownList)gridRow.FindControl("ddlItemId");
                    //DropDownList ddlReceivingOperatingUnit = (DropDownList)gridRow.FindControl("ddlReceivingOperatingUnit");
                    //DropDownList ddlReceivingOperatingUnitRecId = (DropDownList)gridRow.FindControl("ddlReceivingOperatingUnitRecId");
                    DropDownList ddlVendAccount = (DropDownList)gridRow.FindControl("ddlVendAccount");
                    DropDownList ddlProductUnit = (DropDownList)gridRow.FindControl("ddlProductUnit");
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


                    row["Requisitioner"] = SessionVariables.getCurrentEmployeeId();
                    row["ItemId"] = ddlItemId.SelectedValue;
                    row["PurchQty"] = purchQty;
                    row["DataAreaId"] = dataAreaId; // ✅ New
                    row["PurchReqId"] = Session["PurchReqId"];
                    //row["DepartmentRecId"] = ddlReceivingOperatingUnitRecId.SelectedValue;
                    //row["DepartmentName"] = ddlReceivingOperatingUnit.SelectedValue;
                    //row["VendAccount"] = ddlVendAccount.SelectedValue;
                    //row["VendorName"] = ddlVendAccount.Text;
                    //row["Unitofmeasuresymbol"] = ddlProductUnit.SelectedValue;
                    //dr["PurchUnitOfMeasure"] = ddlProductUnit.SelectedValue;

                    row["InventBatchId"] = ddlInventBatchId.SelectedValue;
                    row["WmsLocationId"] = ddlWMSLocationId.SelectedValue;
                    row["WmsPalletId"] = ddlWmsPalletId.SelectedValue;
                    row["InventSerialId"] = ddlInventSerialId.SelectedValue;
                    //string fromWarehouse = Session["FromWarehouse"] as string;
                    //row["InventLocationId"] = fromWarehouse;
                    row["ConfigId"] = ddlConfigId.SelectedValue;
                    row["InventSizeId"] = ddlSizeId.SelectedValue;
                    row["InventColorId"] = ddlColorId.SelectedValue;
                    row["InventSiteId"] = ddlInventSiteId.SelectedValue;
                    row["InventStyle"] = ddlStyleId.SelectedValue;
                    row["InventSiteId"] = ddlInventSiteId.SelectedValue;
                    row["InventLocation"] = ddlInventLocationId.SelectedValue;

                    dt.Rows.Add(row);

                    Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
                {
                    { "Batch number",("ddlInventBatchId", BindInventBatchId(ddlItemId.SelectedValue, gridRow) )},
                    { "Location", ("ddlWMSLocationId", BindWmsLocationId(ddlItemId.SelectedValue, gridRow)) },
                    { "Serial number", ("ddlInventSerialId", BindInventSerialId(ddlItemId.SelectedValue, gridRow)) },
                    { "Configuration", ("ddlConfigId", BindConfigId(ddlItemId.SelectedValue, gridRow)) },
                    { "Size", ("ddlInventSizeId", BindInventSizeId(ddlItemId.SelectedValue, gridRow)) },
                    { "Color", ("ddlInventColorId", BindInventColorId(ddlItemId.SelectedValue, gridRow)) },
                    { "Style", ("ddlInventStyleId", BindInventStyleId(ddlItemId.SelectedValue, gridRow)) },
                    { "Combinations", ("", BindCombination(ddlItemId.SelectedValue, gridRow)) },
                    { "WMS Pallet", ("ddlWMSPalletId", BindWmsPalletId(ddlItemId.SelectedValue, gridRow)) },
                    { "Site", ("ddlInventSiteId", BindInventSiteId(gridRow)) },
                    { "Warehouse", ("ddlInventLocationId", BindInventLocationId(/*row["InventSiteId"].ToString(),*/ gridRow)) }
                };

                    ddlInventBatchId.SelectedValue = row["InventBatchId"]?.ToString();
                    ddlWMSLocationId.SelectedValue = row["WmsLocationId"]?.ToString();
                    ddlWmsPalletId.SelectedValue = row["WmsPalletId"]?.ToString();
                    ddlInventSerialId.SelectedValue = row["InventSerialId"]?.ToString();
                    ddlConfigId.SelectedValue = row["ConfigId"]?.ToString();
                    ddlSizeId.SelectedValue = row["InventSizeId"]?.ToString();
                    ddlColorId.SelectedValue = row["InventColorId"]?.ToString();
                    ddlStyleId.SelectedValue = row["InventStyle"]?.ToString();
                    ddlInventSiteId.SelectedValue = row["InventSiteId"]?.ToString();
                    ddlInventLocationId.SelectedValue = row["InventLocation"].ToString();

                    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                    createResult = service.create(dt);


                    if (string.IsNullOrWhiteSpace(createResult.Message))
                    {
                        createResult.isSuccess = false;
                        createResult.Message = "Error while creating Purchase requisition line";
                        createResult.AlertType = AlertType.Error.ToString();
                    }

                    NotificationMessage.showMessage(createResult);

                    if (createResult.isSuccess)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                    }
                }

                if (recId != 0)
                {
                    DataTable dtnew = new DataTable();
                    dtnew.Columns.Add("RecId", typeof(long));
                    dtnew.Columns.Add("PurchQty", typeof(decimal));
                    dtnew.Columns.Add("VendAccount", typeof(string));
                    dtnew.Columns.Add("PurchUnitofMeasure", typeof(decimal));
                    dtnew.Columns.Add("CurrencyCode", typeof(string));
                    dtnew.Columns.Add("PurchPrice", typeof(decimal));


                    DataRow newrow = dtnew.NewRow();
                    newrow["RecId"] = recId;  // 🔑 Important → tells AX/D365 which line to update


                    // Only map fields that are editable in edit mode
                    TextBox updatetxtPurchQty = (TextBox)gridRow.FindControl("txtPurchQty");
                    if (updatetxtPurchQty != null && decimal.TryParse(updatetxtPurchQty.Text.Trim(), out decimal qty))
                        newrow["PurchQty"] = qty;

                    DropDownList updateddlVendAccount = (DropDownList)gridRow.FindControl("ddlVendAccount");
                    if (updateddlVendAccount != null)
                        newrow["VendAccount"] = updateddlVendAccount.SelectedValue;

                    // PurchUnitofMeasure (comes from EditItemTemplate TextBox)
                    TextBox updatetxtPurchUnitofmeasure = (TextBox)gridRow.FindControl("txtPurchUnitofMeasure");
                    if (updatetxtPurchUnitofmeasure != null && !string.IsNullOrWhiteSpace(updatetxtPurchUnitofmeasure.Text))
                        newrow["PurchUnitofMeasure"] = updatetxtPurchUnitofmeasure.Text.Trim();

                    // CurrencyCode (comes from EditItemTemplate TextBox)
                    TextBox txtCurrencyCode = (TextBox)gridRow.FindControl("txtCurrencyCode");
                    if (txtCurrencyCode != null && !string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                        newrow["CurrencyCode"] = txtCurrencyCode.Text.Trim();

                    // UnitPrice (comes from EditItemTemplate TextBox)
                    TextBox txtUnitPrice = (TextBox)gridRow.FindControl("txtUnitPrice");
                    if (txtUnitPrice != null && decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal price))
                        newrow["PurchPrice"] = price;

                    dtnew.Rows.Add(newrow);


                    createResult = purchaseRequisitionLine.update(dtnew);

                    // ✅ If update was successful
                    if (createResult.isSuccess)
                    {
                        NotificationMessage.showMessage(createResult);  // success message
                        gridView.EditIndex = -1;                        // exit edit mode
                        reBindGrid();                                   // refresh grid with latest data
                    }
                    else
                    {
                        NotificationMessage.showMessage(createResult);  // show error message
                    }
                }
            }
            catch (Exception ex)
            {
                createResult.isSuccess = false;
                createResult.Message = "An error occurred: " + ex.Message;
                createResult.AlertType = AlertType.Error.ToString();
                NotificationMessage.showMessage("Error Creaing Purchase requisition line");
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
        {
            { "Batch number",("ddlInventBatchId", true) },
            { "Location", ("ddlWMSLocationId", true) },
            { "Serial number", ("ddlInventSerialId", true) },
            { "Configuration", ("ddlConfigId", true) },
            { "Size", ("ddlInventSizeId", true) },
            { "Color", ("ddlInventColorId", true) },
            { "Style", ("ddlInventStyleId", true) },
            { "Combinations", ("", false )},
            { "WMS Pallet", ("ddlWMSPalletId", false) },
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

            gridView.EditIndex = -1;
            reBindGrid();
        }



        [System.Web.Services.WebMethod]
        public static string GetVendorName(string vendAccount)
        {
            PurchaseRequisitionLine service = new PurchaseRequisitionLine();
            DataTable dt = service.retrieveVendor(vendAccount);
            string text = "";
            if (dt != null && dt.Rows.Count > 0)
                text = dt.Rows[0]["VendorName"].ToString();
            return text;
        }


        protected void BtnGotoReqQuo(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchaseRequisition");
            dt.Columns.Add("ItemNumber");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("BuyingLegalEntity");
            dt.Columns.Add("RecId");

            string purchaseRequisition = headerRequisitionNumber.Text;
            string selectedItemId = string.Empty;
            string selectedProductName = string.Empty;
            string selectedBuyingLegalEntity = string.Empty;


            // Loop through GridView rows to find the selected checkbox
            foreach (GridViewRow row in gridView.Rows)
            {

                DataRow DTrow = dt.NewRow();
                Label lblItemId = row.FindControl("ItemId") as Label;
                Label lblProductName = row.FindControl("PrdouctName") as Label;
                Label lblBuyingLegalEntity = row.FindControl("lblBuyingLegalEntity") as Label;

                if (lblItemId != null) selectedItemId = lblItemId.Text;
                if (lblProductName != null) selectedProductName = lblProductName.Text;
                if (lblBuyingLegalEntity != null) selectedBuyingLegalEntity = lblBuyingLegalEntity.Text;

                int rowIndex = row.RowIndex;
                long selectedRecId = Convert.ToInt64(gridView.DataKeys[rowIndex].Value);

                DTrow["PurchaseRequisition"] = purchaseRequisition;
                DTrow["ItemNumber"] = selectedItemId;
                DTrow["ProductName"] = selectedProductName;
                DTrow["BuyingLegalEntity"] = selectedBuyingLegalEntity;
                DTrow["RecId"] = selectedRecId;
                dt.Rows.Add(DTrow);

            }

            // Store in Session
            Session["PurchLineTable"] = dt;
            // Redirect
            //Response.Redirect("/ESS/PR/PurchaseRequestforQuotation.aspx");

            string script = $"openPopupPanel('/ESS/PR/PurchaseRequestforQuotation.aspx', 1000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);


        }

        private string FormatDateLabel(string input)
        {
            if (DateTime.TryParse(input, out DateTime date))
            {
                return date.ToString("M-d-yyyy");
            }
            return "N/A";
        }

        private void BindHeaderValues()
        {

            if (Session["PurchReqId"] != null)
            {
                headerRequisitionNumber.Text = Session["PurchReqId"].ToString();
                headerTabRequisitionNumber.Text = Session["PurchReqId"].ToString();  // Header tab
            }

            if (Session["Originator"] != null)
            {
                headerPreparer.Text = Session["Originator"].ToString();
                headerTabPreparer.Text = Session["Originator"].ToString();  // Header tab
            }

            if (Session["PurchReqName"] != null)
            {
                headerRequisitionName.Text = Session["PurchReqName"].ToString();
                headerTabRequisitionName.Text = Session["PurchReqName"].ToString();  // Header tab
            }

            if (Session["RequisitionPurpose"] != null)
            {
                headerTabPurpose.Text = Session["RequisitionPurpose"].ToString();
                headerPurpose.Text = Session["RequisitionPurpose"].ToString();  // Header tab
            }

            if (Session["RequisitionStatus"] != null)
            {
                headerTabStatus.Text = Session["RequisitionStatus"].ToString();
                headerStatus.Text = Session["RequisitionStatus"].ToString();  // Header tab
            }

            if (Session["RequiredDate"] != null)
            {
                DateTime requiredDate;
                if (DateTime.TryParse(Session["RequiredDate"].ToString(), out requiredDate))
                {
                    string formatted = requiredDate.ToString("M-d-yyyy"); // adjust as needed
                    headerTabRequestedDate.Text = formatted;
                    headerRequestedDate.Text = formatted;  // Header tab
                }
            }

            if (Session["TransDate"] != null)
            {
                DateTime transDate;
                if (DateTime.TryParse(Session["TransDate"].ToString(), out transDate))
                {
                    string formatted = transDate.ToString("M-d-yyyy"); // adjust as needed
                    headerTabAccountingDate.Text = formatted;
                    headerAccountingDate.Text = formatted;  // Header tab
                }
            }

            if (Session["CreatedBy"] != null)
            {
                historyTabCreatedBy.Text = Session["CreatedBy"].ToString();
            }

            // Created Date
            if (Session["CreatedDateTime"] != null)
            {
                DateTime createdDate;
                if (DateTime.TryParse(Session["CreatedDateTime"].ToString(), out createdDate))
                {
                    historyTabCreatedDate.Text = createdDate.ToString("M-d-yyyy"); // format as needed
                }
            }

            // Modified By (optional, if you have a session for it)
            if (Session["ModifiedBy"] != null)
            {
                historyTabModifiedBy.Text = Session["ModifiedBy"].ToString();
            }

            // Modified Date (optional, if you have a session for it)
            if (Session["ModifiedDateTime"] != null)
            {
                DateTime modifiedDate;
                if (DateTime.TryParse(Session["ModifiedDateTime"].ToString(), out modifiedDate))
                {
                    historyTabModifiedDate.Text = modifiedDate.ToString("M-d-yyyy");
                }
            }

            // Submitted By
            if (Session["SubmittedBy"] != null)
            {
                historyTabSubmittedBy.Text = Session["SubmittedBy"].ToString();
            }

            // Submitted Date
            if (Session["SubmittedDateTime"] != null)
            {
                DateTime submittedDate;
                if (DateTime.TryParse(Session["SubmittedDateTime"].ToString(), out submittedDate))
                {
                    historyTabSubmittedDate.Text = submittedDate.ToString("M-d-yyyy");
                }
            }

            // Source Requisition ID
            if (Session["PurchReqId"] != null)
            {
                historyTabSourceReqId.Text = Session["PurchReqId"].ToString();
            }

            // Source System Name (optional, if you have a session for it)
            if (Session["SourceSystemName"] != null)
            {
                historyTabSourceSystemName.Text = Session["SourceSystemName"].ToString();
            }

            ddlheaderReason.DataSource = purchaseRequisitionLine.retrieveReason();
            ddlheaderReason.DataTextField = "Description";
            ddlheaderReason.DataValueField = "Description";
            ddlheaderReason.DataBind();

            ddlheaderReason.Items.Insert(0, new ListItem("", String.Empty));

            if (Session["ReasonCode"] != null)
            {
                string reason = Session["ReasonCode"] as string;
                ddlheaderReason.SelectedValue = reason;
            }
        }

        protected void ddlReceivingOperatingUnit_SelectedIndexChanged(object sender, EventArgs e)

        {

            GridViewRow gridRow = gridView.Rows[gridView.EditIndex];

            DropDownList ddlReceivingOperatingUnit = gridRow.FindControl("ddlReceivingOperatingUnit") as DropDownList;

            DropDownList ddlReceivingOperatingUnitRecId = gridRow.FindControl("ddlReceivingOperatingUnitRecId") as DropDownList;

            if (ddlReceivingOperatingUnit != null && ddlReceivingOperatingUnit.SelectedItem != null)

            {

                var selectedText = ddlReceivingOperatingUnit.SelectedItem.Text;

                var matchingItem = ddlReceivingOperatingUnitRecId.Items.FindByText(selectedText);

                if (matchingItem != null)

                {

                    ddlReceivingOperatingUnitRecId.ClearSelection();

                    matchingItem.Selected = true;

                }

            }

            DateTime requiredDate = Convert.ToDateTime(Session["RequiredDate"]);

            long receivingUnitRecId = 0;

            if (ddlReceivingOperatingUnitRecId != null && !string.IsNullOrEmpty(ddlReceivingOperatingUnitRecId.SelectedValue))

                Int64.TryParse(ddlReceivingOperatingUnitRecId.SelectedValue, out receivingUnitRecId);

            DropDownList ddlItemid = (DropDownList)gridRow.FindControl("ddlItemId");

            if (ddlItemid != null)

            {

                PurchaseRequisitionLine service = new PurchaseRequisitionLine();

                DataTable dt = service.retrieveAllItemID(receivingUnitRecId, requiredDate);

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

                ddlItemid.CssClass += " filterable-dropdown";

            }

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            string requestId = Session["PurchReqId"] as string;
            recordsId.Add(requestId);
            SysOperationResult_BOL operationResult_BOL = eSSWorkflow.purchaseRequisition_Submit(recordsId.ToArray());
            reBindGrid();

            if (operationResult_BOL.isSuccess) // Assuming there's an isSuccess property
            {
                //// Redirect to the list page after success
                //Response.Redirect("/ESS/PR/PurchaseRequisitionHeader_ListPage.aspx", false);
                //Context.ApplicationInstance.CompleteRequest();

                string message = $"Purchase Requisition {requestId} has been submitted successfully.";
                string script = $"alert('{message}'); window.location='/ESS/PR/PurchaseRequisitionHeader_ListPage.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SubmitSuccess", script, true);
            }
            else
            {
                // Optionally, show a message if submission failed
                NotificationMessage.showMessage("Submission failed. Please try again.");
            }
        }

        public void setFieldsEmpty()
        {
            string empty = "\uFEFF"; // or just "" if you don’t want zero-width space

            // Item Fields
            // ================== ITEM TAB ==================
            txtItemNumber.Text = empty;
            txtItemDescription.Text = empty;
            txtLineType.Text = empty;
            txtInternetAddress.Text = empty;
            txtExternalItemNumber.Text = empty;
            txtProductName.Text = empty;
            txtSupplierPartAuxID.Text = empty;
            txtCategory.Text = empty;
            txtCode.Text = empty;
            txtQuantity.Text = empty;
            txtUnit.Text = empty;
            txtUnitPrice.Text = empty;
            txtNetAmount.Text = empty;
            txtCurrency.Text = empty;
            txtPriceUnit.Text = empty;
            txtDiscountPercent.Text = empty;
            txtDiscount.Text = empty;
            txtChargesOnPurchases.Text = empty;
            txtVendorName.Text = empty;
            txtVendorAccount.Text = empty;
            txtVendorStatus.Text = empty;
            txtRFQRequirement.Text = empty;
            txtFriendlyName.Text = empty;

            // ================== GENERAL TAB ==================
            txtRequester.Text = empty;
            txtBuyingLegalEntity.Text = empty;
            txtReceivingOperatingUnit.Text = empty;
            txtStatus.Text = empty;
            txtRequestedDate.Text = empty;
            txtAccountingDate.Text = empty;
            ddlReason.Text = empty;
            txtDetails.Text = empty;

            // ================== ADDRESS TAB ==================
            txtDeliveryName.Text = empty;
            txtDeliveryAddress.Text = empty;
            txtAddress.Text = empty;

            // ================== DETAILS TAB ==================
            chkPreventPartialDelivery.Checked = false;
            chkPrepaymentRequired.Checked = false;
            txtPrepaymentDetails.Text = empty;
            txtPriceDiscountTransfer.Text = empty;
            txtRFQCase.Text = empty;
            txtRFQCaseStatus.Text = empty;
            txtConsolidationOpportunity.Text = empty;
            txtConsolidationOpportunityStatus.Text = empty;
            txtPurchaseOrder.Text = empty;
            txtPurchaseOrderStatus.Text = empty;
            txtInvoice.Text = empty;
            txtInvoiceStatus.Text = empty;
            txtPurchaseAgreement.Text = empty;
            txtPurchaseAgreementStatus.Text = empty;
            txtItemSalesTaxGroup.Text = empty;
            txtSalesTaxGroup.Text = empty;

            // ================== PROJECT TAB ==================
            txtProjectID.Text = empty;
            txtActivityNumber.Text = empty;
            txtProjectCategory.Text = empty;
            txtProjectItemNumber.Text = empty;
            txtProjectLineProperty.Text = empty;
            txtTransactionID.Text = empty;
            txtCostPriceQuantity.Text = empty;
            txtUnitPrice.Text = empty;
            TxtlinenetAmount.Text = empty;
            txtSalesCurrency.Text = empty;
            txtSalesPrice.Text = empty;
            txtSalesUnit.Text = empty;
            txtProjectSalesTaxGroup.Text = empty;
            txtProjectSalesItemTaxGroup.Text = empty;

            // ================== ASSET GROUP ==================
            txtAssetGroup.Text = empty;
            txtReasonCode.Text = empty;

            //// ================== INVENTORY DIMENSIONS ==================
            //txtConfiguration.Text = empty;
            //txtSize.Text = empty;
            //txtColor.Text = empty;
            //txtVersion.Text = empty;
            //txtSite.Text = empty;
            //txtWarehouse.Text = empty;
            //txtBatchNumber.Text = empty;
            //txtLocation.Text = empty;
            //txtSerialNumber.Text = empty;
            txtInventoryStatus.Text = empty;
            txtLicensePlate.Text = empty;
            txtOwner.Text = empty;
            txtInventoryProfile.Text = empty;


        }

        protected void ddlTemId_fillDimension(object sender, EventArgs e)
        {
            DropDownList ddlItemId = (DropDownList)sender;
            // Get the row containing the dropdown
            GridViewRow row = (GridViewRow)ddlItemId.NamingContainer;

            string selectedItemId = ddlItemId.SelectedValue;

            DropDownList ddlInventSiteId = (DropDownList)row.FindControl("ddlInventSiteId");

            if (!string.IsNullOrEmpty(selectedItemId))
            {
                Dictionary<string, (string FieldId, bool IsVisible)> columnVisibilities = new Dictionary<string, (string FieldId, bool IsVisible)>
                {
                    { "Batch number",("", BindInventBatchId(selectedItemId, row) )},
                    { "Location", ("", BindWmsLocationId(selectedItemId, row)) },
                    { "Serial number", ("", BindInventSerialId(selectedItemId, row)) },
                    { "Configuration", ("", BindConfigId(selectedItemId, row)) },
                    { "Size", ("", BindInventSizeId(selectedItemId, row)) },
                    { "Color", ("", BindInventColorId(selectedItemId, row)) },
                    { "Style", ("", BindInventStyleId(selectedItemId, row)) },
                    { "Combinations", ("", BindCombination(selectedItemId, row)) },
                    { "WMS Pallet", ("", BindWmsPalletId(selectedItemId, row)) },
                    { "Site", ("", BindInventSiteId(row)) },
                    { "Warehouse", ("", BindInventLocationId(row)) }
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

                    TextBox txtProductUnit = row.FindControl("txtProductUnit") as TextBox;
                    if (txtProductUnit != null)
                        txtProductUnit.Text = dtrow["PurchUnitofMeasureCode"].ToString();


                    TextBox txtUnitPrice = row.FindControl("txtUnitPrice") as TextBox;
                    if (txtUnitPrice != null)
                        txtUnitPrice.Text = dtrow["PurchPrice"].ToString();


                    TextBox txtCurrencyCode = row.FindControl("txtCurrencyCode") as TextBox;
                    if (txtCurrencyCode != null)
                        txtCurrencyCode.Text = dtrow["CurrencyCode"].ToString();

                    TextBox txtVendAccount = row.FindControl("txtVendAccount") as TextBox;
                    if (txtVendAccount != null)
                        txtVendAccount.Text = dtrow["VendAccount"].ToString();

                    TextBox txtVendorName = row.FindControl("txtVendorName") as TextBox;
                    if (txtVendorName != null)
                        txtVendorName.Text = dtrow["VendorName"].ToString();

                    TextBox txtPurchQty = row.FindControl("txtPurchQty") as TextBox;
                    if (txtPurchQty != null)
                        txtPurchQty.Text = "1";


                }
            }
        }

        private bool BindInventBatchId(string itemid, GridViewRow e)
        {
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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

            DataTable dt = purchaseRequisitionLine.retrieveinventSerialId(itemid);
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

        private bool BindConfigId(string itemid, GridViewRow e)
        {
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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
            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();
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

        private bool BindWmsPalletId(string itemid, GridViewRow e)

        {

            PurchaseRequisitionLine lines = new PurchaseRequisitionLine();

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

        private bool BindInventSiteId(GridViewRow e)
        {
            PurchaseRequisitionLine service = new PurchaseRequisitionLine();
            DataTable dt = service.retrievesiteId();

            DropDownList ddlSiteId = (DropDownList)e.FindControl("ddlInventSiteId");
            if (ddlSiteId != null)
            {
                ddlSiteId.DataSource = dt;
                ddlSiteId.DataValueField = "InventSiteId";
                ddlSiteId.DataTextField = "InventSiteId";
                ddlSiteId.DataBind();

                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlSiteId.Items.Count == 1 && string.IsNullOrEmpty(ddlSiteId.Items[0].Text)))
                {
                    ddlSiteId.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlSiteId.Items.Clear();
                    // Disable the dropdown
                    ddlSiteId.Enabled = false;
                    // Set grey background color
                    ddlSiteId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlSiteId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlSiteId.Visible;
                }
                else
                {
                    ddlSiteId.Visible = true;

                    // Enable the dropdown
                    ddlSiteId.Enabled = true;
                    // Reset to default background color
                    ddlSiteId.BackColor = System.Drawing.Color.White;

                    ddlSiteId.CssClass += " filterable-dropdown";

                    return ddlSiteId.Visible;
                }
            }
            else
                return false;
        }

        private bool BindInventLocationId(GridViewRow e)
        {
          
            DataTable dt = purchaseRequisitionLine.retrieveinventLocationId();

            DropDownList ddlInventLocationId = (DropDownList)e.FindControl("ddlInventLocationId");
            if (ddlInventLocationId != null)
            {
                ddlInventLocationId.DataSource = dt;
                ddlInventLocationId.DataValueField = "InventLocationId";
                ddlInventLocationId.DataTextField = "InventLocationId";
                ddlInventLocationId.DataBind();

                // Check if dropdown is empty and disable/style accordingly
                if (dt == null || dt.Rows.Count == 0 ||
                    (ddlInventLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventLocationId.Items[0].Text)))
                {
                    ddlInventLocationId.Visible = false;

                    // Clear the dropdown first to remove any empty items
                    ddlInventLocationId.Items.Clear();
                    // Disable the dropdown
                    ddlInventLocationId.Enabled = false;
                    // Set grey background color
                    ddlInventLocationId.BackColor = System.Drawing.Color.LightGray;
                    // Add a default item to show it's empty
                    ddlInventLocationId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

                    return ddlInventLocationId.Visible;
                }
                else
                {
                    ddlInventLocationId.Visible = true;

                    // Enable the dropdown
                    ddlInventLocationId.Enabled = true;
                    // Reset to default background color
                    ddlInventLocationId.BackColor = System.Drawing.Color.White;
                    // Add empty item at the top for selection
                    ddlInventLocationId.Items.Insert(0, new ListItem("", string.Empty));

                    ddlInventLocationId.CssClass += " filterable-dropdown";

                    return ddlInventLocationId.Visible;
                }
            }
            else
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
        protected void BtnDeleteHeader_Click(object sender, EventArgs e)
        {
            // Retrieve recId from Session
            if (Session["RecId"] != null)
            {
                long recId = 0;
                if (Int64.TryParse(Session["RecId"].ToString(), out recId) && recId > 0)
                {
                    PurchaseRequestGroup purchaseRequestGroup = new PurchaseRequestGroup();
                    SysOperationResult_BOL operationResult_BOL = purchaseRequestGroup.delete(new long[] { recId });
                    bool result = operationResults(operationResult_BOL);

                    if (result)
                    {
                        // Redirect after successful deletion
                        //Response.Redirect("/ESS/PR/PurchaseRequisitionHeader_ListPage.aspx", false);
                        //Context.ApplicationInstance.CompleteRequest();

                        string message = $"Purchase Requisition {Session["PurchReqId"]} has been deleted.";
                        string script = $"alert('{message}'); window.location='/ESS/PR/PurchaseRequisitionHeader_ListPage.aspx';";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccess", script, true);
                    }
                    else
                    {
                        bindGrid(); // refresh grid if delete failed
                    }
                }
            }
        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
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
                            ddl.CssClass = "form-control";
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
                        bindItemDimension(_defaultDimensionRecId);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic dimensions", ex);
                }
            }

        }



        protected void bindItemDimension(long __defaultDimensionRecId)
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();

            long Itemdimension = __defaultDimensionRecId;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(Itemdimension);
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

        //protected void ddlInventSiteId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddlInventSiteId = (DropDownList)sender;
        //    // Get the row containing the dropdown
        //    GridViewRow row = (GridViewRow)ddlInventSiteId.NamingContainer;

        //    string selectedSiteId = ddlInventSiteId.SelectedValue;

        //    DropDownList ddlInventLocationId = (DropDownList)row.FindControl("ddlInventLocationId");

        //    if (ddlInventLocationId != null)
        //    {
        //        BindInventLocationId(selectedSiteId, row);
        //    }
        //}


        protected void ddlInventSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlInventSiteId = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlInventSiteId.NamingContainer;
            string selectedSiteId = ddlInventSiteId.SelectedValue;

            DropDownList ddlLocationId = (DropDownList)gridRow.FindControl("ddlInventLocationId");
            if (ddlLocationId != null)
            {
                // Reset location dropdown if no site is selected
                if (string.IsNullOrEmpty(selectedSiteId))
                {
                    ddlLocationId.Items.Clear();
                    ddlLocationId.Items.Insert(0, new ListItem("", String.Empty));
                    ddlLocationId.CssClass = ddlLocationId.CssClass.Replace(" filterable-dropdown", ""); // Optional: Remove class if added previously
                    return;
                }


                
                DataTable dt = purchaseRequisitionLine.retrieveinventLocationId(selectedSiteId);

                dt.Columns.Add("DisplayText", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["InventLocationId"] + " - " + row["InventLocationName"];
                }

                ddlLocationId.DataSource = dt;
                ddlLocationId.DataValueField = "InventLocationId";
                ddlLocationId.DataTextField = "DisplayText";
                ddlLocationId.DataBind();
                ddlLocationId.Items.Insert(0, new ListItem("", String.Empty));
                ddlLocationId.CssClass += " filterable-dropdown";
            }
        }

        protected void ddllocation_selection(object sender, EventArgs e)
        {
            DropDownList ddlInventLocationId = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlInventLocationId.NamingContainer;
            string selectedwarehouse = ddlInventLocationId.SelectedValue;

            if (selectedwarehouse != null)
            {
               
                DataTable dt = purchaseRequisitionLine.findSitebyLocation(selectedwarehouse);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    DropDownList ddlInventSiteId = row.FindControl("ddlInventSiteId") as DropDownList;

                    ddlInventSiteId.SelectedValue = dtrow["InventSiteId"].ToString();

                }

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
                    string inventDimId = (row.FindControl("lblInventDimId") as Label)?.Text.Trim();
                    string itemId = (row.FindControl("ItemId") as Label)?.Text.Trim();
                    string ItemName = (row.FindControl("PrdouctName") as Label)?.Text.Trim();

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
                    string unitId = (row.FindControl("ProductUnit") as Label)?.Text.Trim();

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
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select a Purchase requisition line to view On-hand.');", true);
            }
        }


        protected void EditUpdate_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = purchaseRequisitionLine.createDataTable(); // replace with your actual method
                    DataRow dr = dataTable.NewRow();

                    // PurchQty
                    dr["PurchQty"] = gridViewRow.FindControl("txtPurchQty") != null
                                     ? (gridViewRow.FindControl("txtPurchQty") as TextBox).Text
                                     : ((Label)gridViewRow.FindControl("PurchQty")).Text;

                    // PurchUnitofMeasure (hidden field)
                    dr["PurchUnitofMeasure"] = gridViewRow.FindControl("txtPurchUnitofMeasure") != null
                                               ? (gridViewRow.FindControl("txtPurchUnitofMeasure") as TextBox).Text
                                               : ((Label)gridViewRow.FindControl("lblPurchUnitofMeasure")).Text;

                    // PurchPrice
                    dr["PurchPrice"] = gridViewRow.FindControl("txtUnitPrice") != null
                                       ? (gridViewRow.FindControl("txtUnitPrice") as TextBox).Text
                                       : ((Label)gridViewRow.FindControl("UnitPrice")).Text;

                    // CurrencyCode
                    dr["CurrencyCode"] = gridViewRow.FindControl("txtCurrencyCode") != null
                                         ? (gridViewRow.FindControl("txtCurrencyCode") as TextBox).Text
                                         : ((Label)gridViewRow.FindControl("CurrencyCode")).Text;

                    dataTable.Rows.Add(dr);

                    SysOperationResult_BOL operationResult_BOL = purchaseRequisitionLine.update(dataTable);
                    bool result = operationResults(operationResult_BOL);
                    if (result)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                    }
                    else
                    {
                        bindGrid();
                    }
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PurchaseRequestGroup newgroup = new PurchaseRequestGroup();
            // Loop through GridView to find the selected record
            long selectedRecId = 0;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out selectedRecId);
                    break; // Only take the first selected record
                }
            }

            if (selectedRecId > 0)
            {
                // Call your cancellines method for a single record
                SysOperationResult_BOL operationResult_BOL = newgroup.cancellines(selectedRecId);

                // Check result
                if (operationResult_BOL != null && operationResult_BOL.isSuccess) // Assuming IsSuccess flag
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Success",
                        "alert('Record canceled successfully.');", true);
                    Session["RequisitionStatus"] = "Cancelled";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Fail",
                        "alert('Failed to cancel the record.');", true);
                }
                Page_Load(sender, e);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select a record to cancel.');", true);
            }
        }
        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {

                long recIdLine = Session["RecIdline"] != null ? (long)Session["RecIdline"] : 0;

                string requisitionName = headerRequisitionName.Text.Trim();
                DateTime requestedDate;
                if (!DateTime.TryParseExact(headerRequestedDate.Text.Trim(),
                                            "M-d-yyyy",   
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out requestedDate))
                {
                    lblMessage.Text = "Please enter a valid Requested Date (MM-DD-YYYY).";
                    return;
                }

             
                DateTime accountingDate;
                if (!DateTime.TryParseExact(headerAccountingDate.Text.Trim(),
                                            "M-d-yyyy",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out accountingDate))
                {
                    lblMessage.Text = "Please enter a valid Accounting Date (MM-DD-YYYY).";
                    return;
                }

                // ✅ Collect Financial Dimension Values
                List<DataContract> dimContracts = new List<DataContract>();
                ESSFinancialDimensions finDim = new ESSFinancialDimensions();

                DataContract[] activeDimensions = finDim.retrieveActiveDimensions();
                foreach (var dim in activeDimensions)
                {
                    DropDownList ddl = financialDimensionsContainer.FindControl("ddlFinancialDimension" + dim.Code) as DropDownList;
                    if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                    {
                        DataContract dc = new DataContract();
                        dc.Code = dim.Code;
                        dc.Value1 = ddl.SelectedValue;
                        dc.Value2 = ddl.SelectedItem.Text; // optional, description
                        dimContracts.Add(dc);
                    }
                }

                // ✅ Call setDimensionValues to get DefaultDimension RecId
                long defaultDimensionRecId = 0;
                if (dimContracts.Count > 0)
                {
                    ESSFinancialDimensions newdimension = new ESSFinancialDimensions();
                    defaultDimensionRecId = newdimension.setDimensionValues(dimContracts.ToArray());
                }

                purchaseRequisitionLine.updateFinancialDimension(defaultDimensionRecId, recIdLine);

                // Prepare DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("RequisitionName");
                dt.Columns.Add("RequestedDate", typeof(DateTime));
                dt.Columns.Add("AccountingDate", typeof(DateTime));
                dt.Columns.Add("RecId");
                dt.Columns.Add("HeaderReason");
                // Create DataRow
                DataRow row = dt.NewRow();
                row["RequisitionName"] = requisitionName;
                row["RequestedDate"] = requestedDate;
                row["AccountingDate"] = accountingDate;
                row["RecId"] = Session["RecId"] != null ? Session["RecId"].ToString() : "0";
                row["HeaderReason"] = ddlheaderReason.SelectedValue;
                dt.Rows.Add(row);

                PurchaseRequestGroup purchasereqheader = new PurchaseRequestGroup();
                SysOperationResult_BOL result = purchasereqheader.UpdateRecord(dt);

                if (result != null && result.isSuccess)
                {
                    Session["ReasonCode"] = ddlheaderReason.SelectedValue;
                    NotificationMessage.showMessage(result);
                }
                else
                {
                    NotificationMessage.showMessage(result);
                }



                // ✅ Loop through GridView Rows for Inventory Dimensions
                foreach (GridViewRow gridRow in gridView.Rows)
                {
                    Label lblItemId = (Label)gridRow.FindControl("ItemId");
                    Label lblRecId = (Label)gridRow.FindControl("lblRecId");

                    string itemId = lblItemId != null ? lblItemId.Text.Trim() : string.Empty;
                    long recId = 0;
                    if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
                        long.TryParse(lblRecId.Text, out recId);

                    // Get dimension dropdown values
                    string configuration = ddlConfigurationLineDetail.SelectedValue;
                    string color = ddlColorLineDetail.SelectedValue;
                    string size = ddlSizeLineDetail.SelectedValue;
                    string style = ddlStyleLineDetail.SelectedValue;
                    string site = ddlSiteLineDetail.SelectedValue;
                    string warehouse = ddlWarehouseLineDetail.SelectedValue;
                    string batchNumber = ddlBatchNumberLineDetail.SelectedValue;
                    string wmsLocation = ddlWmsLocationLineDetail.SelectedValue;
                    string serialNumber = ddlSerialNumberLineDetail.SelectedValue;

                    // ✅ Condition: Update only if at least one dimension has a value
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
                        // Create new DataTable for this row
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
                        inventRow["RecId"] = recId;
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

                        SysOperationResult_BOL inventResult = purchaseRequisitionLine.UpdateInventDimRecord(inventDt);
                        NotificationMessage.showMessage(inventResult);
                    }
                }
            


            }
            catch (Exception)
            {

            }
        }

        private void BindConfigurationforLineDetail(string itemid)
        {
            
            DataTable dt = purchaseRequisitionLine.retrieveconfigId(itemid);

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
            
            DataTable dt = purchaseRequisitionLine.retrievesiteId();

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

        private void BindColorLineDetail(string itemid)
        {
           
            DataTable dt = purchaseRequisitionLine.retrieveinventColorId(itemid);

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
           
            DataTable dt = purchaseRequisitionLine.retrieveinventSizeId(itemid);

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
           
            DataTable dt = purchaseRequisitionLine.retrieveinvetstyleid(itemid);

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
                ddlStyleLineDetail.DataValueField = "InventStyle";
                ddlStyleLineDetail.DataTextField = "InventStyle";
                ddlStyleLineDetail.DataBind();

                ddlStyleLineDetail.Items.Insert(0, new ListItem("", string.Empty));
                ddlStyleLineDetail.Enabled = true;
            }
        }

        private void BindWarehouseLineDetail(string itemid)
        {


            DataTable dt = purchaseRequisitionLine.retrieveinventLocationId();

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
            
            DataTable dt = purchaseRequisitionLine.retrieveinventBatchId(itemid);

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
           
            DataTable dt = purchaseRequisitionLine.retrievewmsLocationId(itemid);

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
         
            DataTable dt = purchaseRequisitionLine.retrieveinventSerialId(itemid);

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


        protected void ddlsiteId_selection(object sender, EventArgs e)
        {
            DropDownList ddlInventSiteId = (DropDownList)sender;
            GridViewRow gridRow = (GridViewRow)ddlInventSiteId.NamingContainer;
            string selectedSiteId = ddlInventSiteId.SelectedValue;

            DropDownList ddlWarehouse = (DropDownList)gridRow.FindControl("ddlInventLocationId");
            if (ddlWarehouse != null)
            {
                
                DataTable dt = purchaseRequisitionLine.retrieveinventLocationId(selectedSiteId);

                dt.Columns.Add("DisplayText", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["InventLocationId"] + " - " + row["InventLocationName"];
                }

                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataValueField = "InventLocationId";
                ddlWarehouse.DataTextField = "DisplayText";
                ddlWarehouse.DataBind();
                ddlWarehouse.Items.Insert(0, new ListItem("", String.Empty));
                ddlWarehouse.CssClass += " filterable-dropdown";
            }
        }

        protected void ddlInvetLocation_selection(object sender, EventArgs e)
        {
            DropDownList ddlInventLocationId = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlInventLocationId.NamingContainer;
            string selectedwarehouse = ddlInventLocationId.SelectedValue;

            if (selectedwarehouse != null)
            {
             
                DataTable dt = purchaseRequisitionLine.findSitebyLocation(selectedwarehouse);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dtrow = dt.Rows[0];
                    DropDownList ddlSiteId = row.FindControl("ddlInventSiteId") as DropDownList;

                    ddlSiteId.SelectedValue = dtrow["InventSiteId"].ToString();

                }

            }
        }

        protected void ddlSiteLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSiteId = ddlSiteLineDetail.SelectedValue;

            if (!string.IsNullOrEmpty(selectedSiteId))
            {

                DataTable dt = purchaseRequisitionLine.retrieveinventLocationId(selectedSiteId);

                // Add DisplayText column
                dt.Columns.Add("DisplayText", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["InventLocationId"] + " - " + row["InventLocationName"];
                }

                // Bind to warehouse dropdown
                ddlWarehouseLineDetail.DataSource = dt;
                ddlWarehouseLineDetail.DataValueField = "InventLocationId";
                ddlWarehouseLineDetail.DataTextField = "DisplayText";
                ddlWarehouseLineDetail.DataBind();

                // Insert empty option at top
                ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
            }
            else
            {
                ddlWarehouseLineDetail.Items.Clear();
                ddlWarehouseLineDetail.Items.Insert(0, new ListItem("", string.Empty));
            }
        }

        protected void ddlWarehouseLineDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWarehouse = ddlWarehouseLineDetail.SelectedValue;

            if (!string.IsNullOrEmpty(selectedWarehouse))
            {
               
                DataTable dt = purchaseRequisitionLine.findSitebyLocation(selectedWarehouse);

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

        private bool BindReason()
        {
            PurchaseRequisitionLine line = new PurchaseRequisitionLine();
            DataTable dt = line.retrieveReason();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlReason.DataSource = dt;
                ddlReason.DataTextField = "Description";   // what user sees
                ddlReason.DataValueField = "Reason";       // underlying value
                ddlReason.DataBind();

            }

            return true;
        }

    }
}