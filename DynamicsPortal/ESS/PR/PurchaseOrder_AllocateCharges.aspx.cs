using PortalIntegration;
using PortalIntegration.PurchaseOrderAllocateChargesSvcReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_AllocateCharges : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Allocate Charges";
            }
            if (!IsPostBack)
            {
                // Populate Charges Allocation dropdown
                loadDropdowns();
                string purchId = Request.QueryString["PurchId"];
                if (!string.IsNullOrEmpty(purchId))
                {
                    Session["SelectedPurchId"] = purchId;
                }

            }

        }
        private void loadDropdowns()
        {
            // Charges Allocation
            ddlChargesAllocation.Items.Clear();
            ddlChargesAllocation.Items.Add(new ListItem("Net amount", "Net amount"));
            ddlChargesAllocation.Items.Add(new ListItem("Quantity", "Quantity"));
            ddlChargesAllocation.Items.Add(new ListItem("Per line", "Per line"));
            ddlChargesAllocation.SelectedIndex = 0;

            // Allocate To Lines
            ddlAllocateToLines.Items.Clear();
            ddlAllocateToLines.Items.Add(new ListItem("All lines", "All lines"));
            ddlAllocateToLines.Items.Add(new ListItem("Positive lines", "Positive lines"));
            ddlAllocateToLines.Items.Add(new ListItem("Negative lines", "Negative lines"));
            ddlAllocateToLines.SelectedIndex = 0;
        }

        protected void btnAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                string purchId = Request.QueryString["PurchId"];
                if (string.IsNullOrEmpty(purchId))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "alert",
                        "alert('Purchase Order not found.');", true);
                    return;
                }

                // ✅ Build CONTRACT (USE PARM METHODS)
                PO_AllocateChargesSvcContract contract = new PO_AllocateChargesSvcContract();

                contract.PurchId = purchId;
                contract.ChargesAllocation = ddlChargesAllocation.SelectedValue;
                contract.MarkupAllocateOn = ddlAllocateToLines.SelectedValue;
                contract.AllocateAll = chkAllocateAll.Checked;
                contract.Received = chkReceived.Checked;
                contract.Stocked = chkStocked.Checked;
                // contract.useSpecificLines = chkShowSelections.Checked;

                // ✅ Call service
                PO_AllocateCharges svc = new PO_AllocateCharges();
                GeneralContract result = svc.AllocateCharges(contract);

                if (result != null && result.IsSuccess)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "success",
                        "alert('Charges allocated successfully.'); closeDialog();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "error",
                        $"alert('{result?.Message ?? "Allocation failed"}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "error",
                    $"alert('{ex.Message}');", true);
            }
        }


      

        //public enum MarkupAllocateAfter
        //{
        //    NetAmount,
        //    Quantity,
        //    PerLine
        //}

        //// Matches ddlAllocateToLines
        //public enum MarkupAllocateOn
        //{
        //    All,
        //    Positive,
        //    Negative
        //}


        //private MarkupAllocateAfter getChargesAllocationEnum()
        //{
        //    switch (ddlChargesAllocation.SelectedValue)
        //    {
        //        case "Net amount":
        //            return MarkupAllocateAfter.NetAmount;

        //        case "Quantity":
        //            return MarkupAllocateAfter.Quantity;

        //        case "Per line":
        //            return MarkupAllocateAfter.PerLine;

        //        default:
        //            return MarkupAllocateAfter.NetAmount;
        //    }
        //}

        //private MarkupAllocateOn getAllocateToLinesEnum()
        //{
        //    switch (ddlAllocateToLines.SelectedValue)
        //    {
        //        case "All lines":
        //            return MarkupAllocateOn.All;

        //        case "Positive lines":
        //            return MarkupAllocateOn.Positive;

        //        case "Negative lines":
        //            return MarkupAllocateOn.Negative;

        //        default:
        //            return MarkupAllocateOn.All;
        //    }
        //}



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Close modal
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "close",
                "closeDialog();",
                true
            );
        }

    }
}