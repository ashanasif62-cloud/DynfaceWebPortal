using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PREmployeeLeaveEncashment_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PREmployeeLeaveEncashment_Create";

            base.Page_Load(sender, e);

            // Optional: You could load data only once on first load
            if (!IsPostBack)
            {
                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create Leave Encashment Request";

                }
                bindControlsData();
            }

            txtRequestDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

        }

        private void bindControlsData()
        {
            PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();

            //TextBox test = cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox;

            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = encashment.getEntitlementCode(employeeId);
            ddlEntitlementCode.DataSource = dt;
            ddlEntitlementCode.DataTextField = "EntitlementCode";
            ddlEntitlementCode.DataValueField = "EntitlementCode";
            ddlEntitlementCode.DataBind();

            ddlEntitlementCode.Items.Insert(0, new ListItem("", ""));

            // Make sure nothing is selected
            ddlEntitlementCode.SelectedIndex = 0;

        }


        protected void OnEntitlementCodeSelection(object sender, EventArgs e)
        {
            PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();

            

            string employeeId = SessionVariables.getCurrentEmployeeId();
            string entitlementCode = ddlEntitlementCode.SelectedValue;
            DateTime requestDate;
            if (!DateTime.TryParse(txtRequestDate.Text, out requestDate))
            {
                requestDate = DateTime.Today;   // fallback
            }
            DataTable dt = encashment.getBalanceBeforeApplication(employeeId, entitlementCode, requestDate);

            decimal balance = Convert.ToDecimal(dt.Rows[0]["BalanceBeforeApplication"]);
            txtLeavesBalanceBefore.Text = balance.ToString("0.00");

            txtLeavestoBeEncashed.Text = balance.ToString("0.00");
            CalculateEarningAmount(employeeId, entitlementCode, balance);


        }

        protected void OnLeavesToBeEncashedModified(object sender, EventArgs e)
        {
            PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();

            string employeeId = SessionVariables.getCurrentEmployeeId();
            string entitlementCode = ddlEntitlementCode.SelectedValue;

            decimal balnceBeforeApplication = 0;
            decimal.TryParse(txtLeavesBalanceBefore.Text, out balnceBeforeApplication);
           
            decimal leavesToBeEncashed = 0;
            decimal.TryParse(txtLeavestoBeEncashed.Text, out leavesToBeEncashed);

            DataTable dt = encashment.getEarningAmount(employeeId, entitlementCode, leavesToBeEncashed, balnceBeforeApplication);

            decimal earningAmount = Convert.ToDecimal(dt.Rows[0]["EarningAmount"]);
            //txtEarningAmount.Text = earningAmount.ToString("#,##0.00");

            decimal remainingBalance = Convert.ToDecimal(dt.Rows[0]["RemainingBalance"]);
            txtRemainingBalance.Text = remainingBalance.ToString("0.00");

        }

        private void CalculateEarningAmount(string employeeId, string entitlementCode, decimal leavesToBeEncashed)
        {
            PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();

            decimal balnceBeforeApplication = 0;
            decimal.TryParse(txtLeavesBalanceBefore.Text, out balnceBeforeApplication);

            DataTable dt = encashment.getEarningAmount(employeeId, entitlementCode, leavesToBeEncashed, balnceBeforeApplication);

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal earningAmount = Convert.ToDecimal(dt.Rows[0]["EarningAmount"]);
                //txtEarningAmount.Text = earningAmount.ToString("#,##0.00");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeId", typeof(string));
                dt.Columns.Add("RequestDate", typeof(DateTime));
                dt.Columns.Add("EntitlementCode", typeof(string));
                dt.Columns.Add("LeaveBalanceBefore", typeof(decimal));
                dt.Columns.Add("LeavesToBeEncashed", typeof(decimal));
                dt.Columns.Add("EarningAmount", typeof(decimal));
                dt.Columns.Add("RemainingBalance", typeof(decimal));
                dt.Columns.Add("LastEncashmentDate", typeof(DateTime));
                dt.Columns.Add("LastEncashmentLeaves", typeof(decimal));



                DataRow row = dt.NewRow();

                row["EmployeeId"] = SessionVariables.getCurrentEmployeeId();

                // Request Date
                DateTime requestDate;
                DateTime.TryParse(txtRequestDate.Text, out requestDate);
                row["RequestDate"] = requestDate;

                // Entitlement
                row["EntitlementCode"] = ddlEntitlementCode.SelectedValue;

                // Decimals parsing safely
                decimal leaveBalanceBefore = 0;
                decimal.TryParse(txtLeavesBalanceBefore.Text.Trim(), out leaveBalanceBefore);
                row["LeaveBalanceBefore"] = leaveBalanceBefore;

                decimal leavesToBeEncashed = 0;
                decimal.TryParse(txtLeavestoBeEncashed.Text.Trim(), out leavesToBeEncashed);
                row["LeavesToBeEncashed"] = leavesToBeEncashed;

                //decimal earningAmount = 0;
                //decimal.TryParse(txtEarningAmount.Text.Trim(), out earningAmount);
                //row["EarningAmount"] = earningAmount;

                decimal remainingBalance = 0;
                decimal.TryParse(txtRemainingBalance.Text.Trim(), out remainingBalance);
                row["RemainingBalance"] = remainingBalance;

                //DateTime lastEncashmentDate;
                //DateTime.TryParse(txtLastEncashmentDate.Text, out lastEncashmentDate);
                //row["LastEncashmentDate"] = lastEncashmentDate;

                //decimal lastEncashmentLeaves = 0;
                //decimal.TryParse(txtLastEncashmentLeaves.Text.Trim(), out lastEncashmentLeaves);
                //row["LastEncashmentLeaves"] = lastEncashmentLeaves;

                dt.Rows.Add(row);

                PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();
                createResult = encashment.create(dt);

                if (createResult != null && createResult.isSuccess)
                {
                    NotificationMessage.showMessage(createResult);


                    string script = @"
        setTimeout(function() { 
            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                window.parent.refreshParentGrid();
            }
            closeDialog(); 
        }, 3000);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                }

                NotificationMessage.showMessage(createResult);

            }
            catch (Exception ex)
            {
                createResult.isSuccess = false;
                createResult.Message = "An error occurred: " + ex.Message;
                createResult.AlertType = AlertType.Error.ToString();
                NotificationMessage.showMessage(createResult);
            }
        }
    }
}