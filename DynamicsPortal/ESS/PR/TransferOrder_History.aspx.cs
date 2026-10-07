using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferOrder_History : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "TransferOrderHistory";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!isPageAuthorizated)
                return;

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Transfer Order History"; 
                titleDiv.Style["font-weight"] = "bold";
            }

            if (!IsPostBack)
            {
                btnShipment.Enabled = false;
                btnReceive.Enabled = false;

                string transferId = Session["transferId"] as string;
                bindData(transferId);
            }
        }

        private void bindData(string _transferIds)
        {
            if (string.IsNullOrEmpty(_transferIds))
                return;

            string[] transferIdArray = _transferIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            TransferOrderJournal journal = new TransferOrderJournal();

            DataTable mergedDt = null;

            foreach (string id in transferIdArray)
            {
                string transferId = id.Trim();
                DataTable dt = journal.retrieveAll(transferId);

                if (dt != null)
                {
                    if (mergedDt == null)
                    {
                        mergedDt = dt.Clone();
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        mergedDt.ImportRow(row);
                    }
                }
            }

            if (mergedDt != null)
            {
                gridView.DataSource = mergedDt;
                gridView.DataBind();

                if (gridView.Rows.Count > 0)
                {
                    // Auto-check the row for the first transferId
                    string firstTransferId = transferIdArray[0].Trim();

                    foreach (GridViewRow row in gridView.Rows)
                    {
                        Label lbl = row.FindControl("lbltransferNumber") as Label;
                        CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;

                        if (lbl != null && chk != null && lbl.Text == firstTransferId)
                        {
                            chk.Checked = true;
                            chk_SelectSingle_CheckedChanged(chk, EventArgs.Empty);
                            break;
                        }
                    }
                }
            }
        }


        private void bindLinesData(string _transferId, string _voucherId)
        {
            TransferOrderJournal journal = new TransferOrderJournal();
            DataTable dtLines = journal.retrieveAllLines(_transferId, _voucherId);
            gridView1.DataSource = dtLines;
            gridView1.DataBind();
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            TransferOrderJournal journal = new TransferOrderJournal();

            if (chk.Checked)
            {
                // Uncheck all other checkboxes
                foreach (GridViewRow gvRow in gridView.Rows)
                {
                    if (gvRow != row)
                    {
                        CheckBox otherChk = gvRow.FindControl("chk_SelectSingle") as CheckBox;
                        if (otherChk != null)
                        {
                            otherChk.Checked = false;
                        }
                    }
                }

                // Bind data for this selected row
                string transferId = ((Label)row.FindControl("lbltransferNumber")).Text;
                string voucher = ((Label)row.FindControl("lblVoucher")).Text;

                bindLinesData(transferId, voucher);
                if (journal.isShipmentAllowed(transferId, voucher))
                    btnCancel.Enabled = true;
                else
                    btnCancel.Enabled = false;

                // Enable Shipment / Receive based on the row's Update Type
                string updateType = (row.FindControl("lblUpdateType") as Label)?.Text.Trim();

                bool isShipment = updateType == "Shipment";
                bool isReceive = updateType == "Receive";

                btnShipment.Enabled = isShipment;
                btnShipment.CssClass = isShipment ? "" : "disabled-button";

                btnReceive.Enabled = isReceive;
                btnReceive.CssClass = isReceive ? "" : "disabled-button";
            }
            else
            {
                bool anyChecked = false;
                foreach (GridViewRow gvRow in gridView.Rows)
                {
                    CheckBox otherChk = gvRow.FindControl("chk_SelectSingle") as CheckBox;
                    if (otherChk != null && otherChk.Checked)
                    {
                        anyChecked = true;
                        break;
                    }
                }

                if (!anyChecked)
                {
                    bindLinesData("", "");
                    btnShipment.Enabled = false;
                    btnShipment.CssClass = "disabled-button";
                    btnReceive.Enabled = false;
                    btnReceive.CssClass = "disabled-button";
                }
            }
        }

        private void enabledCancelButton()
        {
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string transferId = "";
            string voucher = "";
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lbltransferNumber = gridViewRow.FindControl("lbltransferNumber") as Label;
                    Label lblVoucher = gridViewRow.FindControl("lblVoucher") as Label;

                    transferId = lbltransferNumber != null ? lbltransferNumber.Text.Trim() : "";
                    voucher = lblVoucher != null ? lblVoucher.Text.Trim() : "";

                    if (!string.IsNullOrEmpty(transferId))
                    {
                        TransferOrderJournal journal = new TransferOrderJournal();
                        journal.cancelTransferOrderShipment(transferId, voucher);
                        break;
                    }
                }
            }
        }

        protected void btnShipment_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    string updateType = (row.FindControl("lblUpdateType") as Label)?.Text.Trim();
                    if (updateType != "Shipment")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertWrongTypeShipment",
                            "alert('The selected record is not a Shipment update.');", true);
                        return;
                    }

                    string transferId = (row.FindControl("lbltransferNumber") as Label)?.Text.Trim();
                    string voucherId = (row.FindControl("lblVoucher") as Label)?.Text.Trim();
                    string postingDateText = (row.FindControl("lblPostingDate") as Label)?.Text.Trim();

                    DateTime postingDate;
                    if (!DateTime.TryParse(postingDateText, out postingDate))
                    {
                        postingDate = DateTime.MinValue;
                    }

                    Session["ShipmentTransferId"] = transferId;
                    Session["ShipmentVoucherId"] = voucherId;
                    Session["ShipmentPostingDate"] = postingDate;

                    isSelected = true;

                    Response.Redirect("/ESS/PR/TransferOrderShipmentReport.aspx");
                    return;
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('Please select a record to view the Shipment report.');", true);
            }
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    string updateType = (row.FindControl("lblUpdateType") as Label)?.Text.Trim();
                    if (updateType != "Receive")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertWrongTypeReceive",
                            "alert('The selected record is not a Receive update.');", true);
                        return;
                    }

                    string transferId = (row.FindControl("lbltransferNumber") as Label)?.Text.Trim();
                    string voucherId = (row.FindControl("lblVoucher") as Label)?.Text.Trim();
                    string postingDateText = (row.FindControl("lblPostingDate") as Label)?.Text.Trim();

                    DateTime postingDate;
                    if (!DateTime.TryParse(postingDateText, out postingDate))
                    {
                        postingDate = DateTime.MinValue;
                    }

                    Session["ReceiveTransferId"] = transferId;
                    Session["ReceiveVoucherId"] = voucherId;
                    Session["ReceivePostingDate"] = postingDate;

                    isSelected = true;

                    Response.Redirect("/ESS/PR/TransferOrderReceiveReport.aspx");
                    return;
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelectionReceive",
                    "alert('Please select a record to view the Receive report.');", true);
            }
        }
    }
}
