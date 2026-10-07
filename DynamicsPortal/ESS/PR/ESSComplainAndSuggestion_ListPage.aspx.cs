using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessObject;
using System.Xml.Schema;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.PR
{
    public partial class ESSComplainAndSuggestion_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSComplainAndSuggestion_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Complain And Suggestions";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Complain And Suggestions";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    BindGrid();

                    
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }




      
             private void BindGrid()
                 { 
                    try
                        {
                        string employeeId = SessionVariables.getCurrentEmployeeId();

                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    gridView.DataSource = null;
                    gridView.DataBind();
                    return;
                }

                ESSComplainAndSuggestionsSvc service = new ESSComplainAndSuggestionsSvc();

                DataTable dataTable = service.retrieveAll(employeeId);

                gridView.DataSource = dataTable;
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                gridView.DataSource = null;
                gridView.DataBind();

                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }


        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                upGrid.Update();          // important for UpdatePanel
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> selectedRecIds = new List<long>();

                // Collect selected RecIds from the grid
                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chk = row.FindControl("chkSelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        // Get RecId from DataKeys
                        long recId = Convert.ToInt64(gridView.DataKeys[row.RowIndex].Value);
                        selectedRecIds.Add(recId);
                    }
                }

                if (selectedRecIds.Count == 0)
                {
                    SysOperationResult_BOL result1 = new SysOperationResult_BOL();

                    result1.isSuccess = false;
                    result1.Message = "Please select at least one record to delete.";

                    NotificationMessage.showMessage(result1);
                    return;
                }

                // Call the delete service
                ESSComplainAndSuggestionsSvc service = new ESSComplainAndSuggestionsSvc();
                SysOperationResult_BOL result = service.delete(selectedRecIds.ToArray());

                if (result != null && result.isSuccess)
                {
                    NotificationMessage.showMessage(result);

                    // Refresh the grid
                    BindGrid();
                    upGrid.Update();
                }
                else
                {
                    SysOperationResult_BOL errorResult =
                new SysOperationResult_BOL();

                    errorResult.isSuccess = false;
                    errorResult.Message =
                        (result != null &&
                         !string.IsNullOrEmpty(result.Message))
                            ? result.Message
                            : "Failed to delete the record(s).";

                    NotificationMessage.showMessage(errorResult);
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(GetType().FullName + ".btnDelete_Click", ex);
                SysOperationResult_BOL errorResult =
            new SysOperationResult_BOL();

                errorResult.isSuccess = false;
                errorResult.Message =
                    "An error occurred while deleting the record(s).";

                NotificationMessage.showMessage(errorResult);
            }
        }


    }
}