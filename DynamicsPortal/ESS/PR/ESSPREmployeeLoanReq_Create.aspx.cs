using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLoanReq_Create : ModalForm
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
                    onAdvanceTypeChange(sender, e);
                    LoanAmountAutoFill();
                }

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Request for loan";
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
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as TextBox).Text;
            PRAdvanceTypes advanceType = new PRAdvanceTypes();
            DataTable dtAdvanceTypes = advanceType.retrieveAllPRAdvanceTypes(personalNumber);
            // DataTable dtAdvanceTypes = advanceType.retrieveByAdvanceTypeCode();

            DataView dv = dtAdvanceTypes.DefaultView;
            dv.RowFilter = "AdvanceType = 'Loan'";

            ddlAdvanceTypeCode.DataSource = dv;
            ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataBind();

            DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();
            ddlCurrency.DataSource = dtCurrency;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();
            ddlCurrency.Enabled = false;

            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestDate.Enabled = false;
            txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); // ✅ Correct format
            txtPaymentDate.Enabled = true;
            DateTime today = DateTime.Now;
            DateTime lastDayOfMonth = new DateTime(today.Year, today.Month,
                                                   DateTime.DaysInMonth(today.Year, today.Month));

            //txtRecoveryStartDate.Text = lastDayOfMonth.ToString("dd/MM/yyyy");
            txtRecoveryStartDate.Enabled = true;

            //PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            //DataTable dtEmployees = pREmploymentInformation.retrieveEmployeeLookup();

            //ddlGuarantor1.DataSource = dtEmployees;
            //ddlGuarantor1.DataTextField = "EmployeeName";
            //ddlGuarantor1.DataValueField = "EmployeeId";
            //ddlGuarantor1.DataBind();
            //ddlGuarantor1.Items.Insert(0, new ListItem("Select Guarantor 1", ""));

            //ddlGuarantor2.DataSource = dtEmployees;
            //ddlGuarantor2.DataTextField = "EmployeeName";
            //ddlGuarantor2.DataValueField = "EmployeeId";
            //ddlGuarantor2.DataBind();
            //ddlGuarantor2.Items.Insert(0, new ListItem("Select Guarantor 2", ""));
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

            double maxRecoveries = 0.0;
            if (Session["Recoveries"] != null)
            {
                double.TryParse(Session["Recoveries"].ToString(), out maxRecoveries);
            }


            double requestedInstallment = 0.0;
            double.TryParse(txtRequestedInstallments.Text, out requestedInstallment);


            if (requestedInstallment > maxRecoveries)
            {
                createResult.Message = $"Recoveries cannot be greater than available recoveries <b>{maxRecoveries}</b>.";
                createResult.AlertType = AlertType.Error.ToString();
                NotificationMessage.showMessage(createResult);
                return;
            }

            //string guarantor1 = ddlGuarantor1.SelectedValue;
            //string guarantor2 = ddlGuarantor2.SelectedValue;
            //dr["Guarantor1"] = guarantor1;
            //dr["Guarantor2"] = guarantor2;

            dr["EmployeeId"] = employeeId;
            dr["Currency"] = ddlCurrency.SelectedValue;
            dr["AdvanceTypeCode"] = ddlAdvanceTypeCode.SelectedValue;


            dr["RequestDate"] = txtRequestDate.Text;
            dr["AdvanceAmount"] = txtLoanAmount.Text;
            dr["RequestedPaymentDate"] = txtPaymentDate.Text;
            dr["RecoveryStartDate"] = txtRecoveryStartDate.Text;
            dr["AdvanceDescription"] = txtAdvanceDescription.Text;
            dr["RequestedInstallments"] = txtRequestedInstallments.Text;
            dr["RequestedInstallmentAmount"] = txtRequestedInstallmentAmount.Text;

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
                return;
            }


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

        protected void onAdvanceTypeChange(object sender, EventArgs e)
        {


            string advancetypeCode = ddlAdvanceTypeCode.SelectedValue;


            DataTable dt = pREmployeeAdvances.retrieveAdvanceTypeDetails(advancetypeCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];


                double recoveries = row["Recoveries"] != DBNull.Value ? Convert.ToDouble(row["Recoveries"]) : 0.0;
                string fromDayMonth = row["FromDayMonth"] != DBNull.Value ? row["FromDayMonth"].ToString() : string.Empty;
                string toDayMonth = row["ToDayMonth"] != DBNull.Value ? row["ToDayMonth"].ToString() : string.Empty;


                Session["Recoveries"] = recoveries;
                Session["FromDayMonth"] = fromDayMonth;
                Session["ToDayMonth"] = toDayMonth;

                // Auto-populate Total Recoveries on the UI
                txtRequestedInstallments.Text = recoveries.ToString();
            }
            else
            {

                Session["Recoveries"] = 0.0;
                Session["FromDayMonth"] = string.Empty;
                Session["ToDayMonth"] = string.Empty;
                txtRequestedInstallments.Text = "0";
            }
            LoanAmountAutoFill();
        }



        protected void onPaymentDateModified(object sender, EventArgs e)
        {
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            if (!string.IsNullOrEmpty(txtRequestDate.Text) && !string.IsNullOrEmpty(txtPaymentDate.Text))
            {
                DateTime requestDate;
                DateTime paymentDate;

                if (DateTime.TryParse(txtRequestDate.Text, out requestDate) &&
                    DateTime.TryParse(txtPaymentDate.Text, out paymentDate))
                {
                    if (paymentDate < requestDate)
                    {
                        submitResult.Message = "Requested Payment date cannot be less than Request Date";
                        submitResult.AlertType = AlertType.Error.ToString();
                        NotificationMessage.showMessage(submitResult);

                        // Clear the Payment Date textbox
                        txtPaymentDate.Text = string.Empty;
                    }
                }
            }
        }

        protected void ValidateGuarantors(object sender, EventArgs e)
        {
            //string g1 = ddlGuarantor1.SelectedValue;
            //string g2 = ddlGuarantor2.SelectedValue;

            //if (!string.IsNullOrEmpty(g1) &&
            //    !string.IsNullOrEmpty(g2) &&
            //    g1 == g2)
            //{
            //    SysOperationResult_BOL result = new SysOperationResult_BOL();
            //    result.Message = "Please select a different guarantor. Guarantor 1 and Guarantor 2 cannot be same.";
            //    result.AlertType = AlertType.Error.ToString();

            //    NotificationMessage.showMessage(result);

            //    // Reset second dropdown
            //    ddlGuarantor2.SelectedIndex = 0;
            //}
        }


        public void LoanAmountAutoFill()
        {
            string personalNumber =
                (cddlEmployeeDetails.FindControl("txtEmployeeId")
                as TextBox).Text;

            if (string.IsNullOrEmpty(personalNumber))
            {
                personalNumber =
                    SessionVariables.getCurrentEmployeeId();

                if (string.IsNullOrEmpty(personalNumber))
                    return;
            }

            string advancetypeCode = ddlAdvanceTypeCode.SelectedValue;

            if (string.IsNullOrEmpty(advancetypeCode))
                return;

            PRAdvanceTypes advanceType = new PRAdvanceTypes();

            DataTable dt2 =
                advanceType.retrieveAllPRAdvanceTypes(personalNumber);

            if (dt2 == null || dt2.Rows.Count == 0)
                return;

            // Filter to the selected loan type
            DataRow[] matchedRows =
                dt2.Select("AdvanceTypeCode = '" + advancetypeCode + "'");

            if (matchedRows.Length == 0)
                return;

            DataRow matchedRow = matchedRows[0];

            string calculationCode =
                matchedRow["CalculationCode"] != DBNull.Value
                ? matchedRow["CalculationCode"].ToString()
                : string.Empty;

            if (string.IsNullOrEmpty(calculationCode))
            {
                // Fallback: use MaxAdanceAmount directly from the row
                decimal maxAmount =
                    matchedRow["MaxAdanceAmount"] != DBNull.Value
                    ? Convert.ToDecimal(matchedRow["MaxAdanceAmount"])
                    : 0;


                txtLoanAmount.Text = maxAmount.ToString("N2");

                return;
            }

            DataTable dtFormula =
                advanceType.retrieveCalculationFormula(
                    calculationCode,
                    personalNumber,
                    txtPaymentDate.Text);

            if (dtFormula == null || dtFormula.Rows.Count == 0)
                return;

            string formula =
                dtFormula.Rows[0]["CalculationFormula"]
                .ToString()
                .Trim();

            decimal advanceAmount = 0;

            object advanceAmountObj = dtFormula.Rows[0]["BasicSalary"]; //Amount is stored in BasicSalary column for now

            if (advanceAmountObj != null &&
                advanceAmountObj != DBNull.Value)
            {
                advanceAmount = Convert.ToDecimal(advanceAmountObj);
            }

            txtLoanAmount.Text = advanceAmount.ToString("N2");

            //decimal basicSalary = 0;

            //object basicSalaryObj = dtFormula.Rows[0]["BasicSalary"];

            //if (basicSalaryObj != null &&
            //    basicSalaryObj != DBNull.Value)
            //{
            //    basicSalary = Convert.ToDecimal(basicSalaryObj);
            //}

            //string basicSalaryStr =
            //    basicSalary.ToString(
            //        "0.##",
            //        System.Globalization.CultureInfo.InvariantCulture);

            //string expression =
            //    System.Text.RegularExpressions.Regex.Replace(
            //        formula,
            //        @"(?i)\bBasic\s*Salary\b",
            //        basicSalaryStr
            //    );

            //expression = expression.Trim();

            //try
            //{
            //    DataTable dt = new DataTable();

            //    decimal result =
            //        Convert.ToDecimal(dt.Compute(expression, null));

            //    txtLoanAmount.Text = result.ToString("N2");
            //}
            //catch (Exception ex)
            //{
            //    txtLoanAmount.Text = string.Empty;

            //    System.Diagnostics.Debug.WriteLine(
            //        "LoanAmountAutoFill Compute failed: " +
            //        ex.Message +
            //        " | Expression: " +
            //        expression);
            //}
        }
    }
}
