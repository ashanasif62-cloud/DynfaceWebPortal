<%@ Page Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TransferJournal_Create.aspx.cs" Inherits="DynamicsPortal.TransferJournal_Create" %>

<%@ Register Src="~/DropDownList_Warehouse.ascx" TagPrefix="uc1" TagName="DropDownList_Warehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
          <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
  <ContentTemplate>
    <!-- Overview Panel -->
    <a href="#OverviewPanel"
        class="d365-toggle-header d-flex justify-content-between align-items-center"
        data-toggle="collapse" role="button" aria-expanded="true" aria-controls="OverviewPanel">
        <span class="section-title">Overview</span>
        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
    </a>

    <div class="collapse show mt-3" id="OverviewPanel">
        <div class="card-body">
            <div class="PR-custom-section-title mb-3"></div>

            <!-- IDENTIFICATION Pane -->
            <div class="mb-2">
                <strong class="d-block mb-2">IDENTIFICATION</strong>
            </div>

            <div class="d-flex flex-wrap justify-content-between">

                <!-- Column 1 -->
                <div style="flex: 0 0 32%;">
                    <!-- Name -->
                    <div class="form-group" style="width: 250px;">
                        <label>Name</label>
                        <asp:DropDownList ID="ddlName" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlName_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                    </div>
                </div>

                <!-- Column 2 -->
                <div style="flex: 0 0 32%;">
                    <!-- Journal -->
                    <div class="form-group mb-3" style="width: 250px;">
                        <label>Journal</label>
                        <asp:Label ID="lblJournal" runat="server" CssClass="form-control autocomplete-input textbox-style" />
                    </div>

                    <!-- Description -->
                    <div class="form-group mb-3" style="width: 250px;">
                        <label>Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control autocomplete-input textbox-style" />
                    </div>
                </div>

                <!-- Column 3 -->
                <div style="flex: 0 0 32%;">
                    <!-- Store inventory Pane -->
                    <div class="mb-2">
                        <strong class="d-block mb-2">STORE INVENTORY</strong>
                    </div>

                    <!-- Site -->
                    <div class="form-group" style="width: 250px;">
                        <label>Site</label>
                        <asp:DropDownList ID="ddlSite" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                    </div>

                    <!-- Warehouse -->
                    <div class="form-group" style="width: 250px;">
                        <label>Warehouse</label>
                        <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                        <%--<uc1:DropDownList_Warehouse ID="ddlWarehouseList" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" />--%>
                    </div>
                </div>

            </div>
        </div>
    </div>



    <%-- General Panel --%>
    <a href="#GeneralPanel" class="d365-toggle-header d-flex justify-content-between align-items-center"
        data-toggle="collapse" role="button" aria-expanded="true" aria-controls="GeneralPanel">
        <span class="section-title">General</span>
        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
    </a>

    <div class="collapse show mt-3" id="GeneralPanel">
        <div class="card-body">
            <div class="PR-custom-section-title mb-3"></div>

            <!-- Voucher Pane -->
            <div class="mb-2">
                <strong class="d-block mb-2">VOUCHER</strong>
            </div>
            <div class="d-flex flex-wrap justify-content-between">

                <!-- Column 1 -->
                <div style="flex: 0 0 32%;">
                    <!-- Voucher series -->
                    <div class="form-group" style="width: 250px;">
                        <label>Voucher series</label>
                        <asp:DropDownList ID="ddlVoucherSeries" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                    </div>
                </div>

                <!-- Column 2 -->
                <div style="flex: 0 0 32%;">
                    <!-- Selection by -->
                    <div class="form-group" style="width: 250px;">
                        <label>Selection by</label>
                        <asp:DropDownList ID="ddlSelectionBy" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                    </div>

                    <!-- New voucher by -->
                    <div class="form-group" style="width: 250px;">
                        <label>New voucher by</label>
                        <asp:DropDownList ID="ddlNewVoucherBy" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                    </div>
                </div>


                <!-- Column 3 -->
                <div style="flex: 0 0 32%;">
                    <!-- Posting Pane -->
                    <div class="mb-2">
                        <strong class="d-block mb-2">POSTING</strong>
                    </div>

                    <!-- Detail level -->
                    <div class="form-group" style="width: 250px;">
                        <label>Detail level</label>
                        <asp:DropDownList ID="ddlDetailLevel" runat="server" CssClass="form-select" AutoPostBack="false" Width="100%"></asp:DropDownList>
                    </div>

                    <!-- Delete lines after posting -->
                    <div class="form-group">
                        <!-- Label -->
                        <label class="mb-2 d-block">Delete lines after posting</label>
                        <!-- Checkbox -->
                        <label class="toggle-switch mb-2 d-block">
                            <input type="checkbox" id="DeleteLinesAfterPostingBox" runat="server" disabled="disabled" />
                            <span class="slider"></span>
                        </label>
                        <!-- Text span -->
                        <span id="DeleteLinesAfterPosting" runat="server"></span>
                    </div>

                    <!-- Offset account -->
                    <div class="form-group mb-3" style="width: 250px;">
                        <label>Offset account</label>
                        <asp:Label ID="lblOffsetAccount" runat="server" CssClass="form-control autocomplete-input textbox-style" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Setup Panel --%>
    <a href="#SetupPanel" class="d365-toggle-header d-flex justify-content-between align-items-center"
        data-toggle="collapse" role="button" aria-expanded="true" aria-controls="SetupPanel">
        <span class="section-title">Setup</span>
        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
    </a>

    <div class="collapse show mt-3" id="SetupPanel">
        <div class="card-body">
            <div class="PR-custom-section-title mb-3"></div>

            <!-- Inventory Pane -->
            <div class="mb-2">
                <strong class="d-block mb-2">INVENTORY</strong>
            </div>

            <!-- Reservation -->
            <div class="form-group" style="width: 250px;">
                <label class="d-block mb-1">Reservation</label>
                <asp:DropDownList ID="ddlReservation" runat="server" CssClass="form-select d-block" Width="120px"></asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="action-footer">
        <asp:LinkButton ID="btnCreate" runat="server" OnClick="btnCreate_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
             </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
