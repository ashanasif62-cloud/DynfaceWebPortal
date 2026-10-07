<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSEmployeeAppraisalPlan_List.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSEmployeeAppraisalPlan_List" %>
<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/HR/ESSEmployeeAppraisalPlan_Create.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
  
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
              <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee ID">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Appraisal Code">
                    <ItemTemplate>
                       <asp:Label ID="lblAppraisalCode" runat="server" Text='<%# Bind("AppraisalCode") %>'></asp:Label>
                    </ItemTemplate>
                   <%-- <EditItemTemplate>
                        <asp:TextBox ID="txtLeaveReqDate" runat="server" Text='<%# Bind("Appraisal Code") %>' autocomplete="off"  masktype="date"></asp:TextBox>
                    </EditItemTemplate>--%>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plan Weightage">
                    <ItemTemplate>
                        <asp:Label ID="lblPlanWeightage" runat="server" Text='<%# Bind("PlanWeightage") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KPI's">
                    <ItemTemplate>
                        <asp:Label ID="lblKPICode" runat="server" Text='<%# Bind("KPICode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                   <asp:TextBox ID="txtKPICode" runat="server" Text='<%# Bind("KPICode") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<asp:DropDownList ID="ddlLeaveCategory" runat="server" />--%>
                    <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>

                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KPI Weightage">
                    <ItemTemplate>
                        <asp:Label ID="lblKPIWeightage" runat="server" Text='<%# Bind("KPIWeightage") %>'></asp:Label>
                    </ItemTemplate>
                     <EditItemTemplate>
                        <%--<asp:DropDownList ID="ddlLeaveCategory" runat="server" />--%>
                    <asp:TextBox ID="txtKPIWeightage" runat="server" Text='<%# Bind("KPIWeightage") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Review Type">
                    <ItemTemplate>
                        <asp:Label ID="lblReviewType" runat="server" Text='<%# Bind("ReviewType") %>'></asp:Label>
                    </ItemTemplate>

                <%--    <EditItemTemplate>
                        <asp:TextBox ID="txtLeaveStartDate" runat="server" Text='<%# Bind("Weight") %>' autocomplete="off"  masktype="date"></asp:TextBox>
                    </EditItemTemplate>--%>

                </asp:TemplateField>
                <asp:templatefield headertext="recid" visible="false">
                    <ItemTemplate>
                       <asp:label id="lblrecid" runat="server" text='<%# Bind("recid") %>'></asp:label>
                    </itemtemplate>
                </asp:TemplateField>
              
                  <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
<%--                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />--%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField headertext="_KPIRecId" visible="false">
                    <ItemTemplate>
                         <asp:Label ID ="lblKPIRecId" runat="server" text='<%# Bind("KPIRecId") %>'></asp:Label>
                         </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
