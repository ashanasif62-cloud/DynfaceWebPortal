<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferJournalReport.aspx.cs" Inherits="DynamicsPortal.ESS.PR.TransferJournalReport" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
         <div class="action-items">
     <asp:LinkButton ID="btnTransferOrder" runat="server" OnClientClick="window.location.href='/ESS/PR/TransferJournal_ListPage.aspx'; return false;">
<i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
     </asp:LinkButton>
 </div>
  <%--  <div class="action-items">
        <div style="display: inline-block;">
            <asp:LinkButton ID="btnView" OnClientClick="showOverlay();" runat="server" OnClick="btnViewReport_Click" CssClass="action-button">
                <i class="mdi mdi-eye"></i> View On Hand Inventory
            </asp:LinkButton>
        </div>--%>
        <%--<div style="display: inline-block; margin-left: 0.02in;">
            <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click" CssClass="action-button">
                <i class="mdi mdi-email"></i> Email Transfer Order Overview
            </asp:LinkButton>
        </div>--%>
    
        <style>
       .form-buttons {
    margin-top: 15px;       /* space above buttons */
    text-align: right;      /* push buttons to right */
}

.form-buttons .btn {
    margin-left: 8px;       /* spacing between buttons */
}

       
    </style>


        
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
   

    <div class="page-container">
        <!-- Main Report Area (PDF Viewer) -->
        <div class="main-report-area">
            <embed id="embed01" runat="server" type="application/pdf" />
        </div>

        <!-- Parameters Panel -->
        <div class="parameters-panel">
            <div class="section-header">Parameters</div>

          <%--  <div class="section-header">PRINT</div>

            <div class="toggle-group">
                <span class="toggle-label-text">Transfer lines</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkTransferLines" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Reservations</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkReservations" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>--%>

            <div class="section-header">VIEW</div>

            <div class="toggle-group">
                <span class="toggle-label-text">Configuration</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkConfiguration" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Size</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkSize" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Color</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkColor" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Style</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkStyle" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Site</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkSite" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Warehouse</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkWarehouse" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">Location</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkLocation" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
    <span class="toggle-label-text">Version</span>
    <label class="toggle-switch">
        <input type="checkbox" id="chkversion" runat="server" />
        <span class="slider"></span>
    </label>
</div>

            <div class="toggle-group">
    <span class="toggle-label-text">Owner</span>
    <label class="toggle-switch">
        <input type="checkbox" id="chkOwnerName" runat="server" />
        <span class="slider"></span>
    </label>
</div>

            <div class="toggle-group">
    <span class="toggle-label-text">Batch number</span>
    <label class="toggle-switch">
        <input type="checkbox" id="chkbatchId" runat="server" />
        <span class="slider"></span>
    </label>
</div>

            <div class="toggle-group">
    <span class="toggle-label-text">Serial ID</span>
    <label class="toggle-switch">
        <input type="checkbox" id="chkserialid" runat="server" />
        <span class="slider"></span>
    </label>
</div>



            <div class="toggle-group">
                <span class="toggle-label-text">Inventory status</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkInventoryStatus" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="toggle-group">
                <span class="toggle-label-text">License plate</span>
                <label class="toggle-switch">
                    <input type="checkbox" id="chkLicensePlate" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>

            <div class="form-buttons">
    <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="btn btn-primary" OnClick="btnOk_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-secondary" />
</div>
        </div>
    </div>
</asp:Content>