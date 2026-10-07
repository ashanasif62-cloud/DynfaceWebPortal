using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Net.Mime.MediaTypeNames;

namespace DynamicsPortal
{
    public partial class AllPurchaseOrder_ListPage : MainForm
    {
        private PurchaseOrderHeader NewPurchaseOrder = new PurchaseOrderHeader();

        #region Pagination Properties
        private long LastRecId
        {
            get { return ViewState["LastRecId"] != null ? (long)ViewState["LastRecId"] : 0; }
            set { ViewState["LastRecId"] = value; }
        }

        private int PageSize
        {
            get { return 15; } // Default page size
        }

        private List<long> PageHistory
        {
            get
            {
                if (ViewState["PageHistory"] == null)
                    ViewState["PageHistory"] = new List<long>();
                return (List<long>)ViewState["PageHistory"];
            }
            set { ViewState["PageHistory"] = value; }
        }
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = NewPurchaseOrder.tableName;
                pageMenuId = "AllPurchaseOrder_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "All purchase orders";

                    // Dynamically set the page title in the master page div
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "All purchase order"; // Set the text in the div
                        titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                    }

                    // Initially load the data for the default date range
                    reBindGrid();
                    btnWorkflow.Enabled = true;
                   

                }
                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();

                    //if (gridView.Rows.Count > 0)
                    //{
                    //    gridView.Rows[0].CssClass = "highlight-row";
                    //}
                }
                btnWorkflow.Enabled = true;

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }

        }

        protected void reBindGrid()
        {
            LastRecId = 0;
            if (hdnHasMore != null) hdnHasMore.Value = "true";
            getGridDataTable(false); // Fresh load
        }

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        private void getGridDataTable(bool append)
        {
            long fetchId = LastRecId;
            if (fetchId == 0) fetchId = 9223372036854775807; // Max Long Value

            DataTable dtNext = NewPurchaseOrder.retrieveAll(fetchId, PageSize);

            Session["PageSize"] = PageSize;
            Session["LastRecId"] = fetchId;

            DataTable dtCurrent = null;
            if (append)
            {
                dtCurrent = SessionVariables.getSessionDataTable();
            }

            if (dtCurrent == null || dtCurrent.Columns.Count == 0)
            {
                dtCurrent = dtNext != null ? dtNext.Clone() : new DataTable();
            }

            if (dtNext != null && dtNext.Rows.Count > 0)
            {
                // Import rows
                foreach (DataRow row in dtNext.Rows)
                {
                    bool exists = false;
                    if (dtCurrent.Columns.Contains("RecId") && row["RecId"] != DBNull.Value)
                    {
                        long rowRecId = Convert.ToInt64(row["RecId"]);
                        exists = dtCurrent.AsEnumerable().Any(r => 
                            r.Table.Columns.Contains("RecId") && 
                            r["RecId"] != DBNull.Value && 
                            Convert.ToInt64(r["RecId"]) == rowRecId
                        );
                    }
                    
                    if (!exists)
                    {
                        dtCurrent.ImportRow(row);
                    }
                }

                // Determine minRecId from the newly fetched rows to update LastRecId
                long minRecId = long.MaxValue;
                foreach (DataRow row in dtNext.Rows)
                {
                    if (row["RecId"] != DBNull.Value)
                    {
                        long currentRecId = Convert.ToInt64(row["RecId"]);
                        if (currentRecId < minRecId) minRecId = currentRecId;
                    }
                }
                LastRecId = (minRecId == long.MaxValue) ? 0 : minRecId;

                if (dtNext.Rows.Count < PageSize)
                {
                    hdnHasMore.Value = "false";
                }
                else
                {
                    hdnHasMore.Value = "true";
                }
            }
            else
            {
                hdnHasMore.Value = "false";
            }

            SessionVariables.setSessionDataTable(dtCurrent);
            gridView.DataSource = dtCurrent;
            gridView.DataBind();
        }

        protected void btnLoadMore_Click(object sender, EventArgs e)
        {
            getGridDataTable(true);
            upGrid.Update();
        }

        protected void btnServerSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string query = hdnSearchQuery.Value.Trim();
                DataTable dtCurrent = SessionVariables.getSessionDataTable();
                if (dtCurrent == null) dtCurrent = new DataTable();

                if (string.IsNullOrEmpty(query))
                {
                    if (hdnHasMore != null) hdnHasMore.Value = "true";
                    gridView.DataSource = dtCurrent;
                    gridView.DataBind();
                }
                else
                {
                    if (hdnHasMore != null) hdnHasMore.Value = "false";
                    DataTable dtFiltered = dtCurrent.Clone();
                    foreach (DataRow row in dtCurrent.Rows)
                    {
                        bool matches = false;
                        foreach (object item in row.ItemArray)
                        {
                            if (item != null && item.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                matches = true;
                                break;
                            }
                        }
                        if (matches)
                        {
                            dtFiltered.ImportRow(row);
                        }
                    }

                    // If not found locally, fetch from backend
                    if (dtFiltered.Rows.Count == 0)
                    {
                        DataTable dtSearch = NewPurchaseOrder.retrieveByPurchID(query);
                        if (dtSearch != null && dtSearch.Rows.Count > 0)
                        {
                            dtFiltered.ImportRow(dtSearch.Rows[0]);

                            // Also save to session so we have it locally
                            DataRow newRow = dtCurrent.NewRow();
                            newRow.ItemArray = dtSearch.Rows[0].ItemArray;
                            dtCurrent.Rows.InsertAt(newRow, 0);
                            SessionVariables.setSessionDataTable(dtCurrent);
                        }
                    }

                    gridView.DataSource = dtFiltered;
                    gridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
            finally
            {
                upGrid.Update();
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Access all label controls in the ItemTemplate (read-only mode)
                Label lblVendorAccount = (Label)e.Row.FindControl("lblVendorAccount");
                Label lblInvoiceAccount = (Label)e.Row.FindControl("lblInvoiceAccount");
                Label lblVendorName = (Label)e.Row.FindControl("lblVendorName");
                Label lblPurchaseType = (Label)e.Row.FindControl("lblPurchaseType");
                Label lblApprovalStatus = (Label)e.Row.FindControl("lblApprovalStatus");
                Label lblPurchaseOrderStatus = (Label)e.Row.FindControl("lblPurchaseOrderStatus");
                Label lblDeliveryMode = (Label)e.Row.FindControl("lblDeliveryMode");
                Label lblDeliveryTerms = (Label)e.Row.FindControl("lblDeliveryTerms");
                Label lblPurchaseAgreement = (Label)e.Row.FindControl("lblPurchaseAgreement");
                Label lblQualityOrderStatus = (Label)e.Row.FindControl("lblQualityOrderStatus");
                Label lblDirectDelivery = (Label)e.Row.FindControl("lblDirectDelivery");

                // Selection preservation
                LinkButton lnkPurchcaseOrderId = (LinkButton)e.Row.FindControl("lnkPurchcaseOrderId");
                if (lnkPurchcaseOrderId != null && Session["PurchaseOrderId"] != null)
                {
                    string selectedPOId = Session["PurchaseOrderId"].ToString();
                    if (lnkPurchcaseOrderId.Text == selectedPOId)
                    {
                        e.Row.CssClass = "selected-row";
                        CheckBox chkSelectRow = (CheckBox)e.Row.FindControl("chk_SelectSingle");
                        if (chkSelectRow != null)
                        {
                            chkSelectRow.Checked = true;
                        }
                    }
                }

                Label lblDate = (Label)e.Row.FindControl("lblrequestedreceiptdate");
                if (lblDate != null && DateTime.TryParse(lblDate.Text, out DateTime parsedDate))
                {
                    lblDate.Text = parsedDate.ToString("M-d-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                Label editDate = (Label)e.Row.FindControl("txtSrequestedreceiptdate");
                if (editDate != null && DateTime.TryParse(editDate.Text, out DateTime parsedEditDate))
                {
                    editDate.Text = parsedEditDate.ToString("M-d-yyyy");
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = e.Row.DataItem as DataRowView;
                    if (drv != null)
                    {
                        var rowData = new System.Collections.Generic.Dictionary<string, string>();
                        foreach (DataColumn col in drv.Row.Table.Columns)
                        {
                            rowData[col.ColumnName] = drv[col.ColumnName]?.ToString() ?? "";
                        }
                        string json = new System.Web.Script.Serialization.JavaScriptSerializer()
                                          .Serialize(rowData);
                        e.Row.Attributes["data-rowjson"] = json;
                    }
                }
            }
        }



        protected void lnkPurchReqId_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string purchaseorderID = btn.CommandArgument;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label lblVendorAccount = (Label)row.FindControl("lblVendorAccount");
            string vendorAccount = lblVendorAccount != null ? lblVendorAccount.Text.Trim() : "";
            Label lblVendorName = (Label)row.FindControl("lblVendorName");
            string vendorName = lblVendorName != null ? lblVendorName.Text.Trim() : "";
            Label lblRequestedReceiptDate = (Label)row.FindControl("lblrequestedreceiptdate");
            string requestedReceiptDate = lblRequestedReceiptDate != null ? lblRequestedReceiptDate.Text.Trim() : "";
            Label lblRecId = row.FindControl("lblRecId") as Label;
            long recId = 0;

            if (lblRecId != null)
            {
                long.TryParse(lblRecId.Text.Trim(), out recId);
            }
            Label lblApprovalStatus = row.FindControl("lblApprovalStatus") as Label;
            string approvalStatus = lblApprovalStatus != null ? lblApprovalStatus.Text.Trim() : "";
            Label lblCreateddatetime = row.FindControl("lblCreateddatetime") as Label;
            string createdDateTime = lblCreateddatetime != null ? lblCreateddatetime.Text.Trim() : "";
            Label lbldefaultdimension = row.FindControl("lblDefaultDimension") as Label;
            string defaultdimensionvalue = lbldefaultdimension != null ? lbldefaultdimension.Text.Trim() : "";

            
            Label lblSiteID = row.FindControl("lblSiteID") as Label;
            string siteId = lblSiteID != null ? lblSiteID.Text.Trim() : "";
            Label lblLocationID = row.FindControl("lblLocationID") as Label;
            string locationId = lblLocationID != null ? lblLocationID.Text.Trim() : "";

            // Store in session
            Session["PurchaseOrderId"] = purchaseorderID;
            Session["VendorAccount"] = vendorAccount;
            Session["VendorName"] = vendorName;
            Session["RequestedReceiptDate"] = requestedReceiptDate;
            Session["RecId"] = recId;
            Session["ApprovalStatus"] = approvalStatus;
            Session["CreatedDate"] = createdDateTime;
            Session["VendorDefaultDimension"] = defaultdimensionvalue;
            Session["ListPageSiteId"] = siteId;
            Session["ListPageLocationId"] = locationId;

            // Redirect to target page (no query string in URL)
            Response.Redirect("/ESS/PR/PurchaseOrderLines_ListPage.aspx");

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
                SysOperationResult_BOL operationResult_BOL = NewPurchaseOrder.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                    upGrid.Update(); // Refresh the grid visually
                }
                else
                {
                    bindGrid();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select at least one Purchase Ordder to proceed.');", true);
            }
        }

        //protected void btnRemovePrepayment_Click(object sender, EventArgs e)
        //{
        //    string purchaseOrderId = string.Empty;

        //    foreach (GridViewRow gridViewRow in gridView.Rows)
        //    {
        //        CheckBox chk_SelectSingle = (CheckBox)gridViewRow.FindControl("chk_SelectSingle");
        //        if (chk_SelectSingle != null && chk_SelectSingle.Checked)
        //        {
        //            LinkButton lnkPurchcaseOrderId = (LinkButton)gridViewRow.FindControl("lnkPurchcaseOrderId");
        //            purchaseOrderId = lnkPurchcaseOrderId?.Text;
        //            break;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(purchaseOrderId))
        //    {
        //        BussinessObject.SysOperationResult_BOL results = PortalIntegration.PurchaseOrderPrepayment.removePrepayment(purchaseOrderId, 0, true);

        //        if (results.isSuccess)
        //        {
        //            NotificationMessage.showMessage(results);
        //            gridView.EditIndex = -1;
        //            reBindGrid();
        //            upGrid.Update();
        //        }
        //        else
        //        {
        //            NotificationMessage.showMessage(results);
        //        }
        //    }
        //}

        protected void btnRemovePrepayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["PurchaseOrderId"] == null)
                {
                    SysOperationResult_BOL noSelection = new SysOperationResult_BOL();
                    noSelection.isSuccess = false;
                    noSelection.AlertType = AlertType.Error.ToString();
                    noSelection.Message = "Please select a Purchase Order first.";
                    NotificationMessage.showMessage(noSelection);
                    return;
                }

                string purchaseOrderId = Session["PurchaseOrderId"].ToString();

                long prepaymentRecId = 0;
                if (Session["RecId"] != null)
                {
                    long.TryParse(Session["RecId"].ToString(), out prepaymentRecId);
                }

                // Build the DataTable expected by the remove(DataTable) consumption method
                DataTable dt = new DataTable();
                dt.Columns.Add("purchId", typeof(string));
                //dt.Columns.Add("prepaymentRecId", typeof(string));
                //dt.Columns.Add("forceRemove", typeof(string));

                DataRow row = dt.NewRow();
                row["purchId"] = purchaseOrderId;
                //row["prepaymentRecId"] = prepaymentRecId.ToString();
                //row["forceRemove"] = "true";
                dt.Rows.Add(row);

                PurchaseOrder_PrePaymentsSvc prepaymentService = new PurchaseOrder_PrePaymentsSvc();
                SysOperationResult_BOL results = prepaymentService.remove(dt);

                if (results != null && results.isSuccess)
                {
                    NotificationMessage.showMessage(results);
                  //  gridView.EditIndex = -1;
                  //  reBindGrid();
                    //upGrid.Update();
                }
                else
                {
                    NotificationMessage.showMessage(results);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;
                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.Text.Trim();
                    }
                    break; // stop after first selected record
                }
            }

            if (!string.IsNullOrEmpty(purchaseOrderId))
            {
                SysOperationResult_BOL results = NewPurchaseOrder.cancelPurchaseOrder(purchaseOrderId);

                if (results.isSuccess)
                {
                    NotificationMessage.showMessage(results);
                    gridView.EditIndex = -1;
                    reBindGrid();
                    upGrid.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "refreshPage",
                          "setTimeout(function(){ window.location.reload(); }, 3000);", true);
                }
                else
                {
                    NotificationMessage.showMessage(results);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select a Purchase Order to cancel.');", true);
            }
        }

        protected void btnRequestChange_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;
                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.Text; // or use CommandArgument
                    }

                    break; // stop after first selected record
                }
            }

            if (!string.IsNullOrEmpty(purchaseOrderId))
            {
                SysOperationResult_BOL results = NewPurchaseOrder.ChangeRequest_PO(purchaseOrderId);

                if (results.isSuccess)
                {
                    NotificationMessage.showMessage(results);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "refreshPage",
                          "setTimeout(function(){ window.location.reload(); }, 3000);", true);
                }
                else
                {
                    NotificationMessage.showMessage(results);
                }
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.purchaseOrder_Submit(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                    upGrid.Update(); // Refresh the grid visually
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "refreshPage",
                          "setTimeout(function(){ window.location.reload(); }, 3000);", true);
                }
                else
                {
                    reBindGrid();
                    upGrid.Update();
                }
            }

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            PurchaseOrderHeader header = new PurchaseOrderHeader();

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    LinkButton lnk = row.FindControl("lnkPurchcaseOrderId") as LinkButton;
                    if (lnk != null && !string.IsNullOrEmpty(lnk.Text))
                    {
                        SysOperationResult_BOL result = header.purchaseOrder_Confirm(lnk.Text);

                        NotificationMessage.showMessage(result);
                    }
                }
            }

            gridView.EditIndex = -1;
            reBindGrid();
        }





        //protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool isAnyRowSelected = false;
        //    btnSubmit.Enabled = false;   // default
        //    btnWorkflow.Enabled = true;  // default

        //    btnProductReceipt.Visible = false;
        //    btnReceiptsList.Visible = false;

        //    foreach (GridViewRow gridViewRow in gridView.Rows)
        //    {
        //        CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
        //        if (chkSelectRow != null && chkSelectRow.Checked)
        //        {
        //            isAnyRowSelected = true;

        //            string approvalStatus = ((Label)gridViewRow.FindControl("lblApprovalStatus")).Text;
        //            string purchaseOrderStatus = ((Label)gridViewRow.FindControl("lblPurchaseOrderStatus")).Text;

        //            if (approvalStatus == "Confirmed" && purchaseOrderStatus == "Open order")
        //            {
        //                btnProductReceipt.Visible = true;
        //                btnReceiptsList.Visible = true;
        //            }
        //            else
        //            {
        //                btnProductReceipt.Visible = false;
        //                btnReceiptsList.Visible = false;
        //            }


        //            if (!string.IsNullOrEmpty(approvalStatus))
        //            {
        //                switch (approvalStatus)
        //                {
        //                    case "Draft":
        //                        btnSubmit.Enabled = true;
        //                        //btnSubmit.CssClass = "enabled-button";
        //                        btnWorkflow.Enabled = true;
        //                        btnDelete.Enabled = true;
        //                        btnRequestChange.Enabled = false;
        //                        break;


        //                    case "In review":
        //                    case "Confirmed":
        //                        //btnSubmit.Enabled = false;
        //                        btnSubmit.CssClass = "disabled-button";
        //                        btnWorkflow.Enabled = false;
        //                        btnWorkflow.CssClass = "disabled-button";
        //                        btnDelete.Enabled = false;
        //                        btnRequestChange.Enabled = false;
        //                        break;

        //                    case "Approved":
        //                        btnSubmit.CssClass = "disabled-button";
        //                        btnWorkflow.Enabled = false;
        //                        btnWorkflow.CssClass = "disabled-button";
        //                        btnDelete.Enabled = false;
        //                        btnRequestChange.Enabled = true;   //
        //                        break;

        //                    default:
        //                        btnSubmit.Enabled = true;
        //                        btnWorkflow.Enabled = true;
        //                        btnDelete.Enabled = true;
        //                        btnRequestChange.Enabled = false;
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    // If no row selected → enable workflow, keep submit disabled
        //    if (!isAnyRowSelected)
        //    {
        //        btnSubmit.Enabled = false;
        //        btnWorkflow.Enabled = true;
        //        btnWorkflow.CssClass = "";
        //    }
        //}
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool isAnyRowSelected = false;

            btnProductReceipt.Visible = false;
            btnReceiptsList.Visible = false;
            btnSubmit.Enabled = false;
            btnWorkflow.Enabled = true;
            btnDelete.Enabled = false;
            btnRequestChange.Enabled = false;
            btnCancelOrder.Enabled = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow != sender)
                {
                    chkSelectRow.Checked = false; // Uncheck others
                }

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    isAnyRowSelected = true;
                    gridViewRow.CssClass = "selected-row"; // Apply Highlight

                    // ===== READ VALUES =====
                    string purchaseOrderId = ((LinkButton)gridViewRow.FindControl("lnkPurchcaseOrderId"))?.Text;
                    string approvalStatus = ((Label)gridViewRow.FindControl("lblApprovalStatus"))?.Text;
                    string purchaseOrderStatus = ((Label)gridViewRow.FindControl("lblPurchaseOrderStatus"))?.Text;

                    // ===== STORE IN SESSION =====
                    Session["PurchaseOrderId"] = purchaseOrderId;
                    Session["ApprovalStatus"] = approvalStatus;
                    Session["PurchaseOrderStatus"] = purchaseOrderStatus;
                    Session["VendorAccount"] = ((Label)gridViewRow.FindControl("lblVendorAccount"))?.Text;
                    Session["InvoiceAccount"] = ((Label)gridViewRow.FindControl("lblInvoiceAccount"))?.Text;
                    Session["VendorName"] = ((Label)gridViewRow.FindControl("lblVendorName"))?.Text;
                    Session["Currency"] = ((Label)gridViewRow.FindControl("lblCurrency"))?.Text;
                    Session["RecId"] = ((Label)gridViewRow.FindControl("lblRecId"))?.Text;

                    // ===== UI LOGIC =====
                    if (approvalStatus == "Confirmed" && purchaseOrderStatus == "Open order")
                    {
                        btnProductReceipt.Visible = true;
                        btnReceiptsList.Visible = true;
                    }

                    switch (approvalStatus)
                    {
                        case "Draft":
                            btnSubmit.Enabled = true;
                            btnWorkflow.Enabled = true;
                            btnDelete.Enabled = true;
                            btnRequestChange.Enabled = false;
                            break;

                        case "In review":
                        case "Confirmed":
                            btnSubmit.Enabled = false;
                            btnWorkflow.Enabled = false;
                            btnDelete.Enabled = false;
                            btnRequestChange.Enabled = false;
                            break;

                        case "Approved":
                            btnSubmit.Enabled = false;
                            btnWorkflow.Enabled = false;
                            btnDelete.Enabled = false;
                            btnRequestChange.Enabled = true;
                            break;
                    }

                    if (purchaseOrderStatus != "Canceled" && purchaseOrderStatus != "Invoiced")
                    {
                        btnCancelOrder.Enabled = true;
                    }
                    else
                    {
                        btnCancelOrder.Enabled = false;
                    }
                    break; // stop loop after finding the selected row
                }
                else
                {
                    gridViewRow.CssClass = ""; // Reset Highlight for other rows
                }
            }

            if (!isAnyRowSelected)
            {
                Session.Remove("PurchaseOrderId");
                btnCancelOrder.Enabled = false;
            }
            
            upGrid.Update();
        }

        protected void btnTotal_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    string purchIdfortotal = (row.FindControl("lnkPurchcaseOrderId") as LinkButton)?.Text.Trim();
                    Session["purchIdTotal"] = purchIdfortotal;

                    isSelected = true;
                    string script = "openPopupPanel('/ESS/PR/PurchaseOrder_Total.aspx', 700);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupOnHand", script, true);
                    return; // stop loop after redirect
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('No  Record was selected');", true);
            }

        }

        //protected void btnMaintainCharges_Click(object sender, EventArgs e)
        //{
        //    string purchaseOrderId= string.Empty;

        //    // Loop through GridView rows to find the selected record
        //    foreach (GridViewRow gridViewRow in gridView.Rows)
        //    {
        //        CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

        //        if (chkSelectRow != null && chkSelectRow.Checked)
        //        {
        //            // Get the Purchase Order ID from the LinkButton
        //            LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchaseOrderId") as LinkButton;

        //            if (lnkPurchOrder != null)
        //            {
        //                purchaseOrderId = lnkPurchOrder.Text; // could also use CommandArgument
        //            }

        //            break; // Stop after first selected record
        //        }
        //    }

        //    // If nothing selected
        //    if (string.IsNullOrEmpty(purchaseOrderId))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //            "alert('Please select a Purchase Order first.');", true);
        //        return;
        //    }

        //    // Redirect to Maintain Charges form with PO ID
        //    Response.Redirect("PurchaseOrderLines_MaintainCharges.aspx?PO=" + purchaseOrderId);
        //}



        //protected void btnMaintainCharges_Click(object sender, EventArgs e)

        //{

        //    string purchaseOrderId = string.Empty;

        //    // Loop through GridView rows to find the selected record

        //    foreach (GridViewRow gridViewRow in gridView.Rows)

        //    {

        //        CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

        //        if (chkSelectRow != null && chkSelectRow.Checked)

        //        {

        //            // Get the Purchase Order ID from the LinkButton

        //            LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

        //            if (lnkPurchOrder != null)

        //            {

        //                purchaseOrderId = lnkPurchOrder.Text; // could also use CommandArgument

        //            }

        //            break; // Stop after first selected record

        //        }

        //    }

        //    // If nothing selected

        //    if (string.IsNullOrEmpty(purchaseOrderId))

        //    {

        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert",

        //            "alert('Please select a Purchase Order first.');", true);

        //        return;

        //    }

        //    // Redirect to Maintain Charges form with PO ID

        //    Response.Redirect("PurchaseOrderLines_MaintainCharges.aspx?PO=" + purchaseOrderId);

        //}



        // customize

        protected void btnMaintainCharges_Click(object sender, EventArgs e)
        {
            try
            {
                bool found = false;

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        found = true;

                        // ✅ Purchase Order ID
                        LinkButton lnkPurchOrder =
                            row.FindControl("lnkPurchcaseOrderId") as LinkButton;

                        string purchaseOrderId = lnkPurchOrder?.Text;

                        // ✅ PO Header RecId (IMPORTANT)
                        Label lblRecId = row.FindControl("lblRecId") as Label;
                        long recId = 0;

                        if (lblRecId == null || !long.TryParse(lblRecId.Text, out recId))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "InvalidRecId",
                                "alert('Invalid Purchase Order RecId.');", true);
                            return;
                        }

                        // ✅ STORE IN SESSION (CRITICAL FIX)
                        Session["PurchaseOrderId"] = purchaseOrderId;
                        Session["RecId"] = recId;
                        Session["MaintainChargesSource"] = "HEADER";

                        // ✅ Redirect
                        Response.Redirect("/ESS/PR/PurchaseOrderLines_MaintainCharges.aspx", false);
                        return;
                    }
                }

                if (!found)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                        "alert('Please select a Purchase Order first.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('Error opening Maintain Charges: {ex.Message}');", true);
            }
        }


        protected void btnSalesTax_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;

            // Loop through GridView rows to find the selected record
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the LinkButton
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.Text; // could also use CommandArgument
                    }

                    break; // Stop after first selected record
                }
            }

            // If nothing selected
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }
           Session["SelectedPurchId"] = purchaseOrderId;


            // Redirect to Maintain Charges form with PO ID
            //string returnUrl = Request.Url.PathAndQuery;
            //Response.Redirect("SalesTax.aspx?PO=" + purchaseOrderId + "&returnUrl=" + Server.UrlEncode(returnUrl));

            string returnUrl = Request.Url.PathAndQuery;
            string encodedReturnUrl = Server.UrlEncode(returnUrl);

            // Open SalesTax.aspx in a popup using JavaScript
            string script = $"openPopupPanel('/ESS/PR/SalesTax.aspx?PurchId={purchaseOrderId}&returnUrl={encodedReturnUrl}', 1200);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupSalesTax", script, true);
        }


        protected void btnProductReceipt_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;


            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the LinkButton
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.CommandArgument; // Use CommandArgument for accuracy
                    }

                    break; // Stop after first selected record
                }
            }

            // If nothing selected
            //if (string.IsNullOrEmpty(purchaseOrderId))
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
            //        "alert('Please select a Purchase Order first.');", true);
            //    return;
            //}
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Please select a Purchase Order first.";
                NotificationMessage.showMessage(result);
                return;
            }



            // Store Purchase Order ID in Session
            Session["PurchaseOrderId"] = purchaseOrderId;
            Session["UpdateMode"] = "Product receipt";

            //// Redirect to Product Receipt Page
            //Response.Redirect("/ESS/PR/PurchaseOrder_ProductReceipt.aspx");

            // ✅ Open as popup
            string script = "openPopupPanel('/ESS/PR/PurchaseOrder_ProductReceipt.aspx', 1200);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupProductReceipt", script, true);
        }


        protected void btnReceiptsList_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;


            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the LinkButton
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.CommandArgument; // Use CommandArgument for accuracy
                    }

                    break; // Stop after first selected record
                }
            }

            // If nothing selected
            //if (string.IsNullOrEmpty(purchaseOrderId))
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
            //        "alert('Please select a Purchase Order first.');", true);
            //    return;
            //}
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Please select a Purchase Order first.";
                NotificationMessage.showMessage(result);
                return;
              
            }

            Session.Remove("UpdateMode");

            // Store Purchase Order ID in Session
            Session["PurchaseOrderId"] = purchaseOrderId;
            Session["UpdateMode"] = "Receipts list";

            // Redirect to Product Receipt Page
            Response.Redirect("/ESS/PR/PurchaseOrder_ReceiptLists.aspx");
        }

        protected void btnReceiptList_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;


            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the LinkButton
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.CommandArgument; // Use CommandArgument for accuracy
                    }

                    break; // Stop after first selected record
                }
            }

            // If nothing selected
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }

            // Store Purchase Order ID in Session
            Session["PurchaseOrderId"] = purchaseOrderId;
            Session["UpdateMode"] = "Receipts list";

            // Redirect to Product Receipt Page
            Response.Redirect("/ESS/PR/PurchaseOrder_ReceiptListJournal.aspx");
        }

        protected void btnProductReceipst_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;


            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the LinkButton
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.CommandArgument; // Use CommandArgument for accuracy
                    }

                    break; // Stop after first selected record
                }
            }

            // If nothing selected
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }

            // Store Purchase Order ID in Session
            Session["PurchaseOrderId"] = purchaseOrderId;

            // Redirect to Product Receipt Page
            Response.Redirect("/ESS/PR/PurchaseOrder_ProductReceiptJournal.aspx");
        }

        protected void btnInvoiceJournal_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ESS/PR/InvoiceJournal.aspx");
        }

        protected void btnPendingInvoice_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ESS/PR/PendingInvoice.aspx");
        }


        //protected void btnInvoice_Click(object sender, EventArgs e)
        //{
        //    string purchaseOrderId = string.Empty;


        //    foreach (GridViewRow gridViewRow in gridView.Rows)
        //    {
        //        CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

        //        if (chkSelectRow != null && chkSelectRow.Checked)
        //        {
        //            // Get the Purchase Order ID from the LinkButton
        //            LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

        //            if (lnkPurchOrder != null)
        //            {
        //                purchaseOrderId = lnkPurchOrder.CommandArgument; // Use CommandArgument for accuracy
        //            }

        //            break; // Stop after first selected record
        //        }
        //    }

        //    // If nothing selected
        //    if (string.IsNullOrEmpty(purchaseOrderId))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //            "alert('Please select a Purchase Order first.');", true);
        //        return;
        //    }

        //    // Store Purchase Order ID in Session
        //    Session["PurchaseOrderId"] = purchaseOrderId;


        //    // Redirect to Product Receipt Page
        //    Response.Redirect("/ESS/PR/PurchaseOrder_Invoices.aspx");
        //}

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkPurchOrder = gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;
                    if (lnkPurchOrder != null)
                    {
                        purchaseOrderId = lnkPurchOrder.CommandArgument?.Trim(); // ✅ Trim whitespace
                    }
                    break;
                }
            }

            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }

            // ✅ Verify purchaseOrderId before storing
            System.Diagnostics.Debug.WriteLine("Invoice button clicked. PurchaseOrderId: " + purchaseOrderId);

            Session["PurchaseOrderId"] = purchaseOrderId;
            Response.Redirect("/ESS/PR/PurchaseOrder_Invoices.aspx");
        }


        protected void btnAllocateCharges_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = string.Empty;

            // 🔍 Find selected row
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow =
                    gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkPurchaseOrder =
                        gridViewRow.FindControl("lnkPurchcaseOrderId") as LinkButton;

                    if (lnkPurchaseOrder != null)
                    {
                        purchaseOrderId = lnkPurchaseOrder.CommandArgument;
                    }

                    break;
                }
            }

            // ❌ No selection
            if (string.IsNullOrEmpty(purchaseOrderId))
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "alert",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }

            // ✅ Store PO in Session
            Session["SelectedPurchId"] = purchaseOrderId;

            // 🔁 Return URL
            string returnUrl = Request.Url.PathAndQuery;
            string encodedReturnUrl = Server.UrlEncode(returnUrl);

            // 🚀 Open Allocate Charges popup
            string script =
                $"openPopupPanel('/ESS/PR/PurchaseOrder_AllocateCharges.aspx?PurchId={purchaseOrderId}&returnUrl={encodedReturnUrl}', 700);";

            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "OpenPopupAllocateCharges", script, true);
        }


        protected void btnCreateNote_Click(object sender, EventArgs e)
        {
            string vendorAccount = Session["VendorAccount"] as string;
            string purchaseOrderId = Session["PurchaseOrderId"].ToString();

            if (string.IsNullOrEmpty(vendorAccount))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select a Purchase Order first.');", true);
                return;
            }

            Session["SelectedPurchIdPo"] = purchaseOrderId;
            Session["SelectedVendorAccount"] = vendorAccount;

            string script = "openPopupPanel('/ESS/PR/PurchaseOrder_CreateNote.aspx', 900);";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenCreditNote", script, true);
        }

        protected void btnPrepayment_Click(object sender, EventArgs e)
        {
            if (Session["PurchaseOrderId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoPoSelected",
                    "alert('Please select a purchase order first.');", true);
                return;
            }

            string purchaseOrderId = Session["PurchaseOrderId"].ToString();
            Session["SelectedPurchIdPo"] = purchaseOrderId;

            string script = "openPopupPanel('/ESS/PR/PurchaseOrder_Prepayment.aspx', 700);";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenPrepayment", script, true);
        }

        private static string FormatLabel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;
            // Fix: insert space before capitals that follow a lowercase letter
            // prevents leading space on first capital
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
            // ✅ Use HttpContext.Current.Session instead of Page.Session
            long lastRecId = HttpContext.Current.Session["LastRecId"] != null
                ? Convert.ToInt64(HttpContext.Current.Session["LastRecId"])
                : 9223372036854775807;

            int pageSize = HttpContext.Current.Session["PageSize"] != null
                ? Convert.ToInt32(HttpContext.Current.Session["PageSize"])
                : 10;


            PurchaseOrderHeader NewPurchaseOrder = new PurchaseOrderHeader();
            DataTable dt = NewPurchaseOrder.retrieveAll(lastRecId, pageSize);

            var columns = dt.Columns
                .Cast<DataColumn>()
                .Select(col => new
                {
                    Field = col.ColumnName,
                    Label = FormatLabel(col.ColumnName),
                    Visible = true
                })
                .ToList();

            return new JavaScriptSerializer().Serialize(columns);
        }

    }
}