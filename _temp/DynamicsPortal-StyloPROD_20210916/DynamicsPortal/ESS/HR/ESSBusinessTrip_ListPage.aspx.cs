using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSBusinessTripSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSBusinessTrip_ListPage : MainForm
    {
        private HRBusinessTrip hRBusinessTrip = new HRBusinessTrip();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();
        private LogisticsAddressCity logisticsAddressCity = new LogisticsAddressCity();
        private LogisticsAddressCountryRegion logisticsAddressCountryRegion = new LogisticsAddressCountryRegion();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hRBusinessTrip.tableName;
                pageMenuId = "ESSHRBusinessTripHistory";

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
        private void getGridDataTable()
        {
            DataTable dt = hRBusinessTrip.retriveEmployeeReportees();
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

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = hRBusinessTrip.createDataTable();

                    DataRow dr = dataTable.NewRow();
                    //dr["RequiredVisa"] = (gridViewRow.FindControl("ddlRequiredVisa") as DropDownList).SelectedValue;
                    //dr["BusinessTripType"] = (gridViewRow.FindControl("ddlBusinessTripType") as DropDownList).SelectedValue;
                    //dr["ExitReentryRequired"] = (gridViewRow.FindControl("ddlExitReentryRequired") as DropDownList).SelectedValue;
                    dr["FlightBookingRequired"] = (gridViewRow.FindControl("ddlFlightBookingRequired") as DropDownList).SelectedValue;
                    dr["HotelBookingRequired"] = (gridViewRow.FindControl("ddlHotelBookingRequired") as DropDownList).SelectedValue;
                    dr["CarRentalBooking"] = (gridViewRow.FindControl("ddlCarRentalBooking") as DropDownList).SelectedValue;

                    dr["Reason"] = (gridViewRow.FindControl("txtReason") as TextBox).Text;
                    dr["ReturnDate"] = (gridViewRow.FindControl("txtReturnDate") as TextBox).Text;
                    dr["DepartureDate"] = (gridViewRow.FindControl("txtDepartureDate") as TextBox).Text;


                    dr["TicketRouting"] = (gridViewRow.FindControl("ddlTicketRouting") as DropDownList).SelectedValue;
                    dr["Meal"] = (gridViewRow.FindControl("ddlMeal") as DropDownList).SelectedValue;
                    dr["NightStay"] = (gridViewRow.FindControl("ddlNightStay") as DropDownList).SelectedValue;
                    dr["PersonalCar"] = (gridViewRow.FindControl("ddlPersonalCar") as DropDownList).SelectedValue;
                    dr["FuelAndToll"] = (gridViewRow.FindControl("ddlFuelAndToll") as DropDownList).SelectedValue;
                    dr["BusinesClass"] = (gridViewRow.FindControl("ddlBusinesClass") as DropDownList).SelectedValue;
                    dr["DepartureCountry"] = (gridViewRow.FindControl("ddlDepartureCountry") as DropDownList).SelectedValue;
                    dr["DestinationCountry"] = (gridViewRow.FindControl("ddlDestinationCountry") as DropDownList).SelectedValue;

                    dr["DepartureCity"] = (gridViewRow.FindControl("ddlDepartureCity") as DropDownList).SelectedValue;
                    dr["DestinationCity"] = (gridViewRow.FindControl("ddlDestinationCity") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = hRBusinessTrip.update(dataTable, recId);
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
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            bindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = hRBusinessTrip.delete(recordsId.ToArray());
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
            }

        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                //DropDownList ddlBusinessTripType = gridViewRow.FindControl("ddlBusinessTripType") as DropDownList;
                //ddlBusinessTripType.DataSource = Enum.GetNames(typeof(ESSBusinessTripType));
                //ddlBusinessTripType.DataBind();

                //DropDownList ddlVisaRequired = gridViewRow.FindControl("ddlRequiredVisa") as DropDownList;
                //ddlVisaRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
                //ddlVisaRequired.DataBind();

                //DropDownList ddlExitReentryRequired = gridViewRow.FindControl("ddlExitReentryRequired") as DropDownList;
                //ddlExitReentryRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
                //ddlExitReentryRequired.DataBind();

                DropDownList ddlFlightBookingRequired = gridViewRow.FindControl("ddlFlightBookingRequired") as DropDownList;
                ddlFlightBookingRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlFlightBookingRequired.DataBind();

                DropDownList ddlHotelBookingRequired = gridViewRow.FindControl("ddlHotelBookingRequired") as DropDownList;
                ddlHotelBookingRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlHotelBookingRequired.DataBind();

                DropDownList ddlCarRentalBooking = gridViewRow.FindControl("ddlCarRentalBooking") as DropDownList;
                ddlCarRentalBooking.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlCarRentalBooking.DataBind();

                DropDownList ddlTicketRouting = gridViewRow.FindControl("ddlTicketRouting") as DropDownList;
                ddlTicketRouting.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlTicketRouting.DataBind();


                DropDownList ddlMeal = gridViewRow.FindControl("ddlMeal") as DropDownList;
                ddlMeal.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlMeal.DataBind();


                DropDownList ddlNightStay = gridViewRow.FindControl("ddlNightStay") as DropDownList;
                ddlNightStay.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlNightStay.DataBind();


                DropDownList ddlPersonalCar = gridViewRow.FindControl("ddlPersonalCar") as DropDownList;
                ddlPersonalCar.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlPersonalCar.DataBind();


                DropDownList ddlFuelAndToll = gridViewRow.FindControl("ddlFuelAndToll") as DropDownList;
                ddlFuelAndToll.DataSource = Enum.GetNames(typeof(ESSNoYes));
                ddlFuelAndToll.DataBind();


                DropDownList ddlBusinesClass = gridViewRow.FindControl("ddlBusinesClass") as DropDownList;
                ddlBusinesClass.DataSource = Enum.GetNames(typeof(ESSBusinesClass));
                ddlBusinesClass.DataBind();


                DataTable dtLogisticsAddressCountryRegion = logisticsAddressCountryRegion.retrieveAll();
                DropDownList ddlDepartureCountry = gridViewRow.FindControl("ddlDepartureCountry") as DropDownList;
                ddlDepartureCountry.DataSource = dtLogisticsAddressCountryRegion;
                ddlDepartureCountry.DataTextField = "Name";
                ddlDepartureCountry.DataValueField = "CountryRegionId";
                ddlDepartureCountry.DataBind();
                ddlDepartureCountry.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlDepartureCountry.SelectedIndex = 0;

                DropDownList ddlDestinationCountry = gridViewRow.FindControl("ddlDestinationCountry") as DropDownList;
                ddlDestinationCountry.DataSource = dtLogisticsAddressCountryRegion;
                ddlDestinationCountry.DataTextField = "Name";
                ddlDestinationCountry.DataValueField = "CountryRegionId";
                ddlDestinationCountry.DataBind();
                ddlDestinationCountry.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlDestinationCountry.SelectedIndex = 0;


                //ddlBusinessTripType.SelectedValue = dataRowView["BusinessTripType"].ToString();
                //ddlVisaRequired.SelectedValue = dataRowView["RequiredVisa"].ToString();
                //ddlExitReentryRequired.SelectedValue = dataRowView["ExitReentryRequired"].ToString();
                ddlFlightBookingRequired.SelectedValue = dataRowView["FlightBookingRequired"].ToString();
                ddlHotelBookingRequired.SelectedValue = dataRowView["HotelBookingRequired"].ToString();
                ddlCarRentalBooking.SelectedValue = dataRowView["CarRentalBooking"].ToString();

                ddlTicketRouting.SelectedValue = dataRowView["TicketRouting"].ToString();
                ddlMeal.SelectedValue = dataRowView["Meal"].ToString();
                ddlNightStay.SelectedValue = dataRowView["NightStay"].ToString();
                ddlPersonalCar.SelectedValue = dataRowView["PersonalCar"].ToString();
                ddlFuelAndToll.SelectedValue = dataRowView["FuelAndToll"].ToString();
                ddlBusinesClass.SelectedValue = dataRowView["BusinesClass"].ToString();
                ddlDepartureCountry.SelectedValue = dataRowView["DepartureCountry"].ToString();
                ddlDestinationCountry.SelectedValue = dataRowView["DestinationCountry"].ToString();


                DropDownList ddlDepartureCity = gridViewRow.FindControl("ddlDepartureCity") as DropDownList;
                bind_ddlCity(ddlDepartureCountry, ddlDepartureCity);

                DropDownList ddlDestinationCity = gridViewRow.FindControl("ddlDestinationCity") as DropDownList;
                bind_ddlCity(ddlDestinationCountry, ddlDestinationCity);

                ddlDepartureCity.SelectedValue = dataRowView["DepartureCity"].ToString();
                ddlDestinationCity.SelectedValue = dataRowView["DestinationCity"].ToString();
            }

            string[] validStatus = new string[] { ESSWorkFlowStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWorkflowState", validStatus);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblBusinessTripId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hRBusinessTripRequest_Submit(recordsId.ToArray());
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
            }

        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        private void bind_ddlCity(DropDownList _ddlCountry, DropDownList _ddlCity)
        {
            DropDownList ddlCountry = _ddlCountry;
            DropDownList ddlCity = _ddlCity;
            DataTable dtLogisticsAddressCity = logisticsAddressCity.createDataTable();

            if (!string.IsNullOrEmpty(ddlCountry.SelectedValue))
            {
                //DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
                dtLogisticsAddressCity = logisticsAddressCity.retrieveByCountry(ddlCountry.SelectedValue);
            }
            ddlCity.DataSource = dtLogisticsAddressCity;
            ddlCity.DataTextField = "Name";
            ddlCity.DataValueField = "Name";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            //ddlCity.SelectedIndex = 0;
            //ddlDepartureCity.SelectedValue = dataRowView["DepartureCity"].ToString();

        }

        protected void ddlDepartureCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDepartureCountry = (sender as DropDownList);
            GridViewRow gridViewRow = ddlDepartureCountry.NamingContainer as GridViewRow;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlDepartureCity = gridViewRow.FindControl("ddlDepartureCity") as DropDownList;
                bind_ddlCity(ddlDepartureCountry, ddlDepartureCity);
            }

        }

        protected void ddlDestinationCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDestinationCountry = (sender as DropDownList);
            GridViewRow gridViewRow = ddlDestinationCountry.NamingContainer as GridViewRow;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            DataTable dtLogisticsAddressCity = logisticsAddressCity.createDataTable();
            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlDestinationCity = gridViewRow.FindControl("ddlDestinationCity") as DropDownList;
                bind_ddlCity(ddlDestinationCountry, ddlDestinationCity);
            }

        }
    }
}