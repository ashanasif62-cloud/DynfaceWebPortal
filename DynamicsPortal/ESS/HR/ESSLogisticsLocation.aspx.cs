using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.LogisticsLocationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSLogisticsLocation : ModalForm
    {
        private LogisticsLocation logisticsLocation = new LogisticsLocation();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = logisticsLocation.tableName;
                pageMenuId = "ESSHRPersonalDetailsHistory";
                showPageTitle = false;

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

        public bool setViewState_logisticsLocation(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_LogisticsLocation"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_logisticsLocation()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_LogisticsLocation"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_LogisticsLocation"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_logisticsLocation();
            }
            return dataTable;
        }


        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = logisticsLocation.retrieveByEmployee(employeeId);
            setViewState_logisticsLocation(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = getViewState_logisticsLocation();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlIsPrimaryTaxRegistration = gridViewRow.FindControl("ddlIsPrimaryTaxRegistration") as DropDownList;
                ddlIsPrimaryTaxRegistration.DataSource = Enum.GetNames(typeof(NoYes));
                ddlIsPrimaryTaxRegistration.DataBind();

                DropDownList ddlIsPrivate = gridViewRow.FindControl("ddlIsPrivate") as DropDownList;
                ddlIsPrivate.DataSource = Enum.GetNames(typeof(NoYes));
                ddlIsPrivate.DataBind();

                DropDownList ddlIsPrimary = gridViewRow.FindControl("ddlIsPrimary") as DropDownList;
                ddlIsPrimary.DataSource = Enum.GetNames(typeof(NoYes));
                ddlIsPrimary.DataBind();

                ddlIsPrimaryTaxRegistration.SelectedValue = dataRowView["IsPrimaryTaxRegistration"].ToString();
                ddlIsPrivate.SelectedValue = dataRowView["IsPrivate"].ToString();
                ddlIsPrimary.SelectedValue = dataRowView["IsPrimary"].ToString();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string tableName = string.Empty;

            dataTable = getViewState_logisticsLocation();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_logisticsLocation(dataTable);
            }

            gridView.EditIndex = 0;
            bindGrid();
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
                    DataTable dataTable = logisticsLocation.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    string employeeId = SessionVariables.getCurrentEmployeeId();
                    long empId = ControlsHelper.getWorkerId(employeeId);

                    dr["EmployeeId"] = empId;                              //C
                    dr["City"] = (gridViewRow.FindControl("txtCity") as TextBox).Text;
                    dr["Description"] = (gridViewRow.FindControl("txtDescription") as TextBox).Text;
                    dr["CountryRegionId"] = (gridViewRow.FindControl("txtCountryRegionId") as TextBox).Text;
                    dr["Purpose"] = (gridViewRow.FindControl("txtPurpose") as TextBox).Text;
                    dr["State"] = (gridViewRow.FindControl("txtState") as TextBox).Text;
                    dr["Street"] = (gridViewRow.FindControl("txtStreet") as TextBox).Text;
                    dr["ZipCode"] = (gridViewRow.FindControl("txtZipCode") as TextBox).Text;

                    dr["IsPrimary"] = (gridViewRow.FindControl("ddlIsPrimary") as DropDownList).SelectedValue;                                  //C
                    dr["IsPrimaryTaxRegistration"] = (gridViewRow.FindControl("ddlIsPrimaryTaxRegistration") as DropDownList).SelectedValue;    //C primary for country
                    dr["IsPrivate"] = (gridViewRow.FindControl("ddlIsPrivate") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);
                    if (true)
                    {
                        operationResult_BOL = logisticsLocation.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        //long recId = Convert.ToInt64(((Label)gridViewRow.FindControl("lblRecId")).Text);
                        //long recId = 0;
                        //Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                        //operationResult_BOL = logisticsLocation.update(dataTable, recId);
                        //result = operationResults(operationResult_BOL);
                    }


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
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    int rowIndex = gridViewRow.RowIndex;
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView.DataKeys[0].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = getViewState_logisticsLocation();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_logisticsLocation(dataTable);
                            }

                        }
                    }
                }
            }
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
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = logisticsLocation.delete(recordsId.ToArray());
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

    }
}