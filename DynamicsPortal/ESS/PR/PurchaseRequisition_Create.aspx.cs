using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System.Globalization;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchasedRequisition_Create : ModalForm
    {
        private PurchaseRequestGroup purchaseRequestGroup = new PurchaseRequestGroup(); 
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "PRPurchaseRequestCreate";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Create purchase requisition";
                        titleDiv.Style["font-weight"] = "600";   // semi-bold
                        titleDiv.Style["font-size"] = "20px";    // slightly larger
                        titleDiv.Style["color"] = "#000000";     // solid black
                        titleDiv.Style["margin"] = "10px 0";     // spacing around
                    }



                    string today = DateTime.Now.ToString("M-d-yyyy");
                    txtRequestedDate.Text = today;
                    txtAccountingDate.Text = today;


                    BindPurchaseReqID();
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                new SysErrorLog().write(GetType().FullName + ".Page_Load", ex);

                // Optionally show a user-friendly message
                ScriptManager.RegisterStartupScript(this, GetType(), "pageLoadError", "alert('An error occurred while loading the form.');", true);
            }
        }


        private void BindPurchaseReqID()
        {
            PurchaseRequestGroup newgroup = new PurchaseRequestGroup();

            // Assuming retrieveVendor returns a DataTable with a column "PurchID"
            DataTable dt = newgroup.retrievenumberseq();

            if (dt != null && dt.Rows.Count > 0)
            {
                // Example: Taking the first row's PurchID
                string purchreqID = dt.Rows[0]["PurchReqId"].ToString();

                // Set the value to the TextBox
                txtPurchReqId.Text = purchreqID;
            }
        }




        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string name = PurchReqName.Text.Trim();
        //        string createdDateText = TextCreatedDateTime.Text.Trim();
        //        string submittedDateText = txtSubmittedDateTime.Text.Trim();

        //        if (string.IsNullOrWhiteSpace(name))
        //        {
        //            ShowMessage("Name is required.");
        //            return;
        //        }

        //        if (!DateTime.TryParse(createdDateText, out DateTime createdDate))
        //        {
        //            ShowMessage("Invalid Created Date.");
        //            return;
        //        }

        //        if (!DateTime.TryParse(submittedDateText, out DateTime submittedDate))
        //        {
        //            ShowMessage("Invalid Submitted Date.");
        //            return;
        //        }

        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("PurchReqName", typeof(string));
        //        dt.Columns.Add("CreatedDateTime", typeof(DateTime));
        //        dt.Columns.Add("SubmittedDateTime", typeof(DateTime));

        //        DataRow dr = dt.NewRow();
        //        dr["PurchReqName"] = name;
        //        dr["CreatedDateTime"] = createdDate;
        //        dr["SubmittedDateTime"] = submittedDate;
        //        dt.Rows.Add(dr);

        //        var service = new PurchaseRequestGroup();
        //        SysOperationResult_BOL result = service.create(dt);

        //        string msg = "Purchase Requisition submitted successfully!";
        //        var type = result.GetType();

        //        var propIsSuccess = type.GetProperty("IsSuccess") ?? type.GetProperty("Success");
        //        var propMessage = type.GetProperty("Message") ?? type.GetProperty("ErrorMessage");

        //        bool isSuccess = false;
        //        string message = null;

        //        if (propIsSuccess != null)
        //        {
        //            isSuccess = (bool)propIsSuccess.GetValue(result);
        //        }

        //        if (propMessage != null)
        //        {
        //            message = (string)propMessage.GetValue(result);
        //        }

        //        if (isSuccess)
        //        {
        //            ShowMessage(message ?? msg, isSuccess: true);
        //            ClearForm();
        //        }
        //        else
        //        {
        //            ShowMessage(message ?? "Failed to submit Purchase Requisition.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new SysErrorLog().write("PurchasedRequisition_Create.aspx.cs", ex);
        //        ShowMessage("An error occurred while submitting the form.");
        //    }
        //}

        //private void ShowMessage(string message, bool isSuccess = false)
        //{
        //    string script = $"alert('{message}');";
        //    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", script, true);
        //}

        //private void ClearForm()
        //{
        //    PurchReqName.Text = string.Empty;
        //    TextCreatedDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //    txtSubmittedDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Submit_Click(object sender, EventArgs e)
        {
            create_SubmitRequest();
        }

        private void createRequest()
        {
            create_SubmitRequest(false);
        }

        //private void create_SubmitRequest(bool _submitRequest = true)
        //{
        //    long requestRecId = 0;
        //    bool submitRequest = _submitRequest;
        //    SysOperationResult_BOL createResult = new SysOperationResult_BOL();
        //    SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

        //    // Step 1: Validate input
        //    string name = PurchReqName.Text.Trim();
        //    if (string.IsNullOrWhiteSpace(name))
        //    {
        //        ShowMessage("Purchase Requisition Name is required.");
        //        return;
        //    }

        //    if (!DateTime.TryParseExact(txtRequestedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime requesteddate))
        //    {
        //        ShowMessage("Invalid Created Date.");
        //        return;
        //    }

        //    if (!DateTime.TryParseExact(txtAccountingDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime accountingdate))
        //    {
        //        ShowMessage("Invalid Submitted Date.");
        //        return;
        //    }

        //    // Step 2: Prepare the DataTable
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PurchReqName", typeof(string));
        //    dt.Columns.Add("RequestedDate", typeof(DateTime));
        //    dt.Columns.Add("AccountingDate", typeof(DateTime));

        //    DataRow dr = dt.NewRow();
        //    dr["PurchReqName"] = name;
        //    dr["RequestedDate"] = requesteddate;
        //    dr["AccountingDate"] = accountingdate;
        //    dt.Rows.Add(dr);

        //    // Step 3: Call Create method
        //    createResult = purchaseRequestGroup.create(dt);
        //    requestRecId = createResult.RecId;

        //    // Step 4: Handle optional submission
        //    //if (createResult.isSuccess && submitRequest)
        //    //{
        //    //    if (requestRecId > 0)
        //    //    {
        //    //        submitResult = submitWFRequest(requestRecId);

        //    //        if (submitResult.isSuccess)
        //    //        {
        //    //            submitResult.Message = "Request successfully submitted.";
        //    //        }
        //    //        else
        //    //        {
        //    //            submitResult.Message = "Failed to submit the request.";
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        submitResult.AlertType = AlertType.Error.ToString();
        //    //        submitResult.isSuccess = false;
        //    //        submitResult.Message = "Failed to submit the created request.";
        //    //    }

        //    //    // Merge messages into final result
        //    //    createResult.Message += " " + submitResult.Message;
        //    //    createResult.AlertType = submitResult.AlertType;
        //    //    createResult.isSuccess = submitResult.isSuccess;
        //    //}

        //    // Step 5: Show result and optionally clear form
        //    bool result = operationResults(createResult, true);

        //    if (createResult.isSuccess)
        //    {
        //        ClearForm();
        //    }
        //}

        //private SysOperationResult_BOL submitWFRequest(long RequestRecId)
        //{
        //    long requestRecId = RequestRecId; // Use the correct parameter name

        //    ESSWorkflow eSSWorkflow = new ESSWorkflow();
        //    SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

        //    if (requestRecId > 0)
        //    {
        //        operationResult_BOL = eSSWorkflow.purchaseRequest_Submit(requestRecId);
        //    }

        //    return operationResult_BOL;
        //}


        private void create_SubmitRequest(bool _submitRequest = true)
        {
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            lblMessage.Text = "";
            lblMessage.Visible = false;
            // Prepare data
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchReqName", typeof(string));
            dt.Columns.Add("RequiredDate", typeof(DateTime));
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("PurchReqId", typeof(string));

            string name = PurchReqName.Text.Trim();
            string requestedDateStr = txtRequestedDate.Text.Trim();
            string accountingDateStr = txtAccountingDate.Text.Trim();
            string purchreqId = txtPurchReqId.Text.Trim();


            // Validation checks
            if (string.IsNullOrWhiteSpace(name))
            {
                  lblMessage.Text = "Field 'Name' must be filled in.";
               







            }
            else if (name.Length > 60)
            {
                lblMessage.Text = "Purchase Requisition <b>Name</b> cannot exceed 60 characters.";
            }
            else if (string.IsNullOrWhiteSpace(requestedDateStr))
            {
                lblMessage.Text = "<b>Requested Date</b> is required.";
            }
            else if (string.IsNullOrWhiteSpace(accountingDateStr))
            {
                lblMessage.Text = "<b>Accounting Date</b> is required.";
            }
            else
            {
                DateTime today = DateTime.Today;
                DateTime requesteddate = DateTime.ParseExact(requestedDateStr, "M-d-yyyy", CultureInfo.InvariantCulture);
                DateTime accountingdate = DateTime.ParseExact(accountingDateStr, "M-d-yyyy", CultureInfo.InvariantCulture);


                if (requesteddate < today || accountingdate < today)
                {
                    lblMessage.Text = "<b>Requested</b> and <b>Accounting</b> dates must be today or a future date.";
                }
                else
                {
                    // Populate data row
                    DataRow row = dt.NewRow();
                    row["PurchReqName"] = name;
                    row["RequiredDate"] = requesteddate;
                    row["TransDate"] = accountingdate;
                    row["PurchReqId"] = purchreqId;
                    dt.Rows.Add(row);

                    // Call business logic
                    PurchaseRequestGroup purchaseRequestGroup = new PurchaseRequestGroup();
                    createResult = purchaseRequestGroup.create(dt);

                    if (createResult != null && createResult.isSuccess)
                    {
                        NotificationMessage.showMessage(createResult);


                        string script = @"
        setTimeout(function() { 
            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                window.parent.refreshParentGrid();
            }
            closeDialog(); 
        }, 3000);";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                    }

                    // Exit early - no need to show validation message
                    return;
                }
            }
            if (!string.IsNullOrEmpty(lblMessage.Text))
            {
                // Show validation error
                lblMessage.CssClass = "text-danger";
                lblMessage.Style["background-color"] = "#f8d7da";
                lblMessage.Style["padding"] = "6px";
                lblMessage.Style["font-size"] = "12px";
                lblMessage.Style["display"] = "block";
                lblMessage.Visible = true;
                lblMessage.Controls.Clear();
                lblMessage.Controls.Add(new LiteralControl($@"
    <div id='{lblMessage.ClientID}' 
         style='background-color:#f8d7da;color:#721c24;padding:8px;
                border:1px solid #f5c6cb;border-radius:4px;position:relative;'>
        {lblMessage.Text}
        <span onclick='document.getElementById(""{lblMessage.ClientID}"").style.display=""none"";' 
              style='cursor:pointer;position:absolute;right:8px;top:4px;font-weight:bold;'>×</span>
    </div>"));
            }
        }



        private void ShowMessage(string message, bool isSuccess = false)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", script, true);
        }

        //protected void btnCreate_Submit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string name = PurchReqName.Text.Trim();
        //        string createdDateText = TextCreatedDateTime.Text.Trim();
        //        string submittedDateText = txtSubmittedDateTime.Text.Trim();

        //        if (string.IsNullOrWhiteSpace(name))
        //        {
        //            ShowMessage("Name is required.");
        //            return;
        //        }

        //        if (!DateTime.TryParse(createdDateText, out DateTime createdDate))
        //        {
        //            ShowMessage("Invalid Created Date.");
        //            return;
        //        }

        //        if (!DateTime.TryParse(submittedDateText, out DateTime submittedDate))
        //        {
        //            ShowMessage("Invalid Submitted Date.");
        //            return;
        //        }

        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("PurchReqName", typeof(string));
        //        dt.Columns.Add("CreatedDateTime", typeof(DateTime));
        //        dt.Columns.Add("SubmittedDateTime", typeof(DateTime));
        //        dt.Columns.Add("IsSubmitted", typeof(bool)); // Optional column to indicate submission

        //        DataRow dr = dt.NewRow();
        //        dr["PurchReqName"] = name;
        //        dr["CreatedDateTime"] = createdDate;
        //        dr["SubmittedDateTime"] = submittedDate;
        //        dr["IsSubmitted"] = true; // Flag for full submission
        //        dt.Rows.Add(dr);

        //        var service = new PurchaseRequestGroup();
        //        SysOperationResult_BOL result = service.create(dt); // You may want to implement a separate method like `submit(dt)`

        //        string msg = "Purchase Requisition submitted successfully!";
        //        var type = result.GetType();

        //        var propIsSuccess = type.GetProperty("IsSuccess") ?? type.GetProperty("Success");
        //        var propMessage = type.GetProperty("Message") ?? type.GetProperty("ErrorMessage");

        //        bool isSuccess = false;
        //        string message = null;

        //        if (propIsSuccess != null)
        //        {
        //            isSuccess = (bool)propIsSuccess.GetValue(result);
        //        }

        //        if (propMessage != null)
        //        {
        //            message = (string)propMessage.GetValue(result);
        //        }

        //        if (isSuccess)
        //        {
        //            ShowMessage(message ?? msg, isSuccess: true);
        //            ClearForm();
        //        }
        //        else
        //        {
        //            ShowMessage(message ?? "Failed to submit Purchase Requisition.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new SysErrorLog().write("PurchasedRequisition_Create.aspx.cs (Submit)", ex);
        //        ShowMessage("An error occurred while submitting the form.");
        //    }
        //}



        //private string GenerateRequestId()
        //{
        //    Dummy logic to generate an ID(replace with real logic or number sequence)
        //    return $"PR-{DateTime.Now:yyyyMMddHHmmss}";
        //}

        private void getGridDataTable()
        {
            DataTable dt = purchaseRequestGroup.retrieveAll();
            SessionVariables.setSessionDataTable(dt);
        }
    }

}
