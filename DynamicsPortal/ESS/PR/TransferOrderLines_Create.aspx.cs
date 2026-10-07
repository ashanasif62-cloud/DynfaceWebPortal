using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class TransferOrderLines_Create : ModalForm
    {
        private TransferOrderLines transferlines = new TransferOrderLines();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferOrderLine_Create";

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
                        titleDiv.InnerText = "Create Transfer Order Lines";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    string transferId = Request.QueryString["TransferID"];
                    if (!string.IsNullOrEmpty(transferId))
                    {
                        txtTransferId.Text = transferId;
                        //txtIsCatchWeight.Text = "false";
                        txtTransferId.BackColor = System.Drawing.Color.LightGray;
                            
                    }
                    
                    bindData(); // Item ID
                    ////BindInventBatchId();
                    ////BindWmsLocationId();
                    ////BindWmsPalletId();
                    ////BindInventSerialId();
                    ////BindInventLocationId();
                    ////BindConfigId();
                    //BindInventSizeId();
                    ////BindInventColorId();
                    ////BindInventSiteId();
                    ////BindInventStyleId();
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

        protected void ddlItemid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItemId = ddlItemid.SelectedValue;

            if (!string.IsNullOrEmpty(selectedItemId))
            {
                // Call your methods to load inventory-related dropdowns
                BindInventBatchId(selectedItemId);
                BindWmsLocationId(selectedItemId);
                BindWmsPalletId(selectedItemId);
                BindInventSerialId(selectedItemId);
                BindWmsLocationId(selectedItemId);
                BindConfigId(selectedItemId);
                BindInventSizeId(selectedItemId);
                BindInventColorId(selectedItemId);
                BindInventStyleId(selectedItemId);
                //LoadSiteId(selectedItemId);
                ProductLookupControl.Load(selectedItemId);

                txtQtyTransfer.Text = "1";
            }
        }

        private void bindData()
        {
            TransferOrderLines newlines = new TransferOrderLines();
            DataTable dt = newlines.retrieveitemid();

            ddlItemid.DataSource = dt;
            ddlItemid.DataValueField = "Itemid";   // value to pass
            ddlItemid.DataTextField = "Itemid";    // text to show
            ddlItemid.DataBind();


            ddlItemid.Items.Insert(0, new ListItem("", string.Empty));

            ddlItemid.CssClass += " filterable-dropdown";
        }

        private void BindInventBatchId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventBatchId(itemid);

            ddlInventBatchId.DataSource = dt;
            ddlInventBatchId.DataValueField = "InventBatchId";
            ddlInventBatchId.DataTextField = "InventBatchId";
            ddlInventBatchId.DataBind();

            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlInventBatchId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventBatchId.Items[0].Text)))
            {
             
                // Clear the dropdown first to remove any empty items
                ddlInventBatchId.Items.Clear();

                // Disable the dropdown
                ddlInventBatchId.Enabled = false;

                // Set grey background color
                ddlInventBatchId.BackColor = System.Drawing.Color.LightGray;

                // Add a default item to show it's empty
                ddlInventBatchId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                // Enable the dropdown
                ddlInventBatchId.Enabled = true;

                // Reset to default background color
                ddlInventBatchId.BackColor = System.Drawing.Color.White;

                // Add empty item at the top for selection
                ddlInventBatchId.Items.Insert(0, new ListItem("", string.Empty));

                ddlInventBatchId.CssClass += " filterable-dropdown";
            }
        }

        private void BindWmsLocationId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrievewmsLocationId(itemid);
            ddlWmsLocationId.DataSource = dt;
            ddlWmsLocationId.DataValueField = "WmsLocationId";
            ddlWmsLocationId.DataTextField = "WmsLocationId";
            ddlWmsLocationId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlWmsLocationId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsLocationId.Items[0].Text)))
            {
                rowWmsLocation.Visible = false;

                // Clear the dropdown first to remove any empty items
                ddlWmsLocationId.Items.Clear();
                // Disable the dropdown
                ddlWmsLocationId.Enabled = false;
                // Set grey background color
                ddlWmsLocationId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlWmsLocationId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowWmsLocation.Visible = true;

                // Enable the dropdown
                ddlWmsLocationId.Enabled = true;
                // Reset to default background color
                ddlWmsLocationId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlWmsLocationId.Items.Insert(0, new ListItem("", string.Empty));

                ddlWmsLocationId.CssClass += " filterable-dropdown";
            }
        }

        private void BindWmsPalletId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrievewmsPalletId(itemid);
            ddlWmsPalletId.DataSource = dt;
            ddlWmsPalletId.DataValueField = "WmsPalletId";
            ddlWmsPalletId.DataTextField = "WmsPalletId";
            ddlWmsPalletId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlWmsPalletId.Items.Count == 1 && string.IsNullOrEmpty(ddlWmsPalletId.Items[0].Text)))
            {
                rowWmsPallet.Visible = false;
                // Clear the dropdown first to remove any empty items
                ddlWmsPalletId.Items.Clear();
                // Disable the dropdown
                ddlWmsPalletId.Enabled = false;
                // Set grey background color
                ddlWmsPalletId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlWmsPalletId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));

            }
            else
            {
                rowWmsPallet.Visible = true;

                // Enable the dropdown
                ddlWmsPalletId.Enabled = true;
                // Reset to default background color
                ddlWmsPalletId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlWmsPalletId.Items.Insert(0, new ListItem("", string.Empty));

                ddlWmsPalletId.CssClass += " filterable-dropdown";

            }
        }

        private void BindInventSerialId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSerialId(itemid);
            ddlInventSerialId.DataSource = dt;
            ddlInventSerialId.DataValueField = "InventSerialId";
            ddlInventSerialId.DataTextField = "InventSerialId";
            ddlInventSerialId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlInventSerialId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSerialId.Items[0].Text)))
            {
                rowInventSerialId.Visible = false;
                // Clear the dropdown first to remove any empty items
                ddlInventSerialId.Items.Clear();
                // Disable the dropdown
                ddlInventSerialId.Enabled = false;
                // Set grey background color
                ddlInventSerialId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlInventSerialId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowInventSerialId.Visible = true;
                // Enable the dropdown
                ddlInventSerialId.Enabled = true;
                // Reset to default background color
                ddlInventSerialId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlInventSerialId.Items.Insert(0, new ListItem("", string.Empty));

                ddlInventSerialId.CssClass += " filterable-dropdown";
            }
        }

        //private void BindInventLocationId(string itemid)
        //{
        //    TransferOrderLines lines = new TransferOrderLines();
        //    DataTable dt = lines.retrieveinventLocationId(itemid);
        //    ddlInventLocationId.DataSource = dt;
        //    ddlInventLocationId.DataValueField = "InventLocationId";
        //    ddlInventLocationId.DataTextField = "InventLocationId";
        //    ddlInventLocationId.DataBind();

        //    ddlInventLocationId.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        //}

        private void BindConfigId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveconfigId(itemid);
            ddlConfigId.DataSource = dt;
            ddlConfigId.DataValueField = "ConfigId";
            ddlConfigId.DataTextField = "ConfigId";
            ddlConfigId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlConfigId.Items.Count == 1 && string.IsNullOrEmpty(ddlConfigId.Items[0].Text)))
            {
                rowConfigid.Visible = false;
                rowCombination.Visible = false;
                rowConfigid.Visible = false;
                // Clear the dropdown first to remove any empty items
                ddlConfigId.Items.Clear();
                // Disable the dropdown
                ddlConfigId.Enabled = false;
                // Set grey background color
                ddlConfigId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlConfigId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowConfigid.Visible = true;
                rowCombination.Visible = true;

                rowConfigid.Visible = true;

                // Enable the dropdown
                ddlConfigId.Enabled = true;
                // Reset to default background color
                ddlConfigId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlConfigId.Items.Insert(0, new ListItem("", string.Empty));

                ddlConfigId.CssClass += " filterable-dropdown";
            }
        }

        private void BindInventSizeId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventSizeId(itemid);
            ddlInventSizeId.DataSource = dt;
            ddlInventSizeId.DataValueField = "InventSizeId";
            ddlInventSizeId.DataTextField = "InventSizeId";
            ddlInventSizeId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlInventSizeId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventSizeId.Items[0].Text)))
            {

                rowSizeid.Visible = false;
                // Clear the dropdown first to remove any empty items
                ddlInventSizeId.Items.Clear();
                // Disable the dropdown
                ddlInventSizeId.Enabled = false;
                // Set grey background color
                ddlInventSizeId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlInventSizeId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowSizeid.Visible=true;

                // Enable the dropdown
                ddlInventSizeId.Enabled = true;
                // Reset to default background color
                ddlInventSizeId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlInventSizeId.Items.Insert(0, new ListItem("", string.Empty));

                ddlInventSizeId.CssClass += " filterable-dropdown";
            }
        }

        private void BindInventColorId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinventColorId(itemid);
            ddlInventColorId.DataSource = dt;
            ddlInventColorId.DataValueField = "InventColorId";
            ddlInventColorId.DataTextField = "InventColorId";
            ddlInventColorId.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlInventColorId.Items.Count == 1 && string.IsNullOrEmpty(ddlInventColorId.Items[0].Text)))
            {
                rowColorid.Visible = false;

                // Clear the dropdown first to remove any empty items
                ddlInventColorId.Items.Clear();
                // Disable the dropdown
                ddlInventColorId.Enabled = false;
                // Set grey background color
                ddlInventColorId.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlInventColorId.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowColorid.Visible = true;

                // Enable the dropdown
                ddlInventColorId.Enabled = true;
                // Reset to default background color
                ddlInventColorId.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlInventColorId.Items.Insert(0, new ListItem("", string.Empty));

                ddlInventColorId.CssClass += " filterable-dropdown";
            }
        }

        //private void BindInventSiteId(string itemid)
        //{
        //    TransferOrderLines lines = new TransferOrderLines();
        //    DataTable dt = lines.retrieveinventSiteId();
        //    ddlInventSiteId.DataSource = dt;
        //    ddlInventSiteId.DataValueField = "InventSiteId";
        //    ddlInventSiteId.DataTextField = "InventSiteId";
        //    ddlInventSiteId.DataBind();

        //    ddlInventSiteId.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        //}

        private void BindInventStyleId(string itemid)
        {
            TransferOrderLines lines = new TransferOrderLines();
            DataTable dt = lines.retrieveinvetstyleid(itemid);
            ddlInventStyle.DataSource = dt;
            ddlInventStyle.DataValueField = "InventStyle";
            ddlInventStyle.DataTextField = "InventStyle";
            ddlInventStyle.DataBind();
            // Check if dropdown is empty and disable/style accordingly
            if (dt == null || dt.Rows.Count == 0 ||
                (ddlInventStyle.Items.Count == 1 && string.IsNullOrEmpty(ddlInventStyle.Items[0].Text)))
            {
                rowStyle.Visible = false;

                // Clear the dropdown first to remove any empty items
                ddlInventStyle.Items.Clear();
                // Disable the dropdown
                ddlInventStyle.Enabled = false;
                // Set grey background color
                ddlInventStyle.BackColor = System.Drawing.Color.LightGray;
                // Add a default item to show it's empty
                ddlInventStyle.Items.Insert(0, new ListItem("No Selection Available", string.Empty));
            }
            else
            {
                rowStyle.Visible = true;

                // Enable the dropdown
                ddlInventStyle.Enabled = true;
                // Reset to default background color
                ddlInventStyle.BackColor = System.Drawing.Color.White;
                // Add empty item at the top for selection
                ddlInventStyle.Items.Insert(0, new ListItem("", string.Empty));

                ddlInventStyle.CssClass += " filterable-dropdown";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

                // Prepare data
                DataTable dt = new DataTable();
                dt.Columns.Add("Itemid");
                dt.Columns.Add("TransferId");
                dt.Columns.Add("LinesShipDate");
                dt.Columns.Add("LinesReceiveDate");
                dt.Columns.Add("ReserveItem");
                dt.Columns.Add("IsCatchWeight");
                dt.Columns.Add("QtyTransfer");
                dt.Columns.Add("QtyShipped");


                dt.Columns.Add("InventBatchId");
                dt.Columns.Add("WmsLocationId");
                dt.Columns.Add("WmsPalletId");
                dt.Columns.Add("InventSerialId");
                dt.Columns.Add("InventLocationId");
                dt.Columns.Add("ConfigId");
                dt.Columns.Add("InventSizeId");
                dt.Columns.Add("InventColorId");
                dt.Columns.Add("InventSiteId");
                dt.Columns.Add("InventStyle");


                // Create new DataRow and add values
                DataRow row = dt.NewRow();

                // Add form field values to DataRow
                row["Itemid"] = ddlItemid.SelectedValue;
                row["TransferId"] = txtTransferId.Text;
                row["LinesShipDate"] = DateTime.Today;
                row["LinesReceiveDate"] = DateTime.Today;
                //row["ReserveItem"] = ddlReserveItem.SelectedValue;
                row["IsCatchWeight"] = false;

                row["InventBatchId"] = ddlInventBatchId.SelectedValue;
                row["WmsLocationId"] = ddlWmsLocationId.SelectedValue;
                row["WmsPalletId"] = ddlWmsPalletId.SelectedValue;
                row["InventSerialId"] = ddlInventSerialId.SelectedValue;
                string fromWarehouse = Session["FromWarehouse"] as string;
                row["InventLocationId"] = fromWarehouse;
                row["ConfigId"] = ddlConfigId.SelectedValue;
                row["InventSizeId"] = ddlInventSizeId.SelectedValue;
                row["InventColorId"] = ddlInventColorId.SelectedValue;
                //row["InventSiteId"] = ddlInventSiteId.SelectedValue;
                row["InventStyle"] = ddlInventStyle.SelectedValue;

                int transferQty;

                if (!int.TryParse(txtQtyTransfer.Text, out transferQty))
                {
                    lblMessage.Text = "Please enter a valid Transfer Quantity.";
                    messageContainer.Visible = true;
                    messageContainer.Attributes["class"] = "alert alert-danger";
                    messageContainer.Style["font-size"] = "12px";
                    messageContainer.Style["padding"] = "6px";
                    return; // Exit if invalid input
                }

                if (transferQty <= 0)
                {
                    lblMessage.Text = "Only a positive Transfer Quantity is allowed.";
                    messageContainer.Visible = true;
                    messageContainer.Attributes["class"] = "alert alert-danger";
                    messageContainer.Style["font-size"] = "12px";
                    messageContainer.Style["padding"] = "6px";
                    return; // Exit if zero or negative
                }

                row["QtyTransfer"] = transferQty;


                dt.Rows.Add(row);

                // Call the business logic/service class

                TransferOrderLines transferLines = new TransferOrderLines();

                SysOperationResult_BOL result = transferLines.create(dt); // Assuming create method returns SysOperationResult_BOL


                if (result != null && result.isSuccess)

                {

                    NotificationMessage.showMessage(result);
                    ClearForm();

                    // NEW script: Refresh parent GridView and reload this form

                    string refreshScript = $@"

                     if (window.opener && !window.opener.closed) {{
                        parent.__doPostBack('', 'RefreshGrid');
                      }}
                      setTimeout(function() {{
                      window.location.href = window.location.pathname + window.location.search;
                       }}, 1000);   ";

                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshAndReload", refreshScript, true);

                }
                else
                {
                    NotificationMessage.showMessage(result);
                }

            }

            catch (Exception ex)

            {
                lblMessage.Text = "Exception: " + ex.Message;
                lblMessage.Style["background-color"] = "#f8d7da"; // Light red
                lblMessage.Style["color"] = "#721c24";            // Dark red
                lblMessage.Style["font-size"] = "12px";
                lblMessage.Style["padding"] = "6px";
                lblMessage.Style["border-radius"] = "4px";
                lblMessage.Style["display"] = "block";
                lblMessage.Visible = true;

            }

        }

        private void ClearForm()
        {
            ddlItemid.ClearSelection();
            ddlInventBatchId.ClearSelection();
            ddlWmsLocationId.ClearSelection();
            ddlWmsPalletId.ClearSelection();
            ddlInventSerialId.ClearSelection();
            //ddlInventLocationId.ClearSelection();
            ddlConfigId.ClearSelection();
            ddlInventSizeId.ClearSelection();
            ddlInventColorId.ClearSelection();
            //ddlInventSiteId.ClearSelection();
            ddlInventStyle.ClearSelection();

            txtQtyTransfer.Text = string.Empty;
            // Add other fields as necessary
        }

        protected void ProductLookupControl_ProductSelected(object sender, DataRow selectedRow)
        {
            string configId = selectedRow["ConfigId"].ToString();
            string style = selectedRow["InventStyle"].ToString();
            string color = selectedRow["InventColorId"].ToString();
            string size = selectedRow["InventSizeId"].ToString();
            string inventDimId = selectedRow["InventDimId"].ToString();

            ddlConfigId.SelectedValue = configId;
            ddlInventStyle.SelectedValue = style;
            ddlInventColorId.SelectedValue = color;
            ddlInventSizeId.SelectedValue = size;
        }
    }
}