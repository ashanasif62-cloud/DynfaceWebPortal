using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSEmploymentCertificateSvcReference;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRRequestForLetter_Create : ModalForm
    {
        private HRRequestForLetters hRRequestForLetters = new HRRequestForLetters();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHREmployeeLettersRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindControlsData();
                    BindCertificateType();
                    BindRequestedFor();
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
            //ddlCertificateType.DataSource = Enum.GetNames(typeof(ESSEmploymentCertificateType));
            //ddlCertificateType.DataBind();

            DataTable dtRequestedFor = ControlsHelper.retrieveAllESSRequestedFor();
            ddlRequestedFor.DataSource = dtRequestedFor;
            ddlRequestedFor.DataTextField = "RequestedForId";
            ddlRequestedFor.DataValueField = "RequestedForId";
            ddlRequestedFor.DataBind();

            DataTable dtReasonCodes = ControlsHelper.retrieveAllHcmReasonCode();
            //ddlReason.DataSource = dtReasonCodes;
            //ddlReason.DataTextField = "Description";
            //ddlReason.DataValueField = "ReasonCodeId";
            //ddlReason.DataBind();



            txtRequestedDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestedDate.Enabled = false;

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

        private bool BindCertificateType(string selectedValue = "")
        {
            HRRequestForLetters letters = new HRRequestForLetters();
            DataTable dt = letters.retrieveCertificateType();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCertificateType.DataSource = dt;
                ddlCertificateType.DataTextField = "CertificateTypeCode";   // what user sees
                ddlCertificateType.DataValueField = "CertificateTypeCode";  // underlying value
                ddlCertificateType.DataBind();

                // Insert empty option at the top
                ddlCertificateType.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlCertificateType.Items.FindByValue(selectedValue) != null)
            {
                ddlCertificateType.SelectedValue = selectedValue;
            }
            else
            {
                ddlCertificateType.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindRequestedFor(string selectedValue = "")
        {
            HRRequestForLetters letters = new HRRequestForLetters();
            DataTable dt = letters.retrieveRequestedFor();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlRequestedFor.DataSource = dt;
                ddlRequestedFor.DataTextField = "RequestedForId";   // what user sees
                ddlRequestedFor.DataValueField = "RequestedForId";  // underlying value
                ddlRequestedFor.DataBind();

                // Insert empty option at the top
                ddlRequestedFor.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlRequestedFor.Items.FindByValue(selectedValue) != null)
            {
                ddlRequestedFor.SelectedValue = selectedValue;
            }
            else
            {
                ddlRequestedFor.SelectedIndex = 0; // keep it empty
            }

            return true;
        }


        private void create_SubmitRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            string remarksField = txtRemarks.Text;
            if (string.IsNullOrEmpty(remarksField.Trim()))
            {
                SysOperationResult_BOL err = new SysOperationResult_BOL();
                err.AlertType = AlertType.Error.ToString();
                err.Message = "Remark field must be filled in";
                NotificationMessage.showMessage(err);
                return;
            }

            #region CreateRequest
            DataTable dataTable = hRRequestForLetters.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmpId"] = employeeId;
            //dr["Authentication"] = txtAuthentication.Text;
            dr["RequestedDate"] = txtRequestedDate.Text;
            dr["Remarks"] = txtRemarks.Text;

            //dr["Reason"] = ddlReason.SelectedValue;
            dr["CertificateTypeCode"] = ddlCertificateType.SelectedValue;
            dr["ReqestedFor"] = ddlRequestedFor.SelectedValue;

            dataTable.Rows.Add(dr);
            #endregion

            createResult = hRRequestForLetters.create(dataTable);
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
                operationResult_BOL = eSSWorkflow.hREmploymentCertificateRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            if (!string.IsNullOrEmpty(personalNumber))
            {
                HcmWorkerDetailsSvcContract getWorkerDetails = ControlsHelper.getWorkerDetails(personalNumber);
                //txtJobId.Text = getWorkerDetails.Job;
                //txtDepartment.Text = getWorkerDetails.DepartmentName;
            }
        }

    }
}
