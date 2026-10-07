<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="HRPersonalContacts_Update.aspx.cs" Inherits="DynamicsPortal.ESS.HR.HRPersonalContacts_Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<style>
    .form-header {
        font-size: 18px;
        font-weight: 600;
        margin-bottom: 12px;
    }

    .form-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 16px;
    }

    .form-group label {
        font-size: 12px;
        color: #555;
        margin-bottom: 4px;
        display: block;
    }

    .fixed-width {
        width: 200px;
    }

    /* ===== TOGGLE SWITCH ===== */
    .toggle-switch {
        position: relative;
        display: inline-block;
        width: 50px;
        height: 24px;
    }

    .toggle-switch input {
        opacity: 0;
        width: 0;
        height: 0;
    }

    .slider {
        position: absolute;
        cursor: pointer;
        top: 0; left: 0;
        right: 0; bottom: 0;
        background-color: #ccc;
        transition: 0.3s;
        border-radius: 12px;
    }

    .slider:before {
        position: absolute;
        content: "";
        height: 20px;
        width: 20px;
        left: 2px;
        bottom: 2px;
        background-color: white;
        transition: 0.3s;
        border-radius: 50%;
    }

    input:checked + .slider {
        background-color: #0d6efd;
    }

    input:checked + .slider:before {
        transform: translateX(26px);
    }

    #dependantFields,
    #beneficiaryFields {
        display: none;
    }

    .form-row {
        display: flex;
        align-items: center;
        gap: 16px;
    }
</style>
    <style>
    /* ── Variables ───────────────────────────────────────── */
    :root {
        --c-bg:        #f8f9fb;
        --c-surface:   #ffffff;
        --c-border:    #e4e7ec;
        --c-border-md: #d0d5dd;
        --c-text:      #101828;
        --c-muted:     #667085;
        --c-accent:    #2563eb;
        --c-accent-h:  #1d4ed8;
        --c-danger:    #d92d20;
        --c-success:   #12b76a;
        --c-tog-off:   #d0d5dd;
        --radius:      8px;
        --shadow-sm:   0 1px 2px rgba(16,24,40,.06), 0 1px 3px rgba(16,24,40,.1);
        --shadow-md:   0 4px 8px -2px rgba(16,24,40,.08), 0 2px 4px -2px rgba(16,24,40,.06);
    }

    /* ── Reset ───────────────────────────────────────────── */
    *, *::before, *::after { box-sizing: border-box; }

    body { background: var(--c-bg); color: var(--c-text); font-family: 'Segoe UI', system-ui, sans-serif; }

    label { display: inline-block; margin-bottom: 0; }

    /* ── Page shell ──────────────────────────────────────── */
    .pf-shell {
        background: var(--c-surface);
        border: 1px solid var(--c-border);
        border-radius: var(--radius);
        box-shadow: var(--shadow-md);
        padding: 24px;
        max-width: 860px;
        margin: 0 auto;
    }

    /* ── Page header ─────────────────────────────────────── */
    .pf-header {
        display: flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 20px;
        padding-bottom: 16px;
        border-bottom: 1px solid var(--c-border);
    }
    .pf-header-icon {
        width: 36px; height: 36px;
        background: #eff6ff;
        border: 1px solid #bfdbfe;
        border-radius: 8px;
        display: flex; align-items: center; justify-content: center;
        color: var(--c-accent);
        font-size: 16px;
        flex-shrink: 0;
    }
    .pf-header h2 {
        font-size: 15px;
        font-weight: 600;
        color: var(--c-text);
        margin: 0;
    }
    .pf-header p {
        font-size: 12px;
        color: var(--c-muted);
        margin: 0;
    }

    /* ── Section label ───────────────────────────────────── */
    .pf-section-label {
        font-size: 11px;
        font-weight: 600;
        letter-spacing: .06em;
        text-transform: uppercase;
        color: var(--c-muted);
        margin: 0 0 12px;
    }

    /* ── Form grid ───────────────────────────────────────── */
    .pf-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 14px 20px;
        margin-bottom: 20px;
    }
    @media (max-width: 640px) { .pf-grid { grid-template-columns: 1fr; } }

    /* ── Form group ──────────────────────────────────────── */
    .pf-group { display: flex; flex-direction: column; gap: 5px; }
    .pf-label {
        font-size: 12px;
        font-weight: 500;
        color: #344054;
    }
    .pf-group .form-control {
        width: 100%;
        height: 36px;
        font-size: 13px;
        border: 1px solid var(--c-border-md);
        border-radius: 6px;
        padding: 0 10px;
        color: var(--c-text);
        background: var(--c-surface);
        transition: border-color .15s, box-shadow .15s;
        outline: none;
    }
    .pf-group .form-control:focus {
        border-color: var(--c-accent);
        box-shadow: 0 0 0 3px rgba(37,99,235,.12);
    }
    select.form-control { padding: 0 8px; cursor: pointer; }

    /* ── Accordion ───────────────────────────────────────── */
    .pf-accordion { display: flex; flex-direction: column; gap: 8px; margin-bottom: 20px; }

    .pf-card {
        border: 1px solid var(--c-border);
        border-radius: var(--radius);
        overflow: hidden;
        transition: box-shadow .2s;
    }
    .pf-card:hover { box-shadow: var(--shadow-sm); }

    .pf-card-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 10px 14px;
        background: #f9fafb;
        cursor: pointer;
        border-bottom: 1px solid transparent;
        transition: background .15s;
        user-select: none;
    }
    .pf-card-header:hover { background: #f3f4f6; }
    .pf-card-header.open { border-bottom-color: var(--c-border); }

    .pf-card-title {
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 13px;
        font-weight: 600;
        color: var(--c-text);
    }
    .pf-card-title-icon {
        width: 24px; height: 24px;
        border-radius: 6px;
        display: flex; align-items: center; justify-content: center;
        font-size: 12px;
    }
    .icon-general   { background: #fef3c7; color: #d97706; }
    .icon-beneficiary { background: #dcfce7; color: #16a34a; }
    .icon-dependant { background: #ede9fe; color: #7c3aed; }

    .pf-chevron {
        font-size: 11px;
        color: var(--c-muted);
        transition: transform .2s;
    }
    .pf-card-header.open .pf-chevron { transform: rotate(180deg); }

    .pf-card-body {
        padding: 16px;
        display: none;
        background: var(--c-surface);
    }
    .pf-card-body.open { display: block; animation: slideDown .2s ease; }

    @keyframes slideDown {
        from { opacity: 0; transform: translateY(-4px); }
        to   { opacity: 1; transform: translateY(0); }
    }

    /* ── Toggle switch ───────────────────────────────────── */
    .pf-toggle-row {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 10px 0;
        border-bottom: 1px solid var(--c-border);
    }
    .pf-toggle-row:last-child { border-bottom: none; }

    .pf-toggle-info { display: flex; flex-direction: column; gap: 1px; }
    .pf-toggle-info span { font-size: 13px; font-weight: 500; color: var(--c-text); }
    .pf-toggle-info small { font-size: 11px; color: var(--c-muted); }

    .toggle-switch {
        position: relative;
        display: inline-block;
        width: 44px;
        height: 24px;
        flex-shrink: 0;
    }
    .toggle-switch input { opacity: 0; width: 0; height: 0; }
    .slider {
        position: absolute;
        inset: 0;
        background: var(--c-tog-off);
        border-radius: 12px;
        cursor: pointer;
        transition: background .25s;
    }
    .slider::before {
        content: '';
        position: absolute;
        width: 18px; height: 18px;
        left: 3px; top: 3px;
        background: #fff;
        border-radius: 50%;
        transition: transform .25s;
        box-shadow: 0 1px 3px rgba(0,0,0,.2);
    }
    input:checked + .slider { background: var(--c-accent); }
    input:checked + .slider::before { transform: translateX(20px); }
    input:disabled + .slider { opacity: .4; cursor: not-allowed; }

    /* ── Sub-fields (date pairs etc.) ────────────────────── */
    .pf-subfields {
        margin-top: 14px;
        padding: 14px;
        background: #f9fafb;
        border: 1px solid var(--c-border);
        border-radius: 6px;
        display: none;
    }
    .pf-subfields.visible { display: block; animation: slideDown .2s ease; }

    .pf-row { display: flex; gap: 14px; flex-wrap: wrap; }
    .pf-row .pf-group { flex: 1; min-width: 140px; }

    /* ── Footer ──────────────────────────────────────────── */
    .pf-footer {
        display: flex;
        align-items: center;
        justify-content: flex-end;
        gap: 10px;
        padding-top: 16px;
        border-top: 1px solid var(--c-border);
    }

    .btn-primary-pf {
        height: 36px;
        padding: 0 18px;
        background: var(--c-accent);
        color: #fff;
        font-size: 13px;
        font-weight: 500;
        border: none;
        border-radius: 6px;
        cursor: pointer;
        transition: background .15s, box-shadow .15s;
        display: inline-flex; align-items: center; gap: 6px;
    }
    .btn-primary-pf:hover { background: var(--c-accent-h); box-shadow: 0 0 0 3px rgba(37,99,235,.2); }

    .btn-ghost-pf {
        height: 36px;
        padding: 0 16px;
        background: var(--c-surface);
        color: #344054;
        font-size: 13px;
        font-weight: 500;
        border: 1px solid var(--c-border-md);
        border-radius: 6px;
        cursor: pointer;
        transition: background .15s;
        text-decoration: none;
        display: inline-flex; align-items: center; gap: 6px;
    }
    .btn-ghost-pf:hover { background: #f9fafb; text-decoration: none; color: #344054; }
</style>

<script>
    document.addEventListener("DOMContentLoaded", function () {

        /* ── Accordion ── */
        document.querySelectorAll('.pf-card-header').forEach(function (hdr) {
            hdr.addEventListener('click', function () {
                var body = hdr.nextElementSibling;
                var open = body.classList.toggle('open');
                hdr.classList.toggle('open', open);
            });
        });

        /* ── Field references ── */
        const ddlRelationship = document.getElementById('<%= ddlRelationship.ClientID %>');
        const chkDependant = document.getElementById('<%= ChkDependant.ClientID %>');
        const chkBeneficiary  = document.getElementById('<%= ChkBeneficiary.ClientID %>');

        const subDependant = document.getElementById('subDependant');
        const subBeneficiary = document.getElementById('subBeneficiary');

        const cardDep = document.getElementById('cardDependant');
        const cardBen = document.getElementById('cardBeneficiary');

        /* ── Dependant logic ── */
        function applyRelationshipRules() {
            if (ddlRelationship.value === "FamilyContact") {
                chkDependant.checked = false;
                chkDependant.disabled = true;
                subDependant.classList.remove('visible');
                cardDep.querySelector('.pf-card-body').classList.remove('open');
                cardDep.querySelector('.pf-card-header').classList.remove('open');
            } else {
                chkDependant.disabled = false;
                var depBody = cardDep.querySelector('.pf-card-body');
                depBody.classList.add('open');
                cardDep.querySelector('.pf-card-header').classList.add('open');
                subDependant.classList.toggle('visible', chkDependant.checked);
            }
        }

        chkDependant.addEventListener('change', function () {
            subDependant.classList.toggle('visible', chkDependant.checked);
        });

        ddlRelationship.addEventListener('change', applyRelationshipRules);

        /* ── Beneficiary logic ── */
        function toggleBeneficiary() {
            subBeneficiary.classList.toggle('visible', chkBeneficiary.checked);
            if (chkBeneficiary.checked) {
                cardBen.querySelector('.pf-card-body').classList.add('open');
                cardBen.querySelector('.pf-card-header').classList.add('open');
            }
        }

        chkBeneficiary.addEventListener('change', toggleBeneficiary);

        /* ── Init ── */
        applyRelationshipRules();
        toggleBeneficiary();
    });
</script>

<script>
    document.addEventListener("DOMContentLoaded", function () {

        const ddlRelationship = document.getElementById('<%= ddlRelationship.ClientID %>');
    const chkDependant = document.getElementById('<%= ChkDependant.ClientID %>');
    const chkBeneficiary = document.getElementById('<%= ChkBeneficiary.ClientID %>');

    const dependantFields = document.getElementById('dependantFields');
    const beneficiaryFields = document.getElementById('beneficiaryFields');

    const collapseDep = document.getElementById('collapseDependant');
    const collapseBen = document.getElementById('collapseBeneficiary');

    /* ---------------- DEPENDANT LOGIC ---------------- */

    function applyRelationshipRules() {
        if (ddlRelationship.value === "FamilyContact") {

            // Hide dependant completely
            chkDependant.checked = false;
            chkDependant.disabled = true;
            dependantFields.style.display = "none";
            collapseDep.classList.remove("show");

        } else {

            // Allow dependant
            chkDependant.disabled = false;
            collapseDep.classList.add("show");

            // Only show fields if toggle is ON
            dependantFields.style.display = chkDependant.checked ? "block" : "none";
        }
    }

    chkDependant.addEventListener("change", function () {
        dependantFields.style.display = chkDependant.checked ? "block" : "none";

        chkDependant.checked
            ? collapseDep.classList.add("show")
            : collapseDep.classList.remove("show");
    });

    ddlRelationship.addEventListener("change", applyRelationshipRules);

    /* ---------------- BENEFICIARY LOGIC ---------------- */

    function toggleBeneficiary() {
        beneficiaryFields.style.display = chkBeneficiary.checked ? "block" : "none";
        chkBeneficiary.checked
            ? collapseBen.classList.add("show")
            : collapseBen.classList.remove("show");
    }

    chkBeneficiary.addEventListener("change", toggleBeneficiary);

    /* ---------------- INIT ---------------- */

    applyRelationshipRules();
    toggleBeneficiary();
});
</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

<div class="form-header">Personal details</div>

<div class="form-grid">
    <div class="form-group">
        <label>First name</label>
        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control fixed-width" />
    </div>

    <div class="form-group">
        <label>Last name</label>
        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control fixed-width" />
    </div>

    <div class="form-group">
        <label>Relationship</label>
        <asp:DropDownList ID="ddlRelationship" runat="server" CssClass="form-control fixed-width">
        </asp:DropDownList>
    </div>
</div>

<div class="accordion">

    <!-- GENERAL -->
    <div class="card mb-1">
        <div class="card-header bg-light p-2">
            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseGeneral">
                <strong>General</strong>
                <i class="fa fa-chevron-down"></i>
            </a>
        </div>

        <div id="collapseGeneral" class="collapse show">
            <div class="card-body">
                <label class="me-3">Emergency contact</label>
                <label class="toggle-switch">
                    <asp:CheckBox ID="chkEmergencyContact" runat="server" />
                    <span class="slider"></span>
                </label>
            </div>
        </div>
    </div>

    <!-- BENEFICIARY -->
    <div class="card mb-1">
        <div class="card-header bg-light p-2">
            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseBeneficiary">
                <strong>Beneficiary</strong>
                <i class="fa fa-chevron-down"></i>
            </a>
        </div>

        <div id="collapseBeneficiary" class="collapse">
            <div class="card-body">

                <label class="me-3">Beneficiary</label>
                <label class="toggle-switch">
                    <asp:CheckBox ID="ChkBeneficiary" runat="server" />
                    <span class="slider"></span>
                </label>

                <div id="beneficiaryFields">
                    <div class="form-row mt-3">
                        <div class="form-group">
                            <label>Valid From</label>
                            <asp:TextBox ID="txtBeneficiaryValidFrom" runat="server"
                                TextMode="Date" CssClass="form-control fixed-width" />
                        </div>

                        <div class="form-group">
                            <label>Valid To</label>
                            <asp:TextBox ID="txtBeneficiaryValidTo" runat="server"
                                TextMode="Date" CssClass="form-control fixed-width" />
                        </div>
                    </div>

                    <div class="mt-3">
                        <label class="me-3">Primary</label>
                        <label class="toggle-switch">
                            <asp:CheckBox ID="ChkPrimary" runat="server" />
                            <span class="slider"></span>
                        </label>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- DEPENDANT -->
    <div class="card mb-1">
        <div class="card-header bg-light p-2">
            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseDependant">
                <strong>Dependant</strong>
                <i class="fa fa-chevron-down"></i>
            </a>
        </div>

        <div id="collapseDependant" class="collapse">
            <div class="card-body">

                <label class="me-3">Dependant</label>
                <label class="toggle-switch">
                    <asp:CheckBox ID="ChkDependant" runat="server" />
                    <span class="slider"></span>
                </label>

                                    <div class="form-group mb-3">
                        <label for="txtDependenatValidFromDate">Valid From</label>
                        <asp:TextBox ID="txtDependenatValidFromDate" runat="server"
                            CssClass="form-control fixed-width" TextMode="Date" />

                            <label for="txtDependenatValidToDate">Valid To</label>
                            <asp:TextBox ID="txtDependenatValidToDate" runat="server"
                                CssClass="form-control fixed-width" TextMode="Date" />
                    </div>

                <div id="dependantFields">
                    <div class="form-group mt-3">
                        <label>Gender</label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control fixed-width">
                            <asp:ListItem Text="" />
                            <asp:ListItem Text="Male" />
                            <asp:ListItem Text="Female" />
                            <asp:ListItem Text="Non-Specific" />
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Birth Date</label>
                        <asp:TextBox ID="txtBirthDate" runat="server"
                            TextMode="Date" CssClass="form-control fixed-width" />
                    </div>
                     <div class="form-group d-flex align-items-center mb-3">
    <label class="me-3">Full-Time Student</label>
    <label class="toggle-switch">
        <asp:CheckBox ID="chkFullTimeStudent" runat="server" />
        <span class="slider"></span>
    </label>
</div>


            <!-- Person with Disabilities Toggle -->
           <div class="form-group d-flex align-items-center mb-3">
    <label class="me-3">Person with Disabilities</label>
    <label class="toggle-switch">
        <asp:CheckBox ID="chkDisability" runat="server" />
        <span class="slider"></span>
    </label>
</div>


            <!-- Verification Date -->
            <div class="form-group mb-3">
                <label for="txtVerificationDate">Verification Date</label>
                <asp:TextBox ID="txtVerificationDate" runat="server"
                    CssClass="form-control fixed-width" TextMode="Date" />
            </div>
                                <div class="form-group mb-3 d-none">
                <label for="lblRecId">ReciId</label>
                <asp:TextBox ID="lblRecId" runat="server"
                    CssClass="form-control fixed-width"/>
            </div>
                </div>

            </div>
        </div>
    </div>

</div>

<div class="action-footer mt-3">
    <asp:Button ID="btnSave" runat="server"
        Text="Save"
        OnClick="btnSave_Click"
        CssClass="btn btn-primary btn-sm" />

    <asp:LinkButton ID="btcancel" runat="server"
        Text="Cancel"
       OnClientClick="closeParentModal(); return false;"
        CssClass="btn btn-secondary btn-sm" />
</div>

</asp:Content>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="HRPersonalContacts_Update.aspx.cs" Inherits="DynamicsPortal.ESS.HR.HRPersonalContacts_Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root {
            --c-bg: #f9fafb;
            --c-surface: #ffffff;
            --c-border: #e4e7ec;
            --c-text: #101828;
            --c-muted: #667085;
            --c-accent: #2563eb;
            --radius: 8px;
        }

        .pf-container { padding: 20px; font-family: 'Inter', system-ui, sans-serif; color: var(--c-text); }

        .section-title {
            font-size: 16px; font-weight: 600; margin-bottom: 20px;
            padding-bottom: 8px; border-bottom: 1px solid var(--c-border);
        }

        /* Responsive Grid */
        .pf-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; margin-bottom: 24px; }
        .pf-group { display: flex; flex-direction: column; gap: 6px; }
        .pf-group label { font-size: 13px; font-weight: 500; color: var(--c-muted); }

        .form-control-pf {
            width: 100%; height: 38px; padding: 8px 12px; font-size: 14px;
            border: 1px solid #d0d5dd; border-radius: 6px; transition: all 0.2s;
        }
        .form-control-pf:focus { border-color: var(--c-accent); box-shadow: 0 0 0 4px rgba(37, 99, 235, 0.1); outline: none; }

        /* Accordion Cards */
        .pf-card { border: 1px solid var(--c-border); border-radius: var(--radius); margin-bottom: 12px; overflow: hidden; background: var(--c-surface); }
        .pf-card-header { 
            background: #fdfdfd; padding: 12px 16px; cursor: pointer; 
            display: flex; justify-content: space-between; align-items: center; 
        }
        .pf-card-header.open { border-bottom: 1px solid var(--c-border); background: #f9fafb; }
        .pf-card-title { font-weight: 600; font-size: 14px; display: flex; align-items: center; gap: 8px; }
        .pf-card-body { padding: 20px; display: none; }
        .pf-card-body.open { display: block; }

        /* Modern Toggle Switch */
        .toggle-container { display: flex; align-items: center; gap: 12px; margin-bottom: 10px; }
        .toggle-switch { position: relative; display: inline-block; width: 42px; height: 22px; flex-shrink: 0; }
        .toggle-switch input { opacity: 0; width: 0; height: 0; }
        .slider {
            position: absolute; cursor: pointer; inset: 0; background-color: #eaecf0;
            transition: .4s; border-radius: 34px;
        }
        .slider:before {
            position: absolute; content: ""; height: 16px; width: 16px;
            left: 3px; bottom: 3px; background-color: white; transition: .4s; border-radius: 50%;
        }
        input:checked + .slider { background-color: var(--c-accent); }
        input:checked + .slider:before { transform: translateX(20px); }
        input:disabled + .slider { opacity: 0.5; cursor: not-allowed; }

        .pf-footer {
            margin-top: 24px; padding-top: 16px; border-top: 1px solid var(--c-border);
            display: flex; justify-content: flex-end; gap: 12px;
        }

        @media (max-width: 768px) { .pf-grid { grid-template-columns: 1fr; } }
        .page-title,
.page-header,
#pageTitle,
.header-title {
    display: none !important;
}
    </style>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            // Unified Accordion Logic
            document.querySelectorAll('.pf-card-header').forEach(hdr => {
                hdr.addEventListener('click', function () {
                    const body = this.nextElementSibling;
                    this.classList.toggle('open');
                    body.classList.toggle('open');
                });
            });

            const ddlRel = document.getElementById('<%= ddlRelationship.ClientID %>');
            const chkDep = document.getElementById('<%= ChkDependant.ClientID %>');
            const chkBen = document.getElementById('<%= ChkBeneficiary.ClientID %>');

            function syncUI() {
                // Dependency Rules
                const isFamily = ddlRel.value === "FamilyContact";
                chkDep.disabled = isFamily;
                if (isFamily) chkDep.checked = false;

                // Toggle Field Visibility
                document.getElementById('depFields').style.display = chkDep.checked ? 'grid' : 'none';
                document.getElementById('benFields').style.display = chkBen.checked ? 'grid' : 'none';
            }

            ddlRel.addEventListener('change', syncUI);
            chkDep.addEventListener('change', syncUI);
            chkBen.addEventListener('change', syncUI);

            syncUI(); // Initial Run for Edit mode
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
        <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="pf-container">
        <div class="section-title">Update Personal Details</div>
        
        <div class="pf-grid">
            <div class="pf-group">
                <label>First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control-pf" />
            </div>
            <div class="pf-group">
                <label>Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control-pf" />
            </div>
            <div class="pf-group">
                <label>Relationship</label>
                <asp:DropDownList ID="ddlRelationship" runat="server" CssClass="form-control-pf" />
            </div>
        </div>

        <!-- Hidden RecId field for backend logic -->
        <asp:TextBox ID="lblRecId" runat="server" style="display:none;" />

        <!-- GENERAL SECTION -->
        <div class="pf-card">
            <div class="pf-card-header open">
                <span class="pf-card-title">General Settings</span>
                <i class="fa fa-chevron-down text-muted" style="font-size:10px;"></i>
            </div>
            <div class="pf-card-body open">
                <div class="toggle-container">
                    <label class="toggle-switch">
                        <asp:CheckBox ID="chkEmergencyContact" runat="server" />
                        <span class="slider"></span>
                    </label>
                    <span class="pf-label">Emergency Contact</span>
                </div>
            </div>
        </div>

        <!-- BENEFICIARY SECTION -->
        <div class="pf-card">
            <div class="pf-card-header">
                <span class="pf-card-title">Beneficiary Details</span>
                <i class="fa fa-chevron-down text-muted" style="font-size:10px;"></i>
            </div>
            <div class="pf-card-body">
                <div class="toggle-container">
                    <label class="toggle-switch">
                        <asp:CheckBox ID="ChkBeneficiary" runat="server" />
                        <span class="slider"></span>
                    </label>
                    <span class="pf-label" style="font-weight:600;">Is Beneficiary</span>
                </div>
                
                <div id="benFields" class="pf-grid" style="margin-top:15px; border-top: 1px dashed #eee; padding-top:15px;">
                    <div class="pf-group" style="display:none">
                        <label>Valid From</label>
                        <asp:TextBox ID="txtBeneficiaryValidFrom" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    <div class="pf-group" style="display:none">
                        <label>Valid To</label>
                        <asp:TextBox ID="txtBeneficiaryValidTo" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    <div class="pf-group">
                        <label>Primary Beneficiary</label>
                        <div class="toggle-container" style="margin-top:8px;">
                            <label class="toggle-switch">
                                <asp:CheckBox ID="ChkPrimary" runat="server" />
                                <span class="slider"></span>
                            </label>
                        </div>
                    </div>
                    <div class="pf-group">
                        <label>Beneficiary Percentage</label>
                        <asp:TextBox ID="txtBeneficiaryPercentage" runat="server" CssClass="form-control-pf" TextMode="Phone" placeholder="Enter Beneficiary Percentage" />
                    </div>
                </div>
            </div>
        </div>

        <!-- DEPENDANT SECTION -->
        <div class="pf-card">
            <div class="pf-card-header">
                <span class="pf-card-title">Dependant Information</span>
                <i class="fa fa-chevron-down text-muted" style="font-size:10px;"></i>
            </div>
            <div class="pf-card-body">
                <div class="toggle-container">
                    <label class="toggle-switch">
                        <asp:CheckBox ID="ChkDependant" runat="server" />
                        <span class="slider"></span>
                    </label>
                    <span class="pf-label" style="font-weight:600;">Is Dependant</span>
                </div>

                <div id="depFields" class="pf-grid" style="margin-top:15px; border-top: 1px dashed #eee; padding-top:15px;">
                    <div class="pf-group">
                        <label>Valid From</label>
                        <asp:TextBox ID="txtDependenatValidFromDate" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    <div class="pf-group">
                        <label>Valid To</label>
                        <asp:TextBox ID="txtDependenatValidToDate" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    <div class="pf-group">
                        <label>Birth Date</label>
                        <asp:TextBox ID="txtBirthDate" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    
                    <div class="pf-group">
                        <label>Gender</label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control-pf">
                            <asp:ListItem Text="Select Gender" Value="" />
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                            <asp:ListItem Text="Non-Specific" Value="Non-Specific" />
                        </asp:DropDownList>
                    </div>
                    <div class="pf-group">
                        <label>Verification Date</label>
                        <asp:TextBox ID="txtVerificationDate" runat="server" TextMode="Date" CssClass="form-control-pf" />
                    </div>
                    <div class="pf-group" style="visibility:hidden;"></div>

                    <div class="pf-group">
                        <label>Full-Time Student</label>
                        <div class="toggle-container" style="margin-top:8px;">
                            <label class="toggle-switch">
                                <asp:CheckBox ID="chkFullTimeStudent" runat="server" />
                                <span class="slider"></span>
                            </label>
                        </div>
                    </div>
                    <div class="pf-group">
                        <label>Person with Disability</label>
                        <div class="toggle-container" style="margin-top:8px;">
                            <label class="toggle-switch">
                                <asp:CheckBox ID="chkDisability" runat="server" />
                                <span class="slider"></span>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="pf-footer">
            <asp:LinkButton ID="btcancel" runat="server" Text="Cancel" OnClientClick="closeParentModal(); return false;" CssClass="btn btn-light" style="border:1px solid #d0d5dd; padding: 7px 15px; font-size:13px; text-decoration:none; color:#344054;" />
            <asp:Button ID="btnSave" runat="server" Text="Update Contact" OnClick="btnSave_Click" CssClass="btn btn-primary" style="background:#2563eb; border:none; padding: 7px 20px; font-size:13px; color:#fff;" />
        </div>
    </div>
        </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>