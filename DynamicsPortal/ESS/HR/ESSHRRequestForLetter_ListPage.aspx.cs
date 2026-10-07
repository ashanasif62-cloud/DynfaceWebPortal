using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSEmploymentCertificateSvcReference;
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
    public partial class ESSHRRequestForLetter_ListPage : MainForm
    {
        private HRRequestForLetters hRRequestForLetters = new HRRequestForLetters();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hRRequestForLetters.tableName;
                pageMenuId = "ESSHREmployeeLettersHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

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
            DataTable dt = hRRequestForLetters.retriveEmployeeReportees();
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
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = hRRequestForLetters.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["Remarks"] = (gridViewRow.FindControl("txtRemarks") as TextBox).Text;

                    dr["ReqestedFor"] = (gridViewRow.FindControl("ddlRequestedFor") as DropDownList).SelectedValue;
                    dr["Reason"] = (gridViewRow.FindControl("ddlReason") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = hRRequestForLetters.update(dataTable, recId);
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
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                Session["SelectedRecIds"] = recordsId;
                SysOperationResult_BOL operationResult_BOL = hRRequestForLetters.delete(recordsId.ToArray());
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // ✅ Store RecId
                    string recordsId = (row.FindControl("lblRecId") as Label)?.Text.Trim();
                    Session["SelectedRecIds"] = recordsId;

                    // ✅ Store CertificateTypeCode
                    Label lblCertificateType = row.FindControl("lblCertificateType") as Label;
                    if (lblCertificateType != null)
                    {
                        Session["CertificateTypeCode"] = lblCertificateType.Text.Trim();
                    }

                    // ✅ Store ReqestedFor
                    Label lblReqestedFor = row.FindControl("lblReqestedFor") as Label;
                    if (lblReqestedFor != null)
                    {
                        Session["ReqestedFor"] = lblReqestedFor.Text.Trim();
                    }

                    // ✅ Store Remarks
                    Label lblRemarks = row.FindControl("lblRemarks") as Label;
                    if (lblRemarks != null)
                    {
                        Session["Remarks"] = lblRemarks.Text.Trim();
                    }

                    isSelected = true;

                    // ✅ Open popup after setting session values
                    string script = "openPopupPanel('/ESS/HR/ESSRequestForLetter_Edit.aspx', 700);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupOnHand", script, true);
                    return; // Stop after first selected record
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('No record was selected');", true);
            }
        }







        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            Label lblRequestedDate = gridViewRow.FindControl("lblRequestedDate") as Label;

            if (lblRequestedDate != null && !string.IsNullOrWhiteSpace(lblRequestedDate.Text))
            {
                DateTime requestedDate;

                if (DateTime.TryParse(lblRequestedDate.Text, out requestedDate))
                {
                    lblRequestedDate.Text = requestedDate.ToString("dd/M/yyyy");
                }
            }


            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlRequestedFor = gridViewRow.FindControl("ddlRequestedFor") as DropDownList;
                DataTable dtRequestedFor = ControlsHelper.retrieveAllESSRequestedFor();
                ddlRequestedFor.DataSource = dtRequestedFor;
                ddlRequestedFor.DataTextField = "RequestedForId";
                ddlRequestedFor.DataValueField = "RequestedForId";
                ddlRequestedFor.DataBind();

                DropDownList ddlReasonCode = gridViewRow.FindControl("ddlReason") as DropDownList;
                DataTable dtReasonCode = ControlsHelper.retrieveAllHcmReasonCode();
                ddlReasonCode.DataSource = dtReasonCode;
                ddlReasonCode.DataTextField = "Description";
                ddlReasonCode.DataValueField = "ReasonCodeId";
                ddlReasonCode.DataBind();

                ddlReasonCode.SelectedValue = dataRowView["Reason"].ToString();
                ddlRequestedFor.SelectedValue = dataRowView["ReqestedFor"].ToString();
            }

            string[] validStatus = new string[] { ESSWorkFlowStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWorkflowState", validStatus);

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
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblCertificateId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hREmploymentCertificateRequest_Submit(recordsId.ToArray());
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
            HRRequestForLetters hRRequestForLetters = new HRRequestForLetters();
            DataTable dt = hRRequestForLetters.retriveEmployeeReportees();

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

    }
}