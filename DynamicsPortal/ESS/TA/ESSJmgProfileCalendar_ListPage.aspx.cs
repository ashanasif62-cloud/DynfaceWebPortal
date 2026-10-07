using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSJmgProfileCalendar_ListPage : MainForm
    {
        private JmgProfileCalendar jmgProfileCalendar = new JmgProfileCalendar();
        private JmgProfileTable jmgProfileTable = new JmgProfileTable();
        private JmgSpecialDayTable jmgSpecialDayTable = new JmgSpecialDayTable();
        private JmgImport jmgImport = new JmgImport();
        private FlushOperationResults flushOperationResults = new FlushOperationResults();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = jmgProfileCalendar.tableName;
                pageMenuId = "ESSJmgProfileCalendarHistory";
                //txtFromDate.Text = (DateTime.Now.AddDays(-7)).ToString();
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
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

        private string getQuery_EmployeeId()
        {
            string employeeId = string.Empty;
            if (string.IsNullOrEmpty(Request.QueryString["EmpId"]))
            {
                employeeId = SessionVariables.getCurrentEmployeeId();
            }
            else
            {
                employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            }
            return employeeId;
        }

        private void getGridDataTable()
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string employeeId = getQuery_EmployeeId();
            DataTable dt = jmgProfileCalendar.createDataTable();

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime fromDateTime = fromDate.toDateTime();
                DateTime toDateTime = toDate.toDateTime();

                dt = jmgProfileCalendar.retrieveEmployeeDateRange(employeeId, fromDateTime, toDateTime);
            }
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
                string employeeId = getQuery_EmployeeId();

                if (!string.IsNullOrEmpty(dataRowView["RecId"].ToString()))
                {
                    e.Row.Cells[2].Enabled = false; //Type
                }

                DropDownList ddlProfileId = gridViewRow.FindControl("ddlProfileId") as DropDownList;
                ddlProfileId.DataSource = jmgProfileTable.retrieveByEmployee(employeeId);
                ddlProfileId.DataTextField = "Profile";
                ddlProfileId.DataValueField = "Profile";
                ddlProfileId.DataBind();
                ddlProfileId.SelectedValue = dataRowView["ProfileId"].ToString();

                DropDownList ddlSpecialDayId = gridViewRow.FindControl("ddlSpecialDayId") as DropDownList;
                ddlSpecialDayId.DataSource = jmgSpecialDayTable.retrieveAllSpecialDay();
                ddlSpecialDayId.DataTextField = "SpecialDayId";
                ddlSpecialDayId.DataValueField = "SpecialDayId";
                ddlSpecialDayId.DataBind();

                ddlSpecialDayId.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlSpecialDayId.SelectedIndex = 0;

                ddlSpecialDayId.SelectedValue = dataRowView["SpecialDayId"].ToString();
            }

        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = SessionVariables.getSessionDataTable();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dr["RelationNumber"] = getQuery_EmployeeId();
                dataTable.Rows.InsertAt(dr, 0);

                SessionVariables.setSessionDataTable(dataTable);
            }

            gridView.EditIndex = 0;
            bindGrid();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    bool result;
                    SysOperationResult_BOL operationResult_BOL;
                    DataTable dataTable = jmgProfileCalendar.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    dr["RelationNumber"] = (gridViewRow.FindControl("lblRelationNumber") as Label).Text;
                    dr["JmgDate"] = (gridViewRow.FindControl("txtJmgDate") as TextBox).Text;
                    //dr["ProfileCalendarType"] = (gridViewRow.FindControl("ddlProfileCalendarType") as DropDownList).SelectedValue;
                    dr["ProfileId"] = (gridViewRow.FindControl("ddlProfileId") as DropDownList).SelectedValue;
                    dr["SpecialDayId"] = (gridViewRow.FindControl("ddlSpecialDayId") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId == 0 && rowIndex == 0)
                    {
                        operationResult_BOL = jmgProfileCalendar.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        operationResult_BOL = jmgProfileCalendar.update(dataTable, Convert.ToInt64(recId));
                        result = operationResults(operationResult_BOL);
                    }


                    if (result)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                    }
                    else
                    {
                        bindGrid();
                    }
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    int rowIndex = gridViewRow.RowIndex;
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView.DataKeys[rowIndex].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = SessionVariables.getSessionDataTable();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                SessionVariables.setSessionDataTable(dataTable);
                            }

                        }
                    }
                }
            }
            gridView.EditIndex = -1;
            bindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            bool includeEmptyRows = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                    else
                    {
                        includeEmptyRows = true;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = jmgProfileCalendar.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid();
                }
                else
                {
                    bindGrid();
                }
                return;
            }
            else if (includeEmptyRows)
            {
                gridView.EditIndex = -1;
                reBindGrid();
            }

        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        protected void Date_TextChanged(object sender, EventArgs e)
        {
            reBindGrid();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
                string message = string.Empty;

                if (!fileUpload.HasFile)
                {
                    message = "Please select a file.";
                    NotificationMessage.showMessage(AlertType.Error, message);
                    return;
                }

                string employeeId = SessionVariables.getCurrentEmployeeId();
                string base64Str = string.Empty;
                string fileExt = "xls";
                string fileName = "import";

                HttpPostedFile uploadedFile = fileUpload.PostedFile;
                int fileSize = uploadedFile.ContentLength;
                if (fileSize < 5000000)
                {
                    string fileType = fileUpload.PostedFile.ContentType;
                    fileExt = Path.GetExtension(uploadedFile.FileName);
                    fileName = Path.GetFileName(uploadedFile.FileName);

                    Stream fileStream = fileUpload.PostedFile.InputStream;

                    // Convert to Base64
                    StreamReader reader = new StreamReader(fileStream);
                    byte[] byteData = System.Text.Encoding.Default.GetBytes(reader.ReadToEnd());
                    string fileDataBase64 = Convert.ToBase64String(byteData);

                    objBOL = jmgImport.importProfileCalendar(fileDataBase64);
                    message = objBOL.Message;
                    flushOperationResults.flushResults(message);
                    NotificationMessage.showMessage(objBOL);


                    //using (Stream fileStream = fileUpload.PostedFile.InputStream)
                    //{
                    //    using (BinaryReader br = new BinaryReader(fileStream))
                    //    {
                    //        byte[] fileBytes = br.ReadBytes((Int32)fileStream.Length);
                    //        // Convert byte[] to Base64 String
                    //        string fileData = Convert.ToBase64String(fileBytes);
                    //        objBOL = jmgImport.importProfileCalendar(fileData);
                    //        message = objBOL.Message;
                    //        flushOperationResults.flushResults(message);
                    //        NotificationMessage.showMessage(objBOL);
                    //    }
                    //}
                }
                else
                {
                    message = "File size must be less than 5mb.";
                    NotificationMessage.showMessage(AlertType.Error, message);
                    return;
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

    }
}