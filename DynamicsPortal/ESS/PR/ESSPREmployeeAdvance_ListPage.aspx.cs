using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeAdvanceRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeAdvance_ListPage : MainForm
    {
        private PREmployeeAdvances pREmployeeAdvances = new PREmployeeAdvances();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeAdvances.tableName;
                pageMenuId = "ESSPREmployeeAdvanceHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                // [COMMENTED] Populate Advance Type JSON for the inline-new-row dropdown
                // loadAdvanceTypesJson();

                if (!IsPostBack)
                {
                    reBindGrid();
                }

                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();
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
        private void getGridDataTable()
        {
            DataTable dt = pREmployeeAdvances.retriveEmployeeReportees();
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();

            if (gridView.HeaderRow != null)
            {
                gridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gridView.BottomPagerRow != null)
            {
                gridView.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            bindGrid();
        }

        public void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gridView.PageSize = int.Parse(ddlPageSize.SelectedValue);
            //gridView.PageIndex = 0;
            //bindGrid();
        }

        //protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gridView.EditIndex = e.NewEditIndex;
        //    bindGrid();
        //}

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = pREmployeeAdvances.createDataTable();
                    DataRow dr = dataTable.NewRow();

                    // Use safer FindControl with null checks
                    TextBox txtDesc = gridViewRow.FindControl("txtAdvanceDescription") as TextBox;
                    TextBox txtAmt = gridViewRow.FindControl("txtAdvanceAmount") as TextBox;
                    DropDownList ddlType = gridViewRow.FindControl("ddlAdvanceTypeCode") as DropDownList;

                    dr["AdvanceDescription"] = txtDesc != null ? txtDesc.Text : "";
                    dr["AdvanceAmount"] = txtAmt != null ? txtAmt.Text : "0";
                    dr["AdvanceTypeCode"] = ddlType != null ? ddlType.SelectedValue : "";

                    string guarantor1 = (gridViewRow.FindControl("txtGuarantor1") as TextBox)?.Text ?? "";
                    string guarantor2 = (gridViewRow.FindControl("txtGuarantor2") as TextBox)?.Text ?? "";
                    dr["Guarantor1"] = guarantor1;
                    dr["Guarantor2"] = guarantor2;

                    dr["RequestedPaymentDate"] = (gridViewRow.FindControl("txtRequestedPaymentDate") as TextBox)?.Text ??
                                               ((Label)gridViewRow.FindControl("lblRequestedPaymentDate"))?.Text ?? "";

                    dr["RequestDate"] = (gridViewRow.FindControl("txtRequestDate") as TextBox)?.Text ??
                                      ((Label)gridViewRow.FindControl("lblRequestDate"))?.Text ?? "";

                    dr["RecoveryStartDate"] = (gridViewRow.FindControl("txtRecoveryStartDate") as TextBox)?.Text ??
                                            ((Label)gridViewRow.FindControl("lblRecoveryStartDate"))?.Text ?? "";

                    dr["RequestedInstallments"] = (gridViewRow.FindControl("txtRecoveries") as TextBox)?.Text ??
                                                ((Label)gridViewRow.FindControl("lblRecoveries"))?.Text ?? "";

                    dr["RequestedInstallmentAmount"] = (gridViewRow.FindControl("txtAmountRecovered") as TextBox)?.Text ??
                                                     ((Label)gridViewRow.FindControl("lblAmountRecovered"))?.Text ?? "";

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    if (gridView.DataKeys[gridViewRow.RowIndex] != null)
                    {
                        Int64.TryParse(gridView.DataKeys[gridViewRow.RowIndex].Value.ToString(), out recId);
                    }

                    SysOperationResult_BOL operationResult_BOL = pREmployeeAdvances.update(dataTable, recId);
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
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            bindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    long recId = 0;
                    // [FIX] Use DataKeys instead of FindControl/Label to be safe in all row states
                    if (gridView.DataKeys[gridViewRow.RowIndex] != null)
                    {
                        Int64.TryParse(gridView.DataKeys[gridViewRow.RowIndex].Value.ToString(), out recId);
                    }

                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = pREmployeeAdvances.delete(recordsId.ToArray());
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
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlAdvanceTypeCode =
                    e.Row.FindControl("ddlAdvanceTypeCode") as DropDownList;

                DataRowView drv = e.Row.DataItem as DataRowView;

                ddlAdvanceTypeCode.DataSource = ControlsHelper.retrieveAllPRAdvanceTypes();
                ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
                ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
                ddlAdvanceTypeCode.DataBind();

                string advanceTypeCode = Convert.ToString(drv["AdvanceTypeCode"]);

                ListItem item = ddlAdvanceTypeCode.Items.FindByValue(advanceTypeCode);
                if (item != null)
                {
                    ddlAdvanceTypeCode.ClearSelection();
                    item.Selected = true;

                    if (advanceTypeCode == "Advance Salary")
                        ddlAdvanceTypeCode.Enabled = false;
                    else
                    {
                        ListItem item1 = ddlAdvanceTypeCode.Items.FindByValue("Advance Salary");
                        if(!string.IsNullOrEmpty(item1?.Text))
                          ddlAdvanceTypeCode.Items.Remove(item1);
                    }
                }

                TextBox txtAmountRecovered = e.Row.FindControl("txtAmountRecovered") as TextBox;
                TextBox txtRecoveries = e.Row.FindControl("txtRecoveries") as TextBox;

                if (txtAmountRecovered != null)
                    txtAmountRecovered.Enabled = false;

                if (txtRecoveries != null)
                    txtRecoveries.Enabled = false;

                TextBox txtRequestedPaymentDate = (TextBox)e.Row.FindControl("txtRequestedPaymentDate");

                if (txtRequestedPaymentDate != null)
                {
                    DateTime requestedPaymentDate =
                        Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RequestedPaymentDate"));

                    txtRequestedPaymentDate.Text =
                        requestedPaymentDate.ToString("yyyy-MM-dd");
                }
            }

            string[] validStatus = new string[] { PRWFStatus.NotSubmitted.ToString() };
            // [RESOLVED] Added null check to prevent crash during edit mode when lblWFStatus might be missing
            if (gridViewRow.FindControl("lblWFStatus") != null)
            {
                controlsHelper.checkWFStatus(gridViewRow, "lblWFStatus", validStatus);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    var rowData = new System.Collections.Generic.Dictionary<string, string>();
                    foreach (DataColumn col in drv.Row.Table.Columns)
                    {
                        rowData[col.ColumnName] = drv[col.ColumnName]?.ToString() ?? "";
                    }
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer()
                                      .Serialize(rowData);
                    e.Row.Attributes["data-rowjson"] = json;
                }
            }

            Label lblRequestDate = e.Row.FindControl("lblRequestDate") as Label;

            if (lblRequestDate != null && !string.IsNullOrWhiteSpace(lblRequestDate.Text))
            {
                DateTime requestdate;

                if (DateTime.TryParse(lblRequestDate.Text, out requestdate))
                {
                    lblRequestDate.Text = requestdate.ToString("dd/M/yyyy");
                }
            }


            Label lblRequestedPaymentDate = e.Row.FindControl("lblRequestedPaymentDate") as Label;

            if (lblRequestedPaymentDate != null && !string.IsNullOrWhiteSpace(lblRequestedPaymentDate.Text))
            {
                DateTime requestpaymentdate;

                if (DateTime.TryParse(lblRequestedPaymentDate.Text, out requestpaymentdate))
                {
                    lblRequestedPaymentDate.Text = requestpaymentdate.ToString("dd/M/yyyy");
                }
            }


            Label lblRecoveryStartDate = e.Row.FindControl("lblRecoveryStartDate") as Label;

            if (lblRecoveryStartDate != null && !string.IsNullOrWhiteSpace(lblRecoveryStartDate.Text))
            {
                DateTime recoverystartdate;

                if (DateTime.TryParse(lblRecoveryStartDate.Text, out recoverystartdate))
                {
                    lblRecoveryStartDate.Text = recoverystartdate.ToString("dd/M/yyyy");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lblReqId = gridViewRow.FindControl("lblAdvanceRequestId") as Label;
                    if (lblReqId != null)
                    {
                        string requestId = lblReqId.Text;
                        if (!string.IsNullOrEmpty(requestId))
                        {
                            recordsId.Add(requestId);
                        }
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeAdvanceRequests_Submit(recordsId.ToArray());
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

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }


        /*
        // ── Inline New Row: populate advance-type dropdown data (COMMENTED OUT) ──
        private void loadAdvanceTypesJson()
        {
            try
            {
                DataTable dtAdvTypes = ControlsHelper.retrieveAllPRAdvanceTypes();
                if (dtAdvTypes == null || dtAdvTypes.Rows.Count == 0) return;

                System.Text.StringBuilder sb = new System.Text.StringBuilder("[");
                foreach (DataRow r in dtAdvTypes.Rows)
                    sb.Append("{\"Code\":\"").Append(r["AdvanceTypeCode"].ToString().Replace("\"", "\\\"")).Append("\"},");

                if (sb.Length > 1) sb.Length--;
                sb.Append("]");
                hdnAdvTypesJson.Value = sb.ToString();
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                objErrorLog.write(currentMethod.DeclaringType.FullName, ex);
            }
            finally { }
        }

        // ── Inline New Row: save handler (COMMENTED OUT) ─────────────────────
        protected void btnSaveNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnNR_Save.Value != "true") return;
                hdnNR_Save.Value = string.Empty;

                string personalNumber = SessionVariables.getCurrentEmployeeId();
                long employeeId = ControlsHelper.getWorkerId(personalNumber);

                DataTable dataTable = pREmployeeAdvances.createDataTable();
                DataRow dr = dataTable.NewRow();

                dr["EmployeeId"]             = employeeId;
                dr["RequestDate"]            = DateTime.Now.ToString("dd/MM/yyyy");
                dr["RequestedPaymentDate"]   = hdnNR_PayDate.Value;
                dr["RecoveryStartDate"]      = hdnNR_RecStartDate.Value;
                dr["AdvanceTypeCode"]        = hdnNR_AdvTypeCode.Value;
                dr["AdvanceDescription"]     = hdnNR_Description.Value;
                dr["AdvanceAmount"]          = hdnNR_Amount.Value;
                dr["Guarantor1"]             = hdnNR_Guarantor1.Value;
                dr["Guarantor2"]             = hdnNR_Guarantor2.Value;
                dr["Currency"]               = ControlsHelper.getEmployeeCurrencyCode(personalNumber);

                decimal installments = 0, advAmount = 0;
                decimal.TryParse(hdnNR_Recoveries.Value, out installments);
                decimal.TryParse(hdnNR_Amount.Value, out advAmount);

                dr["RequestedInstallments"] = installments;
                if (installments > 0)
                {
                    dr["RequestedInstallmentAmount"] = Math.Round(advAmount / installments, 2);
                }
                else
                {
                    dr["RequestedInstallmentAmount"] = 0;
                }


                dataTable.Rows.Add(dr);

                SysOperationResult_BOL createResult = pREmployeeAdvances.create(dataTable);
                bool success = operationResults(createResult);

                if (success) { gridView.EditIndex = -1; reBindGrid(); }
                else { bindGrid(); }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally { }
        }
        */

        // Helper to convert "AdvanceRequestId" → "Advance Request Id"
        private static string FormatLabel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;
            // Fix: insert space before capitals that follow a lowercase letter
            // prevents leading space on first capital
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
            // Instantiate locally since WebMethod must be static
            PREmployeeAdvances advances = new PREmployeeAdvances();
            DataTable dt = advances.retriveEmployeeReportees();

            var columns = dt.Columns
                .Cast<DataColumn>()
                .Select(col => new
                {
                    Field = col.ColumnName,
                    Label = FormatLabel(col.ColumnName),
                    Visible = true
                })
                .ToList();

            return new JavaScriptSerializer().Serialize(columns);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

            DataTable dt = SessionVariables.getSessionDataTable();
            DataRow dr = dt.Rows[row.RowIndex];
            Session["RecordToEdit"] = dr;

            bindGrid();

            if (dr["AdvanceTypeCode"].ToString() == "Advance Salary")
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "OpenModal",
                    "openPopupPanel('/ESS/PR/ESSPREmployeeAdvance_Edit.aspx');",
                    true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "OpenModal",
                    "openPopupPanel('/ESS/PR/ESSPREmployeeLoanReq_Edit.aspx');",
                    true);
            }
        }
    }
}