using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.ExpenseManagmentSvc;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.EM
{
    public partial class EMExpenseReportCreate : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ExpenseReport_Create";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;
                if (!isPageAuthorizated)
                    return;

                showFinancialDimension();
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
            DataTable dtPurpose = ControlsHelper.getPurpose();
            ddlPurpose.DataSource = dtPurpose;
            ddlPurpose.DataTextField = "Purpose";
            ddlPurpose.DataValueField = "Purpose";
            ddlPurpose.DataBind();

            ddlPurpose.Items.Insert(0, new ListItem("", ""));

            DataTable dtLocation = ControlsHelper.getLocation();
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "Locations";
            ddlLocation.DataValueField = "Locations";
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem("", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExpenseReport report = new ExpenseReport();
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();
            try
            {
                var finDimFields = new List<DataContract> {};
                DataContract[] activeDimension = finDim.retrieveActiveDimensions();
                foreach (var dim in activeDimension)
                {
                    string ddlId = "ddl_" + dim.Code;
                    string textId = "txt_" + dim.Code;
                    DropDownList ddlCtrl = expenseReportCreatetable.FindControl(ddlId) as DropDownList;
                    TextBox textCtrl = expenseReportCreatetable.FindControl(textId) as TextBox;
                    if (ddlCtrl != null)
                    {
                        finDimFields.Add(new DataContract
                        {
                            Code = dim.Code,
                            Value1 = ddlCtrl.SelectedValue,
                            Value2 = textCtrl.Text
                        });
                    }
                }

                long finDimId = finDim.setDimensionValues(finDimFields.ToArray());

                var expenseReportFields = new List<ExpenseReportRequestSvc> {};

                if (ddlPurpose.SelectedValue != "")
                {
                    expenseReportFields.Add(new ExpenseReportRequestSvc {
                        __k_parmFieldName = "Purpose",
                        __k_parmFieldValue = ddlPurpose.SelectedValue,
                        __k_parmFieldType = "String"
                    });
                }

                if (ddlLocation.SelectedValue != "")
                {
                    expenseReportFields.Add(new ExpenseReportRequestSvc
                    {
                        __k_parmFieldName = "Location",
                        __k_parmFieldValue = ddlLocation.SelectedValue,
                        __k_parmFieldType = "String"
                    });
                }

                //expenseReportFields.Add(new ExpenseReportRequestSvc
                //{
                //    __k_parmFieldName = "DefaultDimension",
                //    __k_parmFieldValue = finDimId.ToString(),
                //    __k_parmFieldType = "Int64"
                //});

                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result = report.createExpenseReport(expenseReportFields);

                NotificationMessage.showMessage(result);

                if (result.isSuccess)
                {
                    string script = @"
        setTimeout(function() { 
            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                window.parent.refreshParentGrid();
            }
            closeDialog();
        }, 3000);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
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

        private void showFinancialDimension()
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
                        foreach (var dim in dimensions)
                        {
                            // Create label row
                            HtmlTableRow labelRow = new HtmlTableRow();
                            HtmlTableCell labelCell = new HtmlTableCell();
                            labelCell.ColSpan = 2;
                            labelCell.InnerHtml = $"<span>{dim.Code}</span>";
                            labelRow.Cells.Add(labelCell);
                            expenseReportCreatetable.Rows.Add(labelRow);

                            // Create input row
                            HtmlTableRow inputRow = new HtmlTableRow();

                            // Dropdown cell
                            HtmlTableCell ddlCell = new HtmlTableCell();
                            DropDownList ddl = new DropDownList();
                            ddl.ID = $"ddl_{dim.Code}";
                            ddl.CssClass = "form-control";

                            // Get dropdown options from SOAP
                            DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                            ddl.Items.Add(new ListItem("", ""));
                            foreach (DataRow row in dimeVal.Rows)
                            {
                                string value = row["Value1"].ToString(); // or use appropriate column name
                                string valueName = row["Value2"].ToString();
                                ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                                ddl.AutoPostBack = true;
                                ddl.SelectedIndexChanged += new EventHandler(DimensionChanged);
                            }

                            ddlCell.Controls.Add(ddl);

                            // TextBox cell
                            HtmlTableCell txtCell = new HtmlTableCell();
                            TextBox txt = new TextBox();
                            txt.ID = $"txt_{dim.Code}";
                            txt.CssClass = "form-control";
                            txt.Enabled = false;
                            txtCell.Controls.Add(txt);

                            inputRow.Cells.Add(ddlCell);
                            inputRow.Cells.Add(txtCell);

                            expenseReportCreatetable.Rows.Add(inputRow);
                        }

                        bindEmployeeDimension();
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic table", ex);
                }
            }
        }

        protected void bindEmployeeDimension()
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();
            PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            PREmploymentInformationSvcContract pREmploymentInformationSvcContract = pREmploymentInformation.retrieveEmployeeDetails(SessionVariables.getCurrentEmployeeId());

            long empDimension = pREmploymentInformationSvcContract.Dimension;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(empDimension);
            foreach (DataContract dataContract in dataContracts)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                DropDownList ddl = expenseReportCreatetable.FindControl("ddl_" + dimName) as DropDownList;
                TextBox txt = expenseReportCreatetable.FindControl("txt_" + dimName) as TextBox;

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

        protected void DimensionChanged(object sender, EventArgs e)
        {
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();
            DropDownList ddl = sender as DropDownList;
            if (ddl != null)
            {
                string selectedValue = ddl.SelectedValue;
                string dimensionCode = ddl.ID.Replace("ddl_", "");

                // Example: Fetch related info based on selectedValue
                DataTable details = finDim.retrieveDimensionLookUp(dimensionCode);

                foreach (DataRow row in details.Rows)
                {
                    string value = row["Value1"].ToString(); // or use appropriate column name
                    if (value == selectedValue)
                    {
                        string newText = row["Value2"].ToString();
                        string textBoxId = "txt_" + dimensionCode;
                        Control ctrl = expenseReportCreatetable.FindControl(textBoxId);
                        if (ctrl != null && ctrl is TextBox txtBox)
                        {
                            txtBox.Text = newText;
                        }
                    }
                }
            }
        }

    }
}