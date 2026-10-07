<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="ESSEmployeeTimeLog_Listpage.aspx.cs" 
    Inherits="DynamicsPortal.ESS.TA.ESSEmployeeTimeLog_Listpage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Responsive Grid Styles */
        .table-responsive-custom {
            width: 100%;
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .grid-size {
            width: 100%;
            min-width: 700px; /* Prevents columns from becoming too narrow on mobile */
            table-layout: auto;
        }

        /* Make sure header is always visible */
        .grid-size th {
            white-space: nowrap;
            background-color: #f8f9fa;
            position: sticky;
            top: 0;
            z-index: 2;
        }

        .grid-size td {
            white-space: nowrap;
        }

        /* Better empty message styling */
        .grid-size .empty-row td {
            text-align: center;
            padding: 20px;
            color: #6c757d;
            font-style: italic;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
          
            <div style="margin-bottom: 15px;">
                <span style="margin-right: 5px;">From Date:</span>
                <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" autocomplete="off" CssClass="form-control" style="display:inline-block; width:160px;"></asp:TextBox>
                
                <span style="margin-left: 10px;">To Date:</span>
                <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off" CssClass="form-control" style="display:inline-block; width:160px;"></asp:TextBox>
                
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" 
                    CssClass="btn btn-primary" Style="margin-left: 10px;" />
            </div>

            <!-- Responsive wrapper -->
            <div class="table-responsive-custom">
                <asp:GridView ID="gridView" runat="server"
                    CssClass="table table-condensed table-hover table-bordered grid-size"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found."
                    AutoGenerateColumns="false"
                    DataKeyNames="RecId"
                    OnRowDataBound="gridView_RowDataBound"
                    HeaderStyle-CssClass="thead-light"
                    Width="100%">
                    
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort" ItemStyle-Width="40px">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Id">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Punch Date">
                            <ItemTemplate>
                                <asp:Label ID="lblPunchDate" runat="server" Text='<%# FormatDate(Eval("PunchDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="In Out Type">
                            <ItemTemplate>
                                <asp:Label ID="lblInOutType" runat="server" Text='<%# Bind("InOutType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Generation Type">
                            <ItemTemplate>
                                <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GenerationType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:BoundField DataField="DATAAREAID" HeaderText="DataAreaId" Visible="false" />
                        <asp:BoundField DataField="PARTITION" HeaderText="Partition" Visible="false" />
                        <asp:BoundField DataField="RECID" HeaderText="RecId" Visible="false" />
                        <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" Visible="false" />

                        <asp:TemplateField HeaderText="RecId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        <div style="text-align:center; padding:25px; color:#6c757d;">
                            No Record Found for the selected date range.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(function () {
            showAJAXOverlay();
        });
        prm.add_endRequest(function () {
            hideAJAXOverlay();
        });
    </script>

</asp:Content>