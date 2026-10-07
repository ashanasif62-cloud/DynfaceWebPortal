using BussinessObject;
using DynamicsPortal.ESS.EM;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TransferOrderHeaderSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class TransferOrder_Shipment : ModalForm
    {
        TransferOrderHeader transfer = new TransferOrderHeader();
        TransferOrderLines transferLines = new TransferOrderLines();
        string transferId;
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferOrder_Shipment";
                Page.Title = "Shipment";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Shipment"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                //base.Page_Load(sender, e);

                //if (!isUserAuthenticated)
                //    return;

                string transferIdsQuery = Request.QueryString["transferIds"];
                List<string> transferIdList = new List<string>();

                if (!string.IsNullOrEmpty(transferIdsQuery))
                {
                    transferIdList = transferIdsQuery.Split(',').ToList();
                }

                if (!IsPostBack)
                {
                    txtTblPostingDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    reBindGrid();
                    selectFirstRowAndBindLines();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }

        //protected void bindLinesGrid()
        //{
        //    DataTable dtLines = ViewState["TransferLines"] as DataTable;
        //    gridView1.DataSource = dtLines;
        //    gridView1.DataBind();
        //}

        protected void bindLinesGrid()
        {
            DataTable dtLines = ViewState["TransferLines"] as DataTable;
            if (dtLines == null || dtLines.Rows.Count == 0)
                return;

            string updateType = GetUpdateTypeFromOtherGrid();
            DataTable filteredTable = dtLines.Clone();

            foreach (DataRow row in dtLines.Rows)
            {
                decimal qtyShipNow = Convert.ToDecimal(row["QtyShipNow"]);
                decimal qtyRemainShip = Convert.ToDecimal(row["QtyRemainShip"]);
                decimal qtyShipped = Convert.ToDecimal(row["Qtyshipped"]);
                decimal qtyTransfer = Convert.ToDecimal(row["QtyTransfer"]);

                if (updateType == "0" && qtyShipNow > 0)
                {
                    // Ship now: include row and update QtyTransfer = QtyRemainShip
                    DataRow newRow = filteredTable.NewRow();
                    newRow.ItemArray = row.ItemArray.Clone() as object[];
                    newRow["QtyTransfer"] = qtyShipNow;
                    filteredTable.Rows.Add(newRow);
                }
                else if (updateType == "1" || updateType == "2")
                {
                    // All: include row, optionally update QtyTransfer
                    DataRow newRow = filteredTable.NewRow();
                    newRow.ItemArray = row.ItemArray.Clone() as object[];
                    if (qtyShipped != qtyTransfer && qtyRemainShip > 0)
                    {
                        newRow["QtyTransfer"] = qtyRemainShip;
                        filteredTable.Rows.Add(newRow);
                    }
                }
            }   

            gridView1.DataSource = filteredTable;
            gridView1.DataBind();
        }



        protected void bindGrid()
        {
            DataTable dt = ViewState["TransferID"] as DataTable;
            gridView.DataSource = dt;
            gridView.DataBind();
            DataTable dtLines = ViewState["TransferLines"] as DataTable;
            gridView1.DataSource = dtLines;
            gridView1.DataBind();
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        private void getGridDataTable()
        {
            string transferIdsQuery = Request.QueryString["transferIds"];
            List<string> transferIdList = new List<string>();

            if (!string.IsNullOrEmpty(transferIdsQuery))
            {
                transferIdList = transferIdsQuery.Split(',').ToList();
            }

            // Initialize Transfer Order Data Table
            DataTable dt = new DataTable();
            dt.Columns.Add("TransferId", typeof(string));
            dt.Columns.Add("RecId", typeof(Int64));

            foreach (string id in transferIdList)
            {
                // Add row for header grid
                dt.Rows.Add(id, 123456789); // Replace RecId as needed
            }

            ViewState["TransferID"] = dt;
            ViewState["TransferLines"] = transferLines.retrieveAll("");
        }

        protected void chk_updateDimensions(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridView1.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;

                if (chk != null && chk.Checked)
                {
                    txtLineNum.Text = (row.FindControl("lblLineNum") as Label).Text;
                    txtTransferNum.Text = (row.FindControl("lblTransferId") as Label).Text;
                    txtItemNumber.Text = (row.FindControl("lblItemNumber") as Label).Text;
                    txtInvConfig.Text = ((Label)row.FindControl("lblConfigId"))?.Text;
                    txtInvSize.Text = ((Label)row.FindControl("lblInventSizeId"))?.Text;
                    txtInvStyle.Text = ((Label)row.FindControl("lblInventStyle"))?.Text;
                    txtInvWarehouse.Text = ((Label)row.FindControl("lblInventLocationId"))?.Text;
                    txtInvLocation.Text = ((Label)row.FindControl("lblWMSLocationId"))?.Text;
                    txtInvStatus.Text = ((Label)row.FindControl("lblTransferStatus"))?.Text;
                    txtInvLicensePlate.Text = ((Label)row.FindControl("lblWMSPalletId"))?.Text;
                    txtInvColor.Text = ((Label)row.FindControl("lblInventColorId"))?.Text;
                    txtInvSite.Text = ((Label)row.FindControl("lblInventSiteId"))?.Text;
                    txtInvBatch.Text = ((Label)row.FindControl("lblInventBatchId"))?.Text;
                    txtInvSerial.Text = ((Label)row.FindControl("lblInventSerialId"))?.Text;
                    txtInvOwner.Text = ((Label)row.FindControl("lblNewTransitLocationName"))?.Text;
                }
                else
                {
                    txtLineNum.Text = "";
                    txtTransferNum.Text = "";
                    txtItemNumber.Text = "";
                    txtInvConfig.Text = "";
                    txtInvSize.Text = "";
                    txtInvStyle.Text = "";
                    txtInvWarehouse.Text = "";
                    txtInvLocation.Text = "";
                    txtInvStatus.Text = "";
                    txtInvLicensePlate.Text = "";
                    txtInvColor.Text = "";
                    txtInvSite.Text = "";
                    txtInvBatch.Text = "";
                    txtInvSerial.Text = "";
                    txtInvOwner.Text = "";
                }
            }
        }

        protected void chk_UpdateLineGrid(object sender, EventArgs e)
        {
            bool isAnySelected = false;
            string selectedTransferId = null;

            // Uncheck all checkboxes except the one that triggered the event
            CheckBox clickedCheckBox = sender as CheckBox;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                Label lblTransferId = row.FindControl("lblgridTransferNumber") as Label;

                if (chk != null && chk != clickedCheckBox)
                {
                    chk.Checked = false; // uncheck all others
                }

                if (chk != null && chk.Checked && lblTransferId != null)
                {
                    selectedTransferId = lblTransferId.Text.Trim();
                    isAnySelected = true;
                }
            }

            if (isAnySelected && !string.IsNullOrEmpty(selectedTransferId))
            {
                ViewState["TransferLines"] = transferLines.retrieveAll(selectedTransferId);
                bindLinesGrid();
            }
            else
            {
                ViewState["TransferLines"] = new DataTable(); // clear it
                gridView1.DataSource = null;
                gridView1.DataBind();
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                TransferOrderHeaderContract contract = new TransferOrderHeaderContract();
                TransferOrderHeader orders = new TransferOrderHeader();

                foreach (GridViewRow row in gridView.Rows)
                {
                    Label lblTransferId = row.FindControl("lblgridTransferNumber") as Label;
                    TextBox lblgridPostingDate = row.FindControl("lblgridPostingDate") as TextBox;
                    
                    transferId = lblTransferId.Text.Trim();
                    string postingDate = lblgridPostingDate.Text.Trim();
                    DataTable dt = GetGridViewData(gridView1);
                    TransferOrderHeaderContract shipRequestContract = orders.prepareHeaderContract(transferId, dt, postingDate);

                    objBOL = orders.shipTransferOrder(shipRequestContract);

                    if (objBOL.isSuccess)
                    {
                        objBOL.AlertType = AlertType.Information.ToString();
                        objBOL.Message = "Operation Completed";
                        NotificationMessage.showMessage(objBOL);


                        DataTable bindData = orders.retrieveAll();
                        var bindDatarow = bindData.AsEnumerable()
                  .                 FirstOrDefault(r => r.Field<string>("TransferId") == transferId);

                        if (bindDatarow != null)
                        {
                            string status = bindDatarow.Field<string>("InventTransferStatus");
                            Session["Status"] = status;
                        }
                        string script = @"
                        setTimeout(function() { 
                            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                                window.parent.refreshParentGrid();
                            }
                            closeDialog(); 
                        }, 3000);";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
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
        }

        private void selectFirstRowAndBindLines()
        {
            DataTable dt = ViewState["TransferID"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string firstTransferId = dt.Rows[0]["TransferId"].ToString();
                transferId = firstTransferId;

                // Store and bind lines for first row
                ViewState["TransferLines"] = transferLines.retrieveAll(transferId);

                // Optional: visually highlight first row or mark checkbox
                gridView.Rows[0].CssClass += " selected-row";
                CheckBox chk = gridView.Rows[0].FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null)
                {
                    chk.Checked = true;
                }
                bindLinesGrid();
            }
        }

        private DataTable GetGridViewData(GridView gridView)
        {
            DataTable dt = new DataTable();

            // Dynamically create columns from GridView header row
            foreach (DataControlField column in gridView.Columns)
            {
                if (column is BoundField)
                {
                    dt.Columns.Add(((BoundField)column).DataField);
                }
                else if (column is TemplateField templateField)
                {
                    // Add column with header text or ID of inner control
                    string headerText = templateField.HeaderText ?? "UnnamedColumn";
                    dt.Columns.Add(headerText);
                }
            }

            // Loop through each row in GridView
            foreach (GridViewRow row in gridView.Rows)
            {
                DataRow dr = dt.NewRow();

                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    DataControlField column = gridView.Columns[i];

                    if (column is BoundField bf)
                    {
                        dr[bf.DataField] = row.Cells[i].Text.Trim();
                    }
                    else if (column is TemplateField)
                    {
                        // Attempt to read from Label, TextBox, or CheckBox inside TemplateField
                        var cell = row.Cells[i];
                        string value = "";

                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is Label lbl)
                                value = lbl.Text.Trim();
                            else if (ctrl is TextBox txt)
                                value = txt.Text.Trim();
                            else if (ctrl is CheckBox cb)
                                value = cb.Checked.ToString();

                            if (!string.IsNullOrEmpty(value)) break;
                        }

                        dt.Columns[i].ColumnName = dt.Columns[i].ColumnName.Replace(" ", ""); // Optional: sanitize
                        dr[i] = value;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected void lblShipQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txtChanged = sender as TextBox;
            if (txtChanged != null)
            {
                txtShipNow.Text = txtChanged.Text;
            }
        }

        private string GetUpdateTypeFromOtherGrid()
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                DropDownList ddlUpdateType = row.FindControl("ddlgridUpdateType") as DropDownList;

                if (chk != null && chk.Checked && ddlUpdateType != null)
                {
                    return ddlUpdateType.SelectedValue.Trim(); // returns "0", "1", or "2"
                }
            }

            return string.Empty;
        }

        protected void ddlgridUpdateType_bindLinesGrid(object sender, EventArgs e)
        {
            bindLinesGrid();
        }
    }
}