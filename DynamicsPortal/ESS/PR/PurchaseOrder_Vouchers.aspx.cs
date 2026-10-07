using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_Vouchers : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Purchase Order Vouchers";

                // Set title in master page
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Vouchers";
                    titleDiv.Style["font-weight"] = "bold";
                }

                // Bind empty grid on first load
                BindEmptyGrid();

                // Set DocumentDate default
               
            }
        }


        private void BindEmptyGrid()
        {
            DataTable dt = BuildVoucherDataTable();
            gvVouchers.DataSource = dt;
            gvVouchers.DataBind();
        }

        private DataTable BuildVoucherDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("JournalNumber");
            dt.Columns.Add("Voucher");
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("YearClosed");

            dt.Columns.Add("LedgerAccount");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("Description");

            dt.Columns.Add("Currency");
            dt.Columns.Add("AmountTransCurrency");
            dt.Columns.Add("Amount");

            dt.Columns.Add("PostingType");
            dt.Columns.Add("PostingLayer");

            dt.Columns.Add("VendorAccount");
            dt.Columns.Add("VendorName");

            dt.Columns.Add("CustomerAccount");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("CustomerGroups");

            dt.Columns.Add("PaymentReference");
            dt.Columns.Add("Branches");

            return dt;
        }
    }
}