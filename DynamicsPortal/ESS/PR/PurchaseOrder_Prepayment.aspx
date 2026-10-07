<%@ Page Title="Create or edit a prepayment" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true"
    CodeFile="PurchaseOrder_Prepayment.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_Prepayment" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
        <style>
            /* Hide Master Page Header/Title Section */
            .modal-header, .page-header, .title-section, #pageTitle { display: none !important; }

            .prepayment-container {
                padding: 10px 25px;
                font-family: "Segoe UI", Arial, sans-serif;
                background-color: #fff;
            }

            .d365-form-group {
                margin-bottom: 0.75rem;
            }

            .d365-label {
                font-size: 0.8rem;
                color: #333;
                font-weight: 500;
                margin-bottom: 0.2rem;
                display: block;
            }

            .d365-input {
                width: 180px;
                /* Reduced width */
                height: 28px;
                padding: 2px 8px;
                font-size: 0.85rem;
                border: 1px solid #8a8886;
                border-radius: 2px;
                color: #323130;
            }

            .d365-input:focus {
                border-color: #0078d4;
                outline: none;
                box-shadow: 0 0 0 1px #0078d4;
            }

            .required-wrapper {
                position: relative;
                display: inline-block;
                width: 180px;
            }

            .required-wrapper .d365-input {
                border-color: #a4262c;
                border-right: 2px solid #a4262c;
            }

            .required-wrapper::after {
                content: "*";
                color: #a4262c;
                position: absolute;
                right: 8px;
                top: 50%;
                transform: translateY(-35%);
                font-size: 1.1rem;
                pointer-events: none;
            }

            .d365-radio-group {
                display: flex;
                flex-direction: column;
                gap: 8px;
            }

            .d365-radio {
                display: flex;
                align-items: center;
                font-size: 0.85rem;
                color: #323130;
                cursor: pointer;
            }

            .d365-radio input[type="radio"] {
                margin-right: 6px;
                width: 14px;
                height: 14px;
                accent-color: #0078d4;
            }

            .footer-buttons {
                display: flex;
                justify-content: flex-end;
                gap: 10px;
                padding: 15px 30px;
                background-color: #fff;
                position: sticky;
                bottom: 0;
                width: 100%;
                z-index: 1000;
            }

            .btn-d365 {
                font-size: 14px;
                min-width: 90px;
                font-weight: 600;
                border-radius: 4px;
                padding: 6px 16px;
                border: 1px solid transparent;
            }

            .btn-primary {
                background-color: #2b579a;
                /* D365 standard blue */
                border-color: #2b579a;
                color: white;
            }

            .btn-primary:hover {
                background-color: #1e3f73;
            }

            .btn-default {
                background-color: #fff;
                border-color: #8a8886;
                color: #323130;
            }

            .btn-default:hover {
                background-color: #f3f2f1;
            }

            .page-title {
                font-size: 1.1rem;
                font-weight: 600;
                color: #323130;
                margin-bottom: 1rem;
                margin-top: 5px;
            }

            .d365-select {
                width: 180px;
                height: 28px;
                padding: 2px 8px;
                font-size: 0.85rem;
                border: 1px solid #8a8886;
                border-radius: 2px;
                color: #323130;
                background-color: #fff;
                appearance: none;
                background-image: url("data:image/svg+xml;charset=US-ASCII,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%22292.4%22%20height%3D%22292.4%22%3E%3Cpath%20fill%3D%22%23323130%22%20d%3D%22M287%2069.4a17.6%2017.6%200%200%200-13-5.4H18.4c-5%200-9.3%201.8-12.9%205.4A17.6%2017.6%200%200%200%200%2082.2c0%205%201.8%209.3%205.4%2012.9l128%20127.9c3.6%203.6%207.8%205.4%2012.8%205.4s9.2-1.8%2012.8-5.4L287%2095c3.5-3.5%205.4-7.8%205.4-12.8%200-5-1.9-9.2-5.5-12.8z%22%2F%3E%3C%2Fsvg%3E");
                background-repeat: no-repeat, repeat;
                background-position: right .7em top 50%, 0 0;
                background-size: .65em auto, 100%;
            }

            .d365-select:focus {
                border-color: #0078d4;
                outline: none;
            }

            .text-blue {
                color: #2b579a !important;
            }

            .bg-readonly {
                background-color: #f3f2f1 !important;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
            <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>

        <div class="prepayment-container">
            <h2 class="page-title">Create or edit a prepayment</h2>

            <div class="d365-form-group">
                <label class="d365-label">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="d365-input"></asp:TextBox>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Type</label>
                <div class="d365-radio-group">
                    <label class="d365-radio">
                        <input type="radio" name="rdoType" value="Fixed"  checked="checked" /> Fixed
                    </label>
                    <label class="d365-radio">
                        <input type="radio" name="rdoType" value="Percent" /> Percent
                    </label>
                </div>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Value</label>
                <div class="required-wrapper">
                    <asp:TextBox ID="txtValue" runat="server" CssClass="d365-input text-end" Text="0.00"></asp:TextBox>
                </div>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Limit</label>
                <asp:TextBox ID="txtLimit" runat="server" CssClass="d365-input bg-readonly text-end" Text="0.00"
                    ReadOnly="true"></asp:TextBox>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Prepayment remaining</label>
                <asp:TextBox ID="txtPrepaymentRemaining" runat="server" CssClass="d365-input bg-readonly text-end"
                    Text="0.00" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Prepayment application remaining</label>
                <asp:TextBox ID="txtPrepaymentApplicationRemaining" runat="server"
                    CssClass="d365-input bg-readonly text-end" Text="0.00" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Currency</label>
                <asp:TextBox ID="txtCurrency" runat="server" CssClass="d365-input bg-readonly text-blue" Text="USD"
                    ReadOnly="true"></asp:TextBox>
            </div>

            <div class="d365-form-group">
                <label class="d365-label">Prepayment category ID</label>
                <asp:DropDownList ID="ddlPrepaymentCategory" runat="server" CssClass="d365-select text-blue">
                    <asp:ListItem Text="pre payment" Value="pre payment"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="footer-buttons">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-d365 btn-primary"
                OnClick="btnSave_Click" />
            <button type="button" class="btn btn-d365 btn-default" onclick="closeDialog();">Cancel</button>
        </div>
            </ContentTemplate>
</asp:UpdatePanel>
    </asp:Content>