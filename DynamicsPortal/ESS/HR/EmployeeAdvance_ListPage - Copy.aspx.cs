using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal
{
    public partial class EmployeeAdvance_ListPage : System.Web.UI.Page
    {
        PREmployeeAdvances PREmployeeAdvances = new PREmployeeAdvances();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    
                    this.BindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex.Message);
            }
            finally
            { }
        }
        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }
        protected void BindGrid()
        {
            DataSet ds = PREmployeeAdvances.retrieveRecords();//.pREmpoyeeLeaveRequest();
            DataTable dt = ds.Tables[0];
            gridView.DataSource = dt;
            gridView.DataBind();
            Session["dt"] = dt;
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    string[] fieldName = new string[3];
                    fieldName[0] = "AdvanceAmount";
                    fieldName[1] = "AdvanceDescription";
                    fieldName[2] = "RequestedPaymentDate";
                   
                    string[] fieldValue = new string[3];
                    fieldValue[0] = ((TextBox)row.FindControl("AdvanceAmount")).Text;
                    fieldValue[1] = ((TextBox)row.FindControl("AdvanceDescription")).Text;
                    fieldValue[2] = ((TextBox)row.FindControl("RequestedPaymentDate")).Text;

                    Int64 recId = Convert.ToInt64(((Label)row.FindControl("RecId")).Text);

                    bool updateRecord = PREmployeeAdvances.updateRecords("PREmployeeAdvanceRequests", fieldName, fieldValue, recId);
                    gridView.EditIndex = -1;
                    this.BindGrid();

                    //
                }
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            this.BindGrid();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow gvRow in gridView.Rows)
            {
                CheckBox chck = (CheckBox)gvRow.FindControl("chk_SelectSingle");
                if (chck.Checked)
                {
                    Int64 recId = Convert.ToInt64((gvRow.FindControl("RecId")as Label).Text);
                    bool deleteRecord = PREmployeeAdvances.deleteRecords("PREmployeeAdvanceRequests", recId);
                    gridView.EditIndex = -1;
                    this.BindGrid();
                }
            }
            
            //this.BindGrid();
           
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                }
            }
        }
    }
}