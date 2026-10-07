<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TASEmployeeAdjustments_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASEmployeeAdjustments_ListPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    
    <div class="action-items">
        <a href="/ESS/TA/TASEmployeeAdjustmentLines_ListPage.aspx" class="btn-link">
             <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </a>
    </div>
    <style>
    .form-control[readonly] {
        background-color: #f3f2f1;
        border-color: #c8c6c4;
        color: #323130;
    }

    .control-label {
        font-size: 12px;
        font-weight: 600;
        color: #605e5c;
        margin-bottom: 4px;
    }
</style>
<style>
    /* Make all header fields uniform in size */
    .header-field {
        width: 100%;          /* Fill the parent column */
        max-width: 180px;     /* Optional: limit maximum width */
        box-sizing: border-box;
    }

    /* Optional: spacing adjustments for card header */
    .PR-card-header {
        padding: 5px;
    }
</style>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <div class="container-fluid">

        <!-- ================================================= -->
        <!-- HEADER PANEL -->
        <!-- ================================================= -->
        <a href="#headerPanel"
           class="d365-toggle-header d-flex justify-content-between align-items-center"
           data-toggle="collapse"
           role="button"
           aria-expanded="true"
           aria-controls="headerPanel">

            <span class="section-title">Header</span>
            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
        </a>

<div class="collapse show mt-4" id="headerPanel">
    <div class="card shadow-sm">
        <div class="PR-card-header bg-light">
            <div class="card-body">
                <div class="row g-3">

                    <!-- Employee -->
                    <div class="col-md-2 col-sm-6">
                        <label class="form-label">Employee</label>
                        <asp:TextBox ID="txtEmployee" runat="server"
                            CssClass="form-control header-field"
                            ReadOnly="true" />
                    </div>

                    <!-- Employee Id -->
                    <div class="col-md-2 col-sm-6">
                        <label class="form-label">Employee Id</label>
                        <asp:TextBox ID="txtEmployeeId" runat="server"
                            CssClass="form-control header-field"
                            ReadOnly="true" />
                    </div>

                    <!-- Date -->
                    <div class="col-md-2 col-sm-6">
                        <label class="form-label">Date</label>
                        <asp:TextBox ID="txtDate" runat="server"
                            CssClass="form-control header-field"
                            ReadOnly="true" />
                    </div>

                    <!-- Shift Id -->
                    <div class="col-md-2 col-sm-6">
                        <label class="form-label">Shift Id</label>
                        <asp:TextBox ID="txtShiftId" runat="server"
                            CssClass="form-control header-field"
                            ReadOnly="true" />
                    </div>

                    <!-- Right side stacked fields -->
                    <div class="col-md-4 col-sm-12">
                        <div class="mb-3">
                            <label class="form-label">Generation Type</label>
                            <asp:TextBox ID="txtGenerationType" runat="server"
                                CssClass="form-control header-field"
                                ReadOnly="true" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Attendance Status</label>
                            <asp:TextBox ID="txtAttendanceStatus" runat="server"
                                CssClass="form-control header-field"
                                ReadOnly="true" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Workflow Status</label>
                            <asp:TextBox ID="txtWorkflowStatus" runat="server"
                                CssClass="form-control header-field"
                                ReadOnly="true" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</div>

        <!-- ================================================= -->
        <!-- LINES PANEL -->
        <!-- ================================================= -->
        <a href="#linePanel"
           class="d365-toggle-header d-flex justify-content-between align-items-center mt-5"
           data-toggle="collapse"
           role="button"
           aria-expanded="true"
           aria-controls="linePanel">

            <span class="section-title">Lines Detail</span>
            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
        </a>

        <div class="collapse show mt-4" id="linePanel">
            <div class="card shadow-sm">
                <div class="PR-card-header bg-light">
                    <div class="card-body">

                        <asp:GridView ID="gvLines"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="table table-bordered table-striped"
                             ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found.">

                            <Columns>
                                <asp:BoundField DataField="DateTimeIn" HeaderText="Clock-In Date Time" />
                               <asp:BoundField 
    DataField="ClockInDate" 
    HeaderText="Clock-In Date" 
     />

                             <asp:TemplateField HeaderText="Clock-In Time">
    <ItemTemplate>
        <%# FormatSecondsToTime(Eval("TimeIn")) %>
    </ItemTemplate>
</asp:TemplateField>




                                <asp:BoundField DataField="DateTimeOut" HeaderText="Clock-Out Date Time" />
                              <asp:BoundField 
    DataField="ClockOutDate"
    HeaderText="Clock-Out Date"
    />


                              
                              
                               <asp:TemplateField HeaderText="Clock-Out Time">
    <ItemTemplate>
        <%# FormatSecondsToTime(Eval("TimeOut")) %>
    </ItemTemplate>
</asp:TemplateField>


                              <asp:TemplateField HeaderText="Actual Working Hours">
    <ItemTemplate>
        <%# FormatSecondsToTime(Eval("TotalWorkingHours")) %>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Calculated Working Hours">
    <ItemTemplate>
        <%# FormatSecondsToTime(Eval("CalculatedWorkingHours")) %>
    </ItemTemplate>
</asp:TemplateField>

                                <asp:BoundField DataField="GenerationType" HeaderText="Generation Type" />
                                <asp:BoundField DataField="AttendanceRegisterStatus" HeaderText="Attendance Status" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                            </Columns>

                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>