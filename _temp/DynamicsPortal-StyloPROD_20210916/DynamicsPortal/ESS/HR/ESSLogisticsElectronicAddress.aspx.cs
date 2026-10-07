using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.LogisticsElectronicAddressSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSLogisticsElectronicAddress : ModalForm
    {
        private LogisticsElectronicAddress logisticsElectronicAddress = new LogisticsElectronicAddress();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = logisticsElectronicAddress.tableName;
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
        public bool setViewState_LogisticsElectronicAddress(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_LogisticsElectronicAddress"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_LogisticsElectronicAddress()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_LogisticsElectronicAddress"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_LogisticsElectronicAddress"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_LogisticsElectronicAddress();
            }
            return dataTable;
        }


        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = logisticsElectronicAddress.retrieveByEmployee(employeeId);
            setViewState_LogisticsElectronicAddress(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = getViewState_LogisticsElectronicAddress();//SessionVariables.getSessionDataTable();
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

                if (!string.IsNullOrEmpty(dataRowView["RecId"].ToString()))
                {
                    e.Row.Cells[2].Enabled = false; //Type
                }

                DropDownList ddlType = gridViewRow.FindControl("ddlType") as DropDownList;
                ddlType.DataSource = Enum.GetNames(typeof(LogisticsElectronicAddressMethodTypePortalExtension));
                ddlType.DataBind();
                ddlType.SelectedValue = dataRowView["Type"].ToString();

                DropDownList ddlIsPrimary = gridViewRow.FindControl("ddlIsPrimary") as DropDownList;
                ddlIsPrimary.DataSource = Enum.GetNames(typeof(NoYes));
                ddlIsPrimary.DataBind();

                ddlIsPrimary.SelectedValue = dataRowView["IsPrimary"].ToString();
            }

        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string tableName = string.Empty;

            dataTable = getViewState_LogisticsElectronicAddress();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_LogisticsElectronicAddress(dataTable);
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
                    DataTable dataTable = logisticsElectronicAddress.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    string employeeId = SessionVariables.getCurrentEmployeeId();
                    long empId = ControlsHelper.getWorkerId(employeeId);

                    dr["EmployeeId"] = empId;
                    dr["Locator"] = (gridViewRow.FindControl("txtLocator") as TextBox).Text;
                    dr["Description"] = (gridViewRow.FindControl("txtDescription") as TextBox).Text;
                    dr["LocatorExtension"] = (gridViewRow.FindControl("txtLocatorExtension") as TextBox).Text;

                    dr["Type"] = (gridViewRow.FindControl("ddlType") as DropDownList).SelectedValue;                              //C
                    dr["IsPrimary"] = (gridViewRow.FindControl("ddlIsPrimary") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId == 0 && rowIndex == 0)
                    {
                        operationResult_BOL = logisticsElectronicAddress.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {

                        operationResult_BOL = logisticsElectronicAddress.update(dataTable, recId);
                        result = operationResults(operationResult_BOL);
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
                        dataTable = getViewState_LogisticsElectronicAddress();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_LogisticsElectronicAddress(dataTable);
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
            bool includeEmptyRows = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
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
                SysOperationResult_BOL operationResult_BOL = logisticsElectronicAddress.delete(recordsId.ToArray());
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
                return;
            }
            else if (includeEmptyRows)
            {
                gridView.EditIndex = -1;
                reBindGrid();
            }

        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

    }
}