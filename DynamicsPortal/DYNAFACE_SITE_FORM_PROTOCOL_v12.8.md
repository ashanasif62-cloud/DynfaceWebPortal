# DYNAFACE SITE FORM PROTOCOL - VERSION 12.8

## 1. Universal Execution Mandate
* **Zero-Skip Policy:** The AI MUST NOT skip the interaction protocol in any environment (Visual Studio, GitHub, or Gemini), regardless of the detected form type.
* **Manual Override:** Automatic detection of form types is secondary to the Mandatory Question Sequence; the user must be walked through every step individually.
* **No Premature Generation:** No code is to be generated until the specific sequence for the chosen form type is complete.

## 2. Dynamic Workspace Inheritance
* **Master Template:** All forms must inherit from `Site.Master` or `Modal.Master` and utilize their generic global styles.
* **Architecture Inheritance:** When creating a new form, the design, style, and functional logic must be inherited from existing forms in the workspace according to the form type.
* **Global Override:** Always prioritize `distribution/css/custom.css` for custom branding.

## 3. Architecture Enforcement (Strict Protocol Lockdown)
* **Compliance Stop:** If a request violates established architecture, naming conventions, or inheritance patterns, you must strictly stop and insist on the protocol.
* **Feature Restriction:** If a user violates the instruction (e.g., requesting a "Submit" button where not defined, adding buttons manually, or any feature not existing in this protocol), the AI must stop the user and notify them that the request is unauthorized.
* **Hard-Stop Logic:** Ensure the sequence is followed strictly one-by-one without skipping steps.

## 4. Architecture & Naming Standards
* **Namespace Alignment:** UI (`DynamicsPortal`), Logic (`BussinessLogic`), Data (`DataAccess`), Models (`BussinessObject`).
* **Suffixes:** Use `_BOL` (Models) and `_BLL` (Logic).
* **Suffix Exception:** The `_DAL` suffix is strictly removed from class naming conventions.
* **Session:** Exclusively use `SessionVariables` (`GeneralAuxiliary`).

## 5. Mandatory Interaction Protocol (One-by-One)
* **Step 1: Form Type Selection:** "Which master template are we using: **Site.Master** or **Modal.Master**?"
* **Step 2 (If Site.Master):**
    1.  "The **Grid** and **Action Buttons (New, Edit, Delete)** are automatically initialized and will inherit the workspace design. What are the names/types for the **Grid Fields**?"
    2.  "How many fields are **Read-Only**?"
    3.  "How many fields are **Dropdowns vs Textboxes**?"
    4.  "What is the **C# Class Name**?"
    5.  "Which method is used for **Binding, Saving, and Updating**?"
    6.  "**Shall I implement a Tabbed Interface for data organization?**"
    7.  "**Shall I implement a Collapsible Panel for UI efficiency?**"
* **Step 3 (If Modal.Master):**
    1.  "How many fields do you want to add to the **Modal.Master**?"
    2.  "What are the names and types (Dropdown, Textbox, Date) for each field?"
    3.  "Which fields require validation before creation?"
    4.  "**Shall I implement any additional custom buttons beyond the automated 'Create/Submit' action?**"

## 6. Technical Requirements
* **Grid:** Must be Responsive with Pagination, Search, and Auto-Refresh.
* **Date Fields:** Must include a **Calendar Icon** and trigger the date picker.
* **Data Access:** Use `getConnection_DAL` patterns.
* **Notifications:** Use `NotificationMessage.showMessage`. **No JS alerts**.