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

            DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
            string typeValue = dataRowView["Type"].ToString();

            // ── Type icon — always shown in read mode ─────────────────────────
            string iconClass = "mdi mdi-help-circle-outline";
            string iconColor = "#888";

            switch (typeValue)
            {
                case "Phone":
                    iconClass = "mdi mdi-phone";
                    iconColor = "#34a853";
                    break;
                case "Email":
                case "Email address":
                case "Personal E-Mail":
                    iconClass = "mdi mdi-email";
                    iconColor = "#ea4335";
                    break;
                case "URL":
                    iconClass = "mdi mdi-web";
                    iconColor = "#1a73e8";
                    break;
                case "Fax":
                    iconClass = "mdi mdi-fax";
                    iconColor = "#777";
                    break;
                case "Telex":
                    iconClass = "mdi mdi-telegraph";
                    iconColor = "#777";
                    break;
                case "Facebook":
                    iconClass = "mdi mdi-facebook";
                    iconColor = "#1877f2";
                    break;
                case "Twitter":
                    iconClass = "mdi mdi-twitter";
                    iconColor = "#1da1f2";
                    break;
                case "LinkedIn":
                    iconClass = "mdi mdi-linkedin";
                    iconColor = "#0a66c2";
                    break;
                case "None":
                default:
                    iconClass = "mdi mdi-minus-circle-outline";
                    iconColor = "#bbb";
                    break;
            }

            // Apply icon to lblType in read mode
            Label lblType = gridViewRow.FindControl("lblType") as Label;
            if (lblType != null)
            {
                lblType.Text = string.Format(
                    "<i class='{0}' style='color:{1}; font-size:16px; vertical-align:middle; margin-right:5px;'></i>{2}",
                    iconClass,
                    iconColor,
                    System.Web.HttpUtility.HtmlEncode(typeValue)
                );
            }

            // ── Edit mode ─────────────────────────────────────────────────────
            if ((gridViewRow.RowState & DataControlRowState.Edit) == 0)
                return;

            bool isNewRecord = string.IsNullOrEmpty(dataRowView["RecId"].ToString());
            DropDownList ddlType = gridViewRow.FindControl("ddlType") as DropDownList;
            if (isNewRecord)
            {
                ddlType.Items.Add(new ListItem("-- Select Type --", ""));
                BindTypeDropDown(ddlType, typeValue);  // replaces the manual Items.Add block
            }
            else
            {
                // Existing record — fill dropdown, pre-select current type, then lock it
                ddlType.Items.Add(new ListItem("-- Select Type --", ""));
                BindTypeDropDown(ddlType, typeValue);
                ddlType.Enabled = false;
            }

            // ── IsPrimary dropdown ────────────────────────────────────────────
            DropDownList ddlIsPrimary = gridViewRow.FindControl("ddlIsPrimary") as DropDownList;
            if (ddlIsPrimary != null)
            {
                ddlIsPrimary.Items.Clear();
                ddlIsPrimary.Items.Add(new ListItem("Yes", "true"));
                ddlIsPrimary.Items.Add(new ListItem("No", "false"));
                ddlIsPrimary.SelectedValue = Convert.ToBoolean(dataRowView["IsPrimary"]) ? "true" : "false";
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
                dr["Type"] = "Phone";
                dr["IsPrimary"] = false;
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

                    DropDownList ddlType = gridViewRow.FindControl("ddlType") as DropDownList;
                    string type = ddlType?.SelectedValue ?? string.Empty;
                    string typeLabel = ddlType?.SelectedItem?.Text ?? string.Empty;

                    bool isPrimary = Convert.ToBoolean((gridViewRow.FindControl("ddlIsPrimary") as DropDownList).SelectedValue);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (isPrimary)
                    {
                        DataTable dtViewState = getViewState_LogisticsElectronicAddress();
                        for (int i = 0; i < dtViewState.Rows.Count; i++)
                        {
                            if (i == rowIndex) continue;

                            DataRow row = dtViewState.Rows[i];
                            string rowvalue = row["Type"].ToString();
                            if ((row["Type"].ToString() == type || row["Type"].ToString() == typeLabel) && Convert.ToBoolean(row["IsPrimary"]))
                            {
                                SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                                resultValidate.isSuccess = false;
                                resultValidate.Message = "Only one record of the same type can be marked as Primary.";
                                resultValidate.AlertType = AlertType.Error.ToString();
                                operationResults(resultValidate);
                               // bindGrid();
                                return;
                            }
                        }
                    }

                    dr["Type"] = type;                              //C
                    dr["IsPrimary"] = isPrimary;

                    dataTable.Rows.Add(dr);

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


        private void BindTypeDropDown(DropDownList ddl, string selectedValue = "")
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Phone", "Phone"));
            ddl.Items.Add(new ListItem("Email address", "Email"));
            ddl.Items.Add(new ListItem("URL", "URL"));
            ddl.Items.Add(new ListItem("Fax", "Fax"));
            ddl.Items.Add(new ListItem("Telex", "Telex"));
            ddl.Items.Add(new ListItem("Facebook", "Facebook"));
            ddl.Items.Add(new ListItem("Twitter", "Twitter"));
            ddl.Items.Add(new ListItem("LinkedIn", "LinkedIn"));

            if (string.IsNullOrEmpty(selectedValue)) return;

            // Try value match first (e.g. "Email"), then text match (e.g. "Email address")
            ListItem match = ddl.Items.FindByValue(selectedValue)
                          ?? ddl.Items.FindByText(selectedValue);
            if (match != null) match.Selected = true;
        }
    }
}