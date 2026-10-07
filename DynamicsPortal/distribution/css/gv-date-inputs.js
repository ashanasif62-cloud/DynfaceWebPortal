/*
* gv-date-inputs.js
* ─────────────────────────────────────────────────────────────────
* Companion to modal-modern.js.
* Applies the same date guard + masked text replacement + Pikaday
* calendar to <input type="date"> fields inside ASP.NET GridView rows.
*
* OPT-IN
*   Add the CSS class  gv-date-enabled  to any GridView whose date
*   columns should be enhanced:
*
*     <asp:GridView CssClass="gv-date-enabled" ...>
*
* PER-COLUMN OVERRIDES (on the original <input type="date">):
*   data-min="YYYY-MM-DD"         individual min boundary
*   data-max="YYYY-MM-DD"         individual max boundary
*   data-mm-date-format="..."     DD/MM/YYYY | MM/DD/YYYY | YYYY-MM-DD
*
* PAGE-LEVEL DEFAULTS (on <body>):
*   data-mm-date-min="YYYY-MM-DD"
*   data-mm-date-max="YYYY-MM-DD"
*   data-mm-date-format="DD/MM/YYYY"
*
* CODE-LEVEL DEFAULT:
*   GV_DATE_FORMAT constant below.
*
* DEPENDENCIES
*   – modal-modern.js must be loaded first so that:
*       • window.showModalToast() is available
*       • Pikaday load state (_pikadayReady / _pikadayQueue) is shared
*   – Pikaday 1.8.2 is lazy-loaded from cdnjs (same as modal-modern.js)
* ─────────────────────────────────────────────────────────────────
*/

(function () {
    'use strict';

    /* ── Selector: only tables that opted in ───────────────────── */
    var GV_SELECTOR = 'table.gv-date-enabled';

    /* ── Code-level format default ─────────────────────────────── */
    var GV_DATE_FORMAT = 'MM/DD/YYYY';

    /* ── Pikaday CDN (same URLs as modal-modern.js) ────────────── */
    var PIKADAY_CSS = 'https://cdnjs.cloudflare.com/ajax/libs/pikaday/1.8.2/css/pikaday.min.css';
    var PIKADAY_JS = 'https://cdnjs.cloudflare.com/ajax/libs/pikaday/1.8.2/pikaday.min.js';

    /* ── Safe toast: falls back silently if modal-modern not loaded */
    function toast(msg, type, ms) {
        if (typeof window.showModalToast === 'function') {
            window.showModalToast(msg, type || 'warning', ms || 3500);
        }
    }

    /* ═══════════════════════════════════════════════════════════
     * 1.  DATE GUARD
     *     Sets min/max HTML attributes and clamps values at runtime.
     *     Mirrors section 11 of modal-modern.js.
     * ═══════════════════════════════════════════════════════════ */
    function applyGvDateGuards(root) {
        var today = new Date();
        var todayStr = today.toISOString().slice(0, 10);
        var defaultMax = (today.getFullYear() + 10) + todayStr.slice(4);
        var d365Min = '1900-01-01';
        var d365Max = '2154-12-31';

        var bodyMin = document.body.getAttribute('data-mm-date-min') || d365Min;
        var bodyMax = document.body.getAttribute('data-mm-date-max') || d365Max;

        root.querySelectorAll('input[type="date"]').forEach(function (input) {
            if (input.dataset.mmDateGuard) return;
            input.dataset.mmDateGuard = '1';

            var min = d365Min; // input.getAttribute('data-min') || bodyMin;
            var max = d365Max //input.getAttribute('data-max') || bodyMax;

            input.setAttribute('min', min);
            input.setAttribute('max', max);

            if (input.value) {
                if (input.value < min) input.value = min;
                if (input.value > max) input.value = max;
            }

            //input.addEventListener('change', function () {
            //    if (!this.value) return;
            //    var clamped = false;
            //    if (this.value < min) { this.value = min; clamped = true; }
            //    if (this.value > max) { this.value = max; clamped = true; }
            //    if (clamped) {
            //        this.dispatchEvent(new Event('input', { bubbles: true }));
            //        toast('Date adjusted to the nearest allowed date.');
            //    }
            //});
        });
    }

    /* ═══════════════════════════════════════════════════════════
     * 2.  MASKED TEXT REPLACEMENT
     *     Hides the native date input in-place (so ASP.NET postback
     *     still reads it by name/id) and inserts a formatted text
     *     input + calendar icon wrapper before it.
     *     Mirrors section 12 of modal-modern.js.
     * ═══════════════════════════════════════════════════════════ */
    function applyGvDateFormatReplacement(root) {
        var bodyFmt = document.body.getAttribute('data-mm-date-format') || GV_DATE_FORMAT;

        root.querySelectorAll('input[type="date"]').forEach(function (original) {
            if (original.getAttribute('data-mm-date-replaced')) return;

            var fmt = original.getAttribute('data-mm-date-format') || bodyFmt;
            var sep = fmt.indexOf('/') !== -1 ? '/' : '-';

            function toDisplay(iso) {
                if (!iso) return '';
                var p = iso.split('-');
                if (p.length !== 3) return '';
                return fmt.replace('YYYY', p[0]).replace('MM', p[1]).replace('DD', p[2]);
            }

            function toISO(display) {
                var parts = display.split(sep);
                if (parts.length !== 3) return '';
                var keys = fmt.split(sep);
                var map = {};
                keys.forEach(function (k, i) { map[k] = parts[i]; });
                return (map['YYYY'] || '') + '-' + (map['MM'] || '') + '-' + (map['DD'] || '');
            }

            function isValidDisplay(display) {
                var parts = display.split(sep);
                if (parts.length !== 3) return false;
                var keys = fmt.split(sep);
                var map = {};
                keys.forEach(function (k, i) { map[k] = parseInt(parts[i], 10); });
                var y = map['YYYY'], m = map['MM'], d = map['DD'];
                if (!y || !m || !d || m < 1 || m > 12 || d < 1 || d > 31) return false;
                var dt = new Date(y, m - 1, d);
                return dt.getFullYear() === y && dt.getMonth() === m - 1 && dt.getDate() === d;
            }

            /* Keep original hidden in the DOM so ASP.NET postback works */
            original.style.display = 'none';
            original.setAttribute('data-mm-date-replaced', 'true');
            original.setAttribute('tabindex', '-1');

            /* Visible masked text input */
            var textInput = document.createElement('input');
            textInput.type = 'text';
            textInput.className = original.className + ' mm-date-text';
            textInput.placeholder = fmt;
            textInput.maxLength = fmt.length;
            textInput.setAttribute('autocomplete', 'off');
            textInput.setAttribute('data-mm-date-format', fmt);
            textInput.setAttribute('data-original-id', original.id);

            var minAttr = original.getAttribute('data-min') || original.getAttribute('min');
            var maxAttr = original.getAttribute('data-max') || original.getAttribute('max');
            if (minAttr) textInput.setAttribute('data-min', minAttr);
            if (maxAttr) textInput.setAttribute('data-max', maxAttr);
            console.log('original.value:', original.value, 'fmt:', fmt, 'result:', toDisplay(original.value));
            if (original.value) textInput.value = toDisplay(original.value);

            function syncToOriginal(iso) {
                original.value = iso;
                original.dispatchEvent(new Event('change', { bubbles: true }));
            }

            /* Auto-masking while typing */
            textInput.addEventListener('input', function () {
                var raw = this.value.replace(/[^0-9]/g, '');
                var masked = '';
                var segs = (fmt === 'YYYY-MM-DD')
                    ? [[0, 4], [4, 6], [6, 8]]
                    : [[0, 2], [2, 4], [4, 8]];

                for (var i = 0; i < segs.length; i++) {
                    var chunk = raw.slice(segs[i][0], segs[i][1]);
                    if (!chunk) break;
                    masked += (i > 0 ? sep : '') + chunk;
                }
                if (this.value !== masked) this.value = masked;

                var iso = toISO(masked);
                if (iso) syncToOriginal(iso);
            });

            /* Blur: full validation */
            textInput.addEventListener('blur', function () {
                var val = this.value.trim();
                this.classList.remove('mm-date-error', 'mm-date-ok');

                if (!val) { syncToOriginal(''); return; }

                if (!isValidDisplay(val)) {
                    this.classList.add('mm-date-error');
                    toast('Invalid date. Please use the format ' + fmt + '.');
                    return;
                }

                var iso = toISO(val);
                var dMin = this.getAttribute('data-min');
                var dMax = this.getAttribute('data-max');

                if ((dMin && iso < dMin) || (dMax && iso > dMax)) {
                    this.classList.add('mm-date-error');
                    toast('Date is outside the allowed range.');
                    return;
                }

                this.classList.add('mm-date-ok');
                syncToOriginal(iso);
            });

            /*
             * Sync before postback.
             * GridView save buttons may live outside the table, so we scope
             * to the document rather than the grid root.
             */
            document.querySelectorAll(
                'input[type="submit"],' +
                'a[id*="btnSave"], a[id*="btnCreate"], a[id*="btnSubmit"], a[id*="btnUpdate"],' +
                'input[id*="btnSave"], input[id*="btnCreate"], input[id*="btnSubmit"], input[id*="btnUpdate"]'
            ).forEach(function (btn) {
                /* Avoid attaching the same listener multiple times across re-renders */
                if (!btn.dataset.mmGvBlurBound) {
                    btn.dataset.mmGvBlurBound = '1';
                    btn.addEventListener('click', function () {
                        document.querySelectorAll('.mm-date-text').forEach(function (t) {
                            t.dispatchEvent(new Event('blur'));
                        });
                    });
                }
            });

            /* Wrapper + calendar icon */
            var wrapper = document.createElement('span');
            wrapper.className = 'mm-date-wrapper';

            var icon = document.createElement('span');
            icon.className = 'mm-date-icon';
            icon.setAttribute('aria-hidden', 'true');
            icon.setAttribute('title', 'Pick a date');
            icon.innerHTML =
                '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor">' +
                '<path d="M3.5 0a.5.5 0 0 1 .5.5V1h8V.5a.5.5 0 0 1 1 0V1h1a2 2 0 0 1 2 2v11a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V3a2 2 0 0 1 2-2h1V.5a.5.5 0 0 1 .5-.5zM1 4v10a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V4H1zm1-2a1 1 0 0 0-1 1v.5h14V3a1 1 0 0 0-1-1H2z"/>' +
                '</svg>';

            icon.addEventListener('mousedown', function (e) {
                e.preventDefault();
                textInput.focus();
            });

            wrapper.appendChild(textInput);
            wrapper.appendChild(icon);
            original.parentNode.insertBefore(wrapper, original);
        });
    }

    /* ═══════════════════════════════════════════════════════════
     * 3.  PIKADAY CALENDAR
     *     Re-uses the shared _pikadayReady / _pikadayQueue state
     *     that modal-modern.js owns.  If modal-modern.js has not
     *     loaded, falls back to an independent load path.
     *     Mirrors section 13 of modal-modern.js.
     * ═══════════════════════════════════════════════════════════ */
    function loadPikadayGv(cb) {
        /* Prefer the shared state managed by modal-modern.js */
        if (window._pikadayReady) { cb(); return; }

        if (window._pikadayQueue) {
            window._pikadayQueue.push(cb);
            return;
        }

        /* modal-modern.js not present — manage loading ourselves */
        window._pikadayQueue = [cb];

        if (!document.getElementById('mm-pikaday-css')) {
            var link = document.createElement('link');
            link.id = 'mm-pikaday-css';
            link.rel = 'stylesheet';
            link.href = PIKADAY_CSS;
            document.head.appendChild(link);
        }

        var script = document.createElement('script');
        script.src = PIKADAY_JS;
        script.async = true;
        script.onload = function () {
            window._pikadayReady = true;
            window._pikadayQueue.forEach(function (fn) { fn(); });
            window._pikadayQueue = [];
        };
        script.onerror = function () {
            console.warn('[gv-date-inputs] Pikaday failed to load. Falling back to keyboard-only.');
            window._pikadayQueue = [];
        };
        document.head.appendChild(script);
    }

    function attachGvPikaday(textInput) {
        if (textInput.dataset.mmPikaday) return;
        textInput.dataset.mmPikaday = '1';

        var fmt = textInput.getAttribute('data-mm-date-format') ||
            document.body.getAttribute('data-mm-date-format') ||
            GV_DATE_FORMAT;
        var sep = fmt.indexOf('/') !== -1 ? '/' : '-';

        function toISO(display) {
            var parts = display.split(sep);
            if (parts.length !== 3) return '';
            var keys = fmt.split(sep);
            var map = {};
            keys.forEach(function (k, i) { map[k] = parts[i]; });
            return (map['YYYY'] || '') + '-' + (map['MM'] || '') + '-' + (map['DD'] || '');
        }

        function isValidDisplay(display) {
            var parts = display.split(sep);
            if (parts.length !== 3) return false;
            var keys = fmt.split(sep);
            var map = {};
            keys.forEach(function (k, i) { map[k] = parseInt(parts[i], 10); });
            var y = map['YYYY'], m = map['MM'], d = map['DD'];
            if (!y || !m || !d || m < 1 || m > 12 || d < 1 || d > 31) return false;
            var dt = new Date(y, m - 1, d);
            return dt.getFullYear() === y && dt.getMonth() === m - 1 && dt.getDate() === d;
        }

        var minAttr = textInput.getAttribute('data-min');
        var maxAttr = textInput.getAttribute('data-max');

        var opts = {
            field: textInput,
            format: fmt,
            toString: function (date) {
                var y = date.getFullYear();
                var m = String(date.getMonth() + 1).padStart(2, '0');
                var d = String(date.getDate()).padStart(2, '0');
                return fmt.replace('YYYY', y).replace('MM', m).replace('DD', d);
            },
            parse: function (str) {
                var parts = str.split(sep);
                var keys = fmt.split(sep);
                var map = {};
                keys.forEach(function (k, i) { map[k] = parseInt(parts[i], 10); });
                return new Date(map['YYYY'], map['MM'] - 1, map['DD']);
            },
            onSelect: function (date) {
                var y = date.getFullYear();
                var m = String(date.getMonth() + 1).padStart(2, '0');
                var d = String(date.getDate()).padStart(2, '0');
                var iso = y + '-' + m + '-' + d;

                var origId = textInput.getAttribute('data-original-id');
                var origEl = origId ? document.getElementById(origId) : null;
                if (origEl) {
                    origEl.value = iso;
                    origEl.dispatchEvent(new Event('change', { bubbles: true }));
                }

                textInput.value = opts.toString(date);
                textInput.classList.remove('mm-date-error');
                textInput.classList.add('mm-date-ok');

                if (typeof window._formDirty !== 'undefined') window._formDirty = true;
            },
            i18n: {
                previousMonth: 'Previous Month',
                nextMonth: 'Next Month',
                months: ['January', 'February', 'March', 'April', 'May', 'June',
                    'July', 'August', 'September', 'October', 'November', 'December'],
                weekdays: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
                weekdaysShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
            },
            firstDay: 1
        };

        if (minAttr) {
            var minP = minAttr.split('-');
            opts.minDate = new Date(+minP[0], +minP[1] - 1, +minP[2]);
        }
        if (maxAttr) {
            var maxP = maxAttr.split('-');
            opts.maxDate = new Date(+maxP[0], +maxP[1] - 1, +maxP[2]);
        }

        if (textInput.value && isValidDisplay(textInput.value)) {
            opts.defaultDate = opts.parse(textInput.value);
            opts.setDefaultDate = true;
        }

        new Pikaday(opts); // eslint-disable-line no-undef
    }

    function applyGvPikaday(root) {
        var inputs = root.querySelectorAll('.mm-date-text:not([data-mm-pikaday])');
        if (!inputs.length) return;

        loadPikadayGv(function () {
            inputs.forEach(attachGvPikaday);
        });
    }

    /* ═══════════════════════════════════════════════════════════
     * 4.  ORCHESTRATOR
     *     Runs all three phases on a single GridView root element.
     * ═══════════════════════════════════════════════════════════ */
    function enhanceGrid(gridTable) {
        applyGvDateGuards(gridTable);
        applyGvDateFormatReplacement(gridTable);
        applyGvPikaday(gridTable);
    }

    function enhanceAllGrids() {
        document.querySelectorAll(GV_SELECTOR).forEach(enhanceGrid);
    }

    /* ═══════════════════════════════════════════════════════════
     * 5.  MUTATION OBSERVER
     *     Re-runs when the DOM inside any opted-in GridView changes
     *     (paging, sorting, UpdatePanel partial postback).
     *
     *     We observe the *container* of each grid, not the <table>
     *     itself, because ASP.NET UpdatePanel replaces the <table>
     *     element entirely on postback — the old reference goes stale.
     * ═══════════════════════════════════════════════════════════ */
    function observeGridContainers() {
        document.querySelectorAll(GV_SELECTOR).forEach(function (gridTable) {
            var container = gridTable.parentElement;
            if (!container || container.dataset.mmGvObserved) return;
            container.dataset.mmGvObserved = '1';

            var observer = new MutationObserver(function () {
                /* After a postback the old <table> is gone; find the new one */
                var newGrid = container.querySelector(GV_SELECTOR);
                if (newGrid) enhanceGrid(newGrid);
            });

            observer.observe(container, { childList: true, subtree: true });
        });
    }

    /* ═══════════════════════════════════════════════════════════
     * 6.  INIT
     *     Runs on DOMContentLoaded and also hooks ASP.NET's
     *     Sys.WebForms.PageRequestManager for UpdatePanel support.
     * ═══════════════════════════════════════════════════════════ */
    function init() {
        enhanceAllGrids();
        observeGridContainers();

        /* ASP.NET ScriptManager / UpdatePanel hook */
        if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance()
                .add_endRequest(function () {
                    enhanceAllGrids();
                    observeGridContainers();
                });
        } else {
            /* PageRequestManager not yet initialised — wait for it */
            document.addEventListener('DOMContentLoaded', function () {
                if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    Sys.WebForms.PageRequestManager.getInstance()
                        .add_endRequest(function () {
                            enhanceAllGrids();
                            observeGridContainers();
                        });
                }
            });
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();