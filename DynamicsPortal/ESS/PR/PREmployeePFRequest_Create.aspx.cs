using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class PREmployeePFRequest_Create : ModalForm
    {
        private PREmployeePFRequest pREmployeePFRequest = new PREmployeePFRequest();
     //  private PRPFWithDrawlParameter pRPFWithDrawlParameter = new PRPFWithDrawlParameter();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeePFRequest";

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
            DataTable dtAdvanceTypes = pREmployeePFRequest.retrieveAll();
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
            //txtRequestedInstallments.Text = "48";

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
            DataTable dataTable = pREmployeePFRequest.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            //long employeeId = ControlsHelper.getWorkerId(personalNumber);
            //dr["requestId"]                     = txtRequestId.Text;
            //dr["employeeName"]                  = txtEmployeeName.Text;
            //dr["requestedInstallmentAmount"]    = txtRequestedInstallmentAmount.Text;
            //dr["advanceIdRef"]                  = txtAdvanceIdRef.Text;
            //dr["outStandingAmount"]             = txtOutStandingAmount.Text;
            //dr["payGroupCode"]                  = txtPayGroupCode.Text;
            dr["employeeId"]                    = personalNumber;
            dr["pFDescription"]                 = txtPFDescription.Text;
            dr["requestDate"]                   = txtRequestDate.Text;
            dr["requestedPaymentDate"]          = txtRequestedPaymentDate.Text;
            dr["advanceTypeCode"]               = ddlAdvanceTypeCode.SelectedValue;
            dr["pFBalance"]                     = txtPFBalance.Text;
            dr["employerPFBalance"]             = txtEmployerPFBalance.Text;
            dr["requestAmount"]                 = txtRequestAmount.Text;
            dr["currency"]                      = ddlCurrency.SelectedValue;
            dr["recoveryStartDate"]             = txtRecoveryStartDate.Text;
            dr["requestedInstallments"]         = txtRequestedInstallments.Text;
          

            dataTable.Rows.Add(dr);

            #endregion

            createResult = pREmployeePFRequest.create(dataTable);
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
                operationResult_BOL = eSSWorkflow.pREmployeePFRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        //protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        //{
        //    string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
        //    if (!string.IsNullOrEmpty(personalNumber))
        //    {
        //        ddlCurrency.SelectedValue = ControlsHelper.getEmployeeCurrencyCode(personalNumber);
        //        ddlAdvanceTypeCode.SelectedValue = "P.F Loan";

        //        setEmployeePFBalance();
        //        setEmployerPFBalance();
        //    }
        //}
        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;

            if (!string.IsNullOrEmpty(personalNumber))
            {
                ddlCurrency.SelectedValue = ControlsHelper.getEmployeeCurrencyCode(personalNumber);

                // Clear the dropdown and add only the fixed value "P.F Loan"
                ddlAdvanceTypeCode.Items.Clear();
                ddlAdvanceTypeCode.Items.Add(new ListItem("P.F Loan", "P.F Loan"));
                ddlAdvanceTypeCode.SelectedIndex = 0;
                ddlAdvanceTypeCode.Enabled = false;

                // Now this will pick "P.F Loan" as SelectedValue
                setEmployeePFBalance();
                setEmployerPFBalance();
                setRequestAmount();
            }
        }

        protected void ddlAdvanceTypeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeePFBalance();
            setEmployerPFBalance();
            setRequestAmount();

        }

        //private void setEmployeePFBalance()
        //{
        //    string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
        //    string advanceType = ddlAdvanceTypeCode.SelectedValue;
        //    string employeePFBalance = string.Empty;

        //    if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
        //    {
        //        employeePFBalance = pREmployeePFRequest.getEmployeePFBalance(employeeId, advanceType).ToString();
        //    }

        //    txtPFBalance.Text = employeePFBalance;
        //}


        //private void setEmployerPFBalance()
        //{
        //    string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
        //    string advanceType = ddlAdvanceTypeCode.SelectedValue;
        //    string employerPFBalance = string.Empty;

        //    if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
        //    {
        //        employerPFBalance = pREmployeePFRequest.getEmployerPFBalance(employeeId, advanceType).ToString();
        //    }

        //    txtEmployerPFBalance.Text = employerPFBalance;
        //}
        //private void setRequestAmount()
        //{
        //    string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
        //    string advanceType = ddlAdvanceTypeCode.SelectedValue;
        //    string RequestAmount = string.Empty;

        //    if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
        //    {
        //        RequestAmount = pREmployeePFRequest.getRequestAmount(employeeId, advanceType).ToString();
        //    }

        //    txtRequestAmount.Text = RequestAmount;
        //}
        private void setEmployeePFBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                decimal employeePFBalance = pREmployeePFRequest.getEmployeePFBalance(employeeId, advanceType);
                txtPFBalance.Text = employeePFBalance.ToString("N0"); // with thousand separators, no decimals
            }
        }

        private void setEmployerPFBalance()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                decimal employerPFBalance = pREmployeePFRequest.getEmployerPFBalance(employeeId, advanceType);
                txtEmployerPFBalance.Text = employerPFBalance.ToString("N0");
            }
        }

        private void setRequestAmount()
        {
            string employeeId = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string advanceType = ddlAdvanceTypeCode.SelectedValue;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceType))
            {
                decimal requestAmount = pREmployeePFRequest.getRequestAmount(employeeId, advanceType);
                txtRequestAmount.Text = requestAmount.ToString("N0");
            }
        }
    }
}