using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequisitionLineCreate : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "PurchaseRequisitionLineCreate";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {

                    string requisitioner = Session["Originator"] as string;

                    // ✅ Step 2: Populate the text box if the value is present
                    if (!string.IsNullOrWhiteSpace(requisitioner))
                    {
                        txtRequisitioner.Text = requisitioner;
                        txtdataAreaId.Text = SessionVariables.getUserCurrentDataAreaId();
                    }
                    bindControlsData();
                    //BindItemid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        //private void BindItemid()
        //{
        //    PurchaseRequisitionLine service = new PurchaseRequisitionLine();
        //    DataTable dt = service.retrieveAllItemID();

        //    ddlItemId.DataSource = dt;
        //    ddlItemId.DataValueField = "ItemId";
        //    ddlItemId.DataTextField = "ItemId";
        //    ddlItemId.DataBind();
        //    ddlItemId.Items.Insert(0, new ListItem("", ""));

        //    dt = service.retrievedepartment();
        //    ddlDepartment.DataSource = dt;
        //    ddlDepartment.DataTextField = "DepartmentName";
        //    ddlDepartment.DataValueField = "DepartmentRecId";
        //    ddlDepartment.DataBind(); 
            
        //    dt = service.retrieveunitofmeasure();
        //    ddlUnitofMeasure.DataSource = dt;
        //    ddlUnitofMeasure.DataTextField = "Unitofmeasuresymbol";
        //    ddlUnitofMeasure.DataValueField = "PurchUnitOfMeasure";
        //    ddlUnitofMeasure.DataBind();


        //    dt = service.retrieveVendor();
        //    ddlvendoraccnum.DataSource = dt;
        //    ddlvendoraccnum.DataTextField = "VendAccount";
        //    ddlvendoraccnum.DataValueField = "VendAccount";
        //    ddlvendoraccnum.DataBind();
        //    ddlvendoraccnum.Items.Insert(0, new ListItem("", ""));

        //}

        protected void ddlvendoraccnum_Changed(object sender, EventArgs e)
        {
            PurchaseRequisitionLine service = new PurchaseRequisitionLine();
            string selectedvendor = ddlvendoraccnum.SelectedValue;
            DataTable dt = service.retrieveVendor(selectedvendor);
            string text = dt.Rows[0]["VendorName"].ToString();
            txtvendorname.Text = dt.Rows[0]["VendorName"].ToString();
        }

        private void bindControlsData()
        {
            // No controls to bind in current version
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Optional: custom logic on cancel
        }

        private void createRequest()
        {
            create_SubmitRequest(false);
        }
        private void create_SubmitRequest(bool _submitRequest = true)
        {
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();

            try
            {
                // Step 1: Validate input
                string requisitioner = txtRequisitioner.Text.Trim();
                //string itemId = txtItemIdSmall.Text.Trim();
                string qtyText = txtPurchQty.Text.Trim();
                string dataAreaId = txtdataAreaId.Text.Trim(); // ✅ New

                if (string.IsNullOrWhiteSpace(requisitioner))
                {
                    ShowMessage("Requester is required.");
                    return;
                }

                //if (string.IsNullOrWhiteSpace(itemId))
                //{
                //    ShowMessage("Item Number is required.");
                //    return;
                //}

                if (!decimal.TryParse(qtyText, out decimal purchQty))
                {
                    ShowMessage("Quantity must be a valid number.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(dataAreaId))
                {
                    ShowMessage("DataArea Id is required.");
                    return;
                }

                // Step 2: Prepare the DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("Requisitioner", typeof(string));
                dt.Columns.Add("PurchReqId", typeof(string));
                dt.Columns.Add("ItemId");
                dt.Columns.Add("PurchQty", typeof(decimal));
                dt.Columns.Add("DataAreaId", typeof(string)); // ✅ New
                dt.Columns.Add("DepartmentName", typeof(string));
                dt.Columns.Add("VendAccount", typeof(string));
                dt.Columns.Add("VendorName", typeof(string));
                dt.Columns.Add("OperatingUnitNumber", typeof(string));
                dt.Columns.Add("DepartmentRecId", typeof(string));
                dt.Columns.Add("Unitofmeasuresymbol", typeof(string));
                dt.Columns.Add("PurchUnitOfMeasure", typeof(decimal));

                DataRow dr = dt.NewRow();
                dr["Requisitioner"] = SessionVariables.getCurrentEmployeeId();
                dr["ItemId"] = ddlItemId.SelectedValue;
                dr["PurchQty"] = purchQty;
                dr["DataAreaId"] = dataAreaId; // ✅ New
                dr["PurchReqId"] = Session["PurchReqId"];
                dr["DepartmentName"] = ddlDepartment.Text;
                dr["VendAccount"] = ddlvendoraccnum.SelectedValue;
                dr["VendorName"] = ddlvendoraccnum.Text;
                dr["DepartmentRecId"] = ddlDepartment.SelectedValue;
                dr["Unitofmeasuresymbol"] = ddlUnitofMeasure.Text;
                dr["PurchUnitOfMeasure"] = ddlUnitofMeasure.SelectedValue;

                dt.Rows.Add(dr);

                // Step 3: Call Create method
                PurchaseRequisitionLine service = new PurchaseRequisitionLine();
                createResult = service.create(dt);

                // Step 4: Show result to user
                bool result = operationResults(createResult, true);

                if (createResult.isSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "close", "closeDialog(true);", true);
                }
            }
            catch (Exception ex)
            {
                createResult.isSuccess = false;
                createResult.Message = "An error occurred: " + ex.Message;
                createResult.AlertType = AlertType.Error.ToString();
                operationResults(createResult, false);
            }
        }




        //private void create_SubmitRequest(bool _submitRequest = true)
        //{
        //    bool submitRequest = _submitRequest;
        //    SysOperationResult_BOL createResult = new SysOperationResult_BOL();

        //    try
        //    {
        //        // Step 1: Validate input
        //        string requisitioner = txtRequisitioner.Text.Trim();
        //        string itemId = ItemIdSmall.Text.Trim();
        //        string qtyText = PurchQty.Text.Trim();

        //        if (string.IsNullOrWhiteSpace(requisitioner))
        //        {
        //            ShowMessage("Requester is required.");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(itemId))
        //        {
        //            ShowMessage("Item Number is required.");
        //            return;
        //        }

        //        if (!decimal.TryParse(qtyText, out decimal purchQty))
        //        {
        //            ShowMessage("Quantity must be a valid number.");
        //            return;
        //        }

        //        // Step 2: Prepare the DataTable
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("Requisitioner", typeof(string));
        //        dt.Columns.Add("ItemId", typeof(string));
        //        dt.Columns.Add("PurchQty", typeof(decimal));

        //        DataRow dr = dt.NewRow();
        //        dr["Requisitioner"] = requisitioner;
        //        dr["ItemId"] = itemId;
        //        dr["PurchQty"] = purchQty;
        //        dt.Rows.Add(dr);

        //        // Step 3: Call Create method
        //        PurchaseRequisitionLine service = new PurchaseRequisitionLine();
        //        createResult = service.create(dt); // This sets isSuccess, Message, etc.

        //        // Step 4: Optional workflow submission (not needed here, but preserved structure)
        //        if (createResult.isSuccess && submitRequest)
        //        {
        //            // Optional: Add workflow submission here if needed
        //            createResult.Message += " (Auto-submission not implemented.)";
        //        }

        //        // Step 5: Show result to user
        //        bool result = operationResults(createResult, true);

        //        if (createResult.isSuccess)
        //        {
        //            //ClearForm(); // Optional: clear inputs after success
        //            ScriptManager.RegisterStartupScript(this, GetType(), "close", "closeDialog(true);", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        createResult.isSuccess = false;
        //        createResult.Message = "An error occurred: " + ex.Message;
        //        createResult.AlertType = AlertType.Error.ToString();

        //        operationResults(createResult, false);
        //    }
        //}

        private void ShowMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"alert('{message.Replace("'", "\\'")}');", true);
        }



    }
}
