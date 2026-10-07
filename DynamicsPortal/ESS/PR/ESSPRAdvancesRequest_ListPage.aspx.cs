using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class ESSPRAdvancesRequest_ListPage : MainForm
    {
        private PRAdvancesRequest logic = new PRAdvancesRequest();

        protected override void Page_Load(object sender, EventArgs e)
        {
            tableId = "PREmployeeAdvance";
            pageMenuId = "ESSPRAdvancesRequest_ListPage";

            base.Page_Load(sender, e);
            if (!isUserAuthenticated) return;

            if (!IsPostBack)
            {
                reBindGrid();
            }
        }

        protected void reBindGrid()
        {
            DataTable dt = logic.getEmployeeAdvances();
            gvAdvancesRequest.DataSource = dt;
            gvAdvancesRequest.DataBind();

            if (dt != null && dt.Rows.Count > 0)
            {
                CalculateFooter(dt);
            }
        }

        private void CalculateFooter(DataTable dt)
        {
            decimal totalRecAmt = 0;
            foreach (DataRow dr in dt.Rows)
            {
                totalRecAmt += Convert.ToDecimal(dr["RecoveryAmount"]);
            }
            if (gvAdvancesRequest.FooterRow != null)
            {
                gvAdvancesRequest.FooterRow.Cells[2].Text = "Total";
                gvAdvancesRequest.FooterRow.Cells[3].Text = totalRecAmt.ToString("N2");
            }
        }


        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    var rowData = new System.Collections.Generic.Dictionary<string, string>();
                    foreach (DataColumn col in drv.Row.Table.Columns)
                    {
                        rowData[col.ColumnName] = drv[col.ColumnName]?.ToString() ?? "";
                    }
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer()
                                      .Serialize(rowData);
                    e.Row.Attributes["data-rowjson"] = json;
                }
            }
        }

        private static string FormatLabel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;
            // Fix: insert space before capitals that follow a lowercase letter
            // prevents leading space on first capital
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
            PRAdvancesRequest logic = new PRAdvancesRequest();
            DataTable dt = logic.getEmployeeAdvances();

            var columns = dt.Columns
                .Cast<DataColumn>()
                .Select(col => new
                {
                    Field = col.ColumnName,
                    Label = FormatLabel(col.ColumnName),
                    Visible = true
                })
                .ToList();

            return new JavaScriptSerializer().Serialize(columns);
        }
    }
}