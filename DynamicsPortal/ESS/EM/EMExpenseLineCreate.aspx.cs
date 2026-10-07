using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using PortalIntegration.ExpenseManagmentSvc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalIntegration;
using BussinessLogic;
using BussinessObject;
using PortalIntegration.PurchaseRequisitionLineSvcReference;

namespace DynamicsPortal.ESS.EM
{
    public partial class EMExpenseLineCreate : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ExpenseLines_Create";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    string expenseReportNumber = Request.QueryString["ExpenseReportNumber"] ?? string.Empty;
                    txtExpenseReqportNumber.Text = expenseReportNumber;
                    
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
            DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();
            ddlExchangeCode.DataSource = dtCurrency;
            ddlExchangeCode.DataTextField = "CurrencyCode";
            ddlExchangeCode.DataValueField = "CurrencyCode";
            ddlExchangeCode.DataBind();
            
            DataTable dtExpenseCategory = ControlsHelper.getAllExpenseCategory();
            ddlExpenseCategory.DataSource = dtExpenseCategory;
            ddlExpenseCategory.DataTextField = "CategoryId";
            ddlExpenseCategory.DataValueField = "CategoryId";
            ddlExpenseCategory.DataBind();
            ddlExpenseCategory.Items.Insert(0, new ListItem("", ""));

            DataTable dtProject = ControlsHelper.getProject();
            ddlProjId.DataSource = dtProject;
            ddlProjId.DataTextField = "ProjId";
            ddlProjId.DataValueField = "ProjId";
            ddlProjId.DataBind();
            ddlProjId.Items.Insert(0, new ListItem("", ""));

            DataTable dtProjectLineProperty = ControlsHelper.getProjectLineProperty();
            ddlProjStatusId.DataSource = dtProjectLineProperty;
            ddlProjStatusId.DataTextField = "ProjLinePropertyId";
            ddlProjStatusId.DataValueField = "ProjLinePropertyId";
            ddlProjStatusId.DataBind();
            ddlProjStatusId.Items.Insert(0, new ListItem("", ""));

            DataTable dtProjectActivity = ControlsHelper.getProjectActivity();
            ddlProjActivityNumber.DataSource = dtProjectActivity;
            ddlProjActivityNumber.DataTextField = "ProjActivityNumber";
            ddlProjActivityNumber.DataValueField = "ProjActivityNumber";
            ddlProjActivityNumber.DataBind();
            ddlProjActivityNumber.Items.Insert(0, new ListItem("", ""));

            txtTransDate.Text = DateTime.Now.ToString("dd/MM/yyyy");


            ddlProjId.Enabled = false;
            ddlProjActivityNumber.Enabled = false;
            ddlProjStatusId.Enabled = false;
        }

        protected void ddlMerchant_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedExpenseCategory = ddlExpenseCategory.SelectedValue;

            if (!string.IsNullOrEmpty(selectedExpenseCategory))
            {
                ddlMerchant.DataSource = ControlsHelper.getMerchant(selectedExpenseCategory);
                ddlMerchant.DataTextField = "MerchantId";
                ddlMerchant.DataValueField = "MerchantId";
                ddlMerchant.DataBind();

                ddlProjId.Enabled = true;
                ddlProjActivityNumber.Enabled = true;
                ddlProjStatusId.Enabled = true;
            }
            else
            {
                ddlMerchant.Items.Clear();
                ddlProjId.Enabled = false;
                ddlProjActivityNumber.Enabled = false;
                ddlProjStatusId.Enabled = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExpenseLines lines = new ExpenseLines();
            try
            {
                var expenseLinesFields = new List<ExpenseLineRequest> {
                    //new ExpenseLineRequest
                    //{
                    //    __k_parmFieldName = "TransDate",
                    //    __k_parmFieldValue = txtTransDate.Text,
                    //    __k_parmFieldType = "Date"
                    //},
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "AmountCurr",
                        __k_parmFieldValue = txtAmountCur.Text,
                        __k_parmFieldType = "Real"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "CostType",
                        __k_parmFieldValue = ddlExpenseCategory.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExchangeCode",
                        __k_parmFieldValue = ddlExchangeCode.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "MerchantId",
                        __k_parmFieldValue = ddlMerchant.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExpenseReportNumber",
                        __k_parmFieldValue = txtExpenseReqportNumber.Text,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjId",
                        __k_parmFieldValue = ddlProjId.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjActivityNumber",
                        __k_parmFieldValue = ddlProjActivityNumber.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjStatusId",
                        __k_parmFieldValue = ddlProjStatusId.SelectedValue,
                        __k_parmFieldType = "String"
                    },
                };

                SysOperationResult_BOL result = lines.createExpenseLines(expenseLinesFields);

                if (result.isSuccess)
                {
                    NotificationMessage.showMessage(result);
                    // NEW script: Refresh parent GridView and reload this form
                    string refreshScript = $@"
             if (window.opener && !window.opener.closed) {{
                window.opener.__doPostBack('', 'RefreshGrid');
              }}
           setTimeout(function() {{
           window.location.href = window.location.pathname + window.location.search;
             }}, 1000);
             ";


                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshAndReload", refreshScript, true);
                }
                else
                    NotificationMessage.showMessage(result);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }
    }
}