using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequisitionHeader_Listpage : MainForm
    {
        private PurchaseRequestGroup purchaseRequestGroup = new PurchaseRequestGroup();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = purchaseRequestGroup.tableName;
                pageMenuId = "PRPurchasedRequisition";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Purchase requisitions prepared by me";

                    // Dynamically set the page title in the master page div
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Purchase requisitions prepared by me"; // Set the text in the div
                        titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                    }

                    reBindGrid();
                  
                }


                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }

        private void getGridDataTable()
        {
            DataTable dt = purchaseRequestGroup.retrieveAll();
            SessionVariables.setSessionDataTable(dt);
        }

        protected void lnkPurchReqId_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string purchReqId = btn.CommandArgument;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label lblOriginator = (Label)row.FindControl("lblOriginator");
            LinkButton lblPurchReqId = (LinkButton)row.FindControl("lnkPurchReqId");
            string originator = lblOriginator != null ? lblOriginator.Text.Trim() : "";
            string purchReq = lblPurchReqId != null ? lblPurchReqId.Text.Trim() : "";
            Label lblPurchReqName = (Label)row.FindControl("lblPurchReqName");
            string name = lblPurchReqName != null ? lblPurchReqName.Text.Trim() : "";
            Label lblRequisitionPurpose = (Label)row.FindControl("lblRequisitionPurpose");
            string reqpurpose = lblRequisitionPurpose != null ? lblRequisitionPurpose.Text.Trim() : "";
            Label lblRequisitionStatus = (Label)row.FindControl("lblRequisitionStatus");
            string requisitionStatus = lblRequisitionStatus != null ? lblRequisitionStatus.Text.Trim() : "";
            Label lblRequestedDate = row.FindControl("lblRequestedDate") as Label;
            string requestedDate = lblRequestedDate != null ? lblRequestedDate.Text.Trim() : "";
            Label lblAccountingDate = row.FindControl("lblAccountingDate") as Label;
            string accountingDate = lblAccountingDate != null ? lblAccountingDate.Text.Trim() : "";
            Label lblRecId = row.FindControl("lblRecId") as Label;
            string recId = lblRecId != null ? lblRecId.Text.Trim() : "";
            Label lblCreatedBy = row.FindControl("lblCreatedBy") as Label;
            string createdBy = lblCreatedBy != null ? lblCreatedBy.Text.Trim() : "";
            Label lblCreatedDateTime = row.FindControl("lblCreatedDateTime") as Label;
            string createdDateTime = lblCreatedDateTime != null ? lblCreatedDateTime.Text.Trim() : "";
            Label lblSubmittedBy = row.FindControl("lblSubmittedBy") as Label;
            string submittedBy = lblSubmittedBy != null ? lblSubmittedBy.Text.Trim() : "";
            Label lblSubmittedDateTime = row.FindControl("lblSubmittedDateTime") as Label;
            string submittedDateTime = lblSubmittedDateTime != null ? lblSubmittedDateTime.Text.Trim() : "";
            Label lblReasonCode = row.FindControl("lblReasonCode") as Label;
            string reasonCode = lblReasonCode != null ? lblReasonCode.Text.Trim() : "";

            Session["Originator"] = originator;
            Session["PurchReqId"] = purchReq;
            Session["PurchReqName"] = name;
            Session["RequisitionPurpose"] = reqpurpose;
            Session["RequisitionStatus"] = requisitionStatus;
            Session["RequiredDate"] = requestedDate;
            Session["TransDate"] = accountingDate;
            Session["CreatedBy"] = createdBy;
            Session["CreatedDateTime"] = createdDateTime;
            Session["SubmittedBy"] = submittedBy;
            Session["SubmittedDateTime"] = submittedDateTime;
            Session["RecId"] = recId;
            Session["ReasonCode"] = reasonCode;
            // Redirect with query parameter
            //Response.Redirect($"/ESS/PR/PurchaseRequisitionLines.aspx?PurchReqId={Server.UrlEncode(purchReqId)}");

            Session["PurchReqId"] = purchReqId;

            // Redirect to target page (no query string in URL)
            Response.Redirect("/ESS/PR/PurchaseRequisitionLines.aspx");




            //    string originator = lblOriginator != null ? lblOriginator.Text.Trim() : "";

            //    // Escape quotes for JavaScript safety
            //    originator = originator.Replace("'", "\\'");

            //    // Inject JavaScript to set the value of a textbox
            //    string script = $@"
            //<script type='text/javascript'>
            //    document.getElementById('txtOriginator').value = '{originator}';
            //</script>";

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetOriginator", script, false);


        }

        protected void reBindGrid()
        {
            getGridDataTable(); 
            bindGrid();
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lnkPurchReqId") as LinkButton).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.purchaseRequisition_Submit(recordsId.ToArray());
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

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
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
                SysOperationResult_BOL operationResult_BOL = purchaseRequestGroup.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();

                    // Force full page reload
                    Response.Redirect(Request.RawUrl, false);
                }
                else
                {
                    bindGrid();
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblRequisitionStatus");
                Label lblOriginator = (Label)e.Row.FindControl("lblOriginator");
                


                Label lblCreatedDateTime = (Label)e.Row.FindControl("lblCreatedDateTime");
                if (lblCreatedDateTime != null && DateTime.TryParse(lblCreatedDateTime.Text, out DateTime craetedDate))
                {
                    lblCreatedDateTime.Text = craetedDate == new DateTime(1900, 1, 1)
                    ? ""
                    : craetedDate.ToString("M-d-yyyy");
                }

                Label lblSubmittedDateTime = (Label)e.Row.FindControl("lblSubmittedDateTime");
                if (lblSubmittedDateTime != null && DateTime.TryParse(lblSubmittedDateTime.Text, out DateTime submittedDate))
                {
                    lblSubmittedDateTime.Text = submittedDate == new DateTime(1900, 1, 1)
                    ? ""
                    : submittedDate.ToString("M-d-yyyy");

                }
            }
        }

        // Handle LinkButton click inside GridView to open details page
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenRequisition")
            {
                string purchReqId = e.CommandArgument.ToString();
                string url = $"/ESS/PR/PurchaseRequisitionLines.aspx?PurchReqId={Server.UrlEncode(purchReqId)}";
                Response.Redirect(url);
            }
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool noRecordsChecked = true;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow.Checked)
                {
                    noRecordsChecked = false;
                    Label recordStatus = gridViewRow.FindControl("lblRequisitionStatus") as Label;
                    if (recordStatus != null && string.Equals(recordStatus.Text, "Draft"))
                    {
                        btnDelete.Enabled = true;
                        btnSubmit.Enabled = true;
                        btnWorkflow.Enabled = true;
                    }
                    else
                    {
                        btnDelete.Enabled = false;
                        btnSubmit.Enabled = false;
                        btnWorkflow.Enabled = false;
                        break;
                    }
                }
            }
            if (noRecordsChecked)
            {
                btnDelete.Enabled = false;
                btnSubmit.Enabled = true;
                btnWorkflow.Enabled = true;
            }
        }
        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        protected void btnPRTotal_Click(object sender, EventArgs e)
        {
            string purchReqId = "";
            bool singleRecordSelected = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkPurchReqId = gridViewRow.FindControl("lnkPurchReqId") as LinkButton;

                    purchReqId = lnkPurchReqId != null ? lnkPurchReqId.Text.Trim() : "";

                    if (!string.IsNullOrEmpty(purchReqId) && !singleRecordSelected)
                    {
                        singleRecordSelected = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                            "alert('More than one record Selected.\nPlease select only one Record.');", true);
                    }
                }
            }

            if (!string.IsNullOrEmpty(purchReqId))
            {
                string script = $"openPopupPanel('/ESS/PR/PurchaseRequisitionTotal.aspx?PurchReqId={purchReqId}', 1000);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                    "alert('Please select a record to Continue');", true);
            }
        }
    }
}
