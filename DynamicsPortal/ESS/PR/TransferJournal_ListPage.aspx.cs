using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TransferJournalHeaderSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferJournal_ListPage : MainForm
    {
        private TransferJournalHeader transferJournalHeader= new TransferJournalHeader();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = transferJournalHeader.tableName;
                pageMenuId = "TransferJournal_ListPage";
                if (!IsPostBack)
                {
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Transfer";
                        titleDiv.Style["font-weight"] = "600";   
                        titleDiv.Style["font-size"] = "16px";    
                        titleDiv.Style["color"] = "#000000";    
                        titleDiv.Style["margin"] = "10px 0";     
                    }
                    reBindGrid();
                }

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "JournalClick")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridView.Rows[rowIndex];

                string JournalId = ((LinkButton)row.FindControl("lblJournal")).Text;

                string JournalDescription = ((Label)row.FindControl("lblDescription")).Text;
                string VoucherSeries = ((Label)row.FindControl("lblVoucherSeries")).Text;
                string SelectionBy = ((Label)row.FindControl("lblSelectionBy")).Text;
                string NewVoucherBy = ((Label)row.FindControl("lblNewVoucherBy")).Text;
                string Detaillevel = ((Label)row.FindControl("lblDetaillevel")).Text;
                string DeletePostedLines = ((Label)row.FindControl("lblDeletePostedLines")).Text;
                //string DeletePostedLines = ((Label)row.FindControl("lblDeletePostedLines")).Text;
                string InventSiteId = ((Label)row.FindControl("lblInventSiteId")).Text;
                string InventLocationId = ((Label)row.FindControl("lblInventLocationId")).Text;
                Label lblRecId = row.FindControl("lblRecId") as Label;
                if (lblRecId != null)
                {
                    // Parse the text to long (Int64)
                    long recId = 0;
                    if (long.TryParse(lblRecId.Text, out recId))
                    {
                        // Store in session
                        Session["RecId"] = recId;
                    }
                }

                // Redirect
                Session["JournalID"] = JournalId;
                Session["JournalDescription"] = JournalDescription;
                Session["VoucherSeries"] = VoucherSeries;
                Session["SelectionBy"] = SelectionBy;
                Session["NewVoucherBy"] = NewVoucherBy;
                Session["Detaillevel"] = Detaillevel;
                Session["DeletePostedLines"] = DeletePostedLines;
                Session["InventSiteId"] = InventSiteId;
                Session["InventLocationId"] = InventLocationId;
                Response.Redirect("~/ESS/PR/TransferJournalLines_ListPage.aspx");
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = transferJournalHeader.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    bindGrid();
                }
            }

        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        private void getGridDataTable()
        {
            DataTable dt = transferJournalHeader.retrieveAll("USMF");
            SessionVariables.setSessionDataTable(dt);
        }
        protected void bindGrid() 
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt; 
            gridView.DataBind();
        }
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool isAnyRowSelected = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    isAnyRowSelected = true;
                    string status = ((Label)gridViewRow.FindControl("lblPosted")).Text.Trim();

                    // ✅ Disable button if status = "Yes"
                    if (status.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                    {
                        btnDelete.Enabled = false;
                        return; // exit early, since delete must stay disabled
                    }
                }
            }

            // ✅ If no "Yes" rows are selected, allow delete
            if (isAnyRowSelected)
            {
                btnDelete.Enabled = true;
            }
            else
            {
                // optional: disable delete if nothing selected
                btnDelete.Enabled = false;
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("RecId", typeof(long));

            // Loop through GridView rows and add RecIds to the DataTable
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    long recId;

                    if (long.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId))
                    {
                        if (recId > 0)
                        {
                            dt.Rows.Add(recId);
                        }
                    }
                }
            }

            // If nothing selected, stop
            if (dt.Rows.Count == 0)
                return;

            // Pass DataTable to your service call
            SysOperationResult_BOL operationResult_BOL = transferJournalHeader.postTransferJournal(dt);

            // Check result and refresh UI
            bool result = operationResults(operationResult_BOL);
            if (result)
            {
                gridView.EditIndex = -1;
                reBindGrid();
            }
            else
            {
                bindGrid();
            }
        }
    }
}