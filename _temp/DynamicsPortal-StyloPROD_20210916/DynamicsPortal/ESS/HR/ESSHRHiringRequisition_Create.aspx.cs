using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HRHiringRequisitionSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRHiringRequisition_Create : ModalForm
    {
        private HRHiringRequisitionDetails hRHiringRequisitionDetails = new HRHiringRequisitionDetails();
        private HRHiringRequisition hRHiringRequisition = new HRHiringRequisition();
        private string serialNumber;
        private string wfStatus;

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRHiringRequisitionRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    if (string.IsNullOrEmpty(Request.QueryString["SerialNumber"]))//Request.QueryString.GetValues(null).Contains("SerialNumber")
                    {
                        txtHRSerialNumber.Enabled = false;
                        //txtDepartmentName.Enabled = false;

                        bindEmptyData();
                    }
                    else
                    {
                        serialNumber = SecureQueryString.decrypt(Request.QueryString["SerialNumber"]);
                        DataTable dataTable = SessionVariables.getSessionDataTable();
                        DataRow dataRow = dataTable.Select("HRSerialNumber = '" + serialNumber + "'").FirstOrDefault();
                        if (dataRow != null)
                        {
                            wfStatus = dataRow["HRWFStatus"].ToString();
                            txtHRSerialNumber.Text = dataRow["HRSerialNumber"].ToString();
                            //txtDepartmentName.Text = dataRow["DepartmentName"].ToString();
                            txtRequestDate.Text = dataRow["RequestDate"].ToString();
                            (cddlEmployeeDetails.FindControl("txtEmployeeId") as TextBox).Text = dataRow["RequestedByName"].ToString();

                            setViewState_HiringRequisitionId(serialNumber);
                            reBindGrid_HiringRequisitionDetails();

                            if (wfStatus != HRWFStatus.NotSubmitted.ToString())
                            {
                                disableActionControls();
                            }
                        }
                        else
                        {
                            bindEmptyData();
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
            finally
            { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void reBindGrid_HiringRequisitionDetails()
        {
            getGridData_HiringRequisitionDetails();
            bindGrid_HiringRequisitionDetails();
        }

        public void bindEmptyData()
        {
            DataTable dtHiringRequisitionDetails = hRHiringRequisitionDetails.createDataTable();
            setViewState_HiringRequisitionDetails(dtHiringRequisitionDetails);
            gridView_HiringRequisitionDetails.DataSource = dtHiringRequisitionDetails;
            gridView_HiringRequisitionDetails.DataBind();

            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestDate.Enabled = false;
        }


        private void getGridData_HiringRequisitionDetails()
        {
            //GridViewRow row = gridView_HiringRequisitionDetails.SelectedRow;
            string serialNumber = getViewState_HiringRequisitionId();

            DataTable dt = hRHiringRequisitionDetails.retrieveAllHiringRequisitionDetails(serialNumber);
            setViewState_HiringRequisitionDetails(dt);
        }
        protected void bindGrid_HiringRequisitionDetails()
        {
            DataTable dt = getViewState_HiringRequisitionDetails();
            gridView_HiringRequisitionDetails.DataSource = dt;
            gridView_HiringRequisitionDetails.DataBind();
        }

        private void disableActionControls()
        {
            gridView_HiringRequisitionDetails.Enabled = false;
            btnNewDetails.Enabled = false;
            btnDeleteDetails.Enabled = false;
            txtHRSerialNumber.Enabled = false;
            //txtDepartmentName.Enabled = false;
            txtRequestDate.Enabled = false;
            (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Enabled = false;
        }
        private void enableActionButtons()
        {
            gridView_HiringRequisitionDetails.Enabled = true;
            btnNewDetails.Enabled = true;
            btnDeleteDetails.Enabled = true;
        }

        protected void gridView_HiringRequisitionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                //LinkButton btnDelete = gridViewRow.FindControl("gvHiringRequisitionDetails_delete") as LinkButton;
                //btnDelete.Attributes["onclick"] = "return confirm('Do you want to delete this row?');";

                DropDownList ddlLocation = gridViewRow.FindControl("ddlLocation") as DropDownList;
                DataTable dtDimLocation = ControlsHelper.retrieveAllDimLocation();
                ddlLocation.DataSource = dtDimLocation;
                ddlLocation.DataTextField = "Description";
                ddlLocation.DataValueField = "value";
                ddlLocation.DataBind();

                DropDownList ddlDesignation = gridViewRow.FindControl("ddlDesignation") as DropDownList;
                DataTable dtDesignation = ControlsHelper.retrieveAllHcmJob();
                ddlDesignation.DataSource = dtDesignation;
                ddlDesignation.DataTextField = "JobId";
                ddlDesignation.DataValueField = "JobId";
                ddlDesignation.DataBind();

                DropDownList ddlEducation = gridViewRow.FindControl("ddlEducation") as DropDownList;
                DataTable dtEducation = ControlsHelper.retrieveAllHcmEducationDiscipline();
                ddlEducation.DataSource = dtEducation;
                ddlEducation.DataTextField = "EducationDisciplineId";
                ddlEducation.DataValueField = "EducationDisciplineId";
                ddlEducation.DataBind();

                ddlLocation.SelectedValue = dataRowView["Location"].ToString();
                ddlDesignation.SelectedValue = dataRowView["Designation"].ToString();
                ddlEducation.SelectedValue = dataRowView["Education"].ToString();

            }
        }

        protected void gridView_HiringRequisitionDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView_HiringRequisitionDetails.EditIndex = e.NewEditIndex;
            bindGrid_HiringRequisitionDetails();

        }

        protected void btnNewDetails_Click(object sender, EventArgs e)
        {
            serialNumber = getViewState_HiringRequisitionId();

            if (string.IsNullOrEmpty(serialNumber))
            {
                DataTable dt = hRHiringRequisition.createDataTable();
                DataRow dr = dt.NewRow();
                string requestedBy = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
                string requestDate = txtRequestDate.Text;
                if (string.IsNullOrEmpty(requestedBy))
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select a value for Requested By.");
                    return;
                }
                if (string.IsNullOrEmpty(requestDate))
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please provide a value for Request Date.");
                    return;
                }

                long employeeId = ControlsHelper.getWorkerId(requestedBy);

                dr["RequestedBy"] = employeeId;
                dr["RequestDate"] = requestDate;

                dt.Rows.Add(dr);

                SysOperationResult_BOL operationResult_BOL = hRHiringRequisition.create(dt);
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    serialNumber = operationResult_BOL.Message.Replace(" is created.", "");
                    txtHRSerialNumber.Text = serialNumber;
                    setViewState_HiringRequisitionId(serialNumber);
                    (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Enabled = false;
                    txtRequestDate.Enabled = false;
                }
            }
            DataTable dataTable = new DataTable();
            dataTable = getViewState_HiringRequisitionDetails();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);
                setViewState_HiringRequisitionDetails(dataTable);
            }
            gridView_HiringRequisitionDetails.EditIndex = 0;
            bindGrid_HiringRequisitionDetails();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    bool result;
                    SysOperationResult_BOL operationResult_BOL;
                    DataTable dataTable = hRHiringRequisitionDetails.createDataTable();

                    DataRow dr = dataTable.NewRow();
                    string serialNumber = getViewState_HiringRequisitionId();
                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        dr["HRSerialNumber"] = serialNumber;
                        dr["Experience"] = (gridViewRow.FindControl("txtExperience") as TextBox).Text;
                        dr["JobDescription"] = (gridViewRow.FindControl("txtJobDescription") as TextBox).Text;
                        dr["Remarks"] = (gridViewRow.FindControl("txtRemarks") as TextBox).Text;
                        dr["Vacancies"] = (gridViewRow.FindControl("txtVacancies") as TextBox).Text;
                        //dr["ExpectedHiringDate"] = (gridViewRow.FindControl("txtExpectedHiringDate") as TextBox).Text;

                        dr["Designation"] = (gridViewRow.FindControl("ddlDesignation") as DropDownList).SelectedValue;
                        dr["Education"] = (gridViewRow.FindControl("ddlEducation") as DropDownList).SelectedValue;
                        dr["Location"] = (gridViewRow.FindControl("ddlLocation") as DropDownList).SelectedValue;

                        dataTable.Rows.Add(dr);

                        int rowIndex = gridViewRow.RowIndex;
                        long recId = 0;
                        Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                        if (recId == 0 && rowIndex == 0)
                        {
                            operationResult_BOL = hRHiringRequisitionDetails.create(dataTable);
                            result = operationResults(operationResult_BOL);
                        }
                        else
                        {
                            operationResult_BOL = hRHiringRequisitionDetails.update(dataTable, Convert.ToInt64(recId));
                            result = operationResults(operationResult_BOL);
                        }
                    }
                    else
                    {
                        result = false;
                        NotificationMessage.showMessage(AlertType.Error, "Unable to retrieve Hiring Requisition Reference.");
                    }

                    if (result)
                    {
                        gridView_HiringRequisitionDetails.EditIndex = -1;
                        reBindGrid_HiringRequisitionDetails();
                    }
                    else
                    {
                        bindGrid_HiringRequisitionDetails();
                    }
                }
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    int rowIndex = gridViewRow.RowIndex;
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView_HiringRequisitionDetails.DataKeys[rowIndex].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = getViewState_HiringRequisitionDetails();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_HiringRequisitionDetails(dataTable);
                            }
                        }
                    }
                }
            }
            gridView_HiringRequisitionDetails.EditIndex = -1;
            bindGrid_HiringRequisitionDetails();

        }

        protected void btnDeleteDetails_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            bool includeEmptyRows = false;
            foreach (GridViewRow gridViewRow in gridView_HiringRequisitionDetails.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                    else
                    {
                        includeEmptyRows = true;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = hRHiringRequisitionDetails.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView_HiringRequisitionDetails.EditIndex = -1;
                    reBindGrid_HiringRequisitionDetails();
                }
                else
                {
                    bindGrid_HiringRequisitionDetails();
                }
                return;
            }
            else if (includeEmptyRows)
            {
                gridView_HiringRequisitionDetails.EditIndex = -1;
                reBindGrid_HiringRequisitionDetails();
            }


        }

        #region ViewState
        public bool setViewState_HiringRequisitionDetails(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_HiringRequisitionDetails"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_HiringRequisitionDetails()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_HiringRequisitionDetails"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_HiringRequisitionDetails"] as DataTable).Copy();
            }
            //else
            //{
            //    getGridData_HiringRequisitionDetails();
            //    dataTable = getViewState_HiringRequisitionDetails();
            //}
            return dataTable;
        }

        public bool setViewState_HiringRequisitionId(string _serialNumber)
        {
            bool isStored = false;
            string serialNumber = _serialNumber;

            if (ViewState != null)
            {
                ViewState["HiringRequisition_serialNumber"] = serialNumber;
                isStored = true;
            }
            return isStored;
        }

        public string getViewState_HiringRequisitionId()
        {
            string appraisalId = string.Empty;

            if (ViewState["HiringRequisition_serialNumber"] != null)
            {
                appraisalId = ViewState["HiringRequisition_serialNumber"].ToString();
            }
            return appraisalId;
        }
        #endregion

    }
}
