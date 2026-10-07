using BussinessObject;
using DynamicsPortal.ESS.PR;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.ExpenseManagmentSvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Shapes;

namespace DynamicsPortal.ESS.EM
{
    public partial class EMExpenseLines : MainForm
    {
        private ExpenseLines expenseLines = new ExpenseLines();
        string expenseReportNumber;

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (eventArgument == "Refresh")
            {
                reBindGrid();
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Always rebuild dynamic controls early
            showFinancialDimension(0);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = expenseLines.tableName;
                pageMenuId = "ExpenseLine_ListPage";
                expenseReportNumber = Session["ExpenseReportNumber"].ToString() ?? string.Empty;
                //btnNew.OnClientClick = $"return openPopupPanel('/ESS/EM/EMExpenseLineCreate.aspx?expenseReportNumber={expenseReportNumber}');";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;
                if (!isPageAuthorizated)
                    return;
                long defaultDimension = Session["DefaultDimension"] != null
                 ? Convert.ToInt64(Session["DefaultDimension"])
                      : 0;

                if (!IsPostBack)
                {
                    string newexpensenumber = Session["ExpenseReportNumber"] as string;

                    reBindGrid();
                    bindHeaderFields();
                    bindExpenseCategory();


                    bindCurrencyDropdown();
                    SetLineValues();
                   
                    //if (Session["CostType"] != null)
                    //{
                    //    string costType = Session["CostType"].ToString();

                    //    if (ddlExpenseCategorylines.Items.FindByValue(costType) != null)
                    //    {
                    //        ddlExpenseCategorylines.SelectedValue = costType;
                    //    }
                    //}

                    //if (Session["MerchantId"] != null)
                    //{
                    //    string merchant = Session["MerchantId"].ToString();

                    //    if (ddlMerchantlines.Items.FindByValue(merchant) != null)
                    //    {
                    //        ddlMerchantlines.SelectedValue = merchant;
                    //    }
                    //}
                 



                    showFinancialDimension(defaultDimension);
                }
                enableDisableButtons();

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

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        private void getGridDataTable()
        {
            string expenseReportNumber = Session["ExpenseReportNumber"].ToString() ?? string.Empty;
            DataTable dt = expenseLines.retrieveAll(expenseReportNumber);
            SessionVariables.setSessionDataTable(dt);
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
            EditMode.Value = "Update";
        }
        protected void btnAddGrid_Click(object sender, EventArgs e)
        {
            getGridDataTable();
            DataTable dt = SessionVariables.getSessionDataTable();

            DataRow newRow = dt.NewRow();
            newRow["ExpenseReportNumber"] = Request.QueryString["ExpenseReportNumber"] ?? string.Empty;
            dt.Rows.InsertAt(newRow, 0);

            gridView.EditIndex = 0;

            // 5. Rebind the GridView
            gridView.DataSource = dt;
            gridView.DataBind();

            EditMode.Value = "Create";
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string expenseTransactionNumber = "";
                    expenseTransactionNumber = ((Label)gridViewRow.FindControl("lblExpenseTransactionNumber")).Text;
                    if (expenseTransactionNumber != "")
                    {
                        recordsId.Add(expenseTransactionNumber);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                //expenseLines.DeleteExpenseLine(recordsId.ToArray());
                SysOperationResult_BOL operationResult_BOL = expenseLines.DeleteExpenseLine(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    bindGrid();
                }
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTransDate = e.Row.FindControl("lblTransDate") as Label;
                if (lblTransDate != null && DateTime.TryParse(lblTransDate.Text, out DateTime viewDate))
                {
                    lblTransDate.Text = viewDate == new DateTime(1900, 1, 1)
                        ? ""
                        : viewDate.ToString("yyyy-MM-dd");
                    Session["TransDate"] = viewDate;
                }
                Label lblMerchant = e.Row.FindControl("lblMerchant") as Label;
                if (lblMerchant != null)
                {
                    Session["MerchantId"] = lblMerchant.Text;
                }

                Label lblCostType = e.Row.FindControl("lblCostType") as Label;

                if (lblCostType != null)
                {
                    Session["CostType"] = lblCostType.Text;
                }
                Label lblAmountCurr = e.Row.FindControl("lblAmountCurr") as Label;
                if (lblAmountCurr != null)
                {
                    Session["AmountCurr"] = lblAmountCurr.Text;
                }



                if (gridView.EditIndex == e.Row.RowIndex)
                {
                    try
                    {
                        string tdate = DataBinder.Eval(e.Row.DataItem, "TransDate")?.ToString();
                        TextBox txtTransDate = e.Row.FindControl("txtTransDate") as TextBox;
                        if (tdate != null && DateTime.TryParse(tdate, out DateTime parsedDate) && txtTransDate != null)
                            txtTransDate.Text = parsedDate.ToString("yyyy-MM-dd");

                        string AmountCurr = DataBinder.Eval(e.Row.DataItem, "AmountCurr")?.ToString();
                        TextBox txtAmountCurr = e.Row.FindControl("txtAmountCurr") as TextBox;
                        if (AmountCurr != null && txtAmountCurr != null)
                            txtAmountCurr.Text = AmountCurr;

                        DropDownList ddlExchangeCode = e.Row.FindControl("ddlExchangeCode") as DropDownList;
                        if (ddlExchangeCode != null)
                        {
                            ddlExchangeCode.Items.Clear();

                            // Retrieve or cache purpose data
                            DataTable dtExchangeCode = ViewState["ExchangeCodeData"] as DataTable ?? ControlsHelper.retrieveAllCurrencyDetails();
                            if (dtExchangeCode != null && dtExchangeCode.Columns.Contains("CurrencyCode"))
                            {
                                ViewState["ExchangeCodeData"] = dtExchangeCode;
                                ddlExchangeCode.DataSource = dtExchangeCode;
                                ddlExchangeCode.DataTextField = "CurrencyCode";
                                ddlExchangeCode.DataValueField = "CurrencyCode";
                                ddlExchangeCode.DataBind();
                                ddlExchangeCode.Items.Insert(0, new ListItem("", String.Empty));
                                ddlExchangeCode.CssClass += " filterable-dropdown";
                                // Set selected value
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "ExchangeCode")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlExchangeCode.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlExchangeCode.SelectedValue = selectedValue;
                                }
                            }
                        }

                        DropDownList ddlCostType = e.Row.FindControl("ddlCostType") as DropDownList;
                        if (ddlCostType != null)
                        {
                            ddlCostType.Items.Clear();

                            DataTable dtCostType = ViewState["CostTypeData"] as DataTable ?? ControlsHelper.getAllExpenseCategory();
                            if (dtCostType != null && dtCostType.Columns.Contains("CategoryId"))
                            {
                                ViewState["CostTypeData"] = dtCostType;
                                ddlCostType.DataSource = dtCostType;
                                ddlCostType.DataTextField = "CategoryId";
                                ddlCostType.DataValueField = "CategoryId";
                                ddlCostType.DataBind();
                                ddlCostType.Items.Insert(0, new ListItem("", String.Empty));
                                ddlCostType.CssClass += " filterable-dropdown";
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "CostType")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlCostType.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlCostType.SelectedValue = selectedValue;
                                }
                            }
                        }

                        DropDownList ddlMerchant = e.Row.FindControl("ddlMerchant") as DropDownList;
                        if (ddlMerchant != null)
                        {
                            ddlMerchant.Items.Clear();
                            string expenseCategory = DataBinder.Eval(e.Row.DataItem, "CostType")?.ToString();
                            DataTable dtMerchant = ViewState["MerchantData"] as DataTable ?? ControlsHelper.getMerchant(expenseCategory);
                            if (dtMerchant != null && dtMerchant.Columns.Contains("MerchantId"))
                            {
                                ViewState["MerchantData"] = dtMerchant;
                                ddlMerchant.DataSource = dtMerchant;
                                ddlMerchant.DataTextField = "MerchantId";
                                ddlMerchant.DataValueField = "MerchantId";
                                ddlMerchant.DataBind();
                                ddlMerchant.Items.Insert(0, new ListItem("", String.Empty));
                                ddlMerchant.CssClass += " filterable-dropdown";
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "MerchantId")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlMerchant.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlMerchant.SelectedValue = selectedValue;
                                }
                            }
                        }

                        DropDownList ddlProjectId = e.Row.FindControl("ddlProjectId") as DropDownList;
                        if (ddlProjectId != null)
                        {
                            ddlProjectId.Items.Clear();
                            DataTable dtProject = ViewState["ProjectData"] as DataTable ?? ControlsHelper.getProject();
                            if (dtProject != null && dtProject.Columns.Contains("ProjId"))
                            {
                                ViewState["ProjectData"] = dtProject;
                                ddlProjectId.DataSource = dtProject;
                                ddlProjectId.DataTextField = "ProjId";
                                ddlProjectId.DataValueField = "ProjId";
                                ddlProjectId.DataBind();
                                ddlProjectId.Items.Insert(0, new ListItem("", String.Empty));
                                ddlProjectId.CssClass += " filterable-dropdown";
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "ProjId")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlProjectId.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlProjectId.SelectedValue = selectedValue;
                                }
                            }
                        }

                        DropDownList ddlProjStatusId = e.Row.FindControl("ddlProjStatusId") as DropDownList;
                        if (ddlProjStatusId != null)
                        {
                            ddlProjStatusId.Items.Clear();
                            DataTable dtProject = ViewState["ProjectLinePropertyData"] as DataTable ?? ControlsHelper.getProjectLineProperty();
                            if (dtProject != null && dtProject.Columns.Contains("ProjLinePropertyId"))
                            {
                                ViewState["ProjectLinePropertyData"] = dtProject;
                                ddlProjStatusId.DataSource = dtProject;
                                ddlProjStatusId.DataTextField = "ProjLinePropertyId";
                                ddlProjStatusId.DataValueField = "ProjLinePropertyId";
                                ddlProjStatusId.DataBind();
                                ddlProjStatusId.Items.Insert(0, new ListItem("", String.Empty));
                                ddlProjStatusId.CssClass += " filterable-dropdown";
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "ProjStatusId")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlProjStatusId.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlProjStatusId.SelectedValue = selectedValue;
                                }
                            }
                        }

                        DropDownList ddlProjActivityNumber = e.Row.FindControl("ddlProjActivityNumber") as DropDownList;
                        if (ddlProjActivityNumber != null)
                        {
                            ddlProjActivityNumber.Items.Clear();
                            DataTable dtProjActivityNumber = ViewState["ProjectActivityData"] as DataTable ?? ControlsHelper.getProjectActivity();
                            if (dtProjActivityNumber != null && dtProjActivityNumber.Columns.Contains("ProjActivityNumber"))
                            {
                                ViewState["ProjectData"] = dtProjActivityNumber;
                                ddlProjActivityNumber.DataSource = dtProjActivityNumber;
                                ddlProjActivityNumber.DataTextField = "ProjActivityNumber";
                                ddlProjActivityNumber.DataValueField = "ProjActivityNumber";
                                ddlProjActivityNumber.DataBind();
                                ddlProjActivityNumber.Items.Insert(0, new ListItem("", String.Empty));
                                ddlProjActivityNumber.CssClass += " filterable-dropdown";
                                string selectedValue = DataBinder.Eval(e.Row.DataItem, "ProjActivityNumber")?.ToString();
                                if (!string.IsNullOrEmpty(selectedValue) && ddlProjActivityNumber.Items.FindByValue(selectedValue) != null)
                                {
                                    ddlProjActivityNumber.SelectedValue = selectedValue;
                                }
                            }
                        }
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

        protected void ddlExpenseCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlExpenseCategory = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlExpenseCategory.NamingContainer;
            DropDownList ddlMerchant = (DropDownList)row.FindControl("ddlMerchant");

            string selectedExpenseCategory = ddlExpenseCategory.SelectedValue;

            if (!string.IsNullOrEmpty(selectedExpenseCategory))
            {
                ddlMerchant.DataSource = ControlsHelper.getMerchant(selectedExpenseCategory);
                ddlMerchant.DataTextField = "MerchantId";
                ddlMerchant.DataValueField = "MerchantId";
                ddlMerchant.DataBind();
                ddlMerchant.Items.Insert(0, new ListItem("", ""));
            }
            else
            {
                ddlMerchant.Items.Clear();
                ddlMerchant.Items.Insert(0, new ListItem("", ""));
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            reBindGrid();
            EditMode.Value = "";
        }

        protected void SetLineValues()
        {
            // ----- Set Expense Category -----
            if (Session["CostType"] != null)
            {
                string costType = Session["CostType"].ToString();

                if (ddlExpenseCategorylines.Items.FindByValue(costType) != null)
                {
                    ddlExpenseCategorylines.SelectedValue = costType;
                }

                // IMPORTANT: Merchant depends on CostType
                bindMerchantDropdown(costType);
            }

            // ----- Set Merchant -----
            if (Session["MerchantId"] != null)
            {
                string merchant = Session["MerchantId"].ToString();

                if (ddlMerchantlines.Items.FindByValue(merchant) != null)
                {
                    ddlMerchantlines.SelectedValue = merchant;
                }
            }

            if (Session["AmountCurr"] != null)
            {
                txtTransactionAmountlines.Text = Session["AmountCurr"].ToString();
            }
            if (Session["TransDate"] is DateTime transDate)
            {
                txtTransactionDatelines.Text = transDate.ToString("yyyy-MM-dd");
            }

        }



        protected void Update_Click(object sender, EventArgs e)
        {
            ExpenseLines lines = new ExpenseLines();

            GridViewRow row = gridView.Rows[gridView.EditIndex];
            string recId = gridView.DataKeys[gridView.EditIndex].Value.ToString();

            Label lblExpenseReportNumber = (Label)row.FindControl("lblExpenseReportNumber");
            string expenseReportNumber = lblExpenseReportNumber?.Text;

            TextBox txtTransDate = (TextBox)row.FindControl("txtTransDate");
            string transDate = txtTransDate.Text;

            TextBox txtAmountCurr = (TextBox)row.FindControl("txtAmountCurr");
            string amountCurr = txtAmountCurr.Text;

            DropDownList ddlExchangeCode = (DropDownList)row.FindControl("ddlExchangeCode");
            string exchangeCode = ddlExchangeCode?.SelectedValue;

            DropDownList ddlCostType = (DropDownList)row.FindControl("ddlCostType");
            string costType = ddlCostType?.SelectedValue;

            DropDownList ddlMerchant = (DropDownList)row.FindControl("ddlMerchant");
            string merchat = ddlMerchant?.SelectedValue;

            DropDownList ddlProjectId = (DropDownList)row.FindControl("ddlProjectId");
            string ProjId = ddlProjectId?.SelectedValue;

            DropDownList ddlProjStatusId = (DropDownList)row.FindControl("ddlProjStatusId");
            string ProjStatusId = ddlProjStatusId?.SelectedValue;

            DropDownList ddlProjActivityNumber = (DropDownList)row.FindControl("ddlProjActivityNumber");
            string ProjActivityNumber = ddlProjActivityNumber?.SelectedValue;

            Label lblExpenseTransactionNumber = (Label)row.FindControl("lblExpenseTransactionNumber");
            string expenseTransNumber = lblExpenseTransactionNumber?.Text;
            if (EditMode.Value == "Update")
            {
                try
                {
                    var expenseLinesFields = new List<ExpenseLineRequest> {
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "TransDate",
                        __k_parmFieldValue = transDate,
                        __k_parmFieldType = "Date"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "AmountCurr",
                        __k_parmFieldValue = amountCurr,
                        __k_parmFieldType = "Real"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "CostType",
                        __k_parmFieldValue = costType,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExchangeCode",
                        __k_parmFieldValue = exchangeCode,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "MerchantId",
                        __k_parmFieldValue = merchat,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExpenseTransactionNumber",
                        __k_parmFieldValue = expenseTransNumber,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjId",
                        __k_parmFieldValue = ProjId,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjActivityNumber",
                        __k_parmFieldValue = ProjActivityNumber,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjStatusId",
                        __k_parmFieldValue = ProjStatusId,
                        __k_parmFieldType = "String"
                    },
                };

                    SysOperationResult_BOL result = lines.updateExpenseLine(expenseLinesFields, expenseTransNumber);

                    if (result.isSuccess)
                    {
                        NotificationMessage.showMessage(result);

                        gridView.EditIndex = -1;
                        reBindGrid();
                        EditMode.Value = "";
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
            else if (EditMode.Value == "Create")
            {
                try
                {
                    var expenseLinesFields = new List<ExpenseLineRequest> {
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "TransDate",
                        __k_parmFieldValue = transDate,
                        __k_parmFieldType = "Date"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "AmountCurr",
                        __k_parmFieldValue = amountCurr,
                        __k_parmFieldType = "Real"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "CostType",
                        __k_parmFieldValue = costType,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExchangeCode",
                        __k_parmFieldValue = exchangeCode,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "MerchantId",
                        __k_parmFieldValue = merchat,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ExpenseReportNumber",
                        __k_parmFieldValue = expenseReportNumber,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjId",
                        __k_parmFieldValue = ProjId,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjActivityNumber",
                        __k_parmFieldValue = ProjActivityNumber,
                        __k_parmFieldType = "String"
                    },
                    new ExpenseLineRequest
                    {
                        __k_parmFieldName = "ProjStatusId",
                        __k_parmFieldValue = ProjStatusId,
                        __k_parmFieldType = "String"
                    },
                };

                    SysOperationResult_BOL result = lines.createExpenseLines(expenseLinesFields);

                    if (result.isSuccess)
                    {
                        NotificationMessage.showMessage(result);

                        gridView.EditIndex = -1;
                        reBindGrid();
                        EditMode.Value = "";
                    }
                    else
                        NotificationMessage.showMessage(result);
                }
                catch (Exception ex)
                {

                }


            }
        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            LinkButton ddlExpenseCategory = (LinkButton)sender;
            GridViewRow row = (GridViewRow)ddlExpenseCategory.NamingContainer;
            Label lblExpenseTransactionNumber = (Label)row.FindControl("lblExpenseTransactionNumber");
            Session["ExpTransNumber"] = lblExpenseTransactionNumber.Text;
            base.btnAttachment_Click(sender, e);
        }

        private void enableDisableButtons()
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label status = row.FindControl("lblApprovalStatus") as Label;
                    if (status != null && status.Text != "Draft")
                    {
                        btnDelete.Enabled = false;
                        //btnNew.Enabled = false;
                        //btnNew.CssClass = "disabled-buttonLines";
                        break;
                    }
                }
            }
        }

      


        protected void bindExpenseLines(string expenseLineRecId)
        {
            // Retrieve all expense lines for the selected report
            DataTable dt = SessionVariables.getSessionDataTable();

            DataRow[] foundRows = dt.Select("RecId = " + expenseLineRecId);

            if (foundRows.Length > 0)
            {
                DataRow dr = foundRows[0];

                // LEFT COLUMN
                //ddlExpenseCategorylines.SelectedValue = dr["CostType"].ToString();
                txtTransactionDatelines.Text = Convert.ToDateTime(dr["TransDate"]).ToString("yyyy-MM-dd");
                //ddlMerchantlines.SelectedValue = dr["MerchantId"].ToString();
                ddlPaymentMethodlines.SelectedValue = dr["PayMethod"].ToString();
                //txtTransactionAmountlines.Text = dr["NetTransactionAmount"].ToString();
                ddlCurrenylines.SelectedValue = dr["ExchangeCode"].ToString();
                txtReceiptNumberlines.Text = dr["ReceiptNumber"].ToString();
                txtCategorydescriptionlines.Text = dr["Description"].ToString();
                txtInvoiceNumberlines.Text = dr["InvoiceAmt"].ToString();
                txtAdditionalInformationlines.Text = dr["AdditionalInformation"].ToString();
                txtProjectIdlines.SelectedValue = dr["ProjectTransactionID"].ToString();
                ddlBillablelines.SelectedValue = dr["ProjStatusId"].ToString();
                txtActivityNumberlines.SelectedValue = dr["ProjActivityNumber"].ToString();
                txtInternalNotelines.Text = dr["TransactionText"].ToString();
                ddlCountryRegionlines.SelectedValue = dr["CountryRegion"].ToString();
                ddlStatelines.SelectedValue = dr["AddressState"].ToString();
                ddlCitylines.SelectedValue = dr["City"].ToString();
                ddlZipCodelines.SelectedValue = dr["AddressZipCode"].ToString();
                ddlSalestaxlines.SelectedValue = dr["TaxGroup"].ToString();
                ddlItemsSalesTaxGrouplines.SelectedValue = dr["TaxItemGroup"].ToString();

                // RIGHT COLUMN
                chkSite.Checked = dr["TaxIncluded"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);
                txtCalculatedsalestaxlines.Text = dr["SalesTaxAmountInCompanyCurrency"].ToString();
                txtActualsalestaxlines.Text = dr["CorrectedTaxAmount"].ToString();
                txtNetTransactionAmountlines.Text = dr["NetTransactionAmount"].ToString();
                txtReimbursementamount.Text = dr["ReimburseAmt"].ToString();
                txtReasonlines.Text = dr["Reason"].ToString();
                txtCarRentalCheckOutDatelines.Text = Convert.ToDateTime(dr["CarRentalCheckOutDate"]).ToString("yyyy-MM-dd");
                txtCheckOutLocationlines.Text = dr["CheckOutLocation"].ToString();
                txtCarRentalReturnDate.Text = Convert.ToDateTime(dr["ReturnDate"]).ToString("yyyy-MM-dd");
                txtReturnLocationlines.Text = dr["ReturnLocation"].ToString();
                txtRenterNamelines.Text = dr["RenterName"].ToString();
                txtRentalReservationNumberlines.Text = dr["ReservationNumber"].ToString();
                txtDailyRentalRatelines.Text = dr["DailyRentalRate"].ToString();
                txtWeeklyRentalRate.Text = dr["WeeklyRentalRate"].ToString();
                txtMonthlyRentalRatelines.Text = dr["MonthlyRentalRate"].ToString();
                txtTotallines.Text = dr["TotalMiles"].ToString();
                ddlVehicleClasslines.SelectedValue = dr["VehicleClass"].ToString();
            }
            else
            {
                clearLineDetailsFields();
            }

        }

        private void bindHeaderFields()
        {
            txtheaderpurpose.Text = Session["Purpose"] as string;
            txtheaderlocation.Text = Session["Location"] as string;
        }

        private void bindExpenseCategory()
        {


            ddlExpenseCategorylines.Items.Clear();

            DataTable dtExpenseCategory = ViewState["ExpenseCategoryData"] as DataTable
                                          ?? ControlsHelper.getAllExpenseCategory();

            if (dtExpenseCategory != null && dtExpenseCategory.Columns.Contains("CategoryId"))
            {
                ViewState["ExpenseCategoryData"] = dtExpenseCategory;

                ddlExpenseCategorylines.DataSource = dtExpenseCategory;
                ddlExpenseCategorylines.DataTextField = "CategoryName";   // Display field
                ddlExpenseCategorylines.DataValueField = "CategoryId";    // Value field
                ddlExpenseCategorylines.DataBind();

                ddlExpenseCategorylines.Items.Insert(0, new ListItem("", String.Empty));
                ddlExpenseCategorylines.CssClass += " filterable-dropdown";
            }

        }

        private void bindMerchantDropdown(string costType)
        {



            ddlMerchantlines.Items.Clear();

            // Get selected expense category (used to filter merchants)
            string expenseCategory = ddlExpenseCategorylines?.SelectedValue ?? string.Empty;

            // Get data from ViewState or database
            DataTable dtMerchant = ViewState["MerchantData"] as DataTable ?? ControlsHelper.getMerchant(expenseCategory);

            if (dtMerchant != null && dtMerchant.Columns.Contains("MerchantId"))
            {
                ViewState["MerchantData"] = dtMerchant;

                ddlMerchantlines.DataSource = dtMerchant;
                ddlMerchantlines.DataTextField = "MerchantId";
                ddlMerchantlines.DataValueField = "MerchantId";
                ddlMerchantlines.DataBind();

                ddlMerchantlines.Items.Insert(0, new ListItem("", String.Empty));
                ddlMerchantlines.CssClass += " filterable-dropdown";
            }

        }

        private void bindCurrencyDropdown()
        {

            ddlCurrenylines.Items.Clear();

            // Retrieve or cache currency data
            DataTable dtExchangeCode = ViewState["ExchangeCodeData"] as DataTable ?? ControlsHelper.retrieveAllCurrencyDetails();

            if (dtExchangeCode != null && dtExchangeCode.Columns.Contains("CurrencyCode"))
            {
                ViewState["ExchangeCodeData"] = dtExchangeCode;

                ddlCurrenylines.DataSource = dtExchangeCode;
                ddlCurrenylines.DataTextField = "CurrencyCode";
                ddlCurrenylines.DataValueField = "CurrencyCode";
                ddlCurrenylines.DataBind();

                ddlCurrenylines.Items.Insert(0, new ListItem("", String.Empty));
                ddlCurrenylines.CssClass += " filterable-dropdown";
            }

        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool isAnyChecked = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    isAnyChecked = true;
                    Label expenseLineRecId = gridViewRow.FindControl("lblRecId") as Label;
                    if (expenseLineRecId != null)
                        bindExpenseLines(expenseLineRecId.Text);
                    else
                        clearLineDetailsFields();
                    break;
                }
            }
            if (!isAnyChecked)
            {
                clearLineDetailsFields();
            }
        }

        private void clearLineDetailsFields()
        {
            // LEFT COLUMN
            ddlExpenseCategorylines.SelectedIndex = 0;
            txtTransactionDatelines.Text = string.Empty;
            ddlMerchantlines.SelectedIndex = 0;
            ddlPaymentMethodlines.SelectedIndex = 0;
            txtTransactionAmountlines.Text = string.Empty;
            ddlCurrenylines.SelectedIndex = 0;
            txtReceiptNumberlines.Text = string.Empty;
            txtCategorydescriptionlines.Text = string.Empty;
            txtInvoiceNumberlines.Text = string.Empty;
            txtAdditionalInformationlines.Text = string.Empty;
            txtProjectIdlines.SelectedIndex = 0;
            ddlBillablelines.SelectedIndex = 0;
            txtActivityNumberlines.SelectedIndex = 0;
            txtInternalNotelines.Text = string.Empty;
            ddlCountryRegionlines.SelectedIndex = 0;
            ddlStatelines.SelectedIndex = 0;
            ddlCitylines.SelectedIndex = 0;
            ddlZipCodelines.SelectedIndex = 0;
            ddlSalestaxlines.SelectedIndex = 0;
            ddlItemsSalesTaxGrouplines.SelectedIndex = 0;

            // RIGHT COLUMN
            chkSite.Checked = false;
            txtCalculatedsalestaxlines.Text = string.Empty;
            txtActualsalestaxlines.Text = string.Empty;
            txtNetTransactionAmountlines.Text = string.Empty;
            txtReimbursementamount.Text = string.Empty;
            txtReasonlines.Text = string.Empty;
            txtCarRentalCheckOutDatelines.Text = string.Empty;
            txtCheckOutLocationlines.Text = string.Empty;
            txtCarRentalReturnDate.Text = string.Empty;
            txtReturnLocationlines.Text = string.Empty;
            txtRenterNamelines.Text = string.Empty;
            txtRentalReservationNumberlines.Text = string.Empty;
            txtDailyRentalRatelines.Text = string.Empty;
            txtWeeklyRentalRate.Text = string.Empty;
            txtMonthlyRentalRatelines.Text = string.Empty;
            txtTotallines.Text = string.Empty;
            ddlVehicleClasslines.SelectedIndex = 0;

        }

        private void showFinancialDimension(long _defaultDimensionRecId)
        {
            Int64 workerDimension = 0;
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();

            if (workerDimension == 0)
            {
                try
                {
                    DataContract[] dimensions = finDim.retrieveActiveDimensions();

                    if (dimensions != null && dimensions.Length > 0)
                    {
                        // Container where you want to add rows
                        // (put a <div runat="server" id="financialDimensionsContainer"></div> in your .aspx)
                        financialDimensionsContainer.Controls.Clear();

                        int colCount = 0;
                        HtmlGenericControl rowDiv = null;

                        foreach (var dim in dimensions)
                        {
                            if (colCount % 2 == 0)
                            {
                                // Start a new row after every two blocks
                                rowDiv = new HtmlGenericControl("div");
                                rowDiv.Attributes["class"] = "info-row";
                                financialDimensionsContainer.Controls.Add(rowDiv);
                            }

                            // Create info-block
                            HtmlGenericControl blockDiv = new HtmlGenericControl("div");
                            blockDiv.Attributes["class"] = "info-block";

                            // Strong title
                            HtmlGenericControl strong = new HtmlGenericControl("strong");
                            strong.InnerText = dim.Code;   // e.g. Business Unit / Cost Center
                            blockDiv.Controls.Add(strong);

                            // Span wrapper
                            HtmlGenericControl span = new HtmlGenericControl("span");

                            //// Label
                            //Label lbl = new Label();
                            //lbl.ID = $"FinancialDimension{dim.Code}";
                            //lbl.Text = "N/A";
                            //span.Controls.Add(lbl);

                            // Dropdown
                            DropDownList ddl = new DropDownList();
                            ddl.ID = $"ddlFinancialDimension{dim.Code}";
                            ddl.CssClass = "form-control";
                            ddl.Visible = true;

                            // Populate dropdown from SOAP lookup
                            DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                            ddl.Items.Add(new ListItem("", ""));
                            foreach (DataRow row in dimeVal.Rows)
                            {
                                string value = row["Value1"].ToString();
                                string valueName = row["Value2"].ToString();
                                ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                            }

                            span.Controls.Add(ddl);
                            blockDiv.Controls.Add(span);

                            // Add block to row
                            rowDiv.Controls.Add(blockDiv);

                            colCount++;
                        }
                        bindItemDimension(_defaultDimensionRecId);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic dimensions", ex);
                }
            }

        }

        protected void bindItemDimension(long __defaultDimensionRecId)
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();

            long Itemdimension = __defaultDimensionRecId;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(Itemdimension);
            foreach (DataContract dataContract in dataContracts)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                DropDownList ddl = financialDimensionsContainer.FindControl("ddlFinancialDimension" + dimName) as DropDownList;
                TextBox txt = financialDimensionsContainer.FindControl("txt_" + dimName) as TextBox;

                if (ddl != null)
                {
                    // Check if value exists in dropdown items
                    if (!string.IsNullOrEmpty(dimValue) && ddl.Items.FindByValue(dimValue) != null)
                    {
                        ddl.SelectedValue = dimValue;
                        if (txt != null)
                            txt.Text = dimDescription;
                    }

                }
            }
        }

        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {
                ExpenseReport report = new ExpenseReport();
                var expenseReportFields = new List<ExpenseReportRequestSvc> { };

                List<DataContract> dimContracts = new List<DataContract>();
                ESSFinancialDimensions finDim = new ESSFinancialDimensions();

                DataContract[] activeDimensions = finDim.retrieveActiveDimensions();
                foreach (var dim in activeDimensions)
                {
                    DropDownList ddl = financialDimensionsContainer.FindControl("ddlFinancialDimension" + dim.Code) as DropDownList;
                    if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                    {
                        DataContract dc = new DataContract();
                        dc.Code = dim.Code;
                        dc.Value1 = ddl.SelectedValue;
                        dc.Value2 = ddl.SelectedItem.Text; // optional, description
                        dimContracts.Add(dc);
                    }
                }

                long defaultDimensionRecId = 0;
                if (dimContracts.Count > 0)
                {
                    ESSFinancialDimensions newdimension = new ESSFinancialDimensions();
                    defaultDimensionRecId = newdimension.setDimensionValues(dimContracts.ToArray());
                }

                string defaultDimension = defaultDimensionRecId.ToString();
                string expenseReportNumber = Session["ExpenseReportNumber"] as string;

                expenseReportFields.Add(new ExpenseReportRequestSvc
                {
                    __k_parmFieldName = "Dimension",
                    __k_parmFieldValue = defaultDimension,
                    __k_parmFieldType = "Int64"
                });

                report.updateExpenseReport(expenseReportFields, expenseReportNumber);

                Response.Redirect(Request.RawUrl);
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