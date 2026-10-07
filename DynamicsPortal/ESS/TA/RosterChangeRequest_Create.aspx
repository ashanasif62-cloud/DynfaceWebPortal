<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true"
    CodeBehind="RosterChangeRequest_Create.aspx.cs"
    Inherits="DynamicsPortal.ESS.TA.RosterChangeRequest_Create" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .form-wrapper {
            padding: 20px;
            position: relative;
            min-height: 350px;
        }

        .form-table { width: 100%; }

        .form-table td {
            padding: 8px 4px;
            vertical-align: middle;
        }

        .form-table td:first-child {
            width: 140px;
            font-size: 14px;
            color: #333;
        }

        .form-table input[type="text"],
        .form-table input[type="date"],
        .form-table select {
            width: 220px;
            height: 32px;
            font-size: 14px;
            padding: 4px 8px;
            border: 1px solid #ccc;
            border-radius: 2px;
        }

        .action-footer {
            display: flex;
            justify-content: flex-end;
            gap: 8px;
            margin-top: 30px;
            padding: 10px 0;
            border-top: 1px solid #e0e0e0;
        }

        .btn-ok, .btn-cancel {
            background-color: #5a5a5a;
            color: #fff;
            border: none;
            padding: 6px 20px;
            font-size: 14px;
            border-radius: 2px;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
        }

        .btn-ok:hover, .btn-cancel:hover {
            background-color: #444;
            color: #fff;
            text-decoration: none;
        }

        /* ===== Loading Overlay ===== */
        .loading-overlay {
            position: absolute;
            top: 0; left: 0; right: 0; bottom: 0;
            background: rgba(0, 0, 0, 0.4);
            z-index: 9999;
            display: none;
        }

        .loading-overlay.show { display: block; }

        .loading-box {
            position: absolute;
            top: 50%; left: 50%;
            transform: translate(-50%, -50%);
            background: #ffffff;
            border-radius: 12px;
            padding: 30px 40px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
            text-align: center;
            min-width: 220px;
        }

        .spinner {
            width: 42px;
            height: 42px;
            border: 4px dotted #2196F3;
            border-top: 4px solid #2196F3;
            border-radius: 50%;
            margin: 0 auto 18px auto;
            animation: spin 0.9s linear infinite;
        }

        .loading-text {
            font-size: 15px;
            color: #333;
            font-family: Arial, sans-serif;
        }

        @keyframes spin {
            0%   { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

     
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

    <div class="form-wrapper">

        <table class="form-table">
            <tr>
                <td><span>Request Date</span></td>
                <td>
                    <asp:TextBox ID="txtRequestDate" runat="server" Enabled="false" TextMode="Date" />
                </td>
            </tr>

            <tr>
                <td><span>Employee</span></td>
                <td>
                    <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails"
                        OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />
                </td>
            </tr>

            <tr>
                <td><span>Employee Name</span></td>
                <td>
                    <asp:TextBox ID="txtEmployeeName" Enabled="false" runat="server" />
                </td>
            </tr>

          <tr>
    <td><span>Shift Id</span></td>
    <td>
        <asp:DropDownList
            ID="ddlShiftId"
            runat="server"
            AutoPostBack="false" />
    </td>
</tr>

            <tr>
                <td><span>From Date</span></td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" />
                </td>
            </tr>

            <tr>
                <td><span>To Date</span></td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" />
                </td>
            </tr>
        </table>

        <div class="action-footer">
            <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">OK</asp:LinkButton>
            <asp:LinkButton ID="btnCancel" runat="server"
                OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
        </div>

        <!-- Loading Overlay -->
        <div id="loadingOverlay" class="loading-overlay">
            <div class="loading-box">
                <div class="spinner"></div>
                <div class="loading-text">Processing, please wait...</div>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        function showLoading() {
            var overlay = document.getElementById('loadingOverlay');
            if (overlay) overlay.classList.add('show');
        }
    </script>


</asp:Content>