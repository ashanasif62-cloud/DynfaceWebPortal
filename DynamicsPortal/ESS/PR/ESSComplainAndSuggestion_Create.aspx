<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" 
    CodeBehind="ESSComplainAndSuggestion_Create.aspx.cs" 
    Inherits="DynamicsPortal.ESS.PR.ESSComplainAndSuggestion_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .complaint-form {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0 12px;
        table-layout: fixed;
    }

    .complaint-form td {
        padding: 5px 10px;
        vertical-align: top;
    }

    .complaint-form .form-label {
        width: 25%;
        font-size: 13px;
        font-weight: 600;
        color: #333;
        padding-top: 11px;
    }

    .complaint-form .form-input {
        width: 75%;
    }

    .complaint-form input[type="text"],
    .complaint-form input[type="date"],
    .complaint-form select,
    .complaint-form textarea {
        width: 100%;
        max-width: 100%;
        box-sizing: border-box;
        padding: 9px 11px;
        font-size: 13px;
        border: 1px solid #d5dbe3;
        border-radius: 4px;
        background: #fff;
        color: #333;
    }

    .complaint-form input[readonly],
    .complaint-form input:disabled {
        background: #f3f5f7;
        color: #666;
    }

    .complaint-form textarea {
        min-height: 110px;
        resize: vertical;
    }

    .required-mark {
        color: #d93025;
    }

    /* Request Type: horizontal radio buttons */
    .radio-list {
        display: flex;
        flex-direction: row;
        align-items: center;
        gap: 24px;
        min-height: 35px;
    }

    .radio-list td {
        padding: 0 !important;
        vertical-align: middle;
    }

    .radio-list label {
        display: inline-flex;
        align-items: center;
        gap: 5px;
        margin: 0;
        font-size: 13px;
        font-weight: 400;
        cursor: pointer;
        white-space: nowrap;
    }

    .radio-list input[type="radio"] {
        appearance: auto;
        -webkit-appearance: radio;
        width: 13px !important;
        height: 13px !important;
        margin: 0 4px 0 0;
        padding: 0 !important;
        border: none !important;
        outline: none;
        box-shadow: none !important;
        accent-color: #1677d2;
        cursor: pointer;
    }

    .radio-list input[type="radio"]:hover,
    .radio-list input[type="radio"]:focus {
        outline: none;
        box-shadow: none !important;
    }

    .searchable-dropdown {
        position: relative;
        width: 100%;
    }

    .dropdown-input {
        width: 100%;
        padding: 9px 11px;
        font-size: 13px;
        box-sizing: border-box;
        border: 1px solid #d5dbe3;
        border-radius: 4px;
    }

    .searchable-panel {
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        max-height: 220px;
        overflow-y: auto;
        background: #fff;
        border: 1px solid #d5dbe3;
        border-radius: 4px;
        z-index: 9999;
        box-shadow: 0 4px 8px rgba(0,0,0,.12);
    }

    .searchable-option {
        padding: 9px 11px;
        cursor: pointer;
        font-size: 13px;
    }

    .searchable-option:hover {
        background: #f2f6fa;
    }

    .action-footer {
        display: flex;
        justify-content: flex-end;
        gap: 10px;
        padding: 15px 10px;
    }

    .action-footer a {
        display: inline-block;
        min-width: 85px;
        padding: 9px 18px;
        text-align: center;
        text-decoration: none;
        border-radius: 4px;
        font-size: 13px;
        font-weight: 600;
    }

    .action-footer .btn-save {
        background: #1677d2;
        color: #fff;
    }

    .action-footer .btn-cancel {
        background: #edf0f4;
        color: #333;
    }

    @media (max-width: 600px) {
        .complaint-form,
        .complaint-form tbody,
        .complaint-form tr,
        .complaint-form td {
            display: block;
            width: 100%;
            box-sizing: border-box;
        }

        .complaint-form .form-label {
            padding-bottom: 3px;
        }

        .complaint-form .form-input {
            padding-top: 0;
        }
    }
</style>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="updComplainSuggestionForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
        <table class="form-table complaint-form">
    <tr>
        <td class="form-label">Employee</td>
        <td class="form-input">
            <uc1:DropDownList_EmployeeDetails
                runat="server"
                ID="cddlEmployeeDetails" />
        </td>
    </tr>

    <tr>
        <td class="form-label">Request Date</td>
        <td class="form-input">
            <asp:TextBox
                ID="txtRequestDate"
                runat="server"
                TextMode="Date"
                Enabled="false"
                ReadOnly="true" />
        </td>
    </tr>

    <tr>
        <td class="form-label">
            Request Type <span class="required-mark">*</span>
        </td>
        <td class="form-input">
            <asp:RadioButtonList
                ID="rblRequestType"
                runat="server"
                RepeatDirection="Horizontal"
                RepeatLayout="Flow"
                AutoPostBack="true"
                OnSelectedIndexChanged="rblRequestType_SelectedIndexChanged"
                CssClass="radio-list">
                <asp:ListItem Text="Complaint" Value="Complaint" />
                <asp:ListItem Text="Suggestion" Value="Suggestion" />
            </asp:RadioButtonList>
        </td>
    </tr>

    <tr>
        <td class="form-label">
            Type Code <span class="required-mark">*</span>
        </td>
        <td class="form-input">
            <div class="searchable-dropdown"
                 id="typeCodeDropdownWrapper">

                <input type="text"
                       id="typeCodeSearchInput"
                       class="dropdown-input"
                       placeholder="Select Type Code..."
                       autocomplete="off"
                       onkeyup="filterTypeCodeOptions()"
                       onclick="showTypeCodePanel()" />

                <div class="searchable-panel"
                     id="typeCodePanel"
                     style="display:none;">
                </div>
            </div>

            <asp:DropDownList
                ID="ddlTypeCode"
                runat="server"
                Style="display:none;" />
        </td>
    </tr>

    <tr>
        <td class="form-label">
            Description <span class="required-mark">*</span>
        </td>
        <td class="form-input">
            <asp:TextBox
                ID="txtDescription"
                runat="server"
                TextMode="MultiLine"
                Rows="5"
                placeholder="Enter description..." />
        </td>
    </tr>

           <tr>
      <td class="form-label">
          Remarks 
      </td>
      <td class="form-input">
          <asp:TextBox
              ID="txtRemarks"
              runat="server"
              TextMode="MultiLine"
              Rows="5"
              placeholder="Enter Remarks..." />
      </td>
  </tr>
</table>

<div class="action-footer">
    <asp:LinkButton
        ID="LinkButton1"
        runat="server"
        CssClass="btn-cancel"
        CausesValidation="false"
        OnClientClick="closeDialog(); return false;">
        Cancel
    </asp:LinkButton>

    <asp:LinkButton
        ID="LinkButton2"
        runat="server"
        CssClass="btn-save"
        OnClientClick="return validateDescription();"
        OnClick="btnOK_Click">
        Save
    </asp:LinkButton>
</div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        // ========== Validation ==========
        function validateDescription() {
            var txt = document.getElementById('<%= txtDescription.ClientID %>');
            if (!txt.value.trim()) {
                txt.classList.add("invalid");
                return false;
            } else {
                txt.classList.remove("invalid");
                return true;
            }
        }

        // ========== Searchable dropdown - Type Code ==========
        function showTypeCodePanel() {
            var panel = document.getElementById('typeCodePanel');
            if (panel) {
                panel.style.display = 'block';
                populateTypeCodePanel();
            }
        }

        function populateTypeCodePanel(filterText) {
            var ddl = document.getElementById('<%= ddlTypeCode.ClientID %>');
            var panel = document.getElementById('typeCodePanel');
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
                    selectTypeCode(this.getAttribute('data-value'), this.innerText);
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

        function filterTypeCodeOptions() {
            var input = document.getElementById('typeCodeSearchInput');
            populateTypeCodePanel(input.value);
            showTypeCodePanel();
        }

        function selectTypeCode(value, text) {
            var ddl = document.getElementById('<%= ddlTypeCode.ClientID %>');
            var input = document.getElementById('typeCodeSearchInput');
            var panel = document.getElementById('typeCodePanel');

            if (ddl) ddl.value = value;
            if (input) input.value = text;
            if (panel) panel.style.display = 'none';
        }

        // Close panel when clicking outside
        document.addEventListener('click', function (e) {
            var wrapper = document.getElementById('typeCodeDropdownWrapper');
            var panel = document.getElementById('typeCodePanel');
            if (wrapper && panel && !wrapper.contains(e.target)) {
                panel.style.display = 'none';
            }
        });

        // Keep visible input in sync after UpdatePanel postback
        function syncDropdownDisplays() {
            var ddl = document.getElementById('<%= ddlTypeCode.ClientID %>');
            var input = document.getElementById('typeCodeSearchInput');
            if (ddl && input && ddl.selectedIndex >= 0) {
                input.value = ddl.options[ddl.selectedIndex].text;
            }
        }

        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                syncDropdownDisplays();
            });
        }

        document.addEventListener('DOMContentLoaded', function () {
            syncDropdownDisplays();
        });
    </script>
</asp:Content>