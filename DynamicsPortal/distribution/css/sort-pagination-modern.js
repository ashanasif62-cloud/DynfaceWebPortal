/**
 * sort-pagination-modern.js
 * Handles pagination only.
 * Sorting and filtering are handled by listpage-modern.js — do NOT duplicate here.
 */

/* ═══════════════════════════════════════════════════════════════
   SAFE UTILITIES
═══════════════════════════════════════════════════════════════ */
function dean_addEvent(element, type, handler) {
    if (element.addEventListener) {
        element.addEventListener(type, handler, false);
    }
}

function removeEvent(element, type, handler) {
    if (element.removeEventListener) {
        element.removeEventListener(type, handler, false);
    }
}

/* ═══════════════════════════════════════════════════════════════
   PAGINATION
   Same signature as old file:
   pagination(tableId, selectedPageNo, recordPerPage, createNew)
═══════════════════════════════════════════════════════════════ */
function pagination(_tableId, _selectedPageNo, _recordPerPage, _createNew) {
    var tableId = '#' + _tableId;
    var recPerPage = parseInt(_recordPerPage, 10) || 20;
    var $table = $(tableId);
    if (!$table.length) return;

    var $parent = $table.parent();
    var $pages = $parent.find('#pages');

    /* Remove old pagination if rebuilding */
    if (_createNew === 'true') {
        $pages.remove();
        $pages = $();
    }

    /* Only count rows not hidden by listpage-modern.js filters */
    var $allRows = $table.find('tbody tr:has(td)').filter(function () {
        return $(this).css('display') !== 'none';
    });
    var totalRows = $allRows.length;
    var totalPages = Math.ceil(totalRows / recPerPage);

    /* Build pagination bar if needed */
    if ($pages.length === 0 && totalPages > 1) {
        $pages = $('<div id="pages" class="pagination-container"></div>');

        /* Per-page selector */
        var $opt = $([
            '<div class="pagination-option">Show ',
            '<select id="ddlPagination">',
            '<option value="10">10</option>',
            '<option value="20">20</option>',
            '<option value="50">50</option>',
            '<option value="100">100</option>',
            '<option value="1000">All</option>',
            '</select> entries</div>'
        ].join(''));
        $opt.appendTo($pages);

        /* Page number buttons */
        var $nums = $('<div id="paginationNumbers" class="pagination-Numbers"></div>');
        for (var p = 0; p < totalPages; p++) {
            $('<span class="pageNumber' + (p === 0 ? ' active' : '') + '">' + (p + 1) + '</span>')
                .appendTo($nums);
        }
        $nums.appendTo($pages);
        $pages.appendTo($parent);
    }

    /* Set selector to current value */
    var $sel = $pages.find('#ddlPagination');
    $sel.val(String(recPerPage));

    /* Show first page of visible (non-filtered) rows only */
    $allRows.hide();
    $allRows.slice(0, recPerPage).show();

    /* Per-page change */
    $sel.off('change.pgn').on('change.pgn', function () {
        var rpp = parseInt($(this).val(), 10);
        pagination(_tableId, '', rpp, 'true');
        gridPaginationSorting(_tableId, '', rpp, '', '');
    });

    /* Page number click */
    $pages.find('#paginationNumbers span').off('click.pgn').on('click.pgn', function () {
        var pageNum = parseInt($(this).text(), 10);
        var begin = (pageNum - 1) * recPerPage;
        var end = pageNum * recPerPage;

        $allRows.hide();
        $allRows.slice(begin, end).show();

        $parent.find('.pageNumber').removeClass('active');
        $(this).addClass('active');

        var rpp = parseInt($sel.val(), 10);
        gridPaginationSorting(_tableId, pageNum, rpp, '', '');
    });

    /* Restore saved page if supplied */
    if (_selectedPageNo && String(_selectedPageNo).trim()) {
        var target = String(_selectedPageNo).trim();
        $pages.find('#paginationNumbers span').each(function () {
            if ($(this).text().trim() === target) {
                $(this).trigger('click.pgn');
            }
        });
    }
}

/* ═══════════════════════════════════════════════════════════════
   GRID STATE HELPER  (used by master page hidden fields)
═══════════════════════════════════════════════════════════════ */
function gridPaginationSorting(_gridName, _gridPagination, _gridRecordsPerPage, _sortColumn, _sortDirection) {
    try {
        if (_sortColumn !== '' && _sortDirection !== '') {
            var sortVals = JSON.stringify({
                gridName: _gridName,
                sortColumn: _sortColumn,
                sortDirection: _sortDirection
            });
            $("input[name*='hdnGridSortable']").val(sortVals);
        }
        if (_gridName && _gridRecordsPerPage) {
            var pageVals = JSON.stringify({
                gridName: _gridName,
                gridPagination: _gridPagination,
                gridRecordsPerPage: _gridRecordsPerPage
            });
            $("input[name*='hdnGridPaginationable']").val(pageVals);
        }
    } catch (ex) { /* silent */ }
}

/* ═══════════════════════════════════════════════════════════════
   INIT — called by Site.master's initPaginationAndSorting()
   and also on DOM ready as a fallback.
   Does NOT init sorting or filtering — listpage-modern.js owns those.
═══════════════════════════════════════════════════════════════ */
function initPaginationAndSorting() {
    $('table.sortable').each(function () {
        var $table = $(this);
        if ($table.data('pgn-initialized')) return;
        $table.data('pgn-initialized', true);

        var tableId = this.id;
        if (tableId) {
            /* Remove the data-lp-col-filters flag so listpage-modern.js
               can re-attach its own column filter/sort dropdowns cleanly */
            this.removeAttribute('data-lp-col-filters');
            pagination(tableId, '', 20);
            /* No initColumnSorting / initColumnFiltering here —
               listpage-modern.js handles both via applyColumnFilters() */
        }
    });

    // After any UpdatePanel partial postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm) {
        prm.add_endRequest(function () {
            // Clear sortable-initialized flag so tables re-initialize
            $('table.sortable').each(function () {
                $(this).removeData('sortable-initialized');
            });
            initPaginationAndSorting();
        });
    }
}

$(document).ready(function () {
    initPaginationAndSorting();
});