using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeavesSvcReference;

using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLeave_Create : ModalForm
    {
        private PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();
        private PRDropDown pRDropDown = new PRDropDown();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeLeaveRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindControlsData();
                    setDefaultNumericValues();
                    BindLeaveCategory();
                    SetShortLeaveTimeVisibility(false);
                }

                // Apply End Date readonly state on every load
                ApplyEndDateReadOnlyState();
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

        private void ApplyEndDateReadOnlyState()
        {
            bool isShortOrHalf = IsShortOrHalfLeaveCategory();

            if (isShortOrHalf)
            {
                // Short Leave or Half Leave - End Date is readonly
                txtLeaveEndDate.ReadOnly = true;
                txtLeaveEndDate.Attributes["readonly"] = "readonly";
                txtLeaveEndDate.Style["background-color"] = "#e9ecef";
                txtLeaveEndDate.Style["cursor"] = "not-allowed";
                txtLeaveEndDate.Style["opacity"] = "1";
                txtLeaveEndDate.Style["color"] = "#495057";

                // Set end date equal to start date
                if (!string.IsNullOrEmpty(txtLeaveStartDate.Text))
                {
                    txtLeaveEndDate.Text = txtLeaveStartDate.Text;
                }
            }
            else
            {
                // Full Day Leave - End Date is editable
                txtLeaveEndDate.ReadOnly = false;
                txtLeaveEndDate.Attributes.Remove("readonly");
                txtLeaveEndDate.Style["background-color"] = "";
                txtLeaveEndDate.Style["cursor"] = "";
                txtLeaveEndDate.Style["opacity"] = "";
                txtLeaveEndDate.Style["color"] = "";
            }
        }

        private bool IsShortOrHalfLeaveCategory()
        {
            if (ddlLeaveCategory.SelectedItem == null)
                return false;

            string selectedValue = (ddlLeaveCategory.SelectedValue ?? "").ToLower();
            string selectedText = (ddlLeaveCategory.SelectedItem.Text ?? "").ToLower();

            // Check for Short Leave
            bool isShortLeave = selectedValue.Contains("shortleave") ||
                                selectedText.Contains("short leave") ||
                                selectedText.Contains("shortleave");

            // Check for Half Leave
            bool isHalfLeave = selectedValue.Contains("halfleave") ||
                               selectedText.Contains("half leave") ||
                               selectedText.Contains("halfleave") ||
                               selectedText.Contains("half day") ||
                               selectedText.Contains("halfday");

            return isShortLeave || isHalfLeave;
        }

        private void SetShortLeaveTimeVisibility(bool isShortLeave)
        {
            trLeaveStartTime.Visible = isShortLeave;
            trLeaveEndTime.Visible = isShortLeave;

            if (!isShortLeave)
            {
                txtLeaveStartTime.Text = "";
                txtLeaveEndTime.Text = "";
            }
        }

        private void bindControlsData()
        {
            //ddlLeaveCategory.DataSource = Enum.GetNames(typeof(PRLeaveCategory));
            //ddlLeaveCategory.DataBind();

            //  DataTable leaveCode = pRDropDown.retrieveLeaveCode();
            DataTable leaveCode = ControlsHelper.retrieveAllPRLeaveCodes();
            ddlLeaveCode.DataSource = leaveCode;
            ddlLeaveCode.DataTextField = "LeaveCode";
            ddlLeaveCode.DataValueField = "LeaveCode";
            ddlLeaveCode.DataBind();

            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestDate.Enabled = false;
            txtLeaveStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Submit_Click(object sender, EventArgs e)
        {
            create_SubmitRequest();
        }

        private void createRequest()
        {
            create_SubmitRequest(false);
        }

        //protected void txtLeaveDays_TextChanged(object sender, EventArgs e)
        //{
        //    if (int.TryParse(txtLeaveDays.Text, out int leaveDays) && leaveDays > 0)
        //    {
        //        DateTime startDate;

        //        if (DateTime.TryParse(txtLeaveStartDate.Text, out startDate))
        //        {
        //            DateTime endDate = startDate.AddDays(leaveDays - 1);

        //            txtLeaveEndDate.Text = endDate.ToString("yyyy-MM-dd");
        //        }
        //        else
        //        {
        //            txtLeaveEndDate.Text = ""; // invalid start date
        //        }
        //    }
        //    else
        //    {
        //        txtLeaveEndDate.Text = ""; // invalid or empty leave days
        //    }
        //}



        private void BindLeaveCategory()
        {
            try
            {
                PREmployeeLeaveRequest dynafaceReport = new PREmployeeLeaveRequest();
                DataTable dt = dynafaceReport.retrieveLeaveCategory();
                ddlLeaveCategory.Items.Clear();
                ddlLeaveCategory.Items.Insert(0, new ListItem("", ""));
                if (dt == null) return;
                foreach (DataRow row in dt.Rows)
                {
                    string detailSummaryValue = row["LeaveCategory"]?.ToString() ?? "";
                    string detailSummaryLabel = row["LeaveCategoryLabel"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(detailSummaryValue)) continue;
                    string text = string.IsNullOrEmpty(detailSummaryLabel)
                        ? detailSummaryValue
                        :  detailSummaryLabel;
                    ddlLeaveCategory.Items.Add(new ListItem(text, detailSummaryValue));
                }
            }
            catch (Exception ex) { LogError(ex); }
        }

        //protected void ddlLeaveCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Change "2" to the real value that comes from your LeaveCategory table for Short Leave
        //    bool isShortLeave = ddlLeaveCategory.SelectedValue == "2";

        //    // Show / hide the whole rows
        //    SetShortLeaveTimeVisibility(isShortLeave);

        //    // Clear times when switching away from Short Leave
        //    if (!isShortLeave)
        //    {
        //        txtLeaveStartTime.Text = "";
        //        txtLeaveEndTime.Text = "";
        //    }
        //    ApplyLeaveCategoryLogicAndUpdateUI();



        //}

        protected void ddlLeaveCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShortLeave = IsShortLeaveCategory();

            // Show/hide the time rows
            SetShortLeaveTimeVisibility(isShortLeave);

            // Clear times when not short leave
            if (!isShortLeave)
            {
                txtLeaveStartTime.Text = "";
                txtLeaveEndTime.Text = "";
            }

            // Apply End Date readonly state
            ApplyEndDateReadOnlyState();

            ApplyLeaveCategoryLogicAndUpdateUI();
        }

        private bool IsShortLeaveCategory()
        {
            if (ddlLeaveCategory.SelectedItem == null)
                return false;

            string selectedValue = ddlLeaveCategory.SelectedValue;
            string selectedText = ddlLeaveCategory.SelectedItem.Text;

            // Check for "ShortLeave" in either value or text (case-insensitive)
            return selectedValue.Equals("ShortLeave", StringComparison.OrdinalIgnoreCase) ||
                   selectedText.ToLower().Contains("shortleave") ||
                   selectedText.ToLower().Contains("short leave");
        }



        private void LogError(Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            objErrorLog.write(currentMethod.DeclaringType.FullName, ex);
        }


        private void create_SubmitRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();


            #region CreateRequest
            DataTable dataTable = pREmployeeLeaveRequest.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            //long employeeId = ControlsHelper.getWorkerId(personalNumber);


            dr["EmployeeId"] = personalNumber;
            dr["EmployeeName"] = SessionVariables.getCurrentEmployeeName();
            //dr["LeaveCategory"] = ddlLeaveCategory.SelectedValue;
            dr["LeaveCode"] = ddlLeaveCode.SelectedValue;
            dr["LeaveDays"] = txtNewLeaveDays.Text;
            dr["Balance"] = txtBalance.Text;
            dr["LeaveReqDate"] = txtRequestDate.Text;
            dr["LeaveStartDate"] = txtLeaveStartDate.Text;
            dr["LeaveEndDate"] = txtLeaveEndDate.Text;
            dr["Quota"] = txtQuota.Text;
            dr["Eligibility"] = txtEligibility.Text;
            dr["Reason"] = txtReason.Text;

            dataTable.Rows.Add(dr);
            #endregion

            createResult = pREmployeeLeaveRequest.create(dataTable);
            requestRecId = createResult.RecId;

            if (createResult.isSuccess && submitRequest)
            {
                if (requestRecId > 0)
                {
                    submitResult = submitWFRequest(requestRecId);
                    if (submitResult.isSuccess)
                    {
                        submitResult.Message = " Request successfully submitted.";
                    }
                    else
                    {
                        submitResult.Message = " Failed to submit the request.";
                    }
                }
                else
                {
                    submitResult.AlertType = AlertType.Error.ToString();
                    submitResult.isSuccess = false;
                    submitResult.Message = " Failed to submit the created request.";
                }
                createResult.Message += " " + submitResult.Message;
                createResult.AlertType = submitResult.AlertType;
                createResult.isSuccess = submitResult.isSuccess;
            }

            bool result = operationResults(createResult);

            if (createResult != null && createResult.isSuccess)
            {
                //NotificationMessage.showMessage(createResult);

                string script = @"setTimeout(function() { 
                    if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }
                    closeDialog(); 
                }, 3000);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);

                // ⭐⭐⭐ ADD THIS RETURN STATEMENT ⭐⭐⭐
                return; // Exit the method here!
            }
            else
            {
                NotificationMessage.showMessage(createResult);
            }

        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.pREmployeeLeaveRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void ddlLeaveCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeeLeaveBalance();
            setEmployeeQuota();
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            setEmployeeQuota();
            setEmployeeLeaveBalance();
        }

        private void setEmployeeLeaveBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string leaveCode = ddlLeaveCode.SelectedValue;
            string leaveBalance = string.Empty;
            string leaveStartDateText = txtLeaveStartDate.Text;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveBalance = ControlsHelper.getEmployeeLeaveBalance(employeeId, leaveCode, leaveStartDateText).ToString();
            }

            double balanceValue;
            if (!double.TryParse(leaveBalance, out balanceValue))
            {
                balanceValue = 0; // default to 0 if parse fails
            }

            txtBalance.Text = balanceValue.ToString("0.00"); // display as 0.00 or real value
            //Compare values of txtBalance and txtQuota and update txtEligibility text to which is higher
            CalculateEligibility();

        }

        private void setEmployeeLeaveDays()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string leaveCode = ddlLeaveCode.SelectedValue;

            DateTime leaveStartDate, leaveEndDate;

            if (!DateTime.TryParse(txtLeaveStartDate.Text, out leaveStartDate) ||
                !DateTime.TryParse(txtLeaveEndDate.Text, out leaveEndDate))
            {
                txtNewLeaveDays.Text = "0.00";
                return;
            }

            double leaveDaysValue = 0;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                long workerRecId = ControlsHelper.getWorkerId(employeeId); // ✅ moved inside
                                                                           // string workerRecId = SessionVariables.getCurrentEmployeeId();// ✅ moved inside

                double.TryParse(
                    ControlsHelper.getEmployeeLeaveDays(workerRecId, leaveCode, leaveStartDate, leaveEndDate).ToString(),
                    out leaveDaysValue
                );
            }

            txtNewLeaveDays.Text = leaveDaysValue.ToString("0.00");
        }
        private void setEmployeeQuota()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long workerRecId = ControlsHelper.getWorkerId(employeeId);
            string leaveCode = ddlLeaveCode.SelectedValue;
            DateTime leaveStartDate = DateTime.Parse(txtLeaveStartDate.Text);
            string leaveDays = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveDays = ControlsHelper.getEmployeeQuota(employeeId, leaveCode, leaveStartDate).ToString();
            }

            double quotaValue;
            if (!double.TryParse(leaveDays, out quotaValue))
            {
                quotaValue = 0; // default to 0 if parse fails
            }

            txtQuota.Text = quotaValue.ToString("0.00");
            txtEligibility.Text = quotaValue.ToString("0.00");
        }

        protected void txtleaveEndDate_modified(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtLeaveStartDate.Text, out DateTime startDate) &&
               DateTime.TryParse(txtLeaveEndDate.Text, out DateTime endDate) &&
               endDate < startDate)
            {
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Leave End Date cannot be less than Leave Start Date.";
                operationResults(result);
                txtLeaveEndDate.Text = string.Empty;
                return;
            }
            setEmployeeLeaveDays();
            CalculateEligibility();
        }
        protected void txtLeaveStartDate_modified(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtLeaveStartDate.Text, out DateTime startDate) &&
                    DateTime.TryParse(txtLeaveEndDate.Text, out DateTime endDate) &&
                    endDate < startDate && !IsShortOrHalfLeaveCategory())
            {
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Leave End Date cannot be less than Leave Start Date.";
                operationResults(result);
                txtLeaveEndDate.Text = string.Empty;
                return;
            }

            // If End Date is readonly (Short/Half Leave), update it to match Start Date
            if (IsShortOrHalfLeaveCategory() && !string.IsNullOrEmpty(txtLeaveStartDate.Text))
            {
                txtLeaveEndDate.Text = txtLeaveStartDate.Text;
            }

            setEmployeeQuota();
            setEmployeeLeaveDays();
            CalculateEligibility();
        }

        private void setDefaultNumericValues()
        {
            txtNewLeaveDays.Text = "0.00";
            txtBalance.Text = "0.00";
            txtQuota.Text = "0.00";
            txtEligibility.Text = "0.00";
        }

        //protected void CalculateEligibility()
        //{

        //    decimal balance = 0;
        //    decimal quota = 0;



        //    decimal.TryParse(txtBalance.Text, out balance);

        //    decimal.TryParse(txtQuota.Text, out quota);



        //    txtEligibility.Text = Math.Max(balance, quota).ToString("0.00");
        //}

        private void ApplyLeaveCategoryLogicAndUpdateUI()
        {
            try
            {
                // Basic validation before calling service
                if (string.IsNullOrEmpty(ddlLeaveCode.SelectedValue) ||
                    string.IsNullOrEmpty(ddlLeaveCategory.SelectedValue) ||
                    string.IsNullOrEmpty(txtLeaveStartDate.Text))
                {
                    return;
                }

                // Build the contract
                PREmployeeLeavesSvcContract contract = new PREmployeeLeavesSvcContract();

                contract.EmployeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as TextBox)?.Text ?? "";
                contract.LeaveCode = ddlLeaveCode.SelectedValue;
                contract.LeaveCategory = (PRLeaveCategory)Enum.Parse(typeof(PRLeaveCategory), ddlLeaveCategory.SelectedValue);
                contract.LeaveStartDate = DateTime.Parse(txtLeaveStartDate.Text);

                // Optional – send current values if needed
                if (DateTime.TryParse(txtLeaveEndDate.Text, out DateTime endDate))
                    contract.LeaveEndDate = endDate;

                if (decimal.TryParse(txtNewLeaveDays.Text, out decimal days))
                    contract.LeaveDays = days;

                // Call your consumption method
                PREmployeeLeavesSvcContract result = pREmployeeLeaveRequest.applyLeaveCategoryLogic(contract);

                if (result != null)
                {
                    // Get values calculated by D365
                    txtLeaveEndDate.Text =
                        result.LeaveEndDate.ToString("yyyy-MM-dd");

                    txtNewLeaveDays.Text =
                        result.LeaveDays.ToString("0.00");

                    CalculateEligibility();
                }
                else
                {
                    SysOperationResult_BOL errorResult =
                        new SysOperationResult_BOL();

                    errorResult.isSuccess = false;
                    errorResult.AlertType =
                        AlertType.Error.ToString();

                    errorResult.Message =
                        "Failed to apply leave category logic.";

                    operationResults(errorResult);
                }

                // Re-apply end date readonly state after logic
                ApplyEndDateReadOnlyState();
            }
            catch (Exception ex)
            {
                LogError(ex);

                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Error applying leave category logic: " + ex.Message;
                operationResults(result);
            }
        }

        protected void CalculateEligibility()
        {
            decimal quota = 0;
            decimal balance = 0;

            decimal.TryParse(txtQuota.Text, out quota);
            decimal.TryParse(txtBalance.Text, out balance);

            txtEligibility.Text = quota > balance ? quota.ToString("0.00") : balance.ToString("0.00");
        }
    }
}