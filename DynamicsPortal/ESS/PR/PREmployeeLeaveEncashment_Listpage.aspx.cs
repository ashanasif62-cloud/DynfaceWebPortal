using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeaveEncashmentSvcReferences;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PREmployeeLeaveEncashment_Listpage : MainForm
    {
        private PREmployeeLeaveEncashment encashment = new PREmployeeLeaveEncashment();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
                pageMenuId = "PREmployeeLeaveEncashment_Listpage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        // Microsoft Dynamics–like styling
                        titleDiv.Style["font-family"] = "'Segoe UI', SegoeUI, Arial, sans-serif";
                       
                    }

                    reBindGrid();

                }
                
                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();
                }

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }


        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRequestedDate = e.Row.FindControl("lblRequestedDate") as Label;

                if (lblRequestedDate != null && !string.IsNullOrWhiteSpace(lblRequestedDate.Text))
                {
                    DateTime requestDate;

                    if (DateTime.TryParse(lblRequestedDate.Text, out requestDate))
                    {
                        lblRequestedDate.Text = requestDate.ToString("dd/M/yyyy");
                    }
                }
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

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = encashment.retrieveAll(employeeId);
            SessionVariables.setSessionDataTable(dt);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
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
                SysOperationResult_BOL operationResult_BOL = encashment.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    NotificationMessage.showMessage(operationResult_BOL.Message);

                    gridView.EditIndex = -1;
                    reBindGrid();

                    // Refresh page after 2 seconds
                    string script = "setTimeout(function(){ window.location = window.location.href; }, 2000);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshPage", script, true);
                }
                else
                {
                    NotificationMessage.showMessage(operationResult_BOL.Message);
                    bindGrid();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lblRequestId = gridViewRow.FindControl("lblRequestId") as Label;
                    if (lblRequestId != null && !string.IsNullOrEmpty(lblRequestId.Text))
                    {
                        recordsId.Add(lblRequestId.Text);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.SubmitEmployeeLeaveEncashmentReq(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    reBindGrid();
                }
            }

        }


    }
}