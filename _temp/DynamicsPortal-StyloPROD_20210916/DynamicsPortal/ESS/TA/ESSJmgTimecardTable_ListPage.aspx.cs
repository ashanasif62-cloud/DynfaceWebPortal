using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.JmgTimecardTableSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSJmgTimecardTable_ListPage : MainForm
    {
        private JmgTimecardTable timecardTable = new JmgTimecardTable();
        private HcmReasonCode hcmReasonCode = new HcmReasonCode();
        private JmgAttendanceIntegration jmgAttendanceIntegration = new JmgAttendanceIntegration();


        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = timecardTable.tableName;
                pageMenuId = "ESSJmgTimecardTableHistory";
                //showActionPanel = false;

                //txtFromDate.Text = (DateTime.Now.AddDays(-7)).ToShortDateString();
                //txtToDate.Text = DateTime.Now.ToShortDateString();

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
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

        protected void Date_TextChanged(object sender, EventArgs e)
        {
            reBindGrid();
        }

        protected void reBindGrid()
        {
            getGridData();
            bindGrid();
        }

        private string getQuery_EmployeeId()
        {
            string employeeId = string.Empty;
            if (string.IsNullOrEmpty(Request.QueryString["EmpId"]))
            {
                employeeId = SessionVariables.getCurrentEmployeeId();
            }
            else
            {
                employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            }
            return employeeId;
        }

        private void getGridData()
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string employeeId = getQuery_EmployeeId();
            DataTable dt = timecardTable.createDataTable();

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime fromDateTime = fromDate.toDateTime();
                DateTime toDateTime = toDate.toDateTime();

                dt = timecardTable.retrieveSingleLine(employeeId, fromDateTime, toDateTime);
            }
            SessionVariables.setSessionDataTable(dt);
        }

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView_TimecardTable.DataSource = dt;
            gridView_TimecardTable.DataBind();
        }

        protected void gridView_TimecardTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string selectedRecId;
            GridViewRow gridViewRow = e.Row;

            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
            //selectedRecId = dataRowView["RecId"].ToString();
            //gridViewRow.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataTable dtReasonCode = hcmReasonCode.retrieveAllHcmReasonCode();

                DropDownList ddlClockInReason = gridViewRow.FindControl("ddlClockInReasonCodeId") as DropDownList;
                ddlClockInReason.DataSource = dtReasonCode;
                ddlClockInReason.DataTextField = "ReasonCodeId";
                ddlClockInReason.DataValueField = "ReasonCodeId";
                ddlClockInReason.DataBind();
                ddlClockInReason.SelectedValue = dataRowView["ClockInReasonCodeId"].ToString();

                DropDownList ddlClockOutReason = gridViewRow.FindControl("ddlClockOutReasonCodeId") as DropDownList;
                ddlClockOutReason.DataSource = dtReasonCode;
                ddlClockOutReason.DataTextField = "ReasonCodeId";
                ddlClockOutReason.DataValueField = "ReasonCodeId";
                ddlClockOutReason.DataBind();
                ddlClockOutReason.SelectedValue = dataRowView["ClockOutReasonCodeId"].ToString();

                string profileDate = setDateTimeFormat(dataRowView["ProfileDate"].ToString());
                if (!string.IsNullOrEmpty(profileDate))
                {
                    string clockInDatetime = setDateTimeFormat(dataRowView["ClockInDateTime"].ToString());
                    string clockOutDatetime = setDateTimeFormat(dataRowView["ClockOutDateTime"].ToString());

                    if (string.IsNullOrEmpty(clockInDatetime))
                    {
                        TextBox txtClockInDateTime = gridViewRow.FindControl("txtClockInDateTime") as TextBox;
                        txtClockInDateTime.Text = profileDate;
                    }

                    if (string.IsNullOrEmpty(clockOutDatetime))
                    {
                        TextBox txtClockOutDateTime = gridViewRow.FindControl("txtClockOutDateTime") as TextBox;
                        txtClockOutDateTime.Text = profileDate;
                    }
                }
            }

            checkTimecardTable_status(gridViewRow, true);

        }

        protected void gridView_TimecardTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView_TimecardTable.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void Update_TimecardTable_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    bool result;
                    SysOperationResult_BOL operationResult_BOL;
                    DataTable dataTable = timecardTable.createDataTable();

                    DataRow dr = dataTable.NewRow();
                    dr["EmployeeId"] = (gridViewRow.FindControl("lblEmployeeId") as Label).Text;
                    dr["Remarks"] = (gridViewRow.FindControl("txtRemarks") as TextBox).Text;
                    dr["ProfileId"] = (gridViewRow.FindControl("lblProfileId") as Label).Text;
                    dr["RecId"] = (gridViewRow.FindControl("lblRecId") as Label).Text;

                    dr["ClockInDateTime"] = (gridViewRow.FindControl("txtClockInDateTime") as TextBox).Text;
                    dr["ClockInReasonCodeId"] = (gridViewRow.FindControl("ddlClockInReasonCodeId") as DropDownList).Text;
                    dr["ClockInRecId"] = (gridViewRow.FindControl("lblClockInRecId") as Label).Text;

                    dr["ClockOutDateTime"] = (gridViewRow.FindControl("txtClockOutDateTime") as TextBox).Text;
                    dr["ClockOutReasonCodeId"] = (gridViewRow.FindControl("ddlClockOutReasonCodeId") as DropDownList).Text;
                    dr["ClockOutRecId"] = (gridViewRow.FindControl("lblClockOutRecId") as Label).Text;

                    dataTable.Rows.Add(dr);

                    //int rowIndex = gridViewRow.RowIndex;
                    //long recId = 0;
                    //Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    //if (recId == 0 && rowIndex == 0)
                    //{
                    //    operationResult_BOL = timecardTable.create(dataTable);
                    //    result = operationResults(operationResult_BOL);
                    //}
                    //else
                    //{
                    //}
                    operationResult_BOL = timecardTable.update(dataTable);
                    result = operationResults(operationResult_BOL);


                    if (result)
                    {
                        gridView_TimecardTable.EditIndex = -1;
                        reBindGrid();
                    }
                    else
                    {
                        bindGrid();
                    }
                }
            }
        }

        protected void Cancel_TimecardTable_Click(object sender, EventArgs e)
        {
            gridView_TimecardTable.EditIndex = -1;
            bindGrid();
        }

        private void checkTimecardTable_status(GridViewRow _gridViewRow, bool isHeader = false)
        {
            GridViewRow gridViewRow = _gridViewRow;

            string wfStatus = (gridViewRow.FindControl("lblWFStatus") as Label).Text.Replace(" ", "");
            string registrationsLocked = ((Label)gridViewRow.FindControl("lblRegistrationsLocked")).Text;
            string registrationsTransferred = ((Label)gridViewRow.FindControl("lblRegistrationsTransferred")).Text;
            string registrationsCalculated = ((Label)gridViewRow.FindControl("lblRegistrationsCalculated")).Text;


            if (registrationsLocked == NoYes.Yes.ToString() || registrationsTransferred == NoYes.Yes.ToString() ||
                registrationsCalculated == NoYes.Yes.ToString() || wfStatus != JmgWFStatus.NotSubmitted.ToString())
            {
                if (isHeader)
                {
                    if (gridViewRow.FindControl("ctl00") != null)
                    {
                        //(gridViewRow.Cells[12].Controls[1] as LinkButton).Enabled = false;
                        (gridViewRow.FindControl("ctl00") as LinkButton).Enabled = false;
                        (gridViewRow.FindControl("ctl00") as LinkButton).Visible = false;
                    }
                    gridViewRow.Attributes.Add("nonactionable", "true");

                    //if (wfStatus != JmgWFStatus.NotSubmitted.ToString())
                    //{
                    //    CheckBox cbSelect = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                    //    cbSelect.Checked = false;
                    //    cbSelect.Enabled = false;
                    //    gridViewRow.Attributes.Add("nonactionable", "true");
                    //}

                    //string[] validStatus = new string[] { JmgWFStatus.NotSubmitted.ToString() };
                    //base.checkWFStatus(gridViewRow, "lblWFStatus", validStatus);
                }
                else
                {
                    gridViewRow.Enabled = false;
                }
            }
            else
            {
                if (!isHeader)
                {
                    gridViewRow.Enabled = true;
                }
            }

        }



        private SysOperationResult_BOL submitWFRequest(long[] _recId, string _requestedBy)
        {
            long[] requestRecId = _recId;
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            string requestedBy = _requestedBy;

            if (requestRecId.Length > 0)
            {
                submitResult = eSSWorkflow.jmgTimecardTable_Submit(requestRecId, requestedBy);
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
                submitResult.Message = " Failed to submit the request.";
            }

            return submitResult;
        }


        protected string setDateTimeFormat(string _value)
        {
            string value = _value;
            value = string.IsNullOrEmpty(value) || value == "1/1/1900 12:00:00 AM" ? "" : Convert.ToDateTime(_value).ToLocalTime().ToString();   //DateTime.MinValue.ToString()
            return value;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView_TimecardTable.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recordRecId = 0;
                    string requestId = (gridViewRow.FindControl("lblRecId") as Label).Text;
                    long.TryParse(requestId, out recordRecId);
                    if (recordRecId > 0)
                    {
                        recordsId.Add(recordRecId);
                    }
                }
            }
            if (recordsId.Count > 0)
            {
                string requestedBy = SessionVariables.getCurrentEmployeeId();
                SysOperationResult_BOL operationResult_BOL = submitWFRequest(recordsId.ToArray(), requestedBy);
                bool result = operationResults(operationResult_BOL);
                reBindGrid();
            }

        }

        protected void btnUpdateAttendance_Click(object sender, EventArgs e)
        {
            jmgAttendanceIntegration.updateEmpLocationAttendance(SessionVariables.getCurrentEmployeeId());
        }
    }
}