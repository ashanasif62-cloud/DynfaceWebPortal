/**
 * modal-modern.js
 * Drop-in enhancement for DynamicsPortal modal / slide-in panels.
 * Features:
 *   1.  Page title enhancement
 *   2.  Required-field marking
 *   3.  Character counter on textareas
 *   4.  Auto-resize textarea
 *   5.  Button loading states (prevents double-submit)
 *   6.  Dirty-form close confirmation
 *   7.  Keyboard shortcut: Ctrl+Enter submits the primary button
 *   8.  Scroll-to-first-error on validation failure
 *   9.  Toast notification system
 *   10. Number input comma formatting
 *   11. Date input min/max guard
 *   12. Date input → formatted text replacement (DD/MM/YYYY by default)
 *   13. Pikaday calendar picker on mm-date-text inputs (loaded dynamically)
 */
(function () {
    'use strict';

    /* ─── Utility ──────────────────────────────────────────────── */
    function ready(fn) {
        if (document.readyState !== 'loading') { fn(); }
        else { document.addEventListener('DOMContentLoaded', fn); }
    }

    function isModalContext() {
        try {
            return window.self !== window.top ||
                !!document.querySelector('div[style*="height: 100vh"] .form-table') ||
                !!document.querySelector('#pageTitle');
        } catch (e) { return true; }
    }

    function escHtml(s) {
        return (s || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }

    /* ─── 1. Page Title ────────────────────────────────────────── */
    function enhancePageTitle() {
        var el = document.getElementById('pageTitle');
        if (!el || el.dataset.mmEnhanced) return;
        el.dataset.mmEnhanced = '1';

        var text = (el.innerText || el.textContent || '').trim();
        if (text && !el.querySelector('.mm-title-text')) {
            el.innerHTML = '<span class="mm-title-text">' + escHtml(text) + '</span>';
        }
    }

    /* ─── 2. Required-field visual cues ────────────────────────── */
    function markRequiredFields() {
        document.querySelectorAll('.form-table tr').forEach(function (row) {
            var labelCell = row.children[0];
            var inputCell = row.children[1];
            if (!labelCell || !inputCell) return;

            var labelText = (labelCell.innerText || '').trim();
            if (labelText.indexOf('*') !== -1 || labelCell.querySelector('.required')) {
                var inp = inputCell.querySelector('input, select, textarea');
                if (inp) {
                    inp.setAttribute('required', '');
                    inp.classList.add('mm-required');
                }
            }
        });
    }

    /* ─── 3. Character counter on textareas ────────────────────── */
    function applyTextareaCounters() {
        document.querySelectorAll('.form-table textarea, .modal-body textarea').forEach(function (ta) {
            if (ta.dataset.mmCounter) return;
            ta.dataset.mmCounter = '1';

            var max = parseInt(ta.getAttribute('maxlength') || '0', 10);
            if (!max) return;

            var counter = document.createElement('div');
            counter.className = 'mm-char-counter';
            counter.style.cssText = [
                'font-family:var(--mm-font,\'Segoe UI\',sans-serif)',
                'font-size:11px',
                'color:var(--mm-text-disabled,#a19f9d)',
                'text-align:right',
                'margin-top:3px'
            ].join(';');

            function update() {
                var remaining = max - ta.value.length;
                counter.textContent = remaining + ' characters remaining';
                counter.style.color = remaining < 20
                    ? 'var(--mm-danger,#c4314b)'
                    : 'var(--mm-text-disabled,#a19f9d)';
            }

            ta.addEventListener('input', update);
            update();
            ta.parentNode.insertBefore(counter, ta.nextSibling);
        });
    }

    /* ─── 4. Auto-resize textarea ──────────────────────────────── */
    function applyAutoResize() {
        document.querySelectorAll('.form-table textarea, .modal-body textarea').forEach(function (ta) {
            if (ta.dataset.mmResize) return;
            ta.dataset.mmResize = '1';
            ta.style.overflow = 'hidden';
            ta.style.resize = 'none';

            function resize() {
                ta.style.height = 'auto';
                ta.style.height = Math.min(ta.scrollHeight, 200) + 'px';
            }

            ta.addEventListener('input', resize);
            resize();
        });
    }

    /* ─── 5. Button loading state ──────────────────────────────── */
    function applyButtonLoadingState() {
        var primarySelectors = [
            'input[type="submit"][id*="btnCreate"]',
            'input[type="submit"][id*="btnSave"]',
            'input[type="submit"][id*="btnSubmit"]',
            'input[type="submit"][id*="btnUpdate"]',
            'input[type="submit"][id*="btnAdd"]',
            'div[style*="height: 100vh"] input[value="Create"]',
            'div[style*="height: 100vh"] input[value="Save"]'
        ].join(',');

        document.querySelectorAll(primarySelectors).forEach(function (btn) {
            if (btn.dataset.mmLoading) return;
            btn.dataset.mmLoading = '1';

            btn.addEventListener('click', function () {
                setTimeout(function () {
                    var isValid = true;
                    if (typeof Page_IsValid !== 'undefined') isValid = Page_IsValid;
                    if (!isValid) return;

                    var originalText = btn.value || btn.innerText || '';
                    var originalWidth = btn.offsetWidth + 'px';

                    btn.style.minWidth = originalWidth;
                    btn.value = 'Saving\u2026';
                    btn.disabled = true;
                    btn.style.opacity = '0.65';

                    /* Safety valve — re-enable after 8s */
                    setTimeout(function () {
                        btn.disabled = false;
                        btn.style.opacity = '';
                        if (btn.tagName === 'INPUT') btn.value = originalText;
                    }, 8000);
                }, 10);
            });
        });
    }

    

    

    /* ─── 7. Ctrl+Enter → primary button ───────────────────────── */
    function applyCtrlEnterSubmit() {
        document.addEventListener('keydown', function (e) {
            if ((e.ctrlKey || e.metaKey) && e.key === 'Enter') {
                var primaryBtn = document.querySelector(
                    'div[style*="height: 100vh"] input[value="Create"]:not([disabled]),' +
                    'input[type="submit"][id*="btnCreate"]:not([disabled]),' +
                    'input[type="submit"][id*="btnSave"]:not([disabled])'
                );
                if (primaryBtn) {
                    e.preventDefault();
                    primaryBtn.click();
                }
            }
        });
    }

    /* ─── 8. Scroll-to-first-error ─────────────────────────────── */
    function applyScrollToError() {
        var forms = document.querySelectorAll('form');
        forms.forEach(function (form) {
            form.addEventListener('submit', function () {
                setTimeout(function () {
                    var firstError = document.querySelector(
                        '.field-validation-error, .mm-required:invalid, span[style*="color:Red"]'
                    );
                    if (firstError) {
                        firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    }
                }, 50);
            });
        });
    }

    /* ─── 9. Toast Notification System ─────────────────────────── */
    var TOAST_ICONS = {
        success: 'mdi mdi-check-circle-outline',
        error: 'mdi mdi-alert-circle-outline',
        warning: 'mdi mdi-alert-outline',
        info: 'mdi mdi-information-outline'
    };

    var TOAST_TITLES = {
        success: 'Success',
        error: 'Error',
        warning: 'Warning',
        info: 'Information'
    };

    var TOAST_TYPE_MAP = {
        success: 'success',
        error: 'error',
        danger: 'error',
        warning: 'warning',
        info: 'info'
    };

    function ensureToastContainer() {
        var pageName = window.parent.location.pathname
            .split('/')
            .pop()
            .replace(/\.[^/.]+$/, '')
            .toLowerCase();

        var containerId = (pageName === 'esshrpersonaldetails')
            ? 'mm-toast-container-dashboard'
            : 'mm-toast-container';

        var c = document.getElementById(containerId);
        console.log("Using against containerId : ", c);
        if (!c) {
            c = document.createElement('div');
            c.id = containerId;
            document.body.appendChild(c);
        }
        return c;
    }

    function dismissToast(toast) {
        if (toast.classList.contains('mm-toast-hiding')) return;
        toast.classList.add('mm-toast-hiding');
        setTimeout(function () {
            if (toast.parentNode) toast.parentNode.removeChild(toast);
        }, 260);
    }

    function showModalToast(message, type, duration) {
        type = TOAST_TYPE_MAP[(type || 'info').toLowerCase()] || 'info';
        duration = duration || 5000;

        var container = ensureToastContainer();

        var toast = document.createElement('div');
        toast.className = 'mm-toast mm-toast-' + type;
        toast.style.setProperty('--mm-toast-duration', duration + 'ms');

        toast.innerHTML =
            '<i class="' + (TOAST_ICONS[type]) + ' mm-toast-icon" aria-hidden="true"></i>' +
            '<div class="mm-toast-body">' +
            '<div class="mm-toast-title">' + escHtml(TOAST_TITLES[type]) + '</div>' +
            '<div class="mm-toast-msg">' + message + '</div>' +
            '</div>' +
            '<button class="mm-toast-close" aria-label="Dismiss">&times;</button>';

        container.appendChild(toast);

        toast.querySelector('.mm-toast-close').addEventListener('click', function () {
            dismissToast(toast);
        });

        var timer = setTimeout(function () { dismissToast(toast); }, duration);

        toast.addEventListener('mouseenter', function () { clearTimeout(timer); });
        toast.addEventListener('mouseleave', function () {
            timer = setTimeout(function () { dismissToast(toast); }, 1500);
        });
    }

    /* ─── 10. Number input comma formatting ─────────────────────── */
    function applyNumberFormatting() {
        document.querySelectorAll(
            '.form-table input[masktype="number"], .modal-body input[masktype="number"]'
        ).forEach(function (input) {
            if (input.dataset.mmNumber) return;
            input.dataset.mmNumber = '1';

            function format(raw) {
                var clean = raw.replace(/[^0-9.]/g, '');
                var parts = clean.split('.');
                parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                return parts.join('.');
            }

            function strip(val) {
                return val.replace(/,/g, '');
            }

            input.addEventListener('input', function () {
                var pos = input.selectionStart;
                var before = input.value.substring(0, pos).replace(/,/g, '').length;
                input.value = format(input.value);
                /* Restore cursor accounting for added commas */
                var newPos = 0, counted = 0;
                for (var i = 0; i < input.value.length; i++) {
                    if (input.value[i] !== ',') counted++;
                    if (counted === before) { newPos = i + 1; break; }
                }
                input.setSelectionRange(newPos, newPos);
            });

            /* Strip commas before any postback so the server gets a clean number */
            document.querySelectorAll(
                'input[type="submit"], a[id*="btnSave"], a[id*="btnCreate"], a[id*="btnSubmit"], a[id*="btnUpdate"]'
            ).forEach(function (btn) {
                btn.addEventListener('click', function () {
                    input.value = strip(input.value);
                });
            });
        });
    }

        /* ─── 11. Date input min/max guard ─────────────────────────── */
        /*
         * Finds all date inputs (or inputs with data-mm-date) and enforces
         * min/max boundaries both as HTML attributes and via a change listener.
         *
         * Per-input overrides: data-min="YYYY-MM-DD"  data-max="YYYY-MM-DD"
         * Global defaults   : data-mm-date-min / data-mm-date-max on <body>
         *                     or falls back to today / today+10y.
         */
        function applyDateGuards() {
            var today = new Date();
            var todayStr = today.toISOString().slice(0, 10);
            var maxYear = today.getFullYear() + 10;
            var defaultMax = maxYear + todayStr.slice(4); // same MM-DD, +10 years
            var d365Min = '1900-01-01';
            var d365Max = '2154-12-31';

            /* Allow page-level defaults via <body data-mm-date-min="..." data-mm-date-max="..."> */
            var bodyMin = document.body.getAttribute('data-mm-date-min') || todayStr;
            var bodyMax = document.body.getAttribute('data-mm-date-max') || defaultMax;

            var selector = '.form-table input[type="date"], .modal-body input[type="date"],' +
                '.form-table input[data-mm-date], .modal-body input[data-mm-date]';

            document.querySelectorAll(selector).forEach(function (input) {
                if (input.dataset.mmDateGuard) return; // skip if already processed
                input.dataset.mmDateGuard = '1';

                var min = d365Min; //input.getAttribute('data-min') || bodyMin;
                var max = d365Max; //input.getAttribute('data-max') || bodyMax;

                input.setAttribute('min', min);
                input.setAttribute('max', max);

                /* Clamp pre-filled value */
                if (input.value) {
                    if (input.value < min) input.value = min;
                    if (input.value > max) input.value = max;
                }

                /* Runtime guard — catches manual keyboard entry that bypasses the picker */
                input.addEventListener('change', function () {
                    if (!this.value) return;
                    var clamped = false;
                    if (this.value < min) { this.value = min; clamped = true; }
                    if (this.value > max) { this.value = max; clamped = true; }
                    if (clamped) {
                        /* Notify bound validators / UpdatePanel watchers */
                        this.dispatchEvent(new Event('input', { bubbles: true }));
                        showModalToast('Date adjusted to the nearest allowed date.', 'warning', 3500);
                    }
                });
            });
        }

        /* ─── 12. Date input → formatted text replacement ───────────── */
        /*
         * Swaps every date input for a masked text input + a hidden field.
         * The hidden field keeps the original name/id so ASP.NET postback works.
         *
         * Format is controlled via:
         *   • data-mm-date-format="DD/MM/YYYY"  on the individual input  (highest priority)
         *   • data-mm-date-format="DD/MM/YYYY"  on <body>                (page-level default)
         *   • MM_DATE_FORMAT global constant below                        (code-level default)
         *
         * Supported formats: 'DD/MM/YYYY'  'MM/DD/YYYY'  'YYYY-MM-DD'
         */
        var MM_DATE_FORMAT = 'MM/DD/YYYY'; // ← change here to affect the whole file

        function applyDateFormatReplacement() {
            var bodyFormat = document.body.getAttribute('data-mm-date-format') || MM_DATE_FORMAT;

            var selector = '.form-table input[type="date"], .modal-body input[type="date"]';

            document.querySelectorAll(selector).forEach(function (original) {
                if (original.getAttribute('data-mm-date-replaced')) return;

                /* ── resolve format for this specific input ── */
                var fmt = original.getAttribute('data-mm-date-format') || bodyFormat;
                var sep = fmt.indexOf('/') !== -1 ? '/' : '-';

                /* ── helper: ISO → display ── */
                function toDisplay(iso) {
                    if (!iso) return '';
                    var p = iso.split('-');
                    if (p.length !== 3) return '';
                    return fmt
                        .replace('YYYY', p[0])
                        .replace('MM', p[1])
                        .replace('DD', p[2]);
                }

                /* ── helper: display → ISO ── */
                function toISO(display) {
                    var parts = display.split(sep);
                    if (parts.length !== 3) return '';
                    var keys = fmt.split(sep);
                    var map = {};
                    keys.forEach(function (k, i) { map[k] = parts[i]; });
                    return (map['YYYY'] || '') + '-' + (map['MM'] || '') + '-' + (map['DD'] || '');
                }

                /* ── helper: validate display value ── */
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

                /* ── 1. Keep the original input hidden in the DOM ──────────────
                 *   This is the key fix: instead of removing it and creating a
                 *   separate hidden field, we hide it in place.
                 *   ASP.NET's txtPaymentDate.Text still reads from it on postback
                 *   because the control is still in the control tree with its
                 *   original name/id intact. No C# changes needed anywhere.
                 * ─────────────────────────────────────────────────────────────── */
                original.style.display = 'none';
                original.setAttribute('data-mm-date-replaced', 'true');
                original.setAttribute('tabindex', '-1'); // exclude from tab order

                /* ── 2. Visible formatted text input ── */
                var textInput = document.createElement('input');
                textInput.type = 'text';
                textInput.className = original.className + ' mm-date-text';
                textInput.placeholder = fmt;
                textInput.maxLength = fmt.length;
                textInput.setAttribute('autocomplete', 'off');
                textInput.setAttribute('data-mm-date-format', fmt);
                textInput.setAttribute('data-original-id', original.id); // points to original
                console.log("Orignal Input HTML :", original);
                if (original.disabled) {
                    textInput.disabled = true;
                }

                /* Carry over min/max as data attributes for optional downstream guard */
                var minAttr = original.getAttribute('data-min') || original.getAttribute('min');
                var maxAttr = original.getAttribute('data-max') || original.getAttribute('max');
                if (minAttr) textInput.setAttribute('data-min', minAttr);
                if (maxAttr) textInput.setAttribute('data-max', maxAttr);

                /* Pre-fill display value from original's current value */
                if (original.value) textInput.value = toDisplay(original.value);

                /* ── Shared sync helper: writes ISO back to the original input ── */
                function syncToOriginal(iso) {
                    original.value = iso;
                    /* Notify ASP.NET validators that the value changed */
                    original.dispatchEvent(new Event('change', { bubbles: true }));
                }

                /* ── 3. Auto-masking: insert separator as user types digits ── */
                textInput.addEventListener('input', function () {
                    var raw = this.value.replace(/[^0-9]/g, '');
                    var masked = '';

                    /* Segment lengths depend on format */
                    var segs = (fmt === 'YYYY-MM-DD')
                        ? [[0, 4], [4, 6], [6, 8]]   // 4 – 2 – 2
                        : [[0, 2], [2, 4], [4, 8]];   // 2 – 2 – 4

                    for (var i = 0; i < segs.length; i++) {
                        var chunk = raw.slice(segs[i][0], segs[i][1]);
                        if (!chunk) break;
                        masked += (i > 0 ? sep : '') + chunk;
                    }

                    /* Prevent cursor jump: only update if value changed */
                    if (this.value !== masked) this.value = masked;

                    /* Best-effort sync while typing */
                    var iso = toISO(masked);
                    if (iso) syncToOriginal(iso);
                });

                /* ── 4. Blur: full validation + error/ok state ── */
                textInput.addEventListener('blur', function () {
                    var val = this.value.trim();

                    this.classList.remove('mm-date-error', 'mm-date-ok');

                    if (!val) {
                        syncToOriginal('');
                        return;
                    }

                    if (!isValidDisplay(val)) {
                        this.classList.add('mm-date-error');
                        showModalToast('Invalid date. Please use the format ' + fmt + '.', 'warning', 3500);
                        return;
                    }

                    /* Range check against data-min / data-max */
                    var iso = toISO(val);
                    var dMin = this.getAttribute('data-min');
                    var dMax = this.getAttribute('data-max');
                    if ((dMin && iso < dMin) || (dMax && iso > dMax)) {
                        this.classList.add('mm-date-error');
                        showModalToast('Date is outside the allowed range.', 'warning', 3500);
                        return;
                    }

                    this.classList.add('mm-date-ok');
                    syncToOriginal(iso);
                });

                /* ── 5. Ensure original is synced before postback ── */
                document.querySelectorAll(
                    'input[type="submit"], a[id*="btnSave"], a[id*="btnCreate"], a[id*="btnSubmit"], a[id*="btnUpdate"]'
                ).forEach(function (btn) {
                    btn.addEventListener('click', function () {
                        textInput.dispatchEvent(new Event('blur'));
                    });
                });

                /* ── 6. Wrap input + calendar icon, insert BEFORE the original ── */
                var wrapper = document.createElement('span');
                wrapper.className = 'mm-date-wrapper';

                var icon = document.createElement('span');
                icon.className = 'mm-date-icon';
                icon.setAttribute('aria-hidden', 'true');
                icon.setAttribute('title', 'Pick a date');
                icon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor">' +
                    '<path d="M3.5 0a.5.5 0 0 1 .5.5V1h8V.5a.5.5 0 0 1 1 0V1h1a2 2 0 0 1 2 2v11a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V3a2 2 0 0 1 2-2h1V.5a.5.5 0 0 1 .5-.5zM1 4v10a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V4H1zm1-2a1 1 0 0 0-1 1v.5h14V3a1 1 0 0 0-1-1H2z"/>' +
                    '</svg>';

                if (original.disabled) {
                    icon.classList.add('disabled');
                }
                icon.addEventListener('mousedown', function (e) {
                    e.preventDefault();

                    if (!textInput.disabled) {
                        textInput.focus();
                    }
                });

                wrapper.appendChild(textInput);
                wrapper.appendChild(icon);

                /* Insert wrapper before the original (which stays hidden in place) */
                original.parentNode.insertBefore(wrapper, original);
            });
        }

        /* ─── 13. Pikaday calendar picker ──────────────────────────── */
        /*
         * Dynamically loads Pikaday (CSS + JS) from cdnjs, then attaches a
         * calendar picker to every .mm-date-text input.
         * When the user picks a day Pikaday writes directly to the visible input
         * AND syncs the hidden ISO field — no extra wiring needed.
         *
         * Format bridging:
         *   Pikaday works in plain JS Date objects internally.
         *   We use its toString() / parse() hooks to match whatever MM_DATE_FORMAT
         *   is set to, so the visible field always shows the right format.
         */

        var PIKADAY_CSS = 'https://cdnjs.cloudflare.com/ajax/libs/pikaday/1.8.2/css/pikaday.min.css';
        var PIKADAY_JS = 'https://cdnjs.cloudflare.com/ajax/libs/pikaday/1.8.2/pikaday.min.js';

        var _pikadayReady = false;
        var _pikadayQueue = []; // callbacks waiting for Pikaday to load

        function loadPikaday(cb) {
            if (_pikadayReady) { cb(); return; }
            _pikadayQueue.push(cb);
            if (_pikadayQueue.length > 1) return; // already loading

            /* Inject CSS */
            if (!document.getElementById('mm-pikaday-css')) {
                var link = document.createElement('link');
                link.id = 'mm-pikaday-css';
                link.rel = 'stylesheet';
                link.href = PIKADAY_CSS;
                document.head.appendChild(link);
            }

            /* Inject JS */
            var script = document.createElement('script');
            script.src = PIKADAY_JS;
            script.async = true;
            script.onload = function () {
                _pikadayReady = true;
                _pikadayQueue.forEach(function (fn) { fn(); });
                _pikadayQueue = [];
            };
            script.onerror = function () {
                console.warn('[modal-modern] Pikaday failed to load. Falling back to keyboard-only.');
                _pikadayQueue = [];
            };
            document.head.appendChild(script);
        }

        function attachPikaday(textInput, fmt, sep, toISO, toDisplay, isValidDisplay) {
            if (textInput.dataset.mmPikaday) return; // already attached
            textInput.dataset.mmPikaday = '1';

            /* Read min/max from data attributes if present */
            var minAttr = textInput.getAttribute('data-min');
            var maxAttr = textInput.getAttribute('data-max');

            var pikadayOpts = {
                field: textInput,
                format: fmt,
                toString: function (date) {
                    /* Format the JS Date object into the display string */
                    var y = date.getFullYear();
                    var m = String(date.getMonth() + 1).padStart(2, '0');
                    var d = String(date.getDate()).padStart(2, '0');
                    return fmt
                        .replace('YYYY', y)
                        .replace('MM', m)
                        .replace('DD', d);
                },
                parse: function (str) {
                    /* Parse the display string back into a JS Date */
                    var parts = str.split(sep);
                    var keys = fmt.split(sep);
                    var map = {};
                    keys.forEach(function (k, i) { map[k] = parseInt(parts[i], 10); });
                    return new Date(map['YYYY'], map['MM'] - 1, map['DD']);
                },
                onSelect: function (date) {
                    /* Sync original hidden date input when user picks from calendar */
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

                    /* Update visible field via toString hook (Pikaday does this,
                       but we also set it explicitly to be safe) */
                    textInput.value = pikadayOpts.toString(date);

                    /* Mark as valid */
                    textInput.classList.remove('mm-date-error');
                    textInput.classList.add('mm-date-ok');

                   
                },
                i18n: {
                    previousMonth: 'Previous Month',
                    nextMonth: 'Next Month',
                    months: ['January', 'February', 'March', 'April', 'May', 'June',
                        'July', 'August', 'September', 'October', 'November', 'December'],
                    weekdays: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
                    weekdaysShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
                },
                firstDay: 1  /* Monday first — change to 0 for Sunday */
            };

            /* Apply min/max bounds to the Pikaday calendar */
            if (minAttr) {
                var minParts = minAttr.split('-');
                pikadayOpts.minDate = new Date(
                    parseInt(minParts[0], 10),
                    parseInt(minParts[1], 10) - 1,
                    parseInt(minParts[2], 10)
                );
            }
            if (maxAttr) {
                var maxParts = maxAttr.split('-');
                pikadayOpts.maxDate = new Date(
                    parseInt(maxParts[0], 10),
                    parseInt(maxParts[1], 10) - 1,
                    parseInt(maxParts[2], 10)
                );
            }

            /* Pre-select current value in the calendar if the field is pre-filled */
            if (textInput.value && isValidDisplay(textInput.value)) {
                pikadayOpts.defaultDate = pikadayOpts.parse(textInput.value);
                pikadayOpts.setDefaultDate = true;
            }

            new Pikaday(pikadayOpts); // eslint-disable-line no-undef
        }

        function applyPikaday() {
            var inputs = document.querySelectorAll('.mm-date-text:not([data-mm-pikaday])');
            if (!inputs.length) return;

            loadPikaday(function () {
                inputs.forEach(function (textInput) {
                    var fmt = textInput.getAttribute('data-mm-date-format') ||
                        document.body.getAttribute('data-mm-date-format') ||
                        MM_DATE_FORMAT;
                    var sep = fmt.indexOf('/') !== -1 ? '/' : '-';

                    /* Recreate the same helpers used during replacement */
                    function toISO(display) {
                        var parts = display.split(sep);
                        if (parts.length !== 3) return '';
                        var keys = fmt.split(sep);
                        var map = {};
                        keys.forEach(function (k, i) { map[k] = parts[i]; });
                        return (map['YYYY'] || '') + '-' + (map['MM'] || '') + '-' + (map['DD'] || '');
                    }

                    function toDisplay(iso) {
                        if (!iso) return '';
                        var p = iso.split('-');
                        if (p.length !== 3) return '';
                        return fmt.replace('YYYY', p[0]).replace('MM', p[1]).replace('DD', p[2]);
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

                    attachPikaday(textInput, fmt, sep, toISO, toDisplay, isValidDisplay);
                });
            });
        }

        /* Expose globally so Modal.master's showNotificationMessage can call it */
        window.showModalToast = showModalToast;

        /* ─── Inject minimal keyframe + date styles ─────────────────── */
        function injectStyles() {
            if (document.getElementById('mm-keyframes')) return;
            var s = document.createElement('style');
            s.id = 'mm-keyframes';
            s.textContent = [
                '@keyframes mm-spin { to { transform: rotate(360deg); } }',
                '.mm-required:invalid { border-color: var(--mm-danger,#c4314b) !important; box-shadow: 0 0 0 1px var(--mm-danger,#c4314b) !important; }',
                /* ── date text input styles ── */
                '.mm-date-wrapper { position: relative; display: inline-block; width: 100%; }',
                '.mm-date-text { letter-spacing: 0.04em; padding-right: 28px !important; width: 100%; box-sizing: border-box; }',
                '.mm-date-text.mm-date-error { border-color: var(--mm-danger,#c4314b) !important; background-color: #fff5f5 !important; }',
                '.mm-date-text.mm-date-ok   { border-color: var(--mm-success,#27ae60) !important; }',
                '.mm-date-icon { position: absolute; right: 7px; top: 50%; transform: translateY(-50%); width: 14px; height: 14px; color: var(--mm-text-disabled,#a19f9d); pointer-events: auto; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: color .15s; }',
                '.mm-date-icon:hover { color: var(--mm-primary,#0078d4); }',
                '.mm-date-icon svg { width: 14px; height: 14px; display: block; }',
                /* ── Pikaday theme overrides to match portal styling ── */
                '.pika-single { font-family: var(--mm-font,\'Segoe UI\',sans-serif); font-size: 13px; border-radius: 4px; box-shadow: 0 4px 16px rgba(0,0,0,.12); }',
                '.pika-button:hover { background: var(--mm-primary,#0078d4) !important; color: #fff !important; }',
                '.is-selected .pika-button { background: var(--mm-primary,#0078d4) !important; }'
            ].join('\n');
            document.head.appendChild(s);
        }

    /* ═══════════════════════════════════════════════════════════════════
SECTION 1 — Status Badge Colouring
═══════════════════════════════════════════════════════════════════ */
    var STATUS_MAP = {
        'not submitted': 'lp-badge-notsubmitted',
        'Draft': 'lp-badge-notsubmitted',
        'draft': 'lp-badge-notsubmitted',
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
        'in review': 'lp-badge-pending',
        'cancelled': 'lp-badge-notsubmitted',
        'closed': 'lp-badge-notsubmitted',
        'open': 'lp-badge-approved',
        'active': 'lp-badge-submitted',
        'inactive': 'lp-badge-notsubmitted'
    };

    function escapeHtml(s) {
        return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
    function applyStatusBadges() {
        document.querySelectorAll('table[id*="gridView"] tbody td span').forEach(function (span) {
            if (span.classList.contains('lp-badge')) return;
            if (span.querySelector('input, select, a, button')) return;
            if (span.closest('td').querySelector('input, select, a, button')) return;

            var raw = (span.textContent || span.innerText || '').trim();
            var key = raw.toLowerCase();
            console.log("SPAN ITEMS KEY:", key);
            if (STATUS_MAP[key]) {
                console.log("SPAN ITEMS KEY Found:", key);
                span.textContent = '';  // clear text
                var badge = document.createElement('span');
                badge.className = 'lp-badge ' + STATUS_MAP[key];
                badge.textContent = raw;
                console.log("Badge CREATED:", badge);
                span.appendChild(badge);
                console.log("SPAN AFTER ADDING Badge:", span);
            }
        });
    }


    /* ─── Master init ───────────────────────────────────────────── */
    function init() {
        if (!isModalContext()) return;

        applyStatusBadges();
        injectStyles();
        enhancePageTitle();
        markRequiredFields();
        applyTextareaCounters();
        applyAutoResize();
        applyButtonLoadingState();
        // trackDirtyState();
        // resetDirtyOnSubmit();
        applyCtrlEnterSubmit();
        applyScrollToError();
        applyNumberFormatting();
        applyDateFormatReplacement(); // 12 — must run before guard so type="date" inputs are gone
        applyDateGuards();            // 11 — guards the newly created mm-date-text inputs too
        applyPikaday();               // 13 — attach calendar picker to all mm-date-text inputs
    }

    ready(function () {
        setTimeout(init, 30);
    });

    /* Re-run after UpdatePanel postbacks */
    if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setTimeout(init, 50);
        });
    }

    window.modalModernInit = init;

})();