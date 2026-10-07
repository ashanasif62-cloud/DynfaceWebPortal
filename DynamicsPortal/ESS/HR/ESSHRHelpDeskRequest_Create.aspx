<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRHelpDeskRequest_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRHelpDeskRequest_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
        <style>
.required-field {
    border: 1px solid #ccc;
    transition: 0.2s;
}

.required-field.invalid {
    border: 1px solid red;
    outline: none;
}
.searchable-dropdown {
    position: relative;
    width: 100%;
    max-width: 320px;
}

.dropdown-input {
    width: 100%;
    padding: 6px 8px;
    font-size: 12px;
    box-sizing: border-box;
    border: 1px solid #ccc;
}

.searchable-panel {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    max-height: 220px;
    overflow-y: auto;
    background: #fff;
    border: 1px solid #ccc;
    border-top: none;
    z-index: 9999;
    box-shadow: 0 4px 8px rgba(0,0,0,0.12);
}

.searchable-option {
    padding: 7px 10px;
    cursor: pointer;
    font-size: 12px;
}

.searchable-option:hover {
    background-color: #f0f0f0;
}
</style>


</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
    <table class="form-table">
        <%--        <tr>
            <td>
                <span>Help Desk Request Id</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSHRHelpDeskRequestId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
                <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
            </td>
        </tr>
        <%--        <tr>
            <td>
                <span>Division</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSDivision" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>--%>
      
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSDate" runat="server" TextMode="date" Enabled="false" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>

         <%-- <tr>
      <td>
          <span>Type of Request</span>
      </td>

      <td>
          <asp:DropDownList ID="ddlESSTypeOfProblem" runat="server"></asp:DropDownList>
      </td>
  </tr> --%>

        <tr>
    <td>
        <span>Type of Request</span>
    </td>
    <td>
        <div style="display: inline-block; overflow: visible; position: relative; width: 100%;">
            <div class="searchable-dropdown" id="typeOfProblemDropdownWrapper">
                <input type="text" 
                       id="typeOfProblemSearchInput" 
                       class="dropdown-input required-field"
                       placeholder="Select Type of Request..." 
                       autocomplete="off"
                       onkeyup="filterTypeOfProblemOptions()" 
                       onclick="showTypeOfProblemPanel()" />
                <div class="searchable-panel" id="typeOfProblemPanel" style="display:none;"></div>
            </div>

            <!-- Real control stays for postback / SelectedValue; hidden from view -->
            <asp:DropDownList ID="ddlESSTypeOfProblem" runat="server"
                CssClass="dropdown-input"
                style="display:none;" />
        </div>
    </td>
</tr>
       <%-- <tr>
            <td>
                <span>Transaction Status</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlTransactionStatus" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>--%>
      <tr>
    <td>
    <label for="txtDetail">Detail <span style="color:red">*</span></label>
    <asp:TextBox ID="txtDetail" runat="server" TextMode="MultiLine"
        style="width:100%; min-height:120px; font-size:12px; box-sizing:border-box;"></asp:TextBox>
    </td>
</tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" OnClientClick="return validateDetail();" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" OnClientClick="return validateDetail();" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Close</asp:LinkButton>
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
<%--    <script>
        function validateDetail() {
            var txt = document.getElementById('<%= txtDetail.ClientID %>');
    var errMsg = document.getElementById('detailError');

    if (!txt.value.trim()) {
        txt.classList.add("invalid");
        errMsg.style.display = 'inline';
        return false;
    } else {
        txt.classList.remove("invalid");
        errMsg.style.display = 'none';
        return true;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var txt = document.getElementById('<%= txtDetail.ClientID %>');

    txt.addEventListener("input", function () {
        if (txt.value.trim()) {
            txt.classList.remove("invalid");
            document.getElementById('detailError').style.display = 'none';
        }
    });
});
    </script>--%>

    <script type="text/javascript">
    // ========== Searchable dropdown for Type of Request ==========
    function showTypeOfProblemPanel() {
        var panel = document.getElementById('typeOfProblemPanel');
        if (panel) {
            panel.style.display = 'block';
            populateTypeOfProblemPanel();
        }
    }

    function populateTypeOfProblemPanel(filterText) {
        var ddl = document.getElementById('<%= ddlESSTypeOfProblem.ClientID %>');
        var panel = document.getElementById('typeOfProblemPanel');
        if (!ddl || !panel) return;

        filterText = (filterText || '').toLowerCase();
        panel.innerHTML = '';

        for (var i = 0; i < ddl.options.length; i++) {
            var opt = ddl.options[i];
            var text = opt.text || '';
            var value = opt.value || '';

            if (filterText && text.toLowerCase().indexOf(filterText) === -1) {
                continue;
            }

            var div = document.createElement('div');
            div.className = 'searchable-option';
            div.innerText = text;
            div.setAttribute('data-value', value);
            div.onclick = function () {
                selectTypeOfProblem(this.getAttribute('data-value'), this.innerText);
            };
            panel.appendChild(div);
        }

        if (panel.children.length === 0) {
            var noResult = document.createElement('div');
            noResult.className = 'searchable-option';
            noResult.style.color = '#999';
            noResult.innerText = 'No matching type found';
            panel.appendChild(noResult);
        }
    }

    function filterTypeOfProblemOptions() {
        var input = document.getElementById('typeOfProblemSearchInput');
        populateTypeOfProblemPanel(input.value);
        showTypeOfProblemPanel();
    }

    function selectTypeOfProblem(value, text) {
        var ddl = document.getElementById('<%= ddlESSTypeOfProblem.ClientID %>');
        var input = document.getElementById('typeOfProblemSearchInput');
        var panel = document.getElementById('typeOfProblemPanel');

        if (ddl) {
            ddl.value = value;
        }
        if (input) {
            input.value = text;
        }
        if (panel) {
            panel.style.display = 'none';
        }
    }

    // Close panel when clicking outside
    document.addEventListener('click', function (e) {
        var wrapper = document.getElementById('typeOfProblemDropdownWrapper');
        var panel = document.getElementById('typeOfProblemPanel');
        if (wrapper && panel && !wrapper.contains(e.target)) {
            panel.style.display = 'none';
        }
    });

    // Keep the visible input in sync after postback / UpdatePanel refresh
    function syncTypeOfProblemDisplay() {
        var ddl = document.getElementById('<%= ddlESSTypeOfProblem.ClientID %>');
        var input = document.getElementById('typeOfProblemSearchInput');
        if (ddl && input && ddl.selectedIndex >= 0) {
            input.value = ddl.options[ddl.selectedIndex].text;
        }
    }

    // Run after page load and after every UpdatePanel partial postback
    if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            syncTypeOfProblemDisplay();
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        syncTypeOfProblemDisplay();
    });
    </script>
</asp:Content>
