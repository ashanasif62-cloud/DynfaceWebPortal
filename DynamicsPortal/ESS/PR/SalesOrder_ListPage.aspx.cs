using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class SalesOrder_ListPage : MainForm
    {
        // ── Pagination state ─────────────────────────────────────────────────
        private const int PageSize = 20;

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalRecords
        {
            get { return ViewState["TotalRecords"] != null ? (int)ViewState["TotalRecords"] : 0; }
            set { ViewState["TotalRecords"] = value; }
        }

        // ── Page Load ─────────────────────────────────────────────────────────
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "SalesOrderListPage";

            var titleDiv = Master.FindControl("pageTitle")
                           as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
                titleDiv.InnerText = "All sales orders";

            if (!IsPostBack)
            {
                CurrentPage = 1;
                LoadSalesOrders();
            }
        }

        // ── Data loading ──────────────────────────────────────────────────────
        private void LoadSalesOrders()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveAll();

            TotalRecords = dt.Rows.Count;

            // Client-side paging
            DataTable paged = GetPagedData(dt, CurrentPage, PageSize);

            gvSalesOrders.DataSource = paged;
            gvSalesOrders.DataBind();

            UpdatePaginationControls();
        }

        private DataTable GetPagedData(DataTable source, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            DataTable paged = source.Clone();
            foreach (DataRow row in source.AsEnumerable().Skip(skip).Take(pageSize))
                paged.ImportRow(row);
            return paged;
        }

        private void UpdatePaginationControls()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            if (totalPages < 1) totalPages = 1;

            lblPageInfo.Text = $"Page {CurrentPage} of {totalPages}";
            btnPrev.Visible  = CurrentPage > 1;
            btnNext.Visible  = CurrentPage < totalPages;
        }

        // ── Row data bound ────────────────────────────────────────────────────
        protected void gvSalesOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Highlight a freshly created/updated row (stored in Session by create page)
                if (Session["HighlightSalesId"] != null)
                {
                    var lnk = e.Row.FindControl("lnkSalesOrderId") as LinkButton;
                    if (lnk != null && lnk.Text == Session["HighlightSalesId"].ToString())
                    {
                        e.Row.CssClass += " highlight-row";
                        Session.Remove("HighlightSalesId");
                    }
                }
            }
        }

        // ── Checkbox selection ────────────────────────────────────────────────
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk    = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;

            if (chk.Checked)
                row.CssClass = "selected-row";
            else
                row.CssClass = row.RowIndex % 2 == 0 ? "" : "alt-row";
        }

        // ── Get selected row ──────────────────────────────────────────────────
        private GridViewRow GetSelectedRow()
        {
            foreach (GridViewRow row in gvSalesOrders.Rows)
            {
                var chk = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null && chk.Checked)
                    return row;
            }
            return null;
        }

        private string GetSelectedSalesId()
        {
            var row = GetSelectedRow();
            if (row == null) return null;
            var lnk = row.FindControl("lnkSalesOrderId") as LinkButton;
            return lnk?.Text;
        }

        // ── Sales Order link click ─────────────────────────────────────────────
        //protected void lnkSalesOrderId_Click(object sender, EventArgs e)
        //{
        //    LinkButton lnk = (LinkButton)sender;
        //    string salesId = lnk.CommandArgument;
        //    Response.Redirect($"~/ESS/PR/SalesOrder_Detail.aspx?SalesId={salesId}");
        //}

        protected void lnkSalesOrderId_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string salesId = lnk.CommandArgument;

            // ✅ Set in Session too, as a fallback
            Session["SalesId"] = salesId;

            Response.Redirect($"~/ESS/PR/SalesOrder_Detail.aspx?SalesId={salesId}");
        }

        // ── Action Panel buttons ──────────────────────────────────────────────

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId))
            {
                ShowMessage("Please select a sales order to delete.");
                return;
            }
            // TODO: call delete service
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId))
            {
                ShowMessage("Please select a sales order to submit.");
                return;
            }
            // TODO: submit to workflow
        }

        protected void btnRequestChange_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: open request-change page
        }

        protected void btnTotal_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: show totals
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: confirm order
        }

        protected void btnMaintainCharges_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: maintain charges
        }

        protected void btnSalesTax_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: sales tax
        }

        protected void btnPickingList_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: generate picking list
        }

        protected void btnPackingSlip_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: generate packing slip
        }

        protected void btnPickingListJournal_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: picking list journal
        }

        protected void btnPackingSlipJournal_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: packing slip journal
        }

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: generate invoice
        }

        protected void btnInvoiceJournal_Click(object sender, EventArgs e)
        {
            string salesId = GetSelectedSalesId();
            if (string.IsNullOrEmpty(salesId)) return;
            // TODO: invoice journal
        }

        protected void btnAttachment_Click(object sender, EventArgs e)
        {
            LinkButton lnk  = (LinkButton)sender;
            GridViewRow row  = (GridViewRow)lnk.NamingContainer;
            string salesId   = ((LinkButton)row.FindControl("lnkSalesOrderId"))?.Text;
            if (!string.IsNullOrEmpty(salesId))
                ScriptManager.RegisterStartupScript(this, GetType(), "attach",
                    $"openPopupPanel('/ESS/PR/Attachment.aspx?SalesId={salesId}', 700);", true);
        }

        // ── Pagination ────────────────────────────────────────────────────────
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadSalesOrders();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            if (CurrentPage < totalPages)
            {
                CurrentPage++;
                LoadSalesOrders();
            }
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private void ShowMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                $"alert('{msg}');", true);
        }
    }
}