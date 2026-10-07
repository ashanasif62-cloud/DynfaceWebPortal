using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeEOSRequestsSvcReference;

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeEOS_Create : ModalForm
    {
        private PREmployeeEOS pREmployeeEOS = new PREmployeeEOS();
        private PRDropDown pRDropDown = new PRDropDown();


        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeEOSRequest";

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Resignation Notice";
                    titleDiv.Style["font-weight"] = "600";
                    titleDiv.Style["font-size"] = "16px";
                    titleDiv.Style["color"] = "#000000";
                    titleDiv.Style["margin"] = "10px 0";
                }


                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindControlsData();
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
            ddlEOSType.DataSource = Enum.GetNames(typeof(PREOSType));
            ddlEOSType.DataBind();

            DataTable noticePeriodCode = ControlsHelper.retrieveAllPREOSNoticePeriods();

            DataView dv = new DataView(noticePeriodCode);
            dv.RowFilter = "EOSType = 'Resignation'";

            ddlNoticePeriodCode.DataSource = dv;
            ddlNoticePeriodCode.DataTextField = "NoticePeriodCode";
            ddlNoticePeriodCode.DataValueField = "NoticePeriodCode";
            ddlNoticePeriodCode.DataBind();
            ddlNoticePeriodCode.Items.Insert(0, new ListItem("-- Select --", ""));

            //   DataTable reasonCode = pRDropDown.retrieveReasonCode();
            DataTable reasonCode = ControlsHelper.retrieveAllHcmReasonCode();
            ddlReasonCode.DataSource = reasonCode;
            ddlReasonCode.DataTextField = "Description";
            ddlReasonCode.DataValueField = "ReasonCodeId";
            ddlReasonCode.DataBind();

            //    txtLastWorkingDateRequested.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"); // DateTime.Now.ToString("dd/MM/yyyy");
            txtNotificationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtNotificationDate.Enabled = false;
            //txtLastWorkingDatePlanned.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
            //txtLastWorkingDatePlanned.Enabled = false;
            DateTime today = DateTime.Now;
            DateTime lastWorkingDate = today.AddDays(30);

            txtLastWorkingDatePlanned.Text = lastWorkingDate.ToString("yyyy-MM-dd");
            txtLastWorkingDateRequested.Text = lastWorkingDate.ToString("yyyy-MM-dd");
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

        private bool isRecordExists(long employeeId)
        {
            DataTable dt = pREmployeeEOS.retriveEmployeeReportees();

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt64(row["EmployeeId"]) == employeeId )
                    
                {
                    return true;
                }
            }
            return false;
        }
        private void create_SubmitRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            #region CreateRequest
            DataTable dataTable = pREmployeeEOS.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);
            if (isRecordExists(employeeId))
            {
                createResult.isSuccess = false;
                createResult.AlertType = AlertType.Error.ToString();
                createResult.Message = "Record already exists for this employee.";
                operationResults(createResult);
                return;
            }


            dr["EmployeeId"] = employeeId;
            dr["EOSNotificationDate"] = txtNotificationDate.Text;
            dr["LastWorkingDate_Actual"] = txtLastWorkingDateRequested.Text;
            dr["LastWorkingDate_Calculated"] = txtLastWorkingDatePlanned.Text;
            dr["Remarks"] = txtRemarks.Text;

            dr["EOSType"] = ddlEOSType.SelectedValue;
            dr["NoticePeriodCode"] = ddlNoticePeriodCode.SelectedValue;
            dr["EOSReasonCode"] = ddlReasonCode.SelectedValue;

            dataTable.Rows.Add(dr);

            #endregion

            DateTime notificationDate;
            DateTime lastWorkingDateRequested;

            if (!DateTime.TryParse(txtNotificationDate.Text, out notificationDate) ||
                !DateTime.TryParse(txtLastWorkingDateRequested.Text, out lastWorkingDateRequested))
            {
                createResult.isSuccess = false;
                createResult.AlertType = AlertType.Error.ToString();
                createResult.Message = "Invalid date format.";
                operationResults(createResult);
                return;
            }

            // 🔴 VALIDATION: Last Working Date < Notification Date
            if (lastWorkingDateRequested < notificationDate)
            {
                createResult.isSuccess = false;
                createResult.AlertType = AlertType.Error.ToString();
                createResult.Message = "Last Working Date Requested cannot be less than Notification Date.";
                operationResults(createResult);
                return;
            }

            createResult = pREmployeeEOS.create(dataTable);
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
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.pREmployeeEOSRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;


            if (!string.IsNullOrEmpty(personalNumber))
            {
                txtActivePayPeriod.Text = ControlsHelper.getEmployeeActivePayPeriod(personalNumber);
                DateTime hiringDate = ControlsHelper.getEmployeeJoiningDate(personalNumber);

                txtHiringDate.Text = hiringDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //                txtServiceDuration.Text = ControlsHelper.getEmployeeServiceDuration(personalNumber);
                UpdateServiceDuration(hiringDate);
            }
        }

        protected void onNoticePeriodModified(object sender, EventArgs e)
        {
            string noticePeriodCode = ddlNoticePeriodCode.SelectedValue;

            DateTime notificationDate = txtNotificationDate.Text.toDateTime();

            DataTable dt = pREmployeeEOS.onNotificePeriodCodeModified(noticePeriodCode, notificationDate);

            txtLastWorkingDatePlanned.Text =
    dt.Rows[0]["LastWorkingDate_Calculated"].ToString().toDateTime()
    .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            txtLastWorkingDateRequested.Text =
                dt.Rows[0]["LastWorkingDate_Actual"].ToString().toDateTime()
                .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            DateTime hiringDate = ControlsHelper.getEmployeeJoiningDate(personalNumber);
            UpdateServiceDuration(hiringDate);
        }

        private void UpdateServiceDuration(DateTime _hiringDate)
        {
            if (!DateTime.TryParse(txtLastWorkingDateRequested.Text, out DateTime currentDate))
            {
                MessageBox.Show("Invalid date format.");
                return;
            }

            DateTime startDate = _hiringDate;
            DateTime endDate = currentDate;

            // Mirror X++: if (endDate < startDate) return ""
            if (endDate < startDate)
            {
                txtServiceDuration.Text = "";
                return;
            }

            int years = endDate.Year - startDate.Year;
            int months = endDate.Month - startDate.Month;
            int days = endDate.Day - startDate.Day;

            // Adjust days — mirrors X++ prevMth(endDate) + dayOfMth(endMth(...))
            if (days < 0)
            {
                months--;
                DateTime prevMonthDate = endDate.AddMonths(-1);
                days += DateTime.DaysInMonth(prevMonthDate.Year, prevMonthDate.Month);
            }

            // Adjust months
            if (months < 0)
            {
                years--;
                months += 12;
            }

            txtServiceDuration.Text = $"{years} Years, {months} Months, {days} Days";
        }

        protected void txtLastWorkingDateRequested_TextChanged(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            DateTime hiringDate = ControlsHelper.getEmployeeJoiningDate(personalNumber);
            UpdateServiceDuration(hiringDate);
        }
    }
}
