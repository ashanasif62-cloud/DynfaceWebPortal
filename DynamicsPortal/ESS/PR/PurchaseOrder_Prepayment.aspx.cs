using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_Prepayment : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedPurchIdPo"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoPoSelected",
                        "alert('No purchase order selected.'); closeDialog();", true);
                    return;
                }
                // Initialize form data if needed
                BindPrepaymentCategory();
            }
        }


        private void BindPrepaymentCategory()
        {
            PurchaseOrder_PrePaymentsSvc neworder = new PurchaseOrder_PrePaymentsSvc();
            DataTable dt = neworder.retrieveCategoryId(); // You should implement this method

            ddlPrepaymentCategory.Items.Clear();
            ddlPrepaymentCategory.Items.Insert(0, new ListItem("", ""));

            foreach (DataRow row in dt.Rows)
            {
                string categoryId = row["categoryName"].ToString();     // confirm actual column name
                string categoryName = row["descriptioncategory"].ToString(); // confirm actual column name
                string text = categoryId + " - " + categoryName;
                ddlPrepaymentCategory.Items.Add(new ListItem(text, categoryId));
            }

            ddlPrepaymentCategory.CssClass += " filterable-dropdown";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // --- 1. Validate form input ---
            string purchId = Session["SelectedPurchIdPo"] as string;
            if (string.IsNullOrEmpty(purchId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "PurchIdError",
                    "alert('Missing purchase order id.');", true);
                return;
            }

            string categoryName = ddlPrepaymentCategory.SelectedValue;
            if (string.IsNullOrEmpty(categoryName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "CategoryError",
                    "alert('Please select a prepayment category.');", true);
                return;
            }

            decimal prepaymentValue;
            if (!decimal.TryParse(txtValue.Text, out prepaymentValue) || prepaymentValue <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ValueError",
                    "alert('Prepayment value must be greater than zero.');", true);
                return;
            }

            string description = txtDescription.Text.Trim();

            // --- 2. Build the single-row DataTable that create() expects ---
            DataTable dt = new DataTable();
            dt.Columns.Add("purchId", typeof(string));
            dt.Columns.Add("categoryName", typeof(string));
            dt.Columns.Add("description", typeof(string));
            dt.Columns.Add("prepaymentValue", typeof(string));

            DataRow row = dt.NewRow();
            row["purchId"] = purchId;
            row["categoryName"] = categoryName;
            row["description"] = description;
            row["prepaymentValue"] = prepaymentValue.ToString();
            dt.Rows.Add(row);

            // --- 3. Call the service that wraps create(DataTable) ---
            try
            {
                PurchaseOrder_PrePaymentsSvc svc = new PurchaseOrder_PrePaymentsSvc();
                SysOperationResult_BOL result = svc.create(dt);

                if (result != null && result.isSuccess) // confirm actual property name on SysOperationResult_BOL
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseDialog",
                        "closeDialog();", true);
                    NotificationMessage.showMessage(result);
                }
                //else
                //{
                //    string msg = (result != null && !string.IsNullOrEmpty(result.Message))
                //        ? result.Message
                //        : "Unable to create prepayment.";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "ServiceError",
                //        "alert(" + System.Web.HttpUtility.JavaScriptStringEncode(msg, true) + ");", true);
                //}
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                objErrorLog.write(currentMethod.DeclaringType.FullName, ex);

                ScriptManager.RegisterStartupScript(this, GetType(), "UnexpectedError",
                    "alert('An unexpected error occurred while saving the prepayment.');", true);
            }

        }
    }
    }
