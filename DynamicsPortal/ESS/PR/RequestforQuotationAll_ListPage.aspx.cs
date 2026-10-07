using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class RequestforQuotationAll_ListPage : MainForm
    {
        private RequestforQuotationAll requestforall = new RequestforQuotationAll();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = requestforall.tableName;
                pageMenuId = "RequestforQuotationAll_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "All Request for Quotation";

                    // Dynamically set the page title in the master page div
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "All Request for Quotation"; // Set the text in the div
                        titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                    }

                    
                }

                reBindGrid();
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }

        }

        private void getGridDataTable()
        {
            DataTable dt = requestforall.retrieveAll();
            SessionVariables.setSessionDataTable(dt);
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Checkbox (optional if you want to do something with it)
                CheckBox chkSelectSingle = (CheckBox)e.Row.FindControl("chk_SelectSingle");

                // RFQ Case ID
                Label lblRFQCaseId = (Label)e.Row.FindControl("lblRFQCaseId");

                // RFQ Type
                Label lblRFQType = (Label)e.Row.FindControl("lblRFQType");

                // Document Title
                Label lblDocumentTitle = (Label)e.Row.FindControl("lblDocumentTitle");

                // Solicitation Type
                Label lblSolicitationType = (Label)e.Row.FindControl("lblSolicitationType");

                // Bid Type
                Label lblBidType = (Label)e.Row.FindControl("lblBidType");

                // Requester Name
                Label lblRequesterName = (Label)e.Row.FindControl("lblRequesterName");

                // Status Low
                Label lblStatusLow = (Label)e.Row.FindControl("lblStatusLow");

                // Status High
                Label lblStatusHigh = (Label)e.Row.FindControl("lblStatusHigh");

                // Vendor Status
                Label lblVendorStatus = (Label)e.Row.FindControl("lblVendorStatus");

                // Expiry Date Time (Formatted)
                Label lblExpiryDateTime = (Label)e.Row.FindControl("lblExpiryDateTime");
                if (lblExpiryDateTime != null && DateTime.TryParse(lblExpiryDateTime.Text, out DateTime expiryDate))
                {
                    lblExpiryDateTime.Text = expiryDate.ToString("dd-MMM-yyyy hh:mm tt"); // Example format
                }

                //// RecId (hidden field, optional)
                //Label lblRecId = (Label)e.Row.FindControl("Label1");

               
            }
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
    }
}