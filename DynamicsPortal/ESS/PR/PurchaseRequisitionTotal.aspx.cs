using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Shapes;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequisitionTotal : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string purchaseReqId = Request.QueryString["PurchReqId"];
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoSelection",
                    purchaseReqId, true);
                bindCurrencyCode();
                getData(purchaseReqId, ddlCurrency.Text);
            }
        }

        private void getData(string _purchReqId, string _currencyCode)
        {
            PurchaseRequestGroup purchReq = new PurchaseRequestGroup();
            DataTable purchReqTable = purchReq.retrievePurchTotals(_purchReqId, _currencyCode);

            if (purchReqTable != null)
            {
                DataRow row = purchReqTable.Rows[0];
                txtLineDiscount.Text = row["LineDiscount"].ToString();
                txtSubtotalAmount.Text = row["SubTotalAmount"].ToString();
                txtCharges.Text = row["Charges"].ToString();
                txtSalesTax.Text = row["SalesTax"].ToString();
                txtRoundOff.Text = row["RoundOff"].ToString();
                txtTotalAmount.Text = row["TotalAmount"].ToString();
            }
        }

        private void bindCurrencyCode()
        {
            DataTable dtExchangeCode = ViewState["ExchangeCodeData"] as DataTable ?? ControlsHelper.retrieveAllCurrencyDetails();
            if (dtExchangeCode != null && dtExchangeCode.Columns.Contains("CurrencyCode"))
            {
                ViewState["ExchangeCodeData"] = dtExchangeCode;
                ddlCurrency.DataSource = dtExchangeCode;
                ddlCurrency.DataTextField = "CurrencyCode";
                ddlCurrency.DataValueField = "CurrencyCode";
                ddlCurrency.DataBind();
                ddlCurrency.Items.Insert(0, new ListItem("", String.Empty));
                ddlCurrency.CssClass += " filterable-dropdown";

                if (ddlCurrency.Items.FindByValue("USD") != null)
                {
                    ddlCurrency.SelectedValue = "USD";
                }
            }
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            string purchaseReqId = Request.QueryString["PurchReqId"];
            getData(purchaseReqId, ddlCurrency.Text);
        }
    }
}