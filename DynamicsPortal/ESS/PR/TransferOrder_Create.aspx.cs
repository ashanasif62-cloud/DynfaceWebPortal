using BussinessObject;
using GeneralAuxiliary;
using OfficeOpenXml.Style;
using PortalIntegration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class TransferOrder_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "TransferOrder_Create";

            base.Page_Load(sender, e);

            // Optional: You could load data only once on first load
            if (!IsPostBack)
            {
                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create Transfer Order";
                    titleDiv.Style["font-weight"] = "600";   // semi-bold
                    titleDiv.Style["font-size"] = "20px";    // slightly larger
                    titleDiv.Style["color"] = "#000000";     // solid black
                    titleDiv.Style["margin"] = "10px 0";     // spacing around
                }

                //ddlTransferStatus.Text = "Created";



                //bindData(); //comment due to change Drop Down
                //bindDatatowarehouse(); //comment due to change Drop Down
                   //bindDatatransferstatus();
            }

            string today = DateTime.Now.ToString("M-d-yyyy");
            txtShippingDate.Text = today;
            txtReceiptDate.Text = today;
        }

        private void bindData()
        {
            //TransferOrderHeader transferordernew = new TransferOrderHeader();
            //DataTable dt = transferordernew.retrievefromwarehouse();

            //dt.Columns.Add("DisplayText", typeof(string));

            //foreach (DataRow row in dt.Rows)
            //{
            //    row["DisplayText"] = row["FromWarehouse"] + " - " + row["WarehouseName"];
            //}

            //ddlFromWarehouse.DataSource = dt;
            //ddlFromWarehouse.DataValueField = "FromWarehouse";   // value to pass
            //ddlFromWarehouse.DataTextField = "DisplayText";    // text to show
            //ddlFromWarehouse.DataBind();

            //ddlFromWarehouse.Items.Insert(0, new ListItem("", string.Empty));

            //ddlFromWarehouse.CssClass += " filterable-dropdown";
        }

        private void bindDatatowarehouse()
        {
            //TransferOrderHeader transferordernew = new TransferOrderHeader();
            //DataTable dt = transferordernew.retrievetowarehouse();

            //dt.Columns.Add("DisplayText", typeof(string));

            //foreach (DataRow row in dt.Rows)
            //{
            //    row["DisplayText"] = row["ToWarehouse"] + " - " + row["WarehouseName"];
            //}

            //ddlToWarehouse.DataSource = dt;
            //ddlToWarehouse.DataValueField = "ToWarehouse";   // value to pass
            //ddlToWarehouse.DataTextField = "DisplayText";    // text to show
            //ddlToWarehouse.DataBind();

            //// Add a default item to show it's empty
            //    ddlToWarehouse.Items.Insert(0, new ListItem("", string.Empty));

            //ddlToWarehouse.CssClass += " filterable-dropdown";
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare validation variables
                //string fromWarehouse = ddlFromWarehouse.SelectedValue;
                string fromWarehouse = (DropDownList_FromWearhouse.FindControl("txtWarehouseId") as System.Web.UI.WebControls.TextBox).Text;
                string toWarehouse = (DropDownList_ToWearhouse.FindControl("txtWarehouseId") as System.Web.UI.WebControls.TextBox).Text;
                //string toWarehouse = ddlToWarehouse.SelectedValue;
                string shippingDateText = txtShippingDate.Text.Trim();
                string receiptDateText = txtReceiptDate.Text.Trim();
                DateTime shippingDate, receiptDate, today = DateTime.Today;
                // Define accepted formats
                string[] formats = { "M-d-yyyy"};


                // Validate required fields
                if (string.IsNullOrEmpty(fromWarehouse) && string.IsNullOrEmpty(toWarehouse))
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Please select both From and To warehouses.";
                    NotificationMessage.showMessage(error);
                    return;
                }

                // Validate required fields
                else if (string.IsNullOrEmpty(fromWarehouse))
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Please select From warehouses.";
                    NotificationMessage.showMessage(error);
                    return;
                }

                // Validate required fields
                else if (string.IsNullOrEmpty(toWarehouse))
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Please select To warehouses.";
                    NotificationMessage.showMessage(error);
                    return;
                }

                // Validate same warehouse
                if (fromWarehouse == toWarehouse)
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "From and To Warehouse cannot be the same";
                    NotificationMessage.showMessage(error);
                    return;
                }

                if (!DateTime.TryParseExact(shippingDateText, formats,
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out shippingDate) ||
    !DateTime.TryParseExact(receiptDateText, formats,
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out receiptDate))
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Please enter valid Shipping and Receipt dates (MM-DD-YYYY).";
                    NotificationMessage.showMessage(error);
                    return;
                }


                // Validate dates are today or future
                if (shippingDate < today || receiptDate < today)
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Shipping and Receipt Date must be today or greater";
                    NotificationMessage.showMessage(error);
                    return;
                }

                // Prepare DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("FromWarehouse");
                dt.Columns.Add("ToWarehouse");
                dt.Columns.Add("ShippingDate");
                dt.Columns.Add("ReceiptDate");

                DataRow row = dt.NewRow();
                row["FromWarehouse"] = fromWarehouse;
                row["ToWarehouse"] = toWarehouse;
                row["ShippingDate"] = shippingDate;
                row["ReceiptDate"] = receiptDate;
                dt.Rows.Add(row);

                // Call service
                TransferOrderHeader transferOrder = new TransferOrderHeader();
                SysOperationResult_BOL result = transferOrder.create(dt);

                if (result != null && result.isSuccess)
                {
                    NotificationMessage.showMessage(result);

                    Match match = Regex.Match(result.Message, @"\d+"); // find one or more digits

                    if (match.Success)
                    {
                        string transferNumber = match.Value;
                        
                        Session["Status"] = "Created";

                        Session["ShipDate"] = shippingDate;

                        Session["ReceiveDate"] = receiptDate;

                        Session["FromWarehouse"] = fromWarehouse;

                        Session["ToWarehouse"] = toWarehouse;

                        Session["TransferID"] = transferNumber;
                    }
                    // Close modal after delay
                    string script = @"
                            setTimeout(function() { 
                                window.top.location = '/ESS/PR/TransferOrderLines_ListPage.aspx'; 
                            }, 3000);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                }
                else
                {
                    NotificationMessage.showMessage(result);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.pREmployeeLoanRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

      
    }
}
    