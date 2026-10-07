<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_EmployeeDetails.ascx.cs" 
    Inherits="DynamicsPortal.DropDownList_EmployeeDetails" %>

<div class="usercontrol-dropdown" style="display: inline-block; overflow: visible;">
    <div style="display: inline-block; position: relative;">
        <a class="toggle-dropdown" style="position: relative; display: block;">
            <asp:TextBox ID="txtEmployeeId" CssClass="form-control autocomplete-input" 
                ReadOnly="true" runat="server" 
                style="padding-right: 30px; cursor: pointer;"></asp:TextBox>
            <span class="dropdown-arrow">&#9660;</span>
        </a>
    </div>

    <div id="mddEmployeeDetails" runat="server" class="dropdown-panel">
        <!-- Search Box -->
        <div class="dropdown-search-container" style="padding: 10px; border-bottom: 1px solid #ddd;">
            <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search..." runat="server" 
                onkeyup="filterGridView(this.value)"></asp:TextBox>
        </div>

        <!-- Scrollable area -->
        <div id="employeeScrollContainer" style="max-height: 320px; overflow-y: auto;">
            <asp:GridView ID="gvEmployeeDetails" CssClass="table no-border table-hover" runat="server" 
                AutoGenerateColumns="false"
                OnSelectedIndexChanged="gvEmployeeDetails_SelectedIndexChanged" 
                OnRowDataBound="gvEmployeeDetails_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="EmployeeId" HeaderText="Emp Id" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                    <asp:BoundField DataField="Currency" HeaderText="Currency" Visible="false" />
                </Columns>
            </asp:GridView>

            <!-- Loading indicator (added) -->
            <div id="loadingMoreEmployees" style="display:none; text-align:center; padding:10px; color:#666;">
                Loading more...
            </div>
            <div id="noMoreEmployees" style="display:none; text-align:center; padding:8px; color:#999; font-size:12px;">
                No more records
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    // ========== YOUR ORIGINAL FUNCTIONS (kept as-is) ==========
    function filterGridView(searchText) {
        searchText = searchText.toLowerCase();
        var grid = document.getElementById('<%= gvEmployeeDetails.ClientID %>');
        if (!grid) return;
        var rows = grid.getElementsByTagName('tr');
        for (var i = 1; i < rows.length; i++) {
            var row = rows[i];
            var cells = row.getElementsByTagName('td');
            var found = false;
            for (var j = 0; j < cells.length; j++) {
                var cellText = cells[j].textContent || cells[j].innerText;
                if (cellText.toLowerCase().indexOf(searchText) > -1) {
                    found = true;
                    break;
                }
            }
            row.style.display = found ? '' : 'none';
        }
    }

    function selectEmployee(panelId, postbackScript) {
        var panel = document.getElementById(panelId);
        if (panel) {
            panel.style.display = 'none';
        }
        window.suppressOverlay = true;
        setTimeout(function () {
            eval(postbackScript);
        }, 0);
    }
    // ==========================================================

    // ========== NEW CODE FOR INFINITE SCROLL + PAGING ==========
    (function () {
        var currentPage = 1;
        var isLoading = false;
        var hasMore = true;
        var pageSize = 5;
        var scrollContainer = null;

        // Initialize after page load
        Sys.Application.add_load(function () {
            scrollContainer = document.getElementById('employeeScrollContainer');
            if (scrollContainer) {
                scrollContainer.onscroll = function () {
                    if (isLoading || !hasMore) return;

                    // When user reaches near the bottom
                    if (scrollContainer.scrollTop + scrollContainer.clientHeight >= scrollContainer.scrollHeight - 50) {
                        loadNextPage();
                    }
                };
            }
        });

        function loadNextPage() {
            if (isLoading || !hasMore) return;
            isLoading = true;

            document.getElementById('loadingMoreEmployees').style.display = 'block';
            document.getElementById('noMoreEmployees').style.display = 'none';

            var searchText = document.getElementById('<%= txtSearch.ClientID %>').value || '';

            PageMethods.GetEmployees(searchText, currentPage + 1, function (result) {
                isLoading = false;
                document.getElementById('loadingMoreEmployees').style.display = 'none';

                if (!result || !result.items || result.items.length === 0) {
                    hasMore = false;
                    document.getElementById('noMoreEmployees').style.display = 'block';
                    return;
                }

                // Append new rows to the existing GridView
                var grid = document.getElementById('<%= gvEmployeeDetails.ClientID %>');
                var tbody = grid.tBodies[0] || grid;

                result.items.forEach(function (item) {
                    var tr = document.createElement('tr');
                    tr.style.cursor = 'pointer';
                    tr.innerHTML = '<td>' + item.EmployeeId + '</td><td>' + (item.EmployeeName || '') + '</td>';

                    // Make the new row clickable (same as original rows)
                    tr.onclick = function () {
                        // You can trigger selection here if needed
                        // For now we just let the user click (or enhance later)
                    };

                    tbody.appendChild(tr);
                });

                currentPage++;
                hasMore = result.more === true;

                if (!hasMore) {
                    document.getElementById('noMoreEmployees').style.display = 'block';
                }
            }, function () {
                isLoading = false;
                document.getElementById('loadingMoreEmployees').style.display = 'none';
            });
        }
    })();
    // ==========================================================
</script>

<style>
    .dropdown-search-container {
        background-color: #f8f9fa;
        padding: 8px;
        box-sizing: border-box;
    }
    .dropdown-search-container .form-control {
        width: 100% !important; 
        max-width: 100%;
        border-radius: 4px;
        box-sizing: border-box;
        margin: 0;
    }
    .dropdown-panel {
        max-height: 400px;  
        overflow-y: auto;
        box-sizing: border-box;
    }
</style>