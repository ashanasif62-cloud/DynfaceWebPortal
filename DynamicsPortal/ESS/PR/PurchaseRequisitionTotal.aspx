<%@ Page Title="Purchase Requisition Totals" Language="C#" MasterPageFile="~/Modal.Master"
    AutoEventWireup="true" CodeBehind="PurchaseRequisitionTotal.aspx.cs"
    Inherits="DynamicsPortal.ESS.PR.PurchaseRequisitionTotal" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .pr-container {
            width: 100%;
            padding: 20px;
            font-family: 'Segoe UI', sans-serif;
        }

        .pr-header {
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 15px;
        }

        .pr-section {
            border-top: 1px solid #ddd;
            padding-top: 15px;
            margin-top: 10px;
        }

        .pr-grid {
            display: flex;
            gap: 60px;
        }

        .pr-column {
            flex: 1;
        }

        .form-group {
            margin-bottom: 12px;
        }

        label {
            display: block;
            font-weight: 500;
            margin-bottom: 3px;
        }

        select,
        input[type="text"] {
            width: 150px;
            padding: 6px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        input[readonly] {
            background-color: #f9f9f9;
        }

        .pr-total-box {
            font-size: 22px;
            font-weight: bold;
            text-align: right;
            padding: 5px;
        }

        .btn-footer {
            text-align: right;
            margin-top: 20px;
        }

        .btn-ok {
            background-color: #2f6fed;
            color: white;
            border: none;
            padding: 8px 20px;
            border-radius: 4px;
            cursor: pointer;
        }

        .btn-ok:hover {
            background-color: #1e56d0;
        }
    </style>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="pr-container">
        <div class="pr-header">Totals</div>

        <div class="pr-section">
            <h4>Purchase requisition totals</h4>

            <div class="pr-grid">
                <!-- LEFT COLUMN -->
                <div class="pr-column">
                    <div class="form-group">
                        <label>Currency</label>
                        <asp:DropDownList ID="ddlCurrency" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                    </div>

                        <div class="form-group">
                            <label>Line discount</label>
                            <asp:TextBox ID="txtLineDiscount" runat="server" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Subtotal amount</label>
                            <asp:TextBox ID="txtSubtotalAmount" runat="server" CssClass="pr-total-box" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Charges</label>
                            <asp:TextBox ID="txtCharges" runat="server" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Sales tax</label>
                            <asp:TextBox ID="txtSalesTax" runat="server" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Round-off</label>
                            <asp:TextBox ID="txtRoundOff" runat="server" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Total amount</label>
                            <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="pr-total-box" Text="0.00" ReadOnly="true"></asp:TextBox>
                        </div>
                </div>

                <!-- RIGHT COLUMN -->
                <div class="pr-column">
                    <div class="form-group">
                        <label>Number of lines</label>
                        <asp:TextBox ID="txtNumberOfLines" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="btn-footer">
                <asp:Button ID="btnOK" OnClientClick="closeDialog();" runat="server" Text="OK" CssClass="btn-ok" />
            </div>
        </div>
    </div>
</asp:Content>
