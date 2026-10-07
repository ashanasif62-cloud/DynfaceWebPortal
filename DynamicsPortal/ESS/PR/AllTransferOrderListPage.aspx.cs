using BussinessObject;
using GeneralAuxiliary;
using OfficeOpenXml;
using PortalIntegration;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class AllTransferOrderListPage : MainForm
    {
        private TransferOrderHeader newTransferorder = new TransferOrderHeader();

        private TransferOrderLines newlines = new TransferOrderLines();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = newTransferorder.tableName;
                pageMenuId = "AllTransferOrder_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Transfer orders";

                    // Dynamically set the page title in the master page div
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Transfer orders";
                    }

                    reBindGrid();
                   
                }

                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "TransferClick")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridView.Rows[rowIndex];

                string transferId = ((LinkButton)row.FindControl("lnkTransferID")).Text;

                string fromWarehouse = ((Label)row.FindControl("lblFromWarehouse")).Text;

                string toWarehouse = ((Label)row.FindControl("lblToWarehouse")).Text;

                string Status = ((Label)row.FindControl("lblStatus")).Text;

                // Get new fields
                string fromWarehouseName = ((Label)row.FindControl("lblFromWarehouseName")).Text;
                string toWarehouseName = ((Label)row.FindControl("lblToWarehouseName")).Text;
                string fromAddress = ((Label)row.FindControl("lblFromAddress")).Text;
                string toAddress = ((Label)row.FindControl("lblToAddress")).Text;
                string createddateandtime = ((Label)row.FindControl("lblCreatedDateandTime")).Text;

                Label lblShipDate = (Label)row.FindControl("lblShipDate");
                if (lblShipDate != null)
                {
                    Session["ShipDate"] = lblShipDate.Text; // stores value in session
                }

                Label lblReceiveDate = (Label)row.FindControl("lblReceiveDate");
                if (lblReceiveDate != null)
                {
                    Session["ReceiveDate"] = lblReceiveDate.Text;
                }

                Session["Status"] = Status;

                // Store in Session
                Session["FromWarehouse"] = fromWarehouse;

                Session["ToWarehouse"] = toWarehouse;

                Session["FromWarehouseName"] = fromWarehouseName;
                Session["ToWarehouseName"] = toWarehouseName;
                Session["FromAddress"] = fromAddress;
                Session["ToAddress"] = toAddress;
                Session["CreatedDateAndTime"] = createddateandtime;

                // Redirect
                Session["TransferID"] = transferId;
                Response.Redirect("~/ESS/PR/TransferOrderLines_ListPage.aspx");
            }
        }

        private void getGridDataTable()
        {
            DataTable dt = newTransferorder.retrieveAll(true);
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Columns.Contains("ShipDate") && row["ShipDate"] != DBNull.Value)
                {
                    row["ShipDate"] = Convert.ToDateTime(row["ShipDate"]).ToString("M-dd-yyyy");
                }

                if (dt.Columns.Contains("ReceiveDate") && row["ReceiveDate"] != DBNull.Value)
                {
                    row["ReceiveDate"] = Convert.ToDateTime(row["ReceiveDate"]).ToString("M-dd-yyyy");
                }
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

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {

                    DataTable dataTable = newTransferorder.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["TransferID"] = ((LinkButton)gridViewRow.FindControl("lnkTransferID")).Text;
                    //dr["FromWarehouse"] = ((DropDownList)gridViewRow.FindControl("ddlFromWarehouse")).SelectedValue;
                    //dr["ToWarehouse"] = ((DropDownList)gridViewRow.FindControl("ddlToWarehouse")).SelectedValue;
                    dr["ShipDate"] = ((TextBox)gridViewRow.FindControl("txtShipDate")).Text;
                    dr["ReceiveDate"] = ((TextBox)gridViewRow.FindControl("txtReceiveDate")).Text;

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    dataTable.Rows.Add(dr);

                    SysOperationResult_BOL operationResult_BOL = newTransferorder.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = newTransferorder.delete(recordsId.ToArray());
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select at least one transfer to proceed.');", true);
            }
        }

        protected void btnShip_Click(object sender, EventArgs e)
        {
            List<string> selectedTransferIds = new List<string>();

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkTransferID = gridViewRow.FindControl("lnkTransferID") as LinkButton;

                    string transferId = lnkTransferID != null ? lnkTransferID.Text.Trim() : "";

                    if (!string.IsNullOrEmpty(transferId))
                    {
                        selectedTransferIds.Add(transferId);
                    }
                }
            }

            if (selectedTransferIds.Count > 0)
            {
                // Join transfer IDs into a single comma-separated string and URL encode it
                string joinedIds = string.Join(",", selectedTransferIds.Select(HttpUtility.UrlEncode));
                string script = $"openPopupPanel('/ESS/PR/TransferOrder_Shipment.aspx?transferIds={joinedIds}', 1000);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                    "alert('Please select at least one transfer to proceed.');", true);
            }
        }

        protected void btnRecieve_Click(object sender, EventArgs e)
        {
            List<string> selectedTransferIds = new List<string>();

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    LinkButton lnkTransferID = gridViewRow.FindControl("lnkTransferID") as LinkButton;

                    string transferId = lnkTransferID != null ? lnkTransferID.Text.Trim() : "";

                    if (!string.IsNullOrEmpty(transferId))
                    {
                        selectedTransferIds.Add(transferId);
                    }
                }
            }

            if (selectedTransferIds.Count > 0)
            {
                // Join transfer IDs into a single comma-separated string and URL encode it
                string joinedIds = string.Join(",", selectedTransferIds.Select(HttpUtility.UrlEncode));
                string script = $"openPopupPanel('/ESS/PR/TransferOrder_Recieve.aspx?transferIds={joinedIds}', 1000);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopup", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                    "alert('Please select at least one transfer to proceed.');", true);
            }
        }

        protected void btnRecieve_All_Click(object sender, EventArgs e)
        {
            TransferOrderHeader transfer = new TransferOrderHeader();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();
            bool selectedRecords = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    selectedRecords = true;
                    string transferId = "";
                    transferId = ((LinkButton)gridViewRow.FindControl("lnkTransferID")).Text;
                    if (!string.IsNullOrEmpty(transferId))
                    {
                        operationResult_BOL = transfer.recieveTransferOrder(transferId);
                    }
                }
            }
            if (selectedRecords)
            {
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select at least one transfer to proceed.');", true);
            }
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;

            // Check if the row is a DataRow
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;
            if (gridViewRow.RowIndex == 0 && !IsPostBack) // only first load
            {
                CheckBox chk = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null)
                {
                    chk.Checked = true;
                }


            }

  
            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {

                DropDownList ddlFromWarehouse = (DropDownList)e.Row.FindControl("ddlFromWarehouse");
                if (ddlFromWarehouse != null)
                {
                    TransferOrderHeader transferordernew = new TransferOrderHeader();
                    DataTable dt = transferordernew.retrievefromwarehouse();

                    ddlFromWarehouse.DataSource = dt;
                    ddlFromWarehouse.DataValueField = "FromWarehouse";
                    ddlFromWarehouse.DataTextField = "FromWarehouse";
                    ddlFromWarehouse.DataBind();

                    // Optional: Preselect current value
                    string selectedValue = DataBinder.Eval(e.Row.DataItem, "FromWarehouse")?.ToString();
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        ddlFromWarehouse.SelectedValue = selectedValue;
                    }
                    DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                }

                DropDownList ddlToWarehouse = (DropDownList)e.Row.FindControl("ddlToWarehouse");
                if (ddlToWarehouse != null)
                {

                    TransferOrderHeader transferordernew = new TransferOrderHeader();
                    DataTable dt = transferordernew.retrievetowarehouse();

                    ddlToWarehouse.DataSource = dt;
                    ddlToWarehouse.DataValueField = "ToWarehouse";
                    ddlToWarehouse.DataTextField = "ToWarehouse";
                    ddlToWarehouse.DataBind();

                    string selectedValue = DataBinder.Eval(e.Row.DataItem, "ToWarehouse")?.ToString();
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        ddlToWarehouse.SelectedValue = selectedValue;
                    }
                }
            }
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool isAnyRowSelected = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    isAnyRowSelected = true;
                    string status = ((Label)gridViewRow.FindControl("lblStatus")).Text;
                    string transferId = ((LinkButton)gridViewRow.FindControl("lnkTransferID")).Text;
                    if (!string.IsNullOrEmpty(status))
                    {
                        switch (status)
                        {
                            case "Created":
                                btnReceive.Enabled = false;
                                //btnRecieve_All.Enabled = false;
                                btnDelete.Enabled = true;
                                btnShip_All.Enabled = true;
                                if (!string.IsNullOrEmpty(transferId))
                                {
                                    TransferOrderLines checkShipment = new TransferOrderLines();
                                    DataTable linesData = checkShipment.retrieveAll(transferId);
                                    foreach (DataRow row in linesData.Rows)
                                    {
                                        if (linesData.Columns.Contains("Qtyshipped") && linesData.Columns.Contains("QtyRecieved"))
                                        {
                                            var shippedQty = row["Qtyshipped"];
                                            var recievedQty = row["QtyRecieved"];
                                            int shippedQtyValue = shippedQty != DBNull.Value ? Convert.ToInt32(shippedQty) : 0;
                                            int recievedQtyValue = recievedQty != DBNull.Value ? Convert.ToInt32(recievedQty) : 0;
                                            int remainingValue = shippedQtyValue - recievedQtyValue;
                                            if (remainingValue > 0)
                                            {
                                                //btnRecieve_All.Enabled = true;
                                                btnReceive.Enabled = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "Shipped":
                                btnReceive.Enabled = true;
                                //btnRecieve_All.Enabled = true;
                                btnShip_All.Enabled = false;
                                btnDelete.Enabled = false;
                                break;
                            case "Received":
                                btnDelete.Enabled = false;
                                btnShip_All.Enabled = false;
                                btnReceive.Enabled = false;
                                //btnRecieve_All.Enabled = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
                // If no checkboxes are checked, enable all buttons
                if (!isAnyRowSelected)
                {
                    btnDelete.Enabled = true;
                    btnReceive.Enabled = true;
                    //btnRecieve_All.Enabled = true;
                    btnShip_All.Enabled = true;
                }
            }

        }
        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        protected void btnHistory_Click(object sender, EventArgs e)
        {
            bool selectedRecords = false;
            string transferIds = "";

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    selectedRecords = true;
                    string transferId = ((LinkButton)gridViewRow.FindControl("lnkTransferID")).Text;

                    if (!string.IsNullOrEmpty(transferId))
                    {
                        if (string.IsNullOrEmpty(transferIds))
                            transferIds = transferId;
                        else
                            transferIds += "," + transferId;
                    }
                }
            }
            if (selectedRecords)
            {
                Session["transferId"] = transferIds;
                Response.Redirect(ResolveUrl("~/ESS/PR/TransferOrder_History.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                "alert('Please select at least one transfer to proceed.');", true);
            }
        }
    }



}

       

       
                 