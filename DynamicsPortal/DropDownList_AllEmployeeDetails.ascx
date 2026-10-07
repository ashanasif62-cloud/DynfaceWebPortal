<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_AllEmployeeDetails.ascx.cs" Inherits="DynamicsPortal.DropDownList_AllEmployeeDetails" %>

<div>
    <div style="float: left; width: 100%;">
        <a href="#" onclick="showMenu(this); return false;">
            <asp:TextBox ID="txtEmployeeId" CssClass="textbox" runat="server"></asp:TextBox></a>
    </div>
    <div id="mddAllEmployeeDetails" class="dropdown-panel" style="right: 0px;">
        <asp:GridView ID="gvAllEmployeeDetails" data="searchable" CssClass="table no-border table-hover" runat="server" AutoGenerateColumns="false"
            OnSelectedIndexChanged="gvAllEmployeeDetails_SelectedIndexChanged" OnRowDataBound="gvAllEmployeeDetails_RowDataBound">
            <Columns>
                <asp:BoundField DataField="EmployeeId" HeaderText="Emp Id" />
                <asp:BoundField DataField="EmployeeName" HeaderText="Name" />
                <asp:BoundField DataField="Designation" HeaderText="Desig" />
                <asp:BoundField DataField="Department" HeaderText="Dept" />
            </Columns>
        </asp:GridView>
    </div>
</div>

<script>
    //////////////////////Search////////////////////////////////////
    //(function (document) {
    //    'use strict';

    //    var tableFilter = (function (Arr) {

    //        var _input;

    //        function _onInputEvent(e) {
    //            _input = e.target;
    //            $('table[id*=gvAllEmployeeDetails]').each(function (n, table) {
    //                var data = table.getAttribute('data');
    //                if (data && data.search(/\bsearchable\b/) !== -1) {
    //                    Arr.forEach.call(table.tBodies, function (tbody) {
    //                        Arr.forEach.call(tbody.rows, _filter);
    //                    });
    //                }
    //            });
    //        }

    //        function _filter(row) {
    //            var text = row.textContent.toLowerCase(), val = _input.value.toLowerCase();
    //            row.style.display = text.indexOf(val) === -1 ? 'none' : 'table-row';
    //        }

    //        return {
    //            init: function () {
    //                var inputs = $('input[id*=txtEmployeeId]');
    //                Arr.forEach.call(inputs, function (input) {
    //                    input.oninput = _onInputEvent;
    //                });
    //            }
    //        };
    //    })(Array.prototype);

    //    document.addEventListener('readystatechange', function () {
    //        if (document.readyState === 'complete') {
    //            tableFilter.init();
    //        }
    //    });

    //})(document);
    var _0xddf3 = ["\x75\x73\x65\x20\x73\x74\x72\x69\x63\x74", "\x70\x72\x6F\x74\x6F\x74\x79\x70\x65", "\x74\x61\x72\x67\x65\x74", "\x64\x61\x74\x61", "\x67\x65\x74\x41\x74\x74\x72\x69\x62\x75\x74\x65", "\x73\x65\x61\x72\x63\x68", "\x74\x42\x6F\x64\x69\x65\x73", "\x72\x6F\x77\x73", "\x63\x61\x6C\x6C", "\x66\x6F\x72\x45\x61\x63\x68", "\x65\x61\x63\x68", "\x74\x61\x62\x6C\x65\x5B\x69\x64\x2A\x3D\x67\x76\x41\x6C\x6C\x45\x6D\x70\x6C\x6F\x79\x65\x65\x44\x65\x74\x61\x69\x6C\x73\x5D", "\x74\x6F\x4C\x6F\x77\x65\x72\x43\x61\x73\x65", "\x74\x65\x78\x74\x43\x6F\x6E\x74\x65\x6E\x74", "\x76\x61\x6C\x75\x65", "\x64\x69\x73\x70\x6C\x61\x79", "\x73\x74\x79\x6C\x65", "\x69\x6E\x64\x65\x78\x4F\x66", "\x6E\x6F\x6E\x65", "\x74\x61\x62\x6C\x65\x2D\x72\x6F\x77", "\x69\x6E\x70\x75\x74\x5B\x69\x64\x2A\x3D\x74\x78\x74\x45\x6D\x70\x6C\x6F\x79\x65\x65\x49\x64\x5D", "\x6F\x6E\x69\x6E\x70\x75\x74", "\x72\x65\x61\x64\x79\x73\x74\x61\x74\x65\x63\x68\x61\x6E\x67\x65", "\x72\x65\x61\x64\x79\x53\x74\x61\x74\x65", "\x63\x6F\x6D\x70\x6C\x65\x74\x65", "\x69\x6E\x69\x74", "\x61\x64\x64\x45\x76\x65\x6E\x74\x4C\x69\x73\x74\x65\x6E\x65\x72"]; (function (_0x3101x1) { _0xddf3[0]; var _0x3101x2 = (function (_0x3101x3) { var _0x3101x4; function _0x3101x5(_0x3101x6) { _0x3101x4 = _0x3101x6[_0xddf3[2]]; $(_0xddf3[11])[_0xddf3[10]](function (_0x3101x7, _0x3101x8) { var _0x3101x9 = _0x3101x8[_0xddf3[4]](_0xddf3[3]); if (_0x3101x9 && _0x3101x9[_0xddf3[5]](/\bsearchable\b/) !== -1) { _0x3101x3[_0xddf3[9]][_0xddf3[8]](_0x3101x8[_0xddf3[6]], function (_0x3101xa) { _0x3101x3[_0xddf3[9]][_0xddf3[8]](_0x3101xa[_0xddf3[7]], _0x3101xb) }) } }) } function _0x3101xb(_0x3101xc) { var _0x3101xd = _0x3101xc[_0xddf3[13]][_0xddf3[12]](), _0x3101xe = _0x3101x4[_0xddf3[14]][_0xddf3[12]](); _0x3101xc[_0xddf3[16]][_0xddf3[15]] = _0x3101xd[_0xddf3[17]](_0x3101xe) === -1 ? _0xddf3[18] : _0xddf3[19] } return { init: function () { var _0x3101xf = $(_0xddf3[20]); _0x3101x3[_0xddf3[9]][_0xddf3[8]](_0x3101xf, function (_0x3101x10) { _0x3101x10[_0xddf3[21]] = _0x3101x5 }) } } })(Array[_0xddf3[1]]); _0x3101x1[_0xddf3[26]](_0xddf3[22], function () { if (_0x3101x1[_0xddf3[23]] === _0xddf3[24]) { _0x3101x2[_0xddf3[25]]() } }) })(document)
    //////////////////////Search END////////////////////////////////////
</script>

<script>
    function showMenu(args) {
        $(args).parent().parent().find(".dropdown-panel").toggleClass("show");
    }
</script>
