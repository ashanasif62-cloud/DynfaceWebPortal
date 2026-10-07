using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class SysRolesMenuItems_ListPage : MainForm
    {
        private SysRolesMenuItems_BLL sysRolesMenuItems_BLL = new SysRolesMenuItems_BLL();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "SysRolesMenuItemsHistory";

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

        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = sysRolesMenuItems_BLL.sysRoleMenuItems_Retrieve();
            setViewState(dt);
        }

        protected void bindGrid()
        {
            DataTable dt = getViewState();//sysRolesMenuItems_BLL.sysRolesMenuItems_Retrieve();
            gridView.DataSource = dt;
            gridView.DataBind();
        }


        public bool setViewState(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState();
            }
            return dataTable;
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

                SysRoles_BLL sysRoles_BLL = new SysRoles_BLL();
                DropDownList ddlRoleId = gridViewRow.FindControl("ddlRoleId") as DropDownList;
                ddlRoleId.DataSource = sysRoles_BLL.sysRoles_Retrieve();
                ddlRoleId.DataTextField = "RoleId";
                ddlRoleId.DataValueField = "RoleId";
                ddlRoleId.DataBind();

                SysMenuItems_BLL sysMenuItems_BLL = new SysMenuItems_BLL();
                DropDownList ddlMenuItemId = gridViewRow.FindControl("ddlMenuItemId") as DropDownList;
                ddlMenuItemId.DataSource = sysMenuItems_BLL.SysMenuItems_Retrieve();
                ddlMenuItemId.DataTextField = "Label";
                ddlMenuItemId.DataValueField = "MenuItemId";
                ddlMenuItemId.DataBind();

                ddlRoleId.SelectedValue = dataRowView["RoleId"].ToString();
                ddlMenuItemId.SelectedValue = dataRowView["MenuItemId"].ToString();
            }
        }



        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable = getViewState();// sysRolesMenuItems_BLL.sysRolesMenuItems_Retrieve();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState(dataTable);
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
                    string results = string.Empty;
                    SysRolesMenuItem_BOL sysRolesMenuItems_BOL = new SysRolesMenuItem_BOL();

                    sysRolesMenuItems_BOL.RoleId = (gridViewRow.FindControl("ddlRoleId") as DropDownList).SelectedValue;
                    sysRolesMenuItems_BOL.MenuItemId = (gridViewRow.FindControl("ddlMenuItemId") as DropDownList).SelectedValue;
                    sysRolesMenuItems_BOL.CreatedBy = SessionVariables.getCurrentUserId();
                    sysRolesMenuItems_BOL.ModifiedBy = sysRolesMenuItems_BOL.CreatedBy;
                    sysRolesMenuItems_BOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    sysRolesMenuItems_BOL.Partition = SessionVariables.getCurrentUserPartition();
                    sysRolesMenuItems_BOL.RecVersion = 1;

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    sysRolesMenuItems_BOL.RecId = recId;
                    if (recId == 0 && rowIndex == 0)
                    {
                        results = sysRolesMenuItems_BLL.sysRolesMenuItems_Create(sysRolesMenuItems_BOL);
                        //result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        results = sysRolesMenuItems_BLL.sysRolesMenuItems_Update(sysRolesMenuItems_BOL);
                        //result = operationResults(operationResult_BOL);
                    }


                    //if (result)
                    //{
                    gridView.EditIndex = -1;
                    reBindGrid();
                    //}
                    //else
                    //{
                    //    bindGrid();
                    //}
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
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView.DataKeys[rowIndex].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = getViewState();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState(dataTable);
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
            //bool includeEmptyRows = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);
                    if (recId > 0)
                    {
                        SysRolesMenuItem_BOL sysRolesMenuItems_BOL = new SysRolesMenuItem_BOL();
                        sysRolesMenuItems_BOL.RecId = recId;
                        //recordsId.Add(recId);
                        sysRolesMenuItems_BLL.sysRolesMenuItems_Delete(sysRolesMenuItems_BOL);
                    }
                    //else
                    //{
                    //    includeEmptyRows = true;
                    //}
                }
            }

            //if (recordsId.Count > 0)
            //{
            //    SysOperationResult_BOL operationResult_BOL = sysRolesMenuItems_BLL.sysRolesMenuItems_Delete(recordsId.ToArray());
            //    bool result = operationResults(operationResult_BOL);
            //    if (result)
            //    {
            //        gridView.EditIndex = -1;
            //        reBindGrid();
            //    }
            //    else
            //    {
            //        bindGrid();
            //    }
            //    return;
            //}
            //else if (includeEmptyRows)
            //{
            gridView.EditIndex = -1;
            reBindGrid();
            //}

        }

    }
}