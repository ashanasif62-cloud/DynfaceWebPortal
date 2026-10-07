using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeavesSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLeave_ListPage : MainForm
    {
        private PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeLeaveRequest.tableName;
                pageMenuId = "ESSPREmployeeLeaveHistory";

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
            DataTable dt = pREmployeeLeaveRequest.retriveEmployeeReportees();
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
        //protected void Update_Click(object sender, EventArgs e)
        //{
        //    GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
        //    if (gridViewRow.RowType == DataControlRowType.DataRow)
        //    {
        //        if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
        //        {
        //            DataTable dataTable = pREmployeeLeaveRequest.createDataTable();
        //            DataRow dr = dataTable.NewRow();
        //            dr["LeaveCategory"] = ((DropDownList)gridViewRow.FindControl("ddlLeaveCategory")).SelectedValue;
        //            dr["LeaveCode"] = ((DropDownList)gridViewRow.FindControl("ddlLeaveCode")).SelectedValue;

        //            dr["LeaveDays"] = ((TextBox)gridViewRow.FindControl("txtLeaveDays")).Text;
        //            dr["LeaveReqDate"] = ((TextBox)gridViewRow.FindControl("txtLeaveReqDate")).Text;
        //            dr["LeaveStartDate"] = ((TextBox)gridViewRow.FindControl("txtLeaveStartDate")).Text;
        //            dr["Reason"] = ((TextBox)gridViewRow.FindControl("txtReason")).Text;

        //            long recId = 0;
        //            Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
        //            dataTable.Rows.Add(dr);

        //            SysOperationResult_BOL operationResult_BOL = pREmployeeLeaveRequest.update(dataTable, recId);
        //            bool result = operationResults(operationResult_BOL);
        //            if (result)
        //            {
        //                gridView.EditIndex = -1;
        //                reBindGrid();
        //            }
        //            else
        //            {
        //                bindGrid();
        //            }
        //        }
        //    }
        //}

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
                SysOperationResult_BOL operationResult_BOL = pREmployeeLeaveRequest.delete(recordsId.ToArray());
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

        protected string FormatLeaveTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            string timeValue = value.ToString().Trim();

            if (string.IsNullOrEmpty(timeValue))
                return string.Empty;

            DateTime time;

            if (DateTime.TryParse(timeValue, out time))
            {
                return time.ToString("hh:mm:ss tt").ToLower();
            }

            return string.Empty;
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;

            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
                DropDownList ddlLeaveCategory = gridViewRow.FindControl("ddlLeaveCategory") as DropDownList;
                ddlLeaveCategory.DataSource = Enum.GetNames(typeof(PRLeaveCategory));
                ddlLeaveCategory.DataBind();

                DropDownList ddlLeaveCode = gridViewRow.FindControl("ddlLeaveCode") as DropDownList;
                ddlLeaveCode.DataSource = ControlsHelper.retrieveAllPRLeaveCodes();
                ddlLeaveCode.DataTextField = "LeaveCode";
                ddlLeaveCode.DataValueField = "LeaveCode";
                ddlLeaveCode.DataBind();

                ddlLeaveCategory.SelectedValue = dataRowView["LeaveCategory"].ToString().Replace(" ", "");
                ddlLeaveCode.SelectedValue = dataRowView["LeaveCode"].ToString();
            }

            // ✅ Format date labels
            FormatDateLabel(gridViewRow, "lblLeaveReqDate");
            FormatDateLabel(gridViewRow, "lblLeaveStartDate");
            FormatDateLabel(gridViewRow, "lblLeaveEndDate");

            string[] validStatus = new string[] { PRWFStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWFStatus", validStatus);

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

        private void FormatDateLabel(GridViewRow row, string controlId)
        {
            Label lbl = row.FindControl(controlId) as Label;
            if (lbl != null && DateTime.TryParse(lbl.Text, out DateTime parsedDate))
            {
                lbl.Text = parsedDate.ToString("dd/M/yyyy");
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
                    string leaveRequestId = (gridViewRow.FindControl("lblLeaveReqId") as Label).Text;
                    if (!string.IsNullOrEmpty(leaveRequestId))
                    {
                        recordsId.Add(leaveRequestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeLeaveRequests_Submit(recordsId.ToArray());
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //List<string> selectedRecId = new List<string>();
            long selectedRecId = 0;
            string LeaveDays = string.Empty;
            string leaveReqId = string.Empty;
            string employeeId = string.Empty;
            DateTime leaveStartDate = DateTime.MinValue;
            DateTime leaveEndDate = DateTime.MinValue;
            DateTime leaveReqDate = DateTime.MinValue;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lblRecId = gridViewRow.FindControl("lblRecId") as Label;
                    Label lblLeaveDays = gridViewRow.FindControl("lblLeaveDays") as Label;
                    Label lblLeaveStartDate = gridViewRow.FindControl("lblLeaveStartDate") as Label;
                    Label lblLeaveEndDate = gridViewRow.FindControl("lblLeaveEndDate") as Label;
                    Label lblLeaveReqId = gridViewRow.FindControl("lblLeaveReqId") as Label;
                    Label lblEmployeeId = gridViewRow.FindControl("lblEmployeeId") as Label;
                    Label lblLeaveReqDate = gridViewRow.FindControl("lblLeaveReqDate") as Label;

                    if (lblRecId != null)
                    {
                        long.TryParse(lblRecId.Text, out selectedRecId);
                    }

                    if (lblLeaveDays != null)
                    {
                        LeaveDays = lblLeaveDays.Text;
                    }

                    if (lblLeaveReqId != null)
                    {
                        leaveReqId = lblLeaveReqId.Text;
                    }

                    if (lblEmployeeId != null)
                    {
                        employeeId = lblEmployeeId.Text;
                    }

                    if (lblLeaveStartDate != null)
                    {
                        DateTime.TryParse(lblLeaveStartDate.Text, out leaveStartDate);
                    }

                    if (lblLeaveEndDate != null)
                    {
                        DateTime.TryParse(lblLeaveEndDate.Text, out leaveEndDate);
                    }

                    if (lblLeaveReqDate != null)
                    {
                        DateTime.TryParse(lblLeaveReqDate.Text, out leaveReqDate);
                    }

                    break;
                }
            }

            if (selectedRecId > 0)
            {
                Session["RecId"] = selectedRecId;
                Session["LeaveDays"] = LeaveDays;
                Session["LeaveReqId"] = leaveReqId;
                Session["EmployeeId"] = employeeId;
                Session["LeaveStartDate"] = leaveStartDate.ToString("yyyy-MM-dd");
                Session["LeaveEndDate"] = leaveEndDate.ToString("yyyy-MM-dd");
                Session["LeaveReqDate"] = leaveReqDate.ToString("yyyy-MM-dd");

                string script = "openPopupPanel('/ESS/PR/ESSPREmployeeLeave_Edit.aspx');";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "OpenPopup",
                    script,
                    true
                );
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                    "alert('Please select at least one record to proceed.');", true);
            }
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
            PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();
            DataTable dt = pREmployeeLeaveRequest.retriveEmployeeReportees();

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