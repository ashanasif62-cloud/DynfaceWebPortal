<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="DynamicsPortal.WebForm1" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClick="Button1_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server" OnClick="btnView_Click"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="Button2_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
    <!--<div class="action-items"><a onclick=""><i class="mdi mdi-delete"></i></a>Delete</div>-->
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gv" runat="server"  data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
            OnRowEditing="gv_RowEditing" OnRowCancelingEdit="gv_RowCancelingEdit" OnRowDataBound="gv_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderStyle Width="15px" />
                    <HeaderTemplate>
                        <input type="checkbox" id="selectAll" />
                    </HeaderTemplate>
                    <ItemStyle Width="15px" />
                    <ItemTemplate>
                        <input type="checkbox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderStyle-CssClass="sorttable_nosort" ShowEditButton="True"></asp:CommandField>
            </Columns>

        </asp:GridView>
        <!-- 
        <asp:GridView ID="GridView1"  data="searchable" CssClass="table table-condensed no-border table-hover sortable" runat="server">
        </asp:GridView>


            sortable
            DataKeyNames="ProductID"
            add class="sorttable_nosort" to the <th> column header
            AllowPaging="true" PageSize="7" OnPageIndexChanging ="gv_PageIndexChanging"         <PagerStyle CssClass="" HorizontalAlign="Center" />
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
            
        <div style="display: none;">
            <a href="#" class="paginate" id="previous">Previous</a> |
            <a href="#" class="paginate" id="next">Next</a>
        </div>
        <embed id="embed01" runat="server" height="700" width="800" type="application/pdf" />
            
            -->

        <!--<iframe id="iframe01" runat="server" height="600" width="600" type="application/vnd.ms-excel" />-->
    </div>
</asp:Content>
