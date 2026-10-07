using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequestGroup_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PRPurchasedRequisition";
            if (!IsPostBack)
            {
                getGridDataTable();
                bindGrid();
            }
        }
        
        private void getGridDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("VenderAcount");
            dt.Columns.Add("InvoiceAccount");
            dt.Columns.Add("VenderName");
            dt.Columns.Add("PurchaseType");
            dt.Columns.Add("ApprovalStatus");
            dt.Columns.Add("PurchaseOrderStatus");
            dt.Columns.Add("Currency");
            dt.Columns.Add("RequestReceiptDate");
            dt.Columns.Add("ModeOfDelivery");
            dt.Columns.Add("DeliveryTerms");
            dt.Columns.Add("PurchaseAgreement");
            dt.Columns.Add("DirectDelivery");
            dt.Columns.Add("ProjectSubContractNumber");
            dt.Columns.Add("RecId");

            // Add test row
         

            Session["MyDataTable"] = dt;
        }

        protected void bindGrid()
        {
            DataTable dt = Session["MyDataTable"] as DataTable;

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
            {
                // Just to ensure grid doesn't break if session is missing
                gridView.DataSource = new DataTable();
                gridView.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    long recId;
                    long.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                        recordsId.Add(recId);
                }
            }

            // Do your delete logic here
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Optional customization
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Optional command logic
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            bindGrid();
        }
    }
}
