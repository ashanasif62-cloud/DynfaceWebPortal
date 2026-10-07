using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRSubDepartment_ListPage : MainForm
    {
        private HcmPersonLaborUnion hcmPersonLaborUnion = new HcmPersonLaborUnion();
        private HcmUnions hcmUnions = new HcmUnions();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hcmPersonLaborUnion.tableName;
                pageMenuId = "ESSHRSubDepartmentHistory";
                //txtFromDate.Text = (DateTime.Now.AddDays(-7)).ToString();
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

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

        private string getQuery_EmployeeId()
        {
            string employeeId = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["EmpId"]))
                employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            //{
            //    employeeId = SessionVariables.getCurrentEmployeeId();
            //}
            //else
            //{
            //    employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            //}
            return employeeId;
        }

        private void getGridDataTable()
        {
            string employeeId = getQuery_EmployeeId();
            DataTable dt = hcmPersonLaborUnion.createDataTable();

            if (!string.IsNullOrEmpty(employeeId))
            {
                dt = hcmPersonLaborUnion.findByEmployee(employeeId);
            }
            else
            {
                NotificationMessage.showMessage(AlertType.Error, "Invalid Employee Request.");
            }
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

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlLaborUnion = gridViewRow.FindControl("ddlLaborUnion") as DropDownList;
                ddlLaborUnion.DataSource = hcmUnions.retrieveAll();
                ddlLaborUnion.DataTextField = "Name";
                ddlLaborUnion.DataValueField = "RecId";
                ddlLaborUnion.DataBind();
                ddlLaborUnion.SelectedValue = dataRowView["LaborUnion"].ToString();
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = SessionVariables.getSessionDataTable();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dr["EmployeeId"] = getQuery_EmployeeId();
                dataTable.Rows.InsertAt(dr, 0);

                SessionVariables.setSessionDataTable(dataTable);
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
                    DataTable dataTable = hcmPersonLaborUnion.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    dr["EmployeeId"] = (gridViewRow.FindControl("lblEmployeeId") as Label).Text;
                    dr["StartDate"] = (gridViewRow.FindControl("txtStartDate") as TextBox).Text;
                    dr["EndDate"] = (gridViewRow.FindControl("txtEndDate") as TextBox).Text;
                    dr["LaborUnion"] = (gridViewRow.FindControl("ddlLaborUnion") as DropDownList).SelectedValue;

                    dr["Person"] = (gridViewRow.FindControl("lblPerson") as Label).Text;

                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId == 0 && rowIndex == 0)
                    {
                        operationResult_BOL = hcmPersonLaborUnion.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        operationResult_BOL = hcmPersonLaborUnion.update(dataTable, Convert.ToInt64(recId));
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
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView.DataKeys[rowIndex].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = SessionVariables.getSessionDataTable();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                SessionVariables.setSessionDataTable(dataTable);
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
                SysOperationResult_BOL operationResult_BOL = hcmPersonLaborUnion.delete(recordsId.ToArray());
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