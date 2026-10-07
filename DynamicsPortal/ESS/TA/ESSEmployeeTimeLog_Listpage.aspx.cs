using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.TA
{
    public partial class ESSEmployeeTimeLog_Listpage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSEmployeeTimeLog_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;
                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Employee Time Log";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Employee Time Log";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    // Default dates
                    txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                    // Optional: Show empty grid with headers on first load
                    BindEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            TASEmployeeTimeLog svc = new TASEmployeeTimeLog();

            try
            {
                if (string.IsNullOrWhiteSpace(txtFromDate.Text) || string.IsNullOrWhiteSpace(txtToDate.Text))
                {
                    BindEmptyGrid();
                    return;
                }

                DateTime fromDate, toDate;

                if (!DateTime.TryParse(txtFromDate.Text, out fromDate) ||
                    !DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    BindEmptyGrid();
                    return;
                }

                if (fromDate > toDate)
                {
                    BindEmptyGrid();
                    return;
                }

                DataTable dt = svc.getTimeLogHistoryByDate(fromDate, toDate);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridView.DataSource = dt;
                    gridView.DataBind();
                }
                else
                {
                    BindEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                BindEmptyGrid();
            }
        }

        /// <summary>
        /// Binds an empty DataTable so that headers are always visible
        /// </summary>
        private void BindEmptyGrid()
        {
            DataTable emptyTable = new DataTable();
            emptyTable.Columns.Add("EmployeeId");
            emptyTable.Columns.Add("EmployeeName");
            emptyTable.Columns.Add("PunchDate");
            emptyTable.Columns.Add("InOutType");
            emptyTable.Columns.Add("GenerationType");
            emptyTable.Columns.Add("DATAAREAID");
            emptyTable.Columns.Add("PARTITION");
            emptyTable.Columns.Add("RECID");
            emptyTable.Columns.Add("RecVersion");
            emptyTable.Columns.Add("RecId");

            gridView.DataSource = emptyTable;
            gridView.DataBind();
        }

        protected string FormatDate(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            DateTime dt;
            if (DateTime.TryParse(value.ToString(), out dt))
            {
                return dt.ToString("MM/dd/yyyy");
            }
            return value.ToString();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // You can add custom logic here later if needed
        }
    }
}