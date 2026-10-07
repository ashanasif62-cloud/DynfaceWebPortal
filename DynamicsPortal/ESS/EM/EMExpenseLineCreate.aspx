<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="EMExpenseLineCreate.aspx.cs" Inherits="DynamicsPortal.ESS.EM.EMExpenseLineCreate" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            const numericInputs = document.querySelectorAll('.numeric-input');

            numericInputs.forEach(function (input) {
                input.addEventListener('input', function () {
                    this.value = this.value.replace(/[^0-9.]/g, '');
                });
            });
        });
    </script>
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Transaction Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtTransDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblExpenseReprotNumber">Expense Report Number</asp:Label>
            </td>

            <td>
                <asp:TextBox ID="txtExpenseReqportNumber" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Expense Category</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlExpenseCategory" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMerchant_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Merchant</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlMerchant" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Amount</span>
            </td>
            <td>
                <asp:TextBox ID="txtAmountCur" runat="server" CssClass="numeric-input"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <span>Currency</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlExchangeCode" runat="server"></asp:DropDownList>
            </td>
        </tr>

        <asp:PlaceHolder ID="phProjectRow" runat="server">
            <tr>
                <td>
                    <span>Project ID</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlProjId" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phBillableRow" runat="server">
            <tr>
                <td>
                    <span>Billable</span>
                </td>

                <td>
                    <asp:DropDownList ID="ddlProjStatusId" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phProjActivityNumberRow" runat="server">
            <tr>
                <td>
                    <span>Activity Number</span>
                </td>

                <td>
                    <asp:DropDownList ID="ddlProjActivityNumber" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
