
//$(document).ready(function () {
//    //$("#table_one").on("click", "tr", function () {
//    //    $(".active").removeClass("active");
//    //    $(this).children("td").addClass("active");
//    //    $(this).children("tr").addClass("active");
//    //});
//});


//function preventMultipleSubmissions() {
//    //$('').prop('disabled', true);
//}
//window.onbeforeunload = preventMultipleSubmissions;

//function preventMultipleSubmissions() {
//    $('[id*=btn]').prop('disabled', true);
//}
//window.onbeforeunload = preventMultipleSubmissions;


$(document).ready(function () {
    //var clicks = 0;
    //$("input[id*='btn']").click(function (e) {
    //    clicks = clicks + 1;
    //    alert(clicks);
    //    if (clicks === 1)
    //        return true;
    //    else {
    //        e.disabled = 'true';
    //        return false;
    //    }
    //});

    try {
        $("input[id*='btnDelete']").click(function (e) {
            if (confirm("Do you want to delete selected record(s)?"))
                e.preventDefault();
        });
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
});


try {
    $(window).on('load', function () {
        //$(window.parent.$(".preloader")).fadeOut("1000");
        $(".preloader").fadeOut("1000");
    });
}
catch (ex) {
    var errorMsg = ex;
    //alert(errorMsg);
}

//////////////////////Search START////////////////////////////////////
(function (document) {
    try {
        'use strict';
        var tableFilter = (function (Arr) {
            var _input;
            function _onInputEvent(e) {
                _input = e.target;

                $('table[id*=gridView]').each(function (n, table) {
                    var searchable = table.getAttribute('data');//.hasAttribute('searchable');
                    //debugger;
                    if (searchable && searchable.search(/\bsearchable\b/) !== -1) {
                        Arr.forEach.call(table.tBodies, function (tbody) {
                            Arr.forEach.call(tbody.rows, _filter);
                        });
                    }
                });

                //var tables = document.get.getElementsByClassName(_input.getAttribute('data-table'));//document.querySelectorAll("[xlink|href='"+id+"']");
                //Arr.forEach.call(tables, function (table) {
                //    Arr.forEach.call(table.tBodies, function (tbody) {
                //        Arr.forEach.call(tbody.rows, _filter);
                //    });
                //});
            }

            function _filter(row) {
                var text = row.textContent.toLowerCase(), val = _input.value.toLowerCase();
                row.style.display = text.indexOf(val) === -1 ? 'none' : 'table-row';
            }

            return {
                init: function () {
                    var inputs = document.getElementsByClassName('search-input');
                    Arr.forEach.call(inputs, function (input) {
                        input.oninput = _onInputEvent;
                    });
                }
            };
        })(Array.prototype);

        document.addEventListener('readystatechange', function () {
            if (document.readyState === 'complete') {
                tableFilter.init();
            }
        });
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }

})(document);

$(document).ready(function () {
    try {
        var showSearch = false;
        $('table[id*=gridView]').each(function (n, table) {
            var searchable = table.getAttribute('data');
            if (searchable && searchable.search(/\bsearchable\b/) !== -1) {
                showSearch = true;
            }
        });
        if (showSearch) {
            $('.search-input').show();
            //$("[id*='searchPanel']").show();
        }
        else {
            $('.search-input').hide();
            //$("[id*='searchPanel']").hide();
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
});

//////////////////////Search END////////////////////////////////////

function refreshPage() {
    try {
        //alert(document.URL);
        window.location = document.URL.replace('#', '');
        //window.location.reload();           //javascript: document.location.reload(true);
        //var newResult = t.substring(0, t.lastIndexOf("#"));
    }
    catch (err) {
        //err.message;
    }
}
function toggleFullscreen() {
    try {
        var elem = document.getElementById("form1");
        elem = elem || document.documentElement;
        if (!document.fullscreenElement && !document.mozFullScreenElement &&
            !document.webkitFullscreenElement && !document.msFullscreenElement) {
            if (elem.requestFullscreen) {
                elem.requestFullscreen();
            } else if (elem.msRequestFullscreen) {
                elem.msRequestFullscreen();
            } else if (elem.mozRequestFullScreen) {
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) {
                elem.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            }
        } else {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            }
        }
    }
    catch (err) {
        //err.message;
    }
}

document.onkeydown = function (evt) {
    try {
        evt = evt || window.event;
        var isEscape = false;
        if ("key" in evt) {
            isEscape = (evt.key === "Escape" || evt.key === "Esc");
        } else {
            isEscape = (evt.keyCode === 27);
        }
        if (isEscape) {
            closeDialog();
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
};

function reloadParent() {
    try {
        var parent = window.parent;
        //debugger;
        if (parent) {
            parent.location.reload();
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
    return false;
}

function closeDialog() {
    try {
        var parent = window.parent;
        $(parent.$("#modal_dialog iframe")).attr({ src: '' });
        //$(parent.$('iframe')).attr('src', '');
        parent.$('#form1').removeClass("blur-filter");

        parent.$('.ui-dialog-content:visible').dialog('destroy');
        //$(parent.$('.ui-widget-overlay:visible')).remove();
        //$(parent.$('.ui-dialog-content:visible')).dialog('destroy');
        //$(parent.$('.ui-dialog:visible')).dialog('destroy');

        //$(parent.$('.ui-dialog-content:visible')).remove();
    }
    catch (ex) {
        var errorMsg = ex;
        parent.$('.ui-dialog:visible').remove();
        parent.$('.ui-widget-overlay:visible').remove();
        //alert(errorMsg);
    }
    return false;
}

function openPopupPanel(_Panel, _width) {
    try {
        var width = 380;
        if (_width) {
            width = _width;
        }
        var parent = window.parent;
        $(parent.$("#modal_dialog iframe")).attr({ src: _Panel });
        $(parent.$("#modal_dialog")).dialog({
            //title: "Select the Product Code to fill the Tire size",
            //css: { border: 'Yellow' },
            autoOpen: true,
            draggable: false,
            width: width,
            resizable: false,
            modal: true,
            closeOnEscape: true,
            position: {
                my: "right center",
                at: "right bottom"
            },
            show: {
                effect: "slideLeft",
                duration: 2000
            },
            close: function (event, ui) {
                $(this).dialog('destroy').remove();
            }
            //,buttons: {
            //    Close: function () {
            //        $(this).dialog("close");
            //        $(this).find("#modal_dialog iframe").attr({ src: "" });
            //        $(this).dialog('destroy').remove();
            //        $('form').removeClass("blur-filter");
            //        RefreshPage();
            //    }
            //}
        }).dialog("open");
        //$(parent.$('form'))
        $(parent.$('#form1')).addClass("blur-filter");
        $(parent.$(".ui-dialog-titlebar")).hide();
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
    return false;
}


function closePopupPanel(args) {
    var $btn = args;
    //debugger;
}


function showNotificationMessage(_errorMsg, _alertType, _parent) {
    try {
        var errorMsg = _errorMsg;
        var alertType = _alertType.toLowerCase();
        //showNotificationMessage('20 Record(s) are deleted.', 'success')
        var $notificationPanel;
        if (_parent === 'True' || _parent === 'true') {
            $notificationPanel = $(window.parent.$('#notificationPanel'));
            closeDialog();
        }
        else {
            $notificationPanel = $('#notificationPanel');
        }

        var cssclass;
        switch (alertType) {
            case 'success':
                cssclass = ' success';
                break;
            case 'error':
                cssclass = ' error';
                break;
            case 'warning':
                cssclass = ' warning';
                break;
            default:
                cssclass = ' info';
        }

        $notificationPanel.show();
        $notificationPanel.addClass('' + cssclass + '');

        var msgBox = $notificationPanel.find('p');
        msgBox.html(errorMsg);
        //var oldMsg = msgBox.html();
        //msgBox.html(oldMsg + errorMsg);
        //$('#notificationPanel .notification-container').append('<p>Test</p>');
    }
    catch (ex) {
        errorMsg = ex;
        //alert(errorMsg);
    }
}


function gridPaginationSorting(_gridName, _gridPagination, _gridRecordsPerPage, _sortColumn, _sortDirection) {
    try {
        //debugger;
        if (_sortColumn && _sortDirection) {
            var gridSortableValues = '{"gridName":"' + _gridName + '", "sortColumn":"' + _sortColumn + '", "sortDirection":"' + _sortDirection + '"}';
            $("input[name*='hdnGridSortable']").val(gridSortableValues);
        }

        if (_gridName && _gridRecordsPerPage) {
            var gridPaginationValues = '{"gridName":"' + _gridName + '", "gridPagination":"' + _gridPagination + '", "gridRecordsPerPage":"' + _gridRecordsPerPage + '"}';
            $("input[name*='hdnGridPaginationable']").val(gridPaginationValues);
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
}


//$('#userMenuItems_ESS a').on('click', function () {
$(document).ready(function () {
    /*
    <div class="breadcrumb-item" > <a href="Default.aspx">Home</a></div>
    <div class="breadcrumb-item">Other Page</div>
    <div class="breadcrumb-item">Page</div>
    <div class="breadcrumb-item">Page</div>
    <div class="breadcrumb-item active">Test Page</div>
    */
    /*
         $('.items a').on('click', function() {
          var $this = $(this),
              $bc = $('<div class="item"></div>');

          $this.parents('li').each(function(n, li) {
              var $a = $(li).children('a').clone();
              $bc.prepend(' / ', $a);
          });
            $('.breadcrumb').html( $bc.prepend('<a href="#home">Home</a>') );
            return false;
        })
     */
    try {
        var currentPage = window.location.pathname;//.split("/").pop()/ESS/HR/ESSHRPersonalDetails.aspx
        var containsHomePage = false;
        var $menu = $(".main-menu");
        var $menuItem = $menu.find('a[href="' + currentPage + '"]');
        var $breadcrumbMenu = $('<div class="breadcrumb"></div>');//$('.breadcrumb');<div id="breadcrumbPanel" class="navbar-brand d-none d-md-inline-block">
        if ($menuItem.length > 0) {
            //var menuItemURL = $menuItem[0].href;    //.pathname;
            //var menuItemLabel = $menuItem[0].text;
            ////$menuItem[0].text
            //$breadcrumbMenu.prepend($('<div class="breadcrumb-item"><a href="' + menuItemURL + '">' + menuItemLabel + '</a></div>'));

            $menuItem.parents('li').each(function (n, li) {
                if (($(li).closest("ul").attr('id')) !== "mddFav" && ($(li).closest("ul").attr('id')) !== "userMenuItems_fav") {

                    var $a = $(li).children('a').clone();
                    if ($a.length > 0) {
                        var currentURL = $a[0].href;
                        //if (currentURL !== menuItemURL)Dashboard
                        if (currentURL.indexOf('Default') > -1) {
                            containsHomePage = true;
                        }
                        if (currentURL.indexOf('#') > -1) {
                            $breadcrumbMenu.prepend($('<div class="breadcrumb-item"><a>' + $a[0].text + '</a></div>'));
                            //$breadcrumbMenu.prepend($('<div class="breadcrumb-item">' + $a[0].text + '</div>'));
                        }
                        else {
                            $breadcrumbMenu.prepend($('<div class="breadcrumb-item"><a href="' + $a[0].href + '">' + $a[0].text + '</a></div>'));
                        }

                        //($($menuItem.parents('li')[0]).children('a')[0]).href

                        //$breadcrumbMenu.prepend($('<div class="breadcrumb-item">' + $a[0].text + '</div>'));
                        ////$breadcrumbMenu.prepend($('<div class="breadcrumb-item"><a href="' + $a[0].pathname + '">' + $a[0].text + '</a></div>'));
                    }
                    ////else {
                    ////$breadcrumbMenu.prepend($('<div class="breadcrumb-item">' + $(li)[0].text + '</div>'));
                    ////}

                }
            });

            if (!containsHomePage) {
                $breadcrumbMenu.prepend('<div class="breadcrumb-item"><a href="' + window.location.origin + '/Default.aspx">ESS Home</a></div>');
            }

            $('#breadcrumbPanel').html($breadcrumbMenu);
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
});







$("input[id*='txtRequestedInstallmentAmount']").keyup(function () {
    try {
        var inputVal = $(this).val();

        if (inputVal.length > 0 && inputVal > 0) {
            $("input[id*='txtRequestedInstallments']").val('').prop('disabled', true);
        }
        else {
            $("input[id*='txtRequestedInstallments']").prop('disabled', false);
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
});

$("input[id*='txtRequestedInstallments']").keyup(function () {
    try {
        var inputVal = $(this).val();

        if (inputVal.length > 0 && inputVal > 0) {
            $("input[id*='txtRequestedInstallmentAmount']").val('').prop('disabled', true);
        }
        else {
            $("input[id*='txtRequestedInstallmentAmount']").prop('disabled', false);
        }
    }
    catch (ex) {
        var errorMsg = ex;
        //alert(errorMsg);
    }
});


$(document).ready(function () {
    $("input[id*='chk_SelectAll']").click(function () {
        try {
            //$("li").not(":visible")
            //$($(this).closest('table[id*=gridView]').find('tr[style!="display: none"]').find("input[id*='chk_SelectSingle']")).prop('checked', this.checked);
            var chkSellectAll = this;
            $($(chkSellectAll).closest('table[id*=gridView]').find('tr[style!="display: none"]').find("input[id*='chk_SelectSingle']")).each(function (n, element) {
                var nonactionable = (element).parentElement.parentElement.hasAttribute('nonactionable');
                var disabled = (element).hasAttribute('disabled');
                if (!nonactionable && !disabled)
                    element.checked = chkSellectAll.checked;//$(element).prop('checked', this.checked);
            });
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }

    });




    $("span[masktype=number]").each(function (n, element) {
        try {
            element.innerText = element.innerText.toString().match(/^-?\d+(?:\.\d{0,2})?/)[0];
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });

    $("input[masktype=number]").keyup(function () {
        try {
            var inputVal = $(this).val();
            var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
            if (!numericReg.test(inputVal)) {
                $(this).css("border", "red solid 1px");//.css("border-color", "red");
            }
            else {
                $(this).css("border", "green solid 1px");//.css("border-color", "green");
            }
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });

    $("span[masktype=enum]").each(function (n, element) {
        try {
            element.innerText = element.innerText.replace(/([A-Z])/g, ' $1').trim();
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });

    //$("span[masktype=datetime]").each(function (n, element) {
    //    //labelDate.innerText = ;
    //});
    var doptions = { year: 'numeric', month: '2-digit', day: '2-digit' };
    var dtoptions = { year: 'numeric', month: '2-digit', day: '2-digit', hour: "2-digit", minute: "2-digit" };

    $("input[masktype=date]").each(function (n, inputDate) {
        try {
            var txtValue = inputDate.value.trim();
            if (txtValue.length > 10)
                inputDate.value = new Date("" + txtValue + "").toLocaleDateString("en-GB", doptions);

            $(inputDate).datepicker({
                autoSize: true,
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true
                //gotoCurrent: true, minDate: new Date(2007, 1 - 1, 1), selectOtherMonths: true, showMonthAfterYear: true, showOtherMonths: true, yearRange: "2002:2012",
            });
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });

    $("input[masktype=datetime]").each(function (n, inputDate) {
        try {
            var txtValue = inputDate.value.trim();
            if (txtValue.length > 10) {
                var d = new Date("" + txtValue + ""),
                    dformat = d.getDate().toString().padStart(2, 0) + "/" + (d.getMonth() + 1).toString().padStart(2, 0) + "/" + d.getFullYear().toString().padStart(2, 0) + ' ' + d.getHours().toString().padStart(2, 0) + ":" + d.getMinutes().toString().padStart(2, 0);
                //[(d.getMonth() + 1).padLeft(),
                //d.getDate().padLeft(),
                //d.getFullYear()].join('/') +
                //' ' +
                //[d.getHours().padLeft(),
                //d.getMinutes().padLeft(),
                //d.getSeconds().padLeft()].join(':');
                inputDate.value = dformat;//new Date("" + txtValue + "").toLocaleDateString("dd/mm/yy HH:mm");.toLocaleDateString("en-GB", doptions)


            }
            $(inputDate).datetimepicker({
                dateFormat: "dd/mm/yy",
                //controlType: 'select',
                //oneLine: true,
                //timeFormat: 'hh:mm',
                timeInput: true,
                timeFormat: "HH:mm",
                showHour: false,
                showMinute: false,
                showSecond: false,
                showMillisec: false,
                showMicrosec: false,
                showTimezone: false
            });

        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });

    //day:     "numeric", "2-digit".
    //month:   "numeric", "2-digit", "narrow", "short", "long".
    //year:    "numeric", "2-digit".
    //hour:    "numeric", "2-digit".
    //minute:  "numeric", "2-digit".
    //second:  "numeric", "2-digit".
    //weekday: "narrow", "short", "long".
    $("span[masktype=date]").each(function (n, labelDate) {
        try {
            labelDate.innerText = new Date("" + labelDate.innerText + "").toLocaleDateString("en-GB", doptions);  //en-GB     dd/mm/yy    //en-US     mm/dd/yy
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });


    $("span[masktype=datetime]").each(function (n, labelDate) {
        try {

            var txtValue = labelDate.innerText.trim();
            if (txtValue.length > 10) {
                var d = new Date("" + txtValue + ""),
                    dformat = d.getDate().toString().padStart(2, 0) + "/" + (d.getMonth() + 1).toString().padStart(2, 0) + "/" + d.getFullYear().toString().padStart(2, 0) + ' ' + d.getHours().toString().padStart(2, 0) + ":" + d.getMinutes().toString().padStart(2, 0);

                labelDate.innerText = dformat;//new Date("" + labelDate.innerText + "").toLocaleDateString("dd/MM/yyyy HH:mm", dtoptions);  //en-GB     dd/mm/yy    //en-US     mm/dd/yy
            }
        }
        catch (ex) {
            var errorMsg = ex;
            //alert(errorMsg);
        }
    });
});


//function jsChartAdvances(_chartLabels, _chartData) {
//    'use strict';
//    var chartLabels = _chartLabels;//["Taken", "Outstanding"];
//    var chartData = _chartData;//[25000, 12000];
//    alert(chartLabels);
//    var brandPrimary = '#33b35a';
//    var PIECHART = $('#chartAdvances');
//    var myPieChart = new Chart(PIECHART, {
//        type: 'doughnut',
//        data: {
//            labels: chartLabels,
//            datasets: [
//                {
//                    data: chartData,
//                    borderWidth: [1, 1],
//                    backgroundColor: [
//                        "rgba(75,192,192,1)",
//                        "#FFCE56"
//                    ],
//                    hoverBackgroundColor: [
//                        "rgba(75,192,192,1)",
//                        "#FFCE56"
//                    ]
//                }]
//        }
//    });

//}








