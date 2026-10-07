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
    public partial class ESSJmgTimecardTable_ListPage_del : MainForm
    {
        private JmgTimecardTable timecardTable = new JmgTimecardTable();
        private JmgTimecardTrans timecardTrans = new JmgTimecardTrans();
        private HcmReasonCode hcmReasonCode = new HcmReasonCode();
        private JmgAttendanceIntegration jmgAttendanceIntegration = new JmgAttendanceIntegration();


        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = timecardTable.tableName;
                pageMenuId = "ESSJmgTimecardTableHistory_del";
                //showActionPanel = false;

                //txtFromDate.Text = (DateTime.Now.AddDays(-7)).ToShortDateString();
                //txtToDate.Text = DateTime.Now.ToShortDateString();

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid_TimecardTable();
                    bindEmptyData();
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

        protected void reBindGrid_TimecardTable()
        {
            getGridData_TimecardTable();
            bindGrid_TimecardTable();
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

        private void getGridData_TimecardTable()
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string employeeId = getQuery_EmployeeId();
            DataTable dt = timecardTable.createDataTable();

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime fromDateTime = fromDate.toDateTime();
                DateTime toDateTime = toDate.toDateTime();

                dt = timecardTable.retrive(employeeId, fromDateTime, toDateTime);
            }
            SessionVariables.setSessionDataTable(dt);
        }

        protected void bindGrid_TimecardTable()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView_TimecardTable.DataSource = dt;
            gridView_TimecardTable.DataBind();
        }

        protected void gridView_TimecardTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string selectedRecId, drRecId;
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            selectedRecId = getViewState_RecId();

            if (!string.IsNullOrEmpty(selectedRecId))
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
                drRecId = dataRowView["RecId"].ToString();
                if (selectedRecId.Equals(drRecId))
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
                }
            }

            checkTimecardTable_status(gridViewRow, true);
        }

        protected void gridView_TimecardTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //GridView gvTimecardTable = sender as GridView;
            //GridViewRow gridViewRow = gvTimecardTable.Rows[e.NewEditIndex];

            gridView_TimecardTable.EditIndex = e.NewEditIndex;
            bindGrid_TimecardTable();
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

                    dr["Remarks"] = (gridViewRow.FindControl("txtRemarks") as TextBox).Text;
                    dr["ProfileId"] = (gridViewRow.FindControl("lblProfileId") as Label).Text;
                    dr["RecId"] = (gridViewRow.FindControl("lblRecId") as Label).Text;

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
                        reBindGrid_TimecardTable();
                    }
                    else
                    {
                        bindGrid_TimecardTable();
                    }
                }
            }
        }

        protected void Cancel_TimecardTable_Click(object sender, EventArgs e)
        {
            gridView_TimecardTable.EditIndex = -1;
            bindGrid_TimecardTable();
        }

        protected void Lines_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                long recId = 0;
                string strRecId = ((Label)gridViewRow.FindControl("lblRecId")).Text;
                string profileDate = ((Label)gridViewRow.FindControl("lblProfileDate")).Text;
                string employeeId = ((Label)gridViewRow.FindControl("lblEmployeeId")).Text;

                Int64.TryParse(strRecId, out recId);

                setViewState_ProfileDate(profileDate);
                setViewState_EmployeeId(employeeId);
                setViewState_RecId(strRecId);

                getGridData_TimecardTrans();
                bindGrid_TimecardTrans();

                checkTimecardTable_status(gridViewRow);

                //gridViewRow.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            }
            bindGrid_TimecardTable();
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
                    gridView_TimecardTrans.Enabled = false;
                    btnNew.Enabled = false;
                    btnDelete.Enabled = false;
                    gridViewRow.Enabled = false;
                }
            }
            else
            {
                if (!isHeader)
                {
                    gridView_TimecardTrans.Enabled = true;
                    btnNew.Enabled = true;
                    btnDelete.Enabled = true;
                    gridViewRow.Enabled = true;
                }
            }

        }

        private void getGridData_TimecardTrans()
        {
            string employeeId = getViewState_EmployeeId();
            DateTime profileDate = getViewState_ProfileDate();

            DataTable dt = timecardTrans.retrieveByEmployeeProfileDate(employeeId, profileDate);
            setViewState_TimecardTrans(dt);
        }
        protected void reBindGrid_TimecardTrans()
        {
            getGridData_TimecardTrans();
            bindGrid_TimecardTrans();
        }
        protected void bindGrid_TimecardTrans()
        {
            DataTable dt = getViewState_TimecardTrans();
            gridView_TimecardTrans.DataSource = dt;
            gridView_TimecardTrans.DataBind();
        }

        #region ViewState
        public void bindEmptyData()
        {
            DataTable dtKPIAppraisers = timecardTrans.createDataTable();
            gridView_TimecardTrans.DataSource = dtKPIAppraisers;
            gridView_TimecardTrans.DataBind();
        }

        public bool setViewState_TimecardTrans(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_TimecardTrans"] = dataTable;
                isStored = true;
            }
            return isStored;
        }
        public DataTable getViewState_TimecardTrans()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_TimecardTrans"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_TimecardTrans"] as DataTable).Copy();
            }
            //else
            //{
            //    getGridData_TimecardTrans();
            //    dataTable = getViewState_TimecardTrans();
            //}
            return dataTable;
        }

        public bool setViewState_EmployeeId(string _employeeId)
        {
            bool isStored = false;

            if (ViewState != null)
            {
                ViewState["EmployeeId"] = _employeeId;
                isStored = true;
            }
            return isStored;
        }
        public string getViewState_EmployeeId()
        {
            string employeeId = "";

            if (ViewState["EmployeeId"] != null)
            {
                employeeId = ViewState["EmployeeId"].ToString();
            }
            return employeeId;
        }

        public bool setViewState_ProfileDate(string _profileDate)
        {
            bool isStored = false;

            if (ViewState != null)
            {
                ViewState["ProfileDate"] = _profileDate;
                isStored = true;
            }
            return isStored;
        }
        public DateTime getViewState_ProfileDate()
        {
            DateTime profileDate = DateTime.MinValue;

            if (ViewState["ProfileDate"] != null)
            {
                DateTime.TryParse(ViewState["ProfileDate"].ToString(), out profileDate);
            }
            return profileDate;
        }

        public bool setViewState_RecId(string _recId)
        {
            bool isStored = false;

            if (ViewState != null)
            {
                ViewState["RecId"] = _recId;
                isStored = true;
            }
            return isStored;
        }
        public string getViewState_RecId()
        {
            string recId = "";

            if (ViewState["RecId"] != null)
            {
                recId = ViewState["RecId"].ToString();
            }
            return recId;
        }

        #endregion

        protected void gridView_TimecardTrans_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView_TimecardTrans.EditIndex = e.NewEditIndex;
            bindGrid_TimecardTrans();
        }

        protected void gridView_TimecardTrans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

            //string wfStatus = dataRowView["WfStatus"].ToString().Replace(" ", "");

            //if (appraiserStatus != APAppraiserStatus.InProcess.ToString())
            //{
            //    gridViewRow.Enabled = false;
            //    gridViewRow.Attributes.Add("NonActionable", "true");
            //}
            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                //if (!string.IsNullOrEmpty(dataRowView["RecId"].ToString()))
                //{
                //    e.Row.Cells[2].Enabled = false; //Type
                //}
                DropDownList ddlJourRegType = gridViewRow.FindControl("ddlJourRegType") as DropDownList;
                ddlJourRegType.DataSource = Enum.GetNames(typeof(JmgJourRegTypeEnum_Maison));
                ddlJourRegType.DataBind();
                ddlJourRegType.SelectedValue = dataRowView["JourRegType"].ToString();


                DropDownList ddlReasonCodeId = gridViewRow.FindControl("ddlReasonCodeId") as DropDownList;
                ddlReasonCodeId.DataSource = hcmReasonCode.retrieveAllHcmReasonCode();
                ddlReasonCodeId.DataTextField = "ReasonCodeId";
                ddlReasonCodeId.DataValueField = "ReasonCodeId";
                ddlReasonCodeId.DataBind();
                ddlReasonCodeId.SelectedValue = dataRowView["ReasonCodeId"].ToString();

            }
        }

        protected void Date_TextChanged(object sender, EventArgs e)
        {
            reBindGrid_TimecardTable();
        }

        protected void btnNew_TimecardTrans_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string tableName = string.Empty;

            dataTable = getViewState_TimecardTrans();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dr["JourRegType"] = JmgJourRegTypeEnum_Maison.SignIn.ToString();
                dr["ProfileDate"] = getViewState_ProfileDate().Date;
                dr["GenerationType"] = JmgGenerationType.ESSPortal.ToString();
                dr["StartDateTime"] = getViewState_ProfileDate();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_TimecardTrans(dataTable);
            }

            gridView_TimecardTrans.EditIndex = 0;
            bindGrid_TimecardTrans();
        }

        protected void Update_TimecardTrans_Click(object sender, EventArgs e)
        {
            bool result = false;
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;

            result = save_TimecardTrans(gridViewRow);
            if (result)
            {
                gridView_TimecardTrans.EditIndex = -1;
                reBindGrid_TimecardTrans();
            }
            else
            {
                bindGrid_TimecardTrans();
            }
        }

        protected void UpdateAndSubmit_TimecardTrans_Click(object sender, EventArgs e)
        {
            bool result = false;
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;

            result = save_TimecardTrans(gridViewRow, true);
            if (result)
            {
                reBindGrid_TimecardTable();
                gridView_TimecardTrans.EditIndex = -1;
                reBindGrid_TimecardTrans();

                gridView_TimecardTrans.Enabled = false;
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                gridViewRow.Enabled = false;
            }
            else
            {
                bindGrid_TimecardTrans();
            }
        }

        private bool save_TimecardTrans(GridViewRow _gridViewRow, bool _submit = false)
        {
            bool result = false;
            bool submitRequest = _submit;
            GridViewRow gridViewRow = _gridViewRow;
            SysOperationResult_BOL saveResults = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = timecardTrans.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    string employeeId = getViewState_EmployeeId();
                    //long worker = ControlsHelper.getWorkerDetails(employeeId).WorkerRecId;//getWorkerId(employeeId);
                    if (!string.IsNullOrEmpty(employeeId))
                    {
                        string requestedBy = SessionVariables.getCurrentEmployeeId();
                        dr["ProfileDate"] = (gridViewRow.FindControl("lblProfileDate") as Label).Text;
                        dr["StartDateTime"] = (gridViewRow.FindControl("txtStartDateTime") as TextBox).Text;
                        dr["JourRegType"] = (gridViewRow.FindControl("ddlJourRegType") as DropDownList).SelectedValue;
                        dr["ReasonCodeId"] = (gridViewRow.FindControl("ddlReasonCodeId") as DropDownList).SelectedValue;
                        dr["RequestedBy"] = requestedBy;
                        dr["EmployeeId"] = employeeId;
                        dr["RecId"] = (gridViewRow.FindControl("lblRecId") as Label).Text;

                        dataTable.Rows.Add(dr);

                        int rowIndex = gridViewRow.RowIndex;
                        long recId = 0;
                        Int64.TryParse(dr["RecId"].ToString(), out recId);   //gridView.DataKeys[rowIndex].Values[0]

                        if (recId == 0 && rowIndex == 0)
                        {
                            saveResults = timecardTrans.create(dataTable);
                        }
                        else
                        {
                            saveResults = timecardTrans.update(dataTable);
                        }

                        if (saveResults.isSuccess && submitRequest)
                        {
                            long recordRecId = 0;
                            long.TryParse(getViewState_RecId(), out recordRecId);

                            if (recordRecId > 0)
                            {
                                long[] recordsRecId = new long[] { recordRecId };
                                submitResult = submitWFRequest(recordsRecId, requestedBy);
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
                                submitResult.Message = " Invalid Record. Failed to submit the request.";
                            }
                            saveResults.Message += " " + submitResult.Message;
                            saveResults.AlertType = submitResult.AlertType;
                            saveResults.isSuccess = submitResult.isSuccess;
                        }

                        result = operationResults(saveResults);
                    }
                    else
                    {
                        NotificationMessage.showMessage(AlertType.Error, "Invalid Employee");
                    }
                }

            }

            return result;
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

        protected void Cancel_TimecardTrans_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    int rowIndex = gridViewRow.RowIndex;
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView_TimecardTrans.DataKeys[0].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = getViewState_TimecardTrans();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_TimecardTrans(dataTable);
                            }

                        }
                    }
                }
            }
            gridView_TimecardTrans.EditIndex = -1;
            bindGrid_TimecardTrans();
        }

        protected void btnDelete_TimecardTrans_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView_TimecardTrans.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        string generationType = ((Label)gridViewRow.FindControl("lblGenerationType")).Text;
                        if (string.IsNullOrEmpty(generationType) || generationType == JmgGenerationType.ESSPortal.ToString())
                            recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = timecardTrans.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView_TimecardTrans.EditIndex = -1;
                    reBindGrid_TimecardTrans();
                }
                else
                {
                    bindGrid_TimecardTrans();
                }
            }

        }

        protected string setDateTimeFormat(string _value)
        {
            string value = _value;
            value = string.IsNullOrEmpty(value) ? "" : Convert.ToDateTime(_value).ToLocalTime().ToString();   //DateTime.MinValue.ToString()
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
                if (result)
                {
                    reBindGrid_TimecardTable();
                    gridView_TimecardTrans.EditIndex = -1;
                    reBindGrid_TimecardTrans();

                    gridView_TimecardTrans.Enabled = false;
                    btnNew.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    reBindGrid_TimecardTable();
                }
            }

        }

        protected void btnUpdateAttendance_Click(object sender, EventArgs e)
        {
            jmgAttendanceIntegration.updateEmpLocationAttendance(SessionVariables.getCurrentEmployeeId());
        }
    }
}