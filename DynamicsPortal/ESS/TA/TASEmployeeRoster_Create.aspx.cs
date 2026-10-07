using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal
{
    public partial class TASEmployeeRoster_Create : ModalForm
    {
        private string employeeId;
        private TASEmployeeRoster tASEmployeeRoster = new TASEmployeeRoster();

        protected override void Page_Load(object sender, EventArgs e)
        {

            tableId = tASEmployeeRoster.tableName;
            pageMenuId = "TASEmployeeRoster_Create";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Create Roster";
              //  titleDiv.Style["font-weight"] = "bold";
            }
            // Optional: You could load data only once on first load
            if (!IsPostBack)
            {
                bindData();
            }
        }

        // 🔹 This method is correctly placed outside btnOk_Click now
        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
           // employeeId = (DropDownList_EmployeeDetails1.FindControl("txtEmployeeId") as TextBox)?.Text;
            employeeId = SessionVariables.getCurrentEmployeeId();   
        }
        //private void bindData()
        //{
        //    TASEmployeeRoster TASEmployeeRoster = new TASEmployeeRoster();
        //    DataTable dt = TASEmployeeRoster.retrieveShiftIds();

        //    ddlShiftId.DataSource = dt;
        //    ddlShiftId.DataValueField = "ShiftId";   // value to pass
        //    ddlShiftId.DataTextField = "ShiftId";    // text to show
        //    ddlShiftId.DataBind();
        //}

        private void bindData()
        {
            TASEmployeeRoster TASEmployeeRoster = new TASEmployeeRoster();
            DataTable dt = TASEmployeeRoster.retrieveShiftIds();

            ddlShiftId.Items.Clear();
            ddlShiftId.Items.Add(new ListItem("-- Select Shift --", ""));

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string shiftId = row["ShiftId"]?.ToString() ?? "";
                    string shiftCode = row["code"]?.ToString() ?? "";
                    string description = row["description"]?.ToString() ?? "";

                    string displayText = $"{shiftId} - {shiftCode} - {description}";

                    ddlShiftId.Items.Add(new ListItem(displayText, shiftId));
                }
            }
        }


        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // 🔹 Always re-fetch employeeId in case event didn't fire
                employeeId = SessionVariables.getCurrentEmployeeId();
               // employeeId = (DropDownList_EmployeeDetails1.FindControl("txtEmployeeId") as TextBox)?.Text;


                SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

                // Prepare data
                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeId");
                dt.Columns.Add("ShiftId");
                dt.Columns.Add("fromDate");
                dt.Columns.Add("toDate");
                dt.Columns.Add("GENERATIONTYPE");


                DataRow row = dt.NewRow();
                row["EmployeeId"] = employeeId;
              row["ShiftId"] = ddlShiftId.SelectedValue;//txtShiftId.Text.Trim();
                row["fromDate"] = txtFromDate.Text.Trim();
                row["toDate"] = txtToDate.Text.Trim();
                row["GENERATIONTYPE"] = "Dynaface";

                dt.Rows.Add(row);
                // Call the business logic/service class
                TASEmployeeRoster rosterService = new TASEmployeeRoster();
                SysOperationResult_BOL result = rosterService.create(dt);

                // 🔹 Fix property name from isSuccess → Success
                //    if (result != null && result.isSuccess)
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "CloseDialog", "closeDialog(true);", true);
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error"}');", true);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowException", $"alert('Exception: {ex.Message}');", true);
                //}
                if (result != null && result.isSuccess)
                {
                    // ✅ Show success alert and close dialog
                   ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", $"alert('{result.Message}'); closeDialog(true);", true);

                    string script = @"setTimeout(function() { 
                    if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }
                    closeDialog(); 
                }, 3000);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                }
                else
                {
                    // ❌ Show error alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
                }
            }
            catch (Exception ex)
            {
                // 🔥 Show exception message
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowException", $"alert('Exception: {ex.Message}');", true);
            }
        }
    }
}
