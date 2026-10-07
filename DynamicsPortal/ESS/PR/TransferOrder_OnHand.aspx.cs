using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferOrder_OnHand : ModalForm
    {
        private TransferOrderLines newTransferorderlines = new TransferOrderLines();
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "TransferOrder_OnHand";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!isPageAuthorizated)
                return;
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "On-hand";
                titleDiv.Style["font-weight"] = "bold";
            }
            BindOnHandInventory();
        }


        private void BindOnHandInventory()
        {

            string itemId = Session["ItemId"] as string;
            string inventDimId = Session["InventDimId"] as string;
            string siteId = Session["SiteId"] as string;
            string locationId = Session["Warehouse"] as string;

            DataTable dt = newTransferorderlines.retrieveOnhandInventory(itemId, inventDimId);



            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtProductName.Text = (Session["ItemName"] as string ?? string.Empty).Split('/')[0].Trim();


                // Inventory Dimensions
                txtConfigId.Text = Session["ConfigId"] as string ?? string.Empty;
                txtSizeId.Text = Session["SizeId"] as string ?? string.Empty;
                txtColorId.Text = Session["ColorId"] as string ?? string.Empty;
                txtStyleId.Text = Session["StyleId"] as string ?? string.Empty;
                txtVersionId.Text = Session["VersionId"] as string ?? string.Empty; // if you store VersionId in Session later
                txtSiteId.Text = Session["SiteId"] as string ?? string.Empty;
                txtWareHouse.Text = Session["Warehouse"] as string ?? string.Empty;
                txtBatchNum.Text = Session["BatchNum"] as string ?? string.Empty;
                txtLocationId.Text = Session["WMSLocationId"] as string ?? string.Empty;
                txtSerialNum.Text = Session["SerialNum"] as string ?? string.Empty;
                txtInventStatusId.Text = Session["InventStatusId"] as string ?? string.Empty; // if you store Inventory status in Session

                // Unit
                txtUnitId.Text = Session["UnitId"] as string ?? string.Empty;

                // On-hand quantities
                txtQtyPhyInvent.Text = string.Format("{0:N2}", Convert.ToDecimal(row["PhysicalInventory"] ?? 0));
                txtQtyPhyReserve.Text = string.Format("{0:N2}", Convert.ToDecimal(row["ReservePhysical"] ?? 0));
                txtQtyPhyAvailable.Text = string.Format("{0:N2}", Convert.ToDecimal(row["AvailPhysical"] ?? 0));
                txtQtyAvailForReservation.Text = string.Format("{0:N2}", Convert.ToDecimal(row["AvailReservation"] ?? 0));
                txtQtyTotalOrdered.Text = string.Format("{0:N2}", Convert.ToDecimal(row["OrderedInTotal"] ?? 0));
                txtOnorderinttoal.Text = string.Format("{0:N2}", Convert.ToDecimal(row["OnOrderTotal"] ?? 0));
                txtQtyTotalAvailable.Text = string.Format("{0:N2}", Convert.ToDecimal(row["AvailTotal"] ?? 0));

                txtQtyPosted.Text = string.Format("{0:N2}", Convert.ToDecimal(row["PhysicalInventory"] ?? 0));
                txtQtyOrdered.Text = string.Format("{0:N2}", Convert.ToDecimal(row["OrderedInTotal"] ?? 0));
                txtQtyOnOrder.Text = string.Format("{0:N2}", Convert.ToDecimal(row["OnOrderTotal"] ?? 0));



            }
        }
    }
}