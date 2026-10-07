using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PurchaseOrderHeaderSvcrRefernce;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class PurchaseOrder_Total : ModalForm
    {
        PurchUpdate specQty;
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    pageMenuId = "PurchaseOrder_Total";

                    // Set page title
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Totals";
                        titleDiv.Style["font-weight"] = "600";
                        titleDiv.Style["font-size"] = "20px";
                        titleDiv.Style["color"] = "#000000";
                        titleDiv.Style["margin"] = "10px 0";
                    }
                    BindSelectionDropdown();

                    // Bind dropdown and load totals
                    string purchId = Session["purchIdTotal"]?.ToString();
                    if (!string.IsNullOrEmpty(purchId))
                    {
                        LoadPurchaseTotals(purchId, ddlSelection.SelectedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
                   
        }

        private void BindSelectionDropdown()
        {
            try
            {
                ddlSelection.Items.Clear();

                // Add items with string values instead of integers
                ddlSelection.Items.Add(new ListItem("Receive now quantity", "Receive Now quantity"));
                ddlSelection.Items.Add(new ListItem("Ordered quantity", "Ordered quantity"));
                ddlSelection.Items.Add(new ListItem("Registered quantity", "Registered quantity"));
                ddlSelection.Items.Add(new ListItem("Product receipt quantity", "Product receipt quantity"));
                ddlSelection.Items.Add(new ListItem("Registered quantity and services", "Registered quantity and services"));

                // Set default selection (optional)
                ddlSelection.SelectedValue = "Ordered quantity"; // Receive now quantity
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }


        protected void ddlSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedValue = ddlSelection.SelectedValue; // integer as string
                string purchId = Session["purchIdTotal"]?.ToString();

                if (!string.IsNullOrEmpty(purchId))
                {
                    LoadPurchaseTotals(purchId, selectedValue);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        private void LoadPurchaseTotals(string purchId, string selection)
        {
            try
            {
                PurchaseOrderHeader header = new PurchaseOrderHeader();

                // Call service
                DataTable result = header.retrievePOTotal(purchId, selection);

                if (result != null && result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];

                    // Left side (Totals)
                    Label1.Text = row["Currency"]?.ToString() ?? "";
                    lblExchangeRate.Text = FormatDecimals(row["ExchangeRate"]);
                    lblLineDiscount.Text = FormatDecimal(row["LineDiscount"]);
                    lblSubtotalAmount.Text = FormatDecimal(row["SubTotalAmount"]);
                    lblTotalDiscount.Text = FormatDecimal(row["TotalDiscount"]);
                    lblCharges.Text = FormatDecimal(row["Charges"]);
                    lblSalesTax.Text = FormatDecimal(row["SalesTax"]);
                    lblRoundOff.Text = FormatDecimal(row["RoundOff"]);
                    lblTotalAmount.Text = FormatDecimal(row["TotalAmount"]);
                    lblCashDiscount.Text = FormatDecimal(row["CashDiscount"]);

                    // Right side (Vendor & Prepayment)
                    lblCreditLimit.Text = FormatDecimal(row["CreditLimit"]);
                    lblCreditAvailable.Text = FormatDecimal(row["CreditAvailable"]);
                    lblLimit.Text = FormatDecimal(row["Limit"]);
                    lblRemaining.Text = FormatDecimal(row["Remaining"]);

                    // Measurements
                    lblQuantity.Text = FormatDecimal(row["Quantity"]);
                    lblWeight.Text = FormatDecimal(row["Weight"]);
                    lblCWQuantity.Text = FormatDecimal(row["CWQuantity"]);
                    lblVolume.Text = FormatDecimal(row["Volume"]);
                }
                else
                {
                    ClearLabels();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
                ClearLabels();
            }
        }

        private void ClearLabels()
        {
            Label1.Text = "";
            lblExchangeRate.Text = "";
            lblLineDiscount.Text = "";
            lblSubtotalAmount.Text = "";
            lblTotalDiscount.Text = "";
            lblCharges.Text = "";
            lblSalesTax.Text = "";
            lblRoundOff.Text = "";
            lblTotalAmount.Text = "";
            lblCashDiscount.Text = "";
            lblCreditLimit.Text = "";
            lblCreditAvailable.Text = "";
            lblLimit.Text = "";
            lblRemaining.Text = "";
            lblQuantity.Text = "";
            lblWeight.Text = "";
            lblCWQuantity.Text = "";
            lblVolume.Text = "";
        }

        public string MapDropdownToValue(string selection)
        {
            switch (selection.Trim())
            {
                case "Receive now quantity":
                    return "Receive now quantity";
                case "Ordered quantity":
                    return "Ordered quantity";
                case "Registered quantity":
                    return "Registered quantity";
                case "Product receipt quantity":
                    return "Product receipt quantity";
                case "Registered quantity and services":
                    return "Registered quantity and services";
                default:
                    throw new ArgumentException($"Invalid selection: {selection}");
            }
        }

        private string FormatDecimals(object value)
        {
            if (value == null || value == DBNull.Value)
                return "0.0000";

            if (decimal.TryParse(value.ToString(), out decimal number))
                return number.ToString("0.0000"); // always 4 decimals

            return "0.0000"; // fallback
        }

        private string FormatDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return "0.00";

            if (decimal.TryParse(value.ToString(), out decimal number))
                return number.ToString("#,##0.00"); // thousand separators + 2 decimals

            return "0.00";
        }




    }
}