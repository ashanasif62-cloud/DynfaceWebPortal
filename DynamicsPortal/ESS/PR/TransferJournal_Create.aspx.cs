using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Markup;
using System.Xml.Linq;
using System.ComponentModel;

namespace DynamicsPortal
{
    public partial class TransferJournal_Create : ModalForm
    {
        private TransferJournalHeader transferJournalHeader = new TransferJournalHeader();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    pageMenuId = "TransferJournal_Create";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Create inventory journal";
                        titleDiv.Style["font-weight"] = "600";  
                        titleDiv.Style["font-size"] = "20px";    
                        titleDiv.Style["color"] = "#000000";    
                        titleDiv.Style["margin"] = "10px 0";
                    }
                    bindJournalNameId();
                }
                bindNumSeq();
                bindSiteId();
                bindWarehouseId();
                bindVoucherSeries();
                bindVoucherDraw();
                bindNewVoucherBy();
                bindDetailSummary();
                bindItemReservation();
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
        protected void bindJournalNameId()
        {
                if (ddlName != null)
                {
                    DataTable dt = transferJournalHeader.retrieveJournalNameId();

                    ddlName.DataSource = dt;
                    ddlName.DataTextField  = "JournalName";    // text to show
                    ddlName.DataValueField = "JournalName";   // value to pass
                    ddlName.DataBind();


                    ddlName.Items.Insert(0, new ListItem("", string.Empty));

                    ddlName.CssClass += " filterable-dropdown";

                }
        }
        protected void bindNumSeq()
        {
            if (ddlName != null)
            {
                DataTable dt = transferJournalHeader.retrieveNumberSequence();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lblJournal.Text = dr["JournalId"].ToString();
                        break;
                    }
                }

            }
        }
        protected void bindSiteId()
        {
            DataTable dt = transferJournalHeader.retrieveSiteId();

            dt.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["DisplayText"] = row["InventSiteId"];// + " - " + row["WarehouseName"];
            }

            ddlSite.DataSource = dt;
            ddlSite.DataValueField = "InventSiteId";   // value to pass
            ddlSite.DataTextField = "DisplayText";    // text to show
            ddlSite.DataBind();

            // Add a default item to show it's empty
            ddlSite.Items.Insert(0, new ListItem("", string.Empty));

            ddlSite.CssClass += " filterable-dropdown";
        }
        protected void bindWarehouseId()
        {
            DataTable dt = transferJournalHeader.retrieveWarehouseId();

            dt.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["DisplayText"] = row["InventLocationId"];// + " - " + row["WarehouseName"];
            }

            ddlWarehouse.DataSource = dt;
            ddlWarehouse.DataValueField = "InventLocationId";   // value to pass
            ddlWarehouse.DataTextField = "DisplayText";    // text to show
            ddlWarehouse.DataBind();

            // Add a default item to show it's empty
            ddlWarehouse.Items.Insert(0, new ListItem("", string.Empty));

            ddlWarehouse.CssClass += " filterable-dropdown";
        }
        protected void bindVoucherSeries()
        {
            DataTable dt = transferJournalHeader.retrieveVoucherSeries();

            dt.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["DisplayText"] = row["NumberSequence"];
            }

            ddlVoucherSeries.DataSource = dt;
            ddlVoucherSeries.DataTextField = "DisplayText";    // text to show
            ddlVoucherSeries.DataValueField = "NumberSequence";   // value to pass
            ddlVoucherSeries.DataBind();

            // Add a default item to show it's empty
            ddlVoucherSeries.Items.Insert(0, new ListItem("", string.Empty));

            ddlVoucherSeries.CssClass += " filterable-dropdown";
        }
        
        protected void bindVoucherDraw()
        {
            ddlSelectionBy.DataSource = Enum.GetValues(typeof(JournalVoucherDraw)).Cast<JournalVoucherDraw>().Select(x => new { Id = (int)x, Name = x.ToString() }).ToList();

            ddlSelectionBy.DataTextField = "Name";
            ddlSelectionBy.DataValueField = "Name";
            ddlSelectionBy.DataBind();

            ddlSelectionBy.Items.Insert(0, new ListItem("", "")); // empty default
        }
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr == null ? value.ToString() : attr.Description;
        }

        protected void bindNewVoucherBy()
        {
            ddlNewVoucherBy.DataSource = Enum.GetValues(typeof(InventJournalVoucherChange)).Cast<InventJournalVoucherChange>().Select(x => new { Id = (int)x, Name = GetEnumDescription(x) }).ToList();

            ddlNewVoucherBy.DataTextField = "Name";   // What the user sees
            ddlNewVoucherBy.DataValueField = "Id";    // Actual enum numeric value
            ddlNewVoucherBy.DataBind();

            ddlNewVoucherBy.Items.Insert(0, new ListItem("", ""));  // optional blank first item
        }
        protected void bindDetailSummary()
        {
            ddlDetailLevel.DataSource = Enum.GetValues(typeof(DetailSummary)).Cast<DetailSummary>().Select(x => new { Id = (int)x, Name = x.ToString() }).ToList();

            ddlDetailLevel.DataTextField = "Name";
            ddlDetailLevel.DataValueField = "Name";
            ddlDetailLevel.DataBind();

            ddlDetailLevel.Items.Insert(0, new ListItem("", "")); // empty default
        }
        
        protected void bindItemReservation()
        {
            ddlReservation.DataSource = Enum.GetValues(typeof(ItemReservation)).Cast<ItemReservation>().Select(x => new { Id = (int)x, Name = x.ToString() }).ToList();

            ddlReservation.DataTextField = "Name";
            ddlReservation.DataValueField = "Name";
            ddlReservation.DataBind();

            ddlReservation.Items.Insert(0, new ListItem("", "")); // empty default
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            long requestRecId = 0;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();

            #region CreateRequest
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Journal");
            dataTable.Columns.Add("JournalNameId");
            dataTable.Columns.Add("JournalDescription");
            dataTable.Columns.Add("SiteId");
            dataTable.Columns.Add("WarehouseId");
            dataTable.Columns.Add("VoucherSeries");
            dataTable.Columns.Add("VoucherDraw");
            dataTable.Columns.Add("VoucherChange");
            dataTable.Columns.Add("DetailSummary");
            dataTable.Columns.Add("DeletePostedLines");
            dataTable.Columns.Add("Reservation");
            DataRow dr = dataTable.NewRow();

            string Journal         = lblJournal.Text;
            string journalNameId   = ddlName.SelectedValue;
            string journalDescription = txtDescription.Text;
            string SiteId          = ddlSite.SelectedValue;
            string WarehouseId     = ddlWarehouse.SelectedValue;
            string VoucherSeries   = ddlVoucherSeries.Text;
            string VoucherDraw     = ddlSelectionBy.Text;
            string VoucherChange   = ddlNewVoucherBy.Text;
            string DetailSummary   = ddlDetailLevel.Text;
            //string DeletePostedLines= DeleteLinesAfterPosting.SelectedValue;
            string Reservation     = ddlReservation.Text;
            //string WarehouseId     = (ddlWarehouseList.FindControl("txtWarehouseId") as System.Web.UI.WebControls.TextBox).Text;

            dr["Journal"]          = Journal;
            dr["JournalNameId"]    = journalNameId;
            dr["JournalDescription"] = journalDescription;
            dr["SiteId"]           = SiteId;
            dr["WarehouseId"]      = WarehouseId;
            dr["VoucherSeries"]    = VoucherSeries;
            dr["VoucherDraw"]      = VoucherDraw;
            dr["VoucherChange"]    = VoucherChange;
            dr["DetailSummary"]    = DetailSummary;
           // dr["DeletePostedLines"]= DeletePostedLines;
            dr["Reservation"]      = Reservation;

            dataTable.Rows.Add(dr);
            #endregion

            SysOperationResult_BOL result = transferJournalHeader.create(dataTable);
            requestRecId = result.RecId;

            if (result != null && result.isSuccess)
            {
                NotificationMessage.showMessage(result);

                Match match = Regex.Match(result.Message, @"\d+");
                if (match.Success)
                {
                    string transferNumber = match.Value;
                    Session["Status"] = "Created";
                }

                // Redirect after delay
                string script = @"
            setTimeout(function() { 
                window.top.location = '/ESS/PR/TransferJournal_ListPage.aspx'; 
            }, 3000);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
            }
            else
            {
                NotificationMessage.showMessage(result);
            }
        }
        protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string journalNameId = ddlName.SelectedValue;

            if (string.IsNullOrWhiteSpace(journalNameId))
                return;

            DataTable dt = transferJournalHeader.retrieveJournalNameDetails(journalNameId);

            if (dt.Rows.Count > 0)
            {
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
                ddlVoucherSeries.SelectedValue = dt.Rows[0]["NumberSequence"].ToString();

                // Convert DeleteLinesAfterPosting string/int to match Enum dropdown value
                string deleteLinesValue = dt.Rows[0]["DeletePostedLines"].ToString().Trim();

                // ✅ Set the label text
                DeleteLinesAfterPosting.InnerText = deleteLinesValue;

                // ✅ Set the checkbox checked state
                if (deleteLinesValue.Equals("Yes", StringComparison.OrdinalIgnoreCase) || deleteLinesValue.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteLinesAfterPostingBox.Checked = true;
                }
                else
                {
                    DeleteLinesAfterPostingBox.Checked = false;
                }
                DeleteLinesAfterPostingBox.Disabled = false;
                // Convert VoucherDraw string/int to match Enum dropdown value
                string voucherDrawValue = dt.Rows[0]["VoucherDraw"].ToString();
                JournalVoucherDraw parsedValue;

                if (Enum.TryParse<JournalVoucherDraw>(voucherDrawValue, true, out parsedValue))
                {
                    ddlSelectionBy.SelectedValue = parsedValue.ToString();
                }
                else
                {
                    ddlSelectionBy.SelectedIndex = 0; // fallback if not found
                }

                // Convert NewVoucherBy string/int to match Enum dropdown value
                string dbValue = dt.Rows[0]["VoucherChange"].ToString().Trim();

                // Try to match either enum name, description, or numeric value
                foreach (InventJournalVoucherChange enumValue in Enum.GetValues(typeof(InventJournalVoucherChange)))
                {
                    string enumName = enumValue.ToString();
                    string enumDescription = GetEnumDescription(enumValue);
                    string enumIntValue = ((int)enumValue).ToString();

                    if (dbValue.Equals(enumName, StringComparison.OrdinalIgnoreCase)
                        || dbValue.Equals(enumDescription, StringComparison.OrdinalIgnoreCase)
                        || dbValue.Equals(enumIntValue, StringComparison.OrdinalIgnoreCase))
                    {
                        ddlNewVoucherBy.SelectedValue = ((int)enumValue).ToString();
                        break;
                    }
                }

                //Convert DetailSummary string/ int to match Enum dropdown value
                string DetailSummary = dt.Rows[0]["DetailSummary"].ToString();
                DetailSummary parsedDetailSummary;

                if (Enum.TryParse<DetailSummary>(DetailSummary, true, out parsedDetailSummary))
                {
                    ddlDetailLevel.SelectedValue = parsedDetailSummary.ToString();
                }
                else
                {
                    ddlDetailLevel.SelectedIndex = 0; // fallback if not found
                }

                // Convert ItemReservation string/int to match Enum dropdown value
                string ItemReservation = dt.Rows[0]["Reservation"].ToString();
                ItemReservation parsedItemReservation;

                if (Enum.TryParse<ItemReservation>(ItemReservation, true, out parsedItemReservation))
                {
                    ddlReservation.SelectedValue = parsedItemReservation.ToString();
                }
                else
                {
                    ddlReservation.SelectedIndex = 0; // fallback if not found
                }
            }
        }

    }
}