using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeavesSvcReference;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLeave_Create : ModalForm
    {
        private PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();

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

            DataTable leaveCode = ControlsHelper.retrieveAllPRLeaveCodes();
            ddlLeaveCode.DataSource = leaveCode;
            ddlLeaveCode.DataTextField = "LeaveCode";
            ddlLeaveCode.DataValueField = "LeaveCode";
            ddlLeaveCode.DataBind();

            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestDate.Enabled = false;
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
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmployeeId"] = employeeId;
            dr["LeaveCategory"] = ddlLeaveCategory.SelectedValue;
            dr["LeaveCode"] = ddlLeaveCode.SelectedValue;
            dr["LeaveDays"] = txtLeaveDays.Text;
            dr["LeaveReqDate"] = txtRequestDate.Text;
            dr["LeaveStartDate"] = txtLeaveStartDate.Text;
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

            bool result = operationResults(createResult, true);
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
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            setEmployeeLeaveBalance();
        }

        private void setEmployeeLeaveBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string leaveCode = ddlLeaveCode.SelectedValue;
            string leaveBalance = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                leaveBalance = ControlsHelper.getEmployeeLeaveBalance(employeeId, leaveCode).ToString();
            }

            txtBalance.Text = leaveBalance;
        }

    }
}
