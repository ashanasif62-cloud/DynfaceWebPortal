using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeAdvance_Create : ModalForm
    {
        private PREmployeeAdvances pREmployeeAdvances = new PREmployeeAdvances();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeAdvanceRequest";

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
            DataTable dtAdvanceTypes = ControlsHelper.retrieveByAdvanceType(PRAdvanceType.Salary.GetHashCode());
            ddlAdvanceTypeCode.DataSource = dtAdvanceTypes;
            ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataBind();

            DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();
            ddlCurrency.DataSource = dtCurrency;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();

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
            DataTable dataTable = pREmployeeAdvances.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmployeeId"] = employeeId;
            dr["RequestDate"] = txtRequestDate.Text;
            dr["RequestedPaymentDate"] = txtPaymentDate.Text;
            //dr["AdvanceType"] = PRAdvanceType.Salary.GetHashCode();
            dr["AdvanceTypeCode"] = ddlAdvanceTypeCode.SelectedValue;
            dr["AdvanceDescription"] = txtAdvanceDescription.Text;
            dr["AdvanceAmount"] = txtAdvanceAmount.Text;
            dr["Currency"] = ddlCurrency.SelectedValue;
            dataTable.Rows.Add(dr);

            #endregion

            createResult = pREmployeeAdvances.create(dataTable);
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
                operationResult_BOL = eSSWorkflow.pREmployeeAdvanceRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            if (!string.IsNullOrEmpty(personalNumber))
            {
                ddlCurrency.SelectedValue = ControlsHelper.getEmployeeCurrencyCode(personalNumber);
            }
        }
    }
}
