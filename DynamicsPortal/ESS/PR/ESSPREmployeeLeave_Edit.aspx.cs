using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeavesSvcReference;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLeave_Edit : ModalForm
    {
        private PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();
        private PRDropDown pRDropDown = new PRDropDown();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeLeaveRequest";

                base.Page_Load(sender, e);

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Edit Employee Leave Request";
                    //titleDiv.Style["font-weight"] = "600";   // semi-bold
                    //titleDiv.Style["font-size"] = "20px";    // slightly larger
                    //titleDiv.Style["color"] = "#000000";     // solid black
                    //titleDiv.Style["margin"] = "10px 0";     // spacing around
                }


                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindControlsData();
                    setEmployeeLeaveDays();
                    setEmployeeLeaveBalance();
                    setEmployeeQuota();
                    CalculateEligibility();


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

        private void bindControlsData()
        {
            ddlLeaveCategory.DataSource = Enum.GetNames(typeof(PRLeaveCategory));
            ddlLeaveCategory.DataBind();

            //  DataTable leaveCode = pRDropDown.retrieveLeaveCode();
            DataTable leaveCode = ControlsHelper.retrieveAllPRLeaveCodes();
            ddlLeaveCode.DataSource = leaveCode;
            ddlLeaveCode.DataTextField = "LeaveCode";
            ddlLeaveCode.DataValueField = "LeaveCode";
            ddlLeaveCode.DataBind();

            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestDate.Enabled = false;

            txtNewLeaveDays.Text = Convert.ToDecimal(Session["LeaveDays"] ?? 0).ToString("0.00");
            //txtRequestDate.Text = Session["LeaveReqDate"]?.ToString() ?? string.Empty;
            txtRequestDate.Text =
    Session["LeaveReqDate"] == null
        ? string.Empty
        : Convert.ToDateTime(Session["LeaveReqDate"]).ToString("M/dd/yyyy");
            txtLeaveStartDate.Text = Session["LeaveStartDate"]?.ToString() ?? string.Empty;
            txtLeaveEndDate.Text = Session["LeaveEndDate"]?.ToString() ?? string.Empty;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            updateRequest();
        }


        private void updateRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            SysOperationResult_BOL updateResult = new SysOperationResult_BOL();
          
            long RecId = 0;
            if (Session["RecId"] != null)
            {
                long.TryParse(Session["RecId"].ToString(), out RecId);
            }

            DateTime leaveReqDate = DateTime.MinValue;

            if (Session["LeaveReqDate"] != null)
            {
                DateTime.TryParse(Session["LeaveReqDate"].ToString(), out leaveReqDate);
            }


            #region EditRequest
            DataTable dataTable = pREmployeeLeaveRequest.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;


            dr["LeaveReqId"] = Session["LeaveReqId"] as string;
            dr["LeaveReqDate"] = leaveReqDate;
            dr["EmployeeId"] = personalNumber;
            dr["EmployeeName"] = SessionVariables.getCurrentEmployeeName();
            dr["Eligibility"] = txtEligibility.Text;
            dr["LeaveCategory"] = ddlLeaveCategory.SelectedValue;
            dr["LeaveDays"] = txtNewLeaveDays.Text;
            dr["LeaveStartDate"] = txtLeaveStartDate.Text;
            dr["LeaveEndDate"] = txtLeaveEndDate.Text;
            dr["LeaveCode"] = ddlLeaveCode.SelectedValue;
            dr["Reason"] = txtReason.Text;
            dr["Quota"] = txtQuota.Text;
            dr["RecId"] = RecId;

            dataTable.Rows.Add(dr);
            #endregion

            updateResult = pREmployeeLeaveRequest.update(dataTable, RecId);
            requestRecId = updateResult.RecId;

            bool result = operationResults(updateResult);

            if (updateResult != null && updateResult.isSuccess)
            {
                //NotificationMessage.showMessage(createResult);

                string script = @"setTimeout(function() { 
                    if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }
                    closeDialog(); 
                }, 3000);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);

              
                return; 
            }
        }

        protected void ddlLeaveCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeeLeaveBalance();
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            setEmployeeLeaveBalance();
        }

        private void setEmployeeLeaveBalance()
        {
            string employeeId = Session["EmployeeId"] as string;
            string leaveCode = ddlLeaveCode.SelectedValue;
            string leaveBalance = string.Empty;
            string leaveStartDateText = txtLeaveStartDate.Text;
            string leaveReqId = Session["LeaveReqId"] as string;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveBalance = ControlsHelper.getEmployeeLeaveBalance(employeeId, leaveCode, leaveStartDateText, leaveReqId).ToString();
            }

            double balanceValue;
            if (!double.TryParse(leaveBalance, out balanceValue))
            {
                balanceValue = 0; // default to 0 if parse fails
            }

            txtBalance.Text = balanceValue.ToString("0.00"); // display as 0.00 or real value
        }

        private void setEmployeeLeaveDays()
        {
            string employeeId = Session["EmployeeId"] as string;
           long workerRecId = ControlsHelper.getWorkerId(employeeId);
            string leaveCode = ddlLeaveCode.SelectedValue;
            DateTime leaveStartDate = DateTime.Parse(txtLeaveStartDate.Text);
            DateTime leaveEndDate = DateTime.Parse(txtLeaveEndDate.Text);
            string leaveDays = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveDays = ControlsHelper.getEmployeeLeaveDays(workerRecId, leaveCode, leaveStartDate, leaveEndDate).ToString();
              //  leaveDays = ControlsHelper.getEmployeeLeaveDays(employeeId, leaveCode, leaveStartDate, leaveEndDate).ToString();
            }

            double leaveDaysValue;
            if (!double.TryParse(leaveDays, out leaveDaysValue))
            {
                leaveDaysValue = 0; // default to 0 if parse fails
            }

            txtNewLeaveDays.Text = leaveDaysValue.ToString("0.00");
        }
        private void setEmployeeQuota()
        {
            string employeeId = Session["EmployeeId"] as string;
            long workerRecId = ControlsHelper.getWorkerId(employeeId);
            string leaveCode = ddlLeaveCode.SelectedValue;
            DateTime leaveStartDate = DateTime.Parse(txtLeaveStartDate.Text);
            string leaveDays = string.Empty;
            long RecId = 0;
            if (Session["RecId"] != null)
            {
                long.TryParse(Session["RecId"].ToString(), out RecId);
            }

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveDays = ControlsHelper.getEmployeeQuota(employeeId, leaveCode, leaveStartDate, RecId).ToString();
            }

            double quotaValue;
            if (!double.TryParse(leaveDays, out quotaValue))
            {
                quotaValue = 0; // default to 0 if parse fails
            }

            txtQuota.Text = quotaValue.ToString("0.00");
        }

        protected void txtleaveEndDate_modified(object sender, EventArgs e)
        {
            setEmployeeLeaveDays();
           
        }
        //protected void txtLeaveStartDate_modified(object sender, EventArgs e)
        //{
        //    setEmployeeQuota();
        //}

        protected void CalculateEligibility()
        {
         
            decimal balance = 0;
            decimal quota = 0;

          
            
            decimal.TryParse(txtBalance.Text, out balance);
            
            decimal.TryParse(txtQuota.Text, out quota);
            

           
            txtEligibility.Text = Math.Max(balance, quota).ToString("0.00"); 
        }
    }
}
