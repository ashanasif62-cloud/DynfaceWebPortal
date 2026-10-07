<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmploymentCertificate.aspx.cs" Inherits="DynamicsPortal.ESS.PR.EmploymentCertificate" %>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server"
            OnClientClick="javascript: return openPopupPanel('/ESS/PR/EmploymentCertificate_Create.aspx')">
            <i class="mdi mdi-plus"></i>New
        </asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
            <i class="mdi mdi-delete"></i>Delete
        </asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">
            <i class="mdi mdi-check-circle"></i>Submit
        </asp:LinkButton>
    </div>
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div>
        <asp:GridView ID="gridView" runat="server"
            CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true"
            EmptyDataText="No Record Found."
            AutoGenerateColumns="false"
            DataKeyNames="RecId">

            <Columns>
            
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

               
                <asp:TemplateField HeaderText="Certificate ID">
                    <ItemTemplate>
                        <asp:Label ID="lblCertificateId" runat="server" Text='<%# Bind("CertificateTypeCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Job">
                    <ItemTemplate>
                        <asp:Label ID="lblJob" runat="server" Text='<%# Bind("JobId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Requested Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("CreatedDateTime", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Certificate Type">
                    <ItemTemplate>
                        <asp:Label ID="lblCertificateType" runat="server" Text='<%# Bind("CertificateType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Workflow State">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowState" runat="server" Text='<%# Bind("WorkflowState") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Certificate State">
                    <ItemTemplate>
                        <asp:Label ID="lblCertificateState" runat="server" Text='<%# Bind("CertificateState") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>