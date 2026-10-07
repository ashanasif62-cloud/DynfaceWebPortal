<%--<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="ESSHRPersonalDetails.aspx.cs"
    Inherits="DynamicsPortal.ESSHRPersonalDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        label {
            display: inline-block;
            margin-bottom: 0;
        }

        .tabs {
            position: relative;
            padding: 10px;
        }

        /* Hide radio buttons */
        .tabs input[name="tab-control"] {
            display: none;
        }

        .tabs ul {
            list-style: none;
            padding: 0;
            display: flex;
            gap: 15px;
        }

        .tabs ul li label {
            cursor: pointer;
            color: #2f2f2f;
            padding-bottom: 5px;
            white-space: nowrap;
        }

        /* ============================= */
        /* ACTIVE TAB UNDERLINE (FIXED)  */
        /* ============================= */
        #tab1:checked ~ ul label[for="tab1"],
        #tab2:checked ~ ul label[for="tab2"],
        #tabBank:checked ~ ul label[for="tabBank"],
        #tab3:checked ~ ul label[for="tab3"],
        #tab5:checked ~ ul label[for="tab5"],
        #tabPersonalContacts:checked ~ ul label[for="tabPersonalContacts"] {
            border-bottom: 2px solid black;
            color: #474747;
            cursor: default;
        }

        /* CONTENT */
        .tabs .content section {
            display: none;
            animation: fade 0.3s ease-in-out;
        }

        /* ============================= */
        /* CONTENT VISIBILITY (FIXED)    */
        /* ============================= */
        #tab1:checked ~ .content section:nth-child(1),
        #tab2:checked ~ .content section:nth-child(2),
        #tabBank:checked ~ .content section:nth-child(3),
        #tab3:checked ~ .content section:nth-child(4),
        #tab5:checked ~ .content section:nth-child(5),
        #tabPersonalContacts:checked ~ .content section:nth-child(6) {
            display: block;
        }

        iframe {
            border: none;
            width: 100%;
            height: 60vh;
        }

        @keyframes fade {
            from {
                opacity: 0;
                transform: translateY(5%);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .page-header-wrapper {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 15px;
            padding-right: 10px;
        }

        .refresh-btn {
            background-color: #f0f0f0;
            border: 1px solid #ddd;
            padding: 8px 12px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .refresh-btn:hover {
            background-color: #e0e0e0;
            border-color: #999;
        }

        .refresh-btn i {
            display: inline-block;
        }

        .refresh-btn.spinning i {
            animation: spin 0.6s linear infinite;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }
            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <script>
        function refreshPersonalDetails() {
            var btn = document.getElementById('btnRefresh');
            var icon = btn.querySelector('i');
            
            // Add spinning animation
            icon.classList.add('spinning');
            btn.disabled = true;
            
            // Trigger postback to refresh
            __doPostBack('btnRefresh', '');
            
            // Remove spinning animation after a delay
            setTimeout(function() {
                icon.classList.remove('spinning');
                btn.disabled = false;
            }, 1000);
        }
    </script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
            <div class="page-header-wrapper">
                <div></div>
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="refresh-btn" OnClick="btnRefresh_Click" ToolTip="Refresh Page">
                    <i class="mdi mdi-refresh"></i>
                    <span>Refresh</span>
                </asp:LinkButton>
            </div>

            <div class="tabs">

                <!-- RADIO BUTTONS -->
                <input type="radio" id="tab1" name="tab-control" checked />
                <input type="radio" id="tab2" name="tab-control" />
                <input type="radio" id="tabBank" name="tab-control" />
                <input type="radio" id="tabPersonalContacts" name="tab-control" />
                <input type="radio" id="tab3" name="tab-control" />
                <input type="radio" id="tab5" name="tab-control" />

                <!-- TAB HEADERS -->
                <ul>
                    <li><label for="tab1">Contact Details</label></li>
                    <li><label for="tab2">Identification Number</label></li>
                    <li><label for="tabBank">Bank Account</label></li>
                    <li><label for="tabPersonalContacts">Personal Contacts</label></li>
                    <li><label for="tab3">Education</label></li>
                    <li><label for="tab5">Image</label></li>
                </ul>

                <!-- TAB CONTENT -->
                <div class="content">
                    <section>
                        <iframe runat="server" id="logisticsElectronicAddress" src="#" />
                    </section>
                    <section>
                        <iframe runat="server" id="hcmPersonIdentificationNumber" src="#" />
                    </section>
                    <section>
                        <iframe runat="server" id="hcmBankAccount" src="#" />
                    </section>
                    <section>
                        <iframe runat="server" id="hcmPersonEducation" src="#" />
                    </section>
                    <section>
                        <iframe runat="server" id="hcmPersonImage" src="#" />
                    </section>
                    <section>
                        <iframe runat="server" id="EssPersonalContacts" src="#" />
                    </section>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>--%>


<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="ESSHRPersonalDetails.aspx.cs"
    Inherits="DynamicsPortal.ESSHRPersonalDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    label { display: inline-block; margin-bottom: 0; }

    .page-header {
        display: flex;
        justify-content: flex-end;
        align-items: center;
        padding: 12px 0 8px;
        border-bottom: 1px solid #e0e0e0;
        margin-bottom: 0;
    }

    .refresh-btn {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 13px;
        color: #555;
        background: #f5f5f5;
        border: 1px solid #ddd;
        border-radius: 6px;
        padding: 6px 12px;
        cursor: pointer;
        transition: background 0.2s;
        text-decoration: none;
    }
    .refresh-btn:hover { background: #e8e8e8; color: #333; text-decoration: none; }
    .refresh-btn i { font-size: 14px; display: inline-block; transition: transform 0.6s linear; }
    .refresh-btn.spinning i { transform: rotate(360deg); }

    /* Tab bar */
    .tab-bar {
        display: flex;
        gap: 0;
        border-bottom: 1px solid #ddd;
        overflow-x: auto;
        scrollbar-width: none;  
        margin-top: 0;
    }
    .tab-bar::-webkit-scrollbar {
    display: none;               /* Chrome/Safari */
}

    .tab-bar label {
        font-size: 13px;
        color: #666;
        padding: 10px 16px;
        cursor: pointer;
        white-space: nowrap;
        border-bottom: 2px solid transparent;
        margin-bottom: -1px;
        transition: color 0.15s, border-color 0.15s;
        display: block;
    }
    .tab-bar label:hover { color: #222; }

    /* Hide radios */
    .tab-radios input[type="radio"] { display: none; }

    /* Active tab — order MUST match section nth-child order below */
    #tab1:checked ~ .tab-bar label[for="tab1"],
    #tab2:checked ~ .tab-bar label[for="tab2"],
    #tab3:checked ~ .tab-bar label[for="tab3"],
    #tab4:checked ~ .tab-bar label[for="tab4"],
    #tab5:checked ~ .tab-bar label[for="tab5"],
    #tab6:checked ~ .tab-bar label[for="tab6"] {
        color: #222;
        border-bottom: 2px solid #222;
        font-weight: 500;
        cursor: default;
    }

    /* Content panels */
    .tab-content section { display: none; padding-top: 16px; }

    #tab1:checked ~ .tab-content section:nth-child(1),
    #tab2:checked ~ .tab-content section:nth-child(2),
    #tab3:checked ~ .tab-content section:nth-child(3),
    #tab4:checked ~ .tab-content section:nth-child(4),
    #tab5:checked ~ .tab-content section:nth-child(5),
    #tab6:checked ~ .tab-content section:nth-child(6) {
        display: block;
        animation: fadeIn 0.25s ease;
    }

    @keyframes fadeIn {
        from { opacity: 0; transform: translateY(4px); }
        to   { opacity: 1; transform: translateY(0); }
    }

    iframe {
        border: none;
        width: 100%;
        height: 60vh;
        display: block;
    }
    .refresh-btn {
    margin-left: auto;
}
</style>

<script>
    function triggerRefresh() {
        var btn = document.getElementById('btnRefresh');
        if (!btn) return;
        btn.classList.add('spinning');
        setTimeout(function () { btn.classList.remove('spinning'); }, 700);
    }
</script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <%-- Persists the active tab across postbacks --%>
    <asp:HiddenField ID="hdnActiveTab" runat="server" Value="tab1" />

    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>

        

            <%-- 
                IMPORTANT: radio IDs must be declared in the same order as
                section nth-child below. Current order:
                1=Contact  2=ID Number  3=Bank  4=PersonalContacts  5=Education  6=Image
            --%>
            <div class="tab-radios">
                <input type="radio" id="tab1" name="tab-control" checked="checked" />
                <input type="radio" id="tab2" name="tab-control" />
                <input type="radio" id="tab3" name="tab-control" />
                <input type="radio" id="tab4" name="tab-control" />
                <input type="radio" id="tab5" name="tab-control" />
                <input type="radio" id="tab6" name="tab-control" />

              <div class="tab-bar">
    <label for="tab1">Contact Details</label>
    <label for="tab2">Identification Number</label>
    <label for="tab3">Bank Account</label>
    <label for="tab4">Personal Contacts</label>
    <label for="tab5">Education</label>
    <label for="tab6">Image</label>
                 
    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="refresh-btn"
        OnClick="btnRefresh_Click" OnClientClick="triggerRefresh()" ToolTip="Refresh">
        <i class="mdi mdi-refresh"></i>
    </asp:LinkButton>

</div>

                <div class="tab-content">
                    <section><iframe runat="server" id="logisticsElectronicAddress" src="#" /></section>
                    <section><iframe runat="server" id="hcmPersonIdentificationNumber" src="#" /></section>
                    <section><iframe runat="server" id="hcmBankAccount" src="#" /></section>
                    <section><iframe runat="server" id="EssPersonalContacts" src="#" /></section>
                    <section><iframe runat="server" id="hcmPersonEducation" src="#" /></section>
                    <section><iframe runat="server" id="hcmPersonImage" src="#" /></section>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

   <script>
       function restoreActiveTab() {
           var saved = document.getElementById('<%= hdnActiveTab.ClientID %>');
           var tabId = (saved && saved.value) ? saved.value : 'tab1';
           var radio = document.getElementById(tabId);
           if (radio) radio.checked = true;
       }

       /* Save tab on click */
       document.addEventListener('click', function (e) {
           var lbl = e.target.closest('.tab-bar label');
           if (!lbl) return;
           var saved = document.getElementById('<%= hdnActiveTab.ClientID %>');
        if (saved) saved.value = lbl.getAttribute('for');
    });

       /* Restore immediately on load */
       restoreActiveTab();

       /* Restore after every UpdatePanel postback */
       if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
               restoreActiveTab();
           });
       }
   </script>
</asp:Content>