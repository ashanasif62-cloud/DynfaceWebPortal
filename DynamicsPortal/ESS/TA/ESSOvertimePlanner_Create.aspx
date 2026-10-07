<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSOvertimePlanner_Create.aspx.cs" Inherits="DynamicsPortal.ESS.TA.ESSOvertimePlanner_Create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .form-wrapper {
            padding: 20px;
        }

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 16px;
        }

        .form-group label {
            width: 130px;
            min-width: 130px;
            font-size: 14px;
            color: #333;
        }

        .form-group .form-control,
        .form-group select {
            width: 200px !important;
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

        .btn-ok {
            background-color: #5a5a5a;
            color: #fff;
            border: none;
            padding: 6px 20px;
            font-size: 14px;
            border-radius: 2px;
            cursor: pointer;
        }

        .btn-ok:hover {
            background-color: #444;
            color: #fff;
        }

        .btn-cancel {
            background-color: #5a5a5a;
            color: #fff;
            border: none;
            padding: 6px 20px;
            font-size: 14px;
            border-radius: 2px;
            cursor: pointer;
            text-decoration: none;
        }

        .btn-cancel:hover {
            background-color: #444;
            color: #fff;
            text-decoration: none;
        }

  .form-wrapper {
    padding: 20px;
    position: relative;
    min-height: 450px;   /* important for vertical centering */
}

.loading-overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.4);
    z-index: 9999;
    display: none;
}

.loading-overlay.show {
    display: block;
}

.loading-box {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);   /* perfect center */
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

        <div class="form-group">
            <label>Employee Id</label>
            <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Employee Name</label>
            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Request Date</label>
            <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control"
                Text='<%# DateTime.Now.ToString("MM/dd/yyyy") %>'>
            </asp:TextBox>
        </div>

        <div class="form-group">
            <label>Plan Date</label>
            <asp:TextBox ID="txtPlanDate" runat="server" CssClass="form-control" TextMode="Date">
            </asp:TextBox>
        </div>
<div class="form-group">
    <label>Start Time</label>
    <asp:TextBox ID="txtStartTime" runat="server" 
        CssClass="form-control" 
        TextMode="Time" 
        step="1">
    </asp:TextBox>
</div>

<div class="form-group">
    <label>End Time</label>
    <asp:TextBox ID="txtEndTime" runat="server" 
        CssClass="form-control" 
        TextMode="Time" 
        step="1">
    </asp:TextBox>
</div>

        <div class="action-footer">
         <asp:LinkButton ID="btnSave" runat="server" 
    OnClick="btnSave_Click"
    OnClientClick="showLoading();"
    CssClass="btn-ok" 
    CausesValidation="true">
    OK
</asp:LinkButton>
            <asp:LinkButton ID="LinkButton1" runat="server"
                CssClass="btn-cancel"
                OnClientClick="javascript: return closeDialog();">Cancel
            </asp:LinkButton>
        </div>
     <div id="loadingOverlay" class="loading-overlay">
    <div class="loading-box">
        <div class="spinner"></div>
        <div class="loading-text">Processing, please wait...</div>
    </div>
</div>
    </div>

   <script type="text/javascript">
       function showLoading() {
           document.getElementById('loadingOverlay').classList.add('show');
       }
   </script>
</asp:Content>