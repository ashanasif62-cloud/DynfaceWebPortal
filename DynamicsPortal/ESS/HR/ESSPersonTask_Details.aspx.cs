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
    public partial class ESSPersonTask_Details : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Detail";
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["recId"] != null)
                {
                    Int64 recId;
                    if (Int64.TryParse(Request.QueryString["recId"], out recId))
                    {
                        LoadTaskDetail(recId);
                    }
                }
            }
        }

        private void LoadTaskDetail(Int64 recId)
        {
            try
            {
                // Create service object
                ESSPersonTask taskService = new ESSPersonTask();

                // Call your method
                DataTable dt = taskService.retrieveDetail(recId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Bind fields safely
                    txtDescription.Text = row["description"]?.ToString();
                    txtProcessType.Text = row["displayProcessType"]?.ToString();
                    txtRegarding.Text = row["displayRegarding"]?.ToString();
                    txtContactPerson.Text = row["contactperson"]?.ToString();

                    txtStatus.Text = row["Status"]?.ToString();
                    //txtInstruction.Text = row["instruction"]?.ToString();

                    // Due Date (format properly)
                    if (row["dueDate"] != DBNull.Value)
                    {
                        DateTime dueDate = Convert.ToDateTime(row["dueDate"]);
                        txtDueDate.Text = dueDate.ToString("yyyy-MM-dd"); // required for date picker
                    }

                    // Optional checkbox
                    //if (row["isOptional"] != DBNull.Value)
                    //{
                    //    chkOptional.Checked = Convert.ToBoolean(row["isOptional"]);
                    //}

                    // Task link
                    TxtTaskLink.Text = row["displayActionLinkLabelValue"].ToString();
                    //if (row["displayActionLinkLabelValue"] != DBNull.Value)
                    //{
                    //    lnkTask.NavigateUrl = row["displayActionLinkLabelValue"].ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                // Log error
                SysErrorLog log = new SysErrorLog();
                log.write("ESSPersonTask_Details.LoadTaskDetail", ex);
            }
        }

        protected void BtnSave_Date_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Get RecId from QueryString
                if (Request.QueryString["recId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert", "alert('Record ID not found.');", true);
                    return;
                }

                Int64 recId;
                if (!Int64.TryParse(Request.QueryString["recId"], out recId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert", "alert('Invalid Record ID.');", true);
                    return;
                }

                // 2️⃣ Validate Due Date
                if (string.IsNullOrEmpty(txtDueDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert", "alert('Please select Due Date.');", true);
                    return;
                }

                DateTime dueDate;
                // Use TryParseExact to ensure proper yyyy-MM-dd format for date picker
                if (!DateTime.TryParseExact(txtDueDate.Text, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dueDate))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert", "alert('Invalid Date format.');", true);
                    return;
                }

                // 3️⃣ Create Contract
                ESSPersonTaskContract contract = new ESSPersonTaskContract
                {
                    recId = recId,
                    dueDate = dueDate
                };

                // 4️⃣ Call Update Method
                ESSPersonTask taskService = new ESSPersonTask();
                DataTable result = taskService.update(contract);

                // 5️⃣ Success Message
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "success", "alert('Due Date updated successfully.');", true);
            }
            catch (Exception ex)
            {
                // 6️⃣ Error Message
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "error", $"alert('Error updating record: {ex.Message.Replace("'", "")}');", true);
            }
        }
    }
}