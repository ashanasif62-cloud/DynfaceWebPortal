<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_ProductCombination.ascx.cs" Inherits="DynamicsPortal.DropDownList_ProductCombination" %>

<div class="dropdown-container" style="position: relative; width: 100%;">
    <div class="combobox-wrapper" onclick="showMenu(this);">
        <asp:TextBox ID="txtSelectedCombination" CssClass="textbox" ReadOnly="true" runat="server" />
    </div>

    <div id="mddProductCombinations" class="dropdown-panel">
        <asp:GridView ID="gvProductCombinations" runat="server" CssClass="table no-border table-hover" AutoGenerateColumns="False" OnSelectedIndexChanged="gvProductCombinations_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="✓" />
                <asp:BoundField DataField="productDisplayName" HeaderText="Product No" />
                <asp:BoundField DataField="ConfigId" HeaderText="Config" />
                <asp:BoundField DataField="InventStyle" HeaderText="Style" />
                <asp:BoundField DataField="InventColorId" HeaderText="Color" />
                <asp:BoundField DataField="InventSizeId" HeaderText="Size" />
                <asp:BoundField DataField="ItemName" HeaderText="Name" />
            </Columns>
        </asp:GridView>
    </div>
</div>
<%--
<script src="/distribution/js/jquery-3.3.1.min.js"></script>
<script src="/distribution/vendor/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>
<script src="/distribution/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js"></script>--%>

<%--<script>
    function showMenu(element) {
        var $wrapper = $(element).closest(".combobox-wrapper");
        var $container = $wrapper.closest(".dropdown-container");
        var $dropdown = $container.find(".dropdown-panel");
        var $modalBody = $container.closest(".modal-body");
        var modalWidth = $modalBody.length ? $modalBody.innerWidth() : $(window).width();
        var padding = 20;

        $dropdown.toggleClass("show");
        $wrapper.toggleClass("open");

        if ($dropdown.hasClass("show")) {
            var dropdownWidth = modalWidth - padding;
            var textboxOffset = $wrapper.offset();
            var containerOffset = $container.offset();
            var leftPosition = textboxOffset.left - containerOffset.left - dropdownWidth + $wrapper.width();

            $dropdown.css({
                "max-width": "calc(100% - " + padding + "px)",
                "min-width": "300px",
                "left": leftPosition + "px",
                "top": "100%"
            });
        }
    }

    $(document).click(function (e) {
        if (!$(e.target).closest(".dropdown-container").length) {
            $(".dropdown-panel").removeClass("show");
            $(".combobox-wrapper").removeClass("open");
        }
    });

    $(document).ready(function () {
        $(".dropdown-panel").mCustomScrollbar({
            theme: "dark-thin",
            scrollInertia: 200,
            autoHideScrollbar: true
        });
    });
</script>

<style>
    .dropdown-container {
        position: relative;
        width: 100%;
        background-color: white;
    }

    .combobox-wrapper {
        position: relative;
        width: 100%;
        cursor: pointer;
    }

    .textbox {
        background-color: white !important;
        color: black;
        border: 1px solid #ccc;
        width: 100%;
        padding-right: 30px;
    }

    .caret-icon {
        position: absolute;
        right: 10px;
        top: 50%;
        transform: translateY(-50%);
        font-size: 12px;
        color: gray;
        pointer-events: none;
        transition: transform 0.2s ease;
    }

    .combobox-wrapper.open .caret-icon {
        transform: translateY(-50%) rotate(180deg);
    }

    .dropdown-panel {
        display: none;
        position: absolute;
        z-index: 1060;
        background-color: white;
        border: 1px solid #ccc;
        max-height: 250px;
        overflow-y: auto;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
        width: auto;
        box-sizing: border-box;
    }

    .dropdown-panel.show {
        display: block;
    }

    .dropdown-panel .table {
        width: 100%;
        margin-bottom: 0;
    }

    .modal-body {
        overflow: visible;
        position: relative;
    }

    .modal-content {
        overflow-y: auto;
        max-height: 80vh;
        padding: 10px;
    }

    .textbox:focus {
        outline: none;
        box-shadow: none;
    }
</style>--%>
