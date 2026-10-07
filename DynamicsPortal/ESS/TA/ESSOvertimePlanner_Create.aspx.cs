using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TASOvertimePlannerSvcReference;
using System;
using System.Data;
using System.Web.UI;

namespace DynamicsPortal.ESS.TA
{
    public partial class ESSOvertimePlanner_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "ESSOvertimePlanner_Create";
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Create Employee Overtime Planner";
                //  titleDiv.Style["font-weight"] = "bold";
            }

            if (!IsPostBack)
            {
                
                LoadEmployeeData();
            }
        }

        /// <summary>
        /// Load employee information from session
        /// </summary>
        private void LoadEmployeeData()
        {
            txtEmployeeId.Text = SessionVariables.getCurrentEmployeeId();
            txtEmployeeName.Text = SessionVariables.getCurrentEmployeeName();

            // Request Date = Today
            txtRequestDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

            // Readonly fields
            txtEmployeeId.ReadOnly = true;
            txtEmployeeName.ReadOnly = true;
            txtRequestDate.ReadOnly = true;
        }

        /// <summary>
        /// Save overtime planner
        /// 
        /// 
        /// 
        /// </summary>
        //protected void btnsave_click(object sender, eventargs e)
        //{

        //    try
        //    {
        //        overtimecontract contract = new overtimecontract();

        //        // employee id
        //        contract.employeeid = txtemployeeid.text;

        //        // plan date
        //        if (!string.isnullorempty(txtplandate.text))
        //        {
        //            contract.plandate = convert.todatetime(txtplandate.text);
        //        }

        //        // start time → convert to seconds
        //        if (!string.isnullorempty(txtstarttime.text))
        //        {
        //            timespan starttime = timespan.parse(txtstarttime.text);
        //            contract.starttime = (int)starttime.totalseconds;
        //        }

        //        // end time → convert to seconds
        //        if (!string.isnullorempty(txtendtime.text))
        //        {
        //            timespan endtime = timespan.parse(txtendtime.text);
        //            contract.endtime = (int)endtime.totalseconds;
        //        }

        //        // call service
        //        tasovertimeplannersvc service = new tasovertimeplannersvc();

        //        datatable dt = service.create(contract);

        //        if (dt != null && dt.rows.count > 0)
        //        {
        //            string message = dt.rows[0]["message"].tostring();

        //            // show message safely (replace if your framework differs)
        //            //scriptmanager.registerstartupscript(
        //            //    this,
        //            //    this.gettype(),
        //            //    "msg",
        //            //    "alert('" + message.replace("'", "") + "');",
        //            //    true
        //            //);
        //            notificationmessage.showmessage(message);
        //            string script = @"
        //settimeout(function() { 
        //    if (window.parent && typeof window.parent.refreshparentgrid === 'function') {
        //        window.parent.refreshparentgrid();
        //    }
        //    if (window.parent) {
        //        window.parent.closedialog();
        //    }
        //}, 1500);";
        //            scriptmanager.registerstartupscript(this, this.gettype(), "closemodal", script, true);


        //            // clear fields after save
        //            txtplandate.text = "";
        //            txtstarttime.text = "";
        //            txtendtime.text = "";
        //        }
        //    }
        //    catch (exception ex)
        //    {
        //        scriptmanager.registerstartupscript(
        //            this,
        //            this.gettype(),
        //            "err",
        //            "alert('" + ex.message.replace("'", "") + "');",
        //            true
        //        );
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // ========== VALIDATION ==========
                if (string.IsNullOrWhiteSpace(txtPlanDate.Text))
                {
                    NotificationMessage.showMessage("Please select Plan Date.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtStartTime.Text))
                {
                    NotificationMessage.showMessage("Please enter Start Time.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtEndTime.Text))
                {
                    NotificationMessage.showMessage("Please enter End Time.");
                    return;
                }

                // ========== BUILD CONTRACT ==========
                OvertimeContract contract = new OvertimeContract();
                contract.employeeId = txtEmployeeId.Text.Trim();
                contract.planDate = Convert.ToDateTime(txtPlanDate.Text);
                if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime))
                {
                    NotificationMessage.showMessage("Invalid Start Time. Use HH:mm or HH:mm:ss");
                    return;
                }
                contract.startTime = (int)startTime.TotalSeconds;

                // End Time - with proper validation
                if (!TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
                {
                    NotificationMessage.showMessage("Invalid End Time. Use HH:mm or HH:mm:ss");
                    return;
                }
                contract.endTime = (int)endTime.TotalSeconds;

                if (contract.endTime <= contract.startTime)
                {
                    NotificationMessage.showMessage("End Time must be greater than Start Time.");
                    return;
                }

                // ========== CALL SERVICE ==========
                TASOvertimePlannersSvc service = new TASOvertimePlannersSvc();
                DataTable dt = service.create(contract);

                string message = "Record created successfully.";

                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    message = dt.Rows[0][0]?.ToString() ?? message;
                }

                // ========== CREATE RESULT OBJECT ==========
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = true;
                result.AlertType = AlertType.Success.ToString();
                result.Message = message;

                // Show notification properly
                NotificationMessage.showMessage(result);

                // Close + Refresh after short delay
                string script = @"
            setTimeout(function() {
                if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                    window.parent.refreshParentGrid();
                }
                closeDialog();
            }, 1500);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseAndRefresh", script, true);
            }
            catch (Exception ex)
            {
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                errorResult.isSuccess = false;
                errorResult.AlertType = AlertType.Error.ToString();
                errorResult.Message = "Error: " + ex.Message;

                NotificationMessage.showMessage(errorResult);
            }
        }


    }
}