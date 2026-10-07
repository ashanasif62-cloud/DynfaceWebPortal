using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSPersonTaskSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSPersonTask_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "PersonTask";
            }
            if (!IsPostBack)
            {

                BindGrid();


            }



          



        }
        private void BindGrid()
        {
            try
            {
                // ✅ Get Employee ID from Session
                string employeeId = SessionVariables.getCurrentEmployeeId();    

                // Safety check
                if (string.IsNullOrEmpty(employeeId))
                {
                    gvTasks.DataSource = null;
                    gvTasks.DataBind();
                    return;
                }

                // ✅ Call your integration class
                ESSPersonTask taskService = new ESSPersonTask();
                DataTable dt = taskService.retrieveAll(employeeId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = taskService.createDataTable(); // already in your class
                }
                // ✅ Bind Grid
                gvTasks.DataSource = dt;
                gvTasks.DataBind();
            }
            catch (Exception ex)
            {
                BindEmptyGrid();
            }
        }

        private void BindEmptyGrid()
        {
            DataTable dt = new DataTable();

            // ✅ MUST match GridView fields exactly
            dt.Columns.Add("Task");
            dt.Columns.Add("Description");
            dt.Columns.Add("Regarding");
            dt.Columns.Add("DueDate");
            dt.Columns.Add("Status");
            dt.Columns.Add("Optional", typeof(bool));
            dt.Columns.Add("TaskLink");
            dt.Columns.Add("ProcessType");

            gvTasks.DataSource = dt;
            gvTasks.DataBind();
        }
        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewTask")
            {
                string recId = e.CommandArgument.ToString();

                // Redirect to detail page with RecId
                Response.Redirect("~/ESS/HR/ESSPersonTask_Details.aspx?recId=" + recId);
            }
        }

      
        protected void btnViewDetail_Click(object sender, EventArgs e)
        {
            // Redirect to the detail form
            Response.Redirect("~/ESS/HR/ESSPersonTask_Details.aspx");
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            if (!string.IsNullOrEmpty(selectedStatus))
            {
                // Your logic to update the task status
            }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Get selected row from GridView
                GridViewRow selectedRow = null;
                foreach (GridViewRow row in gvTasks.Rows)
                {
                    RadioButton rbtn = row.FindControl("rbtnSelect") as RadioButton;
                    if (rbtn != null && rbtn.Checked)
                    {
                        selectedRow = row;
                        break;
                    }
                }

                if (selectedRow == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert", "alert('Please select a task to update.');", true);
                    return;
                }

                // 2️⃣ Get RecId from hidden field
                HiddenField hfRecId = selectedRow.FindControl("hfRecId") as HiddenField;
                if (hfRecId == null || string.IsNullOrEmpty(hfRecId.Value))
                    return;

                long recId = Convert.ToInt64(hfRecId.Value);

                // 3️⃣ Get selected status from dropdown
                string newStatus = ddlStatus.SelectedValue;
                if (string.IsNullOrEmpty(newStatus))
                    return;

                // 4️⃣ Create contract
                ESSPersonTaskContract contract = new ESSPersonTaskContract
                {
                    recId = recId,
                    Status = newStatus
                };

                // 5️⃣ Call service to update status
                ESSPersonTask svc = new ESSPersonTask();
                DataTable result = svc.updateStatus(contract);

                // 6️⃣ Reload grid
                BindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "error", $"alert('Error updating status: {ex.Message.Replace("'", "")}');", true);
            }
        }


        //protected void btnChangeStatus_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 1️⃣ Get selected row
        //        GridViewRow selectedRow = null;
        //        foreach (GridViewRow row in gvTasks.Rows)
        //        {
        //            RadioButton rbtn = row.FindControl("rbtnSelect") as RadioButton;
        //            if (rbtn != null && rbtn.Checked)
        //            {
        //                selectedRow = row;
        //                break;
        //            }
        //        }

        //        if (selectedRow == null)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert", "alert('Please select a task to update.');", true);
        //            return;
        //        }

        //        // 2️⃣ Get RecId from hidden field
        //        HiddenField hfRecId = selectedRow.FindControl("hfRecId") as HiddenField;
        //        if (hfRecId == null || string.IsNullOrEmpty(hfRecId.Value))
        //            return;

        //        long recId = Convert.ToInt64(hfRecId.Value);

        //        // 3️⃣ Get selected status
        //        string newStatus = ddlStatus.SelectedValue;
        //        if (string.IsNullOrEmpty(newStatus))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert", "alert('Please select a status before updating.');", true);
        //            return;
        //        }

        //        // 4️⃣ Create contract
        //        ESSPersonTaskContract contract = new ESSPersonTaskContract
        //        {
        //            recId = recId,
        //            Status = newStatus
        //        };

        //        // 5️⃣ Call service
        //        ESSPersonTask svc = new ESSPersonTask();
        //        DataTable result = svc.updateStatus(contract);

        //        // 6️⃣ Show success
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "success", $"alert('Status updated successfully to {newStatus}.');", true);

        //        // 7️⃣ Reload grid
        //        BindGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "error", $"alert('Error updating status: {ex.Message.Replace("'", "")}');", true);
        //    }
        //}

     

    }
}