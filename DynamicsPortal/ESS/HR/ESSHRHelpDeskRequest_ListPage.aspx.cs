using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSHRHelpDeskRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRHelpDeskRequest_ListPage : MainForm
    {
        private ESSHRHelpDeskRequest eSSHRHelpDeskRequest = new ESSHRHelpDeskRequest();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = eSSHRHelpDeskRequest.tableName;
                pageMenuId = "ESSHRHelpDeskRequestHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
                }

                if (IsPostBack && Request["__EVENTTARGET"] == "RefreshGrid")
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
            DataTable dt = eSSHRHelpDeskRequest.retriveEmployeeReportees();
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        //protected void bindGrid()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    gridView.DataSource = dt;
        //    gridView.DataBind();
        //}

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt == null)
            {
                getGridDataTable();
                dt = SessionVariables.getSessionDataTable();
            }
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
                    //eSSHRHelpDeskRequestSvcContract.Detail;               //- CU
                    //eSSHRHelpDeskRequestSvcContract.ESSDate;              //- CU
                    //eSSHRHelpDeskRequestSvcContract.ESSTypeOfProblem;     //- CU
                    DataTable dataTable = eSSHRHelpDeskRequest.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["Detail"] = (gridViewRow.FindControl("txtDetail") as TextBox).Text;
                    dr["ESSDate"] = (gridViewRow.FindControl("lblESSDate") as Label).Text;
                    dr["ESSTypeOfProblem"] = (gridViewRow.FindControl("ddlESSTypeOfProblem") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = eSSHRHelpDeskRequest.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = eSSHRHelpDeskRequest.delete(recordsId.ToArray());
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
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            Label lblESSDate = e.Row.FindControl("lblESSDate") as Label;

            if (lblESSDate != null && !string.IsNullOrWhiteSpace(lblESSDate.Text))
            {
                DateTime date;

                if (DateTime.TryParse(lblESSDate.Text, out date))
                {
                    lblESSDate.Text = date.ToString("dd/M/yyyy");
                }
            }

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataTable dt = eSSHRHelpDeskRequest.retrieveTypesofProblems("");
                DropDownList ddlESSTypeOfProblem = e.Row.FindControl("ddlESSTypeOfProblem") as DropDownList;
                ddlESSTypeOfProblem.DataSource = dt;
                ddlESSTypeOfProblem.DataValueField = "ESSTypeOfProblem";
                ddlESSTypeOfProblem.DataTextField = "ESSTypeOfProblem";
                ddlESSTypeOfProblem.DataBind();

                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                ddlESSTypeOfProblem.SelectedValue = dataRowView["ESSTypeOfProblem"].ToString();
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

            string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
            controlsHelper.checkWFStatus(gridViewRow, "lblESSWorkFlowStatus", validStatus);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblESSHRHelpDeskRequestId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hRHelpDeskRequest_Submit(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    reBindGrid();
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

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            int checkedCount = 0;
            bool allDraft = true;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chk = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chk == null || !chk.Checked) continue;

                checkedCount++;

                Label lblStatus = gridViewRow.FindControl("lblESSWorkFlowStatus") as Label;
                if (lblStatus == null || (lblStatus.Text.Trim() != "Draft" && lblStatus.Text.Trim() != "NotSubmitted"))
                    allDraft = false;
            }

            btnSubmit.Enabled = (checkedCount == 1);           // exactly 1 record
            btnDelete.Enabled = (checkedCount > 0 && allDraft); // all selected must be Draft
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
            ESSHRHelpDeskRequest eSSHRHelpDeskRequest = new ESSHRHelpDeskRequest();
            DataTable dt = eSSHRHelpDeskRequest.retriveEmployeeReportees();
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
