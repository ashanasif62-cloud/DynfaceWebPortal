/**
 * listpage-modern.js
 * Drop-in enhancement for DynamicsPortal list pages.
 * Features: Status badges, Prev/Next pagination, Empty state,
 *           Column widths, Row count, Per-column filter/sort dropdowns.
 */

(function () {
    'use strict';

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 1 — Status Badge Colouring
    ═══════════════════════════════════════════════════════════════════ */
    var STATUS_MAP = {
        'not submitted': 'lp-badge-notsubmitted',
        'Draft': 'lp-badge-notsubmitted',
        'NotSubmitted': 'lp-badge-notsubmitted',
        'notsubmitted': 'lp-badge-notsubmitted',
        'submitted': 'lp-badge-submitted',
        'Completed': 'lp-badge-approved',
        'completed': 'lp-badge-approved',
        'approved': 'lp-badge-approved',
        'rejected': 'lp-badge-rejected',
        'pending': 'lp-badge-pending',
        'pending approval': 'lp-badge-pending',
        'pending review': 'lp-badge-pending',
        'PendingApproval': 'lp-badge-pending',
        'pendingapproval': 'lp-badge-pending',
        'cancelled': 'lp-badge-notsubmitted',
        'closed': 'lp-badge-notsubmitted',
        'open': 'lp-badge-approved',
        'active': 'lp-badge-submitted',
        'inactive': 'lp-badge-notsubmitted'
    };

    function applyStatusBadges() {
        var cells = document.querySelectorAll('table[id*="gridView"] tbody td');
        cells.forEach(function (td) {
            if (td.querySelector('.lp-badge')) return;
            if (td.querySelector('input, select, a, button')) return;
            var raw = (td.textContent || td.innerText || '').trim();
            var key = raw.toLowerCase();
            if (STATUS_MAP[key]) {
                td.innerHTML = '<span class="lp-badge ' + STATUS_MAP[key] + '">' +
                    escapeHtml(toSentenceCase(raw)) + '</span>';
            }
        });
    }

    function escapeHtml(s) {
        return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }

    /* ── Format cell display values ── */
    function formatCellValue(td) {
        if (td.querySelector('.lp-badge, input, select, a, button')) return;

        var raw = (td.textContent || td.innerText || '').trim();
        if (!raw) return;

        /* ── Date: strip time, reformat as M/d/yyyy ── */
        var dateM = raw.match(/^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})(?:\s+.*)?$/);
        var dateY = raw.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})(?:\s+.*)?$/);

        if (dateM) {
            // dd/MM/yyyy → M/d/yyyy
            var d = parseInt(dateM[1], 10);
            var mo = parseInt(dateM[2], 10);
            var yr = dateM[3];
            td.textContent = mo + '/' + d + '/' + yr;
            return;
        }
        if (dateY) {
            // yyyy-MM-dd → M/d/yyyy
            var mo = parseInt(dateY[2], 10);
            var d = parseInt(dateY[3], 10);
            var yr = dateY[1];
            td.textContent = mo + '/' + d + '/' + yr;
            return;
        }

        /* ── Integer: add thousand separator ── */
        if (/^-?\d+$/.test(raw)) {
            // ✅ Skip if it's already classified as an ID column
            // or if it has no siblings (single-column edge case)
            if (td.classList.contains('lp-col-id')) return;
            // Fallback for when classifyColumns hasn't run yet
            var tdIndex = Array.from(td.parentElement.children).indexOf(td);
            if (tdIndex === 1 && !td.closest('table').querySelector('td.lp-col-id')) return;
            // td.textContent = parseInt(raw, 10).toLocaleString();
            td.textContent = toSentenceCase(raw);
            return;
        }
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 2 — Prev / Next Pagination
    ═══════════════════════════════════════════════════════════════════ */
    function applyPrevNext() {
        document.querySelectorAll('.pagination-Numbers').forEach(function (pn) {
            if (pn.dataset.lpWrapped) return;
            pn.dataset.lpWrapped = '1';

            var prev = document.createElement('span');
            prev.className = 'lp-page-prev';
            prev.innerHTML = '<i class="mdi mdi-chevron-left"></i> Prev';
            prev.addEventListener('click', function () {
                if (prev.classList.contains('disabled')) return;
                var active = pn.querySelector('.pageNumber.active, span.active');
                if (active) {
                    var p = active.previousElementSibling;
                    while (p && !p.classList.contains('pageNumber')) p = p.previousElementSibling;
                    if (p) p.click();
                }
            });

            var next = document.createElement('span');
            next.className = 'lp-page-next';
            next.innerHTML = 'Next <i class="mdi mdi-chevron-right"></i>';
            next.addEventListener('click', function () {
                if (next.classList.contains('disabled')) return;
                var active = pn.querySelector('.pageNumber.active, span.active');
                if (active) {
                    var n = active.nextElementSibling;
                    while (n && !n.classList.contains('pageNumber')) n = n.nextElementSibling;
                    if (n) n.click();
                }
            });

            pn.parentNode.insertBefore(prev, pn);
            pn.parentNode.insertBefore(next, pn.nextSibling);
            syncPrevNext(pn, prev, next);
            pn.addEventListener('click', function () {
                setTimeout(function () { syncPrevNext(pn, prev, next); }, 80);
            });
        });
    }

    function syncPrevNext(pn, prev, next) {
        var active = pn.querySelector('.pageNumber.active, span.active');
        if (!active) return;
        var hasPrev = false, hasNext = false;
        var s = active.previousElementSibling;
        while (s) { if (s.classList && s.classList.contains('pageNumber')) { hasPrev = true; break; } s = s.previousElementSibling; }
        s = active.nextElementSibling;
        while (s) { if (s.classList && s.classList.contains('pageNumber')) { hasNext = true; break; } s = s.nextElementSibling; }
        prev.classList.toggle('disabled', !hasPrev);
        next.classList.toggle('disabled', !hasNext);
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 3 — Empty State
    ═══════════════════════════════════════════════════════════════════ */
    function applyEmptyState() {
        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            var tbody = tbl.querySelector('tbody');
            if (!tbody) return;
            var visibleRows = Array.from(tbody.querySelectorAll('tr')).filter(function (r) {
                return r.style.display !== 'none' && !r.querySelector('th') && r.querySelectorAll('td').length > 1;
            });
            var existing = tbl.parentNode.querySelector('.lp-empty-state');
            if (visibleRows.length === 0) {
                if (!existing) {
                    var es = document.createElement('div');
                    es.className = 'lp-empty-state';
                    es.innerHTML = '<i class="mdi mdi-file-search-outline"></i><p>No records found</p>';
                    tbl.parentNode.insertBefore(es, tbl.nextSibling);
                }
                if (tbody) tbody.style.display = 'none';   // in the empty branch
                tbl.style.display = '';                     // keep table always visible
            } else {
                if (existing) existing.remove();
                if (tbody) tbody.style.display = '';        // in the restore branch
            }
        });
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 4 — Column Min-Width
    ═══════════════════════════════════════════════════════════════════ */
    function applyColumnWidths() {
        document.querySelectorAll('table[id*="gridView"] th').forEach(function (th) {
            if (th.dataset.lpSized) return;
            th.dataset.lpSized = '1';
            var text = (th.innerText || '').trim();
            var len = text.length;
            var minW;
            if (text === 'Employee' || text === 'Employee Name') minW = 140;
            else if (len === 0) minW = 40;
            else if (len < 10) minW = len * 8 + 40;
            else minW = len * 7 + 60;
        });
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 5 — Row Count Label
    ═══════════════════════════════════════════════════════════════════ */
    function applyRowCount() {
        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            var tbody = tbl.querySelector('tbody');
            if (!tbody) return;
            var rows = Array.from(tbody.querySelectorAll('tr')).filter(function (r) {
                return r.style.display !== 'none' && !r.querySelector('th') && r.querySelectorAll('td').length > 1;
            });
            var container = tbl.parentNode && tbl.parentNode.querySelector('.pagination-container');
            if (!container) return;
            var label = container.querySelector('.lp-row-count');
            if (!label) {
                label = document.createElement('span');
                label.className = 'lp-row-count';
                label.style.cssText = 'font-size:11px;color:var(--lp-text-secondary);font-family:var(--lp-font);margin-left:auto;padding-right:4px;';
                container.appendChild(label);
            }
            label.textContent = rows.length + ' record' + (rows.length !== 1 ? 's' : '');
        });
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 6 — Per-Column Filter / Sort Dropdowns
    ═══════════════════════════════════════════════════════════════════ */

    /* Inject styles once */
    var COL_FILTER_STYLES_INJECTED = true; // styles now live in style.listpage-modern.css

    function injectColFilterStyles() {
        /* no-op — all styles moved to style.listpage-modern.css */
    }

    function detectColumnType(values) {
        var nonEmpty = values.filter(function (v) { return v.trim() !== ''; });
        if (nonEmpty.length === 0) return 'text';

        var dateCount = 0, numCount = 0;

        // Strict date regex — must have two separators and look like a real date
        var dateRx = /^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{2,4})$|^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})$/;
        // Number regex — pure numeric (with optional commas/decimals), NO letters allowed
        var numRx = /^-?[\d,]+(\.\d+)?$/;

        nonEmpty.forEach(function (v) {
            var t = v.trim().split(/\s+/)[0];
            // Must NOT contain any letters for date/number classification
            if (/[a-zA-Z]/.test(t)) return;
            if (dateRx.test(t)) dateCount++;
            else if (numRx.test(t)) numCount++;
        });

        var ratio = nonEmpty.length;
        if (dateCount / ratio > 0.6) return 'date';
        if (numCount / ratio > 0.6) return 'number';
        return 'text';
    }

    /* ── Parse a dd/MM/yyyy or MM/dd/yyyy string to Date ── */
    function parseDate(str) {
        if (!str) return null;
        // Strip time portion (e.g. "29/04/2026 12:00:00 pm" → "29/04/2026")
        var s = str.trim().split(/\s+/)[0];
        var m;
        m = s.match(/^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})$/);
        if (m) return new Date(+m[3], +m[2] - 1, +m[1]);
        m = s.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})$/);
        if (m) return new Date(+m[1], +m[2] - 1, +m[3]);
        var d = new Date(str); // fallback with original (browser can parse full datetime)
        return isNaN(d) ? null : d;
    }

    /* ── State per table ── */
    var tableStates = {};

    function getTableState(tblId) {
        if (!tableStates[tblId]) tableStates[tblId] = { filters: {}, sorts: {} };
        return tableStates[tblId];
    }

    /* ── Master apply: filter + sort all rows ── */
    function applyFiltersAndSort(tbl) {
        var tblId = tbl.id;
        var state = getTableState(tblId);
        var tbody = tbl.querySelector('tbody');
        if (!tbody) return;

        var allRows = Array.from(tbody.querySelectorAll('tr')).filter(function (r) {
            return !r.querySelector('th') && r.querySelectorAll('td').length > 1;
        });

        /* 1. Determine sort column & direction */
        var sortColIdx = -1, sortDir = 'asc', sortType = 'text';
        Object.keys(state.sorts).forEach(function (k) {
            if (state.sorts[k]) {
                sortColIdx = parseInt(k, 10);
                sortDir = state.sorts[k];
            }
        });

        /* 2. Sort rows in-place */
        if (sortColIdx >= 0) {
            var ths = tbl.querySelectorAll('thead th');
            sortType = detectColumnType(allRows.map(function (r) {
                var td = r.querySelectorAll('td')[sortColIdx];
                return td ? (td.textContent || '').trim() : '';
            }));

            allRows.sort(function (a, b) {
                var tdA = a.querySelectorAll('td')[sortColIdx];
                var tdB = b.querySelectorAll('td')[sortColIdx];
                var va = tdA ? (tdA.textContent || '').trim() : '';
                var vb = tdB ? (tdB.textContent || '').trim() : '';

                var cmp = 0;
                if (sortType === 'date') {
                    var da = parseDate(va), db = parseDate(vb);
                    cmp = (da && db) ? da - db : (da ? 1 : -1);
                } else if (sortType === 'number') {
                    cmp = (parseFloat(va.replace(/,/g, '')) || 0) - (parseFloat(vb.replace(/,/g, '')) || 0);
                } else {
                    cmp = va.localeCompare(vb, undefined, { sensitivity: 'base' });
                }
                return sortDir === 'asc' ? cmp : -cmp;
            });

            allRows.forEach(function (r) { tbody.appendChild(r); });
        }

        /* 3. Filter rows */
        allRows.forEach(function (row) {
            var visible = true;

            Object.keys(state.filters).forEach(function (colIdxStr) {
                if (!visible) return;
                var colIdx = parseInt(colIdxStr, 10);
                var f = state.filters[colIdxStr];
                var td = row.querySelectorAll('td')[colIdx];
                var cellVal = td ? (td.textContent || '').trim() : '';

                if (f.type === 'text' && f.selected && f.selected.length > 0) {
                    if (f.selected.indexOf(cellVal) === -1) visible = false;
                } else if (f.type === 'date') {
                    var cellDate = parseDate(cellVal);
                    if (f.from) {
                        var from = new Date(f.from);
                        if (!cellDate || cellDate < from) visible = false;
                    }
                    if (f.to) {
                        var to = new Date(f.to);
                        to.setHours(23, 59, 59);
                        if (!cellDate || cellDate > to) visible = false;
                    }
                } else if (f.type === 'number') {
                    var num = parseFloat(cellVal.replace(/,/g, ''));
                    if (f.from !== '' && !isNaN(f.from) && num < parseFloat(f.from)) visible = false;
                    if (f.to !== '' && !isNaN(f.to) && num > parseFloat(f.to)) visible = false;
                }
            });

            row.style.display = visible ? '' : 'none';
        });

        /* 4. Update row count */
        applyRowCount();
        applyEmptyState();
    }

    /* ── Close any open dropdown ── */
    var activeDropdown = null;
    function closeActiveDropdown() {
        if (activeDropdown) {
            activeDropdown.remove();
            activeDropdown = null;
        }
        document.querySelectorAll('table[id*="gridView"] thead th.lp-col-active')
            .forEach(function (th) { th.classList.remove('lp-col-active'); });
    }

    /* ── Position dropdown below th ── */
    function positionDropdown(dropdown, th) {
        var rect = th.getBoundingClientRect();
        var dw = 260;
        var left = rect.left;
        if (left + dw > window.innerWidth - 8) left = window.innerWidth - dw - 8;
        dropdown.style.top = (rect.bottom + 4) + 'px';
        dropdown.style.left = left + 'px';
    }

    /* ── Build and show dropdown ── */
    function showColumnDropdown(th, tbl, colIdx) {
        closeActiveDropdown();

        th.classList.add('lp-col-active');

        var tblId = tbl.id;
        var state = getTableState(tblId);
        var colLabel = (th.innerText || '').trim().replace(/[\u2191\u2193\ue000-\uf8ff]/g, '').trim();

        /* Collect all unique values in this column */
        var allRows = Array.from(tbl.querySelectorAll('tbody tr')).filter(function (r) {
            return !r.querySelector('th') && r.querySelectorAll('td').length > 1;
        });
        var allVals = allRows.map(function (r) {
            var td = r.querySelectorAll('td')[colIdx];
            return td ? (td.textContent || '').trim() : '';
        });
        var colType = detectColumnType(allVals);
        var uniqueVals = Array.from(new Set(allVals)).filter(function (v) { return v !== ''; }).sort(function (a, b) {
            if (colType === 'date') {
                var da = parseDate(a), db = parseDate(b);
                return (da && db) ? da - db : 0;
            }
            return a.localeCompare(b, undefined, { sensitivity: 'base' });
        });

        /* Current filter state */
        var fKey = String(colIdx);
        var curFilter = state.filters[fKey] || { type: colType, selected: [], from: '', to: '' };
        var curSort = state.sorts[fKey] || '';

        /* ── Build dropdown DOM ── */
        var dd = document.createElement('div');
        dd.className = 'lp-col-dropdown';
        activeDropdown = dd;

        /* Header */
        dd.innerHTML = [
            '<div class="lp-cd-header">',
            '  <span class="lp-cd-title">' + escapeHtml(colLabel) + '</span>',
            '  <button class="lp-cd-clear">Clear</button>',
            '</div>',
        ].join('');

        /* Sort buttons */
        var sortSection = document.createElement('div');
        sortSection.className = 'lp-cd-sort';

        var ascLabel, descLabel, ascIcon, descIcon;
        if (colType === 'date') {
            ascLabel = 'Oldest First'; descLabel = 'Newest First';
            ascIcon = 'mdi-sort-calendar-ascending'; descIcon = 'mdi-sort-calendar-descending';
        } else if (colType === 'number') {
            ascLabel = 'Low → High'; descLabel = 'High → Low';
            ascIcon = 'mdi-sort-numeric-ascending'; descIcon = 'mdi-sort-numeric-descending';
        } else {
            ascLabel = 'A → Z'; descLabel = 'Z → A';
            ascIcon = 'mdi-sort-alphabetical-ascending'; descIcon = 'mdi-sort-alphabetical-descending';
        }

        var btnAsc = document.createElement('button');
        btnAsc.className = 'lp-cd-sort-btn' + (curSort === 'asc' ? ' lp-sort-active' : '');
        btnAsc.innerHTML = '<i class="mdi ' + ascIcon + '"></i>' + ascLabel;

        var btnDesc = document.createElement('button');
        btnDesc.className = 'lp-cd-sort-btn' + (curSort === 'desc' ? ' lp-sort-active' : '');
        btnDesc.innerHTML = '<i class="mdi ' + descIcon + '"></i>' + descLabel;

        btnAsc.addEventListener('click', function () {
            state.sorts = {};
            state.sorts[fKey] = (curSort === 'asc') ? '' : 'asc';
            curSort = state.sorts[fKey];
            btnAsc.classList.toggle('lp-sort-active', curSort === 'asc');
            btnDesc.classList.remove('lp-sort-active');
            applyFiltersAndSort(tbl);
            updateThSortIcon(tbl);
        });

        btnDesc.addEventListener('click', function () {
            state.sorts = {};
            state.sorts[fKey] = (curSort === 'desc') ? '' : 'desc';
            curSort = state.sorts[fKey];
            btnDesc.classList.toggle('lp-sort-active', curSort === 'desc');
            btnAsc.classList.remove('lp-sort-active');
            applyFiltersAndSort(tbl);
            updateThSortIcon(tbl);
        });

        sortSection.appendChild(btnAsc);
        sortSection.appendChild(btnDesc);
        dd.appendChild(sortSection);

        /* Date range section */
        if (colType === 'date') {
            var dr = document.createElement('div');
            dr.className = 'lp-cd-daterange';
            dr.innerHTML = [
                '<label>From</label>',
                '<input type="date" class="lp-dr-from" value="' + (curFilter.from || '') + '">',
                '<label>To</label>',
                '<input type="date" class="lp-dr-to"   value="' + (curFilter.to || '') + '">',
            ].join('');
            dd.appendChild(dr);
        }

        /* Number range section */
        if (colType === 'number') {
            var nr = document.createElement('div');
            nr.className = 'lp-cd-daterange'; /* reuse date range styles */
            nr.innerHTML = [
                '<label>Min Value</label>',
                '<input type="number" class="lp-dr-from" placeholder="0" value="' + (curFilter.from || '') + '" style="width:100%;border:1px solid #edebe9;border-radius:5px;padding:5px 8px;font-size:12px;font-family:inherit;box-sizing:border-box;">',
                '<label>Max Value</label>',
                '<input type="number" class="lp-dr-to"   placeholder="∞" value="' + (curFilter.to || '') + '" style="width:100%;border:1px solid #edebe9;border-radius:5px;padding:5px 8px;font-size:12px;font-family:inherit;box-sizing:border-box;">',
            ].join('');
            dd.appendChild(nr);
        }

        /* Text search + checkbox list */
        if (colType === 'text') {
            /* Search box */
            var sw = document.createElement('div');
            sw.className = 'lp-cd-search-wrap';
            sw.innerHTML = '<i class="mdi mdi-magnify lp-cd-search-icon"></i><input type="text" class="lp-cd-search" placeholder="Search values…">';
            dd.appendChild(sw);

            /* List */
            var list = document.createElement('div');
            list.className = 'lp-cd-list';

            function buildList(filterText) {
                list.innerHTML = '';
                var filtered = uniqueVals.filter(function (v) {
                    return v.toLowerCase().indexOf(filterText.toLowerCase()) !== -1;
                });

                if (filtered.length === 0) {
                    list.innerHTML = '<div class="lp-cd-no-results">No matching values</div>';
                    return;
                }

                filtered.forEach(function (val) {
                    var item = document.createElement('div');
                    item.className = 'lp-cd-item' + (curFilter.selected.indexOf(val) !== -1 ? ' lp-cd-item-checked' : '');

                    var chk = document.createElement('input');
                    chk.type = 'checkbox';
                    chk.checked = curFilter.selected.indexOf(val) !== -1;

                    var lbl = document.createElement('span');
                    lbl.title = val;
                    lbl.textContent = val;

                    item.appendChild(chk);
                    item.appendChild(lbl);

                    item.addEventListener('click', function (e) {
                        if (e.target !== chk) chk.checked = !chk.checked;
                        var idx = curFilter.selected.indexOf(val);
                        if (chk.checked && idx === -1) {
                            curFilter.selected.push(val);
                            item.classList.add('lp-cd-item-checked');
                        } else if (!chk.checked && idx !== -1) {
                            curFilter.selected.splice(idx, 1);
                            item.classList.remove('lp-cd-item-checked');
                        }
                    });

                    list.appendChild(item);
                });
            }

            buildList('');
            sw.querySelector('.lp-cd-search').addEventListener('input', function () {
                buildList(this.value);
            });
            dd.appendChild(list);
        }

        /* Footer */
        var footer = document.createElement('div');
        footer.className = 'lp-cd-footer';
        footer.innerHTML = '<button class="lp-cd-cancel">Cancel</button><button class="lp-cd-apply">Apply</button>';
        dd.appendChild(footer);

        /* ── Wire up footer buttons ── */
        footer.querySelector('.lp-cd-cancel').addEventListener('click', function () {
            closeActiveDropdown();
        });

        footer.querySelector('.lp-cd-apply').addEventListener('click', function () {
            /* Persist filter state */
            if (colType === 'date' || colType === 'number') {
                var fromEl = dd.querySelector('.lp-dr-from');
                var toEl = dd.querySelector('.lp-dr-to');
                curFilter.from = fromEl ? fromEl.value : '';
                curFilter.to = toEl ? toEl.value : '';
            }
            curFilter.type = colType;
            state.filters[fKey] = curFilter;

            applyFiltersAndSort(tbl);
            updateThActiveIndicator(tbl, state);
            closeActiveDropdown();
        });

        /* Clear button */
        dd.querySelector('.lp-cd-clear').addEventListener('click', function () {
            delete state.filters[fKey];
            delete state.sorts[fKey];
            applyFiltersAndSort(tbl);
            updateThActiveIndicator(tbl, state);
            updateThSortIcon(tbl);
            closeActiveDropdown();
        });

        /* Stop clicks inside dropdown from bubbling to document */
        dd.addEventListener('click', function (e) { e.stopPropagation(); });

        document.body.appendChild(dd);
        positionDropdown(dd, th);

        /* Reposition on scroll/resize */
        var reposition = function () { if (activeDropdown === dd) positionDropdown(dd, th); };
        window.addEventListener('scroll', reposition, true);
        window.addEventListener('resize', reposition);
    }

    /* ── Update th to show active filter indicator ── */
    function updateThActiveIndicator(tbl, state) {
        tbl.querySelectorAll('thead th.lp-col-th').forEach(function (th, i) {
            var f = state.filters[String(i)];
            var hasFilter = false;
            if (f) {
                if (f.type === 'text' && f.selected && f.selected.length > 0) hasFilter = true;
                if ((f.type === 'date' || f.type === 'number') && (f.from || f.to)) hasFilter = true;
            }
            var dot = th.querySelector('.lp-col-dot');
            if (hasFilter && !dot) {
                var d = document.createElement('span');
                d.className = 'lp-col-dot';
                d.style.cssText = 'display:inline-block;width:6px;height:6px;border-radius:50%;background:#0078d4;margin-left:5px;vertical-align:middle;flex-shrink:0;';
                th.querySelector('.lp-col-icon').insertAdjacentElement('beforebegin', d);
            } else if (!hasFilter && dot) {
                dot.remove();
            }
        });
    }

    /* ── Update th sort icon ── */
    function updateThSortIcon(tbl) {
        var state = getTableState(tbl.id);
        tbl.querySelectorAll('thead th.lp-col-th').forEach(function (th, i) {
            var icon = th.querySelector('.lp-col-icon');
            if (!icon) return;
            var s = state.sorts[String(i)];
            if (s === 'asc') {
                icon.textContent = '\uF4BC';              /* mdi-sort-ascending  */
                th.classList.add('lp-sort-asc');
                th.classList.remove('lp-sort-desc');
            } else if (s === 'desc') {
                icon.textContent = '\uF4BD';              /* mdi-sort-descending */
                th.classList.add('lp-sort-desc');
                th.classList.remove('lp-sort-asc');
            } else {
                icon.textContent = '\uF140';              /* mdi-chevron-down    */
                th.classList.remove('lp-sort-asc', 'lp-sort-desc');
            }
        });
    }

    /* ── Attach click listeners to all th ── */
    function applyColumnFilters() {
        injectColFilterStyles();

        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            if (tbl.dataset.lpColFilters) return;
            tbl.dataset.lpColFilters = '1';

            /* ── Promote tbody header row to thead if UseAccessibleHeader was not set ── */
            if (!tbl.querySelector('thead') || tbl.querySelector('thead').children.length === 0) {
                var firstRow = tbl.querySelector('tbody tr:first-child');
                if (firstRow && firstRow.querySelector('th')) {
                    var thead = tbl.querySelector('thead') || document.createElement('thead');
                    if (!tbl.querySelector('thead')) {
                        tbl.insertBefore(thead, tbl.firstChild);
                    }
                    thead.appendChild(firstRow);
                }
            }

            var ths = tbl.querySelectorAll('thead th');
            ths.forEach(function (th, colIdx) {

                var hasCheckbox = !!th.querySelector('input[type="checkbox"]');
                var visibleText = (th.innerText || '').trim().replace(/[\uE000-\uF8FF\u2191-\u2193]/g, '').trim();
                var hasOnlyButtons = !visibleText && th.querySelectorAll('a.grid-img-btn, button').length > 0;
                var isNosort = th.classList.contains('sorttable_nosort');

                if (hasCheckbox || hasOnlyButtons || (isNosort && !visibleText)) {
                    th.classList.add('sorttable_nosort');
                    return;
                }

                th.classList.add('lp-col-th');
                th.classList.add('sorttable_nosort');

                if (!th.querySelector('.lp-col-icon')) {
                    var icon = document.createElement('span');
                    icon.className = 'lp-col-icon mdi';
                    icon.textContent = '\uF140';   // ← mdi-chevron-down (matches your reference image)
                    th.appendChild(icon);
                }

                th.addEventListener('click', function (e) {

                    // Prevent auto-triggered clicks on refresh/postback
                    if (!e.isTrusted) return;

                    e.stopPropagation();
                    if (activeDropdown && th.classList.contains('lp-col-active')) {
                        closeActiveDropdown();
                    } else {
                        var currentIdx = Array.from(tbl.querySelectorAll('thead th')).indexOf(th);
                        showColumnDropdown(th, tbl, currentIdx);
                    }
                });
            });
            applyColumnReorder(tbl);   // ← ADD THIS LINE HERE
            applyColumnChooser(tbl);
        });
    }

    /* ── Close dropdown when clicking outside ── */
    document.addEventListener('click', function (e) {
        if (activeDropdown && !activeDropdown.contains(e.target)) {
            closeActiveDropdown();
        }
    });

    /* ── Column drag-to-reorder ── */
    function applyColumnReorder(tbl) {
        var dragSrcIdx = null;

        tbl.querySelectorAll('thead th.lp-col-th').forEach(function (th) {
            th.setAttribute('draggable', 'true');

            th.addEventListener('dragstart', function (e) {
                dragSrcIdx = Array.from(tbl.querySelectorAll('thead th')).indexOf(th);
                e.dataTransfer.effectAllowed = 'move';
                th.classList.add('lp-col-dragging');
            });

            th.addEventListener('dragend', function () {
                th.classList.remove('lp-col-dragging');
                tbl.querySelectorAll('thead th').forEach(function (t) {
                    t.classList.remove('lp-col-dragover');
                });
            });

            th.addEventListener('dragover', function (e) {
                e.preventDefault();
                e.dataTransfer.dropEffect = 'move';
                tbl.querySelectorAll('thead th').forEach(function (t) {
                    t.classList.remove('lp-col-dragover');
                });
                th.classList.add('lp-col-dragover');
            });

            th.addEventListener('drop', function (e) {
                e.preventDefault();
                var dropIdx = Array.from(tbl.querySelectorAll('thead th')).indexOf(th);
                if (dragSrcIdx === null || dragSrcIdx === dropIdx) return;

                var state = getTableState(tbl.id);
                function shiftKeys(obj) {
                    var newObj = {};
                    Object.keys(obj).forEach(function(k) {
                        var kIdx = parseInt(k, 10);
                        if (kIdx === dragSrcIdx) {
                            newObj[dropIdx] = obj[k];
                        } else if (dragSrcIdx < dropIdx && kIdx > dragSrcIdx && kIdx <= dropIdx) {
                            newObj[kIdx - 1] = obj[k];
                        } else if (dragSrcIdx > dropIdx && kIdx >= dropIdx && kIdx < dragSrcIdx) {
                            newObj[kIdx + 1] = obj[k];
                        } else {
                            newObj[kIdx] = obj[k];
                        }
                    });
                    return newObj;
                }
                state.filters = shiftKeys(state.filters);
                state.sorts = shiftKeys(state.sorts);

                /* Reorder every row in thead and tbody */
                Array.from(tbl.querySelectorAll('tr')).forEach(function (row) {
                    var cells = Array.from(row.children);
                    if (!cells[dragSrcIdx] || !cells[dropIdx]) return;
                    if (dragSrcIdx < dropIdx) {
                        row.insertBefore(cells[dragSrcIdx], cells[dropIdx].nextSibling);
                    } else {
                        row.insertBefore(cells[dragSrcIdx], cells[dropIdx]);
                    }
                });

                dragSrcIdx = null;
                savePreferences(tbl); // ← ADD THIS LINE
            });
        });
    }

    /* ── Column Chooser (3-dot menu → Insert Columns panel) ── */
    /* ── Column Chooser (3-dot menu → Insert Columns panel) ── */
    function applyColumnChooser(tbl) {
        if (tbl.dataset.lpChooser) return;
        tbl.dataset.lpChooser = '1';

        /* ── Put the 3-dot button inside the LAST th so it scrolls with the table ── */
        var ths = tbl.querySelectorAll('thead th');
        var lastTh = ths[ths.length - 1];
        if (!lastTh) return;

        // Make last th a flex container so the dots sit alongside any text
        lastTh.style.cssText += 'position:relative; white-space:nowrap; padding-right:44px !important;';
        //lastTh.style.whiteSpace = 'nowrap';
        //lastTh.style.paddingRight = '44px';

        var btn = document.createElement('button');
        btn.className = 'lp-chooser-btn';
        btn.title = 'Column options';
        btn.type = 'button';
        btn.innerHTML = '<span></span><span></span><span></span>';


        // Instead, append inside the last th:
        lastTh.appendChild(btn);
        /* Give body cells in the last column room for the 3-dot button */
        var lastColIdx = Array.from(tbl.querySelectorAll('thead th')).indexOf(lastTh);
        tbl.querySelectorAll('tbody tr').forEach(function (row) {
            var td = row.children[lastColIdx];
            if (td) td.style.cssText += 'padding-right:44px !important;';
        });

        /* ── Build the menu (appended to body so it overlays correctly) ── */
        var menu = document.createElement('div');
        menu.className = 'lp-chooser-menu';
        menu.innerHTML = '<div class="lp-chooser-item" data-action="insert-cols">' +
            '<i class="mdi mdi-view-column-outline"></i> Insert Columns</div>';
        menu.style.display = 'none';
        document.body.appendChild(menu);

        menu.querySelector('[data-action="insert-cols"]').addEventListener('click', function (e) {
            e.stopPropagation();
            menu.style.display = 'none';
            openColumnPanel(tbl);
        });

        /* Toggle menu — position relative to button, not wrapper */
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            if (menu.style.display === 'none') {
                var r = btn.getBoundingClientRect();
                menu.style.top = (r.bottom + 4) + 'px';
                menu.style.left = (r.right - 180) + 'px';
                menu.style.display = 'block';
            } else {
                menu.style.display = 'none';
            }
        });

        document.addEventListener('click', function (e) {
            if (!menu.contains(e.target) && e.target !== btn && !btn.contains(e.target)) {
                menu.style.display = 'none';
            }
        });

        /* ── Insert Columns action → open panel ── */
        menu.querySelector('[data-action="insert-cols"]').addEventListener('click', function (e) {
            e.stopPropagation();
            menu.style.display = 'none';
            openColumnPanel(tbl);
        });
    }



    function openColumnPanel(tbl) {
        var existing = document.getElementById('lp-col-panel-overlay');
        if (existing) existing.remove();

        /* ── Determine which page this is to call the right WebMethod ── */
        var pageUrl = window.location.pathname; // e.g. /ESS/PR/ESSPREmployeeAdvance_ListPage.aspx

        /* ── Show loading state immediately ── */
        var overlay = document.createElement('div');
        overlay.id = 'lp-col-panel-overlay';
        overlay.className = 'lp-col-panel-overlay';

        var panel = document.createElement('div');
        panel.id = 'lp-col-panel';
        panel.className = 'lp-col-panel';
        panel.innerHTML = '<div class="lp-col-panel-header">' +
            '<span class="lp-col-panel-title"><i class="mdi mdi-view-column-outline"></i> Insert Columns</span>' +
            '<button class="lp-col-panel-close" type="button">&times;</button></div>' +
            '<p class="lp-col-panel-sub">Loading columns...</p>' +
            '<div class="lp-col-panel-list" id="lp-col-panel-list">' +
            '<div style="padding:20px;text-align:center;color:#888;"><i class="mdi mdi-loading mdi-spin"></i> Please wait...</div>' +
            '</div>' +
            '<div class="lp-col-panel-footer">' +
            '<button class="lp-col-panel-cancel" type="button">Cancel</button>' +
            '<button class="lp-col-panel-apply" type="button">Apply</button>' +
            '</div>';

        overlay.appendChild(panel);
        document.body.appendChild(overlay);

        function closePanel() { overlay.remove(); }
        panel.querySelector('.lp-col-panel-close').addEventListener('click', function (e) {
            e.stopPropagation();
            closePanel();
        });
        panel.querySelector('.lp-col-panel-cancel').addEventListener('click', function (e) {
            e.stopPropagation();
            closePanel();
        });
        overlay.addEventListener('click', function (e) {
            if (e.target === overlay) closePanel();
        });

        /* ── Fetch all available columns from the WebMethod ── */
        fetch(pageUrl + '/GetAllAvailableColumns', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: '{}'
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                var allCols = JSON.parse(data.d);

                var ths = Array.from(tbl.querySelectorAll('thead th'));

                /* ── Build rendered map — normalize label for reliable matching ── */
                var renderedMap = {};
                ths.forEach(function (th, i) {
                    var clone = th.cloneNode(true);
                    // Remove icon span and chooser button before reading text
                    clone.querySelectorAll('.lp-col-icon, .lp-chooser-btn').forEach(function (el) {
                        el.remove();
                    });
                    var label = (clone.innerText || clone.textContent || '')
                        .replace(/\s+/g, ' ')
                        .trim()
                        .toLowerCase();
                    if (label) renderedMap[label] = { th: th, idx: i };
                });

                var list = panel.querySelector('#lp-col-panel-list');
                list.innerHTML = '';
                panel.querySelector('.lp-col-panel-sub').textContent = 'Select which columns to show in the grid';

                allCols.forEach(function (col) {
                    /* Normalize WebMethod label the same way */
                    var normalizedLabel = col.Label.replace(/\s+/g, ' ').trim().toLowerCase();
                    var rendered = renderedMap[normalizedLabel];
                    var isVisible = rendered && !rendered.th.dataset.lpHidden;

                    var item = document.createElement('label');
                    item.className = 'lp-col-panel-item' + (isVisible ? ' checked' : '');

                    var chk = document.createElement('input');
                    chk.type = 'checkbox';
                    chk.checked = isVisible;
                    chk.dataset.field = col.Field;
                    chk.dataset.label = col.Label;
                    chk.dataset.colIdx = rendered ? rendered.idx : '-1';

                    var icon = document.createElement('span');
                    icon.className = 'lp-col-panel-icon mdi mdi-reorder-horizontal';

                    var text = document.createElement('span');
                    text.className = 'lp-col-panel-label';
                    text.textContent = col.Label;

                    
                    item.appendChild(chk);
                    item.appendChild(icon);
                    item.appendChild(text);
                    list.appendChild(item);

                    chk.addEventListener('change', function () {
                        item.classList.toggle('checked', chk.checked);
                    });
                });

                /* ── Apply: show/hide existing cols; alert for not-yet-rendered ── */
                panel.querySelector('.lp-col-panel-apply').addEventListener('click', function () {
                    e.stopPropagation();
                    var checks = panel.querySelectorAll('input[type="checkbox"]');
                    var allRows = Array.from(tbl.querySelectorAll('tbody tr')).filter(function (r) {
                        return !r.querySelector('th') && r.querySelectorAll('td').length > 1;
                    });

                    checks.forEach(function (chk) {
                        var idx = parseInt(chk.dataset.colIdx, 10);

                        if (idx >= 0) {
                            /* ── Column already in DOM — toggle visibility ── */
                            var th = ths[idx];
                            var show = chk.checked;
                            th.style.display = show ? '' : 'none';
                            if (show) delete th.dataset.lpHidden;
                            else th.dataset.lpHidden = '1';

                            allRows.forEach(function (row) {
                                var td = row.children[idx];
                                if (td) td.style.display = show ? '' : 'none';
                            });

                        } else if (chk.checked) {
                            /* ── New column — inject <th> and <td> from data-rowjson ── */
                            var fieldName = chk.dataset.field;
                            var fieldLabel = chk.dataset.label;

                            /* 1. Add <th> before the last th (actions column) */
                            var thead = tbl.querySelector('thead tr');
                            var lastTh = thead.querySelector('th:last-child');
                            var newTh = document.createElement('th');
                            newTh.textContent = fieldLabel;
                            newTh.classList.add('lp-col-th', 'sorttable_nosort');
                            newTh.dataset.colIdx = String(ths.length); // approximate
                            thead.insertBefore(newTh, lastTh);

                            /* 2. Add <td> to every data row using data-rowjson */
                            allRows.forEach(function (row) {
                                var json = row.getAttribute('data-rowjson');
                                var rowData = {};
                                try { rowData = JSON.parse(json); } catch (ex) { }

                                var val = rowData[fieldName] !== undefined ? rowData[fieldName] : '';

                                var newTd = document.createElement('td');
                                newTd.textContent = val;

                                /* Insert before last td (actions column) */
                                var lastTd = row.querySelector('td:last-child');
                                row.insertBefore(newTd, lastTd);
                            });

                            /* 3. Update chk.dataset.colIdx so it's treated as "rendered" next time */
                            chk.dataset.colIdx = String(
                                tbl.querySelectorAll('thead th').length - 2
                            );
                        }
                    });

                    /* Re-run init so the new columns get sort/filter icons attached */
                    tbl.removeAttribute('data-lp-col-filters');
                    tbl.removeAttribute('data-lp-chooser');
                    setTimeout(function () {
                        applyColumnFilters();
                        applyStatusBadges();
                    }, 50);

                    savePreferences(tbl); // ← ADD THIS LINE
                    closePanel();
                });
            })
            .catch(function (err) {
                panel.querySelector('#lp-col-panel-list').innerHTML =
                    '<div style="padding:20px;color:#c00;text-align:center;">Failed to load columns. Please try again.</div>';
                console.error('GetAllAvailableColumns failed:', err);
            });
    }
    function applyFormatting() {
        document.querySelectorAll('table[id*="gridView"] tbody td').forEach(function (td) {
            formatCellValue(td);
        });
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 7 — User Preference Persistence (localStorage)
    ═══════════════════════════════════════════════════════════════════ */

    var PREF_KEY_PREFIX = 'lp_prefs_';

    function getPrefKey() {
        /* Key = page path, e.g. "lp_prefs_/ESS/PR/ESSPREmployeeAdvance_ListPage.aspx" */
        return PREF_KEY_PREFIX + window.location.pathname.toLowerCase();
    }

    function savePreferences(tbl) {
        try {
            var ths = Array.from(tbl.querySelectorAll('thead th'));
            var colOrder = [];

            ths.forEach(function (th, i) {
                var label = (th.innerText || '')
                    .replace(/[\uE000-\uF8FF\n\r\t]/g, '')
                    .replace(/\s+/g, ' ')
                    .trim();
                if (!label) return; // skip checkbox & actions columns

                colOrder.push({
                    label: label,
                    hidden: !!th.dataset.lpHidden,
                    position: i
                });
            });

            var prefs = {
                savedAt: new Date().toISOString(),
                columns: colOrder
            };

            localStorage.setItem(getPrefKey(), JSON.stringify(prefs));
        } catch (ex) {
            console.warn('lp: could not save preferences', ex);
        }
    }

    function loadPreferences(tbl) {
        try {
            var raw = localStorage.getItem(getPrefKey());
            if (!raw) return;

            var prefs = JSON.parse(raw);
            if (!prefs || !prefs.columns) return;

            var savedCols = prefs.columns;

            /* ── Step 1: Restore column order ── */
            var thead = tbl.querySelector('thead tr');
            var tbody = tbl.querySelector('tbody');
            if (!thead) return;

            var currentThs = Array.from(tbl.querySelectorAll('thead th'));

            /* Build a map: normalized label → th element */
            var thMap = {};
            currentThs.forEach(function (th) {
                var label = (th.innerText || '')
                    .replace(/[\uE000-\uF8FF\n\r\t]/g, '')
                    .replace(/\s+/g, ' ')
                    .trim()
                    .toLowerCase();
                if (label) thMap[label] = th;
            });

            /* Reorder thead cells to match saved order */
            /* Keep first (checkbox) and last (actions) in place — only reorder middle columns */
            var firstTh = currentThs[0];
            var lastTh = currentThs[currentThs.length - 1];

            savedCols.forEach(function (saved) {
                var key = saved.label.toLowerCase();
                var th = thMap[key];
                if (!th || th === firstTh || th === lastTh) return;
                /* Move th to before the last th */
                thead.insertBefore(th, lastTh);
            });

            /* Reorder tbody cells to match new th order */
            var newThOrder = Array.from(tbl.querySelectorAll('thead th'));
            var origThOrder = currentThs;

            /* Build index mapping: new position → original position */
            var indexMap = newThOrder.map(function (th) {
                return origThOrder.indexOf(th);
            });

            Array.from(tbl.querySelectorAll('tbody tr')).forEach(function (row) {
                var cells = Array.from(row.children);
                var lastTd = cells[cells.length - 1];
                indexMap.forEach(function (origIdx, newIdx) {
                    var cell = cells[origIdx];
                    if (cell && cell !== lastTd) {
                        row.insertBefore(cell, lastTd);
                    }
                });
            });

            /* ── Step 2: Restore hidden/visible state ── */
            var finalThs = Array.from(tbl.querySelectorAll('thead th'));
            savedCols.forEach(function (saved) {
                var key = saved.label.toLowerCase();
                /* Find th by label in final order */
                finalThs.forEach(function (th, idx) {
                    var label = (th.innerText || '')
                        .replace(/[\uE000-\uF8FF\n\r\t]/g, '')
                        .replace(/\s+/g, ' ')
                        .trim()
                        .toLowerCase();
                    if (label !== key) return;

                    var show = !saved.hidden;
                    th.style.display = show ? '' : 'none';
                    if (show) delete th.dataset.lpHidden;
                    else th.dataset.lpHidden = '1';

                    /* Also toggle td cells */
                    Array.from(tbl.querySelectorAll('tbody tr')).forEach(function (row) {
                        var td = row.children[idx];
                        if (td) td.style.display = show ? '' : 'none';
                    });
                });
            });
            setTimeout(function () { classifyColumns(); }, 50); // ← ADD THIS
        } catch (ex) {
            console.warn('lp: could not load preferences', ex);
        }
    }

    /* ═══════════════════════════════════════════════════════════════════
   SECTION 8 — Smart Column Classification
   Replaces fragile nth-child CSS rules with JS-detected semantic classes.
   Runs after grid binds. Safe to re-run on postback.
═══════════════════════════════════════════════════════════════════ */
    function classifyColumns() {
        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            var ths = Array.from(tbl.querySelectorAll('thead th'));
            if (!ths.length) return;

            var hasRows = tbl.querySelectorAll('tbody tr td').length > 0;

            // ── Step 1: Structural columns — detectable even without data rows ──
            ths.forEach(function (th, colIdx) {
                th.classList.remove('lp-col-checkbox', 'lp-col-actions');

                if (th.querySelector('input[type="checkbox"]')) {
                    th.classList.add('lp-col-checkbox');
                    tbl.querySelectorAll('tbody tr').forEach(function (row) {
                        var td = row.children[colIdx];
                        if (td) { td.classList.remove('lp-col-checkbox'); td.classList.add('lp-col-checkbox'); }
                    });
                    return;
                }

                var thText = (th.innerText || '').replace(/[\uE000-\uF8FF\n\r\t]/g, '').trim();
                var hasActionButtonsInTh = !!th.querySelector('a.grid-img-btn, button:not(.lp-chooser-btn)');

                if (!thText && hasActionButtonsInTh) {
                    th.classList.add('lp-col-actions');
                    tbl.querySelectorAll('tbody tr').forEach(function (row) {
                        var td = row.children[colIdx];
                        if (td) { td.classList.remove('lp-col-actions'); td.classList.add('lp-col-actions'); }
                    });
                    return;
                }

                if (!thText && !hasActionButtonsInTh) {
                    // Check TD content for action buttons
                    var firstDataRow = tbl.querySelector('tbody tr');
                    if (firstDataRow) {
                        var tdCheck = firstDataRow.children[colIdx];
                        if (tdCheck) {
                            var hasLinks = !!tdCheck.querySelector('a, button:not(.lp-chooser-btn)');
                            var noText = (tdCheck.textContent || '').trim() === '';
                            if (hasLinks && noText) {
                                th.classList.add('lp-col-actions');
                                tbl.querySelectorAll('tbody tr').forEach(function (row) {
                                    var td = row.children[colIdx];
                                    if (td) { td.classList.remove('lp-col-actions'); td.classList.add('lp-col-actions'); }
                                });
                                return;
                            }
                        }
                    }
                }
            });

            // ── Step 2: Content classification — only when rows exist ──
            if (!hasRows) return;

            var firstDataColFound = false;

            ths.forEach(function (th, colIdx) {
                if (th.classList.contains('lp-col-checkbox') ||
                    th.classList.contains('lp-col-actions')) return;

                var vals = [];
                tbl.querySelectorAll('tbody tr').forEach(function (row) {
                    var td = row.children[colIdx];
                    if (td) vals.push((td.textContent || '').trim());
                });

                var colType = detectColumnType(vals);

                th.classList.remove('lp-col-id', 'lp-col-date', 'lp-col-number');
                tbl.querySelectorAll('tbody tr').forEach(function (row) {
                    var td = row.children[colIdx];
                    if (td) td.classList.remove('lp-col-id', 'lp-col-date', 'lp-col-number');
                });

                var nonEmpty = vals.filter(function (v) { return v !== ''; });
                var avgLen = nonEmpty.length
                    ? nonEmpty.reduce(function (s, v) { return s + v.length; }, 0) / nonEmpty.length
                    : 0;

                // First non-structural column with short avg values = ID column
                var looksLikeId = !firstDataColFound && avgLen > 0 && avgLen < 20 && colType !== 'date';

                if (looksLikeId) {
                    firstDataColFound = true;
                    //th.classList.add('lp-col-id');
                    //tbl.querySelectorAll('tbody tr').forEach(function (row) {
                    //    var td = row.children[colIdx];
                    //    if (td) td.classList.add('lp-col-id');
                    //});
                } else if (colType === 'date') {
                    th.classList.add('lp-col-date');
                    tbl.querySelectorAll('tbody tr').forEach(function (row) {
                        var td = row.children[colIdx];
                        if (td) td.classList.add('lp-col-date');
                    });
                } else if (colType === 'number' && firstDataColFound) {
                    th.classList.add('lp-col-number');
                    tbl.querySelectorAll('tbody tr').forEach(function (row) {
                        var td = row.children[colIdx];
                        if (td) td.classList.add('lp-col-number');
                    });
                } else {
                    // Plain text — still mark first data col as ID if nothing better found
                    if (!firstDataColFound && nonEmpty.length > 0) {
                        firstDataColFound = true;
                    }
                }
            });
        });
    }

    function clearPreferences() {
        try {
            localStorage.removeItem(getPrefKey());
        } catch (ex) { }
    }

    /* ═══════════════════════════════════════════════════════════════════
       SECTION 9 — D365 Inline Row Editing & Auto-Save
       (COMMENTED OUT AS PER USER REQUEST)
    ═══════════════════════════════════════════════════════════════════ */
    /*
    function isFnOForm() {
        var path = window.location.pathname.toLowerCase();
        return path.indexOf('purchaseorderlines_listpage.aspx') !== -1;
    }

    function showAutoSaveToast(isSuccess, message) {
        var existing = document.getElementById('lp-autosave-toast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.id = 'lp-autosave-toast';

        if (isSuccess) {
            toast.style.background = '#107c10'; // Safe green
            toast.style.boxShadow = '0 8px 24px rgba(16, 124, 16, 0.2), 0 2px 8px rgba(0,0,0,0.12)';
            toast.innerHTML = '<i class="mdi mdi-check-circle-outline"></i> <span>' + (message || 'Changes saved successfully') + '</span>';
        } else {
            toast.style.background = '#0078d4'; // Microsoft blue
            toast.style.boxShadow = '0 8px 24px rgba(0, 120, 212, 0.2), 0 2px 8px rgba(0,0,0,0.12)';
            toast.innerHTML = '<i class="mdi mdi-cloud-sync" style="animation: spin 1s linear infinite;"></i> <span>' + (message || 'Saving changes automatically...') + '</span>';
        }

        // Add keyframe for spin if it doesn't exist
        if (!document.getElementById('lp-spin-style')) {
            var style = document.createElement('style');
            style.id = 'lp-spin-style';
            style.innerHTML = '@keyframes spin { 100% { transform: rotate(360deg); } }';
            document.head.appendChild(style);
        }

        document.body.appendChild(toast);

        // Trigger animation
        requestAnimationFrame(function () {
            toast.classList.add('lp-autosave-toast-show');
        });

        // If it's a success toast, dismiss it after 2.5 seconds
        if (isSuccess) {
            setTimeout(function () {
                toast.classList.remove('lp-autosave-toast-show');
                setTimeout(function () { toast.remove(); }, 300);
            }, 2500);
        }
    }

    function applyD365InlineEdit() {
        if (!isFnOForm()) return; // Only run on F&O key forms, not ESS forms

        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            var tbody = tbl.querySelector('tbody');
            if (!tbody) return;

            // Get all actual data rows
            var dataRows = Array.from(tbody.querySelectorAll('tr')).filter(function (r) {
                return r.style.display !== 'none' && !r.querySelector('th') && r.querySelectorAll('td').length > 1;
            });

            // 1. Check if there is a row currently in edit mode (contains .btn-update)
            var activeEditRow = null;
            var activeEditIndex = -1;
            dataRows.forEach(function (row, idx) {
                if (row.querySelector('.btn-update')) {
                    activeEditRow = row;
                    activeEditIndex = idx;
                }
            });

            // 2. If we just saved, show a success toast!
            var justSaved = localStorage.getItem('lp_just_saved');
            if (justSaved === '1' && activeEditRow === null) {
                localStorage.removeItem('lp_just_saved');
                showAutoSaveToast(true, 'Changes saved successfully');
            }

            // 3. Handle pending click from localStorage after page reload/update postback
            var pendingIdx = localStorage.getItem('lp_pending_edit_index');
            if (pendingIdx !== null && activeEditRow === null) {
                localStorage.removeItem('lp_pending_edit_index');
                var targetIdx = parseInt(pendingIdx, 10);
                if (targetIdx >= 0 && targetIdx < dataRows.length) {
                    var btnEdit = dataRows[targetIdx].querySelector('.btn-edit');
                    if (btnEdit) {
                        window.suppressOverlay = true;
                        btnEdit.click();
                        return;
                    }
                }
            }

            // 4. Mark active edit row and store initial field values
            if (activeEditRow) {
                activeEditRow.classList.add('lp-editing-row');
                activeEditRow.querySelectorAll('input, select').forEach(function (input) {
                    if (input.dataset.lpInitial === undefined) {
                        input.dataset.lpInitial = input.value || '';
                    }
                });

            }

            // 5. Setup Click-to-Edit on read-only rows
            dataRows.forEach(function (row, idx) {
                if (idx !== activeEditIndex) {
                    row.querySelectorAll('td').forEach(function (td) {
                        var btnEdit = row.querySelector('.btn-edit');
                        if (btnEdit) {
                            td.style.cursor = 'pointer';
                            if (td.dataset.lpClickBound) return;
                            td.dataset.lpClickBound = '1';

                            td.addEventListener('click', function (e) {
                                var target = e.target;
                                // Ignore clicking checkboxes, links, dropdowns, inputs, buttons, attachment buttons
                                if (target.closest('input, select, a, button, .round-checkbox, .btn-attachment')) {
                                    return;
                                }

                                if (activeEditRow) {
                                    // Check if any changes were made
                                    var hasChanged = false;
                                    activeEditRow.querySelectorAll('input, select').forEach(function (input) {
                                        var initial = input.dataset.lpInitial || '';
                                        var current = input.value || '';
                                        if (initial !== current) {
                                            hasChanged = true;
                                        }
                                    });

                                    localStorage.setItem('lp_pending_edit_index', idx.toString());

                                    if (hasChanged) {
                                        localStorage.setItem('lp_just_saved', '1');
                                        var btnUpdate = activeEditRow.querySelector('.btn-update');
                                        if (btnUpdate) {
                                            window.suppressOverlay = true;
                                            showAutoSaveToast(false, 'Saving changes automatically...');
                                            btnUpdate.click();
                                        }
                                    } else {
                                        // Bypasses the database update and Dynamics 365 web service call
                                        var btnCancel = activeEditRow.querySelector('.btn-cancel');
                                        if (btnCancel) {
                                            window.suppressOverlay = true;
                                            btnCancel.click();
                                        }
                                    }
                                } else {
                                    // No row was editing, enter edit mode immediately via postback
                                    window.suppressOverlay = true;
                                    btnEdit.click();
                                }
                            });
                        }
                    });
                }
            });

            // 6. Setup Click-Away Auto-Save
            if (activeEditRow) {
                if (!window.lp_clickaway_bound) {
                    window.lp_clickaway_bound = true;

                    var docClick = function (e) {
                        var target = e.target;

                        // Check if the page still has the active edit row
                        var currentEditRow = document.querySelector('.lp-editing-row');
                        if (!currentEditRow) {
                            document.removeEventListener('mousedown', docClick);
                            window.lp_clickaway_bound = false;
                            return;
                        }

                        // If user clicks standard Cancel, let it handle naturally
                        if (target.closest('.btn-cancel')) {
                            document.removeEventListener('mousedown', docClick);
                            window.lp_clickaway_bound = false;
                            return;
                        }

                        // If clicking outside the active editing row
                        if (!currentEditRow.contains(target)) {
                            // Ignore clicks on scrollbars, column headers, or empty table container space
                            if (target.closest('th') ||
                                target === tbody ||
                                target.closest('.table-responsive') === target ||
                                target.closest('.page-placeholder') === target ||
                                target.tagName === 'THEAD' ||
                                target.tagName === 'TABLE') {
                                return;
                            }

                            // If they clicked on another row, set it as pending edit
                            var otherRow = target.closest('tr');
                            if (otherRow && otherRow.parentNode === tbody) {
                                var otherIdx = dataRows.indexOf(otherRow);
                                if (otherIdx !== -1 && otherIdx !== activeEditIndex) {
                                    var otherEditBtn = otherRow.querySelector('.btn-edit');
                                    if (otherEditBtn) {
                                        localStorage.setItem('lp_pending_edit_index', otherIdx.toString());
                                    }
                                }
                            }

                            // Check if any changes were actually made
                            var hasChanged = false;
                            currentEditRow.querySelectorAll('input, select').forEach(function (input) {
                                var initial = input.dataset.lpInitial || '';
                                var current = input.value || '';
                                if (initial !== current) {
                                    hasChanged = true;
                                }
                            });

                            document.removeEventListener('mousedown', docClick);
                            window.lp_clickaway_bound = false;

                            if (hasChanged) {
                                var btnUpdate = currentEditRow.querySelector('.btn-update');
                                if (btnUpdate) {
                                    window.suppressOverlay = true;
                                    localStorage.setItem('lp_just_saved', '1');
                                    showAutoSaveToast(false, 'Saving changes automatically...');
                                    btnUpdate.click();
                                }
                            } else {
                                // No changes made, cancel editing instantly to avoid hitting the D365 web service
                                var btnCancel = currentEditRow.querySelector('.btn-cancel');
                                if (btnCancel) {
                                    window.suppressOverlay = true;
                                    btnCancel.click();
                                }
                            }
                        }
                    };

                    document.addEventListener('mousedown', docClick);
                }
            }
        });
    }
    */ // End of commented Section 9



    /* ═══════════════════════════════════════════════════════════════════
   SECTION 10 — Auto-refresh on success action (Delete/Submit/etc.)
═══════════════════════════════════════════════════════════════════ */
    var _autoRefreshObserverAttached = false;

    function applyAutoRefreshOnSuccess() {
        if (!document.querySelector('table[id*="gridView"]')) return;
        if (_autoRefreshObserverAttached) return;
        _autoRefreshObserverAttached = true;

        var refreshScheduled = false;

        var observer = new MutationObserver(function (mutations) {
            if (refreshScheduled) return;

            mutations.forEach(function (mutation) {
                mutation.addedNodes.forEach(function (node) {
                    if (!node || node.nodeType !== 1) return;

                    var isToast = node.classList &&
                        node.classList.contains('lp-toast') &&
                        node.classList.contains('success');

                    if (!isToast) return;
                    if (refreshScheduled) return;

                    refreshScheduled = true;

                    setTimeout(function () {
                        refreshScheduled = false;
                        __doPostBack('RefreshGrid', '');
                    }, 6000);
                });
            });
        });

        observer.observe(document.body, { childList: true, subtree: true });
    }

    /* ═══════════════════════════════════════════════════════════════════
   SECTION 11 — Attachment Count Badges
═══════════════════════════════════════════════════════════════════ */
    var _attachmentCounts = {}; // ref → count

    window.updateAttachmentBadge = function (ref, count) {
        if (!ref) return;
        _attachmentCounts[ref] = count;

        // Find the btn-attachment whose onclick URL contains this ref
        document.querySelectorAll('.btn-attachment').forEach(function (btn) {
            var onclick = btn.getAttribute('href') || btn.getAttribute('onclick') || '';
            if (onclick.indexOf(encodeURIComponent(ref)) !== -1 ||
                onclick.indexOf(ref) !== -1) {
                renderAttachmentBadge(btn, count);
            }
        });
    };

    function renderAttachmentBadge(btn, count) {
        var wrap = btn.closest('.attach-btn-wrap');
        if (!wrap) {
            // Auto-wrap if not already wrapped
            var w = document.createElement('span');
            w.className = 'attach-btn-wrap';
            btn.parentNode.insertBefore(w, btn);
            w.appendChild(btn);
            wrap = w;
        }

        var existing = wrap.querySelector('.attach-count-badge');
        if (existing) existing.remove();

        if (count > 0) {
            var badge = document.createElement('span');
            badge.className = 'attach-count-badge';
            badge.textContent = count > 99 ? '99+' : count;
            wrap.appendChild(badge);
        }
    }


    /* ── Sentence Case helper (Microsoft grid convention) ── */
    function toSentenceCase(str) {
        if (!str) return str;
        var trimmed = str.trim();
        if (!trimmed) return str;

        // Preserve values that are entirely uppercase codes/acronyms/IDs
        // (e.g. "PKR", "IDAP", "PO-008239", "V-001834") — leave untouched
        if (/^[A-Z0-9][A-Z0-9\-\/. ]*$/.test(trimmed) && /[A-Z]/.test(trimmed)) {
            // Only skip if it looks like a code (has a digit or hyphen, or is short & all-caps)
            if (/\d/.test(trimmed) || /-/.test(trimmed) || trimmed === trimmed.toUpperCase()) {
                return str;
            }
        }

        return trimmed.charAt(0).toUpperCase() + trimmed.slice(1).toLowerCase();
    }

    /* ═══════════════════════════════════════════════════════════════════
       MAIN INIT
    ═══════════════════════════════════════════════════════════════════ */
    function init() {
        applyFormatting();
        applyStatusBadges();
        applyPrevNext();
        applyEmptyState();
        applyRowCount();
        applyColumnFilters();
        classifyColumns();
        closeActiveDropdown();
        applyAutoRefreshOnSuccess(); // ← yes, keep this here

        /* ── Restore saved preferences after everything is initialized ── */
        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
            loadPreferences(tbl);
        });
        // applyD365InlineEdit(); // Commented out as per user request
    }
    $(document).ready(function () {
        setTimeout(init, 50);  // slight delay ensures pagination has rendered first
    });

    /* Re-run after ASP.NET UpdatePanel postbacks */
    if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            /* Reset column filter state on postback so fresh data is used */
            tableStates = {};
            _autoRefreshObserverAttached = false; // ← ADD THIS LINE
            setTimeout(init, 20); // Massive speedup: reduced artificial delay from 300ms to 20ms
        });
    }

    //if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

    //        // ── 1. Reset module-level state ──
    //        tableStates = {};
    //        _autoRefreshObserverAttached = false;

    //        // ── 2. Strip ALL guard flags so init() re-runs cleanly ──
    //        document.querySelectorAll('table[id*="gridView"]').forEach(function (tbl) {
    //            delete tbl.dataset.lpColFilters;
    //            delete tbl.dataset.lpChooser;
    //            delete tbl.dataset.lpSized;   // force column width re-calc
    //        });

    //        // Strip prev/next wrapper flag from pagination container
    //        document.querySelectorAll('.pagination-Numbers').forEach(function (pn) {
    //            delete pn.dataset.lpWrapped;
    //        });

    //        // ── 3. Re-run full init ──
    //        setTimeout(init, 20);
    //    });
    //}
})();

