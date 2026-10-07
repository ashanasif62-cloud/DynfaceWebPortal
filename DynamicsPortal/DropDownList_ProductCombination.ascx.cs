using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_ProductCombination : System.Web.UI.UserControl
    {
        public event EventHandler<DataRow> ProductSelected;

        private const string ViewStateKey = "ProductCombinations";

        TransferOrderLines lines = new TransferOrderLines();
        public void Load(string itemId)
        {
            txtSelectedCombination.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(itemId)) return;

            DataTable productTable = lines.retrieveProductCombination(itemId);
            ViewState[ViewStateKey] = productTable;

            gvProductCombinations.DataSource = productTable;
            gvProductCombinations.DataBind();
        }

        protected void gvProductCombinations_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = ViewState[ViewStateKey] as DataTable;
            if (dt == null) return;

            GridViewRow selectedRow = gvProductCombinations.SelectedRow;
            string selectedDimId = selectedRow.Cells[1].Text.Trim(); // productDisplayName

            DataRow selectedDataRow = dt.AsEnumerable()
                .FirstOrDefault(r => r["productDisplayName"].ToString() == selectedDimId);

            if (selectedDataRow != null)
            {
                txtSelectedCombination.Text = $"{selectedDataRow["productDisplayName"]} | {selectedDataRow["ConfigId"]} | {selectedDataRow["InventStyle"]} | {selectedDataRow["InventColorId"]} | {selectedDataRow["InventSizeId"]}";
                ProductSelected?.Invoke(this, selectedDataRow);
            }
        }

        public string SelectedInventDimId
        {
            get
            {
                DataTable dt = ViewState[ViewStateKey] as DataTable;
                string text = txtSelectedCombination.Text;
                if (dt != null && !string.IsNullOrEmpty(text))
                {
                    var parts = text.Split('|');
                    string productDisplayName = parts[0].Trim();

                    var match = dt.AsEnumerable().FirstOrDefault(r =>
                        r["productDisplayName"].ToString() == productDisplayName);
                    return match?["InventDimId"].ToString();
                }
                return null;
            }
        }
    }
}