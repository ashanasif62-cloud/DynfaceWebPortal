using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class PREmployeePFWithDrawl_Create : ModalForm
    {
        private PREmployeePFWithDrawl pREmployeePFWithDrawl = new PREmployeePFWithDrawl();
        private PREmployeePFRequest pREmployeePFRequest = new PREmployeePFRequest();
        private PRPFWithDrawlParameter pRPFWithDrawlParameter = new PRPFWithDrawlParameter();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeePFWithDrawlRequest";

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
            DataTable dtAdvanceTypes = pRPFWithDrawlParameter.retrieveAllAdvances();

            ddlAdvanceTypeCode.DataSource = dtAdvanceTypes;
            ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataBind();

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
            DataTable dataTable = pREmployeePFWithDrawl.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            //long employeeId = ControlsHelper.getWorkerId(personalNumber);

            //dr["requestId"] = txtRequestId.Text;
            dr["employeeId"] = personalNumber;
            //dr["employeeName"] = txtEmployeeName.Text;
            dr["pFDescription"] = txtPFDescription.Text;
            dr["advanceTypeCode"] = ddlAdvanceTypeCode.SelectedValue;
            dr["requestDate"] = txtRequestDate.Text;
            dr["balance"] = txtBalance.Text;
            dr["employerPFBalance"] = txtEmployerPFBalance.Text;
            dr["outStandingAmount"] = txtOutStandingAmount.Text;
            dr["requestAmount"] = txtRequestAmount.Text;

            dataTable.Rows.Add(dr);

            #endregion

            createResult = pREmployeePFWithDrawl.create(dataTable);
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
                operationResult_BOL = eSSWorkflow.hRHelpDeskRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            if (!string.IsNullOrEmpty(personalNumber))
            {
                setEmployeePFBalance();
                setEmployerPFBalance();
                setOutstandingBalance();
            }
        }

        protected void ddlAdvanceTypeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeePFBalance();
            setEmployerPFBalance();
        }

        private void setEmployeePFBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;
            string employeePFBalance = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                employeePFBalance = pREmployeePFRequest.getEmployeePFBalance(employeeId, advanceType).ToString();
            }

            txtBalance.Text = employeePFBalance;
        }

        private void setEmployerPFBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;
            string employerPFBalance = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                employerPFBalance = pREmployeePFRequest.getEmployerPFBalance(employeeId, advanceType).ToString();
            }

            txtEmployerPFBalance.Text = employerPFBalance;
        }

        private void setOutstandingBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;
            string outstandingBalance = string.Empty;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                outstandingBalance = pREmployeePFRequest.getOutstandingBalance(employeeId, advanceType).ToString();
            }

            txtOutStandingAmount.Text = outstandingBalance;
        }
    }
}