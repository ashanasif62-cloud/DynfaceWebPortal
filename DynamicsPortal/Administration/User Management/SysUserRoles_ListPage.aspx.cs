using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class SysUserRoles_ListPage : MainForm
    {
        private SysUserRoles_BLL sysUserRoles_BLL = new SysUserRoles_BLL();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "SysUserRolesHistory";

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

        //protected void reBindGrid()
        //{
        //    string roleId = string.Empty;
        //    if (!string.IsNullOrEmpty(Request.QueryString["RoleId"]))
        //    {
        //        roleId = SecureQueryString.decrypt(Request.QueryString["RoleId"]);
        //    }
        //    reBindGrid(roleId);
        //}

        protected void reBindGrid()
        {
            //string roleId = _roleId;roleId string _roleId
            getGridDataTable();
            bindGrid();
        }

        private void getGridDataTable()
        {
            //string roleId = _roleId;string _roleId
            SysUserRole_BOL objBOL = new SysUserRole_BOL();
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            long partition = SessionVariables.getCurrentUserPartition();

            if (!string.IsNullOrEmpty(dataAreaId) && partition > 0)//!string.IsNullOrEmpty(roleId) && 
            {
                //objBOL.RoleId = roleId;
                objBOL.DataAreaId = dataAreaId;
                objBOL.Partition = partition;
            }
            DataTable dt = sysUserRoles_BLL.SysUserRoles_RetrieveAll(objBOL);//sysUserRoles_BLL.SysUserRoles_RetrieveByRoleId(objBOL);
            setViewState(dt);
        }

        protected void bindGrid()
        {
            DataTable dt = getViewState();
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
                reBindGrid();
                dataTable = getViewState();
            }
            return dataTable;
        }


        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        //protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GridViewRow gridViewRow = e.Row;
        //    if (gridViewRow.RowType != DataControlRowType.DataRow)
        //        return;

        //    if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
        //    {
        //        DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

        //        SysUserInfo_BLL sysUserInfo_BLL = new SysUserInfo_BLL();
        //        DropDownList ddlUserId = gridViewRow.FindControl("ddlUserId") as DropDownList;
        //        ddlUserId.DataSource = sysUserInfo_BLL.retrieveAllSysUserInfo();
        //        ddlUserId.DataTextField = "UserId";
        //        ddlUserId.DataValueField = "UserId";
        //        ddlUserId.DataBind();

        //        SysRoles_BLL sysRoles_BLL = new SysRoles_BLL();
        //        DropDownList ddlRoleId = gridViewRow.FindControl("ddlRoleId") as DropDownList;
        //        ddlRoleId.DataSource = sysRoles_BLL.sysRoles_Retrieve();
        //        ddlRoleId.DataTextField = "RoleId";
        //        ddlRoleId.DataValueField = "RoleId";
        //        ddlRoleId.DataBind();

        //        ddlRoleId.SelectedValue = dataRowView["RoleId"].ToString();
        //        ddlUserId.SelectedValue = dataRowView["UserId"].ToString();
        //    }
        //}


        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;

                // --- USER ID Dropdown ---
                DropDownList ddlUserId = gridViewRow.FindControl("ddlUserId") as DropDownList;
                if (ddlUserId != null)
                {
                    SysUserInfo_BLL sysUserInfo_BLL = new SysUserInfo_BLL();
                    DataTable dt = sysUserInfo_BLL.retrieveAllSysUserInfo(); // Should have UserId and UserName

                    // Add concatenated display column: "UserId - UserName"
                    if (!dt.Columns.Contains("DisplayText"))
                        dt.Columns.Add("DisplayText", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = dr["UserId"]?.ToString();
                        string name = dr["UserName"]?.ToString();
                        dr["DisplayText"] = $"{id} - {name}";
                    }

                    ddlUserId.DataSource = dt;
                    ddlUserId.DataTextField = "DisplayText";   // Show "UserId - UserName"
                    ddlUserId.DataValueField = "UserId";       // Still bind to UserId
                    ddlUserId.DataBind();
                    ddlUserId.Items.Insert(0, new ListItem("", ""));

                    string userId = row["UserId"]?.ToString();
                    if (!string.IsNullOrEmpty(userId) && ddlUserId.Items.FindByValue(userId) != null)
                        ddlUserId.SelectedValue = userId;
                    else
                        ddlUserId.SelectedIndex = 0;

                    ddlUserId.CssClass += " filterable-dropdown";

                }

                // --- ROLE ID Dropdown ---
                //DropDownList ddlRoleId = gridViewRow.FindControl("ddlUsers") as DropDownList;
                DropDownList ddlRoleId = gridViewRow.FindControl("ddlRoleId") as DropDownList;
                if (ddlRoleId != null)
                {
                    SysRoles_BLL roleBLL = new SysRoles_BLL();
                    ddlRoleId.DataSource = roleBLL.sysRoles_Retrieve(); // returns DataTable
                    ddlRoleId.DataTextField = "RoleId";
                    ddlRoleId.DataValueField = "RoleId";
                    ddlRoleId.DataBind();
                    ddlRoleId.Items.Insert(0, new ListItem("", ""));

                    string roleId = row["RoleId"]?.ToString();
                    if (!string.IsNullOrEmpty(roleId) && ddlRoleId.Items.FindByValue(roleId) != null)
                        ddlRoleId.SelectedValue = roleId;
                    else
                        ddlRoleId.SelectedIndex = 0;

                    ddlRoleId.CssClass += " filterable-dropdown";
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable = getViewState();

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

                    SysUserRole_BOL sysUserRoles_BOL = new SysUserRole_BOL();
                    //sysUserRoles_BOL.RoleId = (gridViewRow.FindControl("ddlRoleId") as DropDownList).SelectedValue;
                    sysUserRoles_BOL.UserId = (gridViewRow.FindControl("ddlUserId") as DropDownList).SelectedValue;
                    sysUserRoles_BOL.CreatedBy = SessionVariables.getCurrentUserId();
                    sysUserRoles_BOL.ModifiedBy = sysUserRoles_BOL.CreatedBy;
                    sysUserRoles_BOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    sysUserRoles_BOL.Partition = SessionVariables.getCurrentUserPartition();
                    sysUserRoles_BOL.RecVersion = 1;

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    sysUserRoles_BOL.RecId = recId;
                    if (recId == 0 && rowIndex == 0)
                    {
                        results = sysUserRoles_BLL.sysUserRoles_Create(sysUserRoles_BOL);
                        //result = operationResults(operationResult_BOL);
                    }
                    //else
                    //{
                    //    results = sysUserRoles_BLL.sysUserRoles_Update(sysUserRoles_BOL);
                    //    //result = operationResults(operationResult_BOL);
                    //}


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
                        SysUserRole_BOL sysUserRole_BOL = new SysUserRole_BOL();
                        sysUserRole_BOL.RecId = recId;
                        //recordsId.Add(recId);
                        sysUserRoles_BLL.sysUserRoles_Delete(sysUserRole_BOL);
                    }
                    //else
                    //{
                    //    includeEmptyRows = true;
                    //}
                }
            }

            //if (recordsId.Count > 0)
            //{
            //    SysOperationResult_BOL operationResult_BOL = sysUserRoles_BLL.sysUserRoles_Delete(recordsId.ToArray());
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