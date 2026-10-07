using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeEOSRequestsSvcReference;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeEOS_Create : ModalForm
    {
        private PREmployeeEOS pREmployeeEOS = new PREmployeeEOS();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeEOSRequest";

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
            ddlNoticePeriodCode.DataSource = noticePeriodCode;
            ddlNoticePeriodCode.DataTextField = "NoticePeriodCode";
            ddlNoticePeriodCode.DataValueField = "NoticePeriodCode";
            ddlNoticePeriodCode.DataBind();

            DataTable reasonCode = ControlsHelper.retrieveAllHcmReasonCode();
            ddlReasonCode.DataSource = reasonCode;
            ddlReasonCode.DataTextField = "Description";
            ddlReasonCode.DataValueField = "ReasonCodeId";
            ddlReasonCode.DataBind();

            //txtLastWorkingDateRequested.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNotificationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNotificationDate.Enabled = false;
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
            DataTable dataTable = pREmployeeEOS.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmployeeId"] = employeeId;
            dr["EOSNotificationDate"] = txtNotificationDate.Text;
            //dr["LastWorkingDate_Actual"] = txtLastWorkingDateRequested.Text;
            dr["Remarks"] = txtRemarks.Text;

            dr["EOSType"] = ddlEOSType.SelectedValue;
            dr["NoticePeriodCode"] = ddlNoticePeriodCode.SelectedValue;
            dr["EOSReasonCode"] = ddlReasonCode.SelectedValue;

            dataTable.Rows.Add(dr);

            #endregion

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

            bool result = operationResults(createResult, true);
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
                txtHiringDate.Text = ControlsHelper.getEmployeeJoiningDate(personalNumber);
                txtServiceDuration.Text = ControlsHelper.getEmployeeServiceDuration(personalNumber);
            }
        }
    }
}
