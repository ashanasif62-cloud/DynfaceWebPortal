using BussinessObject;
using GeneralAuxiliary;
using Newtonsoft.Json.Converters;
using PortalIntegration;
using PortalIntegration.ExpenseManagmentSvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.EM
{
    public partial class EMExpenseReport_Listpage : MainForm
    {
        private ExpenseReport expense = new ExpenseReport();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = expense.tableName;
                pageMenuId = "ExpenseReport_ListPage";

                base.addPageToRecent(pageMenuId);
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

               
                    reBindGrid();
                    gridView.RowDataBound += gridView_RowDataBound;
                    gridView.RowCommand += gridView_RowCommand;

              

                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
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
            finally
            { }
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (eventArgument == "RefreshGrid")
            {
                reBindGrid();
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
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
            DataTable dt = expense.retrieveAll();
            SessionVariables.setSessionDataTable(dt);
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridView.Rows[rowIndex];
                string expenseReportNumber = ((Label)row.FindControl("lblExpenseReportNumber")).Text;

                // Redirect with query string
                Response.Redirect("EMExpenseLines.aspx?ExpenseReportNumber=" + Server.UrlEncode(expenseReportNumber));
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            Label lblCraetedDateTime = e.Row.FindControl("lblCraetedDateTime") as Label;
            if (lblCraetedDateTime != null && DateTime.TryParse(lblCraetedDateTime.Text, out DateTime CraetedDate))
            {
                lblCraetedDateTime.Text = CraetedDate == new DateTime(1900, 1, 1)
                    ? ""
                    : CraetedDate.ToString("yyyy-MM-dd");
            }
            Label lblPaymentDate = e.Row.FindControl("lblPaymentDate") as Label;
            if (lblPaymentDate != null && DateTime.TryParse(lblPaymentDate.Text, out DateTime PaymentDate))
            {
                lblPaymentDate.Text = PaymentDate == new DateTime(1900, 1, 1)
                    ? ""
                    : PaymentDate.ToString("yyyy-MM-dd");
            }
            Label lblReceiptsAttached = e.Row.FindControl("lblReceiptsAttached") as Label;
            if (lblReceiptsAttached != null && lblReceiptsAttached.Text != "Draft")
            {
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                btnEdit.Enabled = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow && gridView.EditIndex == e.Row.RowIndex)
            {
                try
                {
                    DropDownList ddlPurpose = e.Row.FindControl("ddlPurpose") as DropDownList;
                    if (ddlPurpose != null)
                    {
                        ddlPurpose.Items.Clear();

                        // Retrieve or cache purpose data
                        DataTable dtPurpose = ViewState["PurposeData"] as DataTable ?? ControlsHelper.getPurpose();
                        if (dtPurpose != null && dtPurpose.Columns.Contains("Purpose"))
                        {
                            ViewState["PurposeData"] = dtPurpose;
                            ddlPurpose.DataSource = dtPurpose;
                            ddlPurpose.DataTextField = "Purpose";
                            ddlPurpose.DataValueField = "Purpose";
                            ddlPurpose.DataBind();

                            // Add empty item
                            if (ddlPurpose.Items.FindByValue("") == null)
                            {
                                ddlPurpose.Items.Insert(0, new ListItem("", ""));
                            }

                            // Set selected value
                            string selectedValue = DataBinder.Eval(e.Row.DataItem, "Purpose")?.ToString();
                            if (!string.IsNullOrEmpty(selectedValue) && ddlPurpose.Items.FindByValue(selectedValue) != null)
                            {
                                ddlPurpose.SelectedValue = selectedValue;
                            }
                        }
                    }

                    DropDownList ddlLocation = e.Row.FindControl("ddlLocation") as DropDownList;
                    if (ddlLocation != null)
                    {
                        ddlLocation.Items.Clear();

                        DataTable dtLocation = ViewState["LocationData"] as DataTable ?? ControlsHelper.getLocation();
                        if (dtLocation != null && dtLocation.Columns.Contains("Locations"))
                        {
                            ViewState["LocationData"] = dtLocation;
                            ddlLocation.DataSource = dtLocation;
                            ddlLocation.DataTextField = "Locations";
                            ddlLocation.DataValueField = "Locations";
                            ddlLocation.DataBind();

                            if (ddlLocation.Items.FindByValue("") == null)
                            {
                                ddlLocation.Items.Insert(0, new ListItem("", ""));
                            }

                            string selectedValue = DataBinder.Eval(e.Row.DataItem, "Location")?.ToString();
                            if (!string.IsNullOrEmpty(selectedValue) && ddlLocation.Items.FindByValue(selectedValue) != null)
                            {
                                ddlLocation.SelectedValue = selectedValue;
                            }
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
        }

        protected void Update_Click(object sender, EventArgs e)
        {
                try
                {
                    ExpenseReport report = new ExpenseReport();

                    GridViewRow row = gridView.Rows[gridView.EditIndex];
                    string recId = gridView.DataKeys[gridView.EditIndex].Value.ToString();

                    DropDownList ddlPurpose = (DropDownList)row.FindControl("ddlPurpose");
                    string selectedPurpose = ddlPurpose?.SelectedValue;

                    DropDownList ddlLocation = (DropDownList)row.FindControl("ddlLocation");
                    string selectedLocation = ddlLocation?.SelectedValue;

                    HyperLink lblExpenseReportNumber = (HyperLink)row.FindControl("lblExpenseReportNumber");
                    string expenseReportNumber = lblExpenseReportNumber?.Text;

                    var expenseReportFields = new List<ExpenseReportRequestSvc> { };

                    expenseReportFields.Add(new ExpenseReportRequestSvc
                    {
                        __k_parmFieldName = "Purpose",
                        __k_parmFieldValue = selectedPurpose,
                        __k_parmFieldType = "String"
                    });

                    expenseReportFields.Add(new ExpenseReportRequestSvc
                    {
                        __k_parmFieldName = "Location",
                        __k_parmFieldValue = selectedLocation,
                        __k_parmFieldType = "String"
                    });

                    report.updateExpenseReport(expenseReportFields, expenseReportNumber);
                }
                catch (Exception ex)
                {
                    SysErrorLog objErrorLog = new SysErrorLog();
                    System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                    string currentMethodName = currentMethod.DeclaringType.FullName;
                    objErrorLog.write(currentMethodName, ex);
                }
            gridView.EditIndex = -1;
            bindGrid();

            Response.Redirect(Request.RawUrl);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            reBindGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ExpenseReport report = new ExpenseReport();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string expenseReportNumber = (gridViewRow.FindControl("lnkExpenseReportNumber") as LinkButton).Text;
                    if (!string.IsNullOrEmpty(expenseReportNumber))
                    {
                        recordsId.Add(expenseReportNumber);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = report.submitExpenseReportToWorkflow(recordsId.ToArray());
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ExpenseReport report = new ExpenseReport();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string expenseTransactionNumber = "";
                    expenseTransactionNumber = ((HyperLink)gridViewRow.FindControl("lblExpenseReportNumber")).Text;
                    if (expenseTransactionNumber != "")
                    {
                        recordsId.Add(expenseTransactionNumber);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                //expenseLines.DeleteExpenseLine(recordsId.ToArray());
                SysOperationResult_BOL operationResult_BOL = report.deleteExpenseReport(recordsId.ToArray());
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

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);

        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool isAnyChecked = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    isAnyChecked = true;
                    Label recordStatus = gridViewRow.FindControl("lblReceiptsAttached") as Label;
                    if (recordStatus != null && string.Equals(recordStatus.Text, "Draft"))
                    {
                        btnDelete.Enabled = true;
                        btnSubmit.Enabled = true;
                    }
                    else
                    {
                        btnDelete.Enabled = false;
                        btnSubmit.Enabled = false;
                        break;
                    }
                }
            }
            if (!isAnyChecked)
            {
                btnDelete.Enabled = true;
                btnSubmit.Enabled = true;
            }
        }

        protected void lnkExpenseReportNumber_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string expenseReportNumber = btn.CommandArgument;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label lblReceiptsAttached = row.FindControl("lblReceiptsAttached") as Label;
            string approvalStatus = lblReceiptsAttached != null ? lblReceiptsAttached.Text.Trim() : "";
            Label lblPurpose = row.FindControl("lblPurpose") as Label;
            string purpose = lblPurpose != null ? lblPurpose.Text.Trim() : "";
            Label lblLocation = row.FindControl("lblLocation") as Label;
            string location = lblLocation != null ? lblLocation.Text.Trim() : "";
            Label lblAmountCurr = row.FindControl("lblAmountCurr") as Label;
            string amount = lblAmountCurr != null ? lblAmountCurr.Text.Trim() : "";
            Label lblCraetedDateTime = row.FindControl("lblCraetedDateTime") as Label;
            string createdDate = lblCraetedDateTime != null ? lblCraetedDateTime.Text.Trim() : "";
            Label lblPaymentDate = row.FindControl("lblPaymentDate") as Label;
            string paymentDate = lblPaymentDate != null ? lblPaymentDate.Text.Trim() : "";
            Label lblPaymentVoucher = row.FindControl("lblPaymentVoucher") as Label;
            string paymentVoucher = lblPaymentVoucher != null ? lblPaymentVoucher.Text.Trim() : "";
            Label lblInvoice = row.FindControl("lblInvoice") as Label;
            string invoice = lblInvoice != null ? lblInvoice.Text.Trim() : "";
            Label lblRecId = row.FindControl("lblRecId") as Label;
            string recId = lblRecId != null ? lblRecId.Text.Trim() : "";
            Label defaultDimension = row.FindControl("lblDefaultDimension") as Label;
            string Dimension = defaultDimension != null ? defaultDimension.Text.Trim() : "";

            Session["ExpenseReportNumber"] = expenseReportNumber;
            Session["ApprovalStatus"] = approvalStatus;
            Session["Purpose"] = purpose;
            Session["Location"] = location;
            Session["Amount"] = amount;
            Session["CreatedDate"] = createdDate;
            Session["PaymentDate"] = paymentDate;
            Session["PaymentVoucher"] = paymentVoucher;
            Session["Invoice"] = invoice;
            Session["RecId"] = recId;
            Session["DefaultDimension"] = Dimension;


            // Redirect to target page (no query string in URL)
            Response.Redirect("/ESS/EM/EMExpenseLines.aspx");

        }
    }
}