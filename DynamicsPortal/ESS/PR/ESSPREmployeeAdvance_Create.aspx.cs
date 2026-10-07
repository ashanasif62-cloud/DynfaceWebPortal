using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeAdvance_Create : ModalForm
    {
        private PREmployeeAdvances pREmployeeAdvances = new PREmployeeAdvances();
        private DataTable dtAdvanceTypes;

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
                    dtAdvanceTypes = ControlsHelper.retrieveByAdvanceType(PRAdvanceType.Salary.ToString());

                    bindControlsData();

                    AdvanceAmountAutoFill();

                    LoadAdvanceTypeData();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();

                System.Reflection.MethodBase currentMethod =
                    System.Reflection.MethodBase.GetCurrentMethod();

                string currentMethodName =
                    currentMethod.DeclaringType.FullName;

                objErrorLog.write(currentMethodName, ex);
            }
        }

        private void bindControlsData()
        {
            ddlAdvanceTypeCode.DataSource = dtAdvanceTypes;
            ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
            ddlAdvanceTypeCode.DataBind();

            DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();

            ddlCurrency.DataSource = dtCurrency;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();

            txtRequestDate.Text =
                DateTime.Now.ToString(
                    "MM/dd/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);

            // SET PAYMENT DATE TO TODAY
            txtPaymentDate.Text =
                DateTime.Now.ToString(
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);

            txtRequestDate.Enabled = false;
            ddlAdvanceTypeCode.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
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
            decimal requestedInstallments = 0;
            decimal advanceAmount = 0;
            decimal requestedInstallmentAmount = 0;

            long requestRecId = 0;

            bool submitRequest = _submitRequest;

            SysOperationResult_BOL createResult =
                new SysOperationResult_BOL();

            SysOperationResult_BOL submitResult =
                new SysOperationResult_BOL();

            #region CreateRequest

            DataTable dataTable =
                pREmployeeAdvances.createDataTable();

            DataRow dr = dataTable.NewRow();

            string personalNumber =
                (cddlEmployeeDetails.FindControl("txtEmployeeId")
                as TextBox).Text;

            long employeeId =
                ControlsHelper.getWorkerId(personalNumber);

            dr["EmployeeId"] = employeeId;

            string requestDateStr = "";
            DateTime parsedDate;

            if (DateTime.TryParseExact(
                    txtRequestDate.Text.Trim(),
                    "MM/dd/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out parsedDate))
            {
                requestDateStr = parsedDate.ToString("dd/MM/yyyy");
            }
            DateTime parsedPaymentDate;

            // ALWAYS SET PAYMENT DATE TO TODAY
            string paymentDateStr = DateTime.Now.ToString(
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            dr["RequestDate"] = requestDateStr;
            dr["RequestedPaymentDate"] = paymentDateStr;
            dr["RecoveryStartDate"] = paymentDateStr;

            dr["Guarantor1"] = string.Empty;
            dr["Guarantor2"] = string.Empty;

            dr["AdvanceTypeCode"] = ddlAdvanceTypeCode.SelectedValue;
            dr["AdvanceDescription"] = txtAdvanceDescription.Text;
            dr["AdvanceAmount"] = txtAdvanceAmount.Text;
            dr["Currency"] = ddlCurrency.SelectedValue;

            // =========================
            // GET RECOVERIES
            // =========================

            decimal recoveries = 0;

            if (Session["Recoveries"] != null)
            {
                Decimal.TryParse(
                    Session["Recoveries"].ToString(),
                    out recoveries);
            }

            requestedInstallments = recoveries;

            if (requestedInstallments > 0)
            {
                Decimal.TryParse(
                    txtAdvanceAmount.Text,
                    out advanceAmount);

                requestedInstallmentAmount =
                    advanceAmount / requestedInstallments;
            }

            dr["RequestedInstallments"] =
                requestedInstallments;

            dr["RequestedInstallmentAmount"] =
                requestedInstallmentAmount;

            dataTable.Rows.Add(dr);

            #endregion

            createResult = pREmployeeAdvances.create(dataTable);

            requestRecId = createResult.RecId;

            if (createResult.isSuccess && submitRequest)
            {
                if (requestRecId > 0)
                {
                    submitResult =
                        submitWFRequest(requestRecId);

                    if (submitResult.isSuccess)
                    {
                        submitResult.Message =
                            " Advance request created successfully.";
                    }
                    else
                    {
                        submitResult.Message =
                            " Failed to submit the request.";
                    }
                }
                else
                {
                    submitResult.AlertType =
                        AlertType.Error.ToString();

                    submitResult.isSuccess = false;

                    submitResult.Message =
                        " Failed to submit the created request.";
                }

                createResult.Message +=
                    " " + submitResult.Message;

                createResult.AlertType =
                    submitResult.AlertType;

                createResult.isSuccess =
                    submitResult.isSuccess;
            }

            bool result = operationResults(createResult);

            if (createResult != null &&
                createResult.isSuccess)
            {
                string script =
                @"setTimeout(function() {
                    if (window.parent &&
                        typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }

                    closeDialog();

                }, 3000);";

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "closeModal",
                    script,
                    true);

                return;
            }
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;

            ESSWorkflow eSSWorkflow =
                new ESSWorkflow();

            SysOperationResult_BOL operationResult_BOL =
                new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL =
                    eSSWorkflow
                    .pREmployeeAdvanceRequestByRecId_Submit(
                        requestRecId);
            }

            return operationResult_BOL;
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(
            object sender,
            EventArgs e)
        {
            string personalNumber =
                (cddlEmployeeDetails.FindControl("txtEmployeeId")
                as TextBox).Text;

            if (!string.IsNullOrEmpty(personalNumber))
            {
                ddlCurrency.SelectedValue =
                    ControlsHelper
                    .getEmployeeCurrencyCode(personalNumber);
            }
        }

        protected void onRequestedDateModified(
            object sender,
            EventArgs e)
        {
            SysOperationResult_BOL submitResult =
                new SysOperationResult_BOL();

            if (!string.IsNullOrEmpty(txtRequestDate.Text)
                &&
                !string.IsNullOrEmpty(txtPaymentDate.Text))
            {
                DateTime requestDate;
                DateTime paymentDate;

                string[] formats =
                {
                    "MM/dd/yyyy",
                    "yyyy-MM-dd",
                    "dd-MM-yyyy"
                };

                if (DateTime.TryParseExact(
                    txtRequestDate.Text,
                    formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out requestDate)
                    &&
                    DateTime.TryParseExact(
                    txtPaymentDate.Text,
                    formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out paymentDate))
                {
                    if (paymentDate < requestDate)
                    {
                        submitResult.Message =
                            "Requested Payment date cannot be less than Request Date";

                        submitResult.AlertType =
                            AlertType.Error.ToString();

                        NotificationMessage.showMessage(submitResult);

                        txtPaymentDate.Text = string.Empty;
                    }
                }
            }
        }

        protected void onAdvanceTypeChange(
            object sender,
            EventArgs e)
        {
            string advancetypeCode =
                ddlAdvanceTypeCode.SelectedValue;

            DataTable dt =
                pREmployeeAdvances
                .retrieveAdvanceTypeDetails(advancetypeCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                double recoveries =
                    row["Recoveries"] != DBNull.Value
                    ?
                    Convert.ToDouble(row["Recoveries"])
                    :
                    0.0;

                string fromDayMonth =
                    row["FromDayMonth"] != DBNull.Value
                    ?
                    row["FromDayMonth"].ToString()
                    :
                    string.Empty;

                string toDayMonth =
                    row["ToDayMonth"] != DBNull.Value
                    ?
                    row["ToDayMonth"].ToString()
                    :
                    string.Empty;

                Session["Recoveries"] = recoveries;
                Session["FromDayMonth"] = fromDayMonth;
                Session["ToDayMonth"] = toDayMonth;
            }
            else
            {
                Session["Recoveries"] = 0.0;
                Session["FromDayMonth"] = string.Empty;
                Session["ToDayMonth"] = string.Empty;
            }

            LoadAdvanceTypeData();
        }

        private void LoadAdvanceTypeData()
        {
            string advancetypeCode =
                ddlAdvanceTypeCode.SelectedValue;

            if (string.IsNullOrEmpty(advancetypeCode))
                return;

            string cacheKey =
                "PRAdvanceTypes_" + advancetypeCode;

            DataTable dt =
                Cache[cacheKey] as DataTable;

            if (dt == null)
            {
                PRAdvanceTypes service =
                    new PRAdvanceTypes();

                dt =
                    service.retrieveAllPRAdvanceTypes("");

                Cache.Insert(
                    cacheKey,
                    dt,
                    null,
                    DateTime.Now.AddMinutes(30),
                    System.Web.Caching.Cache.NoSlidingExpiration
                );
            }

            if (dt == null || dt.Rows.Count == 0)
                return;

            DataRow[] rows =
                dt.Select(
                    "AdvanceTypeCode = '" +
                    advancetypeCode + "'");

            if (rows.Length == 0)
                return;

            DataRow row = rows[0];

            double recoveries =
                row["Recoveries"] != DBNull.Value
                ?
                Convert.ToDouble(row["Recoveries"])
                :
                0;

            Session["Recoveries"] = recoveries;
        }

        public void AdvanceAmountAutoFill()
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

            PRAdvanceTypes advanceType =
                new PRAdvanceTypes();

            DataTable dt2 =
                advanceType.retrieveAllPRAdvanceTypes(personalNumber);

            if (dt2 == null || dt2.Rows.Count == 0)
                return;

            string calculationCode =
                dt2.Rows[0]["CalculationCode"] != DBNull.Value
                ?
                dt2.Rows[0]["CalculationCode"].ToString()
                :
                string.Empty;

            if (string.IsNullOrEmpty(calculationCode))
                return;

            DataTable dtFormula =
                advanceType.retrieveCalculationFormula(
                    calculationCode,
                    personalNumber,
                    txtPaymentDate.Text);

            if (dtFormula == null ||
                dtFormula.Rows.Count == 0)
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

            txtAdvanceAmount.Text = advanceAmount.ToString("N2");

            //decimal basicSalary = 0;

            //object basicSalaryObj =
            //    dtFormula.Rows[0]["BasicSalary"];

            //if (basicSalaryObj != null &&
            //    basicSalaryObj != DBNull.Value)
            //{
            //    basicSalary =
            //        Convert.ToDecimal(basicSalaryObj);
            //}

            //string basicSalaryStr =
            //    basicSalary.ToString(
            //        "0.##",
            //        System.Globalization.CultureInfo.InvariantCulture);

            //string expression =
            //    Regex.Replace(
            //        formula,
            //        @"(?i)\bBasic\s*Salary\b",
            //        basicSalaryStr
            //    );

            //expression = expression.Trim();

            //try
            //{
            //    DataTable dt = new DataTable();

            //    decimal result =
            //        Convert.ToDecimal(
            //            dt.Compute(expression, null));

            //    txtAdvanceAmount.Text =
            //        result.ToString("N2");
            //}
            //catch (Exception ex)
            //{
            //    txtAdvanceAmount.Text = string.Empty;

            //    System.Diagnostics.Debug.WriteLine(
            //        "Compute failed: " +
            //        ex.Message +
            //        " | Expression: " +
            //        expression);
            //}
        }
    }
}