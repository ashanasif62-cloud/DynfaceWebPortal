<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EmployeeAdvance_ListPage.aspx.cs" Inherits="DynamicsPortal.EmployeeAdvance_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>      
        $(document).ready(function () {
            var pageTitle = $("h6[id$='pageTitle']");
            pageTitle.val("Employee Advances");
        });
    </script>
    <script type = "text/javascript">
        var popUpObj;

        function showModalPopUp() {

            //        window.showModalDialog("EmployeeAdvance_Create.aspx", 'window', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=500,height=400,left=400,top=200');
            popUpObj = window.open("EmployeeAdvance_Create.aspx", "ModalPopUp", "modal=yes,draggable=no,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=500,height=400,left=400,top=200");

            popUpObj.focus();
            LoadModalDiv();

        }
        function LoadModalDiv() {

            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "block";

        }
        function HideModalDiv() {

            var bcgDiv = document.getElementById("divBackground");

            bcgDiv.style.display = "none";

        }

</script>

</asp:Content>

<asp:Content ID="breadCrumb" ContentPlaceHolderID="breadcrumb" runat="server">
    <%--    <h3 class="text-themecolor">Home</h3>
    <ol id="breadcrumb01" runat="server" class="breadcrumb">
        <li class="breadcrumb-item active">Home</li>
    </ol>--%>
    <div class="breadcrumb-item active"><a href="index.html">Employee Advances</a></div>
    <%--<div class="breadcrumb-item active">Forms</div>--%>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript: return showModalPopUp()"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
    <%--<div class="action-items"><a onclick=""><i class="mdi mdi-delete"></i></a>Delete</div>--%>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    
    <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; 
    width: 100%; top: 0; left:0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; display:none">
    </div>
    <div>
                <asp:GridView ID="gridView" CssClass="table table-condensed no-border table-hover" runat="server" DataKeyNames="RecId" 
                OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false" >
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="checkbox" id="chk_SelectAll" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_SelectSingle" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Advance Request Id">
                        <ItemTemplate>
                            <asp:Label ID="AdvanceRequestId" runat="server" Text='<%# Bind("AdvanceRequestId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee">
                        <ItemTemplate>
                            <asp:Label ID="Employee" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Advance Type Code">
                        <ItemTemplate>
                            <asp:Label ID="AdvanceTypeCode" runat="server" Text='<%# Bind("AdvanceTypeCode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Advance Amount">
                        <ItemTemplate>
                            <asp:Label ID="AdvanceAmount" runat="server" Text='<%# Bind("AdvanceAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="AdvanceAmount" runat="server" Text='<%# Bind("AdvanceAmount") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Advance Description">
                        <ItemTemplate>
                            <asp:Label ID="AdvanceDescription" runat="server" Text='<%# Bind("AdvanceDescription") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="AdvanceDescription" runat="server" Text='<%# Bind("AdvanceDescription") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recovery Start Date">
                        <ItemTemplate>
                            <asp:Label ID="RecoveryStartDate" runat="server" Text='<%# Bind("RecoveryStartDate") %>'></asp:Label>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="RecoveryStartDate" runat="server" TextMode="DateTime" Text='<%# Bind("RecoveryStartDate") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recovery Amount">
                        <ItemTemplate>
                            <asp:Label ID="AmountRecovered" runat="server" Text='<%# Bind("RequestedInstallmentAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="AmountRecovered" runat="server" Text='<%# Bind("RequestedInstallmentAmount") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recoveries">
                        <ItemTemplate>
                            <asp:Label ID="Recoveries" runat="server" Text='<%# Bind("RequestedInstallments") %>'></asp:Label>
                        </ItemTemplate>
                       <%-- <EditItemTemplate>
                            <asp:TextBox ID="Recoveries" runat="server" Text='<%# Bind("RequestedInstallments") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Date">
                        <ItemTemplate>
                            <asp:Label ID="RequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="RequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>' TextMode="DateTime"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="WFStatus" runat="server" Text='<%# Bind("WFStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton Text="Update" runat="server" OnClick="Update_Click"/>
                            <asp:LinkButton Text="Cancel" runat="server" OnClick="Cancel_Click"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                    <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" DataFormatString="{0:MM/dd/yyyy}" Visible="false" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                    <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" DataFormatString="{0:MM/dd/yyyy}" Visible="false"/>
                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false"/>
                    <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false"/>
                    <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false"/>
                    <asp:TemplateField HeaderText="RecId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="RecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
            </asp:GridView>
        <div style="display: none;">
            <a href="#" class="paginate" id="previous">Previous</a> |
            <a href="#" class="paginate" id="next">Next</a>
        </div>
        <embed id="embed01" runat="server" height="700" width="800" type="application/pdf" />

        <%--<iframe id="iframe01" runat="server" height="600" width="600" type="application/vnd.ms-excel" />--%>
    </div>
</asp:Content>

