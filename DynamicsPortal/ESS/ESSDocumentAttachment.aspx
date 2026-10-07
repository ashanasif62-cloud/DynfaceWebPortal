<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSDocumentAttachment.aspx.cs" Inherits="DynamicsPortal.ESSDocumentAttachment" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function showBrowseDialog() {
            var userImgUpload = document.getElementById('PageContent_fileUpload');
            userImgUpload.click();
            return false;
        }
        function upload() {
            var btnUpload = document.getElementById('PageContent_btnUpload');
            btnUpload.click();
        }
    </script>

    <style>

       html, body { height: 100%; margin: 0; overflow: hidden; }

/* Target the master's outer container */
div[style*="height: 100vh"] {
    display: flex !important;
    flex-direction: column !important;
    padding: 0 !important;
}

/* Hide the page title bar — it's empty/unnecessary in a modal */
#pageTitle { display: none !important; }

/* Force the ContentPlaceHolder wrapper to expand */
div[style*="height: 100vh"] > div:last-child {
    flex: 1 !important;
    display: flex !important;
    flex-direction: column !important;
    overflow: hidden !important;
    min-height: 0 !important;
}

.ess-attach,
.ess-attach .panel {
    flex: 1;
    display: flex;
    flex-direction: column;
    min-height: 0;
    height: 100%;
}
        /* ── Reset & base ───────────────────────────── */
        .ess-attach * { box-sizing: border-box; 
        }

        /* ── Outer panel ────────────────────────────── */
        .ess-attach .panel {
            background: #ffffff;
            border: 1px solid #e5e7eb;
            border-radius: 12px;
            overflow: hidden;
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
            width: 100%;
            display: flex;
            flex-direction: column;
             height: 100%;   /* 🔥 ADD THIS */
        }

        /* ── Panel header ───────────────────────────── */
        .ess-attach .panel-header {
            padding: 14px 20px;
            border-bottom: 1px solid #f0f0f0;
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #fafafa;
            flex-shrink: 0;
        }

        .ess-attach .panel-title {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 14px;
            font-weight: 600;
            color: #111827;
        }

        .ess-attach .panel-title i {
            font-size: 17px;
            color: #6b7280;
        }

        .ess-attach .file-count-badge {
            background: #f3f4f6;
            border: 1px solid #e5e7eb;
            border-radius: 20px;
            font-size: 11px;
            color: #6b7280;
            padding: 2px 10px;
            font-weight: 500;
        }

        /* ── Toolbar ────────────────────────────────── */
        .ess-attach .toolbar {
            padding: 10px 16px;
            display: flex;
            align-items: center;
            gap: 6px;
            border-bottom: 1px solid #f0f0f0;
            background: #f9fafb;
            flex-wrap: wrap;
            flex-shrink: 0;
        }

        .ess-attach .toolbar-sep {
            width: 1px;
            height: 18px;
            background: #e5e7eb;
            margin: 0 2px;
        }

        /* ── Buttons ────────────────────────────────── */
        .ess-attach .btn {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 6px 14px;
            border-radius: 7px;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            border: 1px solid #d1d5db;
            background: #ffffff;
            color: #374151;
            text-decoration: none;
            transition: background 0.15s, border-color 0.15s, transform 0.1s;
            line-height: 1.4;
        }

        .ess-attach .btn:hover {
            background: #f3f4f6;
            border-color: #9ca3af;
            text-decoration: none;
            color: #111827;
        }

        .ess-attach .btn:active { transform: scale(0.97); }
        .ess-attach .btn i { font-size: 15px; }

        .ess-attach .btn-upload {
            background: #111827;
            color: #ffffff;
            border-color: #111827;
        }

        .ess-attach .btn-upload:hover {
            background: #1f2937;
            border-color: #1f2937;
            color: #ffffff;
        }

        .ess-attach .btn-danger { color: #b91c1c; border-color: #fca5a5; }
        .ess-attach .btn-danger:hover { background: #fff1f1; border-color: #ef4444; }

        .ess-attach .btn-close {
            color: #374151;
            border-color: #d1d5db;
            background: #ffffff;
        }
        .ess-attach .btn-close:hover {
            background: #f3f4f6;
            border-color: #9ca3af;
            color: #111827;
        }

        /* ── Inline notification ────────────────────── */
        .ess-attach .notif {
            margin: 14px 18px 0;
            padding: 9px 14px;
            border-radius: 8px;
            font-size: 13px;
            display: none;
            align-items: center;
            gap: 8px;
            flex-shrink: 0;
        }

        .ess-attach .notif.show { display: flex; }
        .ess-attach .notif-error   { background: #fff1f1; color: #b91c1c; border: 1px solid #fca5a5; }
        .ess-attach .notif-success { background: #f0fdf4; color: #15803d; border: 1px solid #86efac; }

        /* ── Upload drop zone ───────────────────────── */
/*        .ess-attach .upload-zone {
            border: 2px dashed #d1d5db;
            border-radius: 10px;
            padding: 200px 150px;
            text-align: center;
            color: #9ca3af;
            font-size: 13px;
            margin-bottom: 18px;
            cursor: pointer;
            transition: border-color 0.2s, background 0.2s;
        }

        .ess-attach .upload-zone:hover,
        .ess-attach .upload-zone.drag-over {
            border-color: #3b82f6;
            background: #eff6ff;
            color: #3b82f6;
        }

        .ess-attach .upload-zone.drag-over i,
        .ess-attach .upload-zone.drag-over strong {
            color: #3b82f6;
        }

        .ess-attach .upload-zone i {
            font-size: 30px;
            display: block;
            margin-bottom: 8px;
            color: #d1d5db;
            transition: color 0.2s;
        }

        .ess-attach .upload-zone strong {
            display: block;
            font-weight: 600;
            color: #374151;
            margin-bottom: 4px;
            font-size: 14px;
            transition: color 0.2s;
        }*/

        /* ── Files area ─────────────────────────────── */
        
.ess-attach .files-area {
    flex: 1;
    overflow-y: auto;
    min-height: 0;
}

        /* ── File grid ──────────────────────────────── */
        .ess-attach .files-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
            gap: 10px;
        }

        /* ── File card ──────────────────────────────── */
        .ess-attach .file-card {
            background: #ffffff;
            border: 1px solid #e5e7eb;
            border-radius: 9px;
            padding: 13px 11px 11px;
            cursor: pointer;
            transition: border-color 0.15s, background 0.15s;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 7px;
            position: relative;
            user-select: none;
        }

        .ess-attach .file-card:hover { border-color: #9ca3af; background: #f9fafb; }

        .ess-attach .file-card.selected {
            border: 2px solid #3b82f6;
            background: #eff6ff;
        }

        /* ── Checkbox circle ────────────────────────── */
        .ess-attach .check-circle {
            position: absolute;
            top: 8px;
            right: 8px;
            width: 17px;
            height: 17px;
            border-radius: 50%;
            border: 1px solid #d1d5db;
            background: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.15s;
        }

        .ess-attach .file-card.selected .check-circle {
            background: #3b82f6;
            border-color: #3b82f6;
        }

        .ess-attach .check-circle i { font-size: 10px; color: #fff; display: none; }
        .ess-attach .file-card.selected .check-circle i { display: block; }

        /* ── File icon ──────────────────────────────── */
        .ess-attach .file-icon-wrap {
            width: 42px;
            height: 50px;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
        }

        .ess-attach .file-icon-wrap i          { font-size: 48px; color: #d1d5db; }

        .ess-attach .file-icon-wrap .mdi-file-pdf-box   { color: #ef4444; }
        .ess-attach .file-icon-wrap .mdi-file-word-box  { color: #2b7cd3; }
        .ess-attach .file-icon-wrap .mdi-file-excel-box { color: #1d6f42; }
        .ess-attach .file-icon-wrap .mdi-file-image     { color: #a855f7; }
        .ess-attach .file-icon-wrap .mdi-file-outline   { color: #9ca3af; }

        .ess-attach .ext-badge {
            position: absolute;
            bottom: -1px;
            right: -7px;
            font-size: 8px;
            font-weight: 700;
            padding: 2px 5px;
            border-radius: 3px;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }

        .ess-attach .ext-pdf { background: #fee2e2; color: #991b1b; }
        .ess-attach .ext-doc { background: #dbeafe; color: #1e40af; }
        .ess-attach .ext-img { background: #dcfce7; color: #166534; }
        .ess-attach .ext-def { background: #f3f4f6; color: #4b5563; }

        .ess-attach .file-name {
            font-size: 11px;
            color: #374151;
            text-align: center;
            line-height: 1.4;
            word-break: break-word;
            max-width: 100%;
            overflow: hidden;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
        }

        .ess-attach .file-meta { font-size: 10px; color: #9ca3af; }

        /* ── Footer bar — pinned to bottom of panel ── */
        .ess-attach .footer-bar {
            padding: 12px 18px;
            border-top: 1px solid #f0f0f0;
            display: flex;
            align-items: center;
            justify-content: flex-end;
            background: #fafafa;
            flex-shrink: 0;
        }

        /* ── Embed viewer ───────────────────────────── */
        .ess-attach .viewer-wrap { padding: 0 18px; }
    </style>
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <!-- Hidden ASP.NET controls -->
    <asp:Button ID="btnUpload" runat="server"
        Style="display:none !important"
        OnClientClick="return confirm('Are you sure you want to add this attachment?')"
        OnClick="btnNew_Click" />

    <asp:FileUpload ID="fileUpload" runat="server"
        Style="display:none"
        accept="application/pdf, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, image/*"
        onchange="upload();" />

    <!-- UI Shell -->
    <div class="ess-attach">
        <div class="panel">

            <!-- Header -->
            <div class="panel-header">
                <div class="panel-title">
                    <i class="mdi mdi-paperclip"></i>
                    Document Attachments
                </div>
                <span class="file-count-badge" id="fileCountBadge">0 files</span>
            </div>

            <!-- Toolbar -->
            <div class="toolbar">
                <asp:LinkButton ID="btnNew" runat="server"
                    CssClass="btn btn-upload"
                    OnClientClick="return showBrowseDialog();">
                    <i class="mdi mdi-plus"></i> Upload
                </asp:LinkButton>

                <div class="toolbar-sep"></div>

                <asp:LinkButton ID="btnDownload" runat="server"
                    CssClass="btn"
                    OnClientClick="return checkSelection(event)"
                    OnClick="btnDownload_Click">
                    <i class="mdi mdi-cloud-download"></i> Download
                </asp:LinkButton>

                <asp:LinkButton ID="btnDelete" runat="server"
                    CssClass="btn btn-danger"
                    OnClientClick="return checkSelection(event)"
                    OnClick="btnDelete_Click">
                    <i class="mdi mdi-delete"></i> Delete
                </asp:LinkButton>
            </div>

            <!-- Inline notification -->
            <div id="inlineNotif" class="notif" role="alert"></div>

            <!-- Files area -->
            <div class="files-area">
                <div class="upload-zone" id="uploadZone"
                     onclick="showBrowseDialog()" role="button" tabindex="0"
                     onkeydown="if(event.key==='Enter')showBrowseDialog()">
                    <i class="mdi mdi-cloud-upload-outline"></i>
                </div>

                <div id="fileList" runat="server" class="files-grid"></div>
            </div>

            <!-- Embed viewer -->
            <div class="viewer-wrap">
                <embed id="embed01" runat="server" />
            </div>

            <!-- Footer: Close button — inside panel, pinned bottom right -->
            <div class="footer-bar">
              <asp:Button ID="btnCancel" runat="server" Text="Close"
    CssClass="btn btn-secondary"
    OnClientClick="document.dispatchEvent(new KeyboardEvent('keydown',{key:'Escape',keyCode:27,which:27,bubbles:true})); return false;" />
            </div>

        </div>
    </div>

    <script type="text/javascript">



        /* ── Drag & Drop ─────────────────────────────────────── */
        (function () {
            var zone = document.getElementById('uploadZone');
            var fileInput = document.getElementById('PageContent_fileUpload');
            if (!zone || !fileInput) return;

            // Prevent browser from opening the file on a missed drop
            ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(function (evt) {
                document.body.addEventListener(evt, function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                });
            });

            zone.addEventListener('dragenter', function () {
                zone.classList.add('drag-over');
            });

            zone.addEventListener('dragover', function () {
                zone.classList.add('drag-over');
            });

            zone.addEventListener('dragleave', function (e) {
                // Only remove if leaving the zone entirely (not a child element)
                if (!zone.contains(e.relatedTarget)) {
                    zone.classList.remove('drag-over');
                }
            });

            zone.addEventListener('drop', function (e) {
                zone.classList.remove('drag-over');
                var files = e.dataTransfer.files;
                if (!files || files.length === 0) return;

                var file = files[0]; // single-file upload to match server logic

                // Validate type client-side before submitting
                var allowed = [
                    'application/pdf',
                    'application/msword',
                    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
                    'application/vnd.ms-excel',
                    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                ];
                var isImage = file.type.indexOf('image/') === 0;
                var isAllowed = isImage || allowed.indexOf(file.type) !== -1;

                if (!isAllowed) {
                    showNotif('Only PDF, Word, or image files are allowed.', 'error');
                    return;
                }

                if (file.size >= 5000000) {
                    showNotif('File size must be less than 5 MB.', 'error');
                    return;
                }

                // Assign the dropped file to the hidden file input and trigger upload
                try {
                    var dt = new DataTransfer();
                    dt.items.add(file);
                    fileInput.files = dt.files;
                    upload(); // triggers the hidden btnUpload click → server PostBack
                } catch (err) {
                    // DataTransfer not supported (old IE/Edge) — fall back to dialog
                    showNotif('Drag & drop not supported in this browser. Please use the Upload button.', 'error');
                }
            });
        })();

        /* ── Selection logic ─────────────────────────────────── */
        function selectAttachment(el) {
            var cards = document.querySelectorAll('.download-attachment');
            var input = el.querySelector('input[type=checkbox]');
            var alreadyChecked = input && input.checked;

            cards.forEach(function (c) {
                var cb = c.querySelector('input[type=checkbox]');
                if (cb) cb.checked = false;
                c.classList.remove('selected');
            });

            if (!alreadyChecked) {
                if (input) input.checked = true;
                el.classList.add('selected');
            }

            return false;
        }

        function checkSelection(event) {
            var selected = document.querySelector('.download-attachment input[type=checkbox]:checked');
            if (!selected) {
                showNotif('Please select a file first.', 'error');
                if (event) { event.stopPropagation(); event.preventDefault(); }
                return false;
            }
            return true;
        }

        /* ── Notifications ───────────────────────────────────── */
        function showNotif(msg, type) {
            var el = document.getElementById('inlineNotif');
            if (!el) return;
            el.className = 'notif show notif-' + type;
            el.innerHTML = '<i class="mdi mdi-' + (type === 'error' ? 'alert-circle' : 'check-circle') + '"></i> ' + msg;
            clearTimeout(el._t);
            el._t = setTimeout(function () { el.className = 'notif'; }, 4000);
        }

        function showNotificationMessage(msg, type) {
            showNotif(msg, type === 'Error' ? 'error' : 'success');
        }

        /* ── File-count badge ────────────────────────────────── */
        function updateFileCount() {
            var badge = document.getElementById('fileCountBadge');
            if (!badge) return;
            var n = document.querySelectorAll('.download-attachment').length;
            badge.textContent = n + (n === 1 ? ' file' : ' files');
        }

        document.addEventListener('DOMContentLoaded', function () {
            updateFileCount();

            // ── Notify parent page of attachment count ──
            try {
                var count = document.querySelectorAll('.download-attachment').length;
                var ref = '<%= HttpUtility.JavaScriptStringEncode(Request.QueryString["ref"] ?? "") %>';
                    if (window.parent && window.parent.updateAttachmentBadge) {
                        window.parent.updateAttachmentBadge(ref, count);
                    }
                } catch (ex) { }

            // Hide upload zone when page is read-only (e.g. HR Policies)
            ////if (<%= isReadOnly.ToString().ToLower() %>) {
            //    var zone = document.getElementById('uploadZone');
            //    if (zone) zone.style.display = 'none';
            //}
        });
    </script>
</asp:Content>
