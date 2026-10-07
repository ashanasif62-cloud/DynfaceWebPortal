<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_AllocateCharges.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_AllocateCharges" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .allocate-container {
            padding: 20px;
            font-family: "Segoe UI", Arial, sans-serif;
            font-size: 14px;
        }

        .allocate-title {
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 20px;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 280px;
            column-gap: 30px;
        }

        .form-row {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
        }

        .form-row label {
            width: 180px; /* slightly reduced to match image */
            margin-right: 10px;
        }

        .checkbox-group {
            display: flex;
            flex-direction: column;
            margin-left: 0;
            margin-bottom: 10px;
        }

        .right-options {
            margin-top: 0;
        }

        .footer-buttons {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
            margin-top: 25px;
        }

        .btn {
            min-width: 90px;
            padding: 6px 12px;
            font-size: 14px;
        }

        .btn-primary {
            background-color: #0078d4;
            color: #fff;
            border: 1px solid #0078d4;
            border-radius: 2px;
        }

        .btn-primary:hover {
            background-color: #005a9e;
            border-color: #005a9e;
        }

        .btn-default {
            background-color: #f3f2f1;
            color: #323130;
            border: 1px solid #8a8886;
            border-radius: 2px;
        }

        .btn-default:hover {
            background-color: #e1dfdd;
        }
        .checkbox-group {
    display: flex;          /* align items in a row */
    align-items: center;    /* vertically center checkbox and text */
    margin-bottom: 5px;
    gap: 5px;               /* space between checkbox and text */
    flex-direction: row;    /* make sure it's horizontal */
}

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

    <div class="allocate-container">

        <div class="allocate-title">
            Allocate charges to order lines
        </div>

        <div class="form-grid">

            <!-- LEFT SIDE -->
            <div>

                <div class="form-row">
                    <label>Charges allocation</label>
                    <asp:DropDownList ID="ddlChargesAllocation" runat="server" Width="200px" />
                </div>

                <div class="form-row">
                    <label>Allocate charges to lines</label>
                    <asp:DropDownList ID="ddlAllocateToLines" runat="server" Width="200px" />
                </div>

                <div class="checkbox-group">
                    <asp:CheckBox ID="chkAllocateAll" runat="server" Text="Allocate all" />
                </div>

            </div>

            <!-- RIGHT SIDE -->
            <div class="right-options">

                <div class="checkbox-group">
                    <asp:CheckBox ID="chkReceived" runat="server" Text="Received" />
                </div>

                <div class="checkbox-group">
                    <asp:CheckBox ID="chkStocked" runat="server" Text="Stocked" />
                </div>

                <div class="checkbox-group">
                    <asp:CheckBox ID="chkShowSelections" runat="server" Text="Show selections and clear specific lines" />
                </div>

            </div>

        </div>

        <div class="footer-buttons">
            <asp:Button ID="btnAllocate" runat="server"
                Text="Allocate"
                CssClass="btn btn-primary"
                OnClick="btnAllocate_Click" />

            <asp:Button ID="btnCancel" runat="server"
                Text="Cancel"
                CssClass="btn btn-default"
                OnClick="btnCancel_Click" />
        </div>

    </div>

</asp:Content>
