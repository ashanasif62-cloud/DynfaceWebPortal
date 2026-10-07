using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLoan_Create : ModalForm
    {
        private PREmployeeLoan pREmployeeLoan = new PREmployeeLoan();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPREmployeeLoanRequest";

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
            DataTable dtAdvanceTypes = ControlsHelper.retrieveAllPRLoanTypes();
            ddlLoanTypeCode.DataSource = dtAdvanceTypes;
            ddlLoanTypeCode.DataTextField = "AdvanceTypeCode";
            ddlLoanTypeCode.DataValueField = "AdvanceTypeCode";
            ddlLoanTypeCode.DataBind();

            DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();
            ddlCurrency.DataSource = dtCurrency;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();

            txtRequestDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtRequestDate.Enabled = false;
            txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtRecoveryStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as TextBox).Text;
            
            SysOperationResult_BOL createResult = ProcessLoanRequest(
                personalNumber,
                ddlCurrency.SelectedValue,
                ddlLoanTypeCode.SelectedValue,
                txtRequestDate.Text,
                txtLoanAmount.Text,
                txtPaymentDate.Text,
                txtRecoveryStartDate.Text,
                txtLoanDescription.Text,
                txtRequestedInstallments.Text,
                txtRequestedInstallmentAmount.Text,
                _submitRequest
            );

            operationResults(createResult, true);
        }

        [WebMethod]
        public static object GetLoanTypes()
        {
            try
            {
                DataTable dt = ControlsHelper.retrieveAllPRLoanTypes();
                return dt.AsEnumerable().Select(r => new { Text = r["AdvanceTypeCode"].ToString(), Value = r["AdvanceTypeCode"].ToString() }).ToList();
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("ESSPREmployeeLoan_Create.GetLoanTypes", ex);
                return null;
            }
        }

        [WebMethod]
        public static object GetCurrencies()
        {
            try
            {
                DataTable dt = ControlsHelper.retrieveAllCurrencyDetails();
                return dt.AsEnumerable().Select(r => new { Text = r["CurrencyCode"].ToString(), Value = r["CurrencyCode"].ToString() }).ToList();
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("ESSPREmployeeLoan_Create.GetCurrencies", ex);
                return null;
            }
        }

        [WebMethod]
        public static string GetEmployeeCurrency(string employeeId)
        {
            try
            {
                return ControlsHelper.getEmployeeCurrencyCode(employeeId);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("ESSPREmployeeLoan_Create.GetEmployeeCurrency", ex);
                return "PKR";
            }
        }

        [WebMethod]
        public static SysOperationResult_BOL SaveLoanAjax(
            string employeeId, string currency, string loanType, string reqDate, 
            string amount, string payDate, string recoveryDate, string desc, 
            string installments, string installmentAmount, bool submit)
        {
            try
            {
                return ProcessLoanRequest(employeeId, currency, loanType, reqDate, amount, payDate, recoveryDate, desc, installments, installmentAmount, submit);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("ESSPREmployeeLoan_Create.SaveLoanAjax", ex);
                return new SysOperationResult_BOL { isSuccess = false, Message = ex.Message, AlertType = AlertType.Error.ToString() };
            }
        }

        private static SysOperationResult_BOL ProcessLoanRequest(
            string personalNumber, string currency, string loanType, string reqDate, 
            string amount, string payDate, string recoveryDate, string desc, 
            string installments, string installmentAmount, bool submitRequest)
        {
            PREmployeeLoan pREmployeeLoan = new PREmployeeLoan();
            long workerRecId = ControlsHelper.getWorkerId(personalNumber);

            DataTable dataTable = pREmployeeLoan.createDataTable();
            DataRow dr = dataTable.NewRow();
            dr["EmployeeId"] = workerRecId;
            dr["Currency"] = currency;
            dr["LoanTypeCode"] = loanType;
            dr["RequestDate"] = reqDate;
            dr["LoanAmount"] = amount;
            dr["RequestedPaymentDate"] = payDate;
            dr["RecoveryStartDate"] = recoveryDate;
            dr["LoanDescription"] = desc;
            dr["RequestedInstallments"] = installments;
            dr["RequestedInstallmentAmount"] = installmentAmount;
            dr["RequestedPaymentDate"] = DateTime.Now.ToString("dd/MM/yyyy");

            // ALWAYS SET RECOVERY START DATE TO TODAY
            dr["RecoveryStartDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            dataTable.Rows.Add(dr);

            SysOperationResult_BOL createResult = pREmployeeLoan.create(dataTable);
            
            if (createResult.isSuccess && submitRequest && createResult.RecId > 0)
            {
                ESSWorkflow eSSWorkflow = new ESSWorkflow();
                SysOperationResult_BOL submitResult = eSSWorkflow.pREmployeeLoanRequestByRecId_Submit(createResult.RecId);
                
                if (submitResult.isSuccess)
                    submitResult.Message = " Request successfully submitted.";
                else
                    submitResult.Message = " Failed to submit the request.";

                createResult.Message += " " + submitResult.Message;
                createResult.AlertType = submitResult.AlertType;
                createResult.isSuccess = submitResult.isSuccess;
            }

            return createResult;
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.pREmployeeLoanRequestByRecId_Submit(requestRecId);
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
