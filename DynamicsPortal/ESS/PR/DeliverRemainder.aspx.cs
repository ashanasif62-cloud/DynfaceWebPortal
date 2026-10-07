using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TransferOrderHeaderSvcReference;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class DeliverRemainder : ModalForm
    {
        private readonly TransferOrderLines _linesSvc = new TransferOrderLines();

        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Update Remaining Quantity";
            }

            if (!IsPostBack)
            {
                LoadSelectedLine();           // show all lines

            }
        }

        //private void BindInitialValues()
        //{
        //    string transferId = Session["TransferID"] as string;
        //    TransferOrderLines lines = new TransferOrderLines();

        //    DataTable dt = lines.retrieveAll(transferId);

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        DataRow row = dt.Rows[0];
        //        lblTransferQuantity.Text = string.Format("{0:0.00}", Convert.ToDecimal(row["QtyTransfer"]));
        //        lblShipQuantity.Text = string.Format("{0:0.00}", Convert.ToDecimal(row["QtyShipNow"]));
        //        lblShipRemain.Text = string.Format("{0:0.00}", Convert.ToDecimal(row["QtyRemainShip"]));
        //        lblShipRemain1.Text = string.Format("{0:0.00}", Convert.ToDecimal(row["QtyRemainShip"]));
        //        Session["QtyRemainShip"] = lblShipRemain1.Text;
        //        lblTotalShipment.Text = string.Format("{0:0.00}", Convert.ToDecimal(row["QtyTransfer"]));
        //    }
        //    else
        //    {
        //        lblTransferQuantity.Text = "";
        //        lblShipQuantity.Text = "";
        //        lblShipRemain.Text = "";
        //        lblShipRemain1.Text = "";
        //        lblTotalShipment.Text = "";
        //    }
        //}




        //private void LoadLineIntoFormByRecId(long recId)
        //{
        //    string transferId = Session["TransferID"] as string;

        //    if (string.IsNullOrWhiteSpace(transferId))
        //    {
        //        NotificationMessage.showMessage("TransferID not found in session.");
        //        return;
        //    }

        //    DataTable dt = _linesSvc.retrieveAll(transferId);
        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        ClearForm();
        //        return;
        //    }

        //    DataRow[] rows = dt.Select("RecId = " + recId);
        //    if (rows.Length == 0)
        //    {
        //        ClearForm();
        //        NotificationMessage.showMessage("Selected line not found.");
        //        return;
        //    }

        //    DataRow row = rows[0];

        //    decimal qtyTransfer = SafeGetDecimal(row, "QtyTransfer");
        //    decimal qtyShipNow = SafeGetDecimal(row, "QtyShipNow");
        //    decimal qtyRemain = SafeGetDecimal(row, "QtyRemainShip");

        //    lblTransferQuantity.Text = qtyTransfer.ToString("0.00", CultureInfo.InvariantCulture);
        //    lblShipQuantity.Text = qtyShipNow.ToString("0.00", CultureInfo.InvariantCulture);
        //    lblShipRemain.Text = qtyRemain.ToString("0.00", CultureInfo.InvariantCulture);
        //    lblShipRemain1.Text = qtyRemain.ToString("0.00", CultureInfo.InvariantCulture);
        //    lblTotalShipment.Text = qtyTransfer.ToString("0.00", CultureInfo.InvariantCulture);

        //    Session["QtyRemainShip"] = lblShipRemain1.Text;
        //}

        private void LoadSelectedLine()
        {
            if (Session["RecId"] == null || string.IsNullOrWhiteSpace(Session["RecId"].ToString()))
                return;

            long recId;
            if (!long.TryParse(Session["RecId"].ToString(), out recId))
                return;

            string transferId = Session["TransferID"] as string;
            if (string.IsNullOrWhiteSpace(transferId))
                return;

            DataTable dt = _linesSvc.retrieveAll(transferId);
            if (dt == null || dt.Rows.Count == 0)
                return;

            DataRow[] rows = dt.Select("RecId = " + recId);
            if (rows.Length == 0)
                return;

            DataRow row = rows[0];

            lblTransferQuantity.Text = SafeGetDecimal(row, "QtyTransfer").ToString("0.00", CultureInfo.InvariantCulture);
            lblShipQuantity.Text = SafeGetDecimal(row, "QtyShipNow").ToString("0.00", CultureInfo.InvariantCulture);
            lblShipRemain.Text = SafeGetDecimal(row, "QtyRemainShip").ToString("0.00", CultureInfo.InvariantCulture);
            lblShipRemain1.Text = SafeGetDecimal(row, "QtyRemainShip").ToString("0.00", CultureInfo.InvariantCulture);
            lblTotalShipment.Text = SafeGetDecimal(row, "QtyTransfer").ToString("0.00", CultureInfo.InvariantCulture);

            Session["QtyRemainShip"] = lblShipRemain1.Text;
        }





        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["RecId"] == null)
                {
                    NotificationMessage.showMessage("No line selected.");
                    return;
                }

                long recId = Convert.ToInt64(Session["RecId"]);
                decimal qtyRemainShip;

                if (!decimal.TryParse(lblShipRemain1.Text.Trim(), out qtyRemainShip))
                {
                    NotificationMessage.showMessage("Invalid Ship Remain value.");
                    return;
                }

                var contract = new TransferOrderUpdateRemainContract
                {
                    RecId = recId,
                    QtyRemainShip = qtyRemainShip
                };

                SysOperationResult_BOL result = _linesSvc.updateDeliverRemainder(contract);

                if (result != null && result.isSuccess)
                {
                    NotificationMessage.showMessage(result);
                    lblShipRemain.Text = qtyRemainShip.ToString("0.00", CultureInfo.InvariantCulture);
                    LoadSelectedLine(); // refresh values
                }
                else
                {
                    string msg = (result != null && !string.IsNullOrWhiteSpace(result.Message))
                        ? result.Message
                        : "Failed to update delivery remainder.";
                    NotificationMessage.showMessage(msg);
                }
            }
            catch (Exception ex)
            {
                NotificationMessage.showMessage("Error: " + ex.Message);
            }
        }



        protected void btnCancel_Quantity_Click(object sender, EventArgs e)
        {
            lblShipRemain1.Text = "0.00";
        }

        private static decimal SafeGetDecimal(DataRow row, string colName)
        {
            if (row.Table.Columns.Contains(colName) && row[colName] != DBNull.Value)
            {
                decimal d;
                if (decimal.TryParse(Convert.ToString(row[colName]), NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                    return d;

                if (decimal.TryParse(Convert.ToString(row[colName]), NumberStyles.Any, CultureInfo.CurrentCulture, out d))
                    return d;
            }

            if (colName == "QtyShipNow" && row.Table.Columns.Contains("QtyShipped"))
            {
                return SafeGetDecimal(row, "QtyShipped");
            }

            return 0m;
        }
    }
}
